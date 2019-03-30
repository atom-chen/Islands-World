//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Sprite is a textured element in the UI hierarchy.
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Sprite")]
public class UISprite : UIBasicSprite
{
	public string atlasName;	//add by chenbin

	// Cached and saved values
	[HideInInspector][SerializeField] UIAtlas mAtlas;
	[HideInInspector][SerializeField] string mSpriteName;

	// Deprecated, no longer used
	[HideInInspector][SerializeField] bool mFillCenter = true;

	[System.NonSerialized] protected UISpriteData mSprite;
	[System.NonSerialized] bool mSpriteSet = false;

	/// <summary>
	/// Retrieve the material used by the font.
	/// </summary>

//	public override Material material { get { return (mAtlas != null) ? mAtlas.spriteMaterial : null; } }
	#region add by chenbin
	public override Material material {
		get {
			Material mat = base.material;
			if (isGrayMode) {
				mat = grayMaterial;
			} else {
				if (mAtlas != null && mAtlas.isBorrowSpriteMode) {
					if (mSprite == null)
						mSprite = GetAtlasSprite ();
					mat = (mSprite != null) ? mSprite.material : null;//modify by chenbin
				} else {
					mat = (mAtlas != null) ? mAtlas.spriteMaterial : null;
					mSprite = null;
				}
			}
			return mat;
		}
	}
	#endregion


	
	#region //chenbin add
	new void OnEnable ()
	{
		base.OnEnable ();
		refresh ();
	}
	
	new void OnDisable ()
	{
		base.OnDisable ();
		
//		if (_grayMaterial != null) {
//			_grayMaterial.mainTexture = null;
//			GameObject.DestroyImmediate (_grayMaterial);
//			_grayMaterial = null;
//		}
		
		if (mAtlas != null) {
			if (mAtlas.isBorrowSpriteMode) {
				if (mSprite != null && !string.IsNullOrEmpty (mSprite.name)) {
					mAtlas.returnSpriteByname (mSprite.name);		// add by chenbin
				}
				mSprite = null;
			}
		}
	}

	public virtual void refresh ()
	{
		if (!gameObject.activeInHierarchy || string.IsNullOrEmpty(spriteName))
			return;
		if (mAtlas != null) {
			if (mAtlas.isBorrowSpriteMode) { // && mSprite == null) {
				if (mSprite != null && mSprite.name != spriteName) {
					mAtlas.returnSpriteByname (mSprite.name);
					mSprite = null;
				} 
				if (mSprite == null) {
					SetAtlasSprite (mAtlas.borrowSpriteByname (spriteName, this));		// add by chenbin
					if (mSprite != null) {
						if (panel != null) 						//add by chenbin
							panel.RemoveWidget (this);	//add by chenbin
						if (panel != null) 						//add by chenbin
							panel.AddWidget (this);		//add by chenbin
						MarkAsChanged ();
					}
				} else {
					mSpriteSet = true;		// add by chenbin
				}
			} else {
				if (mSprite == null) {
					SetAtlasSprite (mAtlas.GetSprite (spriteName));
				}
			}
		}
	}

	public static Hashtable grayMatMap = new Hashtable();
	Material _grayMaterial;
	
	public Material grayMaterial {
		get {
			if (mSprite != null) {
				_grayMaterial = grayMatMap [mSprite.path] as Material;
			} else {
				refresh ();
				return null;
			}
			if (_grayMaterial == null) {
				if (mSprite != null  && mSprite.material != null) {
					if (grayMatMap [mSprite.path] == null) {
						Shader shader = Shader.Find ("Unlit/Transparent Colore Gray");
						grayMatMap [mSprite.path] = new Material (shader);
					}
					_grayMaterial = grayMatMap [mSprite.path] as Material;
					_grayMaterial.mainTexture = mSprite.material.mainTexture;
				} else {
					refresh ();
				}
			} else {
				if (mSprite != null && mSprite.material != null) {
					_grayMaterial.mainTexture = mSprite.material.mainTexture;
				} else {
					refresh ();
				}
			}
			return _grayMaterial;
		}
		set {
			_grayMaterial = value;
		}
	}
	
	[SerializeField]
	bool isGrayMode = false;
	//设置成灰度图
	public void setGray ()
	{
		if(isGrayMode) return;
		isGrayMode = true;
		mSpriteSet = true;
		mChanged = true;
		MarkAsChanged();
		if(panel != null) {
			panel.RebuildAllDrawCalls ();
		}
	}
	
	public void unSetGray ()
	{
		if(!isGrayMode) return;
		isGrayMode = false;
		mSpriteSet = true;
		mChanged = true;
		MarkAsChanged();
		if(panel != null) {
			panel.RebuildAllDrawCalls ();
		}
	}
	#endregion

