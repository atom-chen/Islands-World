using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Coolape;
using System;
using System.IO;

public class ECLSpritePacker : EditorWindow
{
	public enum PackerSize
	{
		max_Sprite_Count_50 = 50,
		max_Sprite_Count_100 = 100,
		max_Sprite_Count_150 = 150,
		max_Sprite_Count_200 = 200,
	}

	public enum PackerTextureSize
	{
		Texture_Size_512 = 512,
		Texture_Size_1024 = 1024,
		Texture_Size_2048 = 2048,
		Texture_Size_4096 = 4096,
	}

	public enum SortSprite
	{
		none,
		SortArea,
		SortLength,
	}

	static string mSpritesBorrowInforFile = "";
	static int cellSize = 70;
	static Rect rect = new Rect (50f, 0, cellSize, cellSize);
	static ArrayList mSpriteList = new ArrayList ();
	PackerSize packerSize = PackerSize.max_Sprite_Count_100;
	PackerTextureSize textureSize = PackerTextureSize.Texture_Size_1024;
	SortSprite sortSprite = SortSprite.SortArea;
	Vector2 scrollPos = Vector2.zero;
	Vector2 scrollPos2 = Vector2.zero;
	static string mSelectedSprite = "";
	Hashtable item;
	bool isSelectedAll = false;
	int selectedCount = 0;
	int allSpriteCount = 0;
	Hashtable currSelectSprite;
	bool isShowParckerView = false;
	bool isShowParckerTextureBg = false;
	Texture2D packTex;
	static Rect[] packRects;
	static List<Hashtable> packSprites = new List<Hashtable> ();
	static string packedName = "";
	Texture2D _empty;
	bool isUseUnityPacking = true;
	bool showDeltail = false;
	bool removePublishRes = true;

	void OnGUI ()
	{
		GUILayout.Space (5);
		ECLEditorUtl.BeginContents ();
		{
			EditorGUILayout.BeginHorizontal ();
			{
				if (GUILayout.Button ("Sprites Borrow Infor File", ECLEditorUtl.width150)) {
					string _mSpritesBorrowInforFile = EditorUtility.OpenFilePanel ("Select Infor File", Application.dataPath + "/KOK/xRes/spriteBorrow", "json");		
					if (!mSpritesBorrowInforFile.Equals (_mSpritesBorrowInforFile)) {
						mSpritesBorrowInforFile = _mSpritesBorrowInforFile;
						initData ();
					}
				}
				if (GUILayout.Button ("Refresh", ECLEditorUtl.width80)) {
					initData ();
				}
				GUILayout.TextField (mSpritesBorrowInforFile);
			}
			EditorGUILayout.EndHorizontal ();
			GUILayout.Space (5);
			EditorGUILayout.BeginHorizontal ();
			{
				packerSize = (PackerSize)EditorGUILayout.EnumPopup ("", packerSize);
				GUI.color = Color.yellow;
				if (GUILayout.Button ("Packer View")) {
					isShowParckerView = true;
					currSelectSprite = null;
				}
				GUI.color = Color.white;
			}
			EditorGUILayout.EndHorizontal ();
		}
		ECLEditorUtl.EndContents ();
		GUILayout.Space (5);

		EditorGUILayout.BeginHorizontal ();
		{
			EditorGUILayout.BeginVertical ();
			{
				bool _isSelectedAll = EditorGUILayout.ToggleLeft ("All", isSelectedAll, ECLEditorUtl.width150);
				if (isSelectedAll != _isSelectedAll) {
					isSelectedAll = _isSelectedAll;
					for (int i = 0; i < mSpriteList.Count && i < (int)packerSize; i++) {
						item = mSpriteList [i] as Hashtable;
						item ["selected"] = isSelectedAll;
					}
					refreshSelectedCount ();
				}
				scrollPos = EditorGUILayout.BeginScrollView (scrollPos, ECLEditorUtl.width150);
				{
					rect.y = 0;
					ECLEditorUtl.BeginContents ();
					{
//						GUILayout.Space (10);
						for (int i = 0; i < mSpriteList.Count && i < (int)packerSize; i++) {
							item = mSpriteList [i] as Hashtable;
							bool selected = GUI.Toggle (new Rect (5, rect.y, 50, cellSize + 30), MapEx.getBool (item, "selected"), (i + 1).ToString ());
							if (selected != MapEx.getBool (item, "selected")) {
								item ["selected"] = selected;
								refreshSelectedCount ();
							}
							showCellSprite (item);
						}
					}
					ECLEditorUtl.EndContents ();
				}
				EditorGUILayout.EndScrollView ();
			}
			EditorGUILayout.EndVertical ();
			//=========================================
			//=========================================
			EditorGUILayout.BeginVertical ();
			{
				GUILayout.Label (PStr.b ().a ("Selected: ").a (selectedCount).a ("/").a (allSpriteCount).e ());
				scrollPos2 = EditorGUILayout.BeginScrollView (scrollPos2, GUILayout.Width (position.width - 150));
				{
					ECLEditorUtl.BeginContents ();
					{
						if (isShowParckerView) {
							showPackerView ();
						} else {
							showSpriteInfor ();
						}
					}
					ECLEditorUtl.EndContents ();
				}
				EditorGUILayout.EndScrollView ();
			}
			EditorGUILayout.EndVertical ();
		}
		EditorGUILayout.EndHorizontal ();
	}

