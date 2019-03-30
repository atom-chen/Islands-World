/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   ui字库、图集初始化
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Coolape
{
	public class CLUIInit : MonoBehaviour
	{
		//正实图集名
		public string atlasAllRealName = "atlasAllReal";
		//空字库
		public UIFont emptFont;
		//字图集
		public UIAtlas emptAtlas;
		public Transform uiPublicRoot;
		public static CLUIInit self;
		public static Dictionary<string, UIFont> fontMap = new Dictionary<string, UIFont> ();
		public static Dictionary<string, UIAtlas> atlasMap = new Dictionary<string, UIAtlas> ();

		public int PanelConfirmDepth {
			get {
				if (CLPanelManager.self.depth > 10000) {
					return CLPanelManager.self.depth + 1000;
				} else {
					return 10000;
				}
			}
		}

		public int PanelHotWheelDepth {
			get {
				if (CLPanelManager.self.depth > 11000) {
					return CLPanelManager.self.depth + 2000;
				} else {
					return 11000;
				}
			}
		}

		public int PanelWWWProgressDepth {
			get {
				if (CLPanelManager.self.depth > 12000) {
					return CLPanelManager.self.depth + 3000;
				} else {
					return 12000;
				}
			}
		}

		public int AlertRootDepth {
			get {
				if (CLPanelManager.self.depth > 13000) {
					return CLPanelManager.self.depth + 4000;
				} else {
					return 13000;
				}
			}
		}

		public CLUIInit ()
		{
			self = this;
		}

		public void clean ()
		{
			try {
                if(emptFont != null) { 
                    foreach (var item in fontMap) {
    					if (item.Key != emptFont.name) {
    						#if UNITY_EDITOR
    						if (Application.isPlaying) {
    							DestroyObject (item.Value);
    						}
    						#else
    						DestroyObject (item.Value);
    						#endif
    					}
                    }
				}
				fontMap.Clear ();
                if(emptAtlas != null) { 
                    foreach (var item in atlasMap) {
    					if (item.Key != emptAtlas.name) {
    						#if UNITY_EDITOR
    						if (Application.isPlaying) {
    							DestroyObject (item.Value);
    						}
    						#else
    						DestroyObject (item.Value);
    						#endif
    					}
    				}
                }
				atlasMap.Clear ();
				emptAtlas.replacement = null;
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public bool init ()
		{
			clean ();
#if !UNITY_EDITOR
			//取得最新的语言
			Callback cb = onGetLocalize;
			StartCoroutine (FileEx.readNewAllBytesAsyn (
				PStr.b (CLPathCfg.self.localizationPath).a(Localization.language).a(".txt").e (), 
				cb));
#endif
		
			return initAtlas ();
		}
		//设置语言
		void onGetLocalize (params object[] para)
		{
			if (para != null && para.Length > 0) {
				byte[] buff = (byte[])(para [0]);
				Localization.Load (Localization.language, buff);
			}
		}

		/// <summary>
		/// Inits the atlas.初始化ui所用的atlas
		/// </summary>
		/// <returns>
		/// The atlas.
		/// </returns>
		public  bool initAtlas ()
		{
            if (emptAtlas != null)
            {
                atlasMap[emptAtlas.name] = emptAtlas;
            }

            if (emptFont != null)
            {
                fontMap[emptFont.name] = emptFont;
            }

			UIAtlas atlas = getAtlasByName (atlasAllRealName);
			if (atlas != null) {
				emptAtlas.replacement = atlas;
				return true;
			}
			return false;
		}

		public UIFont getFontByName (string fontName)
		{
			if (fontMap.ContainsKey (fontName)) {
				return fontMap [fontName];
			}
			
#if UNITY_EDITOR
			string tmpPath = "";
			if (CLCfgBase.self.isEditMode && !Application.isPlaying) {
				tmpPath = PStr.begin ()
					.a ("Assets/").a (CLPathCfg.self.basePath).a ("/").a ("upgradeRes4Dev").
					a ("/priority/font/").a (fontName).a (".prefab").end ();
				UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath (tmpPath, typeof(UnityEngine.Object));
				if (obj != null) {
					return ((GameObject)obj).GetComponent<UIFont> ();
				}
				return null;
			} else {
				return _getFontByName (fontName);
			}
#else
		return _getFontByName (fontName);
#endif
		}

		UIFont _getFontByName (string fontName)
		{
			try {
				string tmpPath = PStr.begin ().a (CLPathCfg.self.basePath).a ("/").a ("upgradeRes").
					a ("/priority/font/").a (CLPathCfg.self.platform).a ("/").a (fontName).a (".unity3d").end ();
				#if UNITY_EDITOR
				if (CLCfgBase.self.isEditMode) {
					tmpPath = tmpPath.Replace ("/upgradeRes/", "/upgradeRes4Publish/");
				}
				#endif
				AssetBundle atlasBundel = AssetBundle.LoadFromMemory (FileEx.readNewAllBytes (tmpPath));
				if (atlasBundel != null) {
					GameObject go = atlasBundel.LoadAsset<GameObject>(atlasBundel.name);
					atlasBundel.Unload (false);
					atlasBundel = null;
					if (go != null) {
						UIFont font = go.GetComponent<UIFont> ();
						fontMap [fontName] = font;
						if(!string.IsNullOrEmpty(font.atlasName)) {
							font.atlas = getAtlasByName(font.atlasName);
						}
						return font;
					}
				}
				return null;
			} catch (System.Exception e) {
				Debug.LogError (e);
				return null;
			}
		}

		public UIAtlas getAtlasByName (string atlasName)
		{
            try { 
            if (string.IsNullOrEmpty(atlasName)) {
                return null;
            }
			if (atlasMap.ContainsKey (atlasName)) {
				return atlasMap [atlasName];
			}

			#if UNITY_EDITOR
			string tmpPath = "";
			if (CLCfgBase.self.isEditMode && !Application.isPlaying) {
				tmpPath = PStr.begin ()
					.a ("Assets/").a (CLPathCfg.self.basePath).a ("/").a ("upgradeRes4Dev").
					a ("/priority/atlas/").a (atlasName).a (".prefab").end ();
				UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath (tmpPath, typeof(UnityEngine.Object));
				if (obj != null) {
					return ((GameObject)obj).GetComponent<UIAtlas> ();
				}
				return null;
			} else {
				return _getAtlasByName (atlasName);
			}
			#else
		return _getAtlasByName (atlasName);
			#endif
            }catch(System.Exception e)
            {
                Debug.LogError(atlasName +"==" +e);
                return null;
            }
        }

		UIAtlas _getAtlasByName (string atlasName)
		{
			try {
				string tmpPath = PStr.begin ().a (CLPathCfg.self.basePath).a ("/").a ("upgradeRes").
					a ("/priority/atlas/").a (CLPathCfg.self.platform).a ("/").a (atlasName).a (".unity3d").end ();
				#if UNITY_EDITOR
				if (CLCfgBase.self.isEditMode) {
					tmpPath = tmpPath.Replace ("/upgradeRes/", "/upgradeRes4Publish/");
				}
				#endif
				AssetBundle atlasBundel = AssetBundle.LoadFromMemory (FileEx.readNewAllBytes (tmpPath));
				if (atlasBundel != null) {
					GameObject go = atlasBundel.mainAsset as GameObject;
					atlasBundel.Unload (false);
					atlasBundel = null;
					if (go != null) {
						UIAtlas atlas = go.GetComponent<UIAtlas> ();
						atlasMap [atlasName] = atlas;
						return atlas;
					}
				}
				return null;
			} catch (System.Exception e) {
				Debug.LogError (e + "===" + atlasName);
				return null;
			}
		}
	}
}
