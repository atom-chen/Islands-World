using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Coolape;
using System.IO;

public class ECLLocalizeSelection : ScriptableWizard
{
	string newKey = "";
	//	string newContent = "";
	static public ECLLocalizeSelection instance;
	string editKey = "";
	string editVal = "";
	bool isShowList = false;

	void OnEnable ()
	{
		instance = this;
	}

	void OnDisable ()
	{
		instance = null;
	}

	public delegate void OnSlecteCallback (string key, string val);

	OnSlecteCallback mCallback;
	static Dictionary<string, ArrayList> dictOrgs = new Dictionary<string, ArrayList> ();
	static Dictionary<string, ArrayList> dict = new Dictionary<string, ArrayList> ();
	ArrayList languageList = new ArrayList ();
	Vector2 scrollPos = Vector2.zero;
	string searchKey = "";
	string oldSearchKey = "";

	public static void open (OnSlecteCallback callback)
	{
		if (instance != null) {
			instance.Close ();
			instance = null;
		}

		ECLLocalizeSelection comp = ScriptableWizard.DisplayWizard<ECLLocalizeSelection> ("Select a Localize");
		comp.mCallback = callback;
		comp.refreshContent ();
		comp.searchKey = "";
		comp.oldSearchKey = "";
	}

	void OnGUI ()
	{
		GUILayout.BeginHorizontal ();
		{
			if (GUILayout.Button ("Rfresh", GUILayout.Width (60))) {
				editKey = "";
				refreshContent ();
			}
			GUI.color = Color.green;
			if (GUILayout.Button ("Search", GUILayout.Width (60))) {
				search (searchKey);
				isShowList = true;
			}
			GUI.color = Color.yellow;
			searchKey = GUILayout.TextField (searchKey);
			GUI.color = Color.white;
			GUILayout.Label ("New Key:", GUILayout.Width (70));
			newKey = GUILayout.TextField (newKey, GUILayout.Width (120));
			GUI.color = Color.yellow;
			//			newContent = GUILayout.TextField (newContent);
			if (GUILayout.Button ("Add", GUILayout.Width (60))) {
				addVal (newKey, searchKey);
				newKey = "";
				isShowList = false;
			}
		}
		GUILayout.EndHorizontal ();
		GUI.color = Color.white;

//		if (searchKey != oldSearchKey) {
//			oldSearchKey = searchKey;
//			search (searchKey);
//		}

		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label ("Key", GUILayout.Width (120));
			for (int i = 0; i < languageList.Count; i++) {
				GUILayout.Label (languageList [i].ToString ());
			}
		}
		GUILayout.EndHorizontal ();

