/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   页面基类
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Coolape
{
	public abstract class CLPanelBase : CLBaseLua
	{
		//	[HideInInspector]
		bool _isActive = false;

		public bool isActive {
			get {
				return _isActive;
			}
			set {
				_isActive = value;
			}
		}

		UIPanel _panel = null;

		public UIPanel panel {
			get {
				if (_panel == null) {
					_panel = gameObject.GetComponent<UIPanel> ();
					if (_panel == null) {
						_panel = gameObject.AddComponent<UIPanel> ();
					}
				}
				return _panel;
			}
		}

		public bool isNeedBackplate = true;
		//是否需要遮罩
		public bool destroyWhenHide = false;
		public bool isNeedResetAtlase = true;
		[HideInInspector]
		public bool isNeedMask4Init = false;
		[HideInInspector]
		public bool isNeedMask4InitOnlyOnce = true;
		public bool isHideWithEffect = true;
		public bool isRefeshContentWhenEffectFinish = false;
		public Transform EffectRoot;

		static int _destoryDelaySec = 5;

		public static int destoryDelaySec {
			get {
				return _destoryDelaySec;
			}
			set {
				_destoryDelaySec = value;
			}
		}
		//特效root节点
		public enum EffectType
		{
			synchronized,
			//同步执行
			ordered,
			//顺序执行
		}

		public EffectType effectType = EffectType.ordered;
		//特效类型
		public List<UITweener> EffectList = new List<UITweener> ();

		static public int SortByName (UITweener a, UITweener b)
		{
			return string.Compare (a.name, b.name);
		}

		bool isFinishStart = false;

		public virtual void Start ()
		{
			if (isFinishStart)
				return;

			getSubPanelsDepth ();
			UITweener[] tws = null;
			if (EffectRoot != null) {
				tws = EffectRoot.GetComponents<UITweener> ();
				sortTweeners (tws);
				for (int j = 0; j < tws.Length; j++) {
					if (tws [j] != null) {
						tws [j].ResetToBeginning ();
						tws [j].enabled = false;
						EffectList.Add (tws [j]);
					}
				}
				for (int i = 0; i < EffectRoot.childCount; i++) {
					tws = EffectRoot.GetChild (i).GetComponents<UITweener> ();
					sortTweeners (tws);
					for (int j = 0; j < tws.Length; j++) {
						if (tws [j] != null) {
							tws [j].ResetToBeginning ();
							tws [j].enabled = false;
							EffectList.Add (tws [j]);
						}
					}
				}
				EffectList.Sort (SortByName);
			}
		
			for (int i = EffectList.Count - 1; i >= 0; i--) {
				if (EffectList [i] == null) {
					EffectList.RemoveAt (i);
				} else {
					EffectList [i].ResetToBeginning ();
				}
			}
			isFinishStart = true;
		}

		/// <summary>
		/// 冒泡排序法1
		/// </summary>
		/// <param name="list"></param>
		public void sortTweeners (UITweener[] list)
		{
			UITweener temp = null;
			for (int i = 0; i < list.Length; i++) {
				for (int j = i; j < list.Length; j++) {
					if (list [i].exeOrder < list [j].exeOrder) {
						temp = list [i];
						list [i] = list [j];
						list [j] = temp;
					}
				}
			}
		}

		object finishShowingCallback = null;

		public void showWithEffect (object finishShowingCallback = null)
		{
			this.finishShowingCallback = finishShowingCallback;
			isMoveOut = false;
            if (_mask4Hide != null)
            {
                NGUITools.SetActive(_mask4Hide, false);
            }
			if (!gameObject.activeInHierarchy || CLPanelManager.showingPanels [gameObject.name] == null) {
				NGUITools.SetActive (gameObject, true);
//				CLPanelManager.showingPanels [gameObject.name] = this;
				CLPanelManager.onShowPanel (this);
			}
			if (!isRefeshContentWhenEffectFinish) {
				callFinishShowingCallback ();
			}
			playEffect (true);
		}

		void callFinishShowingCallback ()
		{
			Utl.doCallback (finishShowingCallback, this);
		}

		/// <summary>
		/// Plaies the effect.
		/// 播放ui特效
		/// </summary>
		/// <param name='forward'>
		/// Forward.
		/// </param>
		int EffectIndex = 0;
		bool EffectForward = true;

		void playEffect (bool forward)
		{
			if (!isFinishStart) {
				Start ();
			}
			if (EffectList.Count <= 0) {
				if (!forward) {
					onFinishHideWithEffect ();
				}
				return;
			}
			if (forward) {
				EffectIndex = 0;
			} else {
				EffectIndex = EffectList.Count - 1;
			}
		
			EffectForward = forward;
			UITweener tw = null;

            //说明有动画可以播放先用mask档一下
            if (!forward)
            {
                NGUITools.SetActive(mask4Hide, true);
            }

            if (effectType == EffectType.synchronized) {
				for (int i = 0; i < EffectList.Count; i++) {
					tw = EffectList [i];
					if (forward && !isHideWithEffect) {
						tw.ResetToBeginning ();
					}
				
					if (!forward && i == EffectList.Count - 1) {
						tw.callWhenFinished = "onFinishHideWithEffect";
						tw.eventReceiver = gameObject;
					} else {
						tw.callWhenFinished = "";
					}
					tw.Play (forward);
				}
			} else {
				tw = EffectList [EffectIndex];
				if (forward && !isHideWithEffect) {
					tw.ResetToBeginning ();
				}
				tw.eventReceiver = gameObject;
				tw.callWhenFinished = "onFinishEffect";
				tw.Play (forward);
			}
		}

		void onFinishEffect (UITweener tweener)
		{
			tweener.callWhenFinished = "";
			tweener.eventReceiver = gameObject;
		
			if (EffectForward) {
				EffectIndex++;
			} else {
				EffectIndex--;
			}
			if (EffectIndex < 0) {
				if (!EffectForward) {
					onFinishHideWithEffect ();
				}
			} else if (EffectIndex >= EffectList.Count) {
				if (isRefeshContentWhenEffectFinish) {
					callFinishShowingCallback ();
				}
			} else {
				UITweener tw = EffectList [EffectIndex];
				if (EffectForward && !isHideWithEffect) {
					tw.ResetToBeginning ();
				}
				tw.eventReceiver = gameObject;
				tw.callWhenFinished = "onFinishEffect";
				tw.Play (EffectForward);
			}
		}

        GameObject _mask4Hide = null;

        public GameObject mask4Hide
        {
            get
            {
                if (_mask4Hide == null)
                {
                    _mask4Hide = new GameObject("_____mask4Hide");
                    _mask4Hide.transform.parent = transform;
                    NGUITools.SetLayer(_mask4Hide, LayerMask.NameToLayer("UI"));
                    UIWidget w = _mask4Hide.AddComponent<UIWidget>();
                    w.depth = 180;
                    w.SetAnchor(CLUIInit.self.gameObject, -2, -2, 2, 2);
                    NGUITools.AddWidgetCollider(_mask4Hide);
                    _mask4Hide.SetActive(false);
                }
                return _mask4Hide;
            }
        }

        bool isMoveOut = true;

		public void hideWithEffect (bool moveOut = false)
        {
            isMoveOut = moveOut;
			if (isMoveOut) {
				Vector3 newPos = transform.localPosition;
				newPos.z = -250;
				transform.localPosition = newPos;
				playEffect (false);
			} else {
				onFinishHideWithEffect ();
			}
		}

		void onFinishHideWithEffect (UITweener tweener = null)
		{
			isActive = false;
			CLPanelManager.onHidePanel (this);
            finishMoveOut ();
		}

        public virtual void finishMoveOut ()
		{
			Vector3 newPos = transform.localPosition;
			newPos.z = -200;
			transform.localPosition = newPos;
			NGUITools.SetActive (gameObject, false);
            if(_mask4Hide != null) {
                NGUITools.SetActive(mask4Hide, false);
            }
        }

		void finishMoveIn (UITweener tween)
		{
			if (CLPanelManager.topPanel == this) {
				callFinishShowingCallback ();
				Vector3 newPos = transform.localPosition;
				newPos.z = -200;
				transform.localPosition = newPos;
			
				if (effectType != EffectType.synchronized) {
					playEffect (true);
				}
			}
		}

		void destroySelf ()
		{
			if (isActive) {
				return;
			}
			if (!CLPanelManager.panelRetainLayer.Contains (this)) {
				//虽然页面是关掉了，但是如果还是在panelRetainLayer里，则不能删除，因为当关掉顶层页面时，这个页面还是会被打开
				CLPanelManager.destroyPanel (this, false);
			}
		}

		public virtual void procNetwork (string fname, int succ, string msg, object pars)
		{
		}

		public bool isFinishLoad {
			set;
			get;
		}
		public bool isFinishInit {
			set;
			get;
		}

		public virtual void init ()
		{
			getSubPanelsDepth ();
			_isActive = false;
			if (Application.isPlaying) {
				isFinishInit = true;
			}

			//		getSubPanelsDepth(); //此时页面还没有显示，通过 GetComponentsInChildren取不到
		}

		bool isFinishGetSubPanelsDepth = false;
		Hashtable subPanelsDepth = new Hashtable ();

		public void getSubPanelsDepth ()
		{
			if (isFinishGetSubPanelsDepth) {
				return;
			}
			isFinishGetSubPanelsDepth = true;
			UIPanel[] ps = gameObject.GetComponentsInChildren<UIPanel> ();
			int count = ps.Length;
			for (int i = 0; i < count; i++) {
				if (ps [i] == panel)
					continue;
				subPanelsDepth [ps [i]] = ps [i].depth;
			}
		}

		public void setSubPanelsDepth ()
		{
			foreach (DictionaryEntry cell in subPanelsDepth) {
				((UIPanel)(cell.Key)).depth = ((int)(cell.Value)) + panel.depth;
			}
		}

		public abstract void setData (object pars);

		public virtual void show (object pars)
		{
			setData (pars);
			show ();
		}

		public virtual void onTopPanelChange (CLPanelBase p)
		{
			//TODO:
		}

		public virtual void show ()
		{
			isActive = true;
			if (!isFinishInit) {
				init ();
			}
			showWithEffect ();
			getSubPanelsDepth ();
			refresh ();
			setSubPanelsDepth ();
			isFinishLoad = true;
		}

		public abstract void refresh ();

		/// <summary>
		/// Raises the press back event.
		/// 当点击返回键
		/// 当返回true时表明已经关闭了最顶层页面
		/// 当返回false时，表明不能关闭最顶层页面，其时可能需要弹出退程序的确认
		/// </summary>
		public virtual bool hideSelfOnKeyBack ()
		{
			return false;
		}

		public virtual void hide ()
		{
			isFinishLoad = false;
			hideWithEffect (isHideWithEffect);
		}

		public int depth {
			get {
				return panel.depth;
			}
			set {
				panel.depth = value;
			}
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			isFinishInit = false;
		}

		public virtual void prepare (object callback, object orgs)
		{
			//sprites
			UISprite[] sps = gameObject.GetComponentsInChildren<UISprite> ();
			UISprite sp = null;
			UISpriteData sd = null;

			ArrayList list = new ArrayList ();
			for (int i = 0; sps != null && i < sps.Length; i++) {
				sp = sps [i];
				if (sp.atlas == null || string.IsNullOrEmpty (sp.spriteName)) {
					continue;
				}
				list.Add (sp.atlas);
				list.Add (sp.spriteName);
			}
			prepareSprites4BorrowMode (list, 0, callback, orgs);
		}

		void onFinishPrepareOneSprite (params object[] paras)
		{
			NewList objList = paras [2] as NewList;
			ArrayList list = objList [0] as ArrayList;
			int i = (int)(objList [1]);
			object callback = objList [2];
			object orgs = objList [3];
			prepareSprites4BorrowMode (list, i + 2, callback, orgs);
			ObjPool.listPool.returnObject (objList);
		}

		public void prepareSprites4BorrowMode (ArrayList list, int i, object callback, object orgs)
		{
			if (list == null) {
				return;
			}
			if (list.Count <= i + 1) {
				Utl.doCallback (callback, orgs);
				return;
			}
			NewList paraList = ObjPool.listPool.borrowObject ();
			paraList.Add (list);
			paraList.Add (i);
			paraList.Add (callback);
			paraList.Add (orgs);
			prepareOneSprite4BorrowMode (list [i] as UIAtlas, list [i + 1] as string, (Callback)onFinishPrepareOneSprite, paraList);
		}

		public void prepareOneSprite4BorrowMode (UIAtlas atlas, string spriteName, object callback, object orgs)
		{
			UISpriteData sd = atlas.getSpriteBorrowMode (spriteName);
			if (sd != null && MapEx.get (UIAtlas.assetBundleMap, sd.path) != null) {
				Utl.doCallback (callback, null, spriteName, orgs);
			} else {
				atlas.borrowSpriteByname (spriteName, null, callback, orgs);
			}
		}
	}
}