	void refreshSelectedCount ()
	{
		int ret = 0;
		allSpriteCount = 0;
		selectedCount = 0;
		for (int i = 0; i < mSpriteList.Count && i < (int)packerSize; i++) {
			allSpriteCount += 1;
			item = mSpriteList [i] as Hashtable;
			if (MapEx.getBool (item, "selected")) {
				ret += 1;	
			}
		}
		selectedCount = ret;
	}

	void showPackerView ()
	{
		if (packTex != null) {
			//=================
			float h = 0;
			float w = position.width - 160;
			float rate = w / packTex.width;
			if (rate < 1) {
				h = packTex.height * rate;
			} else {
				h = packTex.height;
			}
			h = h > 512 ? h : 512;
			Rect r = new Rect (0, 0, NumEx.getIntPart (w), NumEx.getIntPart (h));
			NGUIEditorTools.DrawTiledTexture (r, NGUIEditorTools.backdropTexture);
			if (isShowParckerTextureBg) {
				GUI.DrawTexture (r, _empty, ScaleMode.ScaleToFit, false);
			}
			GUI.DrawTexture (r, packTex, ScaleMode.ScaleToFit);
			GUILayout.Space (r.height + r.y);	//这句主要目的是为了可以滑动

			ECLEditorUtl.BeginContents ();
			{
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.IntField ("width", packTex.width);
					EditorGUILayout.IntField ("height", packTex.height);
				}
				EditorGUILayout.EndHorizontal ();
				if (GUILayout.Button ("Show/Hide Detail")) {
					showDeltail = !showDeltail;
				}
				if (showDeltail) {
					if (packSprites != null) {
						Hashtable m = null;
						Hashtable d = null;
						Rect _rect;
						for (int i = 0; i < packSprites.Count; i++) {
							_rect = packRects [i];
							m = packSprites [i] as Hashtable;
							d = MapEx.getMap (m, "data");
							EditorGUILayout.BeginHorizontal ();
							{
								EditorGUILayout.LabelField (MapEx.getString (d, "name"));
								EditorGUILayout.LabelField (Mathf.RoundToInt (_rect.x) + "x" + Mathf.RoundToInt (_rect.y));
								EditorGUILayout.LabelField (Mathf.RoundToInt (_rect.width) + "x" + Mathf.RoundToInt (_rect.height));
							}
							EditorGUILayout.EndHorizontal ();
						}
					}
				}
			}
			ECLEditorUtl.EndContents ();

		}

		isShowParckerTextureBg = EditorGUILayout.ToggleLeft ("Show Background", isShowParckerTextureBg);
		textureSize = (PackerTextureSize)EditorGUILayout.EnumPopup ("", textureSize);
		isUseUnityPacking = EditorGUILayout.ToggleLeft ("UnityPacking", isUseUnityPacking);
		sortSprite = (SortSprite)EditorGUILayout.EnumPopup ("", sortSprite);
		GUILayout.Space (5);
		GUI.color = Color.yellow;
		if (GUILayout.Button ("Review Pack Texture")) {
			if (!packTextures ((int)textureSize, isUseUnityPacking)) {
				Debug.LogError ("Some errors happened!");
			}
		}
		GUI.color = Color.white;
		GUILayout.Space (10);

