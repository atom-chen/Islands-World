using UnityEditor;
using UnityEngine;
using System.Collections;
using Coolape;
using System.IO;
using System.Threading;
using System.Collections.Generic;

public class ECLUpgradeListProc : EditorWindow
{
	public static ECLUpgradeListProc self;

	public static  ArrayList mList = null;
	public static string cfgPath = "";
	Hashtable item;
	Vector2 scrollPos = Vector2.zero;
	static bool isFinishUpload = false;
	static bool uploadResult = false;
	static float uploadProgerss = 0;
	static bool isUploading = false;
	static string selectedPackageName = "";
	static bool isSelectMod = false;
	static Callback onSelectedCallback;
	static object selectedCallbackParams;
	static int selectedServerIndex = 0;
	static HotUpgradeServerInfor selectedServer = null;

	public ECLUpgradeListProc ()
	{
		self = this;
		EditorApplication.update += OnUpdate;
	}

	void OnUpdate ()
	{
		if (isFinishUpload) {
			isFinishUpload = false;
			//TODO:
			Debug.Log ("finished");
			EditorUtility.ClearProgressBar ();
			if (uploadResult) {
				// success
				updateState (selectedPackageName);
				uploadOss (selectedPackageName);
				EditorUtility.DisplayDialog ("Success", "Success !", "Okey");
			} else {
				EditorUtility.DisplayDialog ("Fail", "Failed !", "Okey");
			}
		}
		if (isUploading) {
			EditorUtility.DisplayProgressBar ("UpLoad", "Uploading....!", uploadProgerss);	
		}
	}

