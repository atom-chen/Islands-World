/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   新建一个scene
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using Coolape;
using UnityEditorHelper;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using UnityEditorHelper;

public class HotUpgradeServerInfor
{
	public string name="";
	public string key="";
	public string host4UploadUpgradePackage = "";
	public int port4UploadUpgradePackage = 21;
	public string ftpUser = "";
	public string ftpPassword = "";
	public string RemoteBaseDir = "";
	public bool useSFTP = false;
	public bool upgradeControledbyEachServer = false;
	public string hotUpgradeBaseUrl = "";
	public string host4Entry = "";
	public int port4Entry = 80;
	public string getServerListUrl = "";
	public string setServerPkgMd5Url = "";
	public string ossCmd = "";
	public bool isUploadOSSTemp = false;

	UnityEngine.Object _ossShell;

	public UnityEngine.Object ossShell {
		get {
			if (_ossShell == null && !string.IsNullOrEmpty (ossCmd)) {
				_ossShell = AssetDatabase.LoadAssetAtPath (ossCmd, 
					typeof(UnityEngine.Object));
			}
			return _ossShell;
		}
		set {
			_ossShell = value;
			if (_ossShell != null) {
				ossCmd = AssetDatabase.GetAssetPath (_ossShell.GetInstanceID ());
			} else {
				ossCmd = "";
			}
		}
	}

	public Hashtable ToMap ()
	{
		Hashtable r = new Hashtable ();
		r ["name"] = name;
		r ["key"] = Utl.MD5Encrypt (name);
		r ["host4UploadUpgradePackage"] = host4UploadUpgradePackage;
		r ["port4UploadUpgradePackage"] = port4UploadUpgradePackage;
		r ["ftpUser"] = ftpUser;
		r ["ftpPassword"] = ftpPassword;
		r ["RemoteBaseDir"] = RemoteBaseDir;
		r ["useSFTP"] = useSFTP;
		r ["upgradeControledbyEachServer"] = upgradeControledbyEachServer;
		r ["hotUpgradeBaseUrl"] = hotUpgradeBaseUrl;
		r ["host4Entry"] = host4Entry;
		r ["port4Entry"] = port4Entry;
		r ["getServerListUrl"] = getServerListUrl;
		r ["setServerPkgMd5Url"] = setServerPkgMd5Url;
		r ["ossCmd"] = ossCmd;
		r ["isUploadOSSTemp"] = isUploadOSSTemp;
		return r;
	}


	public static HotUpgradeServerInfor parse (Hashtable map)
	{
		if (map == null) {
			return null;
		}
		HotUpgradeServerInfor r = new HotUpgradeServerInfor ();
		r.name = MapEx.getString (map, "name");
		r.key = MapEx.getString (map, "key");
		r.host4UploadUpgradePackage = MapEx.getString (map, "host4UploadUpgradePackage");
		r.port4UploadUpgradePackage = MapEx.getInt (map, "port4UploadUpgradePackage");
		r.ftpUser = MapEx.getString (map, "ftpUser");
		r.ftpPassword = MapEx.getString (map, "ftpPassword");
		r.RemoteBaseDir = MapEx.getString (map, "RemoteBaseDir");
		r.useSFTP = MapEx.getBool (map, "useSFTP");
		r.upgradeControledbyEachServer = MapEx.getBool (map, "upgradeControledbyEachServer");
		r.hotUpgradeBaseUrl = MapEx.getString (map, "hotUpgradeBaseUrl");
		r .host4Entry = MapEx.getString(map, "host4Entry");
		r .port4Entry = MapEx.getInt(map, "port4Entry");
		r.getServerListUrl = MapEx.getString (map, "getServerListUrl");
		r.setServerPkgMd5Url = MapEx.getString (map, "setServerPkgMd5Url");
		r.ossCmd = MapEx.getString (map, "ossCmd");
		r.isUploadOSSTemp = MapEx.getBool (map, "isUploadOSSTemp");
		return r;
	}
}

public static class ECLProjectSetting
{
	static ECLProjectManager manager;
	static Texture2D tabTexture;
	static bool state1 = false;
	static bool state2 = false;
	static bool state3 = false;
	static bool state4 = false;

	const int labWidth = 200;
	static bool isProcingNewProject = false;

	static bool haveSetDelegate = false;
	static HotUpgradeServerInfor newServerInfro = new HotUpgradeServerInfor ();
	static int selectedServerIndex = 0;
	static bool isShowServerInfor = true;

	public static void setDelegate ()
	{
		if (haveSetDelegate)
			return;
		haveSetDelegate = true;
		EditorApplication.update -= OnUpdate;
		EditorApplication.update += OnUpdate;
	}

	static void OnUpdate ()
	{
		if (isWaitProcing ()) {
			finishWaitProcing ();
			createProject2 ();
		}
	}

	static ECLProjectManager.ProjectData data {
		get {
			return ECLProjectManager.data;
		}
		set {
			ECLProjectManager.data = value;
		}
	}

	public static bool isProjectExit (ECLProjectManager manager)
	{
		ECLProjectSetting.manager = manager;
		bool ret = CLCfgBase.self != null
		           && CLMainBase.self != null
		           && !string.IsNullOrEmpty (ECLProjectManager.data.name)
		           && Directory.Exists (Application.dataPath + "/" + ECLProjectManager.data.name)
		           && ECLProjectSetting.manager != null
		           && ECLProjectSetting.manager.exitCfgPath ();
		return ret;
	}

