/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  WWW包装
  *Others:  
  *History:
*********************************************************************************
*/

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace Coolape
{
    public enum CLAssetType
    {
        text,
        bytes,
        texture,
        assetBundle,
    }

    public class WWWEx : MonoBehaviour
    {
        public static WWWEx self;
        public static Hashtable wwwMapUrl = new Hashtable();
        public static Hashtable wwwMap4Check = new Hashtable();
        public static Hashtable wwwMap4Get = new Hashtable();
        public int checkTimeOutSec = 5;
        public delegate void RedCallback(string url);
        public bool isCheckWWWTimeOut = false;
        float lastCheckTimeOutTime = 0;

        public WWWEx()
        {
            self = this;
        }

        private void LateUpdate()
        {
            if (!isCheckWWWTimeOut) return;
            if (Time.realtimeSinceStartup - lastCheckTimeOutTime > 0)
            {
                lastCheckTimeOutTime = Time.realtimeSinceStartup + 3;
                checkWWWTimeout();
            }
            if (wwwMap4Check.Count <= 0)
            {
                isCheckWWWTimeOut = false;
            }
            if (wwwMapUrl.Count <= 0)
            {
                enabled = false;
            }
        }

        IEnumerator exeWWW(UnityWebRequest www, string url, CLAssetType type,
                               object successCallback,
                                  object failedCallback, object orgs, bool isCheckTimeout = true, RedCallback redrectioncallback = null)
        {
            wwwMapUrl[url] = www;
            if (isCheckTimeout)
            {
                addCheckWWWTimeout(www, url, checkTimeOutSec, failedCallback, orgs);
            }
            using (www)
            {
                yield return www.SendWebRequest();
                try
                {
                    uncheckWWWTimeout(www, url);

                    if (www.isNetworkError || www.isHttpError)
                    {
                        long retCode = www.responseCode;
                        Debug.LogError(www.error + ",retCode==" + retCode + "," + url);
                        if (retCode == 300 || retCode == 301 || retCode == 302)
                        {
                            // 重定向处理
                            string url2 = www.GetResponseHeader("Location");
                            if (string.IsNullOrEmpty(url2))
                            {
                                Utl.doCallback(failedCallback, null, orgs);
                            }
                            else
                            {
                                if (redrectioncallback != null)
                                {
                                    redrectioncallback(url2);
                                }
                            }
                        }
                        else
                        {
                            Utl.doCallback(failedCallback, null, orgs);
                        }
                    }
                    else
                    {
                        object content = null;
                        switch (type)
                        {
                            case CLAssetType.text:
                                content = www.downloadHandler.text;
                                break;
                            case CLAssetType.bytes:
                                content = www.downloadHandler.data;
                                break;
                            case CLAssetType.texture:
                                content = DownloadHandlerTexture.GetContent(www);
                                break;
                            case CLAssetType.assetBundle:
                                content = DownloadHandlerAssetBundle.GetContent(www);
                                break;
                        }
                        Utl.doCallback(successCallback, content, orgs, www);
                    }

                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                    Utl.doCallback(failedCallback, null, orgs);
                }
            }

            wwwMap4Get.Remove(url);
            wwwMapUrl.Remove(url);

            if (www != null)
            {
                www.Dispose();
                www = null;
            }
        }

        /// <summary>
        /// Get the specified url, type, successCallback, failedCallback, orgs and isCheckTimeout.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="url">URL.</param>
        /// <param name="type">Type.</param>
        /// <param name="successCallback">Success callback.</param>
        /// <param name="failedCallback">Failed callback.</param>
        /// <param name="orgs">Orgs.回调参数</param>
        /// <param name="isCheckTimeout">If set to <c>true</c> is check timeout. 检测超时</param>
        public static UnityWebRequest get(string url, CLAssetType type,
                               object successCallback,
                                   object failedCallback, object orgs, bool isCheckTimeout)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return null;
                self.enabled = true;
                UnityWebRequest www = null;
                switch (type)
                {
                    case CLAssetType.texture:
                        www = UnityWebRequestTexture.GetTexture(url);
                        break;
                    case CLAssetType.assetBundle:
                        www = UnityWebRequestAssetBundle.GetAssetBundle(url);
                        break;
                    default:
                        www = UnityWebRequest.Get(url);
                        break;
                }
                Coroutine cor = self.StartCoroutine(self.exeWWW(www, url, type, successCallback, failedCallback, orgs, isCheckTimeout, (url2) =>
                 {
                     get(url2, type, successCallback, failedCallback, orgs, isCheckTimeout);
                 }));
                wwwMap4Get[url] = cor;
                return www;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                Utl.doCallback(failedCallback, null, orgs);
                return null;
            }
        }

        public static UnityWebRequest post(string url, string jsonMap, CLAssetType type,
                               object successCallback,
                                   object failedCallback, object orgs, bool isCheckTimeout)
        {
            Hashtable map = JSON.DecodeMap(jsonMap);
            return post(url, map, type, successCallback, failedCallback, orgs, isCheckTimeout);
        }

        public static UnityWebRequest post(string url, Hashtable map, CLAssetType type,
                               object successCallback,
                                   object failedCallback, object orgs, bool isCheckTimeout)
        {
            try
            {
                self.enabled = true;
                WWWForm data = new WWWForm();
                if (map != null)
                {
                    foreach (DictionaryEntry cell in map)
                    {
                        if (cell.Value is int)
                        {
                            data.AddField(cell.Key.ToString(), int.Parse(cell.Value.ToString()));
                        }
                        else if (cell.Value is byte[])
                        {
                            data.AddBinaryData(cell.Key.ToString(), (byte[])(cell.Value));
                        }
                        else
                        {
                            data.AddField(cell.Key.ToString(), cell.Value.ToString());
                        }
                    }
                }
                return post(url, data, type, successCallback, failedCallback, orgs, isCheckTimeout);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                Utl.doCallback(failedCallback, null, orgs);
                return null;
            }
        }

        public static UnityWebRequest post(string url, WWWForm data, CLAssetType type,
                               object successCallback,
                                   object failedCallback, object orgs, bool isCheckTimeout)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return null;
                self.enabled = true;
                UnityWebRequest www = UnityWebRequest.Post(url, data);
                Coroutine cor = self.StartCoroutine(self.exeWWW(www, url, type, successCallback, failedCallback, orgs, isCheckTimeout, (url2) =>
                {
                    post(url2, data, type, successCallback, failedCallback, orgs, isCheckTimeout);
                }));
                wwwMap4Get[url] = cor;
                return www;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                Utl.doCallback(failedCallback, null, orgs);
                return null;
            }
        }
        public static UnityWebRequest postString(string url, string strData, CLAssetType type,
                               object successCallback,
                                   object failedCallback, object orgs, bool isCheckTimeout)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return null;
                self.enabled = true;
                UnityWebRequest www = UnityWebRequest.Post(url, strData);
                Coroutine cor = self.StartCoroutine(self.exeWWW(www, url, type, successCallback, failedCallback, orgs, isCheckTimeout, (url2) =>
                {
                    post(url2, strData, type, successCallback, failedCallback, orgs, isCheckTimeout);
                }));
                wwwMap4Get[url] = cor;
                return www;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                Utl.doCallback(failedCallback, null, orgs);
                return null;
            }
        }

        // 因为lua里没有bytes类型，所以不能重载，所以只能通方法名来
        public static UnityWebRequest postBytes(string url, byte[] bytes, CLAssetType type,
                               object successCallback,
                                   object failedCallback, object orgs, bool isCheckTimeout)
        {
            try
            {
                self.enabled = true;
                UnityWebRequest www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
                UploadHandlerRaw MyUploadHandler = new UploadHandlerRaw(bytes);
                DownloadHandlerBuffer downloadHandler = new DownloadHandlerBuffer();
                //MyUploadHandler.contentType = "application/x-www-form-urlencoded"; // might work with 'multipart/form-data'
                www.uploadHandler = MyUploadHandler;
                www.downloadHandler = downloadHandler;
                Coroutine cor = self.StartCoroutine(self.exeWWW(www, url, type, successCallback, failedCallback, orgs, isCheckTimeout, (url2) =>
                {
                    postBytes(url2, bytes, type, successCallback, failedCallback, orgs, isCheckTimeout);
                }));
                wwwMap4Get[url] = cor;
                return www;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                Utl.doCallback(failedCallback, null, orgs);
                return null;
            }
        }

        public static UnityWebRequest put(string url, string data, CLAssetType type,
                               object successCallback,
                                   object failedCallback, object orgs, bool isCheckTimeout)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return null;
                self.enabled = true;
                UnityWebRequest www = UnityWebRequest.Put(url, data);
                Coroutine cor = self.StartCoroutine(self.exeWWW(www, url, type, successCallback, failedCallback, orgs, isCheckTimeout, (url2) =>
                {
                    put(url2, data, type, successCallback, failedCallback, orgs, isCheckTimeout);
                }));
                wwwMap4Get[url] = cor;
                return www;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                Utl.doCallback(failedCallback, null, orgs);
                return null;
            }
        }

        public static UnityWebRequest put(string url, byte[] data, CLAssetType type,
                               object successCallback,
                                   object failedCallback, object orgs, bool isCheckTimeout)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return null;
                self.enabled = true;
                UnityWebRequest www = UnityWebRequest.Put(url, data);
                Coroutine cor = self.StartCoroutine(self.exeWWW(www, url, type, successCallback, failedCallback, orgs, isCheckTimeout, (url2) =>
                {
                    put(url2, data, type, successCallback, failedCallback, orgs, isCheckTimeout);
                }));
                wwwMap4Get[url] = cor;
                return www;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                Utl.doCallback(failedCallback, null, orgs);
                return null;
            }
        }

        /// <summary>
        /// Uploads the file.上传文件
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="url">URL.</param>
        /// <param name="sectionName">Section name. 对应java里的new FilePart("sectionName", f) </param>
        /// <param name="fileName">File name.</param>
        /// <param name="fileContent">File content.</param>
        /// <param name="type">Type.</param>
        /// <param name="successCallback">Success callback.</param>
        /// <param name="failedCallback">Failed callback.</param>
        /// <param name="orgs">Orgs.</param>
        /// <param name="isCheckTimeout">If set to <c>true</c> is check timeout.</param>
        public static UnityWebRequest uploadFile(string url, string sectionName, string fileName, byte[] fileContent, CLAssetType type,
                               object successCallback, object failedCallback, object orgs, bool isCheckTimeout)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return null;
                self.enabled = true;
                MultipartFormFileSection fileSection = new MultipartFormFileSection(sectionName, fileContent, fileName,  "Content-Type: multipart/form-data;");
                List <IMultipartFormSection> multForom = new List<IMultipartFormSection>();
                multForom.Add(fileSection);
                UnityWebRequest www = UnityWebRequest.Post(url, multForom);
                Coroutine cor = self.StartCoroutine(self.exeWWW(www, url, type, successCallback, failedCallback, orgs, isCheckTimeout, (url2) =>
                {
                    uploadFile(url2, sectionName, fileName, fileContent, type, successCallback, failedCallback, orgs, isCheckTimeout);
                }));
                wwwMap4Get[url] = cor;
                return www;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                Utl.doCallback(failedCallback, null, orgs);
                return null;
            }
        }

        /// <summary>
        /// Checks the WWW timeout.
        /// </summary>
        /// <param name="www">Www.</param>
        /// <param name="checkProgressSec">Check progress sec.</param>
        /// <param name="timeoutCallback">Timeout callback.</param>
        public static void addCheckWWWTimeout(UnityWebRequest www, string url, float checkProgressSec, object timeoutCallback, object orgs)
        {
            if (www == null || www.isDone)
                return;
            self.enabled = true;
            self.isCheckWWWTimeOut = true;
            checkProgressSec = checkProgressSec <= 0 ? 5 : checkProgressSec;
            //UnityEngine.Coroutine cor = self.StartCoroutine(doCheckWWWTimeout(www, url, checkProgressSec, timeoutCallback, 0, orgs));
            NewList list = ObjPool.listPool.borrowObject();
            list.Add(url);
            list.Add(timeoutCallback);
            list.Add(checkProgressSec);
            list.Add(orgs);
            list.Add(0f);
            list.Add(Time.realtimeSinceStartup + checkProgressSec);
            wwwMap4Check[www] = list;//DateEx.nowMS + checkProgressSec*1000;
        }


        public static void checkWWWTimeout()
        {
            NewList keys = ObjPool.listPool.borrowObject();
            keys.AddRange(wwwMap4Check.Keys);
            UnityWebRequest www = null;
            NewList list = null;
            for (int i = 0; i < keys.Count; i++)
            {
                www = keys[i] as UnityWebRequest;
                if (www != null)
                {
                    list = wwwMap4Check[www] as NewList;
                    if (list != null)
                    {
                        doCheckWWWTimeout(www, list);
                    }
                }
            }
            keys.Clear();
            ObjPool.listPool.returnObject(keys);
            keys = null;
        }

        public static void doCheckWWWTimeout(UnityWebRequest www, NewList list)
        {
            //yield return new WaitForSeconds(checkProgressSec);
            string url = list[0] as string;
            object timeoutCallback = list[1];
            float checkProgressSec = (float)(list[2]);
            object orgs = list[3];
            float oldProgress = (float)(list[4]);
            float lastCheckTime = (float)(list[5]);
            if (Time.realtimeSinceStartup - lastCheckTime < 0)
            {
                return;
            }
            try
            {
                if (www != null)
                {
                    if (www.isDone)
                    {
                        wwwMap4Check.Remove(www);
                        list.Clear();
                        ObjPool.listPool.returnObject(list);
                    }
                    else
                    {
                        float curProgress = 0;
                        if (www.method == "PUT")
                        {
                            curProgress = www.uploadProgress;
                        }
                        else
                        {
                            curProgress = www.downloadProgress;
                        }

                        if (Mathf.Abs(curProgress - oldProgress) < 0.0001f)
                        {
                            //说明没有变化，可能网络不给力
                            Coroutine corout = wwwMap4Get[url] as Coroutine;
                            if (corout != null)
                            {
                                self.StopCoroutine(corout);
                            }
                            wwwMap4Get.Remove(url);
                            wwwMapUrl.Remove(url);

                            list.Clear();
                            ObjPool.listPool.returnObject(list);
                            wwwMap4Check.Remove(www);

                            www.Abort();
                            www.Dispose();
                            www = null;
                            Debug.LogError("www time out! url==" + url);
                            Utl.doCallback(timeoutCallback, null, orgs);
                        }
                        else
                        {
                            //Coroutine cor = self.StartCoroutine(doCheckWWWTimeout(www, url, checkProgressSec, timeoutCallback, curProgress, orgs));
                            list[4] = curProgress;
                            list[5] = Time.realtimeSinceStartup + checkProgressSec;
                            wwwMap4Check[www] = list;
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }

        public static void uncheckWWWTimeout(UnityWebRequest www, string url)
        {
            try
            {
                if (www != null && !ReferenceEquals(www, null))
                {
                    wwwMap4Get.Remove(url);
                    NewList list = wwwMap4Check[www] as NewList;
                    if (list != null)
                    {
                        list.Clear();
                        ObjPool.listPool.returnObject(list);
                    }
                    wwwMap4Check.Remove(www);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }

        public static UnityWebRequest getWwwByUrl(string Url)
        {
            if (string.IsNullOrEmpty(Url))
                return null;
            return wwwMapUrl[Url] as UnityWebRequest;
        }

    }
}
