/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   项目管理页面
  *			在方法中加上[PublishRes]后，当oneKeyRefresh时会调用，这样可以做一些特殊处理,返回true时默认处理
  *			在方法中加上[SkipCollectRes]后，返回true时，跳过资源共享的处理
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using UnityEditor;
using System.Collections;
using Coolape;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEditorHelper;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Reflection;
using System.Linq;

public class ECLProjectManager : EditorWindow
{
	public static ECLProjectManager self;
	public const string FrameName = "CoolapeFrame";
	public const string FrameData = "CoolapeFrameData";
	public const string configFile = FrameData + "/cfg/projcet.cfg";
	public const string resModifyDateCfg = FrameData + "/verControl/.resModifyDate.v";

	public const string preUpgradeListName = "preUpgradeList";
	#if UNITY_ANDROID
	// 开发中的版本文件
	public const string ver4DevelopeMd5 = FrameData + "/verControl/android/ver4DevelopeMd5.v";
	//打包时的版本
	public const string ver4Publish = FrameData + "/verControl/android/ver4Publish.v";
	//每次更新时的版本
	public const string ver4Upgrade = FrameData + "/verControl/android/ver4Upgrade.v";
	//每次更新时的版本
	public const string ver4UpgradeMd5 = FrameData + "/verControl/android/ver4UpgradeMd5.v";
	// 每次更新的状态管理
	public const string ver4UpgradeList = FrameData + "/verControl/android/ver4UpgradeList.v";
#elif UNITY_IOS
	// 开发中的版本文件
	public const string ver4DevelopeMd5 = FrameData + "/verControl/IOS/ver4DevelopeMd5.v";				
	//打包时的版本
	public const string ver4Publish = FrameData + "/verControl/IOS/ver4Publish.v";											
	//每次更新时的版本
	public const string ver4Upgrade = FrameData + "/verControl/IOS/ver4Upgrade.v";							
	//每次更新时的版本
	public const string ver4UpgradeMd5 = FrameData + "/verControl/IOS/ver4UpgradeMd5.v";
	// 每次更新的状态管理
	public const string ver4UpgradeList = FrameData + "/verControl/IOS/ver4UpgradeList.v";
#endif

#if UNITY_STANDALONE_WIN
	// 开发中的版本文件
	public const string ver4DevelopeMd5 = FrameData + "/verControl/Standalone/ver4DevelopeMd5.v";
	//打包时的版本
	public const string ver4Publish = FrameData + "/verControl/Standalone/ver4Publish.v";
	//每次更新时的版本
	public const string ver4Upgrade = FrameData + "/verControl/Standalone/ver4Upgrade.v";
	//每次更新时的版本
	public const string ver4UpgradeMd5 = FrameData + "/verControl/Standalone/ver4UpgradeMd5.v";
	// 每次更新的状态管理
	public const string ver4UpgradeList = FrameData + "/verControl/Standalone/ver4UpgradeList.v";
#endif

#if UNITY_STANDALONE_OSX
    // 开发中的版本文件
    public const string ver4DevelopeMd5 = FrameData + "/verControl/StandaloneOSX/ver4DevelopeMd5.v";
    //打包时的版本
    public const string ver4Publish = FrameData + "/verControl/StandaloneOSX/ver4Publish.v";
    //每次更新时的版本
    public const string ver4Upgrade = FrameData + "/verControl/StandaloneOSX/ver4Upgrade.v";
    //每次更新时的版本
    public const string ver4UpgradeMd5 = FrameData + "/verControl/StandaloneOSX/ver4UpgradeMd5.v";
    // 每次更新的状态管理
    public const string ver4UpgradeList = FrameData + "/verControl/StandaloneOSX/ver4UpgradeList.v";
#endif

    const int labWidth = 200;
	static bool isFinishInit = false;
	string u3dfrom = "";
	string u3dto = "";
	string newProjName = "";
	Vector2 scrollPos = Vector2.zero;
	public static Texture2D tabShow = null;
	public static Texture2D tabHide = null;
	public static Texture2D tabTexture = null;
	static Hashtable _resModifyDateMap = null;
	public static string sepecificPublishConfig = "";

	public static Hashtable resModifyDateMap {
		get {
			if (_resModifyDateMap == null) {
				_resModifyDateMap = fileToMap (Application.dataPath + "/" + resModifyDateCfg);
			}
			return _resModifyDateMap;
		}
		set {
			_resModifyDateMap = value;
		}
	}

	bool state1 = true;
	bool state2 = true;
	bool state3 = false;
	bool state4 = false;
	bool state5 = false;
	static ProjectData _data;

	public static ProjectData data {
		get {
			if (!isFinishInit || _data == null) {
				isFinishInit = true;
				initData ();
			}
			return _data;
		}
		set {
			_data = value;
		}
	}

	public ECLProjectManager ()
	{
		self = this;
	}

