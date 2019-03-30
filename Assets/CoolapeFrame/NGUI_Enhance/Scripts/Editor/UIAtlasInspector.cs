//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Inspector class used to edit the UIAtlas.
/// </summary>

[CustomEditor(typeof(UIAtlas))]
public class UIAtlasInspector : Editor
{
	static public UIAtlasInspector instance;

	enum AtlasType
	{
		Normal,
		Reference,
	}

	UIAtlas mAtlas;
	AtlasType mType = AtlasType.Normal;
	UIAtlas mReplacement = null;
	
	int runTimes = 0; //add by chenbin
	bool isBorrowMode = false;//add by chenbin
	bool isShowBorrowModeDesc = false;//add by chebin

	void OnEnable () { instance = this; }
	void OnDisable () { instance = null; }

	/// <summary>
	/// Convenience function -- mark all widgets using the sprite as changed.
	/// </summary>

	void MarkSpriteAsDirty ()
	{
		UISpriteData sprite = (mAtlas != null) ? mAtlas.GetSprite(NGUISettings.selectedSprite) : null;
		if (sprite == null) return;

		UISprite[] sprites = NGUITools.FindActive<UISprite>();

		foreach (UISprite sp in sprites)
		{
			if (UIAtlas.CheckIfRelated(sp.atlas, mAtlas) && sp.spriteName == sprite.name)
			{
				UIAtlas atl = sp.atlas;
				sp.atlas = null;
				sp.atlas = atl;
				EditorUtility.SetDirty(sp);
			}
		}

		UILabel[] labels = NGUITools.FindActive<UILabel>();

		foreach (UILabel lbl in labels)
		{
			if (lbl.bitmapFont != null && UIAtlas.CheckIfRelated(lbl.bitmapFont.atlas, mAtlas) && lbl.bitmapFont.UsesSprite(sprite.name))
			{
				UIFont font = lbl.bitmapFont;
				lbl.bitmapFont = null;
				lbl.bitmapFont = font;
				EditorUtility.SetDirty(lbl);
			}
		}
	}

	/// <summary>
	/// Replacement atlas selection callback.
	/// </summary>

	void OnSelectAtlas (Object obj)
	{
		runTimes = 0;	//add by chenbin

		if (mReplacement != obj)
		{
			// Undo doesn't work correctly in this case... so I won't bother.
			//NGUIEditorTools.RegisterUndo("Atlas Change");
			//NGUIEditorTools.RegisterUndo("Atlas Change", mAtlas);

			mAtlas.replacement = obj as UIAtlas;
			mReplacement = mAtlas.replacement;
			NGUITools.SetDirty(mAtlas);
			if (mReplacement == null) mType = AtlasType.Normal;
		}
	}

	/// <summary>
	/// Draw the inspector widget.
	/// </summary>

