/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   音乐、音效对象池
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class CLSoundPool : CLAssetsPoolBase<AudioClip>
	{
		public static CLSoundPool pool = new CLSoundPool ();

		public override string getAssetPath (string name)
		{
			string path = PStr.b ().a (CLPathCfg.self.basePath).a ("/")
				.a (CLPathCfg.upgradeRes).a ("/other/sound").e ();
			return wrapPath (path, name);
		}
		public override AudioClip _borrowObj (string name)
		{
			return base._borrowObj (name, true);
		}

        public override bool isAutoReleaseAssetBundle
        {
            get
            {
                return false;
            }
        }

		public static bool havePrefab (string name)
		{
			return pool._havePrefab (name);
		}

		public static void clean ()
		{
			pool._clean ();
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

		public static AudioClip borrowObj (string name)
		{
			return pool._borrowObj (name, true);
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

		public static void returnObj (string name)
		{
			pool._returnObj (name, null);
		}
	}
}
