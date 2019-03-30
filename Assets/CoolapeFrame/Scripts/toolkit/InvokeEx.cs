using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Coolape
{
	public class InvokeEx : MonoBehaviour
	{
		public bool canFixedUpdate = false;
		public bool canUpdate = false;
		public static InvokeEx self;

		public InvokeEx ()
		{
			self = this;
		}

		Hashtable coroutineMap = Hashtable.Synchronized (new Hashtable ());
		Hashtable coroutineIndex = Hashtable.Synchronized (new Hashtable ());

		/// <summary>
		/// Invoke4s the lua.回调lua函数， 等待时间
		/// </summary>
		/// <param name='callbakFunc'> only support Luafunction or Callback
		/// Callbak func.
		/// </param>
		/// <param name='sec'>
		/// Sec.
		/// </param>

		public static UnityEngine.Coroutine invoke (object callbakFunc, float sec)
		{
			return self.invoke4Lua (callbakFunc, sec);
		}

		public static UnityEngine.Coroutine invoke (object callbakFunc, object orgs, float sec)
		{
			return self.invoke4Lua (callbakFunc, orgs, sec);
		}

		public static UnityEngine.Coroutine invoke (object callbakFunc, object orgs, float sec, bool onlyOneCoroutine)
		{
			return self.invoke4Lua (callbakFunc, orgs, sec, onlyOneCoroutine);
		}

		public UnityEngine.Coroutine invoke4Lua (object callbakFunc, float sec)
		{
			return invoke4Lua (callbakFunc, "", sec);
		}

		public UnityEngine.Coroutine invoke4Lua (object callbakFunc, object orgs, float sec)
		{
			return invoke4Lua (callbakFunc, orgs, sec, false);
		}

		/// <summary>
		/// Invoke4s the lua.
		/// </summary>
		/// <param name="callbakFunc">Callbak func.lua函数</param>
		/// <param name="orgs">Orgs.参数</param>
		/// <param name="sec">Sec.等待时间</param>
		public UnityEngine.Coroutine invoke4Lua (object callbakFunc, object orgs, float sec, bool onlyOneCoroutine)
		{
			if (!gameObject.activeInHierarchy)
				return null;
			if (callbakFunc == null) {
				Debug.LogError ("callbakFunc is null ......");
				return null;
			}
			try {
				UnityEngine.Coroutine ct = null;
				if (onlyOneCoroutine) {
					cleanCoroutines (callbakFunc);
				}
				int index = getCoroutineIndex (callbakFunc);
				ct = StartCoroutine (doInvoke4Lua (callbakFunc, sec, orgs, index));
				setCoroutine (callbakFunc, ct, index);
				return ct;
			} catch (System.Exception e) {
				Debug.LogError (callbakFunc + ":" + e);
				return null;
			}
		}

		public  int getCoroutineIndex (object callbakFunc)
		{
			object key = getKey4InvokeMap (callbakFunc, coroutineIndex);
			int ret = MapEx.getInt (coroutineIndex, key);
			coroutineIndex [key] = ret + 1;
			return ret;
		}

		public  void setCoroutineIndex (object callbakFunc, int val)
		{
			object key = getKey4InvokeMap (callbakFunc, coroutineIndex);
			coroutineIndex [key] = val;
		}

		/// <summary>
		/// Gets the key4 invoke map.当直接传luafunction时，不能直接用，用Equals查找一下key
		/// </summary>
		/// <returns>The key4 invoke map.</returns>
		/// <param name="callbakFunc">Callbak func.</param>
		/// <param name="map">Map.</param>
		public  object getKey4InvokeMap (object callbakFunc, Hashtable map)
		{
            if (callbakFunc == null || map == null)
				return callbakFunc;
			object key = callbakFunc;
			if (callbakFunc != null) {
				NewList keys = ObjPool.listPool.borrowObject ();
				keys.AddRange (map.Keys);
				for (int i = 0; i < keys.Count; i++) {
					if ((callbakFunc).Equals ((keys [i]))) {
						key = keys [i];
						break;
					}
				}
				ObjPool.listPool.returnObject (keys);
				keys = null;
			}
			return key;
		}

		public  Hashtable getCoroutines (object callbakFunc)
		{
			object key = getKey4InvokeMap (callbakFunc, coroutineMap);
            Hashtable ret = MapEx.getMap(coroutineMap, key);
            ret = ret == null ? new Hashtable() : ret;
            coroutineMap[key] = ret;
            return ret;
		}

		public  void setCoroutine (object callbakFunc, UnityEngine.Coroutine ct, int index)
		{
			object key = getKey4InvokeMap (callbakFunc, coroutineMap);
			Hashtable map = getCoroutines (callbakFunc);
			map [index] = ct;
			coroutineMap [key] = map;
		}

		public  void cleanCoroutines (object callbakFunc)
		{
			object key = getKey4InvokeMap (callbakFunc, coroutineMap);
			Hashtable list = getCoroutines (callbakFunc);
			foreach (DictionaryEntry cell in list) {
				StopCoroutine ((UnityEngine.Coroutine)(cell.Value));
			}
			list.Clear ();
			setCoroutineIndex (callbakFunc, 0);
			coroutineMap.Remove (key);
		}

		public  void rmCoroutine (object callbakFunc, int index)
		{
			object key = getKey4InvokeMap (callbakFunc, coroutineMap);
			Hashtable list = getCoroutines (callbakFunc);
			list.Remove (index);
			coroutineMap [key] = list;
		}

		public static void cancelInvoke ()
		{
			self.cancelInvoke4Lua ();
		}

		public static void cancelInvoke (object callbakFunc)
		{
			self.cancelInvoke4Lua (callbakFunc);
		}

		public  void cancelInvoke4Lua ()
		{
			cancelInvoke4Lua (null);
		}

		public  void cancelInvoke4Lua (object callbakFunc)
		{
			if (callbakFunc == null) {
				Hashtable list = null;
                NewList keys = ObjPool.listPool.borrowObject();
                keys.AddRange(coroutineMap.Keys);
                object key = null;
                for (int i = 0; i < keys.Count; i++) {
                    key = keys[i];
                    if (key != null)
                    {
                        list = getCoroutines(key);
                        foreach (DictionaryEntry cell in list)
                        {
                            StopCoroutine((UnityEngine.Coroutine)(cell.Value));
                        }
                        list.Clear();
                    }
				}
				coroutineMap.Clear ();
				coroutineIndex.Clear ();
                ObjPool.listPool.returnObject(keys);
			} else {
				cleanCoroutines (callbakFunc);
			}
		}

		Queue invokeFuncs = new Queue ();

		IEnumerator doInvoke4Lua (object callbakFunc, float sec, object orgs, int index)
		{
			yield return new WaitForSeconds (sec);
			try {
				rmCoroutine (callbakFunc, index);
				Utl.doCallback (callbakFunc, orgs);
			} catch (System.Exception e) {
				string msg = "call err:doInvoke4Lua" + ",callbakFunc=[" + callbakFunc + "]";
				Debug.LogError (msg);
				Debug.LogError (e);
			}
		}

		//================================================
		// Fixed invoke 4 lua
		//================================================
		long _frameCounter = 0;
		object locker=new object();

		public long frameCounter {
			get {
				lock (locker) {
					return _frameCounter;
				}
			}
			set {
				lock (locker) {
					_frameCounter = value;
				}
			}
		}

		Hashtable _fixedInvokeMap = new Hashtable ();

		public Hashtable fixedInvokeMap {
			get {
				if (_fixedInvokeMap == null) {
					_fixedInvokeMap = Hashtable.Synchronized (new Hashtable ());
				}
				return _fixedInvokeMap;
			}
		}

		public static void invokeByFixedUpdate (object luaFunc, float waitSec)
		{
			if (self == null) {
				Debug.LogError ("Must attach InvokeEx on some gameObject!");
				return;
			}
			self._fixedInvoke (luaFunc, null, waitSec);
		}

		public static void invokeByFixedUpdate (object luaFunc, object paras, float waitSec)
		{
			if (self == null) {
				Debug.LogError ("Must attach InvokeEx on some gameObject!");
				return;
			}
			self._fixedInvoke (luaFunc, paras, waitSec);
		}

		public void fixedInvoke4Lua (object luaFunc, float waitSec)
		{
			_fixedInvoke (luaFunc, null, waitSec);
		}

		public void fixedInvoke4Lua (object luaFunc, object paras, float waitSec)
		{
			_fixedInvoke (luaFunc, paras, waitSec);
		}

		void _fixedInvoke (object callback, object paras, float waitSec)
		{
			int waiteFrame = Mathf.CeilToInt (waitSec / Time.fixedDeltaTime);
			waiteFrame = waiteFrame <= 0 ? 1 : waiteFrame; //至少有帧
			long key = frameCounter + waiteFrame; 
			object[] content = new object[2];
			//		print (waiteFrame + "===" + key +"====" + luaFunc);
			List<object[]> funcList = (List<object[]>)(fixedInvokeMap [key]);
			if (funcList == null) {
				funcList = new List<object[]> ();
			}
			content [0] = callback;
			content [1] = paras;
			funcList.Add (content);
			fixedInvokeMap [key] = funcList;
			if(!canFixedUpdate) {
				canFixedUpdate = true;
			}
		}

		public static void cancelInvokeByFixedUpdate ()
		{
			cancelInvokeByFixedUpdate (null);
		}

		public static void cancelInvokeByFixedUpdate (object func)
		{
			self.cancelFixedInvoke4Lua (func);
		}

		public void cancelFixedInvoke4Lua ()
		{
			cancelFixedInvoke4Lua (null);
		}

		public void cancelFixedInvoke4Lua (object func)
		{
			if (func == null) {
				if (fixedInvokeMap != null) {
					fixedInvokeMap.Clear ();
				}
				return;
			}
			List<object[]> list = null;
			int count = 0;
			object[] content = null;
			foreach (DictionaryEntry item in fixedInvokeMap) {
				list = (List<object[]>)(item.Value);
				count = list.Count;
				for (int i = count - 1; i >= 0; i--) {
					content = list [i];
					if (func.Equals (content [0])) {
						list.RemoveAt (i);
					}
				}

				if(list.Count == 0) {
					fixedInvokeMap.Remove (item);
				}
			}
		}

		void doFixedInvoke (long key)
		{
			if (fixedInvokeMap == null && fixedInvokeMap.Count <= 0)
				return;
			if (fixedInvokeMap [key] == null)
				return;
			object[] content = null;
			List<object[]> funcList = (List<object[]>)(fixedInvokeMap [key]);
			object callback = null;
			if (funcList != null) {
				for (int i = 0; i < funcList.Count; i++) {
					content = funcList [i];
					callback = content [0];
					Utl.doCallback (callback, content [1]);
				}
				funcList.Clear ();
				funcList = null;
				fixedInvokeMap.Remove (key);
			}
		}

		//================================================
		// FixedUpdate
		//================================================
		//帧统计
		public virtual void FixedUpdate ()
		{
			if (!canFixedUpdate)
				return;
			frameCounter++;
			if (fixedInvokeMap != null && fixedInvokeMap.Count > 0) {
				doFixedInvoke (frameCounter);
			} else {
				canFixedUpdate = false;
				frameCounter = 0;
			}
		}

		//================================================
		// Update
		//================================================
		ArrayList _invokeByUpdateList = null;

		ArrayList invokeByUpdateList {
			get {
				if (_invokeByUpdateList == null) {
					_invokeByUpdateList = ArrayList.Synchronized (new ArrayList ());
				}
				return _invokeByUpdateList;
			}
		}

		/// <summary>
		/// Invoke4s the lua.
		/// </summary>
		/// <param name="callbakFunc">Callbak func.lua函数</param>
		/// <param name="orgs">Orgs.参数</param>
		/// <param name="sec">Sec.等待时间</param>
		public static void invokeByUpdate (object callbakFunc, float sec)
		{
			self.updateInvoke (callbakFunc, sec);
		}

		public static void invokeByUpdate (object callbakFunc, object orgs, float sec)
		{
			self.updateInvoke (callbakFunc, orgs, sec);
		}

		public void updateInvoke (object callbakFunc, float sec)
		{
			updateInvoke (callbakFunc, null, sec);
		}

		public void updateInvoke (object callbakFunc, object orgs, float sec)
		{
			if (callbakFunc == null)
				return;
			NewList list = ObjPool.listPool.borrowObject ();
			list.Add (callbakFunc);
			list.Add (orgs);
			list.Add (Time.unscaledTime + sec);
			invokeByUpdateList.Add (list);
			canUpdate = true;
		}

		public static void cancelInvokeByUpdate ()
		{
			self.cancelUpdateInvoke ();
		}

		public static void cancelInvokeByUpdate (object callbakFunc)
		{
			self.cancelUpdateInvoke (callbakFunc);
		}

		public void cancelUpdateInvoke ()
		{
			cancelUpdateInvoke (null);
		}

		public void cancelUpdateInvoke (object callbakFunc)
		{
			NewList list = null;
			int count = invokeByUpdateList.Count;
			if (callbakFunc == null) {
				for (int i = 0; i < count; i++) {
					list = (NewList)(invokeByUpdateList [i]);
					ObjPool.listPool.returnObject (list);
				}
				list = null;
				invokeByUpdateList.Clear ();
				return;
			}
			for (int i = count - 1; i >= 0; i--) {
				list = (NewList)(invokeByUpdateList [i]);
				if (callbakFunc.Equals (list [0])) {
					invokeByUpdateList.RemoveAt (i);
					ObjPool.listPool.returnObject (list);
				}
			}
			list = null;
		}

		void doInvokeByUpdate ()
		{
			int count = invokeByUpdateList.Count;
			NewList list = null;
			object callbakFunc;
			object orgs;
			float sec;
			int index = 0;
			while (index < invokeByUpdateList.Count) {
				list = (NewList)(invokeByUpdateList [index]);
				callbakFunc = list [0];
				orgs = list [1];
				sec = (float)(list [2]);
				if (sec <= Time.unscaledTime) {
					Utl.doCallback (callbakFunc, orgs);
					invokeByUpdateList.RemoveAt (index);
					ObjPool.listPool.returnObject (list);
				} else {
					index++;
				}
			}
			list = null;
		}

		public virtual void Update ()
		{
			if (!canUpdate)
				return;
			if (invokeByUpdateList.Count > 0) {
				doInvokeByUpdate ();
			} else {
				canUpdate = false;
			}
		}
	}
}