	public override void OnInspectorGUI ()
	{
		NGUIEditorTools.SetLabelWidth(80f);
		mAtlas = target as UIAtlas;

		UISpriteData sprite = (mAtlas != null) ? mAtlas.GetSprite(NGUISettings.selectedSprite) : null;

		GUILayout.Space(6f);

		if (mAtlas.replacement != null)
		{
			mType = AtlasType.Reference;
			mReplacement = mAtlas.replacement;
		}

		GUILayout.BeginHorizontal();
		AtlasType after = (AtlasType)EditorGUILayout.EnumPopup("Atlas Type", mType);
		NGUIEditorTools.DrawPadding();
		GUILayout.EndHorizontal();

		if (mType != after)
		{
			if (after == AtlasType.Normal)
			{
				mType = AtlasType.Normal;
				OnSelectAtlas(null);
			}
			else
			{
				mType = AtlasType.Reference;
			}
		}

		if (mType == AtlasType.Reference)
		{
			ComponentSelector.Draw<UIAtlas>(mAtlas.replacement, OnSelectAtlas, true);

			GUILayout.Space(6f);
			EditorGUILayout.HelpBox("You can have one atlas simply point to " +
				"another one. This is useful if you want to be " +
				"able to quickly replace the contents of one " +
				"atlas with another one, for example for " +
				"swapping an SD atlas with an HD one, or " +
				"replacing an English atlas with a Chinese " +
				"one. All the sprites referencing this atlas " +
				"will update their references to the new one.", MessageType.Info);

			if (mReplacement != mAtlas && mAtlas.replacement != mReplacement)
			{
				NGUIEditorTools.RegisterUndo("Atlas Change", mAtlas);
				mAtlas.replacement = mReplacement;
				NGUITools.SetDirty(mAtlas);
			}
			return;
		}

		//GUILayout.Space(6f);
		Material mat = EditorGUILayout.ObjectField("Material", mAtlas.spriteMaterial, typeof(Material), false) as Material;

		if (mAtlas.spriteMaterial != mat)
		{
			NGUIEditorTools.RegisterUndo("Atlas Change", mAtlas);
			mAtlas.spriteMaterial = mat;

			// Ensure that this atlas has valid import settings
			if (mAtlas.texture != null) NGUIEditorTools.ImportTexture(mAtlas.texture, false, false, !mAtlas.premultipliedAlpha);

			mAtlas.MarkAsChanged();
		}
		
		#region //add by chenbin
		if (mAtlas.isBorrowSpriteMode) {
			GUI.color = Color.green;
		} else {
			GUI.color = Color.white;
		}
		GUILayout.Label ("\n===	=========================================");
		if (GUILayout.Button ("说明")) {
			isShowBorrowModeDesc = !isShowBorrowModeDesc;
		}
		if (isShowBorrowModeDesc) {
			GUILayout.Label ("勾选isBorrowSpriteMode时，表明该atlas是空atlas，\n" +
			                 "当有sprit对象来borrow时，atlas会根据名字从\n" +
			                 "设置的路径中去取得图片资源。\n" +
			                 //"其中资源必须在Resources/下，\n" +
			                 "另外atlas必须要引用一个material，\n" +
			                 "就算引用的是个空material也行。\n");
		}
		
		if (mAtlas != null && runTimes == 0) {
			runTimes++;
			isBorrowMode = mAtlas.isBorrowSpriteMode;
			//			isUserUnity3DType = mAtlas.useUnity3DType;
		}
		mAtlas.isBorrowSpriteMode = EditorGUILayout.Toggle ("isBorrowSpriteMode", mAtlas.isBorrowSpriteMode);
		if (isBorrowMode != mAtlas.isBorrowSpriteMode) {
			isBorrowMode = mAtlas.isBorrowSpriteMode;
			UnityEditor.EditorUtility.SetDirty (mAtlas.gameObject);
			
		}
		//		if (mAtlas.isBorrowSpriteMode) {
		//			mAtlas.useUnity3DType = EditorGUILayout.Toggle ("useUnity3DType", mAtlas.useUnity3DType);
		//			if (isUserUnity3DType != mAtlas.useUnity3DType) {
		//				isUserUnity3DType = mAtlas.useUnity3DType;
		//				UnityEditor.EditorUtility.SetDirty (NGUISettings.atlas.gameObject);
		//			}
		//		}
		GUILayout.Label ("============================================\n");
		GUI.color = Color.white;
		if (GUILayout.Button ("Clean items if no png")) {
			if (EditorUtility.DisplayDialog ("Alert", "Really ?", "Do it now!", "NO")) {
				UISpriteData item = null;
				List<UISpriteData> list = new List<UISpriteData> ();
				for (int i =0; i < mAtlas.spriteList.Count; i++) {
					item = mAtlas.spriteList [i];
					string path = Application.dataPath + "/" + item.path;
					path = path.Replace ("/upgradeRes/", "/upgradeRes4Dev/");
                    path = path.Replace ("/upgradeRes4Publish/", "/upgradeRes4Dev/");
					if (!File.Exists (path)) {
						Debug.Log (path);
						list.Add (item);
					}
				}
				deleteSprites (list);
			}
		}
		#endregion //end ======add by chenbin

		if (mat != null || mAtlas.isBorrowSpriteMode) 	// modify by chenbin
		{
			TextAsset ta = EditorGUILayout.ObjectField("TP Import", null, typeof(TextAsset), false) as TextAsset;

			if (ta != null)
			{
				// Ensure that this atlas has valid import settings
				if (mAtlas.texture != null) NGUIEditorTools.ImportTexture(mAtlas.texture, false, false, !mAtlas.premultipliedAlpha);

				NGUIEditorTools.RegisterUndo("Import Sprites", mAtlas);
				NGUIJson.LoadSpriteData(mAtlas, ta);
				if (sprite != null) sprite = mAtlas.GetSprite(sprite.name);
				mAtlas.MarkAsChanged();
			}

			float pixelSize = EditorGUILayout.FloatField("Pixel Size", mAtlas.pixelSize, GUILayout.Width(120f));

			if (pixelSize != mAtlas.pixelSize)
			{
				NGUIEditorTools.RegisterUndo("Atlas Change", mAtlas);
				mAtlas.pixelSize = pixelSize;
			}
		}
			
		if (mAtlas.spriteMaterial != null || mAtlas.isBorrowSpriteMode) 	//modify by chenbin
		{
			Color blueColor = new Color(0f, 0.7f, 1f, 1f);
			Color greenColor = new Color(0.4f, 1f, 0f, 1f);

			if (sprite == null && mAtlas.spriteList.Count > 0)
			{
				string spriteName = NGUISettings.selectedSprite;
				if (!string.IsNullOrEmpty(spriteName)) sprite = mAtlas.GetSprite(spriteName);
//				if (sprite == null) sprite = mAtlas.spriteList[0];  // delete by chenbin
				#region add by chenbin
				if (sprite == null) {
					if (mAtlas.isBorrowSpriteMode) {
						sprite = mAtlas.spriteList [0];
						spriteName = sprite.name;
						sprite = mAtlas.borrowSpriteByname (spriteName, null);
					} else {
						sprite = mAtlas.spriteList [0];
					}
				}
				#endregion
			}

			if (sprite != null)
			{
				if (sprite == null) return;
					
//				Texture2D tex = mAtlas.spriteMaterial.mainTexture as Texture2D;	//delete by chenbin
				#region add by chenbin
				Texture2D tex = null;
				if (mAtlas != null) {
					if (mAtlas.isBorrowSpriteMode) {
						if (sprite != null && sprite.material != null) {
							tex = sprite.material.mainTexture as Texture2D;
						}
					} else {
						tex = mAtlas.spriteMaterial.mainTexture as Texture2D;
					}
				}
				#endregion

						
//				if (tex != null)		//modify by chenbin
				if (tex != null || (mAtlas != null && mAtlas.isBorrowSpriteMode && sprite != null))  //modify by chenbin
				{
					if (!NGUIEditorTools.DrawHeader("Sprite Details")) return;

					NGUIEditorTools.BeginContents();

					GUILayout.Space(3f);
					NGUIEditorTools.DrawAdvancedSpriteField(mAtlas, sprite.name, SelectSprite, true);
					GUILayout.Space(6f);

					GUI.changed = false;

					GUI.backgroundColor = greenColor;
					NGUIEditorTools.IntVector sizeA = NGUIEditorTools.IntPair("Dimensions", "X", "Y", sprite.x, sprite.y);
					NGUIEditorTools.IntVector sizeB = NGUIEditorTools.IntPair(null, "Width", "Height", sprite.width, sprite.height);

					EditorGUILayout.Separator();
					GUI.backgroundColor = blueColor;
					NGUIEditorTools.IntVector borderA = NGUIEditorTools.IntPair("Border", "Left", "Right", sprite.borderLeft, sprite.borderRight);
					NGUIEditorTools.IntVector borderB = NGUIEditorTools.IntPair(null, "Bottom", "Top", sprite.borderBottom, sprite.borderTop);

					EditorGUILayout.Separator();
					GUI.backgroundColor = Color.white;
					NGUIEditorTools.IntVector padA = NGUIEditorTools.IntPair("Padding", "Left", "Right", sprite.paddingLeft, sprite.paddingRight);
					NGUIEditorTools.IntVector padB = NGUIEditorTools.IntPair(null, "Bottom", "Top", sprite.paddingBottom, sprite.paddingTop);

					if (GUI.changed)
					{
						NGUIEditorTools.RegisterUndo("Atlas Change", mAtlas);
						
						sprite.x = sizeA.x;
						sprite.y = sizeA.y;
						sprite.width = sizeB.x;
						sprite.height = sizeB.y;

						sprite.paddingLeft = padA.x;
						sprite.paddingRight = padA.y;
						sprite.paddingBottom = padB.x;
						sprite.paddingTop = padB.y;

						sprite.borderLeft = borderA.x;
						sprite.borderRight = borderA.y;
						sprite.borderBottom = borderB.x;
						sprite.borderTop = borderB.y;

						MarkSpriteAsDirty();
					}

					GUILayout.Space(3f);

					GUILayout.BeginHorizontal();

					if (GUILayout.Button("Duplicate"))
					{
						string newName = UIAtlasMaker.DuplicateSprite(mAtlas, sprite.name); // modify by chenbin
						if (newName != null) NGUISettings.selectedSprite = newName;	// modify by chenbin
					}

					if (GUILayout.Button("Save As..."))
					{
#if UNITY_3_5
						string path = EditorUtility.SaveFilePanel("Save As",
							NGUISettings.currentPath, sprite.name + ".png", "png");
#else
						string path = EditorUtility.SaveFilePanelInProject("Save As",
							sprite.name + ".png", "png",
							"Extract sprite into which file?", NGUISettings.currentPath);
#endif

						if (!string.IsNullOrEmpty(path))
						{
							NGUISettings.currentPath = System.IO.Path.GetDirectoryName(path);
							UIAtlasMaker.SpriteEntry se = UIAtlasMaker.ExtractSprite(mAtlas, sprite.name);

							if (se != null)
							{
								byte[] bytes = se.tex.EncodeToPNG();
								File.WriteAllBytes(path, bytes);
								AssetDatabase.ImportAsset(path);
								if (se.temporaryTexture) DestroyImmediate(se.tex);
							}
						}
					}
					GUILayout.EndHorizontal();
					NGUIEditorTools.EndContents();
				}

				if (NGUIEditorTools.DrawHeader("Modify"))
				{
					NGUIEditorTools.BeginContents();

					EditorGUILayout.BeginHorizontal();
					GUILayout.Space(20f);
					EditorGUILayout.BeginVertical();

					NGUISettings.backgroundColor = EditorGUILayout.ColorField("Background", NGUISettings.backgroundColor);

					if (GUILayout.Button("Add a Shadow")) AddShadow(sprite);
					if (GUILayout.Button("Add a Soft Outline")) AddOutline(sprite);

					if (GUILayout.Button("Add a Transparent Border")) AddTransparentBorder(sprite);
					if (GUILayout.Button("Add a Clamped Border")) AddClampedBorder(sprite);
					if (GUILayout.Button("Add a Tiled Border")) AddTiledBorder(sprite);
					EditorGUI.BeginDisabledGroup(!sprite.hasBorder);
					if (GUILayout.Button("Crop Border")) CropBorder(sprite);
					EditorGUI.EndDisabledGroup();

					EditorGUILayout.EndVertical();
					GUILayout.Space(20f);
					EditorGUILayout.EndHorizontal();

					NGUIEditorTools.EndContents();
				}

				if (NGUIEditorTools.previousSelection != null)
				{
					GUILayout.Space(3f);
					GUI.backgroundColor = Color.green;

					if (GUILayout.Button("<< Return to " + NGUIEditorTools.previousSelection.name))
					{
						NGUIEditorTools.SelectPrevious();
					}
					GUI.backgroundColor = Color.white;
				}
			}
		}
	}

