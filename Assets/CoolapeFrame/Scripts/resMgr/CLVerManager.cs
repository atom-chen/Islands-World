/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  热更新处理(处理部分移到lua里了)
  *Others:  
  *History:
*********************************************************************************
*/
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Coolape
{
    public class CLVerManager : CLBaseLua
    {
        public bool isPrintDownload = false;
        public int downLoadTimes4Failed = 3;
        //服务器
        public string baseUrl = "http://your hot fix url";
        [HideInInspector]
        public string platform = "";
        public static CLVerManager self;
        public static string resVer = "resVer";
        public static string versPath = "VerCtl";
        public const string fverVer = "VerCtl.ver";

        //========================
        //本地所有版本的版本信息
        public const string verPriority = "priority.ver";
        public Hashtable localPriorityVer = new Hashtable();

        //本地优先更新资源
        public const string verOthers = "other.ver";
        public Hashtable otherResVerOld = new Hashtable();

        //所有资源的版本管理
        public Hashtable otherResVerNew = new Hashtable();
        //所有资源的版本管理

        public bool haveUpgrade = false;
        public bool is2GNetUpgrade = false;
        public bool is3GNetUpgrade = true;
        public bool is4GNetUpgrade = true;
        [HideInInspector]
        public string mVerverPath = "";
        [HideInInspector]
        public string mVerPrioriPath = "";
        [HideInInspector]
        public string mVerOtherPath = "";

        static string clientVersionPath
        {
            get
            {
                return Path.Combine(CLPathCfg.persistentDataPath, "ver.v");       //客户端版本
            }
        }

        public CLVerManager()
        {
            self = this;
        }

        public string clientVersion
        {
            get
            {
                try
                {
                    if (!File.Exists(clientVersionPath))
                    {
                        return "";
                    }
                    return File.ReadAllText(clientVersionPath);
                }
                catch (System.Exception e)
                {
                    return "";
                }
            }
            set
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(clientVersionPath));
                    File.WriteAllText(clientVersionPath, value);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        // 用文件来表示是否已经处理完资源从包里释放出来的状态
        static string hadPoc
        {
            get
            {
                return Path.Combine(CLPathCfg.persistentDataPath, "resPoced");
            }
        }

        Callback onFinisInitStreaming;

        /// <summary>
        /// Inits the streaming assets packge.
        /// 将流目录中优先需要加载的资源集解压到可读写目录
        /// </summary>
        /// <param name="onFinisInitStreaming">On finis init streaming.</param>
        public void initStreamingAssetsPackge(Callback onFinisInitStreaming)
        {
            this.onFinisInitStreaming = onFinisInitStreaming;
            // clean cache
#if UNITY_EDITOR
            if (CLCfgBase.self.isEditMode)
            {
                onFinisInitStreaming();
                return;
            }
#endif
            try
            {
                // 当版本不同时, clean cache
                if (string.Compare(Application.version, clientVersion) != 0)
                {
                    string path = Application.persistentDataPath;
                    // 先删掉目录下的文件
                    string[] fEntries = Directory.GetFiles(path);
                    foreach (string f in fEntries)
                    {
                        File.Delete(f);
                    }
                    // 再删掉所有文件夹
                    string[] dirEntries = Directory.GetDirectories(path);
                    foreach (string dir in dirEntries)
                    {
                        Directory.Delete(dir, true);
                    }
                    clientVersion = Application.version;
                }

                // 处理资源释放
                if (!File.Exists(hadPoc))
                {
                    string path = "priority.r";
                    Callback cb = onGetStreamingAssets;
                    StartCoroutine(FileEx.readNewAllBytesAsyn(path, cb));
                }
                else
                {
                    onFinisInitStreaming();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                onFinisInitStreaming();
            }
        }

        // 取得"priority.r"
        void onGetStreamingAssets(params object[] para)
        {
            if (para != null && para.Length > 0)
            {
                byte[] buffer = (byte[])(para[0]);
                if (buffer != null)
                {
                    MemoryStream ms = new MemoryStream();
                    ms.Write(buffer, 0, buffer.Length);
                    ms.Position = 0;
                    object obj = B2InputStream.readObject(ms);
                    if (obj != null)
                    {
                        Hashtable map = (Hashtable)(obj);
                        string path = "";
                        foreach (DictionaryEntry cell in map)
                        {
                            try
                            {
                                path = CLPathCfg.persistentDataPath + "/" + cell.Key;
                                Directory.CreateDirectory(Path.GetDirectoryName(path));
                                File.WriteAllBytes(path, (byte[])(cell.Value));
                            }
                            catch (System.Exception e)
                            {
                                Debug.LogError(e);
                            }
                        }
                        // 处理完包的资源释放，弄个标志
                        Directory.CreateDirectory(Path.GetDirectoryName(hadPoc));
                        File.WriteAllText(hadPoc, "ok");
                    }
                }
            }
            Utl.doCallback(onFinisInitStreaming);
        }

        public Hashtable toMap(byte[] buffer)
        {
            Hashtable r = new Hashtable();
            if (buffer != null)
            {
                MemoryStream ms = new MemoryStream();
                ms.Write(buffer, 0, buffer.Length);
                ms.Position = 0;
                object obj = B2InputStream.readObject(ms);
                if (obj != null && obj is Hashtable)
                {
                    r = (Hashtable)obj;
                }
                else
                {
                    r = new Hashtable();
                }
            }
            else
            {
                r = new Hashtable();
            }
            return r;
        }

        public static Hashtable wwwMap = Hashtable.Synchronized(new Hashtable());
        public static Hashtable wwwTimesMap = new Hashtable();

        /// <summary>
        /// Gets the newest res.取得最新的资源
        /// </summary>
        /// <param name='path'>
        /// Path.资源的相对路径
        /// </param>
        /// <param name='type'>
        /// Type.资源的类型
        /// </param>
        /// <param name='onGetAsset'>
        /// On get asset.取得资源后的回调（回调信息中：
        /// 第一个参数是资源路径，
        /// 第二参数是资源的内容，
        /// 第三个参数传入的原样返回的参数)
        /// </param>
        /// <param name='originals'>
        /// Originals.原样返回的参数
        /// </param>
        public void getNewestRes4Lua(string path, CLAssetType type, object onGetAsset, bool autoRealseAB, object originals)
        {
            getNewestRes(path, type, onGetAsset, autoRealseAB, originals);
        }

        public void getNewestRes(string path, CLAssetType type, object onGetAsset, bool autoRealseAB, params object[] originals)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            string verVal = "";
            if (!MapEx.getBool(wwwMap, path))
            {
                bool needSave = false;
                wwwMap[path] = true;
                if (localPriorityVer[path] != null)
                {  //在优先资源里有
                    needSave = false;
                }
                else
                {        //则可能在others里
                    object obj1 = otherResVerOld[path];
                    object obj2 = otherResVerNew[path];
                    if (obj1 == null && obj2 != null)
                    { //本地没有，最新有
                        verVal = MapEx.getString(otherResVerNew, path);
                        needSave = true;
                    }
                    else if (obj1 != null && obj2 != null)
                    {
                        if (obj1.ToString().Equals(obj2.ToString()))
                        {//本地是最新的
                            needSave = false;
                        }
                        else
                        {    //本地不是最新的
                            verVal = MapEx.getString(otherResVerNew, path);
                            needSave = true;
                        }
                    }
                    else if (obj1 != null && obj2 == null)
                    {//本地有，最新没有
                        needSave = false;
                    }
                    else
                    {    //都没有找到
                        needSave = false;
#if UNITY_EDITOR
                        //                  Debug.LogWarning ("Not found.path==" + path);
#endif
                    }
                }
                string url = "";
                if (needSave)
                {
                    if (!string.IsNullOrEmpty(verVal))
                    {
                        url = PStr.begin().a(baseUrl).a("/").a(path).a(".").a(verVal).end();
                    }
                    else
                    {
                        url = PStr.begin().a(baseUrl).a("/").a(path).end();
                    }
                    if (isPrintDownload)
                    {
                        Debug.LogWarning(url);
                    }
                }
                else
                {
                    url = PStr.begin().a(CLPathCfg.persistentDataPath).a("/").a(path).end();
                    if (!File.Exists(url))
                    {
                        url = System.IO.Path.Combine(Application.streamingAssetsPath, path);
#if !UNITY_ANDROID || UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
//                        url = PStr.begin().a("file:///").a(url).end();
						#if UNITY_STANDALONE
						url = PStr.begin ().a ("file:///").a (url).end ();
						#else
						url = PStr.begin ().a ("file://").a (url).end ();
						#endif
#endif
                    }
                    else
                    {
//                        url = PStr.begin().a("file:///").a(url).end();
						#if UNITY_STANDALONE
						url = PStr.begin ().a ("file:///").a (url).end ();
						#else
						url = PStr.begin ().a ("file://").a (url).end ();
						#endif
                    }
                }
                doGetContent(path, url, needSave, type, onGetAsset, autoRealseAB, originals);
            }
        }

        void doGetContent(string path, string url, bool needSave,
                                         CLAssetType type, object onGetAsset, bool autoRealseAB, params object[] originals)
        {
#if UNITY_EDITOR
            getedResMap[path] = true;
#endif
            if (url.StartsWith("http"))
            {
                url = url.Replace("+", "%2B");
            }

            wwwMap[path] = true;

            NewList transParam = ObjPool.listPool.borrowObject();
            transParam.Add(path);
            transParam.Add(url);
            transParam.Add(needSave);
            transParam.Add(type);
            transParam.Add(onGetAsset);
            transParam.Add(originals);
            transParam.Add(autoRealseAB);
            UnityWebRequest www = null;
            if (needSave)
            {
#if UNITY_EDITOR
                Debug.LogWarning(url);
#endif
                CLAssetType tmptype = type;
                if (type == CLAssetType.assetBundle)
                {
                    //因为通过DownloadHandlerAssetBundle无法取得bytes，所以只能先改变取得数据类型，然后再通过AssetBundle.LoadFromMemory取得AssetBundle
                    tmptype = CLAssetType.bytes;
                }
                www = WWWEx.get(url, tmptype, (Callback)onWWWBack, (Callback)onWWWBack, transParam, true);
                addWWW(www, path, url);
            } else {
                www = WWWEx.get(url, type, (Callback)onWWWBack, (Callback)onWWWBack, transParam, true);
            }
        }

        void onWWWBack(params object[] parma) {
            try
            {
                object content = parma[0];
                NewList list = parma[1] as NewList;
                string path = list[0] as string;
                string url = list[1] as string;
                bool needSave = (bool)(list[2]);
                CLAssetType type = (CLAssetType)(list[3]);
                object onGetAsset = list[4];
                object[] originals = list[5] as object[];
                bool autoRealseAB = (bool)(list[6]);

                UnityWebRequest www = null;
                if (parma.Length > 2)
                {
                    www = parma[2] as UnityWebRequest;
                }

                if (needSave && www != null && www.downloadHandler.data != null)
                {
                    saveNewRes(path, www.downloadHandler.data);
                    if (type == CLAssetType.assetBundle)
                    {
                        content = AssetBundle.LoadFromMemory(www.downloadHandler.data);
                    }
                }
                onGetNewstRes(www, url, path, type, content, needSave, onGetAsset, autoRealseAB, originals);
                ObjPool.listPool.returnObject(list);
                list = null;
            }catch(System.Exception e) {
                Debug.LogError(e);
            }
        }

        public void onGetNewstRes(UnityWebRequest www, string url, string path, CLAssetType type, object content, bool needSave, object onGetAsset, bool autoRealseAB, params object[] originals)
        {
            try
            {
                if (needSave)
                {
                    //说明是需要下载的资源
                    if (www != null && content == null && (MapEx.getInt(wwwTimesMap, path) + 1) < downLoadTimes4Failed)
                    {
                        //需要下载资源时，如查下载失败，且少于失败次数，则再次下载
                        wwwTimesMap[path] = MapEx.getInt(wwwTimesMap, path) + 1;
                        doGetContent(path, url, needSave, type, onGetAsset, autoRealseAB, originals);
                        return;
                    }
                    rmWWW(url);
                }
                if (content == null)
                {
                    Debug.LogError("get newstRes is null. url==" + url);
                }
                Utl.doCallback(onGetAsset, path, content, originals);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }

            if(autoRealseAB && content != null && type == CLAssetType.assetBundle) {
                AssetBundle ab = content as AssetBundle;
                if (ab != null) {
                    ab.Unload(false);
                }
            }

            if (www != null)
            {
                www.Dispose();
                www = null;
            }

            wwwMap[path] = false;
            wwwTimesMap[path] = 0;
        }

        object addWWWcb;
        object rmWWWcb;

        public void setWWWListner(object addWWWcb, object rmWWWcb)
        {
            this.addWWWcb = addWWWcb;
            this.rmWWWcb = rmWWWcb;
        }

        public void addWWW(UnityWebRequest www, string path, string url)
        {
            Utl.doCallback(addWWWcb, www, path, url);
        }

        public void rmWWW(string url)
        {
            Utl.doCallback(rmWWWcb, url);
        }

        /// <summary>
        /// Saves the new res.保存最新取得的资源
        /// </summary>
        /// <param name='path'>
        /// Path.
        /// </param>
        /// <param name='content'>
        /// Content.
        /// </param>
        void saveNewRes(string path, byte[] content)
        {
            string file = PStr.begin().a(CLPathCfg.persistentDataPath).a("/").a(path).end();
            Directory.CreateDirectory(Path.GetDirectoryName(file));
            File.WriteAllBytes(file, content);
            if (otherResVerNew[path] != null)
            { //优先更新资源已经是最新的了，所以不用再同步
                otherResVerOld[path] = otherResVerNew[path];
                MemoryStream ms = new MemoryStream();
                B2OutputStream.writeMap(ms, otherResVerOld);

                string vpath = PStr.begin().a(CLPathCfg.persistentDataPath).a("/").a(mVerOtherPath).end();
                Directory.CreateDirectory(Path.GetDirectoryName(vpath));
                File.WriteAllBytes(vpath, ms.ToArray());
            }
        }

        public Texture getAtalsTexture4Edit(string path)
        {
            string url = "";
#if UNITY_EDITOR
            url = "file://" + Application.dataPath + "/" + path;
#endif
            WWW www = new WWW(Utl.urlAddTimes(url));
            while (!www.isDone)
            {
                //				new WaitForSeconds (0.05f);
                System.Threading.Thread.Sleep(50); //比较hacker的做法
            }
            if (string.IsNullOrEmpty(www.error))
            {
                Texture texture = www.texture;
                www.Dispose();
                www = null;
                return texture;
            }
            else
            {
                www.Dispose();
                www = null;
                return null;
            }
        }

        public bool checkNeedDownload(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            bool ret = false;
            if (localPriorityVer[path] != null)
            {  //在优先资源里有
                ret = false;
            }
            else
            {        //则可能在others里
                object obj1 = otherResVerOld[path];
                object obj2 = otherResVerNew[path];
                if (obj1 == null && obj2 != null)
                { //本地没有，最新有
                    ret = true;
                }
                else if (obj1 != null && obj2 != null)
                {
                    if (NumEx.stringToInt(obj1.ToString()) >= NumEx.stringToInt(obj2.ToString()))
                    {//本地是最新的
                        ret = false;
                    }
                    else
                    {    //本地不是最新的
                        ret = true;
                    }
                }
                else if (obj1 != null && obj2 == null)
                {//本地有，最新没有
                    ret = false;
                }
                else
                {    //都没有找到
                     //                NAlertTxt.add("Cannot Found the res. Path= \n"+ path, Color.red, 1);
                    Debug.LogError("Cannot Found the res. Path= \n" + path);
                    ret = true;
                }
            }
            //			if (ret) {
            //				if (!Utl.netIsActived ()) {
            //					NAlertTxt.add (Localization.Get ("MsgNetWorkCannotReached"), Color.red, 1);
            //					reLoadGame ();
            //				}
            //			}
            return ret;
        }

        /// <summary>
        /// Ises the ver newest.
        /// </summary>
        /// <returns>
        /// The ver newest.
        /// </returns>
        /// <param name='path'>
        /// If set to <c>true</c> path.
        /// </param>
        /// <param name='ver'>
        /// If set to <c>true</c> ver.
        /// </param>
        public bool isVerNewest(string path, string ver)
        {
            object newVer = localPriorityVer[path];
            if (newVer != null)
            {   //在优先资源里有
                if (!ver.Equals(newVer.ToString()))
                {
                    return false;
                }
                return true;
            }
            else
            {        //则可能在others里
                newVer = otherResVerNew[path];
                if (newVer != null)
                {
                    if (!ver.Equals(newVer.ToString()))
                    {
                        return false;
                    }
                    return true;
                }
                return true;
            }
        }

        Hashtable getedResMap = new Hashtable();

        [ContextMenu("save loaded assets(with out priority assets)")]
        public void getCurrentRes()
        {
#if UNITY_EDITOR
            string str = "";
            foreach (DictionaryEntry cell in getedResMap)
            {
                str += cell.Key + "," + "true" + "\n";
            }
            Debug.LogError(str);
            string path = EditorUtility.SaveFilePanelInProject("save loaded assets(with out priority assets)", "newAssetsInfor", "v", "");
            if (!string.IsNullOrEmpty(path))
            {
                File.WriteAllText(path, str);
            }
#endif
        }
    }

}