	void OnGUI ()
	{
		if (!isFinishInit || _data == null) {
			isFinishInit = true;
			initData ();
			if (ECLProjectSetting.isProjectExit (this)) {
				state1 = false;
			} else {
				state1 = true;
			}
		}
		if (data == null) {
			return;
		}
		//================================================================
		// Project Config
		//================================================================
		GUI.color = Color.white;
		scrollPos = EditorGUILayout.BeginScrollView (scrollPos);
		{
			tabTexture = state1 ? tabHide : tabShow;
			using (new FoldableBlock (ref state1, "Project Config", tabTexture)) {
				if (state1) {
					if (!ECLProjectSetting.isProjectExit (this)) {
						GUIStyle style = new GUIStyle ();
						style.fontSize = 20;
						style.normal.textColor = Color.yellow;
						GUILayout.Label ("The scene is not ready, create it now?", style);

						if (!string.IsNullOrEmpty (data.name)) {
							string scenePath = PStr.b ().a ("Assets/").a (data.name).a ("/_scene/Main.unity").e ();
							UnityEngine.Object mainScene = ECLEditorUtl.getObjectByPath (scenePath);
							if (mainScene != null) {
								//===================================================
								GUILayout.BeginHorizontal ();
								{
									GUILayout.Label ("There Had A Main Scene:", GUILayout.Width (labWidth));
									EditorGUILayout.ObjectField (mainScene, typeof(UnityEngine.Object));

									GUI.color = Color.green;
									if (GUILayout.Button ("Open")) {
										Scene current = EditorSceneManager.GetActiveScene ();
										if (current.isDirty) {
											if (EditorUtility.DisplayDialog ("Alert", string.Format ("Scene[{0}] have been modifed!", current.name), "Save", "Cancel")) {
												EditorSceneManager.SaveOpenScenes ();
											}
										}
										EditorSceneManager.OpenScene (scenePath);
									}
									GUI.color = Color.white;
								}
								GUILayout.EndHorizontal ();
								//===================================================
							}
						}
					}
					//展示工程的配置信息
					try {
						ECLProjectSetting.showProjectInfor (this);
					} catch (Exception e) {
						Debug.LogWarning (e);
					}
				}
			}

			//================================================================
			// Refresh AssetBundles
			//================================================================
			GUILayout.Space (5);
			GUI.color = Color.white;
			if (ECLProjectSetting.isProjectExit (this)) {
				GUILayout.Space (5);
				tabTexture = state2 ? tabHide : tabShow;
				using (new FoldableBlock (ref state2, "Refresh AssetBundles", tabTexture)) {
					if (state2) {
						using (new HighlightBox ()) {
							GUI.color = Color.green;
							if (GUILayout.Button ("One Key Refresh All AssetBundles", GUILayout.Width (300), GUILayout.Height (50))) {
								if (EditorUtility.DisplayDialog ("Alert", "Really want to refresh all assetbundles!", "Okey", "cancel")) {
									EditorApplication.delayCall += onRefreshAllAssetbundles;
								}
							}
							GUILayout.Space (10);
							GUI.color = Color.white;
							GUI.contentColor = Color.cyan;
							state4 = NGUIEditorTools.DrawHeader ("Upgrade Publish");
							GUI.contentColor = Color.white;
							if (state4) {
								GUILayout.Space (10);
								using (new SwitchColor (Color.red)) {
									if (GUILayout.Button ("Update & Publish All AssetBundles\n(每次更新执行)", GUILayout.Width (300))) {
										if (EditorUtility.DisplayDialog ("Alert", "Really want to Refresh & Publish all assetbundles!", "Okey", "cancel")) {
											if (EditorUtility.DisplayDialog ("Alert", "OKay, let me confirm again:)\n Really want to Refresh & Publish all assetbundles!", "Do it now!", "cancel")) {
												EditorApplication.delayCall += upgrade4Publish;
											}
										}
									}
								}

								GUILayout.Space (10);
								using (new SwitchColor (Color.yellow)) {
									if (GUILayout.Button ("Show Upgrade Res Package\n(需要手工处理)", GUILayout.Width (300))) {
										ECLUpgradeListProc.show4UpgradeList (ver4UpgradeList);
									}
								}

								if (CLCfgBase.self.hotUpgrade4EachServer) {
									GUILayout.Space (5);
									using (new SwitchColor (Color.cyan)) {
										if (GUILayout.Button ("Server Binding Upgrade Res Package Md5\n(需要手工处理)", GUILayout.Width (300))) {
											ECLUpgradeBindingServer.show ();
										}
									}
								}

								state5 = NGUIEditorTools.DrawHeader ("Preupgrade");
								if (state5) {
									GUILayout.Space (10);
									using (new SwitchColor (Color.yellow)) {
										if (GUILayout.Button ("热更新前需要更新的列表\n(需要手工处理)", GUILayout.Width (300))) {
											EditorApplication.delayCall += onShowPreugradeFiles;
										}
									}
								}
							}
						}
					}
				}
			}

			//================================================================
			// Build Project
			//================================================================
			if (ECLProjectSetting.isProjectExit (this)) {
				GUI.color = Color.white;
				GUILayout.Space (5);
				tabTexture = state3 ? tabHide : tabShow;
				using (new FoldableBlock (ref state3, "Build Project", tabTexture)) {
					if (state3) {
						string rootPath = Path.Combine (Application.streamingAssetsPath, data.name);
						using (new HighlightBox ()) {
							GUI.color = Color.yellow;
							//========================================================
							// step 1
							//========================================================
							GUILayout.Label ("Step.1");
							if (GUILayout.Button ("Select Specific Config\n(选择特殊配置)", GUILayout.Width (250))) {
								string path = PStr.b ().a (Application.dataPath).a ("/CoolapeFrameData/verControl/").a (CLPathCfg.self.platform).e ();
								sepecificPublishConfig = EditorUtility.OpenFilePanel ("Select Specific Config", path, "v");
							}

							GUI.color = Color.red;
							if (!string.IsNullOrEmpty (sepecificPublishConfig)) {
								sepecificPublishConfig = GUILayout.TextField (sepecificPublishConfig);
							}

							GUI.color = Color.yellow;
							GUILayout.BeginHorizontal ();
							{
								if (GUILayout.Button ("PubshlishSetting\n(打包配置[每次打包前先执行一次])", GUILayout.Width (250))) {
									if (EditorUtility.DisplayDialog ("Alert", "打包配置（每次打包前先执行一次）!", "Okey", "cancel")) {
										publishSetting (sepecificPublishConfig);
									}
								}

								isDone4BuildStep (Path.Combine (rootPath, "resVer"), 10);
								GUI.color = Color.yellow;
							}
							GUILayout.EndHorizontal ();

							//========================================================
							// step 2
							//========================================================
							GUILayout.Space (5);
							GUILayout.Label ("Step.2");
							GUI.color = Color.red;
							GUILayout.Toggle (CLCfgBase.self.isEncodeLua, "Use encoded Lua (setting in the Cfg object) ");
							GUILayout.Toggle (data.isLuaPackaged, "Package Lua  to【priority.r】");
			
							GUI.color = Color.yellow;

							GUILayout.BeginHorizontal ();
							{
								if (GUILayout.Button ("Move Priority Files to StreamingAssets", GUILayout.Width (250))) {
									CreateStreamingAssets ();
								}
								isDone4BuildStep (Path.Combine (Application.streamingAssetsPath, "priority.r"), 3);
								GUI.color = Color.yellow;
							}
							GUILayout.EndHorizontal ();

							GUILayout.Space (2);

							GUILayout.BeginHorizontal ();
							{
								if (GUILayout.Button ("Move Other Files to StreamingAssets", GUILayout.Width (250))) {
									MoveOtherToStreamingAssets ();
								}

								isDone4BuildStep (Path.Combine (rootPath, "upgradeRes/other"), 3);
								GUI.color = Color.yellow;
							}
							GUILayout.EndHorizontal ();
							//========================================================
							// step 3
							//========================================================
							GUILayout.Space (5);
							GUILayout.Label ("Step.3");
							if (GUILayout.Button ("Show Publish Tool Dialog", GUILayout.Width (300))) {
								EditorWindow.GetWindow<ECLPublisher> (false, "CoolapePublisher", true);
							}
							GUI.color = Color.white;
						}
					}
				}
			}
		}
		EditorGUILayout.EndScrollView ();
	}

	void isDone4BuildStep (string path, float offset)
	{
		bool done = Directory.Exists (path);
		done = done ? done : File.Exists (path);
		if (done) {
			GUI.color = Color.green;
		} else { 
			GUI.color = Color.yellow;
		}
		GUILayout.BeginVertical ();
		{
			GUI.enabled = false;
			GUILayout.Space (offset);
			GUILayout.Toggle (done, "Done");
			GUI.enabled = true;
		}
		GUILayout.EndVertical ();
	}

	public bool exitCfgPath ()
	{
		String cfgPath = Application.dataPath + "/" + configFile;
		return File.Exists (cfgPath);
	}

	public static void initData ()
	{
		tabHide = ECLEditorUtl.getObjectByPath (FrameName + "/Editor/png/tabHide.tga") as Texture2D;
		tabShow = ECLEditorUtl.getObjectByPath (FrameName + "/Editor/png/tabShow.tga") as Texture2D;
		String cfgPath = Application.dataPath + "/" + configFile;
		if (FileEx.FileExists (cfgPath)) {
			byte[] buffer = FileEx.ReadAllBytes (cfgPath);
			if (buffer.Length <= 0) {
				return;
			}
			MemoryStream ms = new MemoryStream ();
			ms.Write (buffer, 0, buffer.Length);
			ms.Position = 0;
			object obj = B2InputStream.readObject (ms);
			if (obj != null) {
				data = ProjectData.parse ((Hashtable)obj);
			} else {
				data = new ProjectData ();
			}
		} else {
			data = new ProjectData ();
		}
	}

	public static void saveData ()
	{
		MemoryStream ms = new MemoryStream ();
		B2OutputStream.writeObject (ms, data.ToMap ());
		String cfgPath = Application.dataPath + "/" + configFile;
		Directory.CreateDirectory (Path.GetDirectoryName (cfgPath));
		FileEx.WriteAllBytes (cfgPath, ms.ToArray ());
	}

	void CreateStreamingAssets ()
	{
		string publishVerPath = "";
		if (string.IsNullOrEmpty (sepecificPublishConfig)) {
			publishVerPath = Application.dataPath + "/" + ver4Publish;
		} else {
			publishVerPath = sepecificPublishConfig;
		}
		if (!File.Exists (publishVerPath)) {
			GUI.color = Color.red;
			EditorUtility.DisplayDialog ("失败!!!!!!!!", "请先设置Publish[PubshlishSetting]!\n失败!失败!失败!失败!失败!失败!", "失败");
			GUI.color = Color.white;
			return;
		}
		Hashtable publishCfg = fileToMap (publishVerPath);
		string streamingAssetsPackge = Application.streamingAssetsPath + "/priority.r";
		string priorityPath = Application.streamingAssetsPath + "/" + data.name + "/upgradeRes/priority/";
		if (Directory.Exists (priorityPath)) {
			Directory.Delete (priorityPath, true);
		}
		copyPriorityFiles (publishCfg);
		Hashtable outMap = new Hashtable ();
		doCreateStreamingAssets (publishCfg, ref outMap);
		
		MemoryStream ms = new MemoryStream ();
		B2OutputStream.writeMap (ms, outMap);
		File.WriteAllBytes (streamingAssetsPackge, ms.ToArray ());
		EditorUtility.DisplayDialog ("success", "Create Priority StreamingAssets cuccess!", "Okey");
	}

