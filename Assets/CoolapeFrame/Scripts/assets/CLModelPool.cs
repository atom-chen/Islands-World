using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coolape
{
	public class CLModePool : CLAssetsPoolBase<GameObject>
	{
		public static CLModePool pool = new CLModePool ();

		public override string getAssetPath (string name)
		{
			string path = PStr.b ().a (CLPathCfg.self.basePath).a ("/")
				.a (CLPathCfg.upgradeRes).a ("/other/model").e ();
			return wrapPath (path, name);
		}

		public override void _returnObj (string name, GameObject unit, bool inActive = true, bool setParent = true)
		{
			base._returnObj (name, unit, inActive, setParent);
		}

        public override bool isAutoReleaseAssetBundle
        {
            get
            {
                return false;
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

		public static bool isNeedDownload (string name)
		{
			#if UNITY_EDITOR
			if (CLCfgBase.self.isEditMode) {
				return false;
			}
			#endif

			string path = pool.getAssetPath (name);

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

		public override GameObject _borrowObj (string name)
		{
			return base._borrowObj (name, true);
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

		public static void returnObj (string name)
		{
			pool._returnObj (name, null);
		}
	}
}
