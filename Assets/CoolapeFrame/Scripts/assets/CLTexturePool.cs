/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   图片对象池
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class CLTexturePool : CLAssetsPoolBase<Texture>
	{
		public static CLTexturePool pool = new CLTexturePool ();


		public override string getAssetPath (string name)
		{
			string path = PStr.b ().a (CLPathCfg.self.basePath).a ("/")
				.a (CLPathCfg.upgradeRes).a ("/other/Textures").e ();
			return wrapPath (path, name);
		}

		public override Texture _borrowObj (string name)
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

        public static bool havePrefab (string path)
		{
			return pool._havePrefab (path);
		}

		public static void clean ()
		{
			pool._clean ();
		}

		public static void setPrefab (string path, object finishCallback)
		{
			setPrefab (path, finishCallback, null, null);
		}

		public static void setPrefab (string path, object finishCallback, object args)
		{
			pool._setPrefab (path, finishCallback, args, null);
		}

		public static void setPrefab (string path, object finishCallback, object args, object progressCB)
		{
			pool._setPrefab (path, finishCallback, args, progressCB);
		}

		public static Texture borrowObj (string path)
		{
			return pool._borrowObj (path, true);
		}

		public static void borrowObjAsyn (string path, object onGetCallbak)
		{
			borrowObjAsyn (path, onGetCallbak, null);
		}

		public static void borrowObjAsyn (string path, object onGetCallbak, object orgs)
		{
			pool._borrowObjAsyn (path, onGetCallbak, orgs, null);
		}

		public static void borrowObjAsyn (string path, object onGetCallbak, object orgs, object progressCB)
		{
			pool._borrowObjAsyn (path, onGetCallbak, orgs, progressCB);
		}

		public static void returnObj (string path)
		{
			pool._returnObj (path, null);
		}
	}
}
