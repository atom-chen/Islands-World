/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  酷猿菜单
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

static public class ECLCoolapeMenu
{
	const string rootName = "Coolape";
	const string toolesName = rootName + "/Tools";
	static int _index = 1;

	static int index {
		get {
			return _index;
		}
	}

	[MenuItem (rootName + "/ProjectManager", false, -9999)]
	static public void showProjectView ()
	{
		EditorWindow.GetWindow<ECLProjectManager> (false, "CoolapeProject", true);
	}

	[@MenuItem (toolesName + "/Asset Maker/Build AssetBundles Selection Files 4 Upgrade", false, 1)]
	static public void makeAssetBundles ()
	{
		ECLCreatAssetBundle4Update.makeAssetBundles ();
	}

	[@MenuItem (toolesName + "/Asset Maker/Build AssetBundles From Directory of Files 4 Upgrade", false, 2)]
	static public void makeAssetBundlesSelections ()
	{
		ECLCreatAssetBundle4Update.makeAssetBundlesSelections ();
	}

	[@MenuItem (toolesName + "/Asset Maker/Build AssetBundles Selection Files 4 Upgrade(Uncompressed)", false, 3)]
	static public void makeAssetBundlesUncompressed ()
	{
		ECLCreatAssetBundle4Update.makeAssetBundlesUncompressed ();
	}

	//	[MenuItem (toolesName + "/Publish Tool", false, 4)]
	//	static public void OpenBuilder ()
	//	{
	//		EditorWindow.GetWindow<ECLPublisher> (false, "CoolapePublisher", true);
	//	}

	[MenuItem (toolesName + "/AllPanels => AB", false, 6)]
	static public void uipanels2U3d ()
	{
		UnityEngine.Object[] objs = Selection.objects;
		int count = objs.Length;
		UnityEngine.Object obj = null;
		CLPanelLua panel = null;
		for (int i = 0; i < count; i++) {
			obj = objs [i];
			panel = ((GameObject)obj).GetComponent<CLPanelLua> ();
			if (panel != null) {
				Debug.LogError (obj.name);
				CLPanelLuaInspector.doSaveAsset (panel.gameObject);
			}
		}
	}

	[MenuItem (toolesName + "/LuaEncode/Encode selected Lua", false, 7)]
	static public void LuaEncodeSelected ()
	{
		ECLLuaEncodeTool.luajitEncode ();
	}

	[MenuItem (toolesName + "/LuaEncode/Encode selected Dir", false, 8)]
	static public void LuaEncodeAll ()
	{
		ECLLuaEncodeTool.luajitEncodeAll ();
	}

    [MenuItem(toolesName + "/Create CfgBioData FromJson", false, 8)]
    static public void createCfgBioDataFromJson()
    {
        ECLProjectManager.self.genCfgData();
    }

    [MenuItem (toolesName + "/GenerateSecondaryUVSet", false, 9)]
	static public void GenerateSecondaryUVSet ()
	{
		MeshFilter mf = null;
		foreach (UnityEngine.Object o in Selection.objects) {
			if (o is GameObject) {
				mf = ((GameObject)o).GetComponent<MeshFilter> ();
				if (mf != null && mf.mesh != null) {
					Unwrapping.GenerateSecondaryUVSet (mf.mesh);
					EditorUtility.SetDirty (o);
				}
			}
		}
	}

	[MenuItem (toolesName + "/DataProc/PrintMd5", false, 10)]
	static public void showFileContentMd5 ()
	{
		UnityEngine.Object obj = Selection.objects [0];
		string path = AssetDatabase.GetAssetPath (obj);//Selection表示你鼠标选择激活的对象
		Debug.Log (path);
		Debug.Log ("md5==[" + Utl.MD5Encrypt (File.ReadAllBytes (path)) + "]");
	}

	[MenuItem (toolesName + "/DataProc/PrintBioFile", false, 10)]
	static public void showBioFileContent ()
	{
		UnityEngine.Object obj = Selection.objects [0];
		string path = AssetDatabase.GetAssetPath (obj);//Selection表示你鼠标选择激活的对象
		object _obj = Utl.fileToObj (path);
		if (_obj is Hashtable) {
			Debug.Log (Utl.MapToString ((Hashtable)_obj));
		} else if (_obj.GetType () == typeof(NewList) ||
		           _obj.GetType () == typeof(ArrayList)) {
			Debug.Log (Utl.ArrayListToString2 ((ArrayList)_obj));
		} else {
			Debug.Log (_obj.ToString ());
		}
	}