	void copyPriorityFiles (Hashtable resMap)
	{
		string extension = "";
		string key = "";
		byte[] buffer = null;
		string filePath = "";
		string basePath = PStr.b ().a (Application.dataPath).a ("/").a (data.name).a ("/upgradeRes4Publish/priority").e ();
		foreach (DictionaryEntry cell in resMap) {
			filePath = Application.dataPath + "/" + cell.Key;
			if (filePath.Contains (PStr.b ().a (basePath).a ("/ui/panel").e ())
			    || filePath.Contains (PStr.b ().a (basePath).a ("/ui/cell").e ())
			    || filePath.Contains (PStr.b ().a (basePath).a ("/ui/other").e ())
				|| filePath.Contains (PStr.b ().a (basePath).a ("/atlas").e ())
				|| filePath.Contains (PStr.b ().a (basePath).a ("/font").e ())
				|| filePath.Contains (PStr.b ().a (basePath).a ("/AnimationTexture").e ())
			    || filePath.Contains (PStr.b ().a (basePath).a ("/localization").e ())
			    || (!data.isLuaPackaged && filePath.Contains (PStr.b ().a (basePath).a ("/lua").e ()))) { 
				key = filter (filePath); 
				key = key.Replace ("/upgradeRes4Publish/", "/upgradeRes/");
				key = key.Replace ("/upgradeRes4Dev/", "/upgradeRes/");
				if (!CLCfgBase.self.isEncodeLua
				    && Path.GetExtension (filePath) == ".lua") {
					filePath = filePath.Replace ("/upgradeRes4Publish/", "/upgradeRes4Dev/");
				}
				string toPath = Application.streamingAssetsPath + "/" + key;
				Directory.CreateDirectory (Path.GetDirectoryName (toPath));
				File.Copy (filePath, toPath, true);
			}
		}
	}

	void doCreateStreamingAssets (Hashtable resMap, ref Hashtable map)
	{
		string extension = "";
		string key = "";
		byte[] buffer = null;
		string basePath = PStr.b ().a (Application.dataPath).a ("/").a (data.name).a ("/upgradeRes4Publish/priority").e ();
		string filePath = "";
		foreach (DictionaryEntry cell in resMap) {
			filePath = Application.dataPath + "/" + cell.Key;
			if (!filePath.Contains (basePath)) {
				continue;
			}

			if (filePath.Contains (PStr.b ().a (basePath).a ("/ui/panel").e ())
			    || filePath.Contains (PStr.b ().a (basePath).a ("/ui/cell").e ())
			    || filePath.Contains (PStr.b ().a (basePath).a ("/ui/other").e ())
			    || filePath.Contains (PStr.b ().a (basePath).a ("/atlas").e ())
				|| filePath.Contains (PStr.b ().a (basePath).a ("/font").e ())
				|| filePath.Contains (PStr.b ().a (basePath).a ("/localization").e ())
				|| filePath.Contains (PStr.b ().a (basePath).a ("/AnimationTexture").e ())
			    || (!data.isLuaPackaged && filePath.Contains (PStr.b ().a (basePath).a ("/lua").e ()))) {
				continue;
			}
			key = filter (filePath); 
			key = key.Replace ("/upgradeRes4Publish/", "/upgradeRes/");
			key = key.Replace ("/upgradeRes4Dev/", "/upgradeRes/");
			if (!CLCfgBase.self.isEncodeLua
			    && Path.GetExtension (filePath) == ".lua") {
				filePath = filePath.Replace ("/upgradeRes4Publish/", "/upgradeRes4Dev/");
			}
			Debug.Log ("filePath==" + filePath);
			buffer = File.ReadAllBytes (filePath);
			map [key] = buffer;
		}
	}

	void doCreateStreamingAssets (string path, ref Hashtable map)
	{
		string[] fileEntries = Directory.GetFiles (path);//因为Application.dataPath得到的是型如 "工程名称/Assets"
		string extension = "";
		string key = "";
		byte[] buffer = null;
		string filePath = "";
		foreach (string fileName in fileEntries) {
			filePath = fileName;
			extension = Path.GetExtension (fileName);
			if (ECLEditorUtl.isIgnoreFile (fileName)) {
				continue;
			}
			key = filter (fileName);
			key = key.Replace ("/upgradeRes4Publish/", "/upgradeRes/");
			key = key.Replace ("/upgradeRes4Dev/", "/upgradeRes/");
			if (!CLCfgBase.self.isEncodeLua
			    && Path.GetExtension (fileName) == ".lua") {
				filePath = fileName.Replace ("/upgradeRes4Publish/", "/upgradeRes4Dev/");
			}
			Debug.Log ("filePath==" + filePath);
			buffer = File.ReadAllBytes (filePath);
			map [key] = buffer;
		}
		
		string[] dirEntries = Directory.GetDirectories (path);
		foreach (string dir in dirEntries) {
            //跳过不同平台的资源
#if UNITY_ANDROID
            if (Path.GetFileName(dir).Equals("IOS") || Path.GetFileName(dir).Equals("Standalone") || Path.GetFileName(dir).Equals("StandaloneOSX"))
            {
                continue;
            }
#elif UNITY_IOS
            if(Path.GetFileName(dir).Equals("Android") || Path.GetFileName(dir).Equals("Standalone") || Path.GetFileName(dir).Equals("StandaloneOSX")) {
                continue;
            }
#elif UNITY_STANDALONE_WIN
            if(Path.GetFileName(dir).Equals("Android") || Path.GetFileName(dir).Equals("IOS") || Path.GetFileName(dir).Equals("StandaloneOSX")) {
                continue;
            }
#elif UNITY_STANDALONE_OSX
            if(Path.GetFileName(dir).Equals("Android") || Path.GetFileName(dir).Equals("IOS") || Path.GetFileName(dir).Equals("Standalone")) {
                continue;
            }
#endif
            doCreateStreamingAssets(dir, ref map);
		}
	}
	
	//过滤路径
	public string filter (string oldStr)
	{
		string basePath = Application.dataPath + "/";
		basePath = basePath.Replace ("/Assets/", "/");
		
		string[] replaces = {basePath + "StreamingAssets/",
			basePath + "Resources/",
			basePath + "Assets/"
		};
		string str = oldStr;
		string rep = "";
		for (int i = 0; i < replaces.Length; i++) {
			rep = replaces [i];
			str = str.Replace (rep, "");
		}
		return str;
	}

	void MoveOtherToStreamingAssets ()
	{
//		string publishVerPath = Application.dataPath + "/" + ver4Publish;
		string publishVerPath = "";
		if (string.IsNullOrEmpty (sepecificPublishConfig)) {
			publishVerPath = Application.dataPath + "/" + ver4Publish;
		} else {
			publishVerPath = sepecificPublishConfig;
		}

		if (!File.Exists (publishVerPath)) {
			GUI.color = Color.red;
			EditorUtility.DisplayDialog ("失败!!!!!!!!", "请先设置Publish[PubshlishSetting]!\n失败!失败!失败!失败!失败!失败!", "失败");
			GUI.color = Color.white;
			return;
		}
		Hashtable publishCfg = fileToMap (publishVerPath);
		
		string asPath = "Assets/StreamingAssets/";
		string basePath = "Assets/" + data.name + "/upgradeRes4Publish/";
		string pPath = asPath + data.name + "/upgradeRes/other/";
		if (Directory.Exists (pPath)) {
			Directory.Delete (pPath, true);
		}
		Directory.CreateDirectory (pPath);
		//		cpyDir (basePath + "other/", pPath);
		
		string filePath = "";
		string toPath = "";
		foreach (DictionaryEntry cell in publishCfg) {
			filePath = Application.dataPath + "/" + cell.Key;
			if (!filePath.Contains (Application.dataPath + "/" + data.name + "/upgradeRes4Publish/other/")) {
				continue;
			}
			toPath = Application.dataPath + "/StreamingAssets/" + cell.Key.ToString ().Replace ("/upgradeRes4Publish/", "/upgradeRes/");
			
			Directory.CreateDirectory (Path.GetDirectoryName (toPath));
			File.Copy (filePath, toPath);
		}
		
		EditorUtility.DisplayDialog ("success", "Move Others to StreamingAssets cuccess!", "Okey");
	}