	/// <summary>
	/// Sprite selection callback.
	/// </summary>

	void SelectSprite (string spriteName)
	{
		if (NGUISettings.selectedSprite != spriteName)
		{
			NGUISettings.selectedSprite = spriteName;
			Repaint();
		}
	}

	/// <summary>
	/// All widgets have a preview.
	/// </summary>

	public override bool HasPreviewGUI () { return true; }

	/// <summary>
	/// Draw the sprite preview.
	/// </summary>

	public override void OnPreviewGUI (Rect rect, GUIStyle background)
	{
		UISpriteData sprite = (mAtlas != null) ? mAtlas.GetSprite(NGUISettings.selectedSprite) : null;
		if (sprite == null) return;
		
		//		Texture2D tex = mAtlas.texture as Texture2D;		//modify by chenbin
		#region modify by chenbin
		Texture2D tex;
		if (mAtlas.isBorrowSpriteMode) {
			if (sprite.material != null) {
				tex = sprite.material.mainTexture as Texture2D;
			} else {
				tex = null;
			}
		} else {
			tex = mAtlas.texture as Texture2D;
		}
		#endregion
		if (tex != null) NGUIEditorTools.DrawSprite(tex, rect, sprite, Color.white);
	}

	/// <summary>
	/// Add a transparent border around the sprite.
	/// </summary>

