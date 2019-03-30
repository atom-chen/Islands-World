//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// UI Atlas contains a collection of sprites inside one large texture atlas.
/// </summary>
using System.Collections;
using System.IO;

[AddComponentMenu ("NGUI/UI/Atlas")]
public class UIAtlas : MonoBehaviour
{
    #region add by chenbin
	public static Hashtable retainCounter = new Hashtable (); //引用计数器
	public static Hashtable assetBundleMap = new Hashtable (); //引用AssetBundle
	static Hashtable notifySprites = new Hashtable ();

	public static Hashtable materailPool = new Hashtable ();
	public static object onBorrowSpriteCallback;
	#endregion

	// Legacy functionality, removed in 3.0. Do not use.
	[System.Serializable]
	class Sprite
	{
		public string name = "Unity Bug";
		public Rect outer = new Rect (0f, 0f, 1f, 1f);
		public Rect inner = new Rect (0f, 0f, 1f, 1f);
		public bool rotated = false;

		// Padding is needed for trimmed sprites and is relative to sprite width and height
		public float paddingLeft = 0f;
		public float paddingRight = 0f;
		public float paddingTop = 0f;
		public float paddingBottom = 0f;
		
		#region add by chenbin
		//====================start
		public string path = "";
		[NonSerialized]
		public Material
			material = null;
		//====================end
		#endregion

		public bool hasPadding { get { return paddingLeft != 0f || paddingRight != 0f || paddingTop != 0f || paddingBottom != 0f; } }
	}

	/// <summary>
	/// Legacy functionality, removed in 3.0. Do not use.
	/// </summary>

	enum Coordinates
	{
		Pixels,
		TexCoords,
	}

	// Material used by this atlas. Name is kept only for backwards compatibility, it used to be public.
	[HideInInspector]
	[SerializeField]
	Material
		material;

	// List of all sprites inside the atlas. Name is kept only for backwards compatibility, it used to be public.
	[HideInInspector]
	[SerializeField]
	List<UISpriteData>
		mSprites = new List<UISpriteData> ();

	// Size in pixels for the sake of MakePixelPerfect functions.
	[HideInInspector]
	[SerializeField]
	float
		mPixelSize = 1f;

	// Replacement atlas can be used to completely bypass this atlas, pulling the data from another one instead.
	[HideInInspector]
	[SerializeField]
	UIAtlas
		mReplacement;

	// Legacy functionality -- do not use
	[HideInInspector]
	[SerializeField]
	Coordinates
		mCoordinates = Coordinates.Pixels;
	[HideInInspector]
	[SerializeField]
	List<Sprite>
		sprites = new List<Sprite> ();

	// Whether the atlas is using a pre-multiplied alpha material. -1 = not checked. 0 = no. 1 = yes.
	int mPMA = -1;

	// Dictionary lookup to speed up sprite retrieval at run-time
	Dictionary<string, int> mSpriteIndices = new Dictionary<string, int> ();


	
	#region //add by chenbin=========start

	[HideInInspector]
	[SerializeField]
	bool
		_isBorrowSpriteMode;
	Hashtable mspritesMap = new Hashtable ();
    //add by chenbin

    public void init ()
	{
		foreach (DictionaryEntry item in mspritesMap) {
			returnSpriteByname (item.Key.ToString ());
		}
	}

	public bool isBorrowSpriteMode {
		get {
			return (mReplacement != null) ? mReplacement.isBorrowSpriteMode : _isBorrowSpriteMode;
		}
		set {
			if (mReplacement != null) {
				//mReplacement.useUnity3DType = value;
				mReplacement.isBorrowSpriteMode = value;
			} else {
				_isBorrowSpriteMode = value;
				//useUnity3DType = value;
			}
		}
	}

	public  Hashtable spriteMap {
		get {
			if (mReplacement != null) {
				return mReplacement.spriteMap;
			} else {
				if (mspritesMap.Count == 0) {
					for (int i = 0; i < mSprites.Count; i++) {
						UISpriteData tmpsp = mSprites [i];
						mspritesMap [tmpsp.name] = i;
					}
				}
				return mspritesMap;
			}
		}
		set {
			if (mReplacement != null) {
				mReplacement.spriteMap = value;
			} else {
				mspritesMap = value;
			}
		}
	}