	/// <summary>
	/// Atlas used by this widget.
	/// </summary>
	public UIAtlas atlas
	{
		get
		{
			return mAtlas;
		}
		set
		{
			if (mAtlas != value)
			{
				RemoveFromPanel();

				mAtlas = value;
				mSpriteSet = false;
				mSprite = null;
				if(mAtlas != null) {
					atlasName = mAtlas.name;	//add by chenbin
				}

				// Automatically choose the first sprite
				if (string.IsNullOrEmpty(mSpriteName))
				{
					if (mAtlas != null && mAtlas.spriteList.Count > 0)
					{
						if(mAtlas.isBorrowSpriteMode) {		//add by chenbin
							spriteName = mAtlas.spriteList [0].name;		//add by chenbin
						} else {
							SetAtlasSprite(mAtlas.spriteList[0]);
							if (mSprite != null) { // add by chenbin
								mSpriteName = mSprite.name;
							}
						}
					}
				}
				
				#region add by chenbin
//				if (mAtlas != null && mAtlas.isBorrowSpriteMode) {
//					SetAtlasSprite (mAtlas.borrowSpriteByname (mSpriteName, this));
//				}
				#endregion add end chenbin

				// Re-link the sprite
				if (!string.IsNullOrEmpty(mSpriteName))
				{
					string sprite = mSpriteName;
					mSpriteName = "";
					spriteName = sprite;
					MarkAsChanged();
				}
			}
		}
	}

	/// <summary>
	/// Sprite within the atlas used to draw this widget.
	/// </summary>
 
