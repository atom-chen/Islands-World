using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using Coolape;

public class ECLGUIResList : ScriptableWizard
{
	Vector2 scrollPos = Vector2.zero;
	Vector2 scrollPos2 = Vector2.zero;
	//	Rect windowRect = new Rect (0,0,Screen.width/2, Screen.height/2);
	public Callback callback;
	public Callback callback2;
	string rootPath = "";
	ArrayList datas = new ArrayList ();
	// FileInfor
	ArrayList selectedDatas = new ArrayList ();
	// FileInfor
	int totalFiles = 0;
	Hashtable resLastUpgradeVer = null;
	Hashtable necessaryResMap = new Hashtable ();
	string searchKey = "";

	void OnGUI ()
	{
		NGUIEditorTools.BeginContents ();
		EditorGUILayout.BeginHorizontal ();
		{
			searchKey = EditorGUILayout.TextField (searchKey);
			if (GUILayout.Button ("Search")) {
			}
			if (GUILayout.Button ("Refresh")) {
				searchKey = "";

			}
		}
		EditorGUILayout.EndHorizontal ();
		NGUIEditorTools.EndContents ();
		
		EditorGUILayout.BeginHorizontal ();
		{
			scrollPos = EditorGUILayout.BeginScrollView (scrollPos, GUILayout.Width (position.width / 2), GUILayout.Height (position.height - 80));
			{
				int count = datas.Count;
				for (int i = 0; i < count; i++) {
					showOneFile ((ECLResInfor)(datas [i]), true, true);
				}
			}
			EditorGUILayout.EndScrollView ();
		
			scrollPos2 = EditorGUILayout.BeginScrollView (scrollPos2, GUILayout.Width (position.width / 2), GUILayout.Height (position.height - 80));
			{
				getSelectedFiles ();
				int count = selectedDatas.Count;
				for (int i = 0; i < count; i++) {
					showOneFile ((ECLResInfor)(selectedDatas [i]), false, false);
				}
			}
			EditorGUILayout.EndScrollView ();
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		{
			GUI.color = Color.yellow;
			EditorGUILayout.LabelField ("Total Files:" + totalFiles);
			EditorGUILayout.LabelField ("Selected Files:" + selectedDatas.Count);
			GUI.color = Color.white;
		}
		EditorGUILayout.EndHorizontal ();

		NGUIEditorTools.BeginContents ();
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Okay")) {
			this.Close ();
			if (callback != null) {
				callback (getSelectedFiles ());
			}
		}
		GUI.color = Color.yellow;
		if (GUILayout.Button ("Cancel")) {
			this.Close ();
			if (callback2 != null) {
				callback2 ();
			}
		}
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal ();
		NGUIEditorTools.EndContents ();
	}

	ArrayList getSelectedFiles ()
	{
		selectedDatas.Clear ();
		ECLResInfor ri = null;
		int count = datas.Count;
		for (int i = 0; i < count; i++) {
			ri = (ECLResInfor)(datas [i]);
			if (ri.selected && !ri.isDir) {
				selectedDatas.Add (ri);
			}
		}
		return selectedDatas;
	}