	public static int releaseSpriteTime = 10;

	/// <summary>
	/// Returns the sprite byname.根据name释放sprite,add by chenbin
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	public void returnSpriteByname (string name)
	{
		if (mReplacement != null) {
			mReplacement.returnSpriteByname (name);
		} else {
			int i = spriteMap [name] == null ? -1 : (int)spriteMap [name];
			if (i < 0) {
				return;
			}
			//          Sprite s = sprites.Count > i ? sprites [i] : null;
			UISpriteData s = mSprites.Count > i ? mSprites [i] : null;
			if (s == null) {
				return;
			}
			int rc = retainCounter [s.path] == null ? 0 : (int)retainCounter [s.path];
			rc--;
			#if UNITY_EDITOR
			if (Coolape.CLAssetsManager.self != null
			    && !string.IsNullOrEmpty (Coolape.CLAssetsManager.self.debugKey)
			    && s.path.Contains (Coolape.CLAssetsManager.self.debugKey)) {
				Debug.LogError ("returnSpriteByname====" + s.path + "===" + name + "=====" + rc);
			}
			#endif
			retainCounter [s.path] = rc < 0 ? 0 : rc;
			if (rc <= 0) {
				s.lastReturnTime = System.DateTime.Now.AddSeconds (releaseSpriteTime).ToFileTime ();
				if (s.material != null && !releaseTex.Contains (name)) {
					releaseTex.Enqueue (name);
					ReleaseTexture ();
				}
			}
		}
		
	}

	Queue<string> releaseTex = new Queue<string> ();

	void ReleaseTexture ()
	{
		if (releaseTex.Count == 0) {
			return;
		}
		
		string name = releaseTex.Dequeue ();
		doReleaseTexture (name);
		CancelInvoke ("ReleaseTexture");
		Invoke ("ReleaseTexture", 0.2f);
	}

	/// <summary>
	/// Releases all textures imm.释放所有图片资源
	/// </summary>
	public void releaseAllTexturesImm ()
	{
		if (mReplacement != null) {
			mReplacement.releaseAllTexturesImm ();
		} else {
			while (releaseTex.Count > 0) {
				string name = releaseTex.Dequeue ();
				doReleaseTexture (name, true);
			}
		}
	}

	public void doReleaseTexture (string name)
	{
		doReleaseTexture (name, false);
	}

	public void doReleaseTexture (string name, bool force)
	{
		if (mReplacement != null) {
			mReplacement.doReleaseTexture (name, force);
			return;
		}
		if (isBorrowSprite) {
			return;
		}
		if (string.IsNullOrEmpty (name)) {
			return;
		}
		
		int i = spriteMap [name] == null ? -1 : (int)spriteMap [name];
		if (i < 0) {
			return;
		}
		
		UISpriteData sp = mSprites.Count > 1 ? mSprites [i] : null;
		if (sp == null) {
			return;
		}

		int rc = retainCounter [sp.path] == null ? 0 : (int)retainCounter [sp.path];
		if (rc <= 0) {
			if (!force && sp.lastReturnTime - System.DateTime.Now.ToFileTime () > 0) {
				if (!releaseTex.Contains (name)) {
					releaseTex.Enqueue (name);
				}
				CancelInvoke ("ReleaseTexture");
				Invoke ("ReleaseTexture", 5);
				return;
			}
			Material mat = materailPool[sp.path] as Material;
			if (mat != null && mat.mainTexture != null) {
				try {
					//=================================================
					#if UNITY_EDITOR
					if (Coolape.CLAssetsManager.self != null
					    && !string.IsNullOrEmpty (Coolape.CLAssetsManager.self.debugKey)
					    && sp.path.Contains (Coolape.CLAssetsManager.self.debugKey)) {
						Debug.LogError ("doReleaseTexture====" + sp.path);
					}
					#endif
					//if(!string.IsNullOrEmpty(mat.mainTexture.name)) {
					//	Resources.UnloadAsset (mat.mainTexture);
					//}
					
					// 其它引用了该图的都要释放
					for (int j = 0; j < mSprites.Count; j++) {
						UISpriteData tmpsp = mSprites [j];
						if (tmpsp.path == sp.path && tmpsp != sp) {
							tmpsp.material = null;
						}
					}
					sp.material = null;
                    materailPool[sp.path] = null;

					mat.mainTexture = null;
                    //DestroyImmediate (mat.mainTexture, true);
                    DestroyImmediate (mat, true);
					mat = null;
                    AssetBundle ab = assetBundleMap[sp.path] as AssetBundle;
                    if (ab != null)
                    {
                        ab.Unload(true);
                    }
                    assetBundleMap[sp.path] = null;
                    //=================================================
                } catch (System.Exception e) {
					Debug.LogError (sp.name + " err:" + e);
				}
			} 
		}
	}

