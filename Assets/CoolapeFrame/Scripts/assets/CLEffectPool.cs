/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   特效对象池
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class CLEffectPool:CLAssetsPoolBase<CLEffect>
	{
		public static CLEffectPool pool = new CLEffectPool ();

		public override string getAssetPath (string name)
		{
			string path = PStr.b ().a (CLPathCfg.self.basePath).a ("/")
				.a (CLPathCfg.upgradeRes).a ("/other/effect").e ();
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

		public static void setPrefab (string name, object finishCallback, object args)
		{
			pool._setPrefab (name, finishCallback, args, null);
		}

		public static void setPrefab (string name, object finishCallback, object args, object progressCB)
		{
			pool._setPrefab (name, finishCallback, args, progressCB);
		}

		public static CLEffect borrowObj (string name)
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

		public static void returnObj (CLEffect effect)
		{
			if (effect == null)
				return;
			pool._returnObj (effect.name, effect);
		}

		public static void returnObj (string name, CLEffect effect)
		{
			if (effect == null)
				return;
			pool._returnObj (name, effect);
		}

		public static void returnObj (string name, CLEffect effect, bool inActive, bool setParent)
		{
			if (effect == null)
				return;
			pool._returnObj (name, effect, inActive, setParent);
		}
	}
}
