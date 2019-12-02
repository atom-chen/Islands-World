/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  把mobobehaviour的处理都转到lua层
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
	public class CLBehaviour4Lua : CLBaseLua
	{
		public override void setLua ()
		{
			base.setLua ();
			initGetLuaFunc ();
		}

		// 把lua方法存在起来
		public virtual void initGetLuaFunc ()
		{
			if (luaTable != null) {
				flStart = getLuaFunction ("Start");
				flAwake = getLuaFunction ("Awake");
//			flReset = getLuaFunction ("Reset");
				flOnTriggerEnter = getLuaFunction ("OnTriggerEnter");
				flOnTriggerExit = getLuaFunction ("OnTriggerExit");
				flOnTriggerStay = getLuaFunction ("OnTriggerStay");
				flOnCollisionEnter = getLuaFunction ("OnCollisionEnter");
				flOnCollisionExit = getLuaFunction ("OnCollisionExit");
				flOnApplicationPause = getLuaFunction ("OnApplicationPause");
				flOnApplicationFocus = getLuaFunction ("OnApplicationFocus");
				flOnBecameInvisible = getLuaFunction ("OnBecameInvisible");
				flOnBecameVisible = getLuaFunction ("OnBecameVisible");
				flOnControllerColliderHit = getLuaFunction ("OnControllerColliderHit");
				flOnDestroy = getLuaFunction ("OnDestroy");
				flOnDisable = getLuaFunction ("OnDisable");
				flOnEnable = getLuaFunction ("OnEnable");
				flOnWillRenderObject = getLuaFunction ("OnWillRenderObject");
				flOnPreRender = getLuaFunction ("OnPreRender");
				flOnPostRender = getLuaFunction ("OnPostRender");
				flOnClick = getLuaFunction ("OnClick");
				flOnPress = getLuaFunction ("OnPress");
				flOnDrag = getLuaFunction ("OnDrag");
				flUIEventDelegate = getLuaFunction ("uiEventDelegate");
				flclean = getLuaFunction ("clean");
				flApplicationQuit = getLuaFunction ("OnApplicationQuit");
			}
		}

		public LuaFunction flclean = null;
		public LuaFunction flApplicationQuit = null;

		bool isQuit = false;
		public virtual void OnApplicationQuit (){
			isQuit = true;
			if (flApplicationQuit != null) {
                call(flApplicationQuit);
			}
		}

		public virtual void clean ()
		{
			if (flclean != null) {
                call(flclean);
			}
			if (isQuit)
				return;
			
			cancelInvoke4Lua (null);
			CancelInvoke ();
			StopAllCoroutines ();
		}

		public LuaFunction flStart = null;
		public LuaFunction flAwake = null;
		// Use this for initialization
		public  virtual void Start ()
		{
			if (flStart != null) {
                call(flStart, gameObject);
			}
		}

		public  virtual void Awake ()
		{
			if (flAwake != null) {
                call(flAwake, gameObject);
			}
		}
	
		public LuaFunction flOnTriggerEnter = null;

		public virtual  void OnTriggerEnter (Collider other)
		{
			if (flOnTriggerEnter != null) {
                call(flOnTriggerEnter, other);
			}
		}

		public LuaFunction flOnTriggerExit = null;

		public virtual  void OnTriggerExit (Collider other)
		{
			if (flOnTriggerExit != null) {
                call(flOnTriggerExit, other);
			}
		}

		public LuaFunction flOnTriggerStay = null;

		public virtual  void OnTriggerStay (Collider other)
		{
			if (flOnTriggerStay != null) {
                call(flOnTriggerStay, other);
			}
		}

		public LuaFunction flOnCollisionEnter = null;

		public virtual  void OnCollisionEnter (Collision collision)
		{
			if (flOnCollisionEnter != null) {
                call(flOnCollisionEnter, collision);
			}
		}

		public LuaFunction flOnCollisionExit = null;

		public virtual  void OnCollisionExit (Collision collisionInfo)
		{
			if (flOnCollisionExit != null) {
                call(flOnCollisionExit, collisionInfo);
			}
		}

		public LuaFunction flOnApplicationPause = null;

		public virtual  void OnApplicationPause (bool pauseStatus)
		{
			if (flOnApplicationPause != null) {
                call(flOnApplicationPause, pauseStatus);
			}
		}

		public LuaFunction flOnApplicationFocus = null;

		public virtual  void OnApplicationFocus (bool focusStatus)
		{
			if (flOnApplicationFocus != null) {
                call(flOnApplicationFocus, focusStatus);
			}
		}

		public LuaFunction flOnBecameInvisible = null;

		public virtual  void OnBecameInvisible ()
		{
			if (isQuit)
				return;
			if (flOnBecameInvisible != null) {
                call(flOnBecameInvisible, gameObject);
			}
		}

		public LuaFunction flOnBecameVisible = null;

		public virtual  void OnBecameVisible ()
		{
			if (flOnBecameVisible != null) {
                call(flOnBecameVisible, gameObject);
			}
		}

		public LuaFunction flOnControllerColliderHit = null;

		public virtual  void OnControllerColliderHit (ControllerColliderHit hit)
		{
			if (flOnControllerColliderHit != null) {
                call(flOnControllerColliderHit, hit);
			}
		}

		public LuaFunction flOnDestroy = null;

		public override  void OnDestroy ()
		{
			if (flOnDestroy != null) {
                call(flOnDestroy, gameObject);
			}
			base.OnDestroy ();
		}

		public LuaFunction flOnDisable = null;

		public virtual  void OnDisable ()
		{
			if (flOnDisable != null) {
                call(flOnDisable, gameObject);
			}
		}

		public LuaFunction flOnEnable = null;

		public virtual  void OnEnable ()
		{
			if (flOnEnable != null) {
                call(flOnEnable, gameObject);
			}
		}

		public LuaFunction flOnWillRenderObject = null;

		public virtual  void OnWillRenderObject ()
		{
			if (flOnWillRenderObject != null) {
                call(flOnWillRenderObject, gameObject);
			}
		}

		public LuaFunction flOnPreRender = null;

		public virtual  void OnPreRender ()
		{
			if (flOnPreRender != null) {
                call(flOnPreRender, gameObject);
			}
		}

		public LuaFunction flOnPostRender = null;

		public virtual  void OnPostRender ()
		{
			if (flOnPostRender != null) {
                call(flOnPostRender, gameObject);
            }
		}

		public LuaFunction flOnClick = null;

		public virtual  void OnClick ()
		{
			if (flOnClick != null) {
                call(flOnClick, gameObject);
            }
		}

		public LuaFunction flOnPress = null;

		public virtual  void OnPress (bool isPressed)
		{
			if (flOnPress != null) {
                call(flOnPress, gameObject, isPressed);
			}
		}

		public LuaFunction flOnDrag = null;

		public virtual  void OnDrag (Vector2 delta)
		{
			if (flOnDrag != null) {
                call(flOnDrag, gameObject, delta);
            }
		}

		public LuaFunction flUIEventDelegate = null;

		/// <summary>
		/// User interfaces the event delegate. 
		/// </summary>
		/// <param name="go">Go.</param>
		public virtual  void uiEventDelegate (GameObject go)
		{
			if (flUIEventDelegate != null) {
                call(flUIEventDelegate, go);
			}
		}
	}
}
