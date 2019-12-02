/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  程序入口的基类
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;
using System.IO;
using XLua;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Coolape
{
	public class CLMainBase : CLBehaviourWithUpdate4Lua
	{
		public static CLMainBase self;
		//第一个页面，可以理解为显示公司LOGO的页面
		public string firstPanel = "";

		public CLMainBase ()
		{
			self = this;
		}

		// Use this for initialization
		public override void Start ()
		{
			DateEx.init ();
			// 显示公司logo页面
			CLPanelBase panel = CLPanelManager.getPanel (firstPanel);
			try {
				CLPanelManager.showPanel (panel);
				//		SoundEx.playSound ("Coolape", 1);
				// 初始化
				StartCoroutine (gameInit ());
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public IEnumerator gameInit ()
		{
            yield return new WaitForSeconds(2);
			init ();
		}

		public virtual void setLanguage(string language) {
			string languageFile = PStr.b (
				CLPathCfg.self.localizationPath, 
				language, ".txt").e ();
			byte[] buff = null;
			#if UNITY_EDITOR
			if (CLCfgBase.self.isEditMode) {
				languageFile = PStr.b().a(CLPathCfg.persistentDataPath).a("/").a(languageFile).e();
				languageFile = languageFile.Replace ("/upgradeRes/", "/upgradeRes4Dev/");
				buff = File.ReadAllBytes(languageFile);
			} else {
				buff = FileEx.readNewAllBytes(languageFile);
			}
			#else
			buff = FileEx.readNewAllBytes(languageFile);
			#endif
			Localization.Load (language, buff);
		}

		public virtual void init ()
		{
			//防止锁屏
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			//设置帧率
			Application.targetFrameRate = 30;

			//设置初始语言,Resource目录下的语言文件只需要包括刚开始加载页面需要的文字即可，
			//等更处理完成后，将会再次设置一次语言
			Localization.language = "Chinese";		//TODO: 根据保存的语文来设置

			// set fps, only none channel can show fps
//			if (CLCfgBase.Channel == CLCfgBase.ChlType.NONE
//			    || CLCfgBase.Channel == CLCfgBase.ChlType.IOSDEMO) {
//				SCfg.self.fps.enabled = true;
//			} else {
//				SCfg.self.fps.enabled = false;
//			}
			//初始化streamingAssetsPackge,把包里的资源释放出来

			CLVerManager.self.initStreamingAssetsPackge ((Callback)onGetStreamingAssets);
		}

		// 当完成把数据从包里释放出来
		public virtual void onGetStreamingAssets (params object[] para)
		{
			//先加载一次atlas，以便可以显示ui
			CLUIInit.self.init ();

			//再次加载语言
			setLanguage(Localization.language);

			//设置lua
			setLua ();
		}

		public virtual void reStart ()
		{
			CancelInvoke ();
			StopAllCoroutines ();
			Invoke ("doRestart", 0.5f); // 必须得用个invoke，否则unity会因为lua.desotry而闪退
		}

		public virtual void doRestart ()
		{
			CLPanelManager.destoryAllPanel ();
			FileEx.cleanCache ();
			CLUtlLua.cleanFileBytesCacheMap ();
			//CLUtlLua.isFinishAddLoader = false;
			CLUIInit.self.clean ();
			//重新把配置清空
			CLMaterialPool.materialTexRefCfg = null;
			if (mainLua != null) {
				destoryLua ();
//				mainLua.Dispose ();
//				mainLua = null;
				resetMainLua();
			} else {
				resetMainLua();
			}
			luaTable = null;
			lua = null;
			Start ();
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
//			if (mainLua != null) {
//				mainLua.Dispose ();
//				mainLua = null;
//			}
		}

		public  override void OnApplicationQuit ()
		{
			base.OnApplicationQuit ();

			#if UNITY_EDITOR
			string atlasPath = PStr.b ().a ("Assets/").a (CLPathCfg.self.basePath).a ("/upgradeRes4Dev/priority/atlas/atlasAllReal.prefab").e ();
			CLUIInit.self.emptAtlas.replacement = (UIAtlas)(AssetDatabase.LoadAssetAtPath (atlasPath, typeof(UIAtlas)));
			#endif
		}

		public override void setLua ()
		{
			//set lua
			if (lua == null || CLVerManager.self.haveUpgrade) {
				CLPanelManager.destoryAllPanel ();
				if (lua == null) {
					lua = mainLua;
					base.setLua ();
				}
			}
		}

		LuaFunction lfexitGmaeConfirm = null;

		public override void initGetLuaFunc ()
		{
			base.initGetLuaFunc ();
			lfexitGmaeConfirm = getLuaFunction ("exitGmaeConfirm");
		}

		// Update is called once per frame
		public override void Update ()
		{
			base.Update ();
			if (Input.GetKeyUp (KeyCode.Escape)) {
				//点了返回键
				if (lfexitGmaeConfirm != null) {
					call (lfexitGmaeConfirm);
				}
			}
			// proc net offline
			if (isOffLine) {
				//断线通知
				isOffLine = false;
				doOffline ();
			}
		}

		bool isOffLine = false;
		//off line
		public void onOffline ()
		{
			isOffLine = true;
		}

		public virtual void doOffline ()
		{
			if (lua == null) {
				return;
			}
			LuaFunction f = getLuaFunction ("onOffline");
			if (f != null) {
				call (f);
			}
		}
	}
}