	void setNotifySprite (string path, UISprite uisp, string spriteName, object callback, object args)
	{
		//		if (string.IsNullOrEmpty(path) || uisp == null) {
		if (string.IsNullOrEmpty (path)) {
			return;
		}
		ArrayList list = null;
		object temp = notifySprites [path];
		if (temp == null) {
			list = ArrayList.Synchronized (new ArrayList ());
			notifySprites [path] = list;
		} else {
			list = (ArrayList)temp;
		}
		if (!list.Contains (uisp)) {
			if (callback != null) {
				object[] objs = { uisp, callback, spriteName, args };
				list.Add (objs);
			} else {
				list.Add (uisp);
			}
			//          Debug.Log ("add sprite name ====" + uisp.spriteName);
			notifySprites [path] = list;
		}
	}

	void doNotifySprite (string path, Texture texture)
	{
		if (string.IsNullOrEmpty (path)) {
			return;
		}
		try {
			ArrayList list = null;
			object temp = notifySprites [path];
			UISprite sp = null;
			object item = null;
			object finishCallback = null;
			string spriteName = null;
			object args = null;
			if (temp != null) {
				list = (ArrayList)temp;
				if (list != null && list.Count > 0) {
					for (int i = 0; i < list.Count; i++) {
						item = list [i];
						if (item == null) {
							continue;
						}
						if (item is UISprite) {
							sp = (UISprite)(item); 
							if (texture != null && sp != null) {
								//                      Debug.LogWarning ("sp.refresh==" + path + "     " + sp.name);
								sp.refresh ();
							}
						} else {
							object[] objs = (object[])item;
							if (objs.Length > 3) {
								sp = (UISprite)(objs [0]);
								if (texture != null && sp != null) {
									//                      Debug.LogWarning ("sp.refresh==" + path + "     " + sp.name);
									sp.refresh ();
								}
								finishCallback = objs [1];
								spriteName = objs [2].ToString ();
								args = objs [3];

								Coolape.Utl.doCallback (finishCallback, sp, spriteName, args);
							}
						}
					}
					list.Clear ();
					notifySprites [path] = list;
				}
			}
		} catch(System.Exception e) {
			Debug.LogError (e);
		}
	}

	public bool hadGetSpriteTexture (string spriteName)
	{
		if (mReplacement != null) {
			return mReplacement.hadGetSpriteTexture (spriteName);
		}
		UISpriteData sd = getSpriteBorrowMode (spriteName);
		if (sd != null && UIAtlas.assetBundleMap [sd.path] != null) {
			return true;
		}
		return false;
	}

	public UISpriteData getSpriteBorrowMode (string name)
	{
		if (string.IsNullOrEmpty (name)) {
			return null;
		}
		if (mReplacement != null) {
			return mReplacement.getSpriteBorrowMode (name);
		}
		int i = spriteMap [name] == null ? -1 : (int)spriteMap [name];
		if (i < 0) {
			#if UNITY_EDITOR
			Debug.LogWarning ("can't find sprite ,name=[" + name + "],objname = [" + name + "]");
			#endif
			return null;
		}
		//      Sprite ret = sprites.Count > i ? sprites [i] : null;
		UISpriteData ret = mSprites.Count > i ? mSprites [i] : null;
		return ret;
	}

	bool isBorrowSprite = false;