	void cpyDir (string path, string toPath)
	{
		string[] fileEntries = Directory.GetFiles (path);
		string f = "";
		string extension = "";
		Directory.CreateDirectory (toPath);
		for (int i = 0; i < fileEntries.Length; i++) {
			f = fileEntries [i];
			extension = Path.GetExtension (f);
			if (ECLEditorUtl.isIgnoreFile (f)) {
				continue;
			}
			Debug.Log (f + "          " + toPath + Path.GetFileName (f));
			File.Copy (f, toPath + Path.GetFileName (f));
		}
		
		string[] dirEntries = Directory.GetDirectories (path);
		foreach (string dir in dirEntries) {
			//跳过不同平台的资源
#if UNITY_ANDROID
            if (Path.GetFileName(dir).Equals("IOS") || Path.GetFileName(dir).Equals("Standalone") || Path.GetFileName(dir).Equals("StandaloneOSX"))
            {
                continue;
            }
#elif UNITY_IOS
            if(Path.GetFileName(dir).Equals("Android") || Path.GetFileName(dir).Equals("Standalone") || Path.GetFileName(dir).Equals("StandaloneOSX")) {
                continue;
            }
#elif UNITY_STANDALONE_WIN
            if(Path.GetFileName(dir).Equals("Android") || Path.GetFileName(dir).Equals("IOS") || Path.GetFileName(dir).Equals("StandaloneOSX")) {
                continue;
            }
#elif UNITY_STANDALONE_OSX
            if(Path.GetFileName(dir).Equals("Android") || Path.GetFileName(dir).Equals("IOS") || Path.GetFileName(dir).Equals("Standalone")) {
                continue;
            }
#endif
            cpyDir(dir, toPath + Path.GetFileName (dir) + "/");
		}
	}

	string createCfgBioDataFromJson (string className, string jsonPath)
	{
		Debug.Log (jsonPath);
		ArrayList list = JSON.DecodeList (File.ReadAllText (Application.dataPath + "/" + jsonPath));
		if (list == null) {
			Debug.LogError ("Json decode error==" + jsonPath);
			return "";
		}
		string outVerFile = getCfgBioDataPath (className);
		Directory.CreateDirectory (Path.GetDirectoryName (outVerFile));
		ArrayList _list = null;
		for (int i = 1; i < list.Count; i++) {
			_list = (ArrayList)(list [i]);
			for (int j = 0; j < _list.Count; j++) {
				if (_list [j] is System.Double) {
					_list [j] = NumEx.int2Bio (NumEx.stringToInt (_list [j].ToString ()));
				}
			}
			list [i] = _list;
		}
		MemoryStream ms = new MemoryStream ();
		B2OutputStream.writeObject (ms, list);
		File.WriteAllBytes (outVerFile, ms.ToArray ());
		return outVerFile;
	}

	string getCfgBioDataPath (string className)
	{
		string outVerFile = "Assets/" + data.name + "/upgradeRes4Publish/priority/cfg/" + className + ".cfg";
		return outVerFile;
	}

	/// <summary>
	/// 取得最后一次更新后的版本信息
	/// </summary>
	/// <returns>The last upgrade ver.</returns>
	public static Hashtable getLastUpgradeVer ()
	{
		string path = Application.dataPath + "/" + ver4Upgrade;
		return fileToMap (path);
	}

	public static Hashtable getLastUpgradeMd5Ver ()
	{
		string path = Application.dataPath + "/" + ver4UpgradeMd5;
		return fileToMap (path);
	}

	public static Hashtable fileToMap (string path)
	{
		Hashtable r = new Hashtable ();
		if (!File.Exists (path)) {
			return r;
		}
		string[] content = File.ReadAllLines (path);
		int count = content.Length;
		string str = "";
		for (int i = 0; i < count; i++) {
			str = content [i];
			if (str.StartsWith ("#"))
				continue;
			string[] strs = str.Split (',');
			if (strs.Length > 1) {
				r [strs [0]] = strs [1];
			}
		}
		return r;
	}

	public static void saveOtherUIAsset (GameObject go)
	{
		CLCellLua cell = go.GetComponent<CLCellLua> ();
		if (cell != null) {
			if (cell.isNeedResetAtlase) {
				CLUIUtl.resetAtlasAndFont (cell.transform, true);
			}
		} else {
			//默认需要重新设置
			CLUIUtl.resetAtlasAndFont (go.transform, true);
		}
		string dir = Application.dataPath + "/" + ECLEditorUtl.getPathByObject (go);
		dir = Path.GetDirectoryName (dir);
		ECLCreatAssetBundle4Update.createAssets4Upgrade (dir, go, true);

		// 必须再取一次，好像执行了上面一句方法后，cell就会变成null
		cell = go.GetComponent<CLCellLua> ();
		if (cell != null) {
			if (cell.isNeedResetAtlase) {
				CLUIUtl.resetAtlasAndFont (cell.transform, false);
			}
		} else {
			//默认需要重新设置
			CLUIUtl.resetAtlasAndFont (go.transform, false);
		}
	}

	//判断是否有修改过文件
	public static bool isModified (string file)
	{
		FileInfo fi = new FileInfo (file);
		string last = MapEx.getString (resModifyDateMap, file);
		string curr = fi.LastWriteTime.ToFileTime ().ToString ();
		if (curr.Equals (last)) {
			return false;
		} else {
			return true;
		}
	}

	public static void saveResModifyDate ()
	{
		resModifyDateMap = null;
		string dir = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/other";
		PStr ps = PStr.b ();
		dosaveResModifyDate (dir, ref ps);
		dir = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority";
		dosaveResModifyDate (dir, ref ps);
		if (!String.IsNullOrEmpty (data.cfgFolderStr)) {
			string basePath = Application.dataPath + "/";
			basePath = basePath.Replace ("/Assets/", "/");
			dir = basePath + data.cfgFolderStr;
			dosaveResModifyDate (dir, ref ps);
		}
		string strs = ps.e ();
		File.WriteAllText (Application.dataPath + "/" + resModifyDateCfg, strs);
	}

	static void dosaveResModifyDate (string dir, ref PStr ps)
	{
		string[] fileEntries = Directory.GetFiles (dir);
		FileInfo fi = null;
		foreach (string fileName in fileEntries) {
			fi = new FileInfo (fileName);
			ps.a (fileName).a (",").a (fi.LastWriteTime.ToFileTime ()).a ("\n");
		}

		string[] dirEntries = Directory.GetDirectories (dir);
		foreach (string path in dirEntries) {
			dosaveResModifyDate (path, ref ps);
		}
	}

	/// <summary>
	/// Collects the assets.重新收集资源
	/// </summary>
	/// <returns><c>true</c>, if assets was collected, <c>false</c> otherwise.</returns>
	public bool collectAssets ()
	{
		bool ret = false;
		Hashtable tmpOtherVer = ECLCreateVerCfg.create2Map ("Assets/" + data.name + "/upgradeRes4Dev/other");
		Hashtable tmpPriorityVer = ECLCreateVerCfg.create2Map ("Assets/" + data.name + "/upgradeRes4Dev/priority");

		string file = "";
		UnityEngine.Object obj = null;
		foreach (DictionaryEntry cell in tmpOtherVer) {
			file = cell.Key.ToString ();
//			if (!callCustomSkipCollectRes (file)) {

			if (!file.Contains ("/other/model/")) {
				if (isModified (Application.dataPath + "/" + file)) {
					obj = ECLEditorUtl.getObjectByPath (file);
					ret = doCollectAssets (obj) || ret ? true : false;
				}
			}
//			}
		}

		foreach (DictionaryEntry cell in tmpPriorityVer) {
			file = cell.Key.ToString ();
//			if (!callCustomSkipCollectRes (file)) {
			if (file.Contains ("/priority/ui/")) {
				if (isModified (Application.dataPath + "/" + file)) {
					obj = ECLEditorUtl.getObjectByPath (file);
					ret = doCollectAssets (obj) || ret ? true : false;
				}
			}
//			}
		}
		return ret;
	}

