/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   打包工具
  *Others:  
  *History:
*********************************************************************************
*/
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.IO.Compression;
using Coolape;
using System.Collections.Generic;
using CSObjectWrapEditor;
using UnityEditor.Callbacks;
using UnityEditor.Purchasing;
using UnityEditor.SceneManagement;


/// <summary>
/// Coolape Publisher .
/// 库猿编译发布工具
/// 2013-11-16
/// create by chenbin
/// </summary>


public class ECLPublisher : EditorWindow
{
    public delegate void OnClickCallback();

    string[] AndroidIconsName = {
        "192x192",
        "144x144",
        "96x96",
        "72x72",
        "48x48",
        "36x36"
    };

    string[] IosIconsName = {
        "180x180",
        "167x167",
        "152x152",
        "144x144",
        "120x120",
        "114x114",
        "76x76",
        "72x72",
        "57x57",
        "120x120(Spotlight)",
        "80x80(Spotlight)",
        "40x40(Spotlight)",
        "87x87(Settings)",
        "58x58(Settings)",
        "29x29(Settings)",
        "60x60(Notification)",
        "40x40(Notification)",
        "20x20(Notification)",
        "1024x1024(App Store)"
    };

    string[] standandsIconsName = {
        "1024x1024",
        "512x512",
        "256x256",
        "128x128",
        "64x64",
        "32x32",
        "16x16",
    };

    const string configFile = ECLProjectManager.FrameData + "/cfg/publishChannel.cfg";
    //渠道列表
    Hashtable channelMap = new Hashtable();
    ArrayList channelEnum = new ArrayList();
    ArrayList channelKey = new ArrayList();
    ArrayList channelAliasEnum = new ArrayList();
    Hashtable channelData = new Hashtable();

    //配置渠道
    int channelCount = 0;
    int enumCount = 0;
    bool isFinishInit = false;
    string newChlKey = "";
    string newChlName = "";
    string newChlAlias = "";
    string copyChlFromKey = "";
    bool isShowCfgChlDesc = false;
    int currChlIndex = -1;
    bool isComfireDelete = false;
    bool isShowHelpBox = false;
    bool isShowCfgFileDesc = false;
    string currChlKey = "";
    string currChlID = "";
    string currChlAlias = "";
    ChlData currChlData = new ChlData();
    Vector2 scrollPos = Vector2.zero;
    Vector2 scrollPos2 = Vector2.zero;
    bool isShowIcons = false;
    bool isCanEdite = false;
    bool haveModifyChlCfg = false;
    static UnityEngine.Object tmpCopyFolder2PluginObj;
    static bool tmpCopyFolder2PluginUnZip = false;

    string[] tabs = new string[] { "Channel List", "Channel Config" };
    int tabIndex = 0;

    void OnLostFocus()
    {
        isFinishInit = false;
    }
    // 防止在 onGUI 中处理点击事件会出现编辑器报错，从而使用 delayCall 的方式来实现点击处理 - by Ken.
    void onClickOK()
    {
        if (onClickCallbak != null)
        {
            onClickCallbak();
        }
    }

    void OnGUI()
    {
        if (!isFinishInit)
        {
            isFinishInit = true;
            initData();
        }

        if (!ECLProjectSetting.isProjectExit(ECLProjectManager.self))
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 20;
            style.normal.textColor = Color.yellow;
            GUILayout.Label("The scene is not ready, create it now?", style);
            if (GUILayout.Button("Show Project Manager"))
            {
                EditorWindow.GetWindow<ECLProjectManager>(false, "CoolapeProject", true);
            }
            return;
        }
        //==========title============
        tabIndex = GUILayout.Toolbar(tabIndex, tabs);