	/// <summary>
	/// Gets the sprite byname.根据name 取得sprite ,add by chenbin
	/// </summary>
	/// <returns>
	/// The sprite byname.
	/// </returns>
	public UISpriteData borrowSpriteByname (string name, UISprite uisp)
	{
		return borrowSpriteByname (name, uisp, null, null);
	}

	public UISpriteData borrowSpriteByname (string name, UISprite uisp, object callback)
	{
		return borrowSpriteByname (name, uisp, callback, null);
	}

	public UISpriteData borrowSpriteByname (string name, UISprite uisp, object callback, object args)
	{
		if (name == null) {
			return null;
		}
		
		if (mReplacement != null) {
			return mReplacement.borrowSpriteByname (name, uisp, callback, args);
		}
		
		//      Debug.Log ("borrow name==" + name);
		int i = spriteMap [name] == null ? -1 : (int)spriteMap [name];
		if (i < 0) {
			#if UNITY_EDITOR
			Debug.LogWarning ("can't find sprite ,name=[" + name + "],objname = [" + (uisp != null ? uisp.name : "") + "]");
			#endif
			Coolape.Utl.doCallback (callback, uisp, name, args);
			return null;
		}
		
		//      Sprite ret = sprites.Count > i ? sprites [i] : null;
		UISpriteData ret = mSprites.Count > i ? mSprites [i] : null;
		if (ret == null) {
			#if UNITY_EDITOR
			Debug.LogWarning ("can't find sprite ,name=[" + name + "]");
			#endif
			Coolape.Utl.doCallback (callback, uisp, name, args);
			return ret;
		}
		
		isBorrowSprite = true;
		int rc = retainCounter [ret.path] == null ? 0 : (int)(retainCounter [ret.path]);

		if (ret.material == null || ret.material.mainTexture == null || assetBundleMap [ret.path] == null) {
//			rc = 0;
			Texture tt = null;
			try {
				if (assetBundleMap [ret.path] == null) {
					#if UNITY_EDITOR
					if (Application.isPlaying) {
						getTuxture (ret, uisp, callback, args);
						isBorrowSprite = false;
						return null;
					} else {
						string path = ret.path;
						path = path.Replace ("/upgradeRes/", "/upgradeRes4Publish/");
						path = path.Replace ("/upgradeRes4Publish/", "/upgradeRes4Dev/");
						assetBundleMap [ret.path] = Coolape.CLVerManager.self.getAtalsTexture4Edit (path);
					}
					#else
					getTuxture (ret, uisp, callback, args);
					isBorrowSprite = false;
					return null;
					#endif
				}
				if (assetBundleMap [ret.path] == null) {
					Debug.LogError (ret.path + " is null . name == " + name);
					isBorrowSprite = false;
					return null;
				}
			} catch (Exception e) {
				isBorrowSprite = false;
				Debug.LogError (e);
				return null;
			}
			
			#if UNITY_EDITOR
			if (Application.isPlaying) {
                if(assetBundleMap[ret.path] is AssetBundle) {
                    tt = (assetBundleMap[ret.path] as AssetBundle).mainAsset as Texture;
                } else {
                    getTuxture(ret, uisp, callback, args);
                    isBorrowSprite = false;
                    return null;
                }
			} else {
				if (assetBundleMap [ret.path].GetType () != typeof(Texture) &&
				    assetBundleMap [ret.path].GetType () != typeof(Texture2D)) {
					assetBundleMap [ret.path] = null;
					return null;
				}
				tt = (Texture)(assetBundleMap [ret.path]);
			}
			#else
			//tt = (Texture)(assetBundleMap [ret.path]);
            tt = (assetBundleMap [ret.path] as AssetBundle).mainAsset as Texture;
			#endif
			if (tt != null) {
				if (ret.material == null) {
					ret.material = getMaterail(tt, ret.path);
				} else {
					ret.material.mainTexture = tt;
				}
			} else {
				assetBundleMap [ret.path] = null;
				#if UNITY_EDITOR
				Debug.LogWarning ("can't find Texture in Resource path :[" + ret.path + "]");
				#endif
				return null;
			}
		}

		rc++;
		retainCounter [ret.path] = rc;
		isBorrowSprite = false;
		#if UNITY_EDITOR
		if (Coolape.CLAssetsManager.self != null
		    && !string.IsNullOrEmpty (Coolape.CLAssetsManager.self.debugKey)
		    && ret.path.Contains (Coolape.CLAssetsManager.self.debugKey)) {
			Debug.LogError ("borrow Sprite==" + ret.path + "==" + name + "====" + rc);
		}
		#endif
		Coolape.Utl.doCallback (callback, uisp, ret.name, args);
		#if UNITY_EDITOR
		Coolape.Utl.doCallback (onBorrowSpriteCallback, this, ret);
		#endif
		return ret;
	}