	public bool doCollectAssets (UnityEngine.Object obj)
	{
		bool ret = false;
		if (obj != null) {
			if (obj is GameObject) {
                obj = PrefabUtility.LoadPrefabContents(AssetDatabase.GetAssetPath(obj));
				CLSharedAssets sharedAsset = ((GameObject)obj).GetComponent<CLSharedAssets> ();
				if (sharedAsset == null && CLSharedAssetsInspector.isCanAddSharedAssetProc ((GameObject)obj)) {
					sharedAsset = ((GameObject)obj).AddComponent<CLSharedAssets> ();

					ret = CLSharedAssetsInspector.getAssets (sharedAsset, sharedAsset.transform) || ret ? true : false;
					if (sharedAsset.isEmpty ()) {
						DestroyImmediate (sharedAsset, true);
					}
					EditorUtility.SetDirty (obj);
				}
//				if (sharedAsset != null) {
//					ret = CLSharedAssetsInspector.getAssets (sharedAsset, sharedAsset.transform) || ret ? true : false;
//					if (sharedAsset.isEmpty ()) {
//						DestroyImmediate (sharedAsset, true);
//					}
//					EditorUtility.SetDirty (obj);
//				}
			} else if (obj is Material) {
				ArrayList propNames = new ArrayList ();
				ArrayList texNames = new ArrayList ();
				ArrayList texPaths = new ArrayList ();
				ret = ECLEditorUtl.getTexturesFromMaterial ((Material)obj, ref propNames, ref texNames, ref texPaths) || ret ? true : false;
				CLSharedAssetsInspector.saveMaterialTexCfg (ECLEditorUtl.getAssetName4Upgrade (obj), propNames, texNames, texPaths);
			}
		}
		return ret;
	}

	static IEnumerable<MethodInfo> customPushlishList = (from type in XLua.Utils.GetAllTypes ()
	                                                     from method in type.GetMethods (BindingFlags.Static | BindingFlags.Public)
	                                                     where method.IsDefined (typeof(PublishResAttribute), false)
	                                                     select method);

	static bool callCustomPublish (string path)
	{
		bool retVal = true;
		foreach (var method in customPushlishList) {
			object ret = method.Invoke (null, new object[] { path });
			retVal = ((bool)ret == false || retVal == false) ? false : true;
		}
		return retVal;
	}

	static IEnumerable<MethodInfo> skipCollectResMethodList = (from type in XLua.Utils.GetAllTypes ()
	                                                           from method in type.GetMethods (BindingFlags.Static | BindingFlags.Public)
	                                                           where method.IsDefined (typeof(SkipCollectResAttribute), false)
	                                                           select method);

	static bool callCustomSkipCollectRes (string path)
	{
		bool retVal = false;
		foreach (var method in skipCollectResMethodList) {
			object ret = method.Invoke (null, new object[] { path });
			retVal = (bool)ret;
		}
		return retVal;
	}

	/// <summary>
	/// Refreshs all assetbundles. 根据md5来刷新资源
	/// </summary>
	/// <returns>The all assetbundles.</returns>

	public void onRefreshAllAssetbundles ()
	{
		refreshAllAssetbundles ();
	}

	/// <summary>
	/// Refreshs all assetbundles. 根据md5来刷新资源
	/// </summary>
	/// <returns>The all assetbundles.</returns>

	public string refreshAllAssetbundles ()
	{
		AssetDatabase.Refresh (ImportAssetOptions.ForceUpdate);

		if (collectAssets ()) {
			if (EditorUtility.DisplayDialog ("Alert", "There are some new assets have moved to [upgradeRes4Dev],  do you want refresh all assets now?", "Do it now", "Cancel")) {
				refreshAllAssetbundles ();
			}
			return "";
		}
		AssetDatabase.Refresh ();

		// get current ver
		Hashtable tmpOtherVer = ECLCreateVerCfg.create2Map ("Assets/" + data.name + "/upgradeRes4Dev/other");
		Hashtable tmpPriorityVer = ECLCreateVerCfg.create2Map ("Assets/" + data.name + "/upgradeRes4Dev/priority");
		Hashtable tmpCfgdataVer = null;
		if (!String.IsNullOrEmpty (data.cfgFolderStr)) {
			tmpCfgdataVer = ECLCreateVerCfg.create2Map (data.cfgFolderStr);
		}
		tmpCfgdataVer = tmpCfgdataVer == null ? new Hashtable () : tmpCfgdataVer;

		// get last time ver
		string lastVerPath = Application.dataPath + "/" + ver4DevelopeMd5;
		Hashtable lastVerMap = null;
		if (File.Exists (lastVerPath)) {
			lastVerMap = Utl.fileToMap (lastVerPath);
		}
		lastVerMap = lastVerMap == null ? new Hashtable () : lastVerMap;

		Hashtable lastOtherVer = null;
		Hashtable lastPriorityVer = null;
		Hashtable lastCfgdataVer = null;
		lastOtherVer = MapEx.getMap (lastVerMap, "other");
		lastPriorityVer = MapEx.getMap (lastVerMap, "priority");
		lastCfgdataVer = MapEx.getMap (lastVerMap, "cfgData");

		// refresh other assetbundle
		PStr resultPstr = PStr.b ();
		string result = "";
		string path = "";
		string md5str = "";
		//==============================================
		// refresh other files
		//==============================================
		foreach (DictionaryEntry cell in tmpOtherVer) {
			path = cell.Key.ToString ();
			md5str = cell.Value.ToString ();
			if (lastOtherVer == null || lastOtherVer [path] == null || lastOtherVer [path].ToString () != md5str) {
				//do refresh asset
				Debug.Log (path);
				resultPstr.a (path).a ("\n");
				if (callCustomPublish (path)) {
					ECLCreatAssetBundle4Update.createAssets4Upgrade (PStr.b ().a ("Assets/").a (path).e ());
				}
			}
		}
		lastVerMap ["other"] = tmpOtherVer;

		//==============================================
		// refresh priority assetsbutndle
		//==============================================
		foreach (DictionaryEntry cell in tmpPriorityVer) {
			path = cell.Key.ToString ();
			md5str = cell.Value.ToString ();
			if (ECLEditorUtl.isIgnoreFile (path)) {
				continue;
			}
			if (path.Contains ("/priority/atlas/") ||
			    path.Contains ("/priority/font/")) {
				//1. atlas
				if (lastPriorityVer == null || lastPriorityVer [path] == null || lastPriorityVer [path].ToString () != md5str) {
					//do refresh asset
					Debug.Log ("Assets/" + path);
					resultPstr.a (path).a ("\n");
					if (callCustomPublish (path)) {
						ECLCreatAssetBundle4Update.createAssets4Upgrade ("Assets/" + path, true);
					}
				}
			} else if (path.Contains ("/priority/AnimationTexture/")) {
				if (lastPriorityVer == null || lastPriorityVer [path] == null || lastPriorityVer [path].ToString () != md5str) {
					//do refresh asset
					Debug.Log ("Assets/" + path);
					resultPstr.a (path).a ("\n");
					if (callCustomPublish (path)) {
						ECLCreatAssetBundle4Update.createAssets4Upgrade ("Assets/" + path);
					}
				}
			} else if (path.Contains ("/priority/lua/")) {
				// encode lua
				if (lastPriorityVer == null || lastPriorityVer [path] == null || lastPriorityVer [path].ToString () != md5str) {
					resultPstr.a (path).a ("\n");
					if (callCustomPublish (path)) {
						ECLLuaEncodeTool.xxteaEncode ("Assets/" + path);
					}
				}
			} else if (path.Contains ("/priority/ui/cell/") ||
			           path.Contains ("/priority/ui/other/")) {
				// refresh ui cell
				if (lastPriorityVer == null || lastPriorityVer [path] == null || lastPriorityVer [path].ToString () != md5str) {
                    GameObject t = AssetDatabase.LoadAssetAtPath("Assets/" + path, typeof(GameObject)) as GameObject;
                    CLCellLua uicell = t.GetComponent<CLCellLua> ();
					resultPstr.a (path).a ("\n");

					if (callCustomPublish (path)) {
						saveOtherUIAsset (t);
					}
				}
			} else if (path.Contains ("/priority/ui/panel/")) {
				// refresh panel
				if (lastPriorityVer == null || lastPriorityVer [path] == null || lastPriorityVer [path].ToString () != md5str) {
					GameObject t = AssetDatabase.LoadAssetAtPath("Assets/" + path, typeof(GameObject)) as GameObject;
					CLPanelBase panel = t.GetComponent<CLPanelBase> ();
					if (panel != null) {
						resultPstr.a (path).a ("\n");

						if (callCustomPublish (path)) {
							CLPanelLuaInspector.doSaveAsset (t);
						}
					} else {
						Debug.LogError (string.Format ("The object can not get the [CLPanelLua]={0}!", path));
					}
				}
			} else {
				if (lastPriorityVer == null || lastPriorityVer [path] == null || lastPriorityVer [path].ToString () != md5str) {
					resultPstr.a (path).a ("\n");
					string cpyPath = path.Replace ("/upgradeRes4Dev", "/upgradeRes4Publish");
					cpyPath = Application.dataPath + "/" + cpyPath;

					if (callCustomPublish (path)) {
						Directory.CreateDirectory (Path.GetDirectoryName (cpyPath));
						File.Copy (Application.dataPath + "/" + path, cpyPath, true);
					}
				}
			}
		}
		lastVerMap ["priority"] = tmpPriorityVer;

		//==============================================
		// refresh cfg data
		//==============================================
		foreach (DictionaryEntry cell in tmpCfgdataVer) {
			path = cell.Key.ToString ();
			md5str = cell.Value.ToString ();
			if (lastCfgdataVer == null || lastCfgdataVer [path] == null || lastCfgdataVer [path].ToString () != md5str) {
				if (callCustomPublish (path)) {
					string className = Path.GetFileNameWithoutExtension (path);
					resultPstr.a (createCfgBioDataFromJson (className, path)).a ("\n");
				}
			}
		}
		lastVerMap ["cfgData"] = tmpCfgdataVer;

		result = resultPstr.e ();

		Debug.Log ("result==" + result);
		ECLGUIMsgBox.show ("Refresh success", result == "" ? "Nothing need refresh!" : result, null);
		//==============================================
		// refresh end, save version file
		//==============================================
		Directory.CreateDirectory (Path.GetDirectoryName (lastVerPath));
		MemoryStream ms = new MemoryStream ();
		B2OutputStream.writeObject (ms, lastVerMap);
		FileEx.WriteAllBytes (lastVerPath, ms.ToArray ());
		if (result.Length > 0) {
			AssetDatabase.Refresh ();
		}
		saveResModifyDate ();
		return "";
	}