        //配置渠道
        if (tabIndex == 1)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            {
                showCfgChannel();
            }
            EditorGUILayout.EndScrollView();
        }
        else
        {
            if (haveModifyChlCfg)
            {
                if (EditorUtility.DisplayDialog("Alert", "You have modified the channel config,do you want to save the data?", "Save", "Cancel"))
                {
                    saveData();
                }
                haveModifyChlCfg = false;
            }
            //渠道详细信息
            GUI.color = Color.white;
            GUILayout.BeginHorizontal();
            {
                //--------------------left------------------------------------
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(245));
                {
                    ECLEditorUtl.BeginContents();
                    {
                        //channel list
                        GUI.color = Color.white;
                        if (channelEnum != null)
                        {
                            channelCount = channelEnum.Count;
                            for (int i = 0; i < channelCount; i++)
                            {
                                if (i == currChlIndex)
                                {
                                    GUI.color = Color.yellow;
                                }
                                else
                                {
                                    GUI.color = Color.white;
                                }
                                if (GUILayout.Button(channelEnum[i].ToString() + " (" + channelAliasEnum[i] + ")"))
                                {
                                    currChlKey = channelKey[i].ToString();
                                    currChlID = channelEnum[i].ToString();
                                    currChlAlias = channelAliasEnum[i].ToString();
                                    isComfireDelete = false;
                                    isShowHelpBox = false;
                                    isShowIcons = false;
                                    currChlIndex = i;
                                    getChlData(currChlKey, currChlID);
                                }
                            }
                        }
                    }
                    ECLEditorUtl.EndContents();
                }
                EditorGUILayout.EndScrollView();

                //========right========
                GUI.color = Color.white;
                GUILayout.BeginVertical();
                {
                    if (currChlIndex >= 0 && currChlIndex < channelEnum.Count)
                    {
                        GUILayout.Space(-5);
                        using (new UnityEditorHelper.HighlightBox())
                        {
                            GUILayout.BeginHorizontal();
                            {
                                if (!isComfireDelete && !isShowHelpBox)
                                {
                                    GUI.color = Color.green;
                                    if (GUILayout.Button("Save"))
                                    {
                                        saveChlBuildSetting();
                                    }
                                    GUI.color = Color.green;
                                    if (GUILayout.Button("Chg4Edit"))
                                    {
                                        chgChl4Edit();
                                    }
                                    GUI.color = Color.green;
                                    if (GUILayout.Button("Apply"))
                                    {
                                        applySetting();
                                    }
                                    GUI.color = Color.green;
                                    if (GUILayout.Button("Apply&Build"))
                                    {
                                        showMsgBox(currChlKey + ":确认各参数是否正确!", MessageType.Warning, applyAndBuild);
                                    }
                                    GUI.color = Color.yellow;
                                    if (GUILayout.Button(isCanEdite ? "Lock Edite" : "Unlock Edite"))
                                    {
                                        isCanEdite = !isCanEdite;
                                    }
                                    GUI.color = Color.red;
                                    if (GUILayout.Button("Clean"))
                                    {
                                        isComfireDelete = true;
                                    }
                                }
                                else if (isComfireDelete)
                                {
                                    GUI.color = Color.white;
                                    EditorGUILayout.HelpBox("确定要清除所选的渠道的配置信息？", MessageType.Warning);
                                    GUI.color = Color.green;
                                    if (GUILayout.Button("Cancel"))
                                    {
                                        isComfireDelete = false;
                                    }
                                    GUI.color = Color.red;
                                    if (GUILayout.Button("Clean Now"))
                                    {
                                        isComfireDelete = false;
                                        cleanChlSetting(currChlKey, currChlID);
                                    }
                                }
                                else if (isShowHelpBox)
                                {
                                    GUI.color = Color.white;
                                    EditorGUILayout.HelpBox(msgBoxMsg, msgBoxType);
                                    GUI.color = Color.green;
                                    if (GUILayout.Button("Okey"))
                                    {
                                        isShowHelpBox = false;
                                        EditorApplication.delayCall += onClickOK;
                                    }
                                    GUI.color = Color.red;
                                    if (GUILayout.Button("Cancel"))
                                    {
                                        isShowHelpBox = false;
                                        onClickCallbak = null;
                                    }
                                    GUI.color = Color.white;
                                }
                            }
                            GUILayout.EndHorizontal();
                            GUI.color = Color.white;
                            GUILayout.Space(10);
                        }
                    }
                    GUILayout.Space(-11);
                    scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2);
                    {
                        if (currChlIndex >= 0)
                        {
                            using (new UnityEditorHelper.HighlightBox())
                            {
                                GUILayout.Space(10);
                                GUI.enabled = isCanEdite;
                                channelCell(currChlKey, false);
                                GUI.enabled = true;
                                GUI.backgroundColor = Color.white;
                            }
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
    }

    void showCfgChannel()
    {
        ECLEditorUtl.BeginContents();
        {
            using (new UnityEditorHelper.HighlightBox())
            {
                GUILayout.BeginHorizontal();
                {
                    GUI.color = Color.white;
                    GUILayout.Label("#", GUILayout.Width(20));
                    GUILayout.Label("Key", GUILayout.Width(125));
                    GUILayout.Label("ID", GUILayout.Width(125));
                    GUILayout.Label("Symbols", GUILayout.Width(125));
                    GUILayout.Label("Alias", GUILayout.Width(125));
                    GUI.color = Color.green;
                    if (GUILayout.Button("Save", GUILayout.Width(125)))
                    {
                        saveData();
                        copyChlFromKey = "";
                    }
                    GUI.color = Color.yellow;
                    if (GUILayout.Button("!", GUILayout.Width(20)))
                    {
                        isShowCfgChlDesc = !isShowCfgChlDesc;
                    }
                    GUI.color = Color.white;
                }
                GUILayout.EndHorizontal();

                if (isShowCfgChlDesc)
                {
                    GUI.color = Color.white;
                    EditorGUILayout.HelpBox(
                        "以下为渠道定义。注意当定义好渠道后，" +
                        "可以在对应的渠道代码中加上宏的预编译判断处理渠道特殊的逻辑，" +
                        "例如小米渠道=“mi”，宏为：“CHL_MI”",
                        MessageType.Info);
                }
                showChannelConfigList();
                showNewChannel();
            }
        }
        ECLEditorUtl.EndContents();
    }


    public void showChannelConfigList()
    {
        GUI.color = Color.white;
        if (channelEnum != null)
        {
            enumCount = channelEnum.Count;
            for (int i = 0; i < enumCount; i++)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label((i + 1).ToString(), GUILayout.Width(20));
                    channelKey[i] = GUILayout.TextField(channelKey[i].ToString(), GUILayout.Width(125));
                    channelEnum[i] = GUILayout.TextField(channelEnum[i].ToString(), GUILayout.Width(125));
                    string symbols = "CHL_" + channelEnum[i].ToString().ToUpper();
                    GUILayout.TextField(symbols, GUILayout.Width(125));
                    if (i >= channelAliasEnum.Count)
                    {
                        channelAliasEnum.Add("");
                    }
                    channelAliasEnum[i] = GUILayout.TextField(channelAliasEnum[i] == null ? "" : channelAliasEnum[i].ToString(), GUILayout.Width(125));

                    if (GUILayout.Button("Modify", GUILayout.Width(60f)))
                    {
                        haveModifyChlCfg = true;
                        channelData["channelKey"] = channelKey;
                        channelData["channelEnum"] = channelEnum;
                        channelData["channelAliasEnum"] = channelAliasEnum;
                        return;
                    }
                    if (GUILayout.Button("Copy", GUILayout.Width(60f)))
                    {
                        haveModifyChlCfg = true;
                        copyChlFromKey = channelKey[i].ToString();
                        newChlKey = "Copy " + channelKey[i];
                        newChlName = "Copy " + channelEnum[i];
                        newChlAlias = "Copy " + channelAliasEnum[i];
                    }
                    GUI.color = Color.red;
                    if (GUILayout.Button("-", GUILayout.Width(20)))
                    {
                        if (EditorUtility.DisplayDialog("Alert", string.Format("Really want to delete [{0}] Chanel!", channelKey[i]), "Okay", "Cancel"))
                        {
                            haveModifyChlCfg = true;
                            channelKey.RemoveAt(i);
                            channelEnum.RemoveAt(i);
                            channelAliasEnum.RemoveAt(i);
                            channelData["channelKey"] = channelKey;
                            channelData["channelEnum"] = channelEnum;
                            channelData["channelAliasEnum"] = channelAliasEnum;
                            return;
                        }
                    }
                    GUI.color = Color.white;
                }
                GUILayout.EndHorizontal();
            }
        }
    }

    public void showNewChannel()
    {
        GUILayout.BeginHorizontal();
        {
            GUI.color = Color.cyan;
            GUILayout.Label((enumCount + 1).ToString(), GUILayout.Width(20));
            newChlKey = GUILayout.TextField(newChlKey, GUILayout.Width(125));
            newChlName = GUILayout.TextField(newChlName, GUILayout.Width(125));
            string symbols = "";
            if (!string.IsNullOrEmpty(newChlName))
            {
                symbols = "CHL_" + newChlName.ToUpper();
            }
            GUILayout.TextField(symbols, GUILayout.Width(125));
            newChlAlias = GUILayout.TextField(newChlAlias, GUILayout.Width(125));
            GUI.color = Color.white;

            GUI.color = Color.green;
            if (GUILayout.Button("Add", GUILayout.Width(60f)))
            {
                if (string.IsNullOrEmpty(newChlKey)
                    || string.IsNullOrEmpty(newChlName))
                {
                    EditorUtility.DisplayDialog("Alert", "Channel key || Channel ID is null!", "Okey");
                    return;
                }
                if (!string.IsNullOrEmpty(newChlKey) &&
                    !string.IsNullOrEmpty(newChlName))
                {
                    if (channelMap[newChlKey] != null)
                    {
                        EditorUtility.DisplayDialog("Alert", string.Format("The key [{0}] is allready exist, Plese given a unique key!", newChlKey), "Okey");
                        return;
                    }
                    channelKey.Add(newChlKey);
                    channelEnum.Add(newChlName);
                    channelAliasEnum.Add(newChlAlias);
                    channelData["channelKey"] = channelKey;
                    channelData["channelEnum"] = channelEnum;
                    channelData["channelAliasEnum"] = channelAliasEnum;

                    haveModifyChlCfg = true;

                    if (!string.IsNullOrEmpty(copyChlFromKey))
                    {
                        object tmpData = channelMap[copyChlFromKey];
                        ChlData ChlData = tmpData == null ? new ChlData() : ChlData.parse((Hashtable)tmpData);
                        ChlData.mChlName = newChlName;
                        channelMap[newChlKey] = ChlData.toMap();
                    }

                    newChlName = "";
                    newChlKey = "";
                    newChlAlias = "";
                    saveData();
                }
                else
                {
                    Debug.LogWarning("Please input the channel name!");
                }
            }

            GUI.color = Color.yellow;
            if (GUILayout.Button("Clear", GUILayout.Width(60f)))
            {
                newChlName = "";
                newChlKey = "";
                newChlAlias = "";
            }
            GUI.color = Color.white;
        }
        GUILayout.EndHorizontal();
        if (channelMap.ContainsKey(newChlKey))
        {
            GUI.color = Color.white;
            EditorGUILayout.HelpBox(
                string.Format("The key [{0}] is allready exist, Plese given a unique key!", newChlKey),
                MessageType.Error);
            GUI.color = Color.white;
        }
    }

    string msgBoxMsg = "";
    MessageType msgBoxType = MessageType.None;
    OnClickCallback onClickCallbak;

    void showMsgBox(string msg, MessageType type, OnClickCallback callbak = null)
    {
        msgBoxMsg = msg;
        msgBoxType = type;
        onClickCallbak = callbak;
        isShowHelpBox = true;
    }

    /// <summary>
    /// Inits the data.
    /// 初始化数据
    /// </summary>
    void initData()
    {
        if (FileEx.FileExists(Application.dataPath + "/" + configFile))
        {
            byte[] buffer = FileEx.ReadAllBytes(Application.dataPath + "/" + configFile);
            if (buffer.Length <= 0)
            {
                return;
            }
            MemoryStream ms = new MemoryStream();
            ms.Write(buffer, 0, buffer.Length);
            ms.Position = 0;
            object obj = B2InputStream.readObject(ms);
            if (obj != null)
            {
                channelData = (Hashtable)obj;
                channelEnum = (ArrayList)(channelData["channelEnum"]);
                channelEnum = channelEnum == null ? new ArrayList() : channelEnum;

                channelKey = (ArrayList)(channelData["channelKey"]);
                channelKey = channelKey == null ? new ArrayList() : channelKey;
                if (channelEnum.Count > channelKey.Count)
                {
                    for (int i = 0; i < channelEnum.Count; i++)
                    {
                        channelKey.Add("");
                    }
                }

                channelAliasEnum = (ArrayList)(channelData["channelAliasEnum"]);
                channelAliasEnum = channelAliasEnum == null ? new ArrayList() : channelAliasEnum;
                channelMap = (Hashtable)(channelData["channelMap"]);
                channelMap = channelMap == null ? new Hashtable() : channelMap;

                ArrayList delKeys = new ArrayList();
                foreach (DictionaryEntry item in channelMap)
                {
                    if (!channelKey.Contains(item.Key.ToString()))
                    {
                        delKeys.Add(item.Key.ToString());
                    }
                }

                for (int i = 0; i < delKeys.Count; i++)
                {
                    channelMap.Remove(delKeys[i].ToString());
                }
            }
        }
        else
        {
            channelMap = new Hashtable();
            channelEnum = new ArrayList();
        }
    }

    void refreshData()
    {
        //		isCanEdite = false;
        //		copyChlFromKey = "";
        //		newChlKey = "";
        //		newChlName = "";
        //		newChlAlias = "";
        //		isFinishInit = false;
        //		isFinishInit = false;
        //		isShowIcons = false;
        //		isShowCfgChlDesc = false;
        //		currChlIndex = -1;
        //		isComfireDelete = false;
        //		isShowHelpBox = false;
        //		scrollPos = Vector2.zero;
        //		scrollPos2 = Vector2.zero;
        initData();
    }

    void saveData()
    {
        MemoryStream ms = new MemoryStream();
        B2OutputStream.writeObject(ms, channelData);
        FileEx.WriteAllBytes(Application.dataPath + "/" + configFile, ms.ToArray());
        haveModifyChlCfg = false;
    }

    /// <summary>
    /// Channels the cell.
    /// 渠道单元
    /// </summary>
    void channelCell(string chlName, bool canEdit)
    {
        if (currChlData == null)
        {
            return;
        }
        int width = 200;
        GUI.color = Color.white;
        //Product Name
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Product Name", GUILayout.Width(width));
            currChlData.mProductName = GUILayout.TextField(currChlData.mProductName);
        }
        GUILayout.EndHorizontal();
        //Platform ios or android
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Select Platform", GUILayout.Width(width));
            currChlData.mPlatform = (ChlPlatform)EditorGUILayout.EnumPopup("", currChlData.mPlatform);
            if (currChlData.mPlatform == ChlPlatform.ios)
            {
                currChlData.mCreateEclipseProject = false;
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            if (currChlData.mPlatform == ChlPlatform.android)
            {
                GUILayout.Label("Md5 of APK Sign Code", GUILayout.Width(width));
                currChlData.md5SignCode = GUILayout.TextField(currChlData.md5SignCode);
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("use Unity IAP plugin", GUILayout.Width(width));
            currChlData.isUseUnityIAP = GUILayout.Toggle(currChlData.isUseUnityIAP, "");
        }
        GUILayout.EndHorizontal();

        // app store
        if (currChlData.mPlatform == ChlPlatform.android)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Target Android Store", GUILayout.Width(width));
#if USE_UNITYIAP
				currChlData.mTargetAndroidStore = (UnityEngine.Purchasing.AndroidStore)EditorGUILayout.EnumPopup ("", currChlData.mTargetAndroidStore);
#endif
            }
            GUILayout.EndHorizontal();
        }

        //Default Icon
        GUILayout.BeginHorizontal();
        {
            GUI.enabled = true;
            if (GUILayout.Button("Icons"))
            {
                isShowIcons = !isShowIcons;
            }
            GUI.enabled = isCanEdite;
        }
        GUILayout.EndHorizontal();

        if (isShowIcons)
        {
            using (new UnityEditorHelper.HighlightBox(Color.gray))
            {
                string[] iconsName = { };
                if (currChlData.mPlatform == ChlPlatform.android)
                {
                    iconsName = AndroidIconsName;
                }
                else if (currChlData.mPlatform == ChlPlatform.ios)
                {
                    iconsName = IosIconsName;
                } else if(currChlData.mPlatform == ChlPlatform.standaloneWIN || currChlData.mPlatform == ChlPlatform.StandaloneOSX) {
                    iconsName = standandsIconsName;
                }

                for (int i = 0; i < iconsName.Length; i++)
                {
                    currChlData.mDefaultIcon[iconsName[i]] =
                        EditorGUILayout.ObjectField(iconsName[i],
                        (Texture2D)(currChlData.mDefaultIcon[iconsName[i]]),
                        typeof(Texture2D), false) as Texture2D;
                }
            }
        }
        //Splash Image
        GUILayout.BeginHorizontal();
        {
            currChlData.mSplashImage = EditorGUILayout.ObjectField("Splash Image",
                currChlData.mSplashImage,
                typeof(Texture2D), false) as Texture2D;
        }
        GUILayout.EndHorizontal();
        //Bundle Indentifier
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Bundle Indentifier", GUILayout.Width(width));
            currChlData.mBundleIndentifier = GUILayout.TextField(currChlData.mBundleIndentifier);
        }
        GUILayout.EndHorizontal();
        //Bundle Version
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Bundle Version", GUILayout.Width(width));
            currChlData.mBundleVersion = GUILayout.TextField(currChlData.mBundleVersion);
        }
        GUILayout.EndHorizontal();
        //Bundle Version Code
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Bundle Version Code", GUILayout.Width(width));
            currChlData.mBundleVersionCode = EditorGUILayout.IntField(currChlData.mBundleVersionCode);
        }
        GUILayout.EndHorizontal();
        //Scripting Define Symbols
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Scripting Define Symbols", GUILayout.Width(width));
            GUILayout.Label(currChlData.mScriptingDefineSymbols);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Other Scripting Define Symbols", GUILayout.Width(width));
            currChlData.mOtherScriptingDefineSymbols = GUILayout.TextField(currChlData.mOtherScriptingDefineSymbols);
        }
        GUILayout.EndHorizontal();

        // SubChannel
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("SubChannel", GUILayout.Width(width));
            currChlData.mSubChannel = EditorGUILayout.TextField(currChlData.mSubChannel);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("SubChannel2", GUILayout.Width(width));
            currChlData.mCtccChannel = EditorGUILayout.TextField(currChlData.mCtccChannel);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("isThirdExit", GUILayout.Width(width));
            currChlData.isThirdExit = GUILayout.Toggle(currChlData.isThirdExit, "");
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("isMoreGame", GUILayout.Width(width));
            currChlData.isMoreGame = GUILayout.Toggle(currChlData.isMoreGame, "");
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("isSwitchAccount", GUILayout.Width(width));
            currChlData.isSwitchAccount = GUILayout.Toggle(currChlData.isSwitchAccount, "");
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            GUI.color = Color.yellow;
            GUILayout.Label("isBuildWithLogView", GUILayout.Width(width));
            currChlData.isBuildWithLogView = GUILayout.Toggle(currChlData.isBuildWithLogView, "");
            GUI.color = Color.white;
        }
        GUILayout.EndHorizontal();

        // Create Eclipse Project
        if (currChlData.mPlatform == ChlPlatform.android)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Create Eclipse Project", GUILayout.Width(width));
                currChlData.mCreateEclipseProject = GUILayout.Toggle(currChlData.mCreateEclipseProject, "");
            }
            GUILayout.EndHorizontal();
        }

        //Build Location
        if (currChlData.mPlatform == ChlPlatform.ios ||
            currChlData.mCreateEclipseProject)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Build Location", GUILayout.Width(width));
                currChlData.mBuildLocation = GUILayout.TextField(currChlData.mBuildLocation);
            }
            GUILayout.EndHorizontal();
            GUI.color = Color.yellow;
            GUILayout.Label("***Build Location是相对于工程目录的路径，但是不包括“Assets/”目录!");
            GUI.color = Color.white;
        }
        GUI.color = Color.white;
        //copy files
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Folder Copy to Plugin", GUILayout.Width(width));
            GUI.color = Color.yellow;
            GUILayout.Label("Note: copy to [Assets/Plugins/Android/] or [Assets/Plugins/iOS/]");
            GUI.color = Color.white;
        }
        GUILayout.EndHorizontal();
        for (int i = 0; i < currChlData.mCopyDirPaths.Count; i++)
        {
            Hashtable m = currChlData.mCopyDirPaths[i] as Hashtable;
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("", GUILayout.Width(width));
                //dir:copy路径, unZip:true
                UnityEngine.Object obj = EditorGUILayout.ObjectField(getObjByPath(MapEx.getString(m, "dir")), typeof(UnityEngine.Object));
                if (obj != getObjByPath(MapEx.getString(m, "dir")))
                {
                    if (obj == null)
                    {
                        m["dir"] = "";
                    }
                    else
                    {
                        m["dir"] = AssetDatabase.GetAssetPath(obj.GetInstanceID());
                    }
                }
                m["unZip"] = GUILayout.Toggle(MapEx.getBool(m, "unZip"), "And UnZip the Zip files");
                currChlData.mCopyDirPaths[i] = m;
                if (GUILayout.Button("-", GUILayout.Width(50)))
                {
                    if (EditorUtility.DisplayDialog("Alert", "Really want to remove it?", "Okay", "Cancel"))
                    {
                        currChlData.mCopyDirPaths.RemoveAt(i);
                        break;
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("", GUILayout.Width(width));
            //dir:copy路径, unZip:true
            tmpCopyFolder2PluginObj = EditorGUILayout.ObjectField(tmpCopyFolder2PluginObj, typeof(UnityEngine.Object));
            tmpCopyFolder2PluginUnZip = GUILayout.Toggle(tmpCopyFolder2PluginUnZip, "And UnZip the Zip files");
            if (GUILayout.Button("+", GUILayout.Width(50)))
            {
                if (tmpCopyFolder2PluginObj != null)
                {
                    Hashtable m = new Hashtable();
                    m["dir"] = AssetDatabase.GetAssetPath(tmpCopyFolder2PluginObj.GetInstanceID());
                    m["unZip"] = tmpCopyFolder2PluginUnZip;
                    currChlData.mCopyDirPaths.Add(m);
                    tmpCopyFolder2PluginObj = null;
                    tmpCopyFolder2PluginUnZip = false;
                }
            }
        }
        GUILayout.EndHorizontal();


        //		GUILayout.BeginHorizontal ();
        //		{
        //			GUILayout.Label ("Special Folder Copy to Plugin", GUILayout.Width (width));
        //			currChlData.mSpecialCopyDir = EditorGUILayout.ObjectField (currChlData.mSpecialCopyDir, typeof(UnityEngine.Object));
        //		}
        //		GUILayout.EndHorizontal ();

        GUI.color = Color.white;
        //server
        ECLEditorUtl.BeginContents();
        {
            GUILayout.Label("Server settings", GUILayout.Width(width));
            List<string> toolbarNames = new List<string>();
            for (int i = 0; i < ECLProjectManager.data.hotUpgradeServers.Count; i++)
            {
                HotUpgradeServerInfor dd = ECLProjectManager.data.hotUpgradeServers[i] as HotUpgradeServerInfor;
                toolbarNames.Add(dd.name);
            }
            currChlData.serverIndex = GUILayout.Toolbar(currChlData.serverIndex, toolbarNames.ToArray());
            if (ECLProjectManager.data.hotUpgradeServers.Count > 0)
            {
                HotUpgradeServerInfor selectedServer = ECLProjectManager.data.hotUpgradeServers[currChlData.serverIndex] as HotUpgradeServerInfor;
                currChlData.serverKey = selectedServer.key;

                //===================================================
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Host 4 entry:", GUILayout.Width(width));
                    GUILayout.TextField(selectedServer.host4Entry);
                }
                GUILayout.EndHorizontal();
                //===================================================
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Port 4 entry:", GUILayout.Width(width));
                    GUILayout.TextField(selectedServer.port4Entry.ToString());
                }
                GUILayout.EndHorizontal();
                //===================================================
            }
        }
        ECLEditorUtl.EndContents();

        //publishing settings
        if (currChlData.mPlatform == ChlPlatform.android)
        {
            GUILayout.Label("Publishing settings", GUILayout.Width(width));
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Use Keystore", GUILayout.Width(width));
                currChlData.mLicenseVerification = GUILayout.Toggle(currChlData.mLicenseVerification, "");
            }
            GUILayout.EndHorizontal();

            if (currChlData.mLicenseVerification)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Keystore", GUILayout.Width(width));
                    currChlData.mKeystoreName = EditorGUILayout.ObjectField(currChlData.mKeystoreName, typeof(UnityEngine.Object));
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Keystore Pass", GUILayout.Width(width));
                    currChlData.mKeystorePass = EditorGUILayout.TextField(currChlData.mKeystorePass);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Keyalias Name", GUILayout.Width(width));
                    currChlData.mKeyaliasName = EditorGUILayout.TextField(currChlData.mKeyaliasName);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Keyalias Pass", GUILayout.Width(width));
                    currChlData.mKeyaliasPass = EditorGUILayout.TextField(currChlData.mKeyaliasPass);
                }
                GUILayout.EndHorizontal();
            }
        }
        //Be careful
        GUILayout.BeginHorizontal();
        {
            GUI.color = Color.red;
            GUILayout.Label("Special Instructions", GUILayout.Width(width));
            GUI.color = Color.white;
            GUI.contentColor = Color.yellow;
            currChlData.mAlertDesc = GUILayout.TextArea(currChlData.mAlertDesc, GUILayout.Height(50f));
            GUI.contentColor = Color.white;
        }
        GUILayout.EndHorizontal();
    }

    void getChlData(string chlKey, string chlID)
    {
        currChlData = new ChlData();
        if (channelMap == null)
        {
            return;
        }
        object tmpData = channelMap[chlKey];
        currChlData = tmpData == null ? new ChlData() : ChlData.parse((Hashtable)tmpData);
        currChlData.mChlName = chlID;
    }

    /// <summary>
    /// Saves the chl build setting.
    /// 保存渠道打包设置
    /// </summary>
    void saveChlBuildSetting()
    {
        if (currChlData == null)
        {
            return;
        }
        currChlData.refreshData();
        if (channelMap == null)
        {
            channelMap = new Hashtable();
        }
        channelMap[currChlKey] = currChlData.toMap();
        channelData["channelKey"] = channelKey;
        channelData["channelMap"] = channelMap;
        channelData["channelAliasEnum"] = channelAliasEnum;
        saveData();
    }

    /// <summary>
    /// Cleans the chl setting.
    /// 清空渠道打包设置
    /// </summary>
    /// <param name='chlName'>
    /// Chl name.
    /// </param>
    void cleanChlSetting(string chlName, string chlID)
    {
        channelMap[chlName] = null;
        channelData["channelKey"] = channelKey;
        channelData["channelMap"] = channelMap;
        channelData["channelAliasEnum"] = channelAliasEnum;
        saveData();
        getChlData(chlName, chlID);
    }

    void chgChl4Edit()
    {
        resetScriptingDefineSymbols();

        applyIcons();
    }

    void applyIcons()
    {
        /*
		 * Most platforms support viewing icons in multiple sizes so Unity lets
		 * you specify multiple icon textures for each platform. The list will only 
		 * be assigned if it has the same length as the list of icon sizes returned
		 * by GetIconSizesForTargetGroup and if the specified platform is supported in this editor.
		 */
        List<Texture2D> icons = new List<Texture2D>();
        //		int[] iconSize = PlayerSettings.GetIconSizesForTargetGroup(currChlData.buildTargetGroup);
        //		Debug.Log("iconSize==" + iconSize.Length);
        //		for (int i = 0; i< iconSize.Length; i++) {
        //		}

        string[] iconNames = null;
        if (currChlData.buildTarget == BuildTarget.Android)
        {
            iconNames = AndroidIconsName;
#if USE_UNITYIAP
			UnityPurchasingEditor.TargetAndroidStore (currChlData.mTargetAndroidStore);
#endif
#if UNITY_5 || UNITY_5_3_OR_NEWER
        }
        else if (currChlData.buildTarget == BuildTarget.iOS)
        {
#else
			} else if (currChlData.buildTarget == BuildTarget.iPhone) {
#endif

            iconNames = IosIconsName;
        }
        else if (currChlData.mPlatform == ChlPlatform.standaloneWIN || currChlData.mPlatform == ChlPlatform.StandaloneOSX)
        {
            iconNames = standandsIconsName;
        }

        if (CLCfgBase.self != null)
        {
            CLCfgBase.self.singinMd5Code = currChlData.md5SignCode;
        }

        for (int i = 0; i < iconNames.Length; i++)
        {
            icons.Add((Texture2D)(currChlData.mDefaultIcon[iconNames[i]]));
        }
        PlayerSettings.SetIconsForTargetGroup(currChlData.buildTargetGroup, icons.ToArray());
    }

    void resetScriptingDefineSymbols()
    {
        string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(currChlData.buildTargetGroup);
        if (!string.IsNullOrEmpty(symbols))
        {
            string[] symbolsList = symbols.Split(';');
            for (int i = 0; i < symbolsList.Length; i++)
            {
                if (symbolsList[i].StartsWith("CHL_"))
                {
                    symbolsList[i] = "";
                }
                else if (symbolsList[i].StartsWith("USE_UNITYIAP"))
                {
                    symbolsList[i] = "";
                }
            }
            symbols = "";
            for (int i = 0; i < symbolsList.Length; i++)
            {
                if (!string.IsNullOrEmpty(symbolsList[i]))
                {
                    symbols += symbolsList[i] + ";";
                }
            }
        }
        symbols += (";" + currChlData.mScriptingDefineSymbols);
        if (currChlData.isUseUnityIAP)
        {
            symbols += (";USE_UNITYIAP");
        }
        if(!string.IsNullOrEmpty(currChlData.mOtherScriptingDefineSymbols))
        {
            symbols += (";" + currChlData.mOtherScriptingDefineSymbols);
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(currChlData.buildTargetGroup, symbols);

        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, symbols);
#if UNITY_5 || UNITY_5_3_OR_NEWER
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, symbols);
#else
		PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.iPhone, symbols);