	void AddTransparentBorder (UISpriteData sprite)
	{
		List<UIAtlasMaker.SpriteEntry> sprites = new List<UIAtlasMaker.SpriteEntry>();
		UIAtlasMaker.ExtractSprites(mAtlas, sprites);
		UIAtlasMaker.SpriteEntry se = null;

		for (int i = 0; i < sprites.Count; ++i)
		{
			if (sprites[i].name == sprite.name)
			{
				se = sprites[i];
				break;
			}
		}

		if (se != null)
		{
			int w1 = se.tex.width;
			int h1 = se.tex.height;

			int w2 = w1 + 2;
			int h2 = h1 + 2;

			Color32[] c2 = NGUIEditorTools.AddBorder(se.tex.GetPixels32(), w1, h1);

			if (se.temporaryTexture) DestroyImmediate(se.tex);

			++se.borderLeft;
			++se.borderRight;
			++se.borderTop;
			++se.borderBottom;

			se.tex = new Texture2D(w2, h2);
			se.tex.name = sprite.name;
			se.tex.SetPixels32(c2);
			se.tex.Apply();
			se.temporaryTexture = true;

			UIAtlasMaker.UpdateAtlas(mAtlas, sprites);

			DestroyImmediate(se.tex);
			se.tex = null;
		}
	}

