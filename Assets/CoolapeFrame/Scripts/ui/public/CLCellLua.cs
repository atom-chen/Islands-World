/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   ui列表元素 绑定lua
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
	public class CLCellLua : CLCellBase
	{
		public bool isNeedResetAtlase = true;
		bool isFinishInit = false;
		object onClickCallback;
		LuaFunction lfInit = null;
		LuaFunction lfshow = null;
		LuaFunction lfRefresh = null;

		public override void setLua ()
		{
			base.setLua ();
			initLuaFunc ();
		}

		public void initLuaFunc ()
		{
			lfInit = getLuaFunction ("init");
			lfshow = getLuaFunction ("show");
			lfRefresh = getLuaFunction ("refresh");
		}

		public void init (object data)
		{
			init (data, null);
		}
		public override void init (object data, object onClick)
		{
			try {
				onClickCallback = onClick;
				if (!isFinishInit) {
					setLua ();
					isFinishInit = true;
					if (lfInit != null) {
						lfInit.Call (this);
					}
				}
		
				if (lfshow != null) {
					lfshow.Call (gameObject, data);
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public void refresh (object paras)
		{
			try {
				if (lfRefresh != null) {
					lfRefresh.Call (paras);
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public void OnClick ()
		{
			try {
				if (onClickCallback != null) {
					if (typeof(LuaFunction) == onClickCallback.GetType ()) {
						((LuaFunction)onClickCallback).Call (this);
					} else if (typeof(Callback) == onClickCallback.GetType ()) {
						((Callback)onClickCallback) (this);
					}
				}
                base.OnClick();
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
	}
}
