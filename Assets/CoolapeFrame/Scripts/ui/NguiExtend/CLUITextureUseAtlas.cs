using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coolape;

namespace Coolape
{
	public class CLUITextureUseAtlas : UITexture
	{
		public bool isSharedMaterial = true;
		public string atlasName = "";
		UIAtlas _atlas;

		public UIAtlas mAtlas {
			get {
				if (_atlas == null) {
					if (!string.IsNullOrEmpty (atlasName)) {
						_atlas = CLUIInit.self.getAtlasByName (atlasName);
					} else {
						_atlas = CLUIInit.self.emptAtlas;
					}
				} else if (!string.IsNullOrEmpty (atlasName) && atlasName != _atlas.name) {
					_atlas = CLUIInit.self.getAtlasByName (atlasName);
				}
				return _atlas;
			}
			set {
				_atlas = value;
			}
		}

		string _spriteName = "";

		public Hashtable subTextureMap = new Hashtable ();
		public Hashtable subTextureNameMap = new Hashtable ();
		public Texture2D newTexture;

		UIPanel _panel;

		public UIPanel panel {
			get {
				if (_panel == null) {
					_panel = GetComponentInParent<UIPanel> ();
				}
				return _panel;
			}
		}

		public override float alpha {
			get {
				return base.alpha;
			}
			set {
				base.alpha = value;

				MarkAsChanged ();
			}
		}

		protected override void Awake ()
		{
			base.Awake ();
			if (!isSharedMaterial) {
				if (material != null) {
					#if UNITY_EDITOR
					if (Application.isPlaying) {
						material = GameObject.Instantiate<Material> (material);
					}
					#else
					material = GameObject.Instantiate<Material> (material);
					#endif
				}
			}
		}