	public Material getMaterail(Texture tt, string texturePath) {
		Material mat = materailPool [texturePath] as Material;
		if(mat == null) {
			Shader shader = Shader.Find ("Unlit/Transparent Colored");
			mat = new Material (shader);
			mat.mainTexture = tt;
			materailPool [texturePath] = mat;
		}
		return mat;
	}

	void getTuxture (UISpriteData ret, UISprite uisp, object callback, object args)
	{
		try {
			Coolape.Callback cb = onGetTuxture;
			string path = ret.path;
			
			setNotifySprite (ret.path, uisp, ret.name, callback, args);
			if (Coolape.CLCfgBase.self.isEditMode) {
				path = path.Replace ("/upgradeRes/", "/upgradeRes4Publish/");
				path = path.Replace ("/upgradeRes4Dev/", "/upgradeRes4Publish/");
			} else {
				path = path.Replace ("/upgradeRes4Dev/", "/upgradeRes/");
				path = path.Replace ("/upgradeRes4Publish/", "/upgradeRes/");
			}
			
#if UNITY_ANDROID
			path = Path.GetDirectoryName (path) + "/Android/" + Path.GetFileNameWithoutExtension (path) + ".unity3d";
#elif UNITY_IOS
			path = Path.GetDirectoryName(path) + "/IOS/" + Path.GetFileNameWithoutExtension(path) + ".unity3d";
#elif UNITY_STANDALONE_WIN
            path = Path.GetDirectoryName(path) + "/Standalone/" + Path.GetFileNameWithoutExtension(path) + ".unity3d";
#elif UNITY_STANDALONE_OSX
            path = Path.GetDirectoryName(path) + "/StandaloneOSX/" + Path.GetFileNameWithoutExtension(path) + ".unity3d";
#endif
            Coolape.CLVerManager.self.getNewestRes (path, Coolape.CLAssetType.assetBundle, cb, false, ret.path, callback, uisp, ret.name);
		} catch (System.Exception e) {
			Debug.LogError (e);
		}
	}

	void onGetTuxture (params object[] paras)
	{
		try {
            AssetBundle tt = null;
			string tPath = "";
			if (paras != null && paras.Length > 1) {
				tPath = (string)(paras [0]); 
                tt = (paras [1]) as AssetBundle;
				object[] org = (object[])(paras [2]);
				string spritePth = (string)(org [0]);
				if (tt != null) {
                    assetBundleMap[spritePth] = tt;// tt.mainAsset as Texture;
					doNotifySprite (spritePth, tt.mainAsset as Texture);
				} else {
                    Debug.LogWarning ("can't find Texture in Resource path :[" + tPath + "]");
					doNotifySprite (spritePth, null);
				}
			} else {
				Debug.LogWarning ("can't find Texture in Resource path :[" + (paras.Length > 0 ? paras [0] : "") + "]");
			}
		} catch(System.Exception e) {
			Debug.LogError (e);
		}
	}

	#endregion //add by chenbin=========end

	/// <summary>
	/// Material used by the atlas.
	/// </summary>

	public Material spriteMaterial {
		get {
			return (mReplacement != null) ? mReplacement.spriteMaterial : material;
		}
		set {
			if (mReplacement != null) {
				mReplacement.spriteMaterial = value;
			} else {
				if (material == null) {
					mPMA = 0;
					material = value;
				} else {
					MarkAsChanged ();
					mPMA = -1;
					material = value;
					MarkAsChanged ();
				}
			}
		}
	}

	/// <summary>
	/// Whether the atlas is using a premultiplied alpha material.
	/// </summary>