	/// <summary>
	/// Add a border around the sprite that extends the existing edge pixels.
	/// </summary>

	void AddClampedBorder (UISpriteData sprite)
	{
		List<UIAtlasMaker.SpriteEntry> sprites = new List<UIAtlasMaker.SpriteEntry>();
		UIAtlasMaker.ExtractSprites(mAtlas, sprites);
		UIAtlasMaker.SpriteEntry se = null;

		for (int i = 0; i < sprites.Count; ++i)
		{
			if (sprites[i].name == sprite.name)
			{
				se = sprites[i];
				break;
			}
		}

		if (se != null)
		{
			int w1 = se.tex.width - se.borderLeft - se.borderRight;
			int h1 = se.tex.height - se.borderBottom - se.borderTop;

			int w2 = se.tex.width + 2;
			int h2 = se.tex.height + 2;

			Color32[] c1 = se.tex.GetPixels32();
			Color32[] c2 = new Color32[w2 * h2];

			for (int y2 = 0; y2 < h2; ++y2)
			{
				int y1 = se.borderBottom + NGUIMath.ClampIndex(y2 - se.borderBottom - 1, h1);

				for (int x2 = 0; x2 < w2; ++x2)
				{
					int x1 = se.borderLeft + NGUIMath.ClampIndex(x2 - se.borderLeft - 1, w1);
					c2[x2 + y2 * w2] = c1[x1 + y1 * se.tex.width];
				}
			}

			if (se.temporaryTexture) DestroyImmediate(se.tex);

			++se.borderLeft;
			++se.borderRight;
			++se.borderTop;
			++se.borderBottom;

			se.tex = new Texture2D(w2, h2);
			se.tex.name = sprite.name;
			se.tex.SetPixels32(c2);
			se.tex.Apply();
			se.temporaryTexture = true;

			UIAtlasMaker.UpdateAtlas(mAtlas, sprites);

			DestroyImmediate(se.tex);
			se.tex = null;
		}
	}

	/// <summary>
	/// Add a border around the sprite that copies the pixels from the opposite side, making it possible for the sprite to tile without seams.
	/// </summary>