	void  initData (string path, int tabs)
	{
		//跳过不同平台的资源
#if UNITY_ANDROID
        if (Path.GetFileName(path).Equals("IOS") || Path.GetFileName(path).Equals("Standalone") || Path.GetFileName(path).Equals("StandaloneOSX"))
        {
            return;
        }
#elif UNITY_IOS
            if(Path.GetFileName(path).Equals("Android") || Path.GetFileName(path).Equals("Standalone") || Path.GetFileName(path).Equals("StandaloneOSX")) {
                return;
            }
#elif UNITY_STANDALONE_WIN
            if(Path.GetFileName(path).Equals("Android") || Path.GetFileName(path).Equals("IOS") || Path.GetFileName(path).Equals("StandaloneOSX")) {
                return;
            }
#elif UNITY_STANDALONE_OSX
            if(Path.GetFileName(path).Equals("Android") || Path.GetFileName(path).Equals("IOS") || Path.GetFileName(path).Equals("Standalone")) {
                return;
            }
#endif

        ECLResInfor ri = new ECLResInfor ();
		path = path.Replace ("\\", "/");
		ri.path = path;
		ri.name = Path.GetFileName (path);
		ri.isDir = true;
		ri.selected = true;
		ri.tabs = tabs;
		ri.relativePath = ri.path.Replace (Application.dataPath + "/", "");
		ri.publishPath = Utl.filterPath (ri.relativePath);
		ri.ver = "";
		datas.Add (ri);

		string[] fileEntries = Directory.GetFiles (path);//因为Application.dataPath得到的是型如 "工程名称/Assets"
		int count = fileEntries.Length;
		string fileName = "";
		string extension = "";
		for (int i = 0; i < count; i++) {
			fileName = fileEntries [i];
			extension = Path.GetExtension (fileName);
			if (ECLEditorUtl.isIgnoreFile (fileName)) {
				continue;
			}
			totalFiles++;
			ri = new ECLResInfor ();
			fileName = fileName.Replace ("\\", "/");
			ri.path = fileName;
			ri.name = Path.GetFileName (fileName);
			ri.isDir = false;
			ri.selected = true;
			ri.tabs = tabs + 1;
			ri.relativePath = ri.path.Replace (Application.dataPath + "/", "");
			ri.publishPath = Utl.filterPath (ri.relativePath);
			ri.ver = Utl.MD5Encrypt (File.ReadAllBytes (fileName));//MapEx.getString (resLastUpgradeVer, ri.publishPath);
			datas.Add (ri);
			selectedDatas.Add (ri);
		}
		
		string[] dirEntries = Directory.GetDirectories (path);
		count = dirEntries.Length;
		string dir = "";
		for (int i = 0; i < count; i++) {
			initData (dirEntries [i], tabs + 1);
		}
	}

	void showOneFile (ECLResInfor ri, bool canEdite, bool needFilter)
	{
		if (needFilter && searchKey != "") {
			if (!ri.path.ToLower ().Contains (searchKey.ToLower ())) {
				return;
			}
		}

		string fname = ri.relativePath;
		EditorGUILayout.BeginHorizontal ();
		{
			if (canEdite) {
				bool selected = GUILayout.Toggle (ri.selected, "", GUILayout.Width (20));
				if (selected != ri.selected) {
					ri.selected = selected;
					if (ri.isDir) {
						ArrayList list = new ArrayList ();
						list.AddRange (datas);
						int count = list.Count;
						ECLResInfor res = null;
						for (int i = 0; i < count; i++) {
							res = (ECLResInfor)(list [i]);
							if (res.path.Contains (ri.path)) {
								res.selected = selected;
							}
						}
					}
				}
			}
			//			EditorGUILayout.LabelField ("", GUILayout.Width (ri.tabs * 4));
//		EditorGUILayout.SelectableLabel (StrEx.appendSpce ("v." + ri.ver, 6) + fname, GUILayout.Height (18));
			EditorGUILayout.SelectableLabel (StrEx.appendSpce ("", 6) + fname, GUILayout.Height (18));

		}
		EditorGUILayout.EndHorizontal ();
	}

	public static void show (string rootPath, Callback callback, Callback callback2)
	{
		ECLGUIResList window = ScriptableWizard.DisplayWizard<ECLGUIResList> ("Res List");
//		if (window == null) {
//			window = new ECLGUIResList ();
//		}
//		window.position = new Rect (Screen.width / 2 - Screen.width / 4, Screen.height / 2 - Screen.height / 4, Screen.width / 2, Screen.height / 2);
		window.title = "资源列表";
		window.rootPath = rootPath;
		window.callback = callback;
		window.callback2 = callback2;
		window.totalFiles = 0;

		window.datas.Clear ();
		window.selectedDatas.Clear ();

		window.resLastUpgradeVer = ECLProjectManager.getLastUpgradeVer ();
		window.initData (rootPath, 0);
		window.Show ();
	}

	public static void show4PublishSeting (string rootPath, Callback callback, Callback callback2)
	{
		show4PublishSeting (rootPath, null, callback, callback2);
	}

