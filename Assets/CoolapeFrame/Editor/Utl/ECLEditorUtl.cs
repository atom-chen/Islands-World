using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using Coolape;
using System.Collections.Generic;
using UnityEditorInternal;

public static class ECLEditorUtl
{
	public static GUILayoutOption width30 = GUILayout.Width (30);
	public static GUILayoutOption width50 = GUILayout.Width (50);
	public static GUILayoutOption width80 = GUILayout.Width (80);
	public static GUILayoutOption width100 = GUILayout.Width (100);
	public static GUILayoutOption width120 = GUILayout.Width (120);
	public static GUILayoutOption width150 = GUILayout.Width (150);
	public static GUILayoutOption width200 = GUILayout.Width (200);
	public static GUILayoutOption width250 = GUILayout.Width (250);
	public static GUILayoutOption width300 = GUILayout.Width (300);
	public static GUILayoutOption width400 = GUILayout.Width (400);
	public static GUILayoutOption width500 = GUILayout.Width (500);

	/// <summary>
	/// Gets the path by object.取得工程对象的路径，但不包含Assets;
	/// </summary>
	/// <returns>The path by object.</returns>
	/// <param name="obj">Object.</param>
	public static string getPathByObject (Object obj)
	{
		if (obj == null)
			return "";
		string tmpPath = AssetDatabase.GetAssetPath (obj.GetInstanceID ());
		if (string.IsNullOrEmpty (tmpPath)) {
			Debug.LogError ("Cannot get path! [obj name]==" + obj.name);
			return "";
		}
		int startPos = 0;

		startPos = tmpPath.IndexOf ("Assets/");
		startPos += 7;
		tmpPath = tmpPath.Substring (startPos, tmpPath.Length - startPos);
		return  tmpPath;
	}

	/// <summary>
	/// Gets the object by path.
	/// </summary>
	/// <returns>The object by path.</returns>
	/// <param name="path">Path.</param>
	public static Object getObjectByPath (string path)
	{
		string tmpPath = path;
		if (!tmpPath.StartsWith ("Assets/")) {
			tmpPath = PStr.b ().a ("Assets/").a (tmpPath).e ();
		}
		return AssetDatabase.LoadAssetAtPath (
			tmpPath, typeof(UnityEngine.Object));
	}

	static public void BeginContents ()
	{
		GUILayout.BeginHorizontal ();
		EditorGUILayout.BeginHorizontal (NGUIEditorTools.textArea, GUILayout.MinHeight (10f));
		GUILayout.BeginVertical ();
		GUILayout.Space (2f);
	}

	/// <summary>
	/// End drawing the content area.
	/// </summary>
	static public void EndContents ()
	{
		GUILayout.Space (3f);
		GUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();
		
		GUILayout.Space (3f);
		GUILayout.EndHorizontal ();
		GUILayout.Space (3f);
	}

