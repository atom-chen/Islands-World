using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Coolape;
using System.IO;
using System.Net;

public class ECLUpgradeBindingServer : EditorWindow
{
	Hashtable servers;
	Hashtable server;
	Vector2 scrollPos = Vector2.zero;
	ArrayList upgradePkgList;
	bool isSelectAll = false;
	static int selectedServerIndex = 0;
	HotUpgradeServerInfor selectedServer = null;

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

		if (ECLProjectManager.data.hotUpgradeServers.Count <= 0) {
			GUI.color = Color.red;
			GUILayout.Label ("There is no server");
			GUI.color = Color.white;
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
				int index = GUILayout.Toolbar (selectedServerIndex, toolbarNames.ToArray ());
				HotUpgradeServerInfor hsi = ECLProjectManager.data.hotUpgradeServers [index] as HotUpgradeServerInfor;
				selectedServer = hsi;

				if (selectedServerIndex != index) {
					selectedServerIndex = index;
					refreshData ();
				}
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
					GUILayout.Label ("URL of get server list:", ECLEditorUtl.width200);
					GUILayout.TextField (selectedServer.getServerListUrl);
				}
				GUILayout.EndHorizontal ();
				//===================================================
				GUILayout.BeginHorizontal ();
				{
					GUILayout.Label ("URL of set upgrade pkg md5:", ECLEditorUtl.width200);
					GUILayout.TextField (selectedServer.setServerPkgMd5Url);
				}
				GUILayout.EndHorizontal ();
			}
			ECLEditorUtl.EndContents ();
		}

		GUI.color = Color.green;
		if (GUILayout.Button ("Refresh")) {
			refreshData ();
		}
		GUI.color = Color.white;
		if (servers == null || servers.Count == 0)
			return;
		GUILayout.Space (5);
		EditorGUILayout.BeginHorizontal ();
		{
			if (GUILayout.Button ("All", GUILayout.Width (30))) {
				isSelectAll = !isSelectAll;
				foreach (DictionaryEntry cell in servers) {
					server = (Hashtable)(cell.Value);
					server ["selected"] = isSelectAll;
				}
			}
			EditorGUILayout.LabelField ("SID", GUILayout.Width (80));
			EditorGUILayout.LabelField ("SName", GUILayout.Width (100));
			GUI.color = Color.yellow;
#if UNITY_ANDROID
			EditorGUILayout.LabelField ("UpgradeMd5Ver(Android)", GUILayout.Width (250));
#elif UNITY_IPHONE || UNITY_IOS
			EditorGUILayout.LabelField ("UpgradeMd5Ver(ios)", GUILayout.Width (250));
#elif UNITY_STANDALONE_WIN
            EditorGUILayout.LabelField ("UpgradeMd5Ver(win)", GUILayout.Width (250));
#elif UNITY_STANDALONE_OSX
            EditorGUILayout.LabelField ("UpgradeMd5Ver(osx)", GUILayout.Width (250));
#endif
            EditorGUILayout.LabelField ("UpgradePkg Name", GUILayout.Width (160));
			EditorGUILayout.LabelField ("UpgradePkg Mark", GUILayout.Width (250));
			GUI.color = Color.white;
#if UNITY_ANDROID
			if (GUILayout.Button ("Select Md5(Android)")) {
				setUpgradePkgMutlMode ("Android");
            }
#elif UNITY_IPHONE || UNITY_IOS
            if (GUILayout.Button ("Select Md5(ios)")) {
                setUpgradePkgMutlMode ("ios");
            }
#elif UNITY_STANDALONE_WIN
            if (GUILayout.Button ("Select Md5(win)")) {
                setUpgradePkgMutlMode ("win");
            }
#elif UNITY_STANDALONE_OSX
            if (GUILayout.Button ("Select Md5(osx)")) {
                setUpgradePkgMutlMode ("osx");
            }
#endif
        }
        EditorGUILayout.EndHorizontal ();
		GUILayout.Space (5);
		ECLEditorUtl.BeginContents ();
		{
			scrollPos = EditorGUILayout.BeginScrollView (scrollPos, GUILayout.Width (position.width), GUILayout.Height (position.height - 50));
			{
				foreach (DictionaryEntry cell in servers) {
					server = (Hashtable)(cell.Value);
					EditorGUILayout.BeginHorizontal ();
					{
						server ["selected"] = EditorGUILayout.Toggle (MapEx.getBool (server, "selected"), GUILayout.Width (30));
						if (MapEx.getBool (server, "selected")) {
							GUI.color = Color.cyan;
						} else {
							GUI.color = Color.white;
						}
						EditorGUILayout.TextField (MapEx.getString (server, "idx"), GUILayout.Width (80));
						EditorGUILayout.TextField (MapEx.getString (server, "servername"), GUILayout.Width (100));
						GUI.color = Color.yellow;
#if UNITY_ANDROID
						EditorGUILayout.TextField (MapEx.getString (server, "androidversion"), GUILayout.Width (250));
#elif UNITY_IPHONE || UNITY_IOS
                        EditorGUILayout.TextField (MapEx.getString (server, "iosversion"), GUILayout.Width (250));
#elif UNITY_STANDALONE_WIN
                        EditorGUILayout.TextField (MapEx.getString (server, "winversion"), GUILayout.Width (250));
#elif UNITY_STANDALONE_OSX
                        EditorGUILayout.TextField (MapEx.getString (server, "osxversion"), GUILayout.Width (250));
#endif
                        EditorGUILayout.TextField (MapEx.getString (server, "pkgName"), GUILayout.Width (160));
						EditorGUILayout.TextArea (MapEx.getString (server, "pkgRemark"), GUILayout.Width (250));
						GUI.color = Color.white;
#if UNITY_ANDROID
						if (GUILayout.Button ("Select Md5(Android)")) {
							ECLUpgradeListProc.popup4Select ((Callback)onGetUpgradePkg, ListEx.builder ().Add (cell.Key).Add ("Android").ToList ());
                        }
#elif UNITY_IPHONE || UNITY_IOS
                        if (GUILayout.Button ("Select Md5(ios)")) {
                            ECLUpgradeListProc.popup4Select ((Callback)onGetUpgradePkg, ListEx.builder().Add(cell.Key).Add("ios").ToList());
                        }
#elif UNITY_STANDALONE_WIN
                        if (GUILayout.Button ("Select Md5(win)")) {
                            ECLUpgradeListProc.popup4Select ((Callback)onGetUpgradePkg, ListEx.builder().Add(cell.Key).Add("win").ToList());
                        }
#elif UNITY_STANDALONE_OSX
                        if (GUILayout.Button ("Select Md5(osx)")) {
                            ECLUpgradeListProc.popup4Select ((Callback)onGetUpgradePkg, ListEx.builder().Add(cell.Key).Add("osx").ToList());
                        }
#endif
                    }
                    EditorGUILayout.EndHorizontal ();
				}
			}
			EditorGUILayout.EndScrollView ();
		}
		ECLEditorUtl.EndContents ();
	}

	public void onGetUpgradePkg (params object[] paras)
	{
		string oldMd5 = "";
		Hashtable d = paras [0] as Hashtable;
		ArrayList orgsList = paras [1] as ArrayList;
		string key = orgsList [0] as string;
		string platform = orgsList [1] as string;

		Hashtable server = MapEx.getMap (servers, key);
		string verKey = "";
		string vetType = "1";
		if (platform.Equals ("ios")) {
			verKey = "iosversion";
			vetType = "1";
        } else if(platform.Equals("Android"))
        {
            verKey = "androidversion";
            vetType = "2";
        }
        else if (platform.Equals("win"))
        {
            verKey = "winversion";
            vetType = "3";
        }
        else if (platform.Equals("osx"))
        {
            verKey = "osxversion";
            vetType = "4";
        }
		oldMd5 = MapEx.getString (server, verKey);
		string newMd5 = MapEx.getString (d, "md5");
		if (!newMd5.Equals (oldMd5)) {
			if (EditorUtility.DisplayDialog ("Alert", "Really want to upgrade this server!!", "Okay", "Cancel")) {
				server [verKey] = newMd5;
				server ["pkgName"] = MapEx.getString (d, "name");
				server ["pkgRemark"] = MapEx.getString (d, "remark");
				servers [key] = server;
				saveData (MapEx.getString (server, "idx"), newMd5, vetType);
			}
		}
	}

	public void setUpgradePkgMutlMode (string platform)
	{
		bool canSetMd5 = false;
		foreach (DictionaryEntry cell in servers) {
			Hashtable server = cell.Value as Hashtable;
			if (MapEx.getBool (server, "selected")) {
				canSetMd5 = true;
				break;
			}
		}
		if (canSetMd5) {
			ECLUpgradeListProc.popup4Select ((Callback)onGetUpgradePkgMultMode, platform);
		} else {
			Debug.LogError ("Please select some servers!!");
		}
	}

	public void onGetUpgradePkgMultMode (params object[] paras)
	{
		Hashtable d = paras [0] as Hashtable;
		string platform = paras [1] as string;
		string oldMd5 = "";
		//
		if (EditorUtility.DisplayDialog ("Alert", "Really want to upgrade all selected servers!!", "Okay", "Cancel")) {
			foreach (DictionaryEntry cell in servers) {
				Hashtable server = cell.Value as Hashtable;
				if (MapEx.getBool (server, "selected")) {
					string verKey = "";
					string vetType = "1";
					if (platform.Equals ("ios")) {
						verKey = "iosversion";
						vetType = "1";
                    }
                    else if (platform.Equals("Android"))
                    {
                        verKey = "androidversion";
                        vetType = "2";
                    }
                    else if (platform.Equals("win"))
                    {
                        verKey = "winversion";
                        vetType = "3";
                    }
                    else if (platform.Equals("osx"))
                    {
                        verKey = "osxversion";
                        vetType = "4";
                    }
					oldMd5 = MapEx.getString (server, verKey);
					string newMd5 = MapEx.getString (d, "md5");
					if (!newMd5.Equals (oldMd5)) {
						server [verKey] = newMd5;
						server ["pkgName"] = MapEx.getString (d, "name");
						server ["pkgRemark"] = MapEx.getString (d, "remark");
						saveData (MapEx.getString (server, "idx"), newMd5, vetType);
					}
				}
			}
		}
	}

	public void getUpgradePkgListData ()
	{
		string	cfgPath = ECLProjectManager.ver4UpgradeList;
		string str = "";
		string p = Application.dataPath + "/" + cfgPath;
		if (File.Exists (p)) {
			str = File.ReadAllText (p); 
		}
		ArrayList list = JSON.DecodeList (str);
		upgradePkgList = list == null ? new ArrayList () : list;
	}

	Hashtable getUpgradePkgByMd5 (string md5)
	{
		if (upgradePkgList == null) {
			getUpgradePkgListData ();
		}
		Hashtable cell = null;
		for (int i = 0; i < upgradePkgList.Count; i++) {
			cell = (Hashtable)(upgradePkgList [i]);
			if (MapEx.getString (cell, "md5") == md5) {
				return cell;
			}
		}
		return null;
	}

	public void saveData (string serverID, string version, string verType)
	{
//		string __httpBaseUrl = PStr.b ().a ("http://").a (Net.self.gateHost).a (":").a (Net.self.gatePort).e ();
//		string url = PStr.b ().a (__httpBaseUrl).a ("/KokDirServer/UpdateVerServlet").e ();
		string url = selectedServer.setServerPkgMd5Url;
		Dictionary<string,object> paras = new Dictionary<string, object> ();
		paras ["serverid"] = serverID;
		paras ["version"] = version;
		paras ["versionType"] = verType;
		HttpWebResponse response = HttpEx.CreatePostHttpResponse (url, paras, 10000, System.Text.Encoding.UTF8);
		if (response == null)
			return;
		response.Close ();
	}

	public void refreshData ()
	{
		servers = null;
		getUpgradePkgListData ();
		if (selectedServer == null) {
			return;
		}
//		string __httpBaseUrl = PStr.b ().a ("http://").a (Net.self.gateHost).a (":").a (Net.self.gatePort).e ();
//		string url = PStr.b ().a (__httpBaseUrl).a ("/KokDirServer/ServerServlet").e ();
		string url = selectedServer.getServerListUrl;
		if (string.IsNullOrEmpty (url)) {
			return;
		}
		Dictionary<string,object> paras = new Dictionary<string, object> ();
		paras ["serverType"] = 1;
		HttpWebResponse response = HttpEx.CreatePostHttpResponse (url, paras, 10000, System.Text.Encoding.UTF8);
		if (response == null)
			return;
		string str = HttpEx.readString (response);
		response.Close ();
//		Debug.Log (url);
//		string str = HttpEx.readString (url, null);
		Debug.Log (str);
		servers = JSON.DecodeMap (str);
		Hashtable server = null;
		Hashtable pkg = null;
		if (servers != null) {
			foreach (DictionaryEntry cell in servers) {
				server = cell.Value as Hashtable;
				pkg = getUpgradePkgByMd5 (MapEx.getString (server, "version"));
				server ["pkgName"] = MapEx.getString (pkg, "name");
				server ["pkgRemark"] = MapEx.getString (pkg, "remark");
			}
		}
	}

	public static void show ()
	{
		ECLUpgradeBindingServer window = ECLUpgradeBindingServer.GetWindow<ECLUpgradeBindingServer> (true, "Server List", true);
		if (window == null) {
			window = new ECLUpgradeBindingServer ();
		}
		//		Vector2 size = Handles.GetMainGameViewSize ();
		Rect rect = window.position;
		rect.x = -Screen.width - Screen.width / 4;
		rect.y = Screen.height / 2 - Screen.height / 4;
		rect.width = Screen.width;
		rect.height = Screen.height / 2;

		//		rect = new Rect (-size.x/2, size.y / 2 - size.y / 4, size.x / 2, size.y / 2);
		window.position = rect;
		window.title = "Server List";
		window.refreshData ();
		window.ShowPopup ();
	}
}