	void OnGUI ()
	{
		if (!ECLProjectSetting.isProjectExit (ECLProjectManager.self)) {
			GUIStyle style = new GUIStyle ();
			style.fontSize = 20;
			style.normal.textColor = Color.yellow;
			GUILayout.Label ("The scene is not ready, create it now?", style);
			if (GUILayout.Button ("Show Project Manager")) {
				EditorWindow.GetWindow<ECLProjectManager> (false, "CoolapeProject", true);
			}
			Close ();
			return;
		}

		if (ECLProjectManager.data.hotUpgradeServers.Count > 0) {
			ECLEditorUtl.BeginContents ();
			{
				List<string> toolbarNames = new List<string> ();
				for (int i = 0; i < ECLProjectManager.data.hotUpgradeServers.Count; i++) {
					HotUpgradeServerInfor dd = ECLProjectManager.data.hotUpgradeServers [i] as HotUpgradeServerInfor;
					toolbarNames.Add (dd.name);
				}
				selectedServerIndex = GUILayout.Toolbar (selectedServerIndex, toolbarNames.ToArray ());
				HotUpgradeServerInfor hsi = ECLProjectManager.data.hotUpgradeServers [selectedServerIndex] as HotUpgradeServerInfor;
				selectedServer = hsi;
//						ECLProjectSetting.cellServerInor (hsi, false);
				//===================================================
				GUILayout.BeginHorizontal ();
				{
					GUILayout.Label ("Key:", ECLEditorUtl.width200);
					GUILayout.TextField (selectedServer.key);
				}
				GUILayout.EndHorizontal ();
				//===================================================
				GUILayout.BeginHorizontal ();
				{
					GUILayout.Label ("Hot Upgrade Base Url:", ECLEditorUtl.width200);
					GUILayout.TextField (selectedServer.hotUpgradeBaseUrl);
				}
				GUILayout.EndHorizontal ();
				//===================================================
				GUILayout.BeginHorizontal ();
				{
					GUILayout.Label ("Host 4 Upload Upgrade Package:", ECLEditorUtl.width200);
					GUILayout.TextField (selectedServer.host4UploadUpgradePackage);
				}
				GUILayout.EndHorizontal ();

				GUILayout.BeginHorizontal ();
				{
					GUILayout.Label ("Port 4 Upload Upgrade Package:", ECLEditorUtl.width200);
					EditorGUILayout.IntField (selectedServer.port4UploadUpgradePackage);
				}
				GUILayout.EndHorizontal ();
			}
			ECLEditorUtl.EndContents ();
		}

		if (selectedServer == null) {
			GUILayout.Label ("Please select a server!");
			return;
		}

		EditorGUILayout.BeginHorizontal ();
		{
			GUI.color = Color.green;
			if (GUILayout.Button ("Refresh", GUILayout.Height (40f))) {
				setData ();
			}
			GUI.color = Color.white;
			if (!isSelectMod) {
				if (GUILayout.Button ("Save", GUILayout.Height (40f))) {
					if (mList == null || mList.Count == 0) {
						Debug.LogWarning ("Nothing need to save!");
						return;
					}
					string str = JSON.JsonEncode (mList);
					File.WriteAllText (Application.dataPath + "/" + cfgPath, str);
				}
			}
		}
		EditorGUILayout.EndHorizontal ();

		ECLEditorUtl.BeginContents ();
		{
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField ("Package Name", GUILayout.Width (160));
				EditorGUILayout.LabelField ("MD5", GUILayout.Width (250));
				EditorGUILayout.LabelField ("Exist?", GUILayout.Width (40));
				EditorGUILayout.LabelField ("Upload?", GUILayout.Width (60));
				EditorGUILayout.LabelField ("...", GUILayout.Width (60));
				EditorGUILayout.LabelField ("Notes");
			}
			EditorGUILayout.EndHorizontal ();
			if (mList == null) {
				return;
			}
			scrollPos = EditorGUILayout.BeginScrollView (scrollPos, GUILayout.Width (position.width), GUILayout.Height (position.height - 75));
			{
				for (int i = mList.Count - 1; i >= 0; i--) {
					item = ListEx.getMap (mList, i);

					EditorGUILayout.BeginHorizontal ();
					{
						EditorGUILayout.TextField (MapEx.getString (item, "name"), GUILayout.Width (160));
						EditorGUILayout.TextField (MapEx.getString (item, "md5"), GUILayout.Width (250));

						if (!MapEx.getBool (item, "exist")) {
							GUI.color = Color.red;
						}
						EditorGUILayout.TextField (MapEx.getBool (item, "exist") ? "Yes" : "No", GUILayout.Width (40));
						GUI.color = Color.white;
						if (!isUploaded (item)) {
							GUI.color = Color.red;
						}
						EditorGUILayout.TextField (isUploaded (item) ? "Yes" : "No", GUILayout.Width (60));
						GUI.color = Color.white;
						if (MapEx.getBool (item, "exist")) {
							GUI.enabled = true;
						} else {
							GUI.enabled = false;
						}
						GUI.color = Color.yellow;
						if (isSelectMod) {
							if (GUILayout.Button ("select", GUILayout.Width (60f))) {
								Close ();
								Utl.doCallback (onSelectedCallback, item, selectedCallbackParams);
							}
						} else {
							if (GUILayout.Button ("upload", GUILayout.Width (60f))) {
								if (EditorUtility.DisplayDialog ("Alert", "Really want to upload the upgrade package?", "Okey", "cancel")) {
									selectedPackageName = MapEx.getString (item, "name");
									uploadUpgradePackage (MapEx.getString (item, "name"));
								}	
							}
							if(GUILayout.Button("同步OSS", GUILayout.Width (60f))) {
								if (EditorUtility.DisplayDialog ("Alert", "Really want to upload the upgrade package?", "Okey", "cancel")) {
									uploadOss (MapEx.getString (item, "name"));
								}	
							}
						}

						GUI.color = Color.white;
						GUI.enabled = true;
						item ["remark"] = EditorGUILayout.TextArea (MapEx.getString (item, "remark"));

						GUILayout.Space (5);
					}
					EditorGUILayout.EndHorizontal ();
				}
			}
			EditorGUILayout.EndScrollView ();
		}
		ECLEditorUtl.EndContents ();
	}

	public bool isUploaded (Hashtable item)
	{
		if (selectedServer == null) {
			return false;
		}
		Hashtable m = MapEx.getMap (item, "upload");
		return MapEx.getBool (m, selectedServer.key);
	}

	public void uploadUpgradePackage (string name)
	{
		if (!Utl.netIsActived ()) {
			EditorUtility.DisplayDialog ("Alert", "The net work is not connected!", "Okay");
			return;
        }
        EditorUtility.ClearProgressBar();
        string localDir = getUpgradePackagePath (name);
		ThreadEx.exec (new System.Threading.ParameterizedThreadStart (doUploadUpgradePackage), localDir);
//		doUploadUpgradePackage (localDir);
	}

	void onSftpProgress (params object[] pars)
	{
		uploadProgerss = (float)(pars [0]);
	}

	void onftpFinish (params object[] pars)
	{
		isUploading = false;
		isFinishUpload = true;
		bool ret = (bool)(pars [0]);
		ECLUpgradeListProc.uploadResult = ret;
	}

	public void doUploadUpgradePackage (object localDir)
	{
		if (selectedServer == null) {
			Debug.LogError ("Please select a server!");
			return;
		}
		isUploading = true;
		if (selectedServer.useSFTP) {
            /*SFTPHelper sftp = new SFTPHelper (selectedServer.host4UploadUpgradePackage,
				                  selectedServer.port4UploadUpgradePackage,
				                  selectedServer.ftpUser, 
				                  selectedServer.ftpPassword);
			if (sftp.Connect ()) {
				if (selectedServer.isUploadOSSTemp) {
					sftp.PutDir (localDir.ToString (), selectedServer.RemoteBaseDir, (Callback)onSftpProgress, null);
					sftp.PutDir (localDir.ToString (), selectedServer.RemoteBaseDir + "_OSS", (Callback)onSftpProgress, (Callback)onftpFinish);
				} else {
					sftp.PutDir (localDir.ToString (), selectedServer.RemoteBaseDir, (Callback)onSftpProgress, (Callback)onftpFinish);
				}
				sftp.Exit ();
				sftp = null;
			} else {
				Utl.doCallback ((Callback)onftpFinish, false);
			}*/

            RenciSFTPHelper sftp = new RenciSFTPHelper(selectedServer.host4UploadUpgradePackage,
                                  selectedServer.ftpUser,
                                  selectedServer.ftpPassword);
            if (sftp.connect())
            {
                sftp.putDir(localDir.ToString(), selectedServer.RemoteBaseDir, (Callback)onSftpProgress, (Callback)onftpFinish);
                sftp.disConnect();
                sftp = null;
            }
            else
            {
                Utl.doCallback((Callback)onftpFinish, false);
            }
        } else {
			bool ret = FTP.UploadDir (localDir.ToString (), 
				           selectedServer.host4UploadUpgradePackage, 
				           selectedServer.ftpUser, 
				           selectedServer.ftpPassword, 
				           selectedServer.RemoteBaseDir, false);
			if (selectedServer.isUploadOSSTemp && ret) {
				ret = FTP.UploadDir (localDir.ToString (), 
					selectedServer.host4UploadUpgradePackage, 
					selectedServer.ftpUser, 
					selectedServer.ftpPassword, 
					selectedServer.RemoteBaseDir + "_OSS", false);
			}
			Utl.doCallback ((Callback)onftpFinish, ret);
		}
	}

	void uploadOss (string name = null)
	{
		if (string.IsNullOrEmpty (selectedServer.ossCmd)) {
			Debug.LogError ("请先设置同步脚本！");
			return;
		}
		if (string.IsNullOrEmpty (name)) {
			name = selectedPackageName;
		}
		string localDir = getUpgradePackagePath (name);
		string shell = Path.Combine(Application.dataPath,  ECLEditorUtl.getPathByObject( selectedServer.ossShell));
		string arg1 = Path.GetDirectoryName(shell);
		string arg2 = localDir.ToString ();
//		Debug.LogError (argss);
//		System.Diagnostics.Process process = System.Diagnostics.Process.Start ("/bin/bash", argss);
		//重新定向标准输入，输入，错误输出
//		process.StartInfo.RedirectStandardInput = true;
//		process.StartInfo.RedirectStandardOutput = true;
//		process.StartInfo.RedirectStandardError = true;
//
//		string ret = process.StandardOutput.ReadToEnd ();
//		Debug.Log (ret);

//		Debug.LogError (shell + " " + arg1 + " " + arg2);
		if ("MacOSX".Equals (SystemInfo.operatingSystemFamily.ToString())) {
			string argss = Path.Combine(Path.GetDirectoryName(shell), Path.GetFileNameWithoutExtension(shell)) +".sh" + " " + arg1 + " " + arg2;
//			Debug.LogError (argss);
			System.Diagnostics.Process process = System.Diagnostics.Process.Start ("/bin/bash", argss);
		} else {
			string batFile = Path.Combine (Path.GetDirectoryName (shell), Path.GetFileNameWithoutExtension (shell)) + ".bat";
			System.Diagnostics.Process.Start (batFile, arg1 + " " + arg2);
		}
		Debug.LogWarning("Finished===" + name);
	}

	public void setData ()
	{
		if (string.IsNullOrEmpty (cfgPath)) {
			cfgPath = ECLProjectManager.ver4UpgradeList;
		}
		string str = "";
		string p = Application.dataPath + "/" + cfgPath;
		if (File.Exists (p)) {
			str = File.ReadAllText (p); 
		}
		ArrayList list = JSON.DecodeList (str);
		list = list == null ? new ArrayList () : list;
		ECLUpgradeListProc.mList = list;
		refreshData ();
	}

	public void refreshData ()
	{
		if (mList == null)
			return;
		Hashtable item = null;

		for (int i = 0; i < mList.Count; i++) {
			item = ListEx.getMap (mList, i);
			if (Directory.Exists (getUpgradePackagePath (MapEx.getString (item, "name")))) {
				item ["exist"] = true;
			} else {
				item ["exist"] = false;
			}
		}
	}

	public void updateState (string name)
	{
		if (selectedServer == null) {
			return;
		}
		if (mList == null) {
			return;
		}
		Hashtable item = null;

		for (int i = 0; i < mList.Count; i++) {
			item = ListEx.getMap (mList, i);
			if (name.Equals (MapEx.getString (item, "name"))) {
//				item ["upload"] = true;
				Hashtable m = MapEx.getMap (item, "upload");
				m = m == null ? new Hashtable () : m;
				m [selectedServer.key] = true;
				item ["upload"] = m;
				break;
			}
		}

		string str = JSON.JsonEncode (mList);
		File.WriteAllText (Application.dataPath + "/" + cfgPath, str);
	}

	public string getUpgradePackageMd5 (string name)
	{
		string p = getUpgradePackagePath (name);
		p = PStr.b ().a (p).a ("/").a (CLPathCfg.self.basePath).a ("/resVer/").a (CLPathCfg.self.platform).e ();
		if (Directory.Exists (p)) {
			string[] files = Directory.GetFiles (p);
			string fileName = "";
			for (int i = 0; i < files.Length; i++) {
				fileName = Path.GetFileName (files [i]);
				if (fileName.StartsWith ("VerCtl.ver")) {
					return Utl.MD5Encrypt (File.ReadAllBytes (files [i]));
				}
			}
		}
		return "";
	}

	public string getUpgradePackagePath (string name)
	{
		string p = Path.Combine (Application.dataPath, name);
		if (Application.platform == RuntimePlatform.WindowsEditor) {
			p = p.Replace ("\\", "/");
			p = p.Replace ("//", "/");
			p = p.Replace ("/Assets/", "/Assets4Upgrade/");
		} else {
			p = p.Replace ("/Assets/", "/Assets4Upgrade/");
		}
//		p = Path.Combine (p, CLPathCfg.self.basePath);
		return p;
	}

	public static void show4UpgradeList (string cfgPath)
	{
		if (string.IsNullOrEmpty (cfgPath)) {
			cfgPath = ECLProjectManager.ver4UpgradeList;
		}
		isSelectMod = false;
		string str = "";
		string p = Application.dataPath + "/" + cfgPath;
		if (File.Exists (p)) {
			str = File.ReadAllText (p); 
		}
		ArrayList list = JSON.DecodeList (str);
		if (list == null || list.Count == 0) {
			EditorUtility.DisplayDialog ("Alert", "no data to show!", "Okay");
			return;
		}

		ECLUpgradeListProc window = ECLUpgradeListProc.GetWindow<ECLUpgradeListProc> (true, "Upgrade Res List", true);
		if (window == null) {
			window = new ECLUpgradeListProc ();
		}
		Vector2 size = Handles.GetMainGameViewSize ();
		Rect rect = window.position;
		rect.x = -Screen.width - Screen.width / 4;
		rect.y = Screen.height / 2 - Screen.height / 4;
		rect.width = Screen.width;
		rect.height = Screen.height / 2;

		rect = new Rect (10, 40, size.x, size.y / 2);
		window.position = rect;
		window.title = "Upgrade资源包列表";
		ECLUpgradeListProc.mList = list;
		window.refreshData ();
		ECLUpgradeListProc.cfgPath = cfgPath;
		// window.ShowPopup ();
		window.ShowAuxWindow ();
		
	}


	public static void popup4Select (Callback cb, object orgs)
	{
		show4UpgradeList (null);
		isSelectMod = true;
		onSelectedCallback = cb;
		selectedCallbackParams = orgs;
	}
}
