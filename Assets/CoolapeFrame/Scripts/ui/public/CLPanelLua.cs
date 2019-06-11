/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   页面绑定lua
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using XLua;

namespace Coolape
{
	public class CLPanelLua : CLPanelBase
	{
		bool isLoadedLua = false;
        public string frameName;
        [HideInInspector]
        public CLCellLua frameObj;

        public void reLoadLua ()
		{
			isLoadedLua = false;
		}

		LuaFunction lfhideSelfOnKeyBack;
		LuaFunction lfhide;
		LuaFunction lfinit;
		LuaFunction lfsetData;
		LuaFunction lfprocNetwork;
		LuaFunction lfshow;
		LuaFunction lfrefresh;
		LuaFunction lfUIEventDelegate;
		LuaFunction lfonTopPanelChange;
		LuaFunction lfOnDestroy;
		LuaFunction lfPrepare;
        LuaFunction lfOnApplicationPause;
        LuaFunction lfonShowFrame;

        public override void setLua ()
		{
			base.setLua ();
			lfhideSelfOnKeyBack = getLuaFunction ("hideSelfOnKeyBack");
			lfhide = getLuaFunction ("hide");
			lfinit = getLuaFunction ("init");
			lfsetData = getLuaFunction ("setData");
			lfprocNetwork = getLuaFunction ("procNetwork");
			lfshow = getLuaFunction ("show");
			lfrefresh = getLuaFunction ("refresh");
			lfUIEventDelegate = getLuaFunction ("uiEventDelegate");
			lfonTopPanelChange = getLuaFunction ("onTopPanelChange");
            lfOnApplicationPause = getLuaFunction("OnApplicationPause");
            lfOnDestroy = getLuaFunction ("OnDestroy");
			lfPrepare = getLuaFunction ("prepare");
            lfonShowFrame = getLuaFunction("onShowFrame");
        }

        public void OnApplicationPause(bool isPause)
        {
            if (lfOnApplicationPause != null)
            {
                lfOnApplicationPause.Call(isPause);
            }
        }

        public override void OnDestroy ()
		{
			if (lfOnDestroy != null) {
				lfOnDestroy.Call ();
			}
			base.OnDestroy ();
		}

