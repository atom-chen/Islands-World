/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   资源对象池基类
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;
using XLua;

namespace Coolape
{
	[LuaCallCSharp]
	public abstract class CLAssetsPoolBase<T> where T: UnityEngine.Object
	{
		public CLDelegate OnSetPrefabCallbacks4Borrow = new CLDelegate ();
		public CLDelegate OnSetPrefabCallbacks = new CLDelegate ();
//		public static ListPool listPool = new ListPool ();
		public bool isFinishInitPool = false;
		public Hashtable poolMap = new Hashtable ();
		public Hashtable prefabMap = new Hashtable ();
		public Hashtable isSettingPrefabMap = new Hashtable ();


		public void _clean ()
		{
			isFinishInitPool = false;
			poolMap.Clear ();
			prefabMap.Clear ();
		}

		public virtual void initPool ()
		{
			if (isFinishInitPool)
				return;
			isFinishInitPool = true;
			//TODO:
		}

		/// <summary>
		/// Wraps the path. 包装路径，以便支持 dir1/dir2/test.unity3d
		/// </summary>
		/// <returns>The path.</returns>
		/// <param name="basePath">Base path.</param>
		/// <param name="thingName">Thing name.</param>
		public static string wrapPath (string basePath, string thingName)
		{
			string tmpStr = thingName.Replace (".", "/");
			string[] strs = tmpStr.Split ('/');
			string path = basePath;
			int len = strs.Length;
			for (int i = 0; i < len - 1; i++) {
				path = PStr.begin (path).a ("/").a (strs [i]).end ();
			}
			path = PStr.begin (path).a ("/").a (CLPathCfg.self.platform).a ("/").a (strs [len - 1]).a (".unity3d").end ();
			return path;
		}

		#region 设置预设

		//设置预设===========
		public bool _havePrefab (string name)
		{
			return prefabMap.Contains (name);
		}


        public virtual bool isAutoReleaseAssetBundle
        {
            get {
                return true;
            }
        }

		/// <summary>
		/// Gets the asset path.需要在扩展类中，实在该方法，调用时会用到doSetPrefab
		/// </summary>
		/// <returns>The asset path.</returns>
		/// <param name="name">Name.</param>
		public abstract string getAssetPath (string name);

        public virtual void _setPrefab (string name, object finishCallback, object  orgs, object progressCB)
		{
			string path = getAssetPath (name);
			OnSetPrefabCallbacks.add (name, finishCallback, orgs);
			doSetPrefab (path, name, (Callback)onFinishSetPrefab, name, progressCB);
		}

		public virtual void onFinishSetPrefab (object[] paras)
		{
			if (paras != null && paras.Length > 1) {
				T unit = paras [0] as T; 
				string name = paras [1].ToString ();
				ArrayList list = OnSetPrefabCallbacks.getDelegates (name);
				int count = list.Count;
				ArrayList cell = null;
				object cb = null;
				object orgs = null;
				for (int i = 0; i < count; i++) {
					cell = list [i] as ArrayList;
					if (cell != null && cell.Count > 1) {
						cb = cell [0];
						orgs = cell [1];
						if (cb != null) {
							Utl.doCallback (cb, unit, orgs);
						}
					}
				}
				list.Clear ();
				OnSetPrefabCallbacks.removeDelegates (name);
			}
		}


		public void doSetPrefab (string path, string name, object finishCallback, object args, object progressCB)
		{
			if (name == null)
				return;
            if (MapEx.getBool (isSettingPrefabMap, name)) {
				return;
			}
			if (_havePrefab (name)) {
				if (finishCallback != null) {
					Utl.doCallback (finishCallback, prefabMap [name], args);
				}
			} else {
				isSettingPrefabMap [name] = true;
				Callback cb = onGetAssetsBundle;
				CLVerManager.self.getNewestRes (path, 
					CLAssetType.assetBundle, 
					cb, isAutoReleaseAssetBundle, finishCallback, name, args, progressCB);
			}
		}

		public void finishSetPrefab (T unit)
		{
            if (unit != null)
            {
                prefabMap[unit.name] = unit;
            }
			isSettingPrefabMap.Remove (unit.name);
		}