		void OnEnable ()
		{
			try {
				if (!string.IsNullOrEmpty (spriteName)) {
					setMainTexture (spriteName);
					MarkAsChanged ();
				}

				Texture tt = null;
				string _spName = "";
				ArrayList list = MapEx.keys2List (subTextureNameMap);
				for (int i = 0; i < list.Count; i++) {
					_spName = subTextureNameMap [list [i]] as string;
					setTexture (list [i].ToString (), _spName);
				}
				list.Clear ();
				list = null;
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		void OnDisable ()
		{
			try {
				if (!string.IsNullOrEmpty (spriteName)) {
					mAtlas.returnSpriteByname (_spriteName);
					if (newTexture != null) {
						material.mainTexture = null;
						Texture2D.DestroyImmediate (newTexture);
						newTexture = null;
					}
					MarkAsChanged ();
				}
				Texture tt = null;
				string _spName = "";
				foreach (DictionaryEntry cell in subTextureMap) {
					tt = cell.Value as Texture;
					if (tt != null) {
						_spName = subTextureNameMap [cell.Key] as string;
						if (mAtlas != null) {
							mAtlas.returnSpriteByname (_spName);
						}
					}
				}
				subTextureMap.Clear ();
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public string spriteName {
			get {
				return _spriteName;
			}
			set {
				try {
					if (!string.IsNullOrEmpty (_spriteName) && _spriteName != value) {
						if (gameObject.activeInHierarchy) {
							mAtlas.returnSpriteByname (_spriteName);
							if (newTexture != null) {
								material.mainTexture = null;
								Texture2D.DestroyImmediate (newTexture);
								newTexture = null;
							}
						}
					}
					if (_spriteName != value) {
						_spriteName = value;
						if (!string.IsNullOrEmpty (_spriteName)) {
							setMainTexture (_spriteName);
						}
					}
				} catch (System.Exception e) {
					Debug.LogError (e);
				}
			}
		}

		void setMainTexture (string spriteName)
		{
			if (!gameObject.activeInHierarchy)
				return;
			UISpriteData sd = mAtlas.getSpriteBorrowMode (spriteName);
			if (sd != null && UIAtlas.assetBundleMap [sd.path] != null) {
				mAtlas.borrowSpriteByname (spriteName, null, (Callback)onGetMainTexture2, null);
			} else {
				mAtlas.borrowSpriteByname (spriteName, null, (Callback)onGetMainTexture, null);
			}
		}

		void onGetMainTexture (params object[] paras)
		{
			string _spName = paras [1] as string;
			UISpriteData sd = mAtlas.getSpriteBorrowMode (_spName);
			if (sd != null && UIAtlas.assetBundleMap [sd.path] != null) {
				setMainTexture (_spName);
			} else {
				Debug.LogError (_spName + ":get spriteData is null!");
			}
		}

		void onGetMainTexture2 (params object[] paras)
		{
			string _spName = paras [1] as string;
			if (_spName.Equals (spriteName)) {
				UISpriteData sd = mAtlas.getSpriteBorrowMode (spriteName);
				if (sd != null) {
					Texture2D tt = UIAtlas.assetBundleMap [sd.path] as Texture2D;
					if (sd.x == 0 && sd.y == 0 && tt.height == sd.height && tt.width == sd.width) {
						material.mainTexture = tt;
						mainTexture = tt;
					} else {
						//Debug.LogError ("@@@@@@@@@@@@@@@@@@@@");
						Color[] colors = tt.GetPixels (sd.x, tt.height - sd.y - sd.height, sd.width, sd.height);
						newTexture = new Texture2D (sd.width, sd.height);
						newTexture.SetPixels (colors);
						newTexture.Apply ();
						material.mainTexture = newTexture;
						mainTexture = newTexture;
					}
					MarkAsChanged ();
					panel.Refresh ();
				}
			} else {
				mAtlas.returnSpriteByname (_spName);
				if (newTexture != null) {
					material.mainTexture = null;
					Texture2D.DestroyImmediate (newTexture);
					newTexture = null;
				}
			}
		}

		public void setTexture (string propName, string spriteName)
		{
			try {
				if (string.IsNullOrEmpty (spriteName)) {
					Debug.Log (spriteName + "<<setTexture null spriteName>>" + propName);
					return;
				}
			
				Texture tt = subTextureMap [propName] as Texture;
				if (tt == null) {
					subTextureNameMap [propName] = spriteName;
					if (!gameObject.activeInHierarchy) {
						return;
					}
					UISpriteData sd = mAtlas.getSpriteBorrowMode (spriteName);
					if (sd != null && UIAtlas.assetBundleMap [sd.path] != null) {
						mAtlas.borrowSpriteByname (spriteName, null, (Callback)onGetSubTexture2, propName);
					} else {
						mAtlas.borrowSpriteByname (spriteName, null, (Callback)onGetSubTexture, propName);
					}
				} else {
					string _spName = subTextureNameMap [propName] as string;
					if (_spName == spriteName) {
						material.SetTexture (propName, tt);

						MarkAsChanged ();
						panel.Refresh ();
					} else {
						mAtlas.returnSpriteByname (_spName);
						subTextureMap.Remove (propName);
						subTextureNameMap.Remove (propName);
						subTextureNameMap [propName] = spriteName;
						if (!gameObject.activeInHierarchy) {
							return;
						}
						UISpriteData sd = mAtlas.getSpriteBorrowMode (spriteName);
						if (sd != null && UIAtlas.assetBundleMap [sd.path] != null) {
							mAtlas.borrowSpriteByname (spriteName, null, (Callback)onGetSubTexture2, propName);
						} else {
							mAtlas.borrowSpriteByname (spriteName, null, (Callback)onGetSubTexture, propName);
						}
					}
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		void onGetSubTexture (params object[] paras)
		{
			string _spName = paras [1] as string;
			string propName = paras [2] as string;

			UISpriteData sd = mAtlas.getSpriteBorrowMode (_spName);
			if (sd != null && UIAtlas.assetBundleMap [sd.path] != null) {
				setTexture (propName, _spName);
			} else {
				Debug.LogError (_spName + ":get spriteData is null!");
			}
		}

		void onGetSubTexture2 (params object[] paras)
		{
			string _spName = paras [1] as string;
			string propName = paras [2] as string;
			if (_spName.Equals (subTextureNameMap [propName] as string)) {
				UISpriteData sd = mAtlas.getSpriteBorrowMode (_spName);
				if (sd != null) {
					Texture tt = UIAtlas.assetBundleMap [sd.path] as Texture;
					subTextureMap [propName] = tt;
					material.SetTexture (propName, tt);
					MarkAsChanged ();
					panel.Refresh ();
				}
			} else {
				mAtlas.returnSpriteByname (_spName);
			}
		}
	}
}