		scrollPos = EditorGUILayout.BeginScrollView (scrollPos, GUILayout.Width (position.width), GUILayout.Height (position.height - 65));
		{
			if (isShowList) {
				foreach (KeyValuePair<string ,ArrayList> cell in dict) {
					GUILayout.BeginHorizontal ();
					{
						GUILayout.TextField (cell.Key, GUILayout.Width (120));
						for (int i = 0; i < cell.Value.Count; i++) {
							if (editKey != cell.Key) {
								GUILayout.TextArea (cell.Value [i].ToString ());
								if (GUILayout.Button ("Edit", GUILayout.Width (50))) {
									editKey = cell.Key;
									editVal = cell.Value [i].ToString ();
								}
							} else {
								GUI.color = Color.green;
								editVal = GUILayout.TextArea (editVal);
								if (GUILayout.Button ("Modify", GUILayout.Width (50))) {
									addVal (editKey, editVal, true);
									editKey = "";
									editVal = "";
									GUI.color = Color.white;
									return;
								}
								GUI.color = Color.white;
							}
							if (GUILayout.Button ("Select", GUILayout.Width (60))) {
								if (mCallback != null) {
									mCallback (cell.Key, cell.Value [0].ToString ());
									Close ();
								}
							}
							if (GUILayout.Button ("-", GUILayout.Width (20))) {
								if (EditorUtility.DisplayDialog ("Alert", "Really want to delete?", "Okay", "Cancel")) {
									cutVal (cell.Key);
									return;
								}
							}
						}
					}
					GUILayout.EndHorizontal ();
				}
			}
		}
		EditorGUILayout.EndScrollView ();
	}

	void search (string searchKey)
	{
		if (string.IsNullOrEmpty (searchKey)) {
			dict = dictOrgs;
			return;
		}
		dict = new Dictionary<string, ArrayList> ();
		foreach (KeyValuePair<string, ArrayList> cell in dictOrgs) {
			if (cell.Key.ToUpper ().Contains (searchKey.ToUpper ())) {
				dict [cell.Key] = cell.Value;
				continue;
			}
			for (int i = 0; i < cell.Value.Count; i++) {
				if (cell.Value [i].ToString ().ToUpper ().Contains (searchKey.ToUpper ())) {
					dict [cell.Key] = cell.Value;
					break;
				}
			}
		}
	}

	void cutVal (string key)
	{
		string fileName = Application.dataPath + "/" + CLPathCfg.self.localizationPath + "Chinese.txt";
		fileName = fileName.Replace ("/upgradeRes/", "/upgradeRes4Dev/");
		Dictionary<string, string> tempDic = null;
		byte[] buff = File.ReadAllBytes (fileName);
		ByteReader reader = new ByteReader (buff);
		tempDic = reader.ReadDictionary ();
		if (!tempDic.ContainsKey (key))
			return;
		tempDic.Remove (key);

		PStr pstr = PStr.b ();
		foreach (KeyValuePair<string, string> cell in tempDic) {
			pstr.a (cell.Key).a ("=").a (cell.Value.Replace ("\n", "\\n")).a ("\n");
		}
		File.WriteAllText (fileName, pstr.e ());
		refreshContent ();
	}

	void addVal (string key, string val, bool isForce = false)
	{
		if (!string.IsNullOrEmpty (key) && !string.IsNullOrEmpty (val)) {
			if (dictOrgs.ContainsKey (key) && !isForce) {
				EditorUtility.DisplayDialog ("error", "The key is allready exist!", "Okay");
			} else {
				string fileName = Application.dataPath + "/" + CLPathCfg.self.localizationPath + "Chinese.txt";
				fileName = fileName.Replace ("/upgradeRes/", "/upgradeRes4Dev/");
				if (isForce) {
					Dictionary<string, string> tempDic = null;
					byte[] buff = File.ReadAllBytes (fileName);
					ByteReader reader = new ByteReader (buff);
					tempDic = reader.ReadDictionary ();
					tempDic [key] = val;

					PStr pstr = PStr.b ();
					foreach (KeyValuePair<string, string> cell in tempDic) {
						pstr.a (cell.Key).a ("=").a (cell.Value.Replace ("\n", "\\n")).a ("\n");
					}
					File.WriteAllText (fileName, pstr.e ());
				} else {
					File.AppendAllText (fileName, "\n" + key + "=" + val);
				}
				refreshContent ();
			}
		}
	}

	void refreshContent ()
	{
		editKey = "";
		languageList.Clear ();
		dictOrgs.Clear ();
		dict.Clear ();
		string dir = Application.dataPath + "/" + CLPathCfg.self.localizationPath;
		dir = dir.Replace ("/upgradeRes/", "/upgradeRes4Dev/");
		string[] files = Directory.GetFiles (dir);
		byte[] buff = null;
		Dictionary<string, string> tempDic = null;
		for (int i = 0; i < files.Length; i++) {
			if (ECLEditorUtl.isIgnoreFile (files [i]))
				continue;
			buff = File.ReadAllBytes (files [i]);
			languageList.Add (Path.GetFileNameWithoutExtension (files [i]));
			ByteReader reader = new ByteReader (buff);
			tempDic = reader.ReadDictionary ();
			foreach (KeyValuePair<string, string> cell in tempDic) {
				if (dictOrgs.ContainsKey (cell.Key.ToString ())) {
					ArrayList list = dictOrgs [cell.Key.ToString ()];
					if (list == null) {
						list = new ArrayList ();
					}
					list.Add (cell.Value);
					dictOrgs [cell.Key.ToString ()] = list;
				} else {
					ArrayList list = new ArrayList ();
					list.Add (cell.Value);
					dictOrgs [cell.Key.ToString ()] = list;
				}
			}
		}
		dict = dictOrgs;
	}
}