	/// <summary>
	/// Publishs the setting. 打包时设置
	/// </summary>
	public void publishSetting (string _sepecificPublishConfig)
	{
		string path = PStr.b ().a (Application.dataPath).a ("/").a (data.name).a ("/upgradeRes4Publish").e ();
		ECLGUIResList.show4PublishSeting (path, _sepecificPublishConfig, (Callback)onGetFiles4PublishSetting, null);
	}

	void onGetFiles4PublishSetting (params object[] args)
	{
		ArrayList files = (ArrayList)(args [0]);
		int count = files.Count;
		ECLResInfor ri = null;
		string path = "";
		if (string.IsNullOrEmpty (sepecificPublishConfig)) {
			path = Application.dataPath + "/" + ver4Publish;
		} else {
			path = sepecificPublishConfig;
		}
		string upgradeVerPath = Application.dataPath + "/" + ver4Upgrade;
		string upgradeVerPathMd5 = Application.dataPath + "/" + ver4UpgradeMd5;
		string content = "";
		string content2 = "";
		Hashtable content3 = new Hashtable ();
		Hashtable content4 = new Hashtable ();
		string md5VerStr = "";
		PStr ps = PStr.b (content);
		PStr ps2 = PStr.b (content2);
		PStr ps3 = PStr.b (md5VerStr);
		
		bool needCreateMd5 = false;
		if (!File.Exists (upgradeVerPath)) {
			needCreateMd5 = true;
		}
		for (int i = 0; i < count; i++) {
			ri = (ECLResInfor)(files [i]);
			ps.a (ri.relativePath).a (",").a (ri.ver).a ("\n");
			if (needCreateMd5) {
				ps2.a (ri.publishPath).a (",").a (ri.ver).a ("\n");
				ps3.a (ri.relativePath).a (",").a (Utl.MD5Encrypt (File.ReadAllBytes (ri.path))).a ("\n");
			}
			if (ri.path.Contains ("/priority/")) {
				content3 [ri.publishPath] = ri.ver;
			} else {
				content4 [ri.publishPath] = ri.ver;
			}
		}
		// save publish cfg data
		File.WriteAllText (path, ps.e ());

		//if upgrade cfg file not exist, save it
		if (!File.Exists (upgradeVerPath)) {
			File.WriteAllText (upgradeVerPath, ps2.e ());
			// create md5version
			File.WriteAllText (upgradeVerPathMd5, ps3.e ());
		}
		
		// create version file
		string mVerverPath = PStr.begin ().a (CLPathCfg.self.basePath).a ("/").a (CLVerManager.resVer).a ("/").a (CLPathCfg.self.platform).a ("/").a (CLVerManager.fverVer).end ();
		string mVerPrioriPath = PStr.begin ().a (CLPathCfg.self.basePath).a ("/").a (CLVerManager.resVer).a ("/").a (CLPathCfg.self.platform).a ("/").a (CLVerManager.versPath).a ("/").a (CLVerManager.verPriority).end ();
		string mVerOtherPath = PStr.begin ().a (CLPathCfg.self.basePath).a ("/").a (CLVerManager.resVer).a ("/").a (CLPathCfg.self.platform).a ("/").a (CLVerManager.versPath).a ("/").a (CLVerManager.verOthers).end ();

		//----------------------------------------------------------------------------------------
		// save VerPrioriPath
		//----------------------------------------------------------------------------------------
		string tmpPath = Application.dataPath + "/StreamingAssets/" + mVerPrioriPath;
		Directory.CreateDirectory (Path.GetDirectoryName (tmpPath));
		ECLCreateVerCfg.saveMap (content3, tmpPath);
		string md5VerPriori = Utl.MD5Encrypt (File.ReadAllBytes (tmpPath));
		tmpPath = Application.dataPath + "/" + CLPathCfg.self.basePath + "/upgradeRes4Ver/" + mVerPrioriPath;
		if (!File.Exists (tmpPath)) {
			Directory.CreateDirectory (Path.GetDirectoryName (tmpPath));
			ECLCreateVerCfg.saveMap (content3, tmpPath);
		}
		//----------------------------------------------------------------------------------------
		// save VerOtherPath
		//----------------------------------------------------------------------------------------
		tmpPath = Application.dataPath + "/StreamingAssets/" + mVerOtherPath;
		ECLCreateVerCfg.saveMap (content4, tmpPath);
		string md5VerOther = Utl.MD5Encrypt (File.ReadAllBytes (tmpPath));
		tmpPath = Application.dataPath + "/" + CLPathCfg.self.basePath + "/upgradeRes4Ver/" + mVerOtherPath;
		if (!File.Exists (tmpPath)) {
			Directory.CreateDirectory (Path.GetDirectoryName (tmpPath));
			ECLCreateVerCfg.saveMap (content4, tmpPath);
		}
		//----------------------------------------------------------------------------------------
		// save VerverPath
		//----------------------------------------------------------------------------------------
		Hashtable verVerMap = new Hashtable ();// Utl.fileToMap(Application.dataPath + "/" + CLPathCfg.self.basePath + "/upgradeRes4Publish/" + mVerverPath);
		verVerMap [mVerPrioriPath] = md5VerPriori;
		verVerMap [mVerOtherPath] = md5VerOther;
		tmpPath = Application.dataPath + "/" + CLPathCfg.self.basePath + "/upgradeRes4Ver/" + mVerverPath;
		if (!File.Exists (tmpPath)) {
			Directory.CreateDirectory (Path.GetDirectoryName (tmpPath));
			ECLCreateVerCfg.saveMap (verVerMap, tmpPath);
		} 
//			else {
//			Hashtable m = Utl.fileToMap (tmpPath);
//			verVerMap [mVerPrioriPath] = MapEx.getString (m, mVerPrioriPath);
//			verVerMap [mVerOtherPath] = MapEx.getString (m, mVerOtherPath);
//		}

		tmpPath = Application.dataPath + "/StreamingAssets/" + mVerverPath;
		Directory.CreateDirectory (Path.GetDirectoryName (tmpPath));
		ECLCreateVerCfg.saveMap (verVerMap, tmpPath);

		EditorUtility.DisplayDialog ("success", "Publish Version File Created!", "Okay");
	}
	