	public bool premultipliedAlpha {
		get {
			if (mReplacement != null)
				return mReplacement.premultipliedAlpha;

			if (mPMA == -1) {
				Material mat = spriteMaterial;
				mPMA = (mat != null && mat.shader != null && mat.shader.name.Contains ("Premultiplied")) ? 1 : 0;
			}
			return (mPMA == 1);
		}
	}

	/// <summary>
	/// List of sprites within the atlas.
	/// </summary>

	public List<UISpriteData> spriteList {
		get {
			if (mReplacement != null)
				return mReplacement.spriteList;
			if (mSprites.Count == 0)
				Upgrade ();
			return mSprites;
		}
		set {
			if (mReplacement != null) {
				mReplacement.spriteList = value;
			} else {
				mSprites = value;
			}
		}
	}

	/// <summary>
	/// Texture used by the atlas.
	/// </summary>

	public Texture texture { get { return (mReplacement != null) ? mReplacement.texture : (material != null ? material.mainTexture as Texture : null); } }

	/// <summary>
	/// Pixel size is a multiplier applied to widgets dimensions when performing MakePixelPerfect() pixel correction.
	/// Most obvious use would be on retina screen displays. The resolution doubles, but with UIRoot staying the same
	/// for layout purposes, you can still get extra sharpness by switching to an HD atlas that has pixel size set to 0.5.
	/// </summary>

	public float pixelSize {
		get {
			return (mReplacement != null) ? mReplacement.pixelSize : mPixelSize;
		}
		set {
			if (mReplacement != null) {
				mReplacement.pixelSize = value;
			} else {
				float val = Mathf.Clamp (value, 0.25f, 4f);

				if (mPixelSize != val) {
					mPixelSize = val;
					MarkAsChanged ();
				}
			}
		}
	}

	/// <summary>
	/// Setting a replacement atlas value will cause everything using this atlas to use the replacement atlas instead.
	/// Suggested use: set up all your widgets to use a dummy atlas that points to the real atlas. Switching that atlas
	/// to another one (for example an HD atlas) is then a simple matter of setting this field on your dummy atlas.
	/// </summary>

	public UIAtlas replacement {
		get {
			return mReplacement;
		}
		set {
			UIAtlas rep = value;
			if (rep == this)
				rep = null;

			if (mReplacement != rep) {
				if (rep != null && rep.replacement == this)
					rep.replacement = null;
				if (mReplacement != null)
					MarkAsChanged ();
				mReplacement = rep;
				if (rep != null)
					material = null;
				MarkAsChanged ();
			}
		}
	}

	/// <summary>
	/// Convenience function that retrieves a sprite by name.
	/// </summary>

	public UISpriteData GetSprite (string name)
	{
		if (mReplacement != null) {
			return mReplacement.GetSprite (name);
		} else if (!string.IsNullOrEmpty (name)) {
			if (mSprites.Count == 0)
				Upgrade ();
			if (mSprites.Count == 0)
				return null;

			// O(1) lookup via a dictionary
#if UNITY_EDITOR
			if (Application.isPlaying)
#endif
			{
				// The number of indices differs from the sprite list? Rebuild the indices.
				if (mSpriteIndices.Count != mSprites.Count)
					MarkSpriteListAsChanged ();

				int index;
				if (mSpriteIndices.TryGetValue (name, out index)) {
					// If the sprite is present, return it as-is
					if (index > -1 && index < mSprites.Count)
						return mSprites [index];

					// The sprite index was out of range -- perhaps the sprite was removed? Rebuild the indices.
					MarkSpriteListAsChanged ();

					// Try to look up the index again
					return mSpriteIndices.TryGetValue (name, out index) ? mSprites [index] : null;
				}
			}

			// Sequential O(N) lookup.
			for (int i = 0, imax = mSprites.Count; i < imax; ++i) {
				UISpriteData s = mSprites [i];

				// string.Equals doesn't seem to work with Flash export
				if (!string.IsNullOrEmpty (s.name) && name == s.name) {
#if UNITY_EDITOR
					if (!Application.isPlaying)
						return s;
#endif
					// If this point was reached then the sprite is present in the non-indexed list,
					// so the sprite indices should be updated.
					MarkSpriteListAsChanged ();
					return s;
				}
			}
		}
		return null;
	}