	public string spriteName
	{
		get
		{
			return mSpriteName;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				// If the sprite name hasn't been set yet, no need to do anything
				if (string.IsNullOrEmpty(mSpriteName)) return;

				// Clear the sprite name and the sprite reference
				mSpriteName = "";
				mSprite = null;
				mChanged = true;
				mSpriteSet = false;
			}
			else if (mSpriteName != value)
			{
				#region add by chenbin
				if (mAtlas != null && mAtlas.isBorrowSpriteMode) {
					//					SetAtlasSprite(mAtlas.borrowSpriteByname (value, this));
					mSpriteName = value;
					mSpriteSet = false;
					mChanged = true;
					refresh ();
				} else {
					// If the sprite name changes, the sprite reference should also be updated
					mSpriteName = value;
					mChanged = true;
					mSpriteSet = false;
					mSprite = null;
					MarkAsChanged ();	//add by chenbin
				}
				#endregion add end chenbin
			}
		}
	}

	/// <summary>
	/// Is there a valid sprite to work with?
	/// </summary>

	public bool isValid { get { return GetAtlasSprite() != null; } }

	/// <summary>
	/// Whether the center part of the sprite will be filled or not. Turn it off if you want only to borders to show up.
	/// </summary>

	[System.Obsolete("Use 'centerType' instead")]
	public bool fillCenter
	{
		get
		{
			return centerType != AdvancedType.Invisible;
		}
		set
		{
			if (value != (centerType != AdvancedType.Invisible))
			{
				centerType = value ? AdvancedType.Sliced : AdvancedType.Invisible;
				MarkAsChanged();
			}
		}
	}

	/// <summary>
	/// Sliced sprites generally have a border. X = left, Y = bottom, Z = right, W = top.
	/// </summary>

	public override Vector4 border
	{
		get
		{
			UISpriteData sp = GetAtlasSprite();
			if (sp == null) return base.border;
			return new Vector4(sp.borderLeft, sp.borderBottom, sp.borderRight, sp.borderTop);
		}
	}

	/// <summary>
	/// Size of the pixel -- used for drawing.
	/// </summary>

	override public float pixelSize { get { return mAtlas != null ? mAtlas.pixelSize : 1f; } }

	/// <summary>
	/// Minimum allowed width for this widget.
	/// </summary>

	override public int minWidth
	{
		get
		{
			if (type == Type.Sliced || type == Type.Advanced)
			{
				float ps = pixelSize;
				Vector4 b = border * pixelSize;
				int min = Mathf.RoundToInt(b.x + b.z);

				UISpriteData sp = GetAtlasSprite();
				if (sp != null) min += Mathf.RoundToInt(ps * (sp.paddingLeft + sp.paddingRight));

				return Mathf.Max(base.minWidth, ((min & 1) == 1) ? min + 1 : min);
			}
			return base.minWidth;
		}
	}

	/// <summary>
	/// Minimum allowed height for this widget.
	/// </summary>

	override public int minHeight
	{
		get
		{
			if (type == Type.Sliced || type == Type.Advanced)
			{
				Vector4 b = border * pixelSize;
				int min = Mathf.RoundToInt(b.y + b.w);

				UISpriteData sp = GetAtlasSprite();
				if (sp != null) min += sp.paddingTop + sp.paddingBottom;

				return Mathf.Max(base.minHeight, ((min & 1) == 1) ? min + 1 : min);
			}
			return base.minHeight;
		}
	}

	/// <summary>
	/// Sprite's dimensions used for drawing. X = left, Y = bottom, Z = right, W = top.
	/// This function automatically adds 1 pixel on the edge if the sprite's dimensions are not even.
	/// It's used to achieve pixel-perfect sprites even when an odd dimension sprite happens to be centered.
	/// </summary>

	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 offset = pivotOffset;

			float x0 = -offset.x * mWidth;
			float y0 = -offset.y * mHeight;
			float x1 = x0 + mWidth;
			float y1 = y0 + mHeight;

			if (GetAtlasSprite() != null && mType != Type.Tiled)
			{
				int padLeft = mSprite.paddingLeft;
				int padBottom = mSprite.paddingBottom;
				int padRight = mSprite.paddingRight;
				int padTop = mSprite.paddingTop;

				float ps = pixelSize;

				if (ps != 1f)
				{
					padLeft = Mathf.RoundToInt(ps * padLeft);
					padBottom = Mathf.RoundToInt(ps * padBottom);
					padRight = Mathf.RoundToInt(ps * padRight);
					padTop = Mathf.RoundToInt(ps * padTop);
				}

				int w = mSprite.width + padLeft + padRight;
				int h = mSprite.height + padBottom + padTop;
				float px = 1f;
				float py = 1f;

				if (w > 0 && h > 0 && (mType == Type.Simple || mType == Type.Filled))
				{
					if ((w & 1) != 0) ++padRight;
					if ((h & 1) != 0) ++padTop;

					px = (1f / w) * mWidth;
					py = (1f / h) * mHeight;
				}

				if (mFlip == Flip.Horizontally || mFlip == Flip.Both)
				{
					x0 += padRight * px;
					x1 -= padLeft * px;
				}
				else
				{
					x0 += padLeft * px;
					x1 -= padRight * px;
				}

				if (mFlip == Flip.Vertically || mFlip == Flip.Both)
				{
					y0 += padTop * py;
					y1 -= padBottom * py;
				}
				else
				{
					y0 += padBottom * py;
					y1 -= padTop * py;
				}
			}

			Vector4 br = (mAtlas != null) ? border * pixelSize : Vector4.zero;

			float fw = br.x + br.z;
			float fh = br.y + br.w;

			float vx = Mathf.Lerp(x0, x1 - fw, mDrawRegion.x);
			float vy = Mathf.Lerp(y0, y1 - fh, mDrawRegion.y);
			float vz = Mathf.Lerp(x0 + fw, x1, mDrawRegion.z);
			float vw = Mathf.Lerp(y0 + fh, y1, mDrawRegion.w);

			return new Vector4(vx, vy, vz, vw);
		}
	}

	/// <summary>
	/// Whether the texture is using a premultiplied alpha material.
	/// </summary>

	public override bool premultipliedAlpha { get { return (mAtlas != null) && mAtlas.premultipliedAlpha; } }

	/// <summary>
	/// Retrieve the atlas sprite referenced by the spriteName field.
	/// </summary>
	public UISpriteData GetAtlasSprite1 ()
	{
		if (!mSpriteSet) mSprite = null;

		if (mSprite == null && mAtlas != null)
		{
			if (!string.IsNullOrEmpty(mSpriteName))
			{
				UISpriteData sp = mAtlas.GetSprite(mSpriteName);
				if (sp == null) return null;
				SetAtlasSprite(sp);
			}

			if (mSprite == null && mAtlas.spriteList.Count > 0)
			{
				UISpriteData sp = mAtlas.spriteList[0];
				if (sp == null) return null;
				SetAtlasSprite(sp);

				if (mSprite == null)
				{
					Debug.LogError(mAtlas.name + " seems to have a null sprite!");
					return null;
				}
				mSpriteName = mSprite.name;
			}
		}
		return mSprite;
	}
	
	public UISpriteData GetAtlasSprite2 ()
	{
		if (!mSpriteSet) mSprite = null;

		if (mSprite == null && mAtlas != null)
		{
			if (!string.IsNullOrEmpty(spriteName))
			{
//				UISpriteData sp = mAtlas.GetSprite(mSpriteName);
				#region add by chenbin
				UISpriteData sp = null;//modify by chenbin
				if (mAtlas != null && mAtlas.isBorrowSpriteMode) {		//modify by chenbin
					sp = mAtlas.borrowSpriteByname (spriteName, this);//modify by chenbin
				} else {
					sp = mAtlas.GetSprite (spriteName);
				}
				#endregion
				if (sp == null) return null;
				SetAtlasSprite(sp);
			}

			if (mSprite == null && mAtlas.spriteList.Count > 0)
			{
				if(mAtlas.isBorrowSpriteMode) {
					return null;	//add by chenbin
				}

				UISpriteData sp = mAtlas.spriteList[0];
				if (sp == null) return null;
				SetAtlasSprite(sp);

				if (mSprite == null)
				{
					Debug.LogError(mAtlas.name + " seems to have a null sprite!");
					return null;
				}
				mSpriteName = mSprite.name;
			}
		}
		return mSprite;
	}
	
	public UISpriteData GetAtlasSprite ()
	{
		if (mAtlas != null && mAtlas.isBorrowSpriteMode)
			return GetAtlasSprite2();
		return GetAtlasSprite1();
	}

	/// <summary>
	/// Set the atlas sprite directly.
	/// </summary>
	protected void SetAtlasSprite1 (UISpriteData sp)
	{
		mChanged = true;
		mSpriteSet = true;

		if (sp != null)
		{
			mSprite = sp;
			mSpriteName = sp.name;
		}
		else
		{
			mSpriteName = (mSprite != null) ? mSprite.name : "";
			mSprite = sp;
		}
	}

	protected void SetAtlasSprite2 (UISpriteData sp)
	{
		if (sp != null ) {
			if (atlas != null && atlas.isBorrowSpriteMode) {
				if ((string.IsNullOrEmpty(spriteName) || spriteName == sp.name) && NGUITools.GetActive (gameObject)) { // modify by chenbin
					mSprite = sp;
					mSpriteSet = true;		// add by chenbin
					mChanged = true;		// add by chenbin
					MarkAsChanged ();
				} else {
					atlas.returnSpriteByname (sp.name);
				}
			} else {
				MarkAsChanged ();
			}
		}
	}
	
	protected void SetAtlasSprite (UISpriteData sp)
	{
		if (mAtlas != null && mAtlas.isBorrowSpriteMode) {	
			SetAtlasSprite2(sp);
			return;
		} 
		SetAtlasSprite1(sp);
	}
	
	/// <summary>
	/// Adjust the scale of the widget to make it pixel-perfect.
	/// </summary>

	public override void MakePixelPerfect ()
	{
		if (!isValid) return;
		base.MakePixelPerfect();
		if (mType == Type.Tiled) return;

		UISpriteData sp = GetAtlasSprite();
		if (sp == null) return;

		Texture tex = mainTexture;
		if (tex == null) return;

		if (mType == Type.Simple || mType == Type.Filled || !sp.hasBorder)
		{
			if (tex != null)
			{
				int x = Mathf.RoundToInt(pixelSize * (sp.width + sp.paddingLeft + sp.paddingRight));
				int y = Mathf.RoundToInt(pixelSize * (sp.height + sp.paddingTop + sp.paddingBottom));
				
				if ((x & 1) == 1) ++x;
				if ((y & 1) == 1) ++y;

				width = x;
				height = y;
			}
		}
	}

	/// <summary>
	/// Auto-upgrade.
	/// </summary>

	protected override void OnInit ()
	{
		if (!mFillCenter)
		{
			mFillCenter = true;
			centerType = AdvancedType.Invisible;
#if UNITY_EDITOR
			NGUITools.SetDirty(this);
#endif
		}
		base.OnInit();
	}

	/// <summary>
	/// Update the UV coordinates.
	/// </summary>

	protected override void OnUpdate ()
	{
		base.OnUpdate();

		if (mChanged || !mSpriteSet)
		{
			mSpriteSet = true;
			if (atlas != null && !atlas.isBorrowSpriteMode) {//add by chenbin
				mSprite = null;
			}
			mChanged = true;
		}
	}

	/// <summary>
	/// Virtual function called by the UIPanel that fills the buffers.
	/// </summary>

	public override void OnFill (BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture tex = mainTexture;
		if (tex == null) return;

		if (mSprite == null) mSprite = atlas.GetSprite(spriteName);
		if (mSprite == null) return;

		Rect outer = new Rect(mSprite.x, mSprite.y, mSprite.width, mSprite.height);
		Rect inner = new Rect(mSprite.x + mSprite.borderLeft, mSprite.y + mSprite.borderTop,
			mSprite.width - mSprite.borderLeft - mSprite.borderRight,
			mSprite.height - mSprite.borderBottom - mSprite.borderTop);

		outer = NGUIMath.ConvertToTexCoords(outer, tex.width, tex.height);
		inner = NGUIMath.ConvertToTexCoords(inner, tex.width, tex.height);

		int offset = verts.size;
		Fill(verts, uvs, cols, outer, inner);

		if (onPostFill != null)
			onPostFill(this, offset, verts, uvs, cols);
	}
}