	public static void showProjectInfor (ECLProjectManager _data)
	{
		setDelegate ();
		manager = _data;
		if (manager == null)
			return;

		using (new HighlightBox ()) {
			GUI.color = Color.white;
			//===================================================
			GUILayout.BeginHorizontal ();
			{
				GUILayout.Label ("Project Name:", GUILayout.Width (labWidth));
				data.name = GUILayout.TextField (data.name);
			}
			GUILayout.EndHorizontal ();
			//===================================================
			GUILayout.BeginHorizontal ();
			{
				GUILayout.Label ("Project Unique ID:", GUILayout.Width (labWidth));
				data.id = EditorGUILayout.IntField (data.id);
			}
			GUILayout.EndHorizontal ();
			//===================================================
			GUILayout.BeginHorizontal ();
			{
				GUILayout.Label ("Company Logo Panel Name:", GUILayout.Width (labWidth));
				data.companyPanelName = GUILayout.TextField (data.companyPanelName);
			}
			GUILayout.EndHorizontal ();
			//===================================================
			GUILayout.BeginHorizontal ();
			{
				GUILayout.Label ("Ingore Res With ExtensionNames\nLike:\".mata;.tmp\":", GUILayout.Width (labWidth));
				GUI.contentColor = Color.yellow;
				data.ingoreResWithExtensionNames = GUILayout.TextArea (data.ingoreResWithExtensionNames, GUILayout.Height (30));
				GUI.contentColor = Color.white;
			}
			GUILayout.EndHorizontal ();
			//===================================================
			GUILayout.BeginHorizontal ();
			{
				GUILayout.Label ("Package Lua  to【priority.r】:", GUILayout.Width (labWidth));
				data.isLuaPackaged = EditorGUILayout.Toggle (data.isLuaPackaged);
			}
			GUILayout.EndHorizontal ();
			//===================================================
			if (isProjectExit (manager)) {
				GUILayout.BeginHorizontal ();
				{
					GUILayout.Label ("Json Cfg data Folder", GUILayout.Width (labWidth));
					data.cfgFolder = EditorGUILayout.ObjectField (data.cfgFolder, 
						typeof(UnityEngine.Object));
				}
				GUILayout.EndHorizontal ();

				showUpgradePackageSettings ();
			}

			GUILayout.BeginHorizontal ();
			{
				GUI.contentColor = Color.green;
				if (!isProjectExit (manager)) {
					if (GUILayout.Button ("New Project")) {
						createProject ();
					}
				}

//				if (isProjectExit (manager)) {
//					if (GUILayout.Button ("Apply Project")) {
//						if (EditorUtility.DisplayDialog ("Alert", "Will cover the previous settings!", "Okey", "cancel")) {
//							if (setScene ()) {
//								EditorUtility.DisplayDialog ("success", "Successed!", "Okey");
//							} else {
//								EditorUtility.DisplayDialog ("success", "There may some errors!", "Okey");
//							}
//						}
//					}
//				}

				if (isProjectExit (manager)) {
					if (GUILayout.Button ("Refresh", GUILayout.Height (30))) {
						ECLProjectManager.initData ();	
					}

					if (GUILayout.Button ("Save Project Config", GUILayout.Height (30))) {
						if (isInputDataValide ()) {
							if (CLCfgBase.self != null) {
								CLCfgBase.self.appUniqueID = data.id;
								EditorUtility.SetDirty (CLCfgBase.self);
							}
							ECLProjectManager.saveData ();
							EditorUtility.DisplayDialog ("success", "Successed!", "Okey");
						}
					}
				}
				GUI.contentColor = Color.white;
			}
			GUILayout.EndHorizontal ();

			//===================================================
			try {
				if (!isProcingNewProject) {
					showOtherSettings ();
				}
			} catch (Exception e) {
				Debug.LogError (e);
			}
			//===================================================
		}
	}

	public static void showUpgradePackageSettings ()
	{
		if (manager == null
		    || !isProjectExit (manager)) {
			return;
		}
		GUILayout.Space (5);
		ECLEditorUtl.BeginContents ();
		{
			GUI.contentColor = Color.cyan;
			state2 = NGUIEditorTools.DrawHeader ("UpgradePackage Settings");
			GUI.contentColor = Color.white;

			if (state2) {
				GUILayout.BeginHorizontal ();
				{
					GUILayout.Label ("Controled by Each Server:", GUILayout.Width (labWidth));
					CLCfgBase.self.hotUpgrade4EachServer = EditorGUILayout.Toggle (CLCfgBase.self.hotUpgrade4EachServer);
				}
				GUILayout.EndHorizontal ();

				ECLEditorUtl.BeginContents ();
				{
					GUI.color = Color.green;
					state3 = NGUIEditorTools.DrawHeader ("Add Server 4 Hot Upgrade");
					if (state3) {
						newServerInfro = cellServerInor (newServerInfro, true);
						if (GUILayout.Button ("Add")) {
							if (string.IsNullOrEmpty (newServerInfro.name)) {
								EditorUtility.DisplayDialog ("Error", "The Name is emtpy!!", "Okay");
							} else {
								bool activeData = true;
								for (int i = 0; i < ECLProjectManager.data.hotUpgradeServers.Count; i++) {
									HotUpgradeServerInfor cellServer = ECLProjectManager.data.hotUpgradeServers [i] as HotUpgradeServerInfor;
									if (cellServer.name.Equals (newServerInfro.name)) {
										activeData = false;
										EditorUtility.DisplayDialog ("Error", "The Name is exsit!!", "Okay");
										break;
									}
								}
								if (activeData) {
									ECLProjectManager.data.hotUpgradeServers.Add (newServerInfro);
									newServerInfro.key = Utl.MD5Encrypt (newServerInfro.name);
									ECLProjectManager.saveData ();
									newServerInfro = new HotUpgradeServerInfor ();
								}
							}
						}
					}
					GUI.color = Color.white;
				}
				ECLEditorUtl.EndContents ();


				ECLEditorUtl.BeginContents ();
				{
					state4 = NGUIEditorTools.DrawHeader ("Servers 4 Hot Upgrade");
					if (state4 && ECLProjectManager.data.hotUpgradeServers.Count > 0) {
						GUILayout.Space (5);

						List<string> toolbarNames = new List<string> ();
						for (int i = 0; i < ECLProjectManager.data.hotUpgradeServers.Count; i++) {
							HotUpgradeServerInfor d = ECLProjectManager.data.hotUpgradeServers [i] as HotUpgradeServerInfor;
							toolbarNames.Add (d.name);
						}
						selectedServerIndex = GUILayout.Toolbar (selectedServerIndex, toolbarNames.ToArray ());

						HotUpgradeServerInfor hsi = ECLProjectManager.data.hotUpgradeServers [selectedServerIndex] as HotUpgradeServerInfor;
						GUILayout.Space (-5);
						using (new UnityEditorHelper.HighlightBox ()) {
							cellServerInor (hsi, false);
							//===================================================
							GUILayout.BeginHorizontal ();
							{
								if (GUILayout.Button ("Apply")) {
									if (CLVerManager.self != null) {
										CLVerManager.self.baseUrl = hsi.hotUpgradeBaseUrl;
										Net.self.host4Publish = hsi.host4Entry;
										Net.self.gatePort = hsi.port4Entry;
									}
								}
								if (GUILayout.Button ("Delete")) {
									if (EditorUtility.DisplayDialog ("Alter", "Really want to delete?", "Okay", "Cancel")) {
										ECLProjectManager.data.hotUpgradeServers.RemoveAt (selectedServerIndex);
										selectedServerIndex = 0;
									}
								}
							}
							GUILayout.EndHorizontal ();
						}
					}
				}
				ECLEditorUtl.EndContents ();
			}
		}
		ECLEditorUtl.EndContents ();
	}