	/// <summary>
	/// Convenience function that returns the name of a random sprite that begins with the specified value.
	/// </summary>

	public string GetRandomSprite (string startsWith)
	{
		if (GetSprite (startsWith) == null) {
			System.Collections.Generic.List<UISpriteData> sprites = spriteList;
			System.Collections.Generic.List<string> choices = new System.Collections.Generic.List<string> ();

			foreach (UISpriteData sd in sprites) {
				if (sd.name.StartsWith (startsWith))
					choices.Add (sd.name);
			}
			return (choices.Count > 0) ? choices [UnityEngine.Random.Range (0, choices.Count)] : null;
		}
		return startsWith;
	}

	/// <summary>
	/// Rebuild the sprite indices. Call this after modifying the spriteList at run time.
	/// </summary>

	public void MarkSpriteListAsChanged ()
	{
#if UNITY_EDITOR
		if (Application.isPlaying)
#endif
		{
			mSpriteIndices.Clear ();
			for (int i = 0, imax = mSprites.Count; i < imax; ++i)
				mSpriteIndices [mSprites [i].name] = i;
		}
	}

	/// <summary>
	/// Sort the list of sprites within the atlas, making them alphabetical.
	/// </summary>

	public void SortAlphabetically ()
	{
		mSprites.Sort (delegate(UISpriteData s1, UISpriteData s2) {
			return s1.name.CompareTo (s2.name);
		});
#if UNITY_EDITOR
		NGUITools.SetDirty (this);
#endif
	}

	/// <summary>
	/// Convenience function that retrieves a list of all sprite names.
	/// </summary>

	public BetterList<string> GetListOfSprites ()
	{
		if (mReplacement != null)
			return mReplacement.GetListOfSprites ();
		if (mSprites.Count == 0)
			Upgrade ();

		BetterList<string> list = new BetterList<string> ();
		
		for (int i = 0, imax = mSprites.Count; i < imax; ++i) {
			UISpriteData s = mSprites [i];
			if (s != null && !string.IsNullOrEmpty (s.name))
				list.Add (s.name);
		}
		return list;
	}

	/// <summary>
	/// Convenience function that retrieves a list of all sprite names that contain the specified phrase
	/// </summary>

	public BetterList<string> GetListOfSprites (string match)
	{
		if (mReplacement)
			return mReplacement.GetListOfSprites (match);
		if (string.IsNullOrEmpty (match))
			return GetListOfSprites ();

		if (mSprites.Count == 0)
			Upgrade ();
		BetterList<string> list = new BetterList<string> ();

		// First try to find an exact match
		for (int i = 0, imax = mSprites.Count; i < imax; ++i) {
			UISpriteData s = mSprites [i];
			
			if (s != null && !string.IsNullOrEmpty (s.name) && string.Equals (match, s.name, StringComparison.OrdinalIgnoreCase)) {
				list.Add (s.name);
				return list;
			}
		}

		// No exact match found? Split up the search into space-separated components.
		string[] keywords = match.Split (new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < keywords.Length; ++i)
			keywords [i] = keywords [i].ToLower ();

		// Try to find all sprites where all keywords are present
		for (int i = 0, imax = mSprites.Count; i < imax; ++i) {
			UISpriteData s = mSprites [i];
			
			if (s != null && !string.IsNullOrEmpty (s.name)) {
				string tl = s.name.ToLower ();
				int matches = 0;

				for (int b = 0; b < keywords.Length; ++b) {
					if (tl.Contains (keywords [b]))
						++matches;
				}
				if (matches == keywords.Length)
					list.Add (s.name);
			}
		}
		return list;
	}

	/// <summary>
	/// Helper function that determines whether the atlas uses the specified one, taking replacements into account.
	/// </summary>

	bool References (UIAtlas atlas)
	{
		if (atlas == null)
			return false;
		if (atlas == this)
			return true;
		return (mReplacement != null) ? mReplacement.References (atlas) : false;
	}