	// 更新前的准备工作
	public void onShowPreugradeFiles ()
	{
		string path = PStr.b ().a (Application.dataPath).a ("/").a (data.name).a ("/upgradeRes4Publish").e ();
		ECLGUIResList.show (path, (Callback)onGetFiles4Preupgrade, null);
	}

	void onGetFiles4Preupgrade (params object[] args)
	{
		ArrayList list = (ArrayList)(args [0]);
		if (list.Count == 0)
			return;

		int count = list.Count;
		string verVal = "";
		ECLResInfor ri = null;
		ArrayList preupgradeList = new ArrayList ();
		for (int i = 0; i < count; i++) {
			ri = (ECLResInfor)(list [i]);
			ArrayList cell = new ArrayList ();
			verVal = Utl.MD5Encrypt (File.ReadAllBytes (ri.path));
			cell.Add (ri.publishPath);
			cell.Add (verVal);
			preupgradeList.Add (cell);
		}
		// 热更新的资源包目录
		string newUpgradeDir = DateEx.format (DateEx.fmt_yyyy_MM_dd_HH_mm_ss_fname);
		string toPathBase = (Application.dataPath + "/").Replace ("/Assets/", "/Assets4PreUpgrade/" + newUpgradeDir + "/");
		Debug.Log (toPathBase);
		string toPath = toPathBase;
		if (Directory.Exists (toPath)) {
			Directory.Delete (toPath, true);
		}
		Directory.CreateDirectory (Path.GetDirectoryName (toPath));

		string path = "";
		path = Application.streamingAssetsPath + "/upgraderVer.json";
		string strs = File.ReadAllText (path);
		Hashtable m = JSON.DecodeMap (strs);
		int verPre = MapEx.getInt (m, "upgraderVer") + 1;
		m ["upgraderVer"] = verPre;
		File.WriteAllText (path, JSON.JsonEncode (m));
		File.Copy (path, toPath + "upgraderVer.json");

		string jsonStr = JSON.JsonEncode (preupgradeList);
		Debug.Log (jsonStr);
		path = toPath + preUpgradeListName + "." + verPre;
		File.WriteAllText (path, jsonStr);
		EditorUtility.DisplayDialog ("success", "success!", "Okay");
	}

	/// <summary>
	/// Upgrade4s the publish. 热更新用
	/// </summary>
	public void upgrade4Publish ()
	{
		string path = PStr.b ().a (Application.dataPath).a ("/").a (data.name).a ("/upgradeRes4Publish").e ();
		ECLGUIResList.show4Upgrade (path, (Callback)onGetFiles4Upgrade, null);
	}

	void onGetFiles4Upgrade (params object[] args)
	{
		ArrayList list = (ArrayList)(args [0]);
		if (list.Count == 0)
			return;
		
		string mVerverPath = PStr.begin ().a (CLPathCfg.self.basePath).a ("/").a (CLVerManager.resVer).a ("/").a (CLPathCfg.self.platform).a ("/").a (CLVerManager.fverVer).end ();
		string mVerPrioriPath = PStr.begin ().a (CLPathCfg.self.basePath).a ("/").a (CLVerManager.resVer).a ("/").a (CLPathCfg.self.platform).a ("/").a (CLVerManager.versPath).a ("/").a (CLVerManager.verPriority).end ();
		string mVerOtherPath = PStr.begin ().a (CLPathCfg.self.basePath).a ("/").a (CLVerManager.resVer).a ("/").a (CLPathCfg.self.platform).a ("/").a (CLVerManager.versPath).a ("/").a (CLVerManager.verOthers).end ();
	
		// get current 
		string tmpPath = PStr.b ().a (Application.dataPath).a ("/").a (CLPathCfg.self.basePath).a ("/upgradeRes4Ver/").a (mVerPrioriPath).e ();
		Hashtable verPrioriMap = Utl.fileToMap (tmpPath);
		if (verPrioriMap == null) {
			verPrioriMap = new Hashtable ();
		}
		
		tmpPath = PStr.b ().a (Application.dataPath).a ("/").a (CLPathCfg.self.basePath).a ("/upgradeRes4Ver/").a (mVerOtherPath).e ();
		Hashtable verOtherMap = Utl.fileToMap (tmpPath);
		if (verOtherMap == null) {
			verOtherMap = new Hashtable ();
		}
		
		Hashtable verLastUpgradeMap = fileToMap (Application.dataPath + "/" + ver4Upgrade);
		verLastUpgradeMap = verLastUpgradeMap == null ? new Hashtable () : verLastUpgradeMap;
		
		bool isNeedUpgradeOther = false;
		bool isNeedUpgradePriori = false;
		ECLResInfor ri = null;
		// 热更新的资源包目录
		string newUpgradeDir = DateEx.format (DateEx.fmt_yyyy_MM_dd_HH_mm_ss_fname);
		string toPathBase = (Application.dataPath + "/").Replace ("/Assets/", "/Assets4Upgrade/" + newUpgradeDir + "/");
		string toPath = toPathBase;
		if (Directory.Exists (toPath)) {
			Directory.Delete (toPath, true);
		}
		int count = list.Count;
		string verVal = "";
		for (int i = 0; i < count; i++) {
			ri = (ECLResInfor)(list [i]);
//			verVal = NumEx.stringToInt (MapEx.getString (verLastUpgradeMap, ri.publishPath)) + 1;
			verVal = Utl.MD5Encrypt (File.ReadAllBytes (ri.path));
			verLastUpgradeMap [ri.publishPath] = verVal;
			
			//要更新的文件后面加一个版本号，这样使得后面做更新时可以使用cdn
			toPath = toPathBase + ri.publishPath;
			if (!string.IsNullOrEmpty (verVal)) {
				toPath = toPath + "." + verVal;
			}
			Directory.CreateDirectory (Path.GetDirectoryName (toPath));
			if (toPath.Contains ("/priority/lua/") && !CLCfgBase.self.isEncodeLua) {
				File.Copy (ri.path.Replace ("/upgradeRes4Publish/", "/upgradeRes4Dev/"), toPath);
			} else {
				File.Copy (ri.path, toPath);
			}
			
			if (ri.relativePath.Contains ("/priority/")) {
				isNeedUpgradePriori = true;
				verPrioriMap [ri.publishPath] = verVal;
			} else {
				isNeedUpgradeOther = true;
				verOtherMap [ri.publishPath] = verVal;
			}
		}

		//------------------------------------------------------------------------------
		// 清除已经被删掉的资源文件
		//------------------------------------------------------------------------------
		List<string> deletKeys = new List<string> ();
		foreach (DictionaryEntry cell in verPrioriMap) {
			tmpPath = Application.dataPath + "/" + cell.Key.ToString ();
			tmpPath = tmpPath.Replace ("/upgradeRes/", "/upgradeRes4Publish/");
			if (!File.Exists (tmpPath)) {
				Debug.LogError (tmpPath + "is not exists!!!!!!");
				deletKeys.Add (cell.Key.ToString ());
			}
		}
		for (int i = 0; i < deletKeys.Count; i++) {
			verPrioriMap.Remove (deletKeys [i]);
		}
		//-------------------------------------------------------------
		deletKeys.Clear ();
		foreach (DictionaryEntry cell in verOtherMap) {
			tmpPath = Application.dataPath + "/" + cell.Key.ToString ();
			tmpPath = tmpPath.Replace ("/upgradeRes/", "/upgradeRes4Publish/");
			if (!File.Exists (tmpPath)) {
				Debug.LogError (tmpPath + "is not exists!!!!!!");
				deletKeys.Add (cell.Key.ToString ());
			}
		}
		for (int i = 0; i < deletKeys.Count; i++) {
			verOtherMap.Remove (deletKeys [i]);
		}
		//------------------------------------------------------------------------------
		// 保存VerPrioriPath
		//------------------------------------------------------------------------------
		tmpPath = PStr.b ().a (Application.dataPath).a ("/").a (CLPathCfg.self.basePath).a ("/upgradeRes4Ver/").a (mVerPrioriPath).e ();	
		Directory.CreateDirectory (Path.GetDirectoryName (tmpPath));
		ECLCreateVerCfg.saveMap (verPrioriMap, tmpPath);
		string md5VerPriori = Utl.MD5Encrypt (File.ReadAllBytes (tmpPath));
		tmpPath = toPathBase + mVerPrioriPath + "." + md5VerPriori; //加上版本号，就可以用cdn了
		Directory.CreateDirectory (Path.GetDirectoryName (tmpPath));
		ECLCreateVerCfg.saveMap (verPrioriMap, tmpPath);

		//------------------------------------------------------------------------------
		// 保存VerOtherPath
		//------------------------------------------------------------------------------
		tmpPath = PStr.b ().a (Application.dataPath).a ("/").a (CLPathCfg.self.basePath).a ("/upgradeRes4Ver/").a (mVerOtherPath).e ();
		Directory.CreateDirectory (Path.GetDirectoryName (tmpPath));
		ECLCreateVerCfg.saveMap (verOtherMap, tmpPath);

		string md5VerOther = Utl.MD5Encrypt (File.ReadAllBytes (tmpPath));
		tmpPath = toPathBase + mVerOtherPath + "." + md5VerOther; //加上版本号，就可以用cdn了
		Directory.CreateDirectory (Path.GetDirectoryName (tmpPath));
		ECLCreateVerCfg.saveMap (verOtherMap, tmpPath);
		//------------------------------------------------------------------------------
		// 保存VerverPath
		//------------------------------------------------------------------------------
		tmpPath = PStr.b ().a (Application.dataPath).a ("/").a (CLPathCfg.self.basePath).a ("/upgradeRes4Ver/").a (mVerverPath).e ();
		Hashtable verVerMap = Utl.fileToMap (tmpPath);
		verVerMap = verVerMap == null ? new Hashtable () : verVerMap;
		if (isNeedUpgradePriori) {
			verVerMap [mVerPrioriPath] = md5VerPriori;
		} else {
			verVerMap [mVerPrioriPath] = MapEx.getString (verVerMap, mVerPrioriPath);
		}
		if (isNeedUpgradeOther) {
			verVerMap [mVerOtherPath] = md5VerOther;
		} else {
			verVerMap [mVerOtherPath] = MapEx.getString (verVerMap, mVerOtherPath);
		}
		Directory.CreateDirectory (Path.GetDirectoryName (tmpPath));
		ECLCreateVerCfg.saveMap (verVerMap, tmpPath);

		// 把配置文件保存到更新资源包目录
		string md5VerVer = Utl.MD5Encrypt (File.ReadAllBytes (tmpPath));
		string toPathVerverPath = "";
		if (CLCfgBase.self.hotUpgrade4EachServer) {
			toPathVerverPath = toPathBase + mVerverPath + "." + md5VerVer; //加上版本号，就可以用cdn了
		} else {
			toPathVerverPath = toPathBase + mVerverPath;
		}
		Directory.CreateDirectory (Path.GetDirectoryName (toPathVerverPath));
		File.Copy (tmpPath, toPathVerverPath);
		//------------------------------------------------------------------------------
		// 保存版本信息
		//------------------------------------------------------------------------------
		PStr pstr = PStr.b ();
		foreach (DictionaryEntry cell in verLastUpgradeMap) {
			pstr.a (cell.Key.ToString ()).a (",").a (cell.Value.ToString ()).a ("\n");
		}
		File.WriteAllText (Application.dataPath + "/" + ver4Upgrade, pstr.e ());
		
		Hashtable tmpResVer = ECLCreateVerCfg.create2Map ("Assets/" + ECLProjectManager.data.name + "/upgradeRes4Publish");
		pstr = PStr.b ();
		foreach (DictionaryEntry cell in tmpResVer) {
			pstr.a (cell.Key.ToString ().Replace ("/upgradeRes", "/upgradeRes4Publish")).a (",").a (cell.Value.ToString ()).a ("\n");
		}
		File.WriteAllText (Application.dataPath + "/" + ver4UpgradeMd5, pstr.e ());

		saveUpgradeList (newUpgradeDir, md5VerVer);
		EditorUtility.DisplayDialog ("success", "Upgrade Version File Created!", "Okay");
	}