	public static HotUpgradeServerInfor cellServerInor (HotUpgradeServerInfor data, bool isNew)
	{
//		ECLEditorUtl.BeginContents ();
		//		{
		//===================================================
		if (isNew) {
			GUILayout.BeginHorizontal ();
			{
				GUILayout.Label ("Name [Can not modify after saved]:", GUILayout.Width (labWidth));
				data.name = GUILayout.TextField (data.name);
			}
			GUILayout.EndHorizontal ();
		}
		//===================================================
		if (!isNew) {
			GUILayout.BeginHorizontal ();
			{
				GUILayout.Label ("Key:", GUILayout.Width (labWidth));
				GUILayout.TextField (data.key);
			}
			GUILayout.EndHorizontal ();
		}
		//===================================================
		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label ("Hot Upgrade Base Url:", GUILayout.Width (labWidth));
			data.hotUpgradeBaseUrl = GUILayout.TextField (data.hotUpgradeBaseUrl);
		}
		GUILayout.EndHorizontal ();
		//===================================================
		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label ("Host 4 Upload Upgrade Package:", GUILayout.Width (labWidth));
			data.host4UploadUpgradePackage = GUILayout.TextField (data.host4UploadUpgradePackage);
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label ("Port 4 Upload Upgrade Package:", GUILayout.Width (labWidth));
			data.port4UploadUpgradePackage = EditorGUILayout.IntField (data.port4UploadUpgradePackage);
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label ("Ftp User:", GUILayout.Width (labWidth));
			data.ftpUser = GUILayout.TextField (data.ftpUser);
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label ("Ftp Password:", GUILayout.Width (labWidth));
			data.ftpPassword = GUILayout.TextField (data.ftpPassword);
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label ("Remote Base Dir:", GUILayout.Width (labWidth));
			data.RemoteBaseDir = GUILayout.TextField (data.RemoteBaseDir);
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label ("SFTP:", GUILayout.Width (labWidth));
			data.useSFTP = EditorGUILayout.Toggle (data.useSFTP);
		}
		GUILayout.EndHorizontal ();

		if (!data.useSFTP) {
			GUILayout.BeginHorizontal ();
			{
				GUILayout.Label ("URI:", GUILayout.Width (labWidth));
				GUILayout.Label ("ftp://" + data.host4UploadUpgradePackage + data.RemoteBaseDir);
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			{
				EditorGUILayout.HelpBox (
					"uri = \"ftp://example.com/%2F/directory\" //Go to a forward directory (cd directory)\nuri = \"ftp://example.com/%2E%2E\" //Go to the previously directory (cd ../)",
					MessageType.Info);
			}
			GUILayout.EndHorizontal ();
		}

		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label ("同步对像存储脚本:", GUILayout.Width (labWidth));
			data.ossShell = EditorGUILayout.ObjectField (data.ossShell, 
				typeof(UnityEngine.Object));
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label ("同步对像存储临时目录:", GUILayout.Width (labWidth));
			data.isUploadOSSTemp = EditorGUILayout.Toggle (data.isUploadOSSTemp);
		}
		GUILayout.EndHorizontal ();

		GUILayout.Label ("入口--------------------------");
		//===================================================
		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label ("Host 4 Entry:", GUILayout.Width (labWidth));
			data.host4Entry = GUILayout.TextField (data.host4Entry);
		}
		GUILayout.EndHorizontal ();
		//===================================================

		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label ("Port 4 Entry:", GUILayout.Width (labWidth));
			data.port4Entry = EditorGUILayout.IntField (data.port4Entry);
		}
		GUILayout.EndHorizontal ();
		//===================================================
		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label ("URL of get server list:", GUILayout.Width (labWidth));
			data.getServerListUrl = GUILayout.TextField (data.getServerListUrl);
		}
		GUILayout.EndHorizontal ();
		//===================================================

		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label ("URL of set upgrade pkg md5:", GUILayout.Width (labWidth));
			data.setServerPkgMd5Url = GUILayout.TextField (data.setServerPkgMd5Url);
		}
		GUILayout.EndHorizontal ();
		//===================================================
