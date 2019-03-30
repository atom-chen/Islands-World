/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   其它物件对象池
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class CLThings4LuaPool:CLAssetsPoolBase<CLBaseLua>
	{
		public static CLThings4LuaPool pool = new CLThings4LuaPool ();

		public override string getAssetPath (string name)
		{
			string path = PStr.b ().a (CLPathCfg.self.basePath).a ("/")
				.a (CLPathCfg.upgradeRes).a ("/other/things").e ();
			return wrapPath (path, name);
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

		public static CLBaseLua borrowObj (string name)
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

		public static void returnObj (string name, CLBaseLua go)
		{
			pool._returnObj (name, go);			
		}
		public static void returnObj (CLBaseLua go)
		{
			pool._returnObj (go.name, go);			
		}
		public static void returnObj (string name, CLBaseLua go, bool inActive, bool setParent)
		{
			pool._returnObj (name, go, inActive, setParent);
		}
	}
}
