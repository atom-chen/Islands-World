/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   管理assetsBundle的加载释放
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class CLAssetsManager : MonoBehaviour
	{
		public static CLAssetsManager self;
		public bool isPuase = false;
		// 暂停释放资源（比如战斗中，先不释放）

		#if UNITY_EDITOR
		public string debugKey = "";
		#endif

		public CLAssetsManager ()
		{
			self = this;
		}

		//未使用超过x时间就释放该资源(秒)
		public int timeOutSec4Realse = 300;

		public static int realseTime {
			get {
				return self.timeOutSec4Realse;
			}
			set {
				self.timeOutSec4Realse = value;
			}
		}

		public Hashtable assetsMap = System.Collections.Hashtable.Synchronized (new Hashtable ());

		public class AssetsInfor
		{
			public string key;
			public string name;
			public long lastUsedTime;
			public int usedCount;
			public AssetBundle asset;
			public Callback doRealse;
		}

		public void pause ()
		{
			isPuase = true;
		}

		public void regain ()
		{
			isPuase = false;
		}

		public void addAsset (string key, string name, AssetBundle asset, Callback onRealse)
		{
			AssetsInfor ai = new AssetsInfor ();
			ai.name = name;
			ai.lastUsedTime = System.DateTime.Now.ToFileTime ();
			ai.usedCount = 0;
			ai.asset = asset;
			ai.doRealse = onRealse;
			ai.key = key;
			assetsMap [key] = ai;
		}

		public void useAsset (string key)
		{
			AssetsInfor ai = (AssetsInfor)(assetsMap [key]);
			if (ai != null) {
				ai.usedCount++;
				ai.lastUsedTime = System.DateTime.Now.ToFileTime ();
				assetsMap [key] = ai;

				#if UNITY_EDITOR
				if (!string.IsNullOrEmpty(debugKey) && key.Contains(debugKey)) 
					Debug.LogWarning (ai.usedCount + "====useAsset===" + key);
				#endif
			}
		}

		public void unUseAsset (string key)
		{
			AssetsInfor ai = (AssetsInfor)(assetsMap [key]);
			if (ai != null) {
				ai.usedCount--;
				if (ai.usedCount < 0) {
					Debug.LogError ("["+ai.key+"] is use time less then zero!!");
					ai.usedCount = 0;
				}
				ai.lastUsedTime = System.DateTime.Now.ToFileTime ();
				assetsMap [key] = ai;

				#if UNITY_EDITOR
				if (!string.IsNullOrEmpty(debugKey) && key.Contains(debugKey)) 
					Debug.LogWarning (ai.usedCount + "===unUseAsset====" + key);
				#endif
			}
		}

		public object getAsset (string key)
		{
			AssetsInfor ai = (AssetsInfor)(assetsMap [key]);
			if (ai != null) {
				return ai.asset;
			}
			return null;
		}

		void Start ()
		{
			InvokeRepeating ("releaseAsset", 10, 6);	
		}

		void OnDestroy ()
		{
			CancelInvoke ();
		}

		public void releaseAsset ()
		{
			releaseAsset (false);
		}

		public void releaseAsset (bool isForceRelease)
		{
			try {
				if (isPuase && !isForceRelease) {
					return;
				}
				AssetsInfor ai = null;
				ArrayList list = new ArrayList ();
				list.AddRange (assetsMap.Values);
				bool isHadRealseAssets = false;
				for (int i = 0; i < list.Count; i++) {
					ai = (AssetsInfor)(list [i]);
					if (ai == null) {
						continue;
					}
					if (ai.usedCount <= 0 &&
					    ((System.DateTime.Now.ToFileTime () - ai.lastUsedTime) / 10000000 > realseTime || isForceRelease)) {
						if (ai.doRealse != null) {
							ai.doRealse (ai.name);
						}
						assetsMap.Remove (ai.key);
						if (ai.asset != null) {
							ai.asset.Unload (true);
							ai.asset = null;
						}
						ai = null;
						isHadRealseAssets = true;
					}
				}
//            UnityEngine.Resources.UnloadUnusedAssets();
				list.Clear ();
				list = null;

				if (isHadRealseAssets) {
					//这种情况是处理当图片已经destory了，材质球却未来得及destory的情况
					releaseAsset(isForceRelease);
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}
	}
}