//		}
//		ECLEditorUtl.EndContents ();
		return data;
	}

	public static void showOtherSettings ()
	{
		if (manager == null
		    || !isProjectExit (manager)) {
			return;
		}
		GUILayout.Space (5);
		ECLEditorUtl.BeginContents ();
		{
			GUI.contentColor = Color.cyan;
			state1 = NGUIEditorTools.DrawHeader ("Project Settings");
			GUI.contentColor = Color.white;

			if (state1) {
				if (CLVerManager.self != null) {
					//===================================================
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label ("Hot Upgrade Base Url:", GUILayout.Width (labWidth));
						GUILayout.TextField (CLVerManager.self.baseUrl);
					}
					GUILayout.EndHorizontal ();

					//===================================================
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label ("is 2G Net Hot Upgrade:", GUILayout.Width (labWidth));
						CLVerManager.self.is2GNetUpgrade = EditorGUILayout.Toggle (CLVerManager.self.is2GNetUpgrade);
					}
					GUILayout.EndHorizontal ();
					//===================================================
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label ("is 3G Net Hot Upgrade:", GUILayout.Width (labWidth));
						CLVerManager.self.is3GNetUpgrade = EditorGUILayout.Toggle (CLVerManager.self.is3GNetUpgrade);
					}
					GUILayout.EndHorizontal ();
					//===================================================
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label ("is 4G Net Hot Upgrade:", GUILayout.Width (labWidth));
						CLVerManager.self.is4GNetUpgrade = EditorGUILayout.Toggle (CLVerManager.self.is4GNetUpgrade);
					}
					GUILayout.EndHorizontal ();
				}
				//===================================================
				if (CLAssetsManager.self != null) {
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label ("Assets Timeout 4 Rlease(Seconds):", GUILayout.Width (labWidth));
						CLAssetsManager.self.timeOutSec4Realse = EditorGUILayout.IntField (CLAssetsManager.self.timeOutSec4Realse);
					}
					GUILayout.EndHorizontal ();
				}
				//===================================================
				if (CLAlert.self != null) {
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label ("Alert BG SpriteName:", GUILayout.Width (labWidth));
						CLAlert.self.hudBackgroundSpriteName = GUILayout.TextField (CLAlert.self.hudBackgroundSpriteName);
					}
					GUILayout.EndHorizontal ();
				
					//===================================================
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label ("Alert BG Color:", GUILayout.Width (labWidth));
						CLAlert.self.hudBackgroundColor = EditorGUILayout.ColorField (CLAlert.self.hudBackgroundColor);
					}
					GUILayout.EndHorizontal ();
				}
				//===================================================
				if (CLCfgBase.self != null) {
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label ("is Not Editor Mode:", GUILayout.Width (labWidth));
						CLCfgBase.self.isNotEditorMode = EditorGUILayout.Toggle (CLCfgBase.self.isNotEditorMode);
					}
					GUILayout.EndHorizontal ();
					//===================================================
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label ("Need Encode Lua:", GUILayout.Width (labWidth));
						CLCfgBase.self.isEncodeLua = EditorGUILayout.Toggle (CLCfgBase.self.isEncodeLua);
					}
					GUILayout.EndHorizontal ();
					//===================================================
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label ("Singin Md5 Code:", GUILayout.Width (labWidth));
						CLCfgBase.self.singinMd5Code = GUILayout.TextField (CLCfgBase.self.singinMd5Code);
					}
					GUILayout.EndHorizontal ();
				}
				//===================================================
				if (Net.self != null) {
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label ("Host 4 Publish:", GUILayout.Width (labWidth));
						if (Net.self.switchNetType == Net.NetWorkType.publish) {
							GUI.color = Color.LerpUnclamped (Color.cyan, Color.green, 0.3f);
						}
						Net.self.host4Publish = GUILayout.TextField (Net.self.host4Publish);
						GUI.color = Color.white;
					}
					GUILayout.EndHorizontal ();
					//===================================================
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label ("Host 4 Test1:", GUILayout.Width (labWidth));
						if (Net.self.switchNetType == Net.NetWorkType.test1) {
							GUI.color = Color.LerpUnclamped (Color.red, Color.green, 0.1f);
						}
						Net.self.host4Test1 = GUILayout.TextField (Net.self.host4Test1);
						GUI.color = Color.white;
					}
					GUILayout.EndHorizontal ();
					//===================================================
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label ("Host 4 Test2:", GUILayout.Width (labWidth));
						if (Net.self.switchNetType == Net.NetWorkType.test2) {
							GUI.color = Color.LerpUnclamped (Color.red, Color.green, 0.1f);
						}
						Net.self.host4Test2 = GUILayout.TextField (Net.self.host4Test2);
						GUI.color = Color.white;
					}
					GUILayout.EndHorizontal ();
					//===================================================
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label ("Port:", GUILayout.Width (labWidth));
						Net.self.gatePort = EditorGUILayout.IntField (Net.self.gatePort);
					}
					GUILayout.EndHorizontal ();
					//===================================================
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label ("Switch Net:", GUILayout.Width (labWidth));

						if (Net.self.switchNetType == Net.NetWorkType.publish) {
							GUI.color = Color.LerpUnclamped (Color.cyan, Color.green, 0.3f);
						} else {
							GUI.color = Color.LerpUnclamped (Color.red, Color.green, 0.1f);
							GUI.contentColor = Color.white;
						}
						Net.self.switchNetType = (Net.NetWorkType)(EditorGUILayout.EnumPopup (Net.self.switchNetType));

						GUI.color = Color.white;
						GUI.contentColor = Color.white;
					}
					GUILayout.EndHorizontal ();
				}
				//===================================================
				if (CLPanelManager.self != null) {
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label ("Main Panel Name:", GUILayout.Width (labWidth));
						CLPanelManager.self.mainPanelName = GUILayout.TextField (CLPanelManager.self.mainPanelName);
					}
					GUILayout.EndHorizontal ();
				}
				//===================================================
				if (!Application.isPlaying) {
					EditorSceneManager.MarkAllScenesDirty ();
				}
			}
		}
		ECLEditorUtl.EndContents ();
	}

	static bool isInputDataValide ()
	{
		if (string.IsNullOrEmpty (data.name)) {
			EditorUtility.DisplayDialog ("Alert", "The [Project Name] is NULL!", "Okey");
			return false;
		}
		if (string.IsNullOrEmpty (data.companyPanelName)) {
			EditorUtility.DisplayDialog ("Alert", "The [Company Panel Name] is NULL!", "Okey");
			return false;
		}
		return true;
	}

	static void begainWaitProcing ()
	{
		string isProcingPath = Application.dataPath + "/" + ECLProjectManager.data.name + "/isProcingPleasWait";
		File.WriteAllText (isProcingPath, "");
	}

	static bool isWaitProcing ()
	{
		try {
			string isProcingPath = Application.dataPath + "/" + ECLProjectManager.data.name + "/isProcingPleasWait";
			return File.Exists (isProcingPath);
		} catch (Exception e) {
			Debug.LogWarning (e);
			return false;
		}
	}

	static void finishWaitProcing ()
	{
		string isProcingPath = Application.dataPath + "/" + ECLProjectManager.data.name + "/isProcingPleasWait";
		try {
			File.Delete (isProcingPath);
		} catch (Exception e) {
			Debug.LogWarning (e);
		}
	}

	static void createProject ()
	{
		if (!isInputDataValide ()) {
			EditorUtility.ClearProgressBar ();
			return;
		}
		showProgressBar (0.1f);
		ECLProjectManager.saveData ();
		showProgressBar (0.3f);
		if (!CreateFolders ()) {
			EditorUtility.ClearProgressBar ();
			return;
		}
		showProgressBar (0.5f);
		if (!prepareAsset ()) {
			EditorUtility.ClearProgressBar ();
			return;
		}
	}

	static void createProject2 ()
	{
		showProgressBar (0.6f);
		if (!createScene ()) {
			EditorUtility.ClearProgressBar ();
			return;
		}

		if (!setScene ()) {
			EditorUtility.ClearProgressBar ();
			return;
		}
		showProgressBar (0.8f);
		AssetDatabase.Refresh ();
		data.cfgFolder = ECLEditorUtl.getObjectByPath (data.name + "/DBCfg/jsonCfg");
		data.cfgFolderStr = "Assets/" + data.name + "/DBCfg/jsonCfg";
		showProgressBar (1);

		ECLProjectManager.saveData ();
		AssetDatabase.Refresh ();
		EditorUtility.ClearProgressBar ();
		EditorUtility.DisplayDialog ("Success", string.Format ("Create project[{0}] cuccess!", data.name), "Okey");
	}

	static void showProgressBar (float val)
	{
		EditorUtility.DisplayProgressBar ("Create Project", "Please wait.........if long time no response,please click the screen!", val);	
	}

	static bool CreateFolders ()
	{
		if (Directory.Exists (Application.dataPath + "/" + data.name)) {
			EditorUtility.DisplayDialog ("Alert", string.Format ("The Director[{0}] allready exit!", Application.dataPath + "/" + data.name), "Okey");
			return false;
		}
		Directory.CreateDirectory (Application.dataPath + "/" + data.name);
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/_scene");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/Resources");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/Resources/Atlas");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/Resources/Font");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/Resources/Shaders");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/DBCfg");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/DBCfg/jsonCfg");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/Editor");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/Scripts");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/Scripts/xLua");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/Scripts/Main");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/Scripts/public");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/other");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/other/bullet");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/other/effect");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/other/sound");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/other/Materials");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/other/roles");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/other/Textures");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/other/things");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/other/uiAtlas");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/other/uiAtlas/public");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/atlas");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/font");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/cfg");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/localization");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/cfg");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/net");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/public");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/toolkit");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/ui");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/ui/panel");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/ui/cell");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/ui");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/ui/panel");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/ui/other");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Publish");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/upgradeRes4Ver");
		Directory.CreateDirectory (Application.dataPath + "/" + data.name + "/xRes");
		Directory.CreateDirectory (Application.dataPath + "/StreamingAssets");

		AssetDatabase.Refresh ();