	public static void show4PublishSeting (string rootPath, string publishConfig, Callback callback, Callback callback2)
	{
		ECLGUIResList window = ScriptableWizard.DisplayWizard<ECLGUIResList> ("Res List");
//		Rect rect = window.position;
//		//		rect = new Rect (-Screen.width/2, Screen.height / 2 - Screen.height / 4, Screen.width / 2, Screen.height / 2);
//		rect.x = -Screen.width - Screen.width / 4;
//		rect.y = Screen.height / 2 - Screen.height / 4;
//		rect.width = Screen.width / 2;
//		rect.height = Screen.height / 2;
//		window.position = rect;

		window.title = "资源列表";
		window.rootPath = rootPath;
		window.callback = callback;
		window.callback2 = callback2;
		window.totalFiles = 0;
		
		window.datas.Clear ();
		window.selectedDatas.Clear ();
		
		window.resLastUpgradeVer = ECLProjectManager.getLastUpgradeVer ();
		window.initData (rootPath, 0);
		window.init4PublishSetting (publishConfig);
//		window.ShowPopup ();
		window.Show ();
	}

	public static void show4Upgrade (string rootPath, Callback callback, Callback callback2)
	{
		ECLGUIResList window = ScriptableWizard.DisplayWizard<ECLGUIResList> ("Res List");

//		Rect rect = window.position;
//		rect.x = -Screen.width - Screen.width / 4;
//		rect.y = Screen.height / 2 - Screen.height / 4;
//		rect.width = Screen.width / 2;
//		rect.height = Screen.height / 2;

//		Vector2 size = Handles.GetMainGameViewSize ();
//		rect = new Rect (-size.x/2, size.y / 2 - size.y / 4, size.x / 2, size.y / 2);
//		window.position = rect;

		window.title = "Upgrade资源列表";
		window.rootPath = rootPath;
		window.callback = callback;
		window.callback2 = callback2;
		window.totalFiles = 0;
		
		window.datas.Clear ();
		window.selectedDatas.Clear ();
		
		window.resLastUpgradeVer = ECLProjectManager.getLastUpgradeVer ();
		window.initData (rootPath, 0);
		window.init4Upgrade4Publish ();

		window.Show ();
	}

	void init4Upgrade4Publish ()
	{
		AssetDatabase.Refresh ();		
		Hashtable tmpResVer = ECLCreateVerCfg.create2Map ("Assets/" + ECLProjectManager.data.name + "/upgradeRes4Publish");
		Hashtable lastResVer = ECLProjectManager.getLastUpgradeMd5Ver ();
		string key = "";
		ECLResInfor ri = null;
		Hashtable needUpgradeRes = new Hashtable ();
		foreach (DictionaryEntry cell in tmpResVer) {
			key = cell.Key.ToString ().Replace ("/upgradeRes/", "/upgradeRes4Publish/");
			if (lastResVer [key] == null || string.Compare (cell.Value.ToString (), lastResVer [key].ToString ()) != 0) {
				needUpgradeRes [key] = true;
			}
		}

		int count = datas.Count;
		for (int i = 0; i < count; i++) {
			ri = (ECLResInfor)(datas [i]);
			if (needUpgradeRes [ri.relativePath] != null) {
				ri.selected = true;
			} else {
				ri.selected = false;
			}
		}
	}

	void init4PublishSetting (string oldPublishPath)
	{
		AssetDatabase.Refresh ();
		string publishVerPath = "";
		if (string.IsNullOrEmpty (oldPublishPath)) {
			publishVerPath = Application.dataPath + "/" + ECLProjectManager.ver4Publish;
		} else {
			publishVerPath = oldPublishPath;
		}
		Hashtable oldSettingCfg = ECLProjectManager.fileToMap (publishVerPath);
		string key = "";
		ECLResInfor ri = null;
		Hashtable needUpgradeRes = new Hashtable ();
		foreach (DictionaryEntry cell in oldSettingCfg) {
			key = cell.Key.ToString ().Replace ("/upgradeRes/", "/upgradeRes4Publish/");
			needUpgradeRes [key] = true;
		}
		
		int count = datas.Count;
		for (int i = 0; i < count; i++) {
			ri = (ECLResInfor)(datas [i]);
			if (needUpgradeRes [ri.relativePath] != null) {
				ri.selected = true;
			} else {
				ri.selected = false;
			}
		}
	}


}

public class ECLResInfor
{
	public string path = "";
	public string name = "";
	public string relativePath = "";
	public string publishPath = "";
	public int tabs = 0;
	public string ver = "";
	public bool isDir = false;
	public bool selected = false;
}