	void AddTiledBorder (UISpriteData sprite)
	{
		List<UIAtlasMaker.SpriteEntry> sprites = new List<UIAtlasMaker.SpriteEntry>();
		UIAtlasMaker.ExtractSprites(mAtlas, sprites);
		UIAtlasMaker.SpriteEntry se = null;

		for (int i = 0; i < sprites.Count; ++i)
		{
			if (sprites[i].name == sprite.name)
			{
				se = sprites[i];
				break;
			}
		}

		if (se != null)
		{
			int w1 = se.tex.width - se.borderLeft - se.borderRight;
			int h1 = se.tex.height - se.borderBottom - se.borderTop;

			int w2 = se.tex.width + 2;
			int h2 = se.tex.height + 2;

			Color32[] c1 = se.tex.GetPixels32();
			Color32[] c2 = new Color32[w2 * h2];

			for (int y2 = 0; y2 < h2; ++y2)
			{
				int y1 = se.borderBottom + NGUIMath.RepeatIndex(y2 - se.borderBottom - 1, h1);

				for (int x2 = 0; x2 < w2; ++x2)
				{
					int x1 = se.borderLeft + NGUIMath.RepeatIndex(x2 - se.borderLeft - 1, w1);
					c2[x2 + y2 * w2] = c1[x1 + y1 * se.tex.width];
				}
			}

			if (se.temporaryTexture) DestroyImmediate(se.tex);

			++se.borderLeft;
			++se.borderRight;
			++se.borderTop;
			++se.borderBottom;

			se.tex = new Texture2D(w2, h2);
			se.tex.name = sprite.name;
			se.tex.SetPixels32(c2);
			se.tex.Apply();
			se.temporaryTexture = true;

			UIAtlasMaker.UpdateAtlas(mAtlas, sprites);

			DestroyImmediate(se.tex);
			se.tex = null;
		}
	}

	/// <summary>
	/// Crop the border pixels around the sprite.
	/// </summary>

	void CropBorder (UISpriteData sprite)
	{
		List<UIAtlasMaker.SpriteEntry> sprites = new List<UIAtlasMaker.SpriteEntry>();
		UIAtlasMaker.ExtractSprites(mAtlas, sprites);
		UIAtlasMaker.SpriteEntry se = null;

		for (int i = 0; i < sprites.Count; ++i)
		{
			if (sprites[i].name == sprite.name)
			{
				se = sprites[i];
				break;
			}
		}

		if (se != null)
		{
			int w1 = se.tex.width;
			int h1 = se.tex.height;

			int w2 = w1 - se.borderLeft - se.borderRight;
			int h2 = h1 - se.borderTop - se.borderBottom;

			Color32[] c1 = se.tex.GetPixels32();
			Color32[] c2 = new Color32[w2 * h2];

			for (int y2 = 0; y2 < h2; ++y2)
			{
				int y1 = y2 + se.borderBottom;

				for (int x2 = 0; x2 < w2; ++x2)
				{
					int x1 = x2 + se.borderLeft;
					c2[x2 + y2 * w2] = c1[x1 + y1 * w1];
				}
			}

			se.borderLeft = 0;
			se.borderRight = 0;
			se.borderTop = 0;
			se.borderBottom = 0;

			if (se.temporaryTexture) DestroyImmediate(se.tex);

			se.tex = new Texture2D(w2, h2);
			se.tex.name = sprite.name;
			se.tex.SetPixels32(c2);
			se.tex.Apply();
			se.temporaryTexture = true;

			UIAtlasMaker.UpdateAtlas(mAtlas, sprites);

			DestroyImmediate(se.tex);
			se.tex = null;
		}
	}

	/// <summary>
	/// Add a dark shadow below and to the right of the sprite.
	/// </summary>

	void AddShadow (UISpriteData sprite)
	{
		List<UIAtlasMaker.SpriteEntry> sprites = new List<UIAtlasMaker.SpriteEntry>();
		UIAtlasMaker.ExtractSprites(mAtlas, sprites);
		UIAtlasMaker.SpriteEntry se = null;

		for (int i = 0; i < sprites.Count; ++i)
		{
			if (sprites[i].name == sprite.name)
			{
				se = sprites[i];
				break;
			}
		}

		if (se != null)
		{
			int w1 = se.tex.width;
			int h1 = se.tex.height;

			int w2 = w1 + 2;
			int h2 = h1 + 2;

			Color32[] c2 = NGUIEditorTools.AddBorder(se.tex.GetPixels32(), w1, h1);
			NGUIEditorTools.AddShadow(c2, w2, h2, NGUISettings.backgroundColor);

			if (se.temporaryTexture) DestroyImmediate(se.tex);

			if ((se.borderLeft | se.borderRight | se.borderBottom | se.borderTop) != 0)
			{
				++se.borderLeft;
				++se.borderRight;
				++se.borderTop;
				++se.borderBottom;
			}

			se.tex = new Texture2D(w2, h2);
			se.tex.name = sprite.name;
			se.tex.SetPixels32(c2);
			se.tex.Apply();
			se.temporaryTexture = true;

			UIAtlasMaker.UpdateAtlas(mAtlas, sprites);

			DestroyImmediate(se.tex);
			se.tex = null;
		}
	}