//		EditorUtility.DisplayDialog ("success", "Create Folders cuccess!", "Okey");
		return true;
	}

	public static bool prepareAsset ()
	{
		//Copy dataCfgTool
		string from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/DataCfg/DataCfgTool.xls";
		string to = Application.dataPath + "/" + data.name + "/DBCfg/DataCfgTool.xls";
		File.Copy (from, to);
		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/DataCfg/dataCpTool.bat";
		to = Application.dataPath + "/" + data.name + "/DBCfg/dataCpTool.bat";
		File.Copy (from, to);

		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Textures/_empty.png";
		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/other/uiAtlas/public/_empty.png";
		File.Copy (from, to);

		AssetDatabase.Refresh ();
		//Prefab
		GameObject atlasAllRealGo = new GameObject ("atlasAllReal");
		UIAtlas atlasAllReal = atlasAllRealGo.AddComponent<UIAtlas> ();
		atlasAllReal.isBorrowSpriteMode = true;
		List<Texture> textureList = new List<Texture> ();
		Texture tex = ECLEditorUtl.getObjectByPath (data.name + "/upgradeRes4Dev/other/uiAtlas/public/_empty.png") as Texture;
		textureList.Add (tex);
		UIAtlasMaker.UpdateAtlas_BorrowMode (atlasAllReal, textureList);
		textureList.Clear ();
		textureList = null;
		atlasAllRealGo = PrefabUtility.CreatePrefab ("Assets/" + data.name + "/upgradeRes4Dev/priority/atlas/atlasAllReal.prefab", atlasAllRealGo) as GameObject;
		atlasAllReal = atlasAllRealGo.GetComponent<UIAtlas> ();

		GameObject atlasGo = new GameObject ();
		UIAtlas atlas = atlasGo.AddComponent<UIAtlas> ();
		atlas.isBorrowSpriteMode = true;
		atlas.replacement = atlasAllReal;
		atlasGo = PrefabUtility.CreatePrefab ("Assets/" + data.name + "/Resources/Atlas/EmptyAtlas.prefab", atlasGo);
		atlas = atlasGo.GetComponent<UIAtlas> ();

		GameObject fontGo = new GameObject ();
		UIFont font = fontGo.AddComponent<UIFont> ();
		font.defaultSize = 22;
		fontGo = PrefabUtility.CreatePrefab ("Assets/" + data.name + "/Resources/Font/EmptyFont.prefab", fontGo);
		font = fontGo.GetComponent<UIFont> ();

		//Copy Lua res
		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/luaTemplates.zip";
		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/";
		ZipEx.UnZip (from, to, 4096);
//		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/TempMainLua.lua";
//		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/CLLMainLua.lua";
//		ECLCreateFile.PubCreatNewFile2 (from, to);
//
//		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/cfg/TempDBCfg.lua";
//		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/cfg/DBCfg.lua";
//		ECLCreateFile.PubCreatNewFile2 (from, to);
//
//		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/cfg/TempDBCfgTool.lua";
//		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/cfg/DBCfgTool.lua";
//		ECLCreateFile.PubCreatNewFile2 (from, to);
//
//		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/net/TempNetDispatch.lua";
//		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/net/NetDispatch.lua";
//		ECLCreateFile.PubCreatNewFile2 (from, to);
//
//		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/public/TempInclude.lua";
//		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/public/CLLInclude.lua";
//		ECLCreateFile.PubCreatNewFile2 (from, to);
//
//		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/public/TempPrefs.lua";
//		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/public/CLLPrefs.lua";
//		ECLCreateFile.PubCreatNewFile2 (from, to);
//
//		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/toolkit/TempLuaUtl.lua";
//		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/toolkit/LuaUtl.lua";
//		ECLCreateFile.PubCreatNewFile2 (from, to);
//
//		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/toolkit/TempUpdateUpgrader.lua";
//		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/toolkit/CLLUpdateUpgrader.lua";
//		ECLCreateFile.PubCreatNewFile2 (from, to);
//
//		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/toolkit/TempVerManager.lua";
//		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/toolkit/CLLVerManager.lua";
//		ECLCreateFile.PubCreatNewFile2 (from, to);
//
//		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/ui/panel/TempPBackplate.lua";
//		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/ui/panel/CLLPBackplate.lua";
//		ECLCreateFile.PubCreatNewFile2 (from, to);
//
//		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/ui/panel/TempPConfirm.lua";
//		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/ui/panel/CLLPConfirm.lua";
//		ECLCreateFile.PubCreatNewFile2 (from, to);
//
//		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/ui/panel/TempPHotWheel.lua";
//		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/ui/panel/CLLPHotWheel.lua";
//		ECLCreateFile.PubCreatNewFile2 (from, to);
//
//		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/ui/panel/TempPSplash.lua";
//		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/ui/panel/CLLPSplash.lua";
//		ECLCreateFile.PubCreatNewFile2 (from, to);
//
//		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/ui/panel/TempPStart.lua";
//		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/ui/panel/CLLPStart.lua";
//		ECLCreateFile.PubCreatNewFile2 (from, to);
//
//		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/ui/panel/TempPWWWProgress.lua";
//		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/ui/panel/CLLPWWWProgress.lua";
//		ECLCreateFile.PubCreatNewFile2 (from, to);
//
//		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Lua/ui/cell/TempCellWWWProgress.lua";
//		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/lua/ui/cell/CLLCellWWWProgress.lua";
//		ECLCreateFile.PubCreatNewFile2 (from, to);

		//==================================================
		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/hotUpgradeCfg/tempChannels.json";
		to = Application.dataPath + "/StreamingAssets/channels.json";
		ECLCreateFile.PubCreatNewFile2 (from, to);
		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/hotUpgradeCfg/tempUpgraderVer.json";
		to = Application.dataPath + "/StreamingAssets/upgraderVer.json";
		ECLCreateFile.PubCreatNewFile2 (from, to);

		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/Localization/TempChinese.txt";
		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/Localization/Chinese.txt";
		ECLCreateFile.PubCreatNewFile2 (from, to);
		//==================================================
		GameObject panelGo = ECLEditorUtl.getObjectByPath (ECLProjectManager.FrameName + "/Templates/prefab/ui/TempPanelBackplate.prefab") as GameObject;
		panelGo = GameObject.Instantiate (panelGo) as GameObject;
		CLPanelLua panelLua = panelGo.GetComponent<CLPanelLua> ();
		panelLua.luaPath = data.name + "/upgradeRes/priority/lua/ui/panel/CLLPBackplate.lua";
		PrefabUtility.CreatePrefab ("Assets/" + data.name + "/upgradeRes4Dev/priority/ui/panel/PanelBackplate.prefab", panelGo);
		//==================================================
		panelGo = ECLEditorUtl.getObjectByPath (ECLProjectManager.FrameName + "/Templates/prefab/ui/TempPanelConfirm.prefab") as GameObject;
		panelGo = GameObject.Instantiate (panelGo) as GameObject;
		panelLua = panelGo.GetComponent<CLPanelLua> ();
		panelLua.luaPath = data.name + "/upgradeRes/priority/lua/ui/panel/CLLPConfirm.lua";
		PrefabUtility.CreatePrefab ("Assets/" + data.name + "/upgradeRes4Dev/priority/ui/panel/PanelConfirm.prefab", panelGo);
		//==================================================
		panelGo = ECLEditorUtl.getObjectByPath (ECLProjectManager.FrameName + "/Templates/prefab/ui/TempPanelHotWheel.prefab") as GameObject;
		panelGo = GameObject.Instantiate (panelGo) as GameObject;
		panelLua = panelGo.GetComponent<CLPanelLua> ();
		panelLua.luaPath = data.name + "/upgradeRes/priority/lua/ui/panel/CLLPHotWheel.lua";
		PrefabUtility.CreatePrefab ("Assets/" + data.name + "/upgradeRes4Dev/priority/ui/panel/PanelHotWheel.prefab", panelGo);
		//==================================================
		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/prefab/ui/TempPanelMask4Panel.prefab";
		to = Application.dataPath + "/" + data.name + "/upgradeRes4Dev/priority/ui/panel/PanelMask4Panel.prefab";
		File.Copy (from, to);
		AssetDatabase.Refresh ();
		//==================================================
		panelGo = ECLEditorUtl.getObjectByPath (ECLProjectManager.FrameName + "/Templates/prefab/ui/TempPanelSplash.prefab") as GameObject;
		panelGo = GameObject.Instantiate (panelGo) as GameObject;
		panelLua = panelGo.GetComponent<CLPanelLua> ();
		panelLua.luaPath = data.name + "/upgradeRes/priority/lua/ui/panel/CLLPSplash.lua";
		PrefabUtility.CreatePrefab ("Assets/" + data.name + "/upgradeRes4Dev/priority/ui/panel/PanelSplash.prefab", panelGo);
		//==================================================
		panelGo = ECLEditorUtl.getObjectByPath (ECLProjectManager.FrameName + "/Templates/prefab/ui/TempLogin.prefab") as GameObject;
		panelGo = GameObject.Instantiate (panelGo) as GameObject;
		panelLua = panelGo.GetComponent<CLPanelLua> ();
		panelLua.luaPath = data.name + "/upgradeRes/priority/lua/ui/panel/CLLPLogin.lua";
		PrefabUtility.CreatePrefab ("Assets/" + data.name + "/upgradeRes4Dev/priority/ui/panel/PanelLogin.prefab", panelGo);
		//==================================================

		panelGo = ECLEditorUtl.getObjectByPath (ECLProjectManager.FrameName + "/Templates/prefab/ui/TempPanelStart.prefab") as GameObject;
		panelGo = GameObject.Instantiate (panelGo) as GameObject;
		panelLua = panelGo.GetComponent<CLPanelLua> ();
		panelLua.luaPath = data.name + "/upgradeRes/priority/lua/ui/panel/CLLPStart.lua";
		PrefabUtility.CreatePrefab ("Assets/" + data.name + "/upgradeRes4Dev/priority/ui/panel/PanelStart.prefab", panelGo);
		//==================================================
		panelGo = ECLEditorUtl.getObjectByPath (ECLProjectManager.FrameName + "/Templates/prefab/ui/TempPanelWWWProgress.prefab") as GameObject;
		panelGo = GameObject.Instantiate (panelGo) as GameObject;
		panelLua = panelGo.GetComponent<CLPanelLua> ();
		panelLua.luaPath = data.name + "/upgradeRes/priority/lua/ui/panel/CLLPWWWProgress.lua";
		PrefabUtility.CreatePrefab ("Assets/" + data.name + "/upgradeRes4Dev/priority/ui/panel/PanelWWWProgress.prefab", panelGo);
		//==================================================
		panelGo = ECLEditorUtl.getObjectByPath (ECLProjectManager.FrameName + "/Templates/prefab/ui/TempAlertRoot.prefab") as GameObject;
		panelGo = GameObject.Instantiate (panelGo) as GameObject;
		PrefabUtility.CreatePrefab ("Assets/" + data.name + "/upgradeRes4Dev/priority/ui/other/AlertRoot.prefab", panelGo);
		//==================================================

		//C# Code
		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/cs/TempXluaGenCodeConfig.cs.bak";
		to = Application.dataPath + "/" + data.name + "/Scripts/xLua/XluaGenCodeConfig.cs";
		ECLCreateFile.PubCreatNewFile2 (from, to);

		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/cs/TempCfg.cs.bak";
		to = Application.dataPath + "/" + data.name + "/Scripts/public/MyCfg.cs";
		ECLCreateFile.PubCreatNewFile2 (from, to);

		from = Application.dataPath + "/" + ECLProjectManager.FrameName + "/Templates/cs/TempMain.cs.bak";
		to = Application.dataPath + "/" + data.name + "/Scripts/Main/MyMain.cs";
		ECLCreateFile.PubCreatNewFile2 (from, to);
		//-----------------------------------------------------------------------------------------
		// 因为新增了c#代码，需要等到编译完成后再继续处理
		//-----------------------------------------------------------------------------------------
		string Wait4CreateScenePath = Application.dataPath + "/" + data.name + "/Wait4CreateScene";
		File.WriteAllText (Wait4CreateScenePath, "");
		AssetDatabase.Refresh ();
		return true;
	}

	public static GameObject createPanelPrefab (string name, string luaPath,
	                                            bool isNeedBackplate, bool destroyWhenHide, 
	                                            bool isNeedResetAtlase, bool isNeedMask4Init)
	{
		GameObject panelGo = new GameObject (name);
		panelGo.AddComponent <UIPanel> ();
		CLPanelLua panel = panelGo.AddComponent <CLPanelLua> ();
		panel.isNeedBackplate = isNeedBackplate;
		panel.destroyWhenHide = destroyWhenHide;
		panel.isNeedResetAtlase = isNeedResetAtlase;
		panel.isNeedMask4Init = isNeedMask4Init;
		panel.luaPath = luaPath;
		return panelGo;
	}

	public static UISprite createSprite (string name, GameObject parent, GameObject panel, UIAtlas atlas, string spriteName, bool addCollider)
	{
		UISprite sp = NGUITools.AddSprite (parent, atlas, spriteName);
		if (addCollider) {
			sp.gameObject.AddComponent<BoxCollider> ();
			sp.autoResizeBoxCollider = true;
		}
		sp.name = name;
		sp.SetAnchor (panel, -5, -5, 5, 5);
		return sp;
	}

	public static bool createScene ()
	{
		Scene scene = EditorSceneManager.NewScene (NewSceneSetup.DefaultGameObjects);
		EditorSceneManager.SaveScene (scene, Application.dataPath + "/" + data.name + "/_scene/Main.unity");
		GameObject mainCamera = GameObject.Find ("Main Camera");

		GameObject go = new GameObject ("cfg");
		//		Type type = Types.GetType ("MyCfg", "Assembly-CSharp");
		Type type = Type.GetType ("MyCfg, Assembly-CSharp");
		if (type == null) {
			Debug.LogError ("Type.GetType MyCfg====null");
			return false;
		}
		go.AddComponent (type);
		go.GetComponent<CLCfgBase> ().appUniqueID = data.id;

		go.AddComponent<CLPathCfg> ();
		go.AddComponent<CLVerManager> ();
		go.AddComponent<CLAssetsManager> ();
		go.AddComponent<SoundEx> ();
        go.AddComponent<InvokeEx>();
        go.AddComponent<WWWEx>();

        go = new GameObject ("Net");
		go.AddComponent<Net> ();

		go = new GameObject ("Main");
		//		type = Types.GetType ("MyMain", "Assembly-CSharp");
		type = Type.GetType ("MyMain, Assembly-CSharp");
		if (type == null) {
			Debug.LogError ("Type.GetType MyMain====null");
			return false;
		}
		go.AddComponent (type);
		go.AddComponent<CLFPS> ();
		/*这是给只能同时播放一个音乐的方法使用的【SoundEx.playSoundSingleton()】。
		 * 例如选择角色时，角色会有很长一句话，当切换角色时，前一个角色所说的话句要关掉
		*/
		go.AddComponent<AudioSource> ().playOnAwake = false;

		GameObject lookAtTarget = new GameObject ("LookAtTarget");
		lookAtTarget.transform.parent = go.transform;
		lookAtTarget.transform.localPosition = Vector3.zero;

		mainCamera.transform.parent = go.transform;
		CLSmoothFollow follow = mainCamera.AddComponent<CLSmoothFollow> ();
		follow.target = lookAtTarget.transform;

		MyMainCamera myCamera = mainCamera.AddComponent<MyMainCamera> ();
		myCamera.eventType = MyMainCamera.EventType.World_3D;
		myCamera.enabled = false;
		myCamera.isbeControled = true;
		mainCamera.AddComponent<AudioSource> ().playOnAwake = false;

		// NGUI
		GameObject uiRoot = UICreateNewUIWizard.CreateNewUI (UICreateNewUIWizard.CameraType.Simple2D);
		GameObject screenTouch = new GameObject ("_ScreenTouch_");
		screenTouch.transform.parent = uiRoot.transform;
		screenTouch.transform.localPosition = Vector3.zero;
		screenTouch.AddComponent<BoxCollider> ();
		UIWidget screenTouchWidget = screenTouch.AddComponent<UIWidget> ();
		screenTouchWidget.depth = -10;
		screenTouchWidget.SetAnchor (uiRoot, -5, 5, -5, 5);
		screenTouchWidget.autoResizeBoxCollider = true;
		CLUIDrag4World drag4World = screenTouch.AddComponent<CLUIDrag4World> ();
		drag4World.main3DCamera = myCamera;
		drag4World.target = lookAtTarget.transform;
		drag4World.scaleTarget = follow;

		GameObject publicUI = new GameObject ("PublicUI");
		publicUI.transform.parent = uiRoot.transform;
		publicUI.AddComponent<UIPanel> ().depth = 10000;
		CLUIInit uiInit = publicUI.AddComponent<CLUIInit> ();
		uiInit.uiPublicRoot = publicUI.transform;

//		go = new GameObject("AlertRoot");
//		go.transform.parent = publicUI.transform;
//		HUDText hudTxt = go.AddComponent<HUDText>();
//		hudTxt.effect = UILabel.Effect.Shadow;
//		hudTxt.needAddValue = false;
//		hudTxt.needQueue = true;
//		go.AddComponent<HUDRoot>();
//		go.AddComponent<UIPanel>().depth = 10100;
//		go.AddComponent<CLAlert>();

		GameObject _PanelManager = new GameObject ("PanelManager");
		_PanelManager.transform.parent = uiRoot.transform;
		CLPanelManager PanelManager = _PanelManager.AddComponent<CLPanelManager> ();
		PanelManager.mainPanelName = "PanelMain";

		go = new GameObject (data.companyPanelName);
		go.transform.parent = PanelManager.transform;
		go.AddComponent <UIPanel> ().depth = 50;
		CLPanelLua panel = go.AddComponent <CLPanelLua> ();
		panel.isNeedBackplate = false;
		panel.destroyWhenHide = false;
		panel.isNeedResetAtlase = false;
		panel.isNeedMask4Init = false;

		NGUITools.SetChildLayer (uiRoot.transform, LayerMask.NameToLayer ("UI"));
		uiRoot.transform.position = new Vector3 (0, 10000, 0);
		//===========================================
		AssetDatabase.Refresh ();

		return true;
	}

	public static bool setScene ()
	{
		if (!isInputDataValide ())
			return false;
		GameObject cfgGo = GameObject.Find ("cfg");
		CLPathCfg pathCfg = cfgGo.GetComponent<CLPathCfg> ();
		pathCfg.resetPath (data.name);
		CLVerManager verManager = cfgGo.GetComponent<CLVerManager> ();
		verManager.is2GNetUpgrade = true;
		verManager.is3GNetUpgrade = true;
		verManager.is4GNetUpgrade = true;
		verManager.luaPath = data.name + "/upgradeRes/priority/lua/toolkit/CLLVerManager.lua";
		EditorUtility.SetDirty (verManager);

		CLAssetsManager assetsManager = cfgGo.GetComponent<CLAssetsManager> ();

		GameObject mainGo = GameObject.Find ("Main");
		GameObject mainCameraGo = GameObject.Find ("Main Camera");
		SoundEx sound = cfgGo.GetComponent<SoundEx> ();
		sound.mainAudio = GameObject.Find ("Main Camera").GetComponent<AudioSource> ();
		sound.singletonAudio = mainGo.GetComponent<AudioSource> ();

		Net net = GameObject.Find ("Net").GetComponent<Net> ();
        CLBaseLua netLua = net.gameObject.AddComponent<CLBaseLua>();
		netLua.luaPath = data.name + "/upgradeRes/priority/lua/net/CLLNet.lua";
        net.lua = netLua;

		CLMainBase main = mainGo.GetComponent<CLMainBase> ();
		main.firstPanel = data.companyPanelName;
		main.luaPath = data.name + "/upgradeRes/priority/lua/CLLMainLua.lua";
		mainGo.GetComponent<AudioSource> ().playOnAwake = false;

		#if UNITY_5_6_OR_NEWER
		Transform lookAtTarget = mainGo.transform.Find ("LookAtTarget");
		#else
		Transform lookAtTarget = mainGo.transform.FindChild ("LookAtTarget");
		#endif

		CLSmoothFollow follow = mainCameraGo.GetComponent<CLSmoothFollow> ();
		follow.target = lookAtTarget;

		MyMainCamera myCamera = mainCameraGo.GetComponent<MyMainCamera> ();
		myCamera.enabled = false;
		myCamera.isbeControled = true;

		// NGUI
		GameObject uiRoot = GameObject.Find ("UI Root");
		uiRoot.GetComponent<UIRoot> ().scalingStyle = UIRoot.Scaling.Constrained;

		GameObject publicUI = GameObject.Find ("PublicUI");
		CLUIInit uiInit = publicUI.GetComponent<CLUIInit> ();
		uiInit.uiPublicRoot = publicUI.transform;
		GameObject prefabGo = ECLEditorUtl.getObjectByPath (data.name + "/Resources/Atlas/EmptyAtlas.prefab") as GameObject;
		uiInit.emptAtlas = prefabGo.GetComponent<UIAtlas> ();
		prefabGo = ECLEditorUtl.getObjectByPath (data.name + "/Resources/Font/EmptyFont.prefab") as GameObject;
		uiInit.emptFont = prefabGo.GetComponent<UIFont> ();
//		HUDText hudTxt = GameObject.Find("AlertRoot").GetComponent<HUDText>();
//		hudTxt.fontName = "EmptyFont";
//		hudTxt.effect = UILabel.Effect.Shadow;
//		hudTxt.needAddValue = false;
//		hudTxt.needQueue = true;
//		CLAlert alert = hudTxt.GetComponent<CLAlert>();

		GameObject _PanelManager = GameObject.Find ("PanelManager");
		CLPanelManager PanelManager = _PanelManager.GetComponent<CLPanelManager> ();
		PanelManager._uiPanelRoot = publicUI.transform;

		EditorSceneManager.SaveScene (EditorSceneManager.GetActiveScene (), Application.dataPath + "/" + data.name + "/_scene/Main.unity");
		return true;
	}

	/// <summary>
	/// Ons the unity compiled. 当完成编译后的回调
	/// </summary>
	[UnityEditor.Callbacks.DidReloadScripts]
	public static void onUnityCompiled ()
	{
		string Wait4CreateScenePath = Application.dataPath + "/" + ECLProjectManager.data.name + "/Wait4CreateScene";
		if (File.Exists (Wait4CreateScenePath)) {
			File.Delete (Wait4CreateScenePath);
			//继续处理新建工程
//			createProject2 ();
			//不能直接调用方法，而是通过写了一个文件，然后在ongui进来时再调用createProject2（）
			begainWaitProcing ();
		}
	}
}
	