#endif

        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbols);
    }

    /// <summary>
    /// Applies the setting.
    /// 应用渠道设置
    /// </summary>
    void applySetting()
    {
        if (currChlData == null)
        {
            return;
        }
        PlayerSettings.productName = currChlData.mProductName;
        //Icons
        applyIcons();

        //Splash imgae
        PlayerSettings.resolutionDialogBanner = currChlData.mSplashImage;
        //indentifier & version

#if UNITY_5_6_OR_NEWER
        PlayerSettings.applicationIdentifier = currChlData.mBundleIndentifier;
        PlayerSettings.SetApplicationIdentifier(currChlData.buildTargetGroup, currChlData.mBundleIndentifier);
#else
		PlayerSettings.bundleIdentifier = currChlData.mBundleIndentifier;
#endif
        PlayerSettings.bundleVersion = currChlData.mBundleVersion;
        //		modifyCfgFile();
        if (currChlData.buildTargetGroup == BuildTargetGroup.Android)
        {
            PlayerSettings.Android.bundleVersionCode = currChlData.mBundleVersionCode;
        } else if (currChlData.buildTargetGroup == BuildTargetGroup.iOS)
        {
            PlayerSettings.iOS.buildNumber = currChlData.mBundleVersionCode + "";
        }

        resetScriptingDefineSymbols();

        // subchannel
        string chlCfgPath = Application.streamingAssetsPath + "/chnCfg.json";
        Hashtable chlMap = null;
        if (File.Exists(chlCfgPath))
        {
            chlMap = JSON.DecodeMap(File.ReadAllText(chlCfgPath));
        }
        else
        {
            chlMap = new Hashtable();
        }
        chlMap["SubChannel"] = currChlData.mSubChannel;
        chlMap["SubChannel2"] = currChlData.mCtccChannel;
        chlMap["chn"] = currChlData.mChlName;
        chlMap["isThirdExit"] = currChlData.isThirdExit;
        chlMap["isMoreGame"] = currChlData.isMoreGame;
        chlMap["IsSwitchAccount"] = currChlData.isSwitchAccount;
        File.WriteAllText(chlCfgPath, JSON.JsonEncode(chlMap));

        // copy files
        if (currChlData.mCopyDirPaths != null)
        {
            Hashtable m = null;
            for (int i = 0; i < currChlData.mCopyDirPaths.Count; i++)
            {
                m = currChlData.mCopyDirPaths[i] as Hashtable;
                copyFilesToPlugin(MapEx.getString(m, "dir"), MapEx.getBool(m, "unZip"));
            }
        }
        // special copy
        //		if (currChlData.mSpecialCopyDir != null) {
        //			copyFilesToPlugin (currChlData.mSpecialCopyDirPath);
        //		}

        //
        if (currChlData.mPlatform == ChlPlatform.android)
        {
            //			PlayerSettings.Android.licenseVerification = currChlData.mLicenseVerification;
            //			PlayerSettings.Android
            if (currChlData.mLicenseVerification)
            {
                PlayerSettings.Android.keystoreName = currChlData.mKeystoreNamePath;
                PlayerSettings.Android.keystorePass = currChlData.mKeystorePass;
                PlayerSettings.Android.keyaliasName = currChlData.mKeyaliasName;
                PlayerSettings.Android.keyaliasPass = currChlData.mKeyaliasPass;
            }
            else
            {
                PlayerSettings.Android.keystoreName = "";
                PlayerSettings.Android.keystorePass = "";
                PlayerSettings.Android.keyaliasName = "";
                PlayerSettings.Android.keyaliasPass = "";
            }
        }

        if (currChlData.isBuildWithLogView && ReporterMessageReceiver.self == null)
        {
            ReporterEditor.CreateReporter();
            ReporterModificationProcessor.BuildInfo.addUpdateDelegate();
        }
        if (ECLProjectManager.data.hotUpgradeServers != null &&
            ECLProjectManager.data.hotUpgradeServers.Count > 0)
        {
            HotUpgradeServerInfor serverInfor = ECLProjectManager.data.hotUpgradeServers[currChlData.serverIndex] as HotUpgradeServerInfor;
            CLVerManager.self.baseUrl = serverInfor.hotUpgradeBaseUrl;
            Net.self.host4Publish = serverInfor.host4Entry;
            Net.self.gatePort = serverInfor.port4Entry;
        }
    }

    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            //			string projPath = PBXProject.GetPBXProjectPath(path);
            //			PBXProject proj = new PBXProject();
            //
            //			proj.ReadFromString(File.ReadAllText(projPath));
            //			string target = proj.TargetGuidByName("Unity-iPhone");
            //
            //			// システムのフレームワークを追加
            //			proj.AddFrameworkToProject(target, "AssetsLibrary.framework", false);
            //
            //			// 自前のフレームワークを追加
            //			CopyAndReplaceDirectory("Assets/Lib/mylib.framework", Path.Combine(path, "Frameworks/mylib.framework"));
            //			proj.AddFileToBuild(target, proj.AddFile("Frameworks/mylib.framework", "Frameworks/mylib.framework", PBXSourceTree.Source));
            //
            //			// ファイルを追加
            //			var fileName = "my_file.xml";
            //			var filePath = Path.Combine("Assets/Lib", fileName);
            //			File.Copy(filePath, Path.Combine(path, fileName));
            //			proj.AddFileToBuild(target, proj.AddFile(fileName, fileName, PBXSourceTree.Source));
            //
            //			// Yosemiteでipaが書き出せないエラーに対応するための設定
            //			proj.SetBuildProperty(target, "CODE_SIGN_RESOURCE_RULES_PATH", "$(SDKROOT)/ResourceRules.plist");
            //
            //			// フレームワークの検索パスを設定・追加
            //			proj.SetBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
            //			proj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Frameworks");
            //
            //			// 書き出し
            //			File.WriteAllText(projPath, proj.WriteToString());
        }
    }

    /// <summary>
    /// Applies the and build.
    /// 设置并编译
    /// </summary>
    void applyAndBuild()
    {
        if (currChlData == null)
        {
            return;
        }
#if UNITY_5 || UNITY_5_3_OR_NEWER
        if (currChlData.buildTarget == BuildTarget.iOS)
        {
#else
		if (currChlData.buildTarget == BuildTarget.iPhone) {
#endif
            if (string.IsNullOrEmpty(currChlData.mBuildLocation))
            {
                EditorUtility.DisplayDialog("Alert", "The BuildLocation is empty!", null);
                return;
            }
        }

        applySetting();

        if (!File.Exists(GeneratorConfig.common_path + "XLuaGenAutoRegister.cs"))
        {
            if (!EditorUtility.DisplayDialog("Alert", "Code has not been genrated for Xlua!", "Generate Now", "Cancel"))
            {
                return;
            }
            else
            {
                Generator.GenAll();
            }
        }

        //如果平台不一样，先切到
        if (EditorUserBuildSettings.activeBuildTarget != currChlData.buildTarget)
        {
            //			EditorUserBuildSettings.SwitchActiveBuildTarget (currChlData.buildTarget);
            EditorUtility.DisplayDialog("Alert", "The active build target is not the same with current build target!", null);
            return;
        }

        if (currChlData.mCreateEclipseProject)
        {
            Debug.LogWarning("The channel need Create Eclipse Project!!");
            showMsgBox("The channel need Create Eclipse Project!!", MessageType.Warning);
            return;
        }

        AssetDatabase.Refresh();

        string[] levels = null;
        if (EditorSceneManager.sceneCount > 0)
        {
            levels = new string[EditorSceneManager.sceneCount];
            for (int i = 0; i < EditorSceneManager.sceneCount; i++)
            {
                levels[i] = EditorSceneManager.GetSceneAt(i).path;
            }
        }
        else
        {
            levels = new string[1];
            levels[0] = EditorSceneManager.GetActiveScene().path;
        }

        string locationName = currChlData.mPlatform.ToString() + "_" + currChlData.mChlName + "_" + currChlData.mBundleIndentifier + "_v" + currChlData.mBundleVersion + "_" + DateEx.format(DateEx.fmt_yyyy_MM_dd_HH_mm_ss_fname);
        //		string locationName = currChlData.mBundleIndentifier;
        //		locationName = locationName.Substring(locationName.LastIndexOf(".")+1);
        //		locationName += "_v" + currChlData.mBundleVersion + "_" + DateEx.format (DateEx.fmt_yyyy_MM_dd_HH_mm_ss_fname);

        if (currChlData.mPlatform == ChlPlatform.android)
        {
            if (!currChlData.mCreateEclipseProject)
            {
                locationName += ".apk";
            }
            else
            {
                locationName = dataPath() + currChlData.mBuildLocation;
            }
#if UNITY_5 || UNITY_5_3_OR_NEWER
        }
        else if (currChlData.buildTarget == BuildTarget.iOS)
        {
#else
		} else if (currChlData.buildTarget == BuildTarget.iPhone) {
#endif
            //			EditorUserBuildSettings.appendProject = true;
            locationName = dataPath() + currChlData.mBuildLocation;
        }
        else if (currChlData.buildTarget == BuildTarget.StandaloneWindows64)
        {
            locationName = currChlData.mProductName + ".exe";
        }
        else if (currChlData.buildTarget == BuildTarget.StandaloneOSX)
        {
            locationName = currChlData.mProductName + ".app";
        }
        bool needApend = true;
        if (!string.IsNullOrEmpty(currChlData.mBuildLocation))
        {
            if (!Directory.Exists(locationName))
            {
                needApend = false;
                Directory.CreateDirectory(locationName);
            }
        }

        Debug.Log("Publish path:" + locationName);

        if (Reporter.self != null)
        {
            Reporter.self.gameObject.SetActive(false);
        }
#if UNITY_5 || UNITY_5_3_OR_NEWER
        if (currChlData.buildTarget == BuildTarget.iOS && needApend)
        {
#else
		if (currChlData.buildTarget == BuildTarget.iPhone && needApend) {
#endif
            BuildPipeline.BuildPlayer(levels, locationName, currChlData.buildTarget, BuildOptions.AcceptExternalModificationsToPlayer);
        }
        else
        {
            BuildPipeline.BuildPlayer(levels, locationName, currChlData.buildTarget, BuildOptions.None);
        }

        if (Reporter.self != null)
        {
            SceneAsset.DestroyImmediate(Reporter.self.gameObject);
            ReporterModificationProcessor.BuildInfo.rmUpdateDelegate();
        }

        string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(currChlData.buildTargetGroup);
        symbols.Replace(";USE_UNITYIAP", "");
        PlayerSettings.SetScriptingDefineSymbolsForGroup(currChlData.buildTargetGroup, symbols);
    }

    //	void modifyCfgFile()
    //	{
    //		if (string.IsNullOrEmpty(cfgFilePath)) {
    //			return;
    //		}
    //		string[] buffer = File.ReadAllLines(dataPath() + cfgFilePath);
    //		string line = "";
    //		bool finishModifyVer = false;
    //		bool finishModifyVerCoder = false;
    //		for (int i = 0; i < buffer.Length; i++) {
    //			line = buffer [i];
    //			if (line.IndexOf("public static string version") >= 0) {
    //				buffer [i] = "    public static string version = \"" + currChlData.mBundleVersion + "\";";
    //				finishModifyVer = true;
    //			} else if (line.IndexOf("public static int versionCode") >= 0) {
    //				buffer [i] = "    public static int versionCode = " + currChlData.mBundleVersionCode + ";";
    //				finishModifyVerCoder = true;
    //			}
    //			if (finishModifyVer && finishModifyVerCoder) {
    //				break;
    //			}
    //		}
    //		File.WriteAllLines(dataPath() + cfgFilePath, buffer);
    //	}

    /// <summary>
    /// Copies the files to plugin.
    /// 把文件拷贝到Plugin目录
    /// </summary>
    void copyFilesToPlugin(string fromPath, bool unZip)
    {
        string toPath = "";
        if (currChlData.mPlatform == ChlPlatform.android)
        {
            toPath = "Assets/Plugins/Android/";
        }
        else if (currChlData.mPlatform == ChlPlatform.ios)
        {
            toPath = "Assets/Plugins/iOS/";
        }
        Debug.Log(dataPath());
        doCopyFiles(dataPath() + fromPath, dataPath() + toPath, unZip);
    }

    /// <summary>
    /// Datas the path.
    /// 取得工程路径，不带"/Assets/"
    /// </summary>
    /// <returns>
    /// The path.
    /// </returns>
    string dataPath()
    {
        string tmpPath = Application.dataPath + "/";
        return tmpPath.Replace("/Assets/", "/");
    }

    void doCopyFiles(string fromPath, string toPath, bool unZip)
    {
        string[] fileEntries = Directory.GetFiles(fromPath);
        string extension = "";
        string fileName = "";
        foreach (string filePath in fileEntries)
        {
            extension = Path.GetExtension(filePath);
            if (ECLEditorUtl.isIgnoreFile(filePath))
            {
                continue;
            }
            fileName = Path.GetFileName(filePath);
            if (unZip && extension.ToLower() == ".zip")
            {
                ZipEx.UnZip(filePath, toPath, 4096);
            }
            else
            {
                if (File.Exists(toPath + fileName))
                {
                    File.Delete(toPath + fileName);
                }
                File.Copy(filePath, toPath + fileName);
            }
        }
        string[] dirEntries = Directory.GetDirectories(fromPath);
        string dirName = "";
        string newToPath = "";
        foreach (string dir in dirEntries)
        {
            dirName = Path.GetFileName(dir);
            newToPath = toPath + dirName + "/";
            if (!Directory.Exists(newToPath))
            {
                Directory.CreateDirectory(newToPath);
            }
            doCopyFiles(dir, newToPath, unZip);
        }
    }

    static Hashtable __objsMap = new Hashtable();

    public static UnityEngine.Object getObjByPath(string path)
    {
        UnityEngine.Object obj = __objsMap[path] as UnityEngine.Object;
        if (obj == null)
        {
            if (!string.IsNullOrEmpty(path))
            {
                obj = AssetDatabase.LoadAssetAtPath(
                    path,
                    typeof(UnityEngine.Object));
            }
        }
        __objsMap[path] = obj;
        return obj;
    }

}

/// <summary>
/// Chl platform.
/// 平台
/// </summary>
public enum ChlPlatform
{
    android,
    ios,
    standaloneWIN,
    StandaloneOSX,
}

/// <summary>
/// Chl data.
/// 渠道数据
/// </summary>
public class ChlData
{
    public string mChlName = "";
    public string mProductName = "";
    public ChlPlatform mPlatform = ChlPlatform.android;
    public bool isUseUnityIAP = false;
#if USE_UNITYIAP
	public UnityEngine.Purchasing.AndroidStore mTargetAndroidStore = UnityEngine.Purchasing.AndroidStore.GooglePlay;
#endif

    public BuildTargetGroup buildTargetGroup
    {
        get
        {
            switch (mPlatform)
            {
                case ChlPlatform.android:
                    return BuildTargetGroup.Android;
                case ChlPlatform.ios:
#if UNITY_5 || UNITY_5_3_OR_NEWER
                    return BuildTargetGroup.iOS;
#else
				return BuildTargetGroup.iPhone;
#endif
                case ChlPlatform.standaloneWIN:
                case ChlPlatform.StandaloneOSX:
                    return BuildTargetGroup.Standalone;
            }
            return BuildTargetGroup.Android;
        }
    }

    public BuildTarget buildTarget
    {
        get
        {
            switch (mPlatform)
            {
                case ChlPlatform.android:
                    return BuildTarget.Android;
                case ChlPlatform.ios:
#if UNITY_5 || UNITY_5_3_OR_NEWER
                    return BuildTarget.iOS;
#else
				return BuildTarget.iPhone;
#endif
                case ChlPlatform.standaloneWIN:
                    return BuildTarget.StandaloneWindows64;
                case ChlPlatform.StandaloneOSX:
                    return BuildTarget.StandaloneOSX;
            }
            return BuildTarget.StandaloneWindows64;
        }
    }

    //	public BuildTarget

    Hashtable _DefaultIcon;
    //Texture2D
    Texture2D _SplashImage;

    public Hashtable mDefaultIcon
    {
        get
        {
            if (_DefaultIcon == null && mDefaultIconPath != null)
            {
                _DefaultIcon = new Hashtable();
                foreach (DictionaryEntry cell in mDefaultIconPath)
                {
                    string iconPath = cell.Value.ToString();
                    _DefaultIcon[cell.Key] = (Texture2D)(AssetDatabase.LoadAssetAtPath("Assets/" + iconPath, typeof(Texture2D)));
                }
            }
            if (_DefaultIcon == null)
            {
                _DefaultIcon = new Hashtable();
            }
            return _DefaultIcon;
        }
    }

    public Texture2D mSplashImage
    {
        get
        {
            if (_SplashImage == null && !string.IsNullOrEmpty(mSplashImagePath))
            {
                _SplashImage = (Texture2D)(AssetDatabase.LoadAssetAtPath("Assets/" + mSplashImagePath, typeof(Texture2D)));

            }
            return _SplashImage;
        }
        set
        {
            _SplashImage = value;
            if (value == null)
            {
                mSplashImagePath = "";
            }
        }
    }

    public Hashtable mDefaultIconPath = new Hashtable();
    public string mSplashImagePath;
    public string mBundleIndentifier = "";
    public string mBundleVersion = "";
    public int mBundleVersionCode;
    public bool mCreateEclipseProject = false;
    public bool isBuildWithLogView = false;
    public string mBuildLocation = "iosBuild";

    public string mScriptingDefineSymbols
    {
        get
        {
            return "CHL_" + mChlName.ToUpper();
        }
    }
    public string mOtherScriptingDefineSymbols = "";

    public string mSubChannel = "";
    public string mCtccChannel = "";
    public bool isThirdExit = false;
    public bool isMoreGame = false;
    public bool isSwitchAccount = false;
    public string mAlertDesc = "";
    public ArrayList mCopyDirPaths = new ArrayList();
    public bool mLicenseVerification;
    public string mKeystoreNamePath = "";
    UnityEngine.Object _KeystoreName;

    public UnityEngine.Object mKeystoreName
    {
        get
        {
            if (_KeystoreName == null)
            {
                if (!string.IsNullOrEmpty(mKeystoreNamePath))
                {
                    _KeystoreName = AssetDatabase.LoadAssetAtPath(
                        mKeystoreNamePath,
                        typeof(UnityEngine.Object));
                }
            }
            return _KeystoreName;
        }
        set
        {
            _KeystoreName = value;
            if (value == null)
            {
                mKeystoreNamePath = "";
            }
            else
            {
                mKeystoreNamePath = AssetDatabase.GetAssetPath(_KeystoreName.GetInstanceID());
            }
        }
    }

    public string mKeystorePass;
    public string mKeyaliasName;
    public string mKeyaliasPass;
    public string md5SignCode = "";

    public string serverKey = "";
    public int serverIndex = 0;

    public Hashtable toMap()
    {
        Hashtable r = new Hashtable();
        r["mChlNmae"] = mChlName;
        r["mProductName"] = mProductName;
        r["mPlatform"] = mPlatform.ToString();
        r["isUseUnityIAP"] = isUseUnityIAP;
#if USE_UNITYIAP
		r ["mTargetAndroidStore"] = (int)mTargetAndroidStore; //UnityEngine.Purchasing.AndroidStore
#endif
        r["mDefaultIconPath"] = mDefaultIconPath;
        r["mSplashImagePath"] = mSplashImagePath;
        r["mBundleIndentifier"] = mBundleIndentifier;
        r["mBundleVersion"] = mBundleVersion;
        r["mBundleVersionCode"] = mBundleVersionCode;
        r["mCreateEclipseProject"] = mCreateEclipseProject;
        r["mBuildLocation"] = mBuildLocation;
        r["mOtherScriptingDefineSymbols"] = mOtherScriptingDefineSymbols;
        r["mAlertDesc"] = mAlertDesc;
        r["mCopyDirPaths"] = mCopyDirPaths;
        //		r ["mSpecialCopyDir"] = mSpecialCopyDirPath;
        //		r ["mUnZip"] = mUnZip;
        r["mSubChannel"] = mSubChannel;
        r["mCtccChannel"] = mCtccChannel;

        r["mLicenseVerification"] = mLicenseVerification;
        r["mKeystoreNamePath"] = mKeystoreNamePath;
        r["mKeystorePass"] = mKeystorePass;
        r["mKeyaliasName"] = mKeyaliasName;
        r["mKeyaliasPass"] = mKeyaliasPass;

        r["isThirdExit"] = isThirdExit;
        r["isMoreGame"] = isMoreGame;
        r["isSwitchAccount"] = isSwitchAccount;
        r["isBuildWithLogView"] = isBuildWithLogView;
        r["md5SignCode"] = md5SignCode;
        r["serverKey"] = serverKey;
        return r;
    }

    public void refreshData()
    {
        if (_DefaultIcon != null)
        {
            foreach (DictionaryEntry cell in _DefaultIcon)
            {
                Texture2D icon = cell.Value == null ? null : (Texture2D)(cell.Value);
                if (icon == null)
                    continue;
                string tmpPath = AssetDatabase.GetAssetPath(icon.GetInstanceID());
                int startPos = 0;
                startPos = tmpPath.IndexOf("Assets/");
                startPos += 7;
                tmpPath = tmpPath.Substring(startPos, tmpPath.Length - startPos);
                if (mDefaultIconPath == null)
                {
                    mDefaultIconPath = new Hashtable();
                }
                mDefaultIconPath[cell.Key] = tmpPath;
            }
        }
        if (_SplashImage != null)
        {
            string tmpPath = AssetDatabase.GetAssetPath(_SplashImage.GetInstanceID());
            int startPos = 0;
            startPos = tmpPath.IndexOf("Assets/");
            startPos += 7;
            tmpPath = tmpPath.Substring(startPos, tmpPath.Length - startPos);
            mSplashImagePath = tmpPath;
        }
    }

    public static ChlData parse(Hashtable map)
    {
        if (map == null)
        {
            return null;
        }
        ChlData r = new ChlData();
        r.mChlName = MapEx.getString(map, "mChlNmae");
        r.mProductName = MapEx.getString(map, "mProductName");
        r.mDefaultIconPath = MapEx.getMap(map, "mDefaultIconPath");
        r.mSplashImagePath = MapEx.getString(map, "mSplashImagePath");
        r.mBundleIndentifier = MapEx.getString(map, "mBundleIndentifier");
        r.mBundleVersion = MapEx.getString(map, "mBundleVersion");
        r.mBundleVersionCode = MapEx.getInt(map, "mBundleVersionCode");
        string platform = MapEx.getString(map, "mPlatform");
        switch (platform)
        {
            case "android":
                r.mPlatform = ChlPlatform.android;
                break;
            case "ios":
                r.mPlatform = ChlPlatform.ios;
                break;
            case "standaloneWIN":
                r.mPlatform = ChlPlatform.standaloneWIN;
                break;
            case "StandaloneOSX":
                r.mPlatform = ChlPlatform.StandaloneOSX;
                break;
        }
        r.isUseUnityIAP = MapEx.getBool(map, "isUseUnityIAP");
#if USE_UNITYIAP
		r.mTargetAndroidStore = (UnityEngine.Purchasing.AndroidStore)(Enum.ToObject (typeof(UnityEngine.Purchasing.AndroidStore), MapEx.getInt (map, "mTargetAndroidStore")));
#endif
        r.mCreateEclipseProject = MapEx.getBool(map, "mCreateEclipseProject");
        r.mBuildLocation = MapEx.getString(map, "mBuildLocation");
        r.mOtherScriptingDefineSymbols = MapEx.getString(map, "mOtherScriptingDefineSymbols");
        r.mAlertDesc = MapEx.getString(map, "mAlertDesc");
        r.mAlertDesc = r.mAlertDesc == null ? "" : r.mAlertDesc;
        r.mCopyDirPaths = MapEx.getList(map, "mCopyDirPaths");
        //		r.mSpecialCopyDirPath = MapEx.getString (map, "mSpecialCopyDir");
        //		r.mUnZip = MapEx.getBool (map, "mUnZip");
        r.mLicenseVerification = MapEx.getBool(map, "mLicenseVerification");
        r.mKeystoreNamePath = MapEx.getString(map, "mKeystoreNamePath");
        r.mKeystorePass = MapEx.getString(map, "mKeystorePass");
        r.mKeyaliasName = MapEx.getString(map, "mKeyaliasName");
        r.mKeyaliasPass = MapEx.getString(map, "mKeyaliasPass");
        r.mSubChannel = MapEx.getString(map, "mSubChannel");
        r.mCtccChannel = MapEx.getString(map, "mCtccChannel");
        r.isThirdExit = MapEx.getBool(map, "isThirdExit");
        r.isMoreGame = MapEx.getBool(map, "isMoreGame");
        r.isSwitchAccount = MapEx.getBool(map, "isSwitchAccount");
        r.isBuildWithLogView = MapEx.getBool(map, "isBuildWithLogView");
        r.md5SignCode = MapEx.getString(map, "md5SignCode");
        r.serverKey = MapEx.getString(map, "serverKey");
        r.serverIndex = getServerIndex(r.serverKey);
        return r;
    }

    static int getServerIndex(string serverKey)
    {
        for (int i = 0; i < ECLProjectManager.data.hotUpgradeServers.Count; i++)
        {
            HotUpgradeServerInfor dd = ECLProjectManager.data.hotUpgradeServers[i] as HotUpgradeServerInfor;
            if (dd.key.Equals(serverKey))
            {
                return i;
            }
        }
        return 0;
    }
}