	/// <summary>
	/// Ises the ignore file.是否需要忽略的文件
	/// </summary>
	/// <returns><c>true</c>, if ignore file was ised, <c>false</c> otherwise.</returns>
	/// <param name="filePath">File path.</param>
	static public bool isIgnoreFile (string filePath)
	{
		if (ECLProjectManager.data == null
		    || string.IsNullOrEmpty (filePath)
		    || string.IsNullOrEmpty (ECLProjectManager.data.ingoreResWithExtensionNames)) {
			return false;
		}
		string extension = Path.GetExtension (filePath).ToLower ();
		string extensionList = ECLProjectManager.data.ingoreResWithExtensionNames.ToLower ();

		if (!string.IsNullOrEmpty (extension) && extensionList.Contains (extension)) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Moves the asset4 upgrade.把想关引用的asset移动对应的目录，以便可以支持热更新
	/// </summary>
	/// <param name="obj">Object.</param>
	static public bool moveAsset4Upgrade (Object obj)
	{
		string objPath = getPathByObject (obj);
        objPath = objPath.Replace("\\", "/");
        if (objPath.Contains ("/upgradeRes4Dev/")) {
			return false;
		}

		string toPathBase = CLPathCfg.self.basePath + "/upgradeRes4Dev/other/";
		if (obj is Material) {
			toPathBase = toPathBase + "Materials/";
		} else if (obj is Texture) {
			toPathBase = toPathBase + "Textures/";
		} else if (obj is CLUnit) {
			toPathBase = toPathBase + "roles/";
		} else if (obj is CLBulletBase) {
			toPathBase = toPathBase + "bullet/";
		} else if (obj is CLEffect) {
			toPathBase = toPathBase + "effect/";
		} else if (obj is AudioClip) {
			toPathBase = toPathBase + "sound/";
		} else if (obj is Mesh || obj is Avatar) {
			toPathBase = toPathBase + "model/";
		} else {
			toPathBase = toPathBase + "things/";
		}
		string toPath = "";
		if (objPath.StartsWith (CLPathCfg.self.basePath + "/xRes/")) {
			toPath = toPathBase + objPath.Replace (CLPathCfg.self.basePath + "/xRes/", "");
		} else if (objPath.StartsWith (CLPathCfg.self.basePath + "/")) {
			toPath = toPathBase + objPath.Replace (CLPathCfg.self.basePath + "/", "");
		} else if (objPath.Contains ("/Resources/")) {
			toPath = toPathBase + objPath.Replace ("/Resources/", "/Res/");
		} else {
			toPath = toPathBase + objPath;
		}
//		Debug.Log ("111=======Assets/" + objPath);
//		Debug.Log ("222=======Assets/" + toPath);
		Directory.CreateDirectory (Path.GetDirectoryName (Application.dataPath + "/" + toPath));
		AssetDatabase.Refresh ();
		string ret = AssetDatabase.MoveAsset ("Assets/" + objPath, "Assets/" + toPath);
		if (!string.IsNullOrEmpty (ret)) {
			Debug.LogError (ret);
			return false;
		} else {
			AssetDatabase.Refresh ();
		}
		return true;
	}

	/// <summary>
	/// Gets the file name4 upgrade.取得asset的名字，给资源管理用
	/// </summary>
	/// <returns>The file name4 upgrade.</returns>
	/// <param name="obj">Object.</param>
	static public string getAssetName4Upgrade (Object obj)
	{
		string objPath = getPathByObject (obj);
//		Debug.Log ("objPath===" + objPath);
		string objName = Path.GetFileNameWithoutExtension (objPath);
		string basePath = Path.GetDirectoryName (objPath) + "/";
        basePath = basePath.Replace("\\", "/");
//		Debug.Log (objName);
//		Debug.Log (basePath);
		string replacePath = CLPathCfg.self.basePath + "/upgradeRes4Dev/other/";
		if (obj is Material) {
			replacePath = replacePath + "Materials/";
		} else if (obj is Texture) {
			replacePath = replacePath + "Textures/";
		} else if (obj is CLUnit) {
			replacePath = replacePath + "roles/";
		} else if (obj is CLBulletBase) {
			replacePath = replacePath + "bullet/";
		} else if (obj is CLEffect) {
			replacePath = replacePath + "effect/";
		} else if (obj is AudioClip) {
			replacePath = replacePath + "sound/";
		} else if (obj is Mesh || obj is Avatar) {
			replacePath = replacePath + "model/";
		} else {
			replacePath = replacePath + "things/";
		}

//		Debug.Log ("replacePath===" + replacePath);
		objName = basePath.Replace (replacePath, "");
		if (objName != "") {
			objName = Path.Combine (objName, Path.GetFileNameWithoutExtension (objPath));
		} else {
			objName = Path.GetFileNameWithoutExtension (objPath);
		}
        objName = objName.Replace("\\", "/");
        objName = objName.Replace ("/", ".");
//		Debug.Log ("objName===" + objName);
		return objName;
	}

	/// <summary>
	/// Get all textures from a material
	/// </summary>
	/// <returns>The textures from material.</returns>
	/// <param name="mat">Mat.</param>
	public static bool getTexturesFromMaterial (Material mat, ref ArrayList propNames, ref ArrayList texNames, ref ArrayList texPaths)
	{
		bool ret = false;
		if (mat == null) {
			Debug.LogWarning ("The mat is null");
			return ret;
		}
		Shader shader = mat.shader;
		string propName = "";
		for (int i = 0; i < ShaderUtil.GetPropertyCount (shader); i++) {
			if (ShaderUtil.GetPropertyType (shader, i) == ShaderUtil.ShaderPropertyType.TexEnv) {
				propName = ShaderUtil.GetPropertyName (shader, i);
				Texture texture = mat.GetTexture (propName);
				if (texture != null) {
					ret = ECLEditorUtl.moveAsset4Upgrade (texture) || ret ? true : false;
					propNames.Add (propName);
					texNames.Add (ECLEditorUtl.getAssetName4Upgrade (texture));
					texPaths.Add (ECLEditorUtl.getPathByObject (texture));
				}
			}
		}
		return ret;
	}

	/// <summary>
	/// Gets the dir list.取得目录列表
	/// </summary>
	/// <param name="dir">Dir.</param>
	/// <param name="left">Left.</param>
	/// <param name="result">Result.</param>
	static public void getDirList (string dir, string left, ref string result)
	{
		result = string.IsNullOrEmpty (result) ? "" : result;
		/*
		─━│┃┄┅┆┇┈┉┊┋┌
		┍┎┏┐┑┒┓└┕┖
		┗┘┙┚┛├┝┞┟
		┠┡┢┣┤┥┦┧┨
		┩┪┫┬┭┮┯┰
		┱┲┳┴┵┶┷┸┹
		┺┻┼┽┾┿╀╁╂
		╃╄╅╆╇╈╉╊╋═║
		╒╓╔╕╖╗╘╙
		╚╛╜╝╞╟╠╡
		╢╣╤╥╦╧╨╩
		╪╫╬╳╔ ╗╝╚
		╬ ═ ╓ ╩ ┠ ┨┯
		┷┏ ┓┗ ┛┳⊥﹃﹄┌╭╮╯╰
		*/
		result = result + left + dir.Replace (Application.dataPath + "/", " ") + "     \n";
		string[] dirs = Directory.GetDirectories (dir);
		string left2 = "";
		if (dirs.Length > 0 && left.Length > 0) {
			char[] chars = left.ToCharArray ();
			if (chars [chars.Length - 1] == '┖') {
				chars [chars.Length - 1] = '　';	
			} else {
				chars [chars.Length - 1] = '┃';
			}
			left2 = new string (chars);
		}
		if (dirs.Length > 0) {
			for (int i = 0; i < dirs.Length; i++) {
				if (i == dirs.Length - 1) {
					getDirList (dirs [i], left2 + "┖", ref result);
				} else {
					getDirList (dirs [i], left2 + "┠", ref result);
				}
			}
		}
	}

	public static bool SaveRenderTextureToPNG (Texture inputTex, Shader outputShader, string contents, string pngName)
	{  
		RenderTexture temp = RenderTexture.GetTemporary (inputTex.width, inputTex.height, 0, RenderTextureFormat.ARGB32);  
		Material mat = new Material (outputShader);  
		Graphics.Blit (inputTex, temp, mat);  
		bool ret = SaveRenderTextureToPNG (temp, contents, pngName);  
		RenderTexture.ReleaseTemporary (temp);  
		return ret;  
	}

	//将RenderTexture保存成一张png图片
	public static bool SaveRenderTextureToPNG (RenderTexture rt, string contents, string pngName)
	{  
		RenderTexture prev = RenderTexture.active;  
		RenderTexture.active = rt;  
		Texture2D png = new Texture2D (rt.width, rt.height, TextureFormat.ARGB32, false);  
		png.ReadPixels (new Rect (0, 0, rt.width, rt.height), 0, 0);  
		byte[] bytes = png.EncodeToPNG ();  
		if (!Directory.Exists (contents))
			Directory.CreateDirectory (contents);  
		FileStream file = File.Open (contents + "/" + pngName + ".png", FileMode.Create);  
		BinaryWriter writer = new BinaryWriter (file);  
		writer.Write (bytes);  
		file.Close ();  
		Texture2D.DestroyImmediate (png);  
		png = null;  
		RenderTexture.active = prev;  
		return true;  
	}

	public static void setModelProp (ModelImporter mi, bool isReadable, ModelImporterNormals modelNormals, ModelImporterTangents modelTangents)
	{
		if (mi != null) {
			mi.importMaterials = false;
			mi.isReadable = isReadable;
			mi.importNormals = modelNormals;
			mi.importTangents = modelTangents;
			AssetDatabase.Refresh ();
		}
	}

	public static void setModelProp (string modelName, bool isReadable, ModelImporterNormals modelNormals, ModelImporterTangents modelTangents)
	{
		string matPath = PStr.b ().a ("Assets/").a (CLPathCfg.self.basePath).a ("/")
			.a ("upgradeRes4Dev").a ("/other/model/").a (modelName.Replace (".", "/")).a (".FBX").e ();

		ModelImporter mi = ModelImporter.GetAtPath (matPath) as ModelImporter;
		setModelProp (mi, isReadable, modelNormals, modelTangents);
		doCleanModelMaterials (matPath);
	}

	public static void cleanModleMaterials (ModelImporter mi)
	{
		if (mi != null) {
			mi.importMaterials = false;
			AssetDatabase.Refresh ();
		}
	}

	public static void cleanModleMaterials (string modelName)
	{
		string matPath = PStr.b ().a ("Assets/").a (CLPathCfg.self.basePath).a ("/")
			.a ("upgradeRes4Dev").a ("/other/model/").a (modelName.Replace (".", "/")).a (".FBX").e ();
		doCleanModelMaterials (matPath);
	}

	public static void doCleanModelMaterials (string matPath)
	{
		checkModleSetting (matPath);
		ModelImporter mi = ModelImporter.GetAtPath (matPath) as ModelImporter;
		if (mi != null) {
			cleanModleMaterials (mi);
			AssetDatabase.ImportAsset (matPath);
		}

		GameObject go = ECLEditorUtl.getObjectByPath (matPath) as GameObject;
		if (go != null) {
			MeshRenderer mf = go.GetComponentInChildren<MeshRenderer> ();
			if (mf != null) {
				mf.sharedMaterial = null;
				Material[] mats = mf.sharedMaterials;
				for (int i = 0; i < mats.Length; i++) {
					mats [i] = null;
				}
				mf.sharedMaterials = mats;
			}
			SkinnedMeshRenderer smr = go.GetComponentInChildren<SkinnedMeshRenderer> ();
			if (smr != null) {
				smr.sharedMaterial = null;
				Material[] mats = smr.sharedMaterials;
				for (int i = 0; i < mats.Length; i++) {
					mats [i] = null;
				}
				smr.sharedMaterials = mats;
			}
			EditorUtility.SetDirty (go);
			AssetDatabase.WriteImportSettingsIfDirty (matPath);
			AssetDatabase.Refresh ();
		}
	}

	public static string checkModleSetting (string path)
	{
		string ret = "";
		ModelImporter mi = ModelImporter.GetAtPath (path) as ModelImporter;
		if (mi != null) {
			if (mi.isReadable) {
				ret = PStr.b ().a (ret).a ("can reade write! ").e ();
			}
			if (mi.importMaterials) {
				ret = PStr.b ().a (ret).a ("import Materials! ").e ();
			}
			if (mi.importNormals != ModelImporterNormals.None) {
				ret = PStr.b ().a (ret).a ("import Normals! ").e ();
			}
		}
		Debug.LogError (ret);
		return ret;
	}

    public static LayerMask drawMaskField(string text, LayerMask mask)
    {
        return drawMaskField(new GUIContent(text, ""), mask);
    }

    public static LayerMask drawMaskField(GUIContent uIContent, LayerMask mask) {
        int tempMask = EditorGUILayout.MaskField(uIContent,
                                                      InternalEditorUtility.LayerMaskToConcatenatedLayersMask(mask),
                                                      InternalEditorUtility.layers);
        return InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
    }
}
