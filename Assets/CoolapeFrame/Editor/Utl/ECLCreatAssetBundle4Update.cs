using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using Coolape;

public class ECLCreatAssetBundle4Update
{
	public delegate void CreateDelegate (string file, UnityEngine.Object obj);

	public static void makeAssetBundles ()//该函数表示通过上面的点击响应的函数
	{
		foreach (UnityEngine.Object o in Selection.objects) {
			Debug.Log (o.name);
			string path = AssetDatabase.GetAssetPath (o);//Selection表示你鼠标选择激活的对象
			UnityEngine.Object t = AssetDatabase.LoadMainAssetAtPath (path);
			path = Path.GetDirectoryName (path);
//			path = path.Replace ("Assets/", "");
			createAssets4Upgrade (path, t, true);
		}
	}

	public static void makeAssetBundlesUncompressed ()//该函数表示通过上面的点击响应的函数
	{
		foreach (UnityEngine.Object o in Selection.objects) {
			Debug.Log (o.name);
			string path = AssetDatabase.GetAssetPath (o);//Selection表示你鼠标选择激活的对象
			UnityEngine.Object t = AssetDatabase.LoadMainAssetAtPath (path);
			path = Path.GetDirectoryName (path);
//			path = path.Replace ("Assets/", "");
			createAssets4Upgrade (path, t, false);
		}
	}