	/// <summary>
	/// Helper function that determines whether the two atlases are related.
	/// </summary>

	static public bool CheckIfRelated (UIAtlas a, UIAtlas b)
	{
		if (a == null || b == null)
			return false;
		return a == b || a.References (b) || b.References (a);
	}

	/// <summary>
	/// Mark all widgets associated with this atlas as having changed.
	/// </summary>

	public void MarkAsChanged ()
	{
#if UNITY_EDITOR
		NGUITools.SetDirty (gameObject);
#endif
		if (mReplacement != null)
			mReplacement.MarkAsChanged ();

		UISprite[] list = NGUITools.FindActive<UISprite> ();

		for (int i = 0, imax = list.Length; i < imax; ++i) {
			UISprite sp = list [i];

			if (CheckIfRelated (this, sp.atlas)) {
				UIAtlas atl = sp.atlas;
				sp.atlas = null;
				sp.atlas = atl;
#if UNITY_EDITOR
				NGUITools.SetDirty (sp);
#endif
			}
		}

		UIFont[] fonts = Resources.FindObjectsOfTypeAll (typeof(UIFont)) as UIFont[];

		for (int i = 0, imax = fonts.Length; i < imax; ++i) {
			UIFont font = fonts [i];

			if (CheckIfRelated (this, font.atlas)) {
				UIAtlas atl = font.atlas;
				font.atlas = null;
				font.atlas = atl;
#if UNITY_EDITOR
				NGUITools.SetDirty (font);
#endif
			}
		}

		UILabel[] labels = NGUITools.FindActive<UILabel> ();

		for (int i = 0, imax = labels.Length; i < imax; ++i) {
			UILabel lbl = labels [i];

			if (lbl.bitmapFont != null && CheckIfRelated (this, lbl.bitmapFont.atlas)) {
				UIFont font = lbl.bitmapFont;
				lbl.bitmapFont = null;
				lbl.bitmapFont = font;
#if UNITY_EDITOR
				NGUITools.SetDirty (lbl);
#endif
			}
		}
	}

	/// <summary>
	/// Performs an upgrade from the legacy way of specifying data to the new one.
	/// </summary>

	bool Upgrade ()
	{
		if (mReplacement)
			return mReplacement.Upgrade ();

		if (mSprites.Count == 0 && sprites.Count > 0 && material) {
			Texture tex = material.mainTexture;
			int width = (tex != null) ? tex.width : 512;
			int height = (tex != null) ? tex.height : 512;

			for (int i = 0; i < sprites.Count; ++i) {
				Sprite old = sprites [i];
				Rect outer = old.outer;
				Rect inner = old.inner;
				
				if (mCoordinates == Coordinates.TexCoords) {
					NGUIMath.ConvertToPixels (outer, width, height, true);
					NGUIMath.ConvertToPixels (inner, width, height, true);
				}

				UISpriteData sd = new UISpriteData ();
				sd.name = old.name;
				
				sd.x = Mathf.RoundToInt (outer.xMin);
				sd.y = Mathf.RoundToInt (outer.yMin);
				sd.width = Mathf.RoundToInt (outer.width);
				sd.height = Mathf.RoundToInt (outer.height);
				
				sd.paddingLeft = Mathf.RoundToInt (old.paddingLeft * outer.width);
				sd.paddingRight = Mathf.RoundToInt (old.paddingRight * outer.width);
				sd.paddingBottom = Mathf.RoundToInt (old.paddingBottom * outer.height);
				sd.paddingTop = Mathf.RoundToInt (old.paddingTop * outer.height);
				
				sd.borderLeft = Mathf.RoundToInt (inner.xMin - outer.xMin);
				sd.borderRight = Mathf.RoundToInt (outer.xMax - inner.xMax);
				sd.borderBottom = Mathf.RoundToInt (outer.yMax - inner.yMax);
				sd.borderTop = Mathf.RoundToInt (inner.yMin - outer.yMin);

				mSprites.Add (sd);
			}
			sprites.Clear ();
#if UNITY_EDITOR
			NGUITools.SetDirty (this);
			UnityEditor.AssetDatabase.SaveAssets ();
#endif
			return true;
		}
		return false;
	}
}
