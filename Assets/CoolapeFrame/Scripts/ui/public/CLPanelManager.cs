/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   控制NGUI每个页面的
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Coolape
{
	public class CLPanelManager :MonoBehaviour
	{
		public static CLPanelManager self;
		bool isUnity3dType = true;
		public Transform _uiPanelRoot;

		public Transform uiPanelRoot {
			get {
				if (_uiPanelRoot == null) {
					_uiPanelRoot = transform;
				}
				return _uiPanelRoot;
			}
		}

		public CLPanelManager ()
		{
			self = this;
		}

		[NonSerialized]
		public int depth = 100;
		public const int Const_RenderQueue = 3000;
		public int cachePanelSize = 10;
		//当前
		public static Hashtable showingPanels = new Hashtable ();
		public static ArrayList panelCacheQueue = ArrayList.Synchronized(new ArrayList());
		public static void onShowPanel(CLPanelBase panel)
		{
			if (panel == null)
				return;
			showingPanels [panel.gameObject.name] = panel;
			panelCacheQueue.Remove (panel);
			panelCacheQueue.Insert (0, panel);
			CLPanelBase bottomPanel = null;
			if (panelCacheQueue.Count > self.cachePanelSize) {
				bottomPanel = (panelCacheQueue [panelCacheQueue.Count - 1]) as CLPanelBase;
				panelCacheQueue.RemoveAt (panelCacheQueue.Count - 1);
				if (bottomPanel.destroyWhenHide) {
					if (!bottomPanel.isActive && !CLPanelManager.panelRetainLayer.Contains (bottomPanel)) {
						//虽然页面是关掉了，但是如果还是在panelRetainLayer里，则不能删除，因为当关掉顶层页面时，这个页面还是会被打开
						CLPanelManager.destroyPanel (bottomPanel, false);
					}
				}
			}
		}

		public static void onHidePanel(CLPanelBase panel) {
			if (panel == null)
				return;
			showingPanels.Remove (panel.gameObject.name);
		}
	
		//显示窗体********************************************************************
		public static Queue<CLPanelBase> seaShowPanel = new Queue<CLPanelBase> ();
		public static bool isShowPanel = false;

		public static void showPanel (CLPanelBase panel)
		{
			//if(panel == null) return;
			//Debug.LogError("====show panel==="+panel.name);
			seaShowPanel.Enqueue (panel);
			isShowPanel = true;
		}
	
		//关闭窗体********************************************************************
		public static bool isHidePanel = false;
		public static Queue<CLPanelBase> seaHidePanel = new Queue<CLPanelBase> ();

		public static void hidePanel (CLPanelBase panel)
		{
			//if(panel == null) return;
			//Debug.LogError("====hide panel==="+panel.name);
			seaHidePanel.Enqueue (panel);
			isHidePanel = true;
			self.Update ();
		}
	
		//显示窗体到最顶层********************************************************************
		public static Stack<CLPanelBase> panelRetainLayer = new Stack<CLPanelBase> ();

		public static CLPanelBase[] panels4Retain {
			get {
				return panelRetainLayer.ToArray ();
			}
		}

		public static bool isShowTopPanel = false;
		public static CLPanelBase topPanel = null;
		public static CLPanelBase oldPanel = null;
		public static CLPanelBase oldoldPanel = null;
		public static bool isShowPrePanel = false;
		public static int depthOffset = 200;

		/// <summary>
		/// Shows the top panel.把panel显示在顶层,如果showTopPanel用打开窗体，就必须用hideTopPanel关闭窗体
		/// </summary>
		/// <param name='panel'>
		/// Panel.
		/// </param>
		/// <param name='isRetainCurr'>
		/// Is retain curr.是否保留当前层
		/// </param>
		/// <param name='isShowCurr'>
		/// Is show curr.是否显示当前层
		/// </param>
		public static void showTopPanel (CLPanelBase panel, bool isRetainCurr, bool isShowCurr)
		{
//		self.StartCoroutine(doShowTopPanel(panel, isRetainCurr, isShowCurr, true));
			doShowTopPanel (panel, isRetainCurr, isShowCurr, true);
		}

		public static void doShowTopPanel (CLPanelBase panel, bool isRetainCurr, bool isShowCurr, bool immed)
		{
//        yield return null;
			isShowPrePanel = isShowCurr;
			oldoldPanel = oldPanel;
			if (panelRetainLayer.Count > 0) {
				if (!isRetainCurr) {
					self.depth -= depthOffset;
					self.depth = self.depth < depthOffset ? depthOffset : self.depth;
					oldPanel = panelRetainLayer.Pop ();
				} else {
					oldPanel = panelRetainLayer.Peek ();
				}
			} else {
				oldPanel = null;
			}
			panelRetainLayer.Push (panel);
//		if (isShowCurr) {
//			self.depth += 10;
//		}
		
			topPanel = panel;
			isShowTopPanel = true;
			if (immed) {
				self.Update ();
			}
		}

		public static void showTopPanel (CLPanelBase panel)
		{
			showTopPanel (panel, false, false);
		}

		public string mainPanelName = "";
		//关闭顶层窗体********************************************************************
		public static void hideTopPanel ()
		{
			hideTopPanel (null, true, true);
		}

		public static void hideTopPanel (CLPanelBase panel)
		{
			hideTopPanel (panel, true, true);
		}

		public static void hideTopPanel (CLPanelBase panel, bool showMain, bool immed)
		{
			if (panel == null || (panelRetainLayer.Count > 0 && panelRetainLayer.Peek() == panel)) {
				if (showMain && !string.IsNullOrEmpty (self.mainPanelName)
				    && topPanel != null && string.Compare (self.mainPanelName, topPanel.name) == 0) {
					topPanel.refresh ();
					return;
				}
				if (panelRetainLayer.Count > 0) {
					self.depth -= depthOffset;
					self.depth = self.depth < depthOffset ? depthOffset : self.depth;
					seaHidePanel.Enqueue (panelRetainLayer.Pop ());
					isHidePanel = true;
				}
				if (panelRetainLayer.Count > 0) {
					isShowTopPanel = true;
				} else {
					if (CLPBackplate.self != null) {
						CLPBackplate.self.proc (null);
					}
				}
				oldPanel = oldoldPanel;
				oldoldPanel = null;
			} else {
				rmPanelRetainLayer (panel);
				seaHidePanel.Enqueue (panel);
				isHidePanel = true;
			}
		
			if (immed) {
				self.Update ();
			}
		}

		public static void rmPanelRetainLayer(CLPanelBase panel) {
			Stack<CLPanelBase> tmpStack = new Stack<CLPanelBase> ();	
			CLPanelBase p = null;
			while(panelRetainLayer.Count > 0 ) {
				p = panelRetainLayer.Pop ();
				if (p != panel) {
					tmpStack.Push (p);
				}
			}
			while(tmpStack.Count > 0) {
				panelRetainLayer.Push (tmpStack.Pop());
			}
		}

		//关闭所有层**********************************************************************
		public static void hideAllPanel ()
		{
			oldPanel = null;
			oldoldPanel = null;
			int count = panelRetainLayer.Count;
			for (int i = 0; i < count; i++) {
//				hideTopPanel (null, false, true);
				self.depth -= depthOffset;
				self.depth = self.depth < depthOffset ? depthOffset : self.depth;
				seaHidePanel.Enqueue (panelRetainLayer.Pop ());
				isHidePanel = true;
			}
			oldPanel = null;
			oldoldPanel = null;
			self.mask.SetActive (false);
			self.Update ();
		}

		public void Update ()
		{
			if (isShowPanel) {
				isShowPanel = false;
				CLPanelBase p = seaShowPanel.Dequeue ();
				if (p != null) {
					p.show ();
					p.panel.renderQueue = UIPanel.RenderQueue.StartAt;
					// 设置startingRenderQueue是为了可以在ui中使用粒子效果，注意在粒子中要绑定CLUIParticle角本
					p.panel.startingRenderQueue = Const_RenderQueue + p.panel.depth;
				}
				if (seaShowPanel.Count > 0) {
					isShowPanel = true;
				}
			}

			if (isHidePanel) {
				isHidePanel = false;
				CLPanelBase p = seaHidePanel.Dequeue ();
				if (p != null) {
					p.hide ();
				}
				if (seaHidePanel.Count > 0) {
					isHidePanel = true;
				} else {
					CLPBackplateProc (null);
				}
			}
		
			if (isShowTopPanel) {
				isShowTopPanel = false;

				if (oldPanel != null) {
					if (!isShowPrePanel) {
						oldPanel.hide ();
						if (oldoldPanel != null) {
//							Vector3 newPos = oldoldPanel.transform.localPosition;
//							oldoldPanel.transform.localPosition = newPos;
							oldPanel = oldoldPanel;
							oldoldPanel = null;
						}
					} else {
//						Vector3 newPos = oldPanel.transform.localPosition;
//						oldPanel.transform.localPosition = newPos;
//						if (oldoldPanel != null) {
//							newPos = oldoldPanel.transform.localPosition;
//							oldoldPanel.transform.localPosition = newPos;
//						}
					}
				}
				//置顶的处理放在后面，防止oldoldPannel = curPannel的情况
				if (panelRetainLayer.Count > 0) {
					//GlobalMemoryVar.curStatus.changeTo (GlobalMemoryVar.sNgui);
					float panelZ = 0;
					if (topPanel != null) {
						panelZ = topPanel.transform.localPosition.z;
					}
					topPanel = panelRetainLayer.Peek ();
					if (topPanel != null) {
						if (!topPanel.isActive) {
							depth += depthOffset;
							topPanel.depth = depth;
							topPanel.show ();
						} else {
							topPanel.depth = depth;
							topPanel.refresh ();
						}
						topPanel.panel.renderQueue = UIPanel.RenderQueue.StartAt;
						// 设置startingRenderQueue是为了可以在ui中使用粒子效果，注意在粒子中要绑定CLUIParticle角本
						topPanel.panel.startingRenderQueue = Const_RenderQueue + depth;

						Vector3 newPos = topPanel.transform.localPosition;
						newPos.z = -topPanel.depth;
						topPanel.transform.localPosition = newPos;
					}

					CLPBackplateProc (topPanel);
					onTopPanelChange (topPanel);
				} else {
					CLPBackplateProc (null);
				}
			}
		}

		void onTopPanelChange (CLPanelBase p)
		{
//			CLPanelBase[] ps = panelRetainLayer.ToArray ();
//			if (ps != null) {
//				for (int i = 0; i < ps.Length; i++) {
//					ps [i].onTopPanelChange (p);
//				}
//			}
			ArrayList list = MapEx.vals2List (showingPanels);
			if (list != null) {
				for (int i = 0; i < list.Count; i++) {
					((CLPanelBase)(list [i])).onTopPanelChange (p);
				}
				list.Clear ();
				list = null;
			}
		}

		void CLPBackplateProc (CLPanelBase p)
		{
			CLPanelBase panel = getPanel ("PanelBackplate");
			if (panel == null)
				return;
			CLPBackplate.self.proc (p);
		}
	
		//=======================================
		public static Hashtable panelBuff = new Hashtable ();
		public static Hashtable panelAssetBundle = new Hashtable ();
		public static bool isFinishStart = false;

		public void Start ()
		{
			if (!isFinishStart) {
				for (int i = 0; i < uiPanelRoot.childCount; i++) {
					CLPanelBase p = uiPanelRoot.GetChild (i).GetComponent<CLPanelBase> ();
					if (p != null) {
						panelBuff [p.name] = p;
					}
				}
				isFinishStart = true;
			}
		}

		GameObject _mask;

		public GameObject mask {
			get {
				if (_mask == null) {
					_mask = new GameObject ("_____mask");
					_mask.transform.parent = CLUIInit.self.transform;
					NGUITools.SetLayer (_mask, LayerMask.NameToLayer ("UI"));
					UIWidget w = _mask.AddComponent<UIWidget> ();
					w.SetAnchor (CLUIInit.self.gameObject, -2, -2, 2, 2);
					NGUITools.AddWidgetCollider (_mask);
					_mask.SetActive (false);
				}
				return _mask;
			}
		}

        public void clean ()
		{
			panelBuff.Clear ();
			panelAssetBundle.Clear ();
			panelCacheQueue.Clear ();
			showingPanels.Clear ();
			topPanel = null;
			isFinishStart = false;
			mask.SetActive (false);
		}

		public void reset ()
		{
			isFinishStart = false;
			Start ();
		}

		public static void resetPanelLua ()
		{
			CLPanelLua p = null;
			foreach (DictionaryEntry cell in CLPanelManager.panelBuff) {
				p = (CLPanelLua)(cell.Value);
				p.reLoadLua ();
				p.setLua ();
			}
		}

		public static void destoryAllPanel ()
		{
			ArrayList list = new ArrayList ();
			list.AddRange (panelAssetBundle.Values);
			for (int i = 0; i < list.Count; i++) {
				destroyPanel ((CLPanelBase)(list [i]));
			}
			panelRetainLayer.Clear ();
			topPanel = null;
		}

		public static void getPanelAsy (string pName, object callback)
		{
			getPanelAsy (pName, callback, null);
		}

		public static void getPanelAsy (string pName, object callback, object paras)
		{
			NGUITools.SetActive (self.mask, true);
			if (!isFinishStart) {
				CLPanelManager.self.Start ();
			}
			CLPanelBase p = null;
			if (panelBuff [pName] != null) {
				p = ((CLPanelBase)(panelBuff [pName]));
			}
			if (p == null) {
#if UNITY_5_6_OR_NEWER
				Transform tr = self.transform.Find (pName);
#else
				Transform tr = self.transform.FindChild (pName);
#endif
				if (tr != null) {
					p = tr.GetComponent<CLPanelBase> ();
					if (p != null) {
						panelBuff [pName] = p;
					}
				}
			}
		
			if (p != null) {
				Utl.doCallback (callback, p, paras);
				NGUITools.SetActive (self.mask, false);
			} else {
				self.StartCoroutine (loadPanel (pName, callback, paras));
			}
		}

		static IEnumerator loadPanel (string pName, object callback, object paras)
		{
			if (CLCfgBase.self.isEditMode) {
				string path = PStr.begin ().a ("file://").a (CLPathCfg.persistentDataPath).a ("/")
					.a (CLPathCfg.self.panelDataPath).a (CLPathCfg.self.platform).a ("/").a (pName).a (".unity3d").end ();
				if (CLCfgBase.self.isEditMode) {
					path = path.Replace ("/upgradeRes/", "/upgradeRes4Publish/");
				}
				WWW www = new WWW (path);
				yield return www;
				if (string.IsNullOrEmpty (www.error)) {
					finishGetPanel (pName, www.assetBundle, callback, paras);
					www.Dispose ();
					www = null;
				}
			} else {
				string path = PStr.begin ().a (CLPathCfg.self.panelDataPath).a (CLPathCfg.self.platform).a ("/").a (pName).a (".unity3d").end ();
				CLVerManager.self.getNewestRes (path, CLAssetType.assetBundle, (Callback)onGetPanelAssetBundle, true, callback, pName, paras);
			}
		}

		public static void onGetPanelAssetBundle (params object[]args)
		{
			string Path = args [0].ToString ();
			object content = args [1];
			object[] orgs = (object[])(args [2]);
			object callback = null;
			string pName = "";
			object paras = null;
			if (orgs != null && orgs.Length > 2) {
				callback = orgs [0];
				pName = orgs [1].ToString ();
				paras = orgs [2];
			}

			finishGetPanel (pName, (AssetBundle)(content), callback, paras);
		}

		public static void finishGetPanel (string pName, AssetBundle ab, object callback, object paras)
		{
			if (ab != null) {
				GameObject prefab = ab.mainAsset as GameObject;
				ab.Unload (false);
				ab = null;
				GameObject go = GameObject.Instantiate (prefab) as GameObject;
				go.name = pName;
				go.transform.parent = self.transform;
				go.transform.localScale = Vector3.one;
				go.transform.localPosition = Vector3.zero;
			
				CLPanelBase p = go.GetComponent<CLPanelBase> ();
				if (p.isNeedResetAtlase) {
					CLUIUtl.resetAtlasAndFont (p.transform, false);
				}
				panelBuff [pName] = p;
				panelAssetBundle [pName] = p;

				CLSharedAssets sharedAsset = go.GetComponent<CLSharedAssets> ();
				if (sharedAsset != null) {
					NewList param = ObjPool.listPool.borrowObject ();
					param.Add (callback);
					param.Add (p);
					param.Add (paras);
					sharedAsset.init ((Callback)onGetSharedAssets, param, null);
				} else {
					if (p != null) {
						Utl.doCallback (callback, p, paras);
					}
				}

			}
			NGUITools.SetActive (self.mask, false);
			return;// null;
		}

		static void onGetSharedAssets (params object[] param)
		{
			if (param == null) {
				Debug.LogWarning ("param == null");
				return;
			}
			NewList list = (NewList)(param [0]);
			if (list.Count >= 3) {
				object cb = list [0];
				CLPanelBase p = list [1] as CLPanelBase;
				object orgs = list [2];
				if (cb != null) {
					Utl.doCallback (cb, p, orgs);
				}
			} else {
				Debug.LogWarning ("list.Count ====0");
			}
			ObjPool.listPool.returnObject (list);
		}

		public static CLPanelBase  getPanel (string pName)
		{
			if (!isFinishStart) {
				CLPanelManager.self.Start ();
			}
			if (panelBuff [pName] != null) {
				return ((CLPanelBase)(panelBuff [pName]));
			}
#if UNITY_5_6_OR_NEWER
			Transform tr = self.transform.Find (pName);
#else
			Transform tr = self.transform.FindChild (pName);
#endif
			if (tr != null) {
				CLPanelBase p = tr.GetComponent<CLPanelBase> ();
				if (p != null) {
					panelBuff [pName] = p;
					return p;
				}
			}
//		if (self.isUnity3dType) {
//#if !UNITY_ANDROID
//			string path = PStr.begin().a(PathCfg.persistentDataPath).a("/")
//				.a(PathCfg.self.panelDataPath).a(PathCfg.self.platform).a("/").a(pName).a(".unity3d").end();
//#if UNITY_EDITOR
//			path = path.Replace("/upgradeRes/", "/upgradeRes4Publish/");
//#endif
//			AssetBundle ab = AssetBundle.LoadFromFile(path);
//#if UNITY_EDITOR
//			if(ab == null) {
//				Debug.LogError(pName + " is null");
//			}
//#endif
//			return onGetPanel(pName, ab, null, null);
//#endif
//		}
			return null;
		}

		public static void destroyPanel (CLPanelBase p)
		{
			destroyPanel (p, true);
		}

		public static void destroyPanel (CLPanelBase p, bool needCallHideFunc)
		{
			if (p == null || p.name == CLMainBase.self.firstPanel)
				return;
			string pName = p.name;
			panelBuff.Remove (pName);
			panelAssetBundle.Remove (pName);
			if (needCallHideFunc) {
				p.hide ();
			}
			GameObject.DestroyImmediate (p.gameObject, true);
			p = null;
		}
	}
}
