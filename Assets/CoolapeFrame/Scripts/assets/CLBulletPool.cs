/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   子弹对象池
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Coolape
{
	public class CLBulletPool : CLAssetsPoolBase<CLBulletBase>
	{
		public static CLBulletPool pool = new CLBulletPool ();

		public override string getAssetPath (string name)
		{
			string path = PStr.b ().a (CLPathCfg.self.basePath).a ("/")
				.a (CLPathCfg.upgradeRes).a ("/other/bullet").e ();
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

		public static void setPrefab (string name, object finishCallback)
		{
			setPrefab (name, finishCallback, null, null);
		}

		public static void setPrefab (string name, object finishCallback, object orgs)
		{
			pool._setPrefab (name, finishCallback, orgs, null);
		}

		public static void setPrefab (string name, object finishCallback, object orgs, object progressCB)
		{
			pool._setPrefab (name, finishCallback, orgs, progressCB);
		}

		public static CLBulletBase borrowObj (string name)
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

		public static void returnObj (CLBulletBase unit)
		{
			pool._returnObj (unit.name, unit);
		}
		public static void returnObj (CLBulletBase unit, bool inActive, bool setParent)
		{
			pool._returnObj (unit.name, unit, inActive, setParent);
		}
	}
}