		public virtual void onGetAssetsBundle (params object[] paras)
		{
			string name = "";
			string path = "";
			try {
				if (paras != null) {
					path = (paras [0]).ToString ();
					AssetBundle asset = (paras [1]) as AssetBundle;
					object[] org = (object[])(paras [2]);
					object cb = org [0];
					name = (org [1]).ToString ();
					object args = org [2];
					object progressCB = org [3];

					if (asset == null) {
						Debug.LogError("get asset is null. path =" + path);
                        finishSetPrefab(null);
                        Utl.doCallback (cb, null, args);
						return;
					}

					GameObject go = null;
					T unit = null;
					if (typeof(T) == typeof(AssetBundle)) {
						unit = asset as T;
						unit.name = name;
					} else {
						if (typeof(T) == typeof(GameObject)) {
//						Debug.Log ("11111name====" + name);
							unit = asset.mainAsset as T;
							unit.name = name;
						} else {
//						Debug.Log ("22222name====" + name);
							go = asset.mainAsset as GameObject;
							if (go != null) {
								go.name = name;
								unit = go.GetComponent<T> ();
							} else {
//							Debug.Log ("33333name====" + name);
								unit = asset.mainAsset as T;
								unit.name = name;
							}
						}
					}
					CLAssetsManager.self.addAsset (getAssetPath (name), name, asset, realseAsset);
					sepcProc4Assets (unit, cb, args, progressCB);
				} else {
					Debug.LogError ("Get assetsbundle failed!");
				}
			} catch (System.Exception e) {
				Debug.LogError ("path==" + path + "," + e + name);
			}
		}

		/// <summary>
		/// Sepcs the proc4 assets.
		/// </summary>
		/// <param name="unit">Unit.</param>
		/// <param name="cb">Cb.</param>
		/// <param name="args">Arguments.</param>
		public virtual void sepcProc4Assets (T unit, object cb, object args, object progressCB)
		{
			GameObject go = null;
			if (typeof(T) == typeof(GameObject)) {
				go = unit as GameObject;
			} else if (unit is MonoBehaviour) {
				go = (unit as MonoBehaviour).gameObject;
			}
			if (go != null) {
				CLSharedAssets sharedAsset = go.GetComponent<CLSharedAssets> ();
				if (sharedAsset != null) {
					NewList param = ObjPool.listPool.borrowObject ();
					param.Add (cb);
					param.Add (unit);
					param.Add (args);
					sharedAsset.init ((Callback)onGetSharedAssets, param, progressCB);
				} else {
					finishSetPrefab (unit);
					Utl.doCallback (cb, unit, args);
				}
			} else {
				finishSetPrefab (unit);
				Utl.doCallback (cb, unit, args);
			}
		}

		public virtual void onGetSharedAssets (params object[] param)
		{
			if (param == null) {
				Debug.LogWarning ("param == null");
				return;
			}
			NewList list = (NewList)(param [0]);
			if (list.Count >= 3) {
				object cb = list [0];
				T obj = list [1] as T;
				object orgs = list [2];
				finishSetPrefab (obj);
				if (cb != null) {
					Utl.doCallback (cb, obj, orgs);
				}
			} else {
				Debug.LogWarning ("list.Count ====0");
			}
			ObjPool.listPool.returnObject (list);
		}

		//释放资源
		public virtual void realseAsset (params object[] paras)
		{
			string name = "";
			try {
				name = paras [0].ToString ();
				object obj = poolMap [name];
				ObjsPubPool pool = null;
				T unit = null;
				MonoBehaviour unitObj = null;

				if (obj != null) {
					pool = obj as ObjsPubPool;
				}
				if (pool != null) {
					while (pool.queue.Count > 0) {
						unit = pool.queue.Dequeue ();
						if (unit != null) {
							if (unit is MonoBehaviour) {
								unitObj = unit as MonoBehaviour;
								GameObject.DestroyImmediate (unitObj.gameObject, true);
							} else if (unit is GameObject) {
								GameObject.DestroyImmediate ((unit as GameObject), true);
							}
						}
						unit = null;
					}
					pool.queue.Clear ();
				}

				unit = (T)(prefabMap [name]);
				prefabMap.Remove (name);
				if (unit != null) {
					if (unit is AssetBundle) {
						//do nothing, CLAssetsManager will unload assetbundle
					} else {
						if (unit is MonoBehaviour) {
							unitObj = unit as MonoBehaviour;
							CLSharedAssets sharedAsset = unitObj.GetComponent<CLSharedAssets> ();
							if (sharedAsset != null) {
								sharedAsset.returnAssets ();
							}
							GameObject.DestroyImmediate (unitObj.gameObject, true);
						} else if (unit is GameObject) {
							CLSharedAssets sharedAsset = (unit as GameObject).GetComponent<CLSharedAssets> ();
							if (sharedAsset != null) {
								sharedAsset.returnAssets ();
							}
							GameObject.DestroyImmediate ((unit as GameObject), true);
						} else {
							//UnityEngine.Resources.UnloadAsset ((Object)unit);
							GameObject.DestroyImmediate (unit, true);
						}
					}
				}
				unit = null;
			} catch (System.Exception e) {
				Debug.LogError ("name==" + name + ":" + e);
			}
		}

		#endregion

