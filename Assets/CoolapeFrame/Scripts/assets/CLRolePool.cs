/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   角色、怪物的对象池
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Coolape
{
	public class CLRolePool:CLAssetsPoolBase<CLUnit>
	{
		public static CLRolePool pool = new CLRolePool ();

		public override string getAssetPath (string name)
		{
			string path = PStr.b ().a (CLPathCfg.self.basePath).a ("/")
				.a (CLPathCfg.upgradeRes).a ("/other/roles").e ();
			return wrapPath (path, name);
		}

		public override void _returnObj (string name, CLUnit unit, bool inActive = true, bool setParent = true)
		{
			base._returnObj (name, unit, inActive, setParent);
			unit.clean ();
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
			return pool.prefabMap.Contains (name);
		}

		public static bool isNeedDownload (string roleName)
		{
			#if UNITY_EDITOR
			if (CLCfgBase.self.isEditMode) {
				return false;
			}
			#endif

			string path = PStr.b ().a (CLPathCfg.self.basePath).a ("/")
				.a (CLPathCfg.upgradeRes).a ("/other/roles").e ();
			path = wrapPath (path, roleName);
			return CLVerManager.self.checkNeedDownload (path);
		}

		public static void setPrefab (string name, object finishCallback)
		{
			pool._setPrefab (name, finishCallback, null, null);
		}

		public static void setPrefab (string name, object finishCallback, object args)
		{
			pool._setPrefab (name, finishCallback, args, null);
		}

		public static void setPrefab (string name, object finishCallback, object args, object progressCB)
		{
			pool._setPrefab (name, finishCallback, args, progressCB);
		}

		public static CLUnit borrowObj (string name)
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

		public static void returnObj (CLUnit unit)
		{
			pool._returnObj (unit.name, unit);
		}

		public static void returnObj (string name, CLUnit unit)
		{
			pool._returnObj (name, unit);
		}
		public static void returnObj (string name, CLUnit unit, bool inActive, bool setParent)
		{
			pool._returnObj (name, unit, inActive, setParent);
		}
	}
}