	public void	saveUpgradeList (string newUpgradeDir, string md5)
	{
		string p = Application.dataPath + "/" + ver4UpgradeList;
		Directory.CreateDirectory (Path.GetDirectoryName (p));
		string str = "";
		if (File.Exists (p)) {
			str = File.ReadAllText (p); 
		}
		ArrayList list = JSON.DecodeList (str);
		list = list == null ? new ArrayList () : list;
		Hashtable map = new Hashtable ();
		map ["upload"] = false;
		map ["name"] = newUpgradeDir;
		map ["md5"] = md5;
		list.Add (map);
		str = JSON.JsonEncode (list);
		File.WriteAllText (p, str);
	}

	/// <summary>
	/// Project data. 工程配置数据
	/// </summary>
	public class ProjectData
	{
		public string name = "";
		public int id = 0;
		//		public string upgradeUrl = "";
		public string cfgFolderStr = "";

		//		public int	assetsTimeout4Rlease = 60;
		public string companyPanelName = "";
		//		public string hudAlertBackgroundSpriteName = "";
		public string ingoreResWithExtensionNames = ".meta;.ds_store;.iml;.idea;.project;.buildpath;.git;.vscode";
		public bool isLuaPackaged = true;
		public ArrayList hotUpgradeServers = new ArrayList ();

		UnityEngine.Object _cfgFolder;

		public UnityEngine.Object cfgFolder {
			get {
				if (_cfgFolder == null && !string.IsNullOrEmpty (cfgFolderStr)) {
					_cfgFolder = AssetDatabase.LoadAssetAtPath (cfgFolderStr, 
						typeof(UnityEngine.Object));
				}
				return _cfgFolder;
			}
			set {
				_cfgFolder = value;
				if (_cfgFolder != null) {
					cfgFolderStr = AssetDatabase.GetAssetPath (_cfgFolder.GetInstanceID ());
				} else {
					cfgFolderStr = "";
				}
			}
		}

		public Hashtable ToMap ()
		{
			Hashtable r = new Hashtable ();
			r ["name"] = name;
			r ["id"] = id;
//			r ["upgradeUrl"] = upgradeUrl;
			r ["cfgFolderStr"] = cfgFolderStr;
//			r ["assetsTimeout4Rlease"] = assetsTimeout4Rlease;
			r ["companyPanelName"] = companyPanelName;
//			r ["hudAlertBackgroundSpriteName"] = hudAlertBackgroundSpriteName;
			r ["ingoreResWithExtensionNames"] = ingoreResWithExtensionNames;
			r ["isLuaPackaged"] = isLuaPackaged;
			HotUpgradeServerInfor s = null;
			ArrayList _list = new ArrayList ();
			for (int i = 0; i < hotUpgradeServers.Count; i++) {
				s = hotUpgradeServers [i] as HotUpgradeServerInfor;
				_list.Add (s.ToMap ());
			}
			r ["hotUpgradeServers"] = _list;
			return r;
		}

		public static ProjectData parse (Hashtable map)
		{
			if (map == null) {
				return null;
			}
			ProjectData r = new ProjectData ();
			r.name = MapEx.getString (map, "name");
			r.id = MapEx.getInt (map, "id");
//			r.upgradeUrl = MapEx.getString (map, "upgradeUrl");
//			r.assetsTimeout4Rlease = MapEx.getInt (map, "assetsTimeout4Rlease");
			r.cfgFolderStr = MapEx.getString (map, "cfgFolderStr");
			r.companyPanelName = MapEx.getString (map, "companyPanelName");
//			r.hudAlertBackgroundSpriteName = MapEx.getString (map, "hudAlertBackgroundSpriteName");
			r.ingoreResWithExtensionNames = MapEx.getString (map, "ingoreResWithExtensionNames");
			r.isLuaPackaged = MapEx.getBool (map, "isLuaPackaged");

			r.hotUpgradeServers = new ArrayList ();
			ArrayList _list = MapEx.getList (map, "hotUpgradeServers");
			for (int i = 0; i < _list.Count; i++) {
				r.hotUpgradeServers.Add (HotUpgradeServerInfor.parse ((Hashtable)(_list [i])));
			}
			return r;
		}
	}
}

//发布资源时回调用
public class PublishResAttribute : Attribute
{
}

public class SkipCollectResAttribute : Attribute
{
}