		ECLEditorUtl.BeginContents ();
		{
			packedName = EditorGUILayout.TextField ("Packed Texture Name", string.IsNullOrEmpty (packedName) ? "Packed" + (int)packerSize : packedName);
			GUI.color = Color.red;
			removePublishRes = EditorGUILayout.ToggleLeft ("Remove Publish AssetsBundle", removePublishRes);
			if (GUILayout.Button ("Apply Pack Texture")) {
				applyPackTexture ((int)textureSize, isUseUnityPacking);
			}
			GUI.color = Color.white;
		}
		ECLEditorUtl.EndContents ();
	}


	void applyPackTexture (int maxSize, bool unityPacking)
	{
		string texturePath = EditorUtility.OpenFolderPanel ("Save packed texture", Application.dataPath + "/" + CLPathCfg.self.basePath + "/upgradeRes4Dev/other", "");
		if (string.IsNullOrEmpty (texturePath)) {
			return;
		}
		if (packTex == null || packRects == null || packRects.Length == 0 || packSprites == null || packSprites.Count == 0 || packRects.Length != packSprites.Count) {
			packTextures (maxSize, isUseUnityPacking);
		}
		string packTextureFile = Path.Combine (texturePath, packedName + ".png");
		string packTextureFile4Atlas = packTextureFile.Replace (Application.dataPath + "/", "");
		Debug.LogWarning (packTextureFile4Atlas);

		byte[] bytes = packTex.EncodeToPNG ();
		Directory.CreateDirectory (Path.GetDirectoryName (packTextureFile));
		File.WriteAllBytes (packTextureFile, bytes);
		AssetDatabase.ImportAsset ("Assets/" + packTextureFile4Atlas); 
		TextureImporter textureImporter = AssetImporter.GetAtPath ("Assets/" + packTextureFile4Atlas) as TextureImporter; 
		textureImporter.textureType = TextureImporterType.GUI;
		textureImporter.mipmapEnabled = false;
		textureImporter.wrapMode = TextureWrapMode.Clamp;
		textureImporter.alphaIsTransparency = true;
		textureImporter.npotScale = TextureImporterNPOTScale.None;
		textureImporter.filterMode = FilterMode.Trilinear;		//改成这种模式好像更省内存
		AssetDatabase.ImportAsset ("Assets/" + packTextureFile4Atlas); 

		Hashtable m = null;
		Hashtable d = null;
		Hashtable atlasMap = null;
		UIAtlas atlas = null;
		UISpriteData spData = null;
		Rect _rect;
		for (int i = 0; i < packSprites.Count; i++) {
			m = packSprites [i] as Hashtable;
			_rect = packRects [i];
			d = MapEx.getMap (m, "data");
			atlasMap = MapEx.getMap (m, "atlas");
			foreach (DictionaryEntry item in atlasMap) {
				atlas = getAtlasByName (item.Key.ToString ());
				if (atlas == null) {
					Debug.LogError ("Get atlas is null!!==" + item.Key);
					continue;
				}

//				spData = atlas.GetSprite (MapEx.getString (d, "name"));
				string spName = MapEx.getString (d, "name");
				if (!atlas.spriteMap.ContainsKey (spName)) {
					Debug.LogError ("atlas.GetSprite  is null!!==" + spName);
					continue;
				}
				int index = MapEx.getInt (atlas.spriteMap, spName);
				spData = atlas.spriteList [index];
				if (spData == null) {
					Debug.LogError ("atlas.GetSprite  is null!!==" + spName);
					continue;
				}
				string toPath = texturePath + "/" + spData.path.Replace (CLPathCfg.self.basePath + "/upgradeRes4Dev/other/", "");
				Directory.CreateDirectory (Path.GetDirectoryName (toPath));
				toPath = toPath.Replace (Application.dataPath + "/", "");
				AssetDatabase.ImportAsset (Path.GetDirectoryName ("Assets/" + toPath));
				string err = AssetDatabase.MoveAsset ("Assets/" + spData.path, "Assets/" + toPath);
				if (!string.IsNullOrEmpty (err)) {
					Debug.LogError (err);
				}
				if (removePublishRes) {
					string fromPath = Path.GetDirectoryName (spData.path) + "/Android/" + Path.GetFileNameWithoutExtension (spData.path) + ".unity3d";
					fromPath = fromPath.Replace ("/upgradeRes4Dev/", "/upgradeRes4Publish/");
					if (File.Exists (Application.dataPath + "/" + fromPath)) {
						File.Delete (Application.dataPath + "/" + fromPath);
					}

					fromPath = Path.GetDirectoryName (spData.path) + "/IOS/" + Path.GetFileNameWithoutExtension (spData.path) + ".unity3d";
					fromPath = fromPath.Replace ("/upgradeRes4Dev/", "/upgradeRes4Publish/");
					if (File.Exists (Application.dataPath + "/" + fromPath)) {
						File.Delete (Application.dataPath + "/" + fromPath);
					}
				}
				spData.path = packTextureFile4Atlas;
				spData.x = Mathf.RoundToInt (_rect.x);
				spData.y = Mathf.RoundToInt (_rect.y);

				EditorUtility.SetDirty (atlas.gameObject);
				AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (atlas.gameObject));
			}
		}
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh (ImportAssetOptions.ForceSynchronousImport);


		EditorUtility.DisplayDialog ("success", "Finished!", "Okay");
	}

	UIAtlas getAtlasByName (string atlasName)
	{
		string tmpPath = PStr.begin ()
			.a ("Assets/").a (CLPathCfg.self.basePath).a ("/").a ("upgradeRes4Dev").
			a ("/priority/atlas/").a (atlasName).a (".prefab").end ();
		UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath (tmpPath, typeof(UnityEngine.Object));
		if (obj != null) {
			return ((GameObject)obj).GetComponent<UIAtlas> ();
		}
		return null;
	}

	bool packTextures (int maxSize, bool unityPacking)//, ref ArrayList outSprites, ref Rect[] rects)
	{
		if (packTex != null) {
			DestroyImmediate (packTex, true);
			packTex = null;
		}

		refreshSelectedCount ();
		if (selectedCount == 0) {
			Debug.LogError ("Please select some sprites, that need packe");
			return false;
		}

		packSprites.Clear ();
		List<Texture2D> listTexs = new List<Texture2D> ();
		for (int i = 0; i < mSpriteList.Count; i++) {
			Hashtable m = mSpriteList [i] as Hashtable;
			if (MapEx.getBool (m, "selected")) {
				Hashtable d = MapEx.getMap (m, "data");
				string name = MapEx.getString (d, "name");
				string path = MapEx.getString (d, "path");
				Texture2D tex = ECLEditorUtl.getObjectByPath (path) as Texture2D;
				listTexs.Add (tex);
				packSprites.Add (m);
			}
		}
		if (sortSprite != SortSprite.none) {
			if (sortSprite == SortSprite.SortArea) { 
				packSprites.Sort (CompareSprite);
			} else {
				packSprites.Sort (CompareSprite2);
			}
			listTexs.Clear ();
			for (int i = 0; i < packSprites.Count; i++) {
				Hashtable d = MapEx.getMap (packSprites [i], "data");
				string path = MapEx.getString (d, "path");
				setTextureReadable ("Assets/" + path, TextureImporterFormat.RGBA32, TextureImporterCompression.Uncompressed, true);
				Texture2D tex = ECLEditorUtl.getObjectByPath (path) as Texture2D;
				listTexs.Add (tex);
			}
		}

//		for (int i = 0; i < listTexs.Count; i++) {
//			setTextureReadable (listTexs [i] as Texture, TextureImporterFormat.RGBA32, TextureImporterCompression.Uncompressed, true);
//		}
		packTex = new Texture2D (1, 1, TextureFormat.ARGB32, false);

		if (unityPacking) {
			packRects = packTex.PackTextures (listTexs.ToArray (), NGUISettings.atlasPadding, maxSize);
		} else {
			packRects = UITexturePacker.PackTextures (packTex, listTexs.ToArray (), 4, 4, NGUISettings.atlasPadding, maxSize);
		}
		_empty = new Texture2D (packTex.width, packTex.height, TextureFormat.ARGB32, false);
		bool ret = true;
		for (int i = 0; i < listTexs.Count; ++i) {
			Rect rect = NGUIMath.ConvertToPixels (packRects [i], packTex.width, packTex.height, true);
			packRects [i] = rect;

			// Apparently Unity can take the liberty of destroying temporary textures without any warning
			if (listTexs [i] == null) {
				Debug.LogWarning ("Apparently Unity can take the liberty of destroying temporary textures without any warning");
				ret = false;
				break;
			}
			
			// Make sure that we don't shrink the textures
			if (Mathf.RoundToInt (rect.width) != listTexs [i].width) {
				Debug.LogError (rect.width + "====" + listTexs [i].width);
				Debug.LogWarning ("Make sure that we don't shrink the textures=" + listTexs [i].name);
				ret = false;
				break;
			}
		}

//		for (int i = 0; i < listTexs.Count; i++) {
//			setTextureReadable (listTexs [i] as Texture, TextureImporterFormat.Automatic, false);
//		}
		return ret;
	}

	int CompareSprite (Hashtable a, Hashtable b)
	{
		// A is null b is not b is greater so put it at the front of the list
		if (a == null && b != null)
			return 1;

		// A is not null b is null a is greater so put it at the front of the list
		if (a != null && b == null)
			return -1;

		Hashtable d1 = MapEx.getMap (a, "data");
		Hashtable d2 = MapEx.getMap (b, "data");
		// Get the total pixels used for each sprite
		int aPixels = MapEx.getInt (d1, "width") * MapEx.getInt (d1, "height");
		int bPixels = MapEx.getInt (d2, "width") * MapEx.getInt (d2, "height");

		if (aPixels > bPixels)
			return -1;
		else if (aPixels < bPixels)
			return 1;
		return 0;
	}

	int CompareSprite2 (Hashtable a, Hashtable b)
	{
		// A is null b is not b is greater so put it at the front of the list
		if (a == null && b != null)
			return 1;

		// A is not null b is null a is greater so put it at the front of the list
		if (a != null && b == null)
			return -1;

		Hashtable d1 = MapEx.getMap (a, "data");
		Hashtable d2 = MapEx.getMap (b, "data");
		// Get the total pixels used for each sprite
		int w = MapEx.getInt (d1, "width");
		int h = MapEx.getInt (d1, "height");
		int aPixels = w > h ? w : h;
		w = MapEx.getInt (d2, "width");
		h = MapEx.getInt (d2, "height");
		int bPixels = w > h ? w : h;

		if (aPixels > bPixels)
			return -1;
		else if (aPixels < bPixels)
			return 1;
		return 0;
	}

	void setTextureReadable (Texture2D tex, TextureImporterFormat format, TextureImporterCompression compression, bool val, bool importNow = true)
	{
		string texPath = AssetDatabase.GetAssetPath (tex); 
		setTextureReadable (texPath, format, compression, val, importNow);
	}

	void setTextureReadable (string texPath, TextureImporterFormat format, TextureImporterCompression compression, bool val, bool importNow = true)
	{
		TextureImporter textureImporter = AssetImporter.GetAtPath (texPath) as TextureImporter; 
		textureImporter.isReadable = val;

        //textureImporter.textureFormat = format;
		textureImporter.textureCompression = compression;
        TextureImporterPlatformSettings texSettings = new TextureImporterPlatformSettings();
        texSettings.format = format;
        texSettings.maxTextureSize = 4096;
        texSettings.allowsAlphaSplitting = true;
        texSettings.compressionQuality = 100;
#if UNITY_ANDROID
        texSettings.name = "Android";
        //textureImporter.SetPlatformTextureSettings ("Android", 4096, format, 100, true);
#elif UNITY_IPHONE || UNITY_IOS
        texSettings.name = "iPhone";
		//textureImporter.SetPlatformTextureSettings("iPhone", 4096, format, 100, true);
#else
        texSettings.name = "Standalone";
		//textureImporter.SetPlatformTextureSettings("Standalone", 4096, format, 100, true);
#endif
        textureImporter.SetPlatformTextureSettings(texSettings);
        EditorUtility.SetDirty (textureImporter);
		textureImporter.SaveAndReimport ();
		if (importNow) {
			AssetDatabase.ImportAsset (texPath); 
		}
	}

	void showSpriteInfor ()
	{
		if (currSelectSprite == null)
			return;
		Hashtable d = MapEx.getMap (currSelectSprite, "data");
		int times = MapEx.getInt (currSelectSprite, "times");
		string name = MapEx.getString (d, "name");
		string path = MapEx.getString (d, "path");
		int x = MapEx.getInt (d, "x");
		int y = MapEx.getInt (d, "y");
		int width = MapEx.getInt (d, "width");
		int height = MapEx.getInt (d, "height");
		int borderLeft = MapEx.getInt (d, "borderLeft");
		int borderRight = MapEx.getInt (d, "borderRight");
		int borderTop = MapEx.getInt (d, "borderTop");
		int borderBottom = MapEx.getInt (d, "borderBottom");
		int paddingLeft = MapEx.getInt (d, "paddingLeft");
		int paddingRight = MapEx.getInt (d, "paddingRight");
		int paddingTop = MapEx.getInt (d, "paddingTop");
		int paddingBottom = MapEx.getInt (d, "paddingBottom");
		Hashtable atlas = MapEx.getMap (currSelectSprite, "atlas");
		string atlasStr = "";
		foreach (DictionaryEntry item in atlas) {
			atlasStr = PStr.b ().a (atlasStr).a (",").a (item.Key.ToString ()).e ();
		}
		Texture tex = ECLEditorUtl.getObjectByPath (path) as Texture;
		Rect r = Rect.zero;
		if (tex != null) {
			float h = 0;
			float w = position.width - 160;
			float rate = w / tex.width;
			if (rate < 1) {
				h = tex.height * rate;
			} else {
				h = tex.height;
			}
			h = h > 200 ? h : 200;
			r = new Rect (0, 0, NumEx.getIntPart (w), NumEx.getIntPart (h));
			GUI.DrawTexture (r, tex, ScaleMode.ScaleToFit);
			GUILayout.Space (r.height + r.y);	//这句主要目的是为了可以滑动
		} else {
			r = new Rect (0, 0, position.width - 160, 100);
			GUILayout.Space (r.height + r.y);	//这句主要目的是为了可以滑动
		}

		GUILayout.Space (10);	
		ECLEditorUtl.BeginContents ();
		{

			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.TextField ("name", name);
				EditorGUILayout.IntField ("times", times);
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.IntField ("x", x);
				EditorGUILayout.IntField ("y", y);
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.IntField ("width", width);
				EditorGUILayout.IntField ("height", height);
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.IntField ("borderLeft", borderLeft);
				EditorGUILayout.IntField ("borderRight", borderRight);
			}
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.IntField ("borderTop", borderTop);
				EditorGUILayout.IntField ("borderBottom", borderBottom);
			}
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.IntField ("paddingLeft", paddingLeft);
				EditorGUILayout.IntField ("paddingRight", paddingRight);
			}
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.IntField ("paddingTop", paddingTop);
				EditorGUILayout.IntField ("paddingBottom", paddingBottom);
			}
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.TextField ("path", path);
			EditorGUILayout.TextField ("Atlas", atlasStr);
		}
		ECLEditorUtl.EndContents ();
	}

	void showCellSprite (Hashtable m)
	{
		Hashtable d = MapEx.getMap (m, "data");
		Texture tex = ECLEditorUtl.getObjectByPath (MapEx.getString (d, "path")) as Texture;
		if (tex == null) {
			return;
		}
//		NGUIEditorTools.DrawTiledTexture (rect, NGUIEditorTools.backdropTexture);
		Rect uv = new Rect (MapEx.getInt (d, "x"), MapEx.getInt (d, "y"), MapEx.getInt (d, "width"), MapEx.getInt (d, "height"));
		uv = NGUIMath.ConvertToTexCoords (uv, tex.width, tex.height);

		float scaleX = rect.width / uv.width;
		float scaleY = rect.height / uv.height;

		// Stretch the sprite so that it will appear proper
		float aspect = (scaleY / scaleX) / ((float)tex.height / tex.width);
		Rect clipRect = rect;

		if (aspect != 1f) {
			if (aspect < 1f) {
				// The sprite is taller than it is wider
				float padding = cellSize * (1f - aspect) * 0.5f;
				clipRect.xMin += padding;
				clipRect.xMax -= padding;
			} else {
				// The sprite is wider than it is taller
				float padding = cellSize * (1f - 1f / aspect) * 0.5f;
				clipRect.yMin += padding;
				clipRect.yMax -= padding;
			}
		}

		if (GUI.Button (rect, "")) {
			mSelectedSprite = MapEx.getString (d, "name");
			isShowParckerView = false;
			currSelectSprite = m;
		}
		GUI.DrawTextureWithTexCoords (clipRect, tex, uv);

		// Draw the selection
		if (mSelectedSprite == MapEx.getString (d, "name")) {
			NGUIEditorTools.DrawOutline (rect, new Color (0.4f, 1f, 0f, 1f));
		}

		GUI.backgroundColor = new Color (1f, 1f, 1f, 0.5f);
		GUI.contentColor = new Color (1f, 1f, 1f, 0.7f);
		GUI.Label (new Rect (rect.x, rect.y + rect.height, rect.width, 32f), MapEx.getString (d, "name"), "ProgressBarBack");
		GUI.contentColor = Color.white;
		GUI.backgroundColor = Color.white;
		GUILayout.Space (cellSize + 30);	//这句主要目的是为了可以滑动
		rect.y += (cellSize + 30);
	}

	//=================================
	/*
        m.atlas[atlas.name] = atlas.name;
            m.data = {};
            m.data.name = spData.name;
            m.data.path = spData.path;

            m.data.x = spData.x;
            m.data.y = spData.y;
            m.data.width = spData.width;
            m.data.height = spData.height;

            m.data.borderLeft = spData.borderLeft
            m.data.borderRight = spData.borderRight
            m.data.borderTop = spData.borderTop
            m.data.borderBottom = spData.borderBottom

            m.data.paddingLeft = spData.paddingLeft
            m.data.paddingRight = spData.paddingRight
            m.data.paddingTop = spData.paddingTop
            m.data.paddingBottom = spData.paddingBottom
        end
        m.times = m.times or 0;
        m.times = m.times + 1;
        
	 */

	void initData ()
	{
		isShowParckerView = false;
		isSelectedAll = false;
		currSelectSprite = null;
		packTex = null;
		packRects = null;
		packSprites.Clear ();
		mSpriteList.Clear ();
		if (string.IsNullOrEmpty (mSpritesBorrowInforFile)) {
			return;
		}
		string content = File.ReadAllText (mSpritesBorrowInforFile);
		Hashtable spriteMap = JSON.DecodeMap (content);
		Hashtable m = null;
		Hashtable data = null;
		mSpriteList = MapEx.vals2List (spriteMap);
		mSpriteList.Sort (new MySortSpriteBorrow ());
//		for (int i = 0; i < mSpriteList.Count; i++) {
//			m = mSpriteList [i] as Hashtable;
//			data = MapEx.getMap (m, "data");
//			Debug.Log (MapEx.getString (data, "name") + "," + MapEx.getInt (m, "times"));
//		}
	}

	int sortBorrowTimes (object a, object b)
	{
		Hashtable m1 = a as Hashtable;
		Hashtable m2 = b as Hashtable;
		int ret = 0;
		if (MapEx.getInt (m1, "times") > MapEx.getInt (m2, "times")) {
			ret = 1;
		} else {
			ret = -1;
		}
		return ret;
	}

	public class MySortSpriteBorrow : IComparer
	{
		// Calls CaseInsensitiveComparer.Compare with the parameters reversed.
		int IComparer.Compare (object a, object b)
		{
			Hashtable m1 = a as Hashtable;
			Hashtable m2 = b as Hashtable;
			int ret = 0;
			int time1 = MapEx.getInt (m1, "times");
			int time2 = MapEx.getInt (m2, "times");
			if (time1 < time2) {
				ret = 1;
			} else if (time1 == time2) {
				ret = 0;
			} else {
				ret = -1;
			}
			return ret;
		}

	}
}