		public override void onTopPanelChange (CLPanelBase p)
		{
			try {
				if (lfonTopPanelChange != null) {
					lfonTopPanelChange.Call (p);
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public override bool hideSelfOnKeyBack ()
		{
			try {
				bool isHide = false;
				object[] rets = null;
				if (lfhideSelfOnKeyBack != null) {
					rets = lfhideSelfOnKeyBack.Call ("");
				}
				if (rets != null && rets.Length > 0) {
					isHide = (bool)(rets [0]);
				}
				if (isHide) {
					CLPanelManager.hideTopPanel ();
					return true;
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
			return false;
		}

		public override void hide ()
		{
			try {
				if (lfhide != null)
					lfhide.Call ("");
				base.hide ();
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public override void init ()
		{
			try {
				if (isFinishInit)
					return;
				getSubPanelsDepth ();
				setLua ();
//      base.init ();
				if (lfinit != null) {
					lfinit.Call (this);
				}
				if (Application.isPlaying) {
					isFinishInit = true;
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public override void setData (object pars)
		{
			try {
				init ();
				if (lfsetData != null) {
					lfsetData.Call (pars);
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public override void procNetwork (string cmd, int succ, string msg, object pars)
		{
			try {
				init ();
				base.procNetwork (cmd, succ, msg, pars);
				if (lfprocNetwork != null) {
					lfprocNetwork.Call (cmd, succ, msg, pars);
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public override void show (object pars)
		{
			base.show (pars);
		}

		bool isFinishedMaskPanel = false;

		public override void show ()
		{
			isActive = true;
			try {
				if (isNeedMask4Init) {
					if (isNeedMask4InitOnlyOnce) {
						if (isFinishedMaskPanel) {
							_show ();
						} else {
							Callback cb = onfinishShowMask;
							CLPanelMask4Panel.show (cb, null);
							isFinishedMaskPanel = true;
						}
					} else {
						Callback cb = onfinishShowMask;
						CLPanelMask4Panel.show (cb, null);
					}
				} else {
					_show ();
				}

                showFrame();
            } catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public void onfinishShowMask (params object[] para)
		{
			_show ();
			doPrepare ((Callback)hideMask);
			Invoke ("doHideMask", 10);
		}

		public  void doPrepare (object callback)
		{
			if (lfPrepare != null) {
				base.prepare (lfPrepare, callback);
			} else {
				base.prepare (callback, null);
			}
		}

		void hideMask (params object[] paras)
		{
			CancelInvoke ("doHideMask");
			doHideMask ();
		}

		public void doHideMask ()
		{
//			if (isNeedMask4Init) {
			CLPanelMask4Panel.hide (null);
//			}
		}

		public void _show ()
		{
			Callback callback = doShowing;
			base.showWithEffect (callback);
		}

		void doShowing (params object[] paras)
		{
			if (!isFinishInit) {
				init ();
			}
        
			if (lfshow != null) {
				lfshow.Call ("");
			}
			getSubPanelsDepth ();
			refresh ();
			isFinishLoad = true;
		}

		public override void refresh ()
		{
			try {
				setSubPanelsDepth ();
				if (lfrefresh != null) {
					lfrefresh.Call ("");
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

        public override void finishMoveOut()
        {
            releaseFrame();
            base.finishMoveOut();
        }

        public void uiEventDelegate (GameObject go)
		{
			try {
				if (lfUIEventDelegate != null) {
					lfUIEventDelegate.Call (go);
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		//== proc event ==============
		public void onClick4Lua (GameObject button, string functionName)
		{
			try {
				LuaFunction f = getLuaFunction (functionName);
				if (f != null) {
					f.Call (button);
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public void onDoubleClick4Lua (GameObject button, string functionName)
		{
			try {
				LuaFunction f = getLuaFunction (functionName);
				if (f != null) {
					f.Call (button);
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public void onHover4Lua (GameObject button, string functionName, bool isOver)
		{
			try {
				LuaFunction f = getLuaFunction (functionName);
				if (f != null) {
					f.Call (button, isOver);
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public void onPress4Lua (GameObject button, string functionName, bool isPressed)
		{
			try {
				LuaFunction f = getLuaFunction (functionName);
				if (f != null) {
					f.Call (button, isPressed);
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public void onDrag4Lua (GameObject button, string functionName, Vector2 delta)
		{
			try {
				LuaFunction f = getLuaFunction (functionName);
				if (f != null) {
					f.Call (button, delta);
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public void onDrop4Lua (GameObject button, string functionName, GameObject go)
		{
			try {
				LuaFunction f = getLuaFunction (functionName);
				if (f != null) {
					f.Call (button, go);
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public void onKey4Lua (GameObject button, string functionName, KeyCode key)
		{
			try {
				LuaFunction f = getLuaFunction (functionName);
				if (f != null) {
					f.Call (button, key);
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}


        public void showFrame()
        {
            if (string.IsNullOrEmpty(frameName)) return;
            CLUIOtherObjPool.borrowObjAsyn(frameName, (Callback)onBorrowFrame);
        }

        void onBorrowFrame(params object[] objs)
        {
            GameObject frame = objs[1] as GameObject;
            if (frame != null)
            {
                if (frameObj != null)
                {
                    CLUIOtherObjPool.returnObj(frame);
                    NGUITools.SetActive(frame, false);
                    return;
                }
                frameObj = frame.GetComponent<CLCellLua>();
                if (frameObj == null)
                {
                    frameObj = frame.AddComponent<CLCellLua>();
                }
                frameObj.transform.parent = transform;
                frameObj.transform.localScale = Vector3.one;
                frameObj.transform.localPosition = Vector3.zero;
                frameObj.transform.localEulerAngles = Vector3.zero;
                NGUITools.SetActive(frameObj.gameObject, true);
                if (frameObj.luaTable == null)
                {
                    frameObj.setLua();
                }

                if (lfonShowFrame != null)
                {
                    lfonShowFrame.Call(this);
                }
            }
        }

        public void releaseFrame()
        {
            if (frameObj != null)
            {
                CLUIOtherObjPool.returnObj(frameObj.gameObject);
                NGUITools.SetActive(frameObj.gameObject, false);
                frameObj = null;
            }
        }
    }
}