	public static void makeAssetBundlesSelections ()//该函数表示通过上面的点击响应的函数
	{
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);//Selection表示你鼠标选择激活的对象
		Debug.Log ("Selected Folder: " + path);
		if (string.IsNullOrEmpty (path) || !Directory.Exists (path)) {
			Debug.LogWarning ("请选择目录!");
			return;
		}
		path = Application.dataPath + "/" + path.Replace ("Assets/", "");
		createUnity3dFiles (path, null, true);
	}

	public static void createAssets4Upgrade (string file, bool isCompress = true)
	{
		if (string.IsNullOrEmpty (file))
			return;
		string path = Path.GetDirectoryName (file);
		createAssets4Upgrade (path, AssetDatabase.LoadMainAssetAtPath (file), isCompress);
	}

	public static void createAssets4Upgrade (string file, UnityEngine.Object obj, bool isCompress)
	{
//		Debug.Log (file);
		if (string.IsNullOrEmpty (file) || obj == null) {
			Debug.LogError ("file==" + file);
			return;
		}
		cleanShardAssets (obj);
		//==================================
		file = file.Replace ("/upgradeRes4Dev", "/upgradeRes4Publish");
		
		BuildAssetBundleOptions opt = BuildAssetBundleOptions.CollectDependencies;
		if (isCompress) {
			opt = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.IgnoreTypeTreeChanges;
		} else {
			opt = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.IgnoreTypeTreeChanges | BuildAssetBundleOptions.UncompressedAssetBundle;
		}
		
		string bundlePath = "";
//		Directory.CreateDirectory (Application.dataPath + "/" + file);
#if UNITY_ANDROID
		bundlePath = file + "/Android/" + obj.name + ".unity3d";
		Directory.CreateDirectory (Path.GetDirectoryName (bundlePath));
		Debug.Log ("bundlePath==" + bundlePath);
        BuildPipeline.BuildAssetBundle (obj, null, bundlePath, opt, BuildTarget.Android);
#elif UNITY_IPHONE || UNITY_IOS
		bundlePath  = file + "/IOS/" +obj.name +  ".unity3d";
		Directory.CreateDirectory(Path.GetDirectoryName(bundlePath));
        BuildPipeline.BuildAssetBundle(obj, null, bundlePath, opt, BuildTarget.iOS);
#elif UNITY_STANDALONE_WIN
		bundlePath = file + "/Standalone/" + obj.name + ".unity3d";
		Directory.CreateDirectory (Path.GetDirectoryName (bundlePath));
		Debug.Log ("bundlePath==" + bundlePath);
		BuildPipeline.BuildAssetBundle (obj, null, bundlePath, opt, BuildTarget.StandaloneWindows64);
#elif UNITY_STANDALONE_OSX
        bundlePath = file + "/StandaloneOSX/" + obj.name + ".unity3d";
        Directory.CreateDirectory (Path.GetDirectoryName (bundlePath));
        Debug.Log ("bundlePath==" + bundlePath);
        BuildPipeline.BuildAssetBundle (obj, null, bundlePath, opt, BuildTarget.StandaloneOSX);
#endif
        FileInfo fileInfo = new FileInfo (bundlePath);
		long size = (fileInfo.Length / 1024);
		if (size >= 900) {
			Debug.LogError (" size== " + size + "KB," + fileInfo.FullName);
		} else {
			Debug.Log (" size== " + size + "KB");
		}

		//==================================
		resetShardAssets (obj);
	}

	public static void cleanShardAssets (UnityEngine.Object obj)
	{
		CLSharedAssets sharedAsset = null;
		CLRoleAvata avata = null;
		if (obj is GameObject) {
			sharedAsset = ((GameObject)obj).GetComponent<CLSharedAssets> ();
			avata = ((GameObject)obj).GetComponent<CLRoleAvata> ();
			if (AssetDatabase.GetAssetPath (obj).Contains ("/other/model/")) {
				ECLEditorUtl.cleanModleMaterials (AssetDatabase.GetAssetPath (obj));
			}
			UIFont font = ((GameObject)obj).GetComponent<UIFont> ();
			if (font != null) {
				string spName = font.spriteName;
				font.atlas = null;
				font.material = null;
				font.spriteName = spName;
			}
		} else if (obj is Material) {
			CLMaterialPool.cleanTexRef (ECLEditorUtl.getAssetName4Upgrade (obj), (Material)obj);
			sharedAsset = null;
		} else {
			sharedAsset = null;
		}
		bool isRefresh = false;
		if (avata != null) {
			avata.cleanMaterial ();
			isRefresh = true;
		}
		if (sharedAsset != null) {
			sharedAsset.cleanRefAssets ();
			isRefresh = true;
		}
		if (isRefresh) {
		//			AssetDatabase.Refresh ();
			string path = AssetDatabase.GetAssetPath (obj);
			EditorUtility.SetDirty (obj);
			AssetDatabase.WriteImportSettingsIfDirty (path);
			AssetDatabase.ImportAsset (path);
		}
	}

	public static void resetShardAssets (UnityEngine.Object obj)
	{
		CLSharedAssets sharedAsset = null;
		CLRoleAvata avata = null;
		if (obj != null && obj is GameObject) {
			// 没搞明白，执行到这里时，textureMgr已经为null了，因此再取一次
			sharedAsset = ((GameObject)obj).GetComponent<CLSharedAssets> ();
			avata = ((GameObject)obj).GetComponent<CLRoleAvata> ();

			UIFont font = ((GameObject)obj).GetComponent<UIFont> ();
			if (font != null) {
                if(!string.IsNullOrEmpty(font.atlasName)) {
    				font.atlas = CLUIInit.self.getAtlasByName (font.atlasName);
                    if(font.atlas) {
    				    font.material = font.atlas.spriteMaterial;
                    }
                }
			}
		} else if (obj != null && obj is Material) {
			CLMaterialPool.resetTexRef (ECLEditorUtl.getAssetName4Upgrade (obj), (Material)obj, null, null);
			sharedAsset = null;
		} else {
			sharedAsset = null;
		}

		bool isRefresh = false;
		if (avata != null) {
			avata.setDefaultMaterial ();
			isRefresh = true;
		}
		if (sharedAsset != null) {
			sharedAsset.reset ();
			sharedAsset.resetAssets ();
			isRefresh = true;
		}
		if (isRefresh) {
			string path = AssetDatabase.GetAssetPath (obj);
			EditorUtility.SetDirty (obj);
			AssetDatabase.WriteImportSettingsIfDirty (path);
			AssetDatabase.ImportAsset (path);
		}
	}

	public static void createUnity3dFiles (string path, CreateDelegate procDelegate, bool isTraversal)
	{
		if (path.Length != 0) {
			//path = path.Replace ("Assets/", "");//因为AssetDatabase.GetAssetPath得到的是型如Assets/文件夹名称，且看下面一句，所以才有这一句。
			string[] fileEntries = Directory.GetFiles (path);//因为Application.dataPath得到的是型如 "工程名称/Assets"
			string[] div_line = new string[] { "Assets/" };
			foreach (string fileName in fileEntries) {
				Debug.Log ("fileName=" + fileName);
				string[] sTemp = fileName.Split (div_line, StringSplitOptions.RemoveEmptyEntries);
				string filePath = sTemp [1];
				//Debug.Log(filePath);S
				string localPath = "Assets/" + filePath;

				UnityEngine.Object t = AssetDatabase.LoadMainAssetAtPath (localPath);
				if (t != null) {
					if (procDelegate != null) {
						procDelegate (path.Replace (Application.dataPath + "/", ""), t);
					} else {
						createAssets4Upgrade (path, t, true);
					}
				}
			}
			//=============
			if (isTraversal) {
				string[] dirEntries = Directory.GetDirectories (path);
				foreach (string dir in dirEntries) {
					createUnity3dFiles (dir, procDelegate, isTraversal);
				}
			}
		}
	}

	//=====================================
	// C# Example
	// Builds an asset bundle from the selected objects in the project view.
	// Once compiled go to "Menu" -> "Assets" and select one of the choices
	// to build the Asset Bundle
	
	//	public class ExportAssetBundles
	//	{
	//		[MenuItem ("Assets/Build AssetBundle From Selection - Track dependencies")]
	//		static void ExportResource ()
	//		{
	//			// Bring up save panel
	//			string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
	//			if (path.Length != 0) {
	//				// Build the resource file from the active selection.
	//				UnityEngine.Object[] selection = Selection.GetFiltered (typeof(UnityEngine.Object), SelectionMode.DeepAssets);
	//				#if UNITY_IPHONE
	//				BuildPipeline.BuildAssetBundle (Selection.activeObject, selection, path,
	//				                                BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,
	//				                                BuildTarget.iOS);
	//				#elif UNITY_ANDROID
	//				BuildPipeline.BuildAssetBundle (Selection.activeObject, selection, path,
	//					BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,
	//					BuildTarget.Android);
	//				#else
	//				BuildPipeline.BuildAssetBundle (Selection.activeObject, selection, path,
	//					BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets);
	//				#endif
	//
	//				Selection.objects = selection;
	//			}
	//		}
	//
	//		[MenuItem ("Assets/Build AssetBundle From Selection - Track dependencies(Uncompress)")]
	//		static void ExportResourceUncompress ()
	//		{
	//			// Bring up save panel
	//			string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
	//			if (path.Length != 0) {
	//				// Build the resource file from the active selection.
	//				UnityEngine.Object[] selection = Selection.GetFiltered (typeof(UnityEngine.Object), SelectionMode.DeepAssets);
	//#if UNITY_IPHONE
	//				BuildPipeline.BuildAssetBundle (Selection.activeObject, selection, path,
	//				                                BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.UncompressedAssetBundle,
	//				                                BuildTarget.iOS);
	//				#elif UNITY_ANDROID
	//				BuildPipeline.BuildAssetBundle (Selection.activeObject, selection, path,
	//					BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.UncompressedAssetBundle,
	//					BuildTarget.Android);
	//				#else
	//		BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path,
	//		BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.UncompressedAssetBundle);
	//#endif
	//				Selection.objects = selection;
	//			}
	//		}
	//
	//		[MenuItem ("Assets/Build AssetBundle From Selection - No dependency tracking")]
	//		static void ExportResourceNoTrack ()
	//		{
	//			// Bring up save panel
	//			string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
	//			if (path.Length != 0) {
	//				// Build the resource file from the active selection.
	//				#if UNITY_IPHONE
	//				BuildPipeline.BuildAssetBundle (Selection.activeObject, Selection.objects, path, BuildAssetBundleOptions.CompleteAssets, BuildTarget.iOS);
	//				#elif UNITY_ANDROID
	//				BuildPipeline.BuildAssetBundle (Selection.activeObject, Selection.objects, path, BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android);
	//				#else
	//		BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path, BuildAssetBundleOptions.CompleteAssets);
	//				#endif
	//			}
	//		}
	//
	//		[MenuItem ("Assets/Build AssetBundle From Selection - No dependency tracking(Uncompress)")]
	//		static void ExportResourceNoTrackUncompress ()
	//		{
	//			// Bring up save panel
	//			string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
	//			if (path.Length != 0) {
	//				// Build the resource file from the active selection.
	//				#if UNITY_IPHONE
	//				BuildPipeline.BuildAssetBundle (Selection.activeObject, Selection.objects, path, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.iOS);
	//				#elif UNITY_ANDROID
	//				BuildPipeline.BuildAssetBundle (Selection.activeObject, Selection.objects, path, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.Android);
	//				#else
	//				BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path, BuildAssetBundleOptions.UncompressedAssetBundle);
	//				#endif
	//			}
	//		}
	//	}
}