	[MenuItem (toolesName + "/DataProc/Bio2Json", false, 11)]
	static public void Bio2Json ()
	{
		UnityEngine.Object[] objs = Selection.objects;
		int count = objs.Length;
		UnityEngine.Object obj = null;
		CLPanelLua panel = null;
		for (int i = 0; i < count; i++) {
			obj = objs [i];
			string path = AssetDatabase.GetAssetPath (obj);//Selection表示你鼠标选择激活的对象
			object map = Utl.fileToObj (path);
			string jsonStr = JSON.JsonEncode (map);
			Debug.Log (jsonStr);

			File.WriteAllText (path + ".json", jsonStr);
		}
	}

	[MenuItem (toolesName + "/DataProc/Json2Bio", false, 12)]
	static public void Json2Bio ()
	{
		UnityEngine.Object[] objs = Selection.objects;
		int count = objs.Length;
		UnityEngine.Object obj = null;
		CLPanelLua panel = null;
		for (int i = 0; i < count; i++) {
			obj = objs [i];
			string path = AssetDatabase.GetAssetPath (obj);//Selection表示你鼠标选择激活的对象
			string jsonStr = File.ReadAllText (path);
			object map = JSON.JsonDecode (jsonStr);
            
			MemoryStream ms = new MemoryStream ();
			B2OutputStream.writeObject (ms, map);
			Directory.CreateDirectory (Path.GetDirectoryName (path));
			File.WriteAllBytes (path + ".bio", ms.ToArray ());
		}
	}

	[MenuItem (toolesName + "/Lightmap/Save", false, 13)]
	static public void SaveLightmapInfo ()
	{
		CLPrefabLightmapDataEditor.SaveLightmapInfo ();
	}

	[MenuItem (toolesName + "/Lightmap/Load", false, 14)]
	static public void LoadLightmapInfo ()
	{
		CLPrefabLightmapDataEditor.LoadLightmapInfo ();
	}

	[MenuItem (toolesName + "/Lightmap/Clear", false, 15)]
	static public void ClearLightmapInfo ()
	{
		CLPrefabLightmapDataEditor.ClearLightmapInfo ();
	}

	[MenuItem (toolesName + "/Show Frame Dir List", false, 16)]
	static public void showDirList ()
	{
		string ret = "";
		string dir = Application.dataPath + "/" + ECLProjectManager.FrameName;
		ECLEditorUtl.getDirList (dir, "", ref ret);
		Debug.Log (ret);
	}

	[MenuItem (toolesName + "/Localize", false, 17)]
	static public void showLocalize ()
	{
		ECLLocalizeSelection.open (null);
	}

	[MenuItem (toolesName + "/setModleProp", false, 17)]
	static public void setModeProp ()
	{
		Object[] objs = Selection.objects;
		if (objs == null || objs.Length == 0)
			return;
		ModelImporter mi = null;
		string path = "";
		for (int j = 0; j < objs.Length; j++) {
			path = AssetDatabase.GetAssetPath (objs [j]);
//			mi = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath (objs[j])) as ModelImporter;
			ECLEditorUtl.setModelProp (path, false, ModelImporterNormals.None, ModelImporterTangents.None);
		}
	}

	[MenuItem (toolesName + "/check Modle Setting", false, 17)]
	static public void checkModleSetting ()
	{
		string pathRoot = "Assets/"+ CLPathCfg.self.basePath + "/";
	}

	[MenuItem (toolesName + "/Close Render shadow", false, 18)]
	static public void closeRenderShadow ()
	{
		Object[] objs = Selection.objects;
		if (objs == null || objs.Length == 0)
			return;
		GameObject go = null;
		for (int j = 0; j < objs.Length; j++) {
			go = objs [j] as GameObject;
			if (go == null)
				continue;
			Renderer[] rds = go.GetComponentsInChildren<Renderer> ();
			Renderer rd = null;
			for (int i = 0; i < rds.Length; i++) {
				rd = rds [i];
				rd.receiveShadows = false;
				rd.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			}

			EditorUtility.SetDirty (go);
		}
		AssetDatabase.SaveAssets ();
	}

	[MenuItem (toolesName + "/CleanMainCityText", false, 18)]
	static public void cleanMainCityText ()
	{
		GameObject go = Selection.activeObject as GameObject;
		TextMesh[] tms = go.GetComponentsInChildren<TextMesh> ();
		for (int i = 0; i < tms.Length; i++) {
			tms [i].font = null;
			Renderer mr = tms [i].GetComponent<Renderer> ();
			mr.material = null;
			mr.sharedMaterial = null;
			mr.materials = new Material[1];
			mr.sharedMaterials = new Material[1];
		}
		EditorUtility.SetDirty (go);
	}

	[MenuItem (toolesName + "/SetMainCityText", false, 19)]
	static public void setMainCityText ()
	{
		GameObject go = Selection.activeObject as GameObject;
		TextMesh[] tms = go.GetComponentsInChildren<TextMesh> ();
		for (int i = 0; i < tms.Length; i++) {
			tms [i].font = CLUIInit.self.emptFont.dynamicFont;
			Renderer mr = tms [i].GetComponent<Renderer> ();
			mr.sharedMaterial = tms [i].font.material;
		}
		EditorUtility.SetDirty (go);
	}

