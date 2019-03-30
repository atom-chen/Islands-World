/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   ui的其它物件对象池，比如血条
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class CLUIOtherObjPool:CLAssetsPoolBase<GameObject>
	{
		public static CLUIOtherObjPool pool = new CLUIOtherObjPool ();

		public override string getAssetPath (string name)
		{
			string path = PStr.b ().a (CLPathCfg.self.basePath).a ("/")
				.a (CLPathCfg.upgradeRes).a ("/priority/ui/other").e ();
			return wrapPath (path, name);
		}

		public override void _returnObj (string name, GameObject unit, bool inActive = true, bool setParent = true)
		{
			base._returnObj (name, unit, inActive, setParent);
			if (unit != null && setParent) {
				NGUITools.MarkParentAsChanged (unit);
			}
		}

        public override bool isAutoReleaseAssetBundle
        {
            get
            {
                return base.isAutoReleaseAssetBundle;
            }
        }

		public static void clean ()
		{
			pool._clean ();
		}

		public static bool havePrefab (string name)
		{
			return pool._havePrefab (name);
		}

		public static void setPrefab (string name, object finishCallback, object args)
		{
			pool._setPrefab (name, finishCallback, args, null);
		}

		public static void setPrefab (string name, object finishCallback, object args, object progressCB)
		{
			pool._setPrefab (name, finishCallback, args, progressCB);
		}

		public override GameObject _borrowObj (string name)
		{
			GameObject go = base._borrowObj (name);
			if (go != null) {
				CLUIUtl.resetAtlasAndFont (go.transform, false);
			}
			return go;
		}

		public static GameObject borrowObj (string name)
		{
			return pool._borrowObj (name);
		}

		public static void borrowObjAsyn (string name, object onGetCallbak)
		{
			borrowObjAsyn (name, onGetCallbak, null);
		}

		public static void borrowObjAsyn (string name, object onGetCallbak, object orgs)
		{
			pool._borrowObjAsyn (name, onGetCallbak, orgs, null);
		}

		public static void borrowObjAsyn (string name, object onGetCallbak, object orgs, object progressCB)
		{
			pool._borrowObjAsyn (name, onGetCallbak, orgs, progressCB);
		}

		public static void returnObj (string name, GameObject go)
		{
			pool._returnObj (name, go);
		}

		public static void returnObj (GameObject go)
		{
			pool._returnObj (go.name, go);
		}

		public static void returnObj (string name, GameObject go, bool inActive, bool setParent)
		{
			pool._returnObj (name, go, inActive, setParent);
		}
	}
}