	/// <summary>
	/// Add a dark shadowy outline around the sprite, giving it some visual depth.
	/// </summary>

	void AddOutline (UISpriteData sprite)
	{
		List<UIAtlasMaker.SpriteEntry> sprites = new List<UIAtlasMaker.SpriteEntry>();
		UIAtlasMaker.ExtractSprites(mAtlas, sprites);
		UIAtlasMaker.SpriteEntry se = null;

		for (int i = 0; i < sprites.Count; ++i)
		{
			if (sprites[i].name == sprite.name)
			{
				se = sprites[i];
				break;
			}
		}

		if (se != null)
		{
			int w1 = se.tex.width;
			int h1 = se.tex.height;

			int w2 = w1 + 2;
			int h2 = h1 + 2;

			Color32[] c2 = NGUIEditorTools.AddBorder(se.tex.GetPixels32(), w1, h1);
			NGUIEditorTools.AddDepth(c2, w2, h2, NGUISettings.backgroundColor);

			if (se.temporaryTexture) DestroyImmediate(se.tex);

			if ((se.borderLeft | se.borderRight | se.borderBottom | se.borderTop) != 0)
			{
				++se.borderLeft;
				++se.borderRight;
				++se.borderTop;
				++se.borderBottom;
			}

			se.tex = new Texture2D(w2, h2);
			se.tex.name = sprite.name;
			se.tex.SetPixels32(c2);
			se.tex.Apply();
			se.temporaryTexture = true;

			UIAtlasMaker.UpdateAtlas(mAtlas, sprites);

			DestroyImmediate(se.tex);
			se.tex = null;
		}
	}
				
	#region add by chenbin
	void createMaterial (Texture tex)
	{
		//chang textures type
		string path = AssetDatabase.GetAssetPath (tex); 
		TextureImporter textureImporter = AssetImporter.GetAtPath (path) as TextureImporter; 
		textureImporter.textureType = TextureImporterType.GUI;    
		textureImporter.filterMode = FilterMode.Trilinear;		//改成这种模式好像更省内存
		AssetDatabase.ImportAsset (path); 
		
		//create material==========
		// Try to load the material
		path = path.Substring (0, path.LastIndexOf ("."));
		string matPath = path + ".mat";
		Material mat = AssetDatabase.LoadAssetAtPath (matPath, typeof(Material)) as Material;
		
		// If the material doesn't exist, create it
		if (mat == null) {
			Shader shader = Shader.Find ("Unlit/Transparent Colored");
			mat = new Material (shader);
			mat.mainTexture = tex;
			
			// Save the material
			AssetDatabase.CreateAsset (mat, matPath);
			AssetDatabase.Refresh ();
			
			// Load the material so it's usable
			mat = AssetDatabase.LoadAssetAtPath (matPath, typeof(Material)) as Material;
		}
	}
	
	public static void deleteSprites (List<UISpriteData> delList)
	{
		List<UISpriteData> list = NGUISettings.atlas.spriteList;
		foreach (UISpriteData s in delList) {
			list.Remove (s);
		}
		NGUISettings.atlas.spriteMap.Clear ();
		for (int i = 0; i < NGUISettings.atlas.spriteList.Count; i++) {
			UISpriteData tmpsp = NGUISettings.atlas.spriteList [i];
			NGUISettings.atlas.spriteMap [tmpsp.name] = i;
		}
		UnityEditor.EditorUtility.SetDirty (NGUISettings.atlas.gameObject);
	}
	#endregion
}