	[MenuItem (toolesName + "/UI/Sprite Packer", false, 20)]
	static public void showSpritePacker ()
	{
		EditorWindow.GetWindow<ECLSpritePacker> (false, "Sprite Packer", true);
	}

	[MenuItem (toolesName + "/Curve 2 Code", false, 21)]
	static public void curve2Code ()
	{
		EditorWindow.GetWindow<ECLCurve2Code> (false, "Curve->Code", true);
	}

	[MenuItem (toolesName + "/Retain one Material", false, 22)]
	static public void retainOneMaterial ()
	{
		GameObject go = Selection.activeObject as GameObject;
		if (go == null)
			return;

		Renderer[] mrs = go.GetComponentsInChildren<Renderer> ();
		for (int i = 0; i < mrs.Length; i++) {
			if (mrs [i].sharedMaterials != null && mrs [i].sharedMaterials.Length > 1) {
				Material[] mrs2 = new Material[1];
				mrs2 [0] = mrs [i].sharedMaterial;
				mrs [i].sharedMaterials = mrs2;
			}
		}
		EditorUtility.SetDirty (go);
	}

	[MenuItem (toolesName + "/Get Atlas And Lable", false, 23)]
	static public void getAtlasAndLable ()
	{
		Hashtable texs = new Hashtable ();
		Hashtable labs = new Hashtable ();
		GameObject[] gos = Selection.gameObjects;
		for (int i = 0; i < gos.Length; i++) {
			GameObject go = gos [i];
			Debug.LogError (go.name);
			dogetAtlasAndLable (go.transform, texs, labs);
		}
		string strs = "";
		foreach (DictionaryEntry cell in texs) {
			strs = strs + cell.Key + ",";
		}
		strs = strs + "\n";
		strs = strs + "\n";
		foreach (DictionaryEntry cell in labs) {
			strs = strs + cell.Key + ",";
		}
		Debug.LogError (strs);
	}

	static void dogetAtlasAndLable (Transform tr, Hashtable texs, Hashtable labs)
	{
		for (int i = 0; i < tr.childCount; i++) {
			UISprite sp = tr.GetChild (i).GetComponent<UISprite> ();
			if (sp != null) {
				texs [sp.spriteName] = 1;
			}
			UILabel lb = tr.GetChild (i).GetComponent<UILabel> ();
			if (lb != null) {
				labs [lb.text] = 1;
			}

			dogetAtlasAndLable (tr.GetChild (i), texs, labs);
		}
	}


	[MenuItem (toolesName + "/chg Atlas And Lable", false, 24)]
	static public void chgAtlasAndLable ()
	{
		GameObject go = Selection.activeGameObject;
		dochgAtlasAndLable (go.transform, "atlasMainUI", "fontMainUI");
	}

	static void dochgAtlasAndLable (Transform tr, string atlasName, string fontName)
	{
		for (int i = 0; i < tr.childCount; i++) {
			UISprite sp = tr.GetChild (i).GetComponent<UISprite> ();
			if (sp != null) {
				sp.atlasName = atlasName;
				sp.atlas = CLUIInit.self.getAtlasByName (atlasName);
			}
			UILabel lb = tr.GetChild (i).GetComponent<UILabel> ();
			if (lb != null) {
				lb.fontName = fontName;
				lb.bitmapFont = CLUIInit.self.getFontByName (fontName);
			}

			dochgAtlasAndLable (tr.GetChild (i), atlasName, fontName);
		}
	}


	[MenuItem (toolesName + "/texture setting", false, 25)]
	static public void setTexturesetting ()
	{
		Object[] gos = Selection.objects;	
		string path = "";
		List<string> refreshFiles = new List<string> ();
		for (int i = 0; i < gos.Length; i++) {
			path = AssetDatabase.GetAssetPath (gos [i]);
			if (ECLTextureSetting.setTexture (path)) {
				// 说明有重新设置
				refreshFiles.Add (path);
			}
		}

		for (int i = 0; i < refreshFiles.Count; i++) {
			ECLCreatAssetBundle4Update.createAssets4Upgrade (refreshFiles [i], true);
		}

		EditorUtility.DisplayDialog ("success", "Finished", "Okay");
	}

	[MenuItem (toolesName + "/Clean Cache", false, 999)]
	static public void cleanCache ()
	{
		Debug.Log (Application.persistentDataPath);
		FileUtil.DeleteFileOrDirectory (Application.persistentDataPath);
		PlayerPrefs.DeleteAll ();
	}

}