		public ObjsPubPool getObjPool (string name)
		{
			object obj = poolMap [name];
			ObjsPubPool pool = null;
			if (obj == null) {
				pool = new ObjsPubPool (prefabMap);
				poolMap [name] = pool;
			} else {
				pool = (ObjsPubPool)obj;
			}
			return pool;
		}

		public virtual T _borrowObj (string name)
		{
			return _borrowObj (name, false);
		}

		public virtual T _borrowObj (string name, bool isSharedResource)
		{
			T r = null;
			if (isSharedResource) {
				r = (T)(prefabMap [name]);
			} else {
				object obj = poolMap [name];
				ObjsPubPool pool = getObjPool (name);
				r = pool.borrowObject (name);
				poolMap [name] = pool;
			}
			if (_havePrefab (name)) {
				CLAssetsManager.self.useAsset (getAssetPath (name));
			}
			return r;
		}

		/// <summary>
		/// Borrows the texture asyn.
		/// 异步取得texture
		/// </summary>
		/// <returns>The texture asyn.</returns>
		/// <param name="path">Path.</param>
		/// <param name="onGetTexture">On get texture.</param>
		/// 回调函数
		/// <param name="org">Org.</param>
		/// 透传参数
		public virtual void _borrowObjAsyn (string name, object onGetCallbak)
		{
			_borrowObjAsyn (name, onGetCallbak, null, null);
		}

		public virtual void _borrowObjAsyn (string name, object onGetCallbak, object orgs)
		{
			_borrowObjAsyn (name, onGetCallbak, orgs, null);
		}

		public virtual void _borrowObjAsyn (string name, object onGetCallbak, object orgs, object progressCB)
		{
			if (string.IsNullOrEmpty (name)) {
				Debug.LogWarning ("The name is null");
				return;
			}
			if (_havePrefab (name)) {
                CLMainBase.self.StartCoroutine(_doBorrowObj(name, onGetCallbak, orgs));
			} else {
				OnSetPrefabCallbacks4Borrow.add (name, onGetCallbak, orgs);
				_setPrefab (name, (Callback)onFinishSetPrefab4Borrow, name, progressCB);
			}
		}

        IEnumerator _doBorrowObj(string name, object onGetCallbak, object orgs)
        {
            yield return null;
            T unit = _borrowObj(name);
            Utl.doCallback(onGetCallbak, name, unit, orgs);
        }

        public virtual void onFinishSetPrefab4Borrow (object[] paras)
		{
			if (paras != null && paras.Length > 1) {
				T unit = paras [0] as T; 
				string name = paras [1].ToString ();
				ArrayList list = OnSetPrefabCallbacks4Borrow.getDelegates (name);
				int count = list.Count;
				ArrayList cell = null;
				object cb = null;
				object orgs = null;
				for (int i = 0; i < count; i++) {
					cell = list [i] as ArrayList;
					if (cell != null && cell.Count > 1) {
						cb = cell [0];
						orgs = cell [1];
						if (cb != null) {
							unit = _borrowObj (name);
							Utl.doCallback (cb, name, unit, orgs);
						}
					}
				}
				list.Clear ();
				OnSetPrefabCallbacks4Borrow.removeDelegates (name);
			}
		}

		/// <summary>
		/// Returns the object. 当参数unit是null时，说明是共享资源
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="unit">Unit.</param>
		public virtual void _returnObj (string name, T unit, bool inActive = true, bool setParent = true)
		{
			if (unit != null) {
				object obj = poolMap [name];
				ObjsPubPool pool = getObjPool (name);
				pool.returnObject (unit);
				poolMap [name] = pool;
				if (unit is MonoBehaviour) {
					MonoBehaviour unitObj = unit as MonoBehaviour;
					if (inActive) {
						unitObj.gameObject.SetActive (false);
					}
					if (setParent) {
						unitObj.transform.parent = null;
					}
				} else if (unit is GameObject) {
					GameObject unitObj = unit as GameObject;
					if (inActive) {
						unitObj.SetActive (false);
					}
					if (setParent) {
						unitObj.transform.parent = null;
					}
				}
			}
			CLAssetsManager.self.unUseAsset (getAssetPath (name));
		}

		public class ObjsPubPool : AbstractObjectPool<T>
		{
			Hashtable prefabMap = null;

			public ObjsPubPool (Hashtable prefabMap)
			{
				this.prefabMap = prefabMap;
			}

			public override T createObject (string key)
			{
				T unit = (prefabMap [key]) as T;
				if (unit != null) {
					Object go = GameObject.Instantiate (unit) as Object;
					go.name = key;

					T ret = null;
					if (go is T) {
						ret = go as T;
					} else {
						ret = ((GameObject)go).GetComponent<T> ();
					}
					return ret;
				}
				return null;
			}

			public override T resetObject (T t)
			{
				return t;
			}
		}
	}
}
