/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  路径配置
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Coolape
{
	public class CLPathCfg : MonoBehaviour
	{
		public static CLPathCfg self;

		public CLPathCfg ()
		{
			self = this;
		}

		static string _persistentDataPath = "";

		public static string persistentDataPath {
			get {
#if UNITY_EDITOR
				if (CLCfgBase.self.isEditMode) {
					return Application.dataPath;
				} else {
					if (_persistentDataPath == null || _persistentDataPath == "") {
						_persistentDataPath = Application.persistentDataPath;
					}
					return _persistentDataPath;
				}
#else
            if(_persistentDataPath == null || _persistentDataPath == "") {
                _persistentDataPath = Application.persistentDataPath;
				#if UNITY_ANDROID
                if(_persistentDataPath == null || _persistentDataPath == "") {
					AndroidJavaClass jc = new AndroidJavaClass("com.x3gu.tools.Tools4FilePath");
                    _persistentDataPath = jc.CallStatic<string>("getPath");
                }
				#endif
            }
            return _persistentDataPath;
#endif
			}
		}

		public string platform {
            get
            {
#if UNITY_IOS
				return "IOS";
#elif UNITY_ANDROID
                return "Android";
#elif UNITY_STANDALONE_WIN
                return "Standalone";
#elif UNITY_STANDALONE_OSX
                return "StandaloneOSX";
#else
                return "Standalone";
#endif
            }
        }

		public string runtimePlatform {
			get {
				switch (Application.platform)
				{
					case RuntimePlatform.WindowsPlayer:
						return "win|player";
					case RuntimePlatform.WindowsEditor:
						return "win|editor";
					case RuntimePlatform.Android:
						return "android|player";
					case RuntimePlatform.IPhonePlayer:
						return "ios|player";
					case RuntimePlatform.OSXEditor:
						return "osx|editor";
					case RuntimePlatform.OSXPlayer:
						return "osx|player";
					case RuntimePlatform.WebGLPlayer:
						return "web|player";
					case RuntimePlatform.LinuxPlayer:
						return "linux|player";
					case RuntimePlatform.LinuxEditor:
						return "linux|editor";
				}
				return "others";
			}
		}
		//
		//	[HideInInspector]
		public string basePath = "xxx";
		//实际的font路径
		//		public string realFontPath = "xxx/upgradeRes/priority/font/FontDynReal.unity3d";
		//页面数据存放路径
		public string panelDataPath = "xxx/upgradeRes/priority/ui/panel/";
		public string cellDataPath = "xxx/upgradeRes/priority/ui/cell/";

		public  string _cellDataPath {
			get {
				#if UNITY_EDITOR
				if (!CLCfgBase.self.isEditMode) {
					return cellDataPath;
				}
				return  cellDataPath.Replace ("/upgradeRes/", "/upgradeRes4Publish/");
				#else
            return cellDataPath;
				#endif
			}
		}

		//多语言文件路径
		public string localizationPath = "xxx/upgradeRes/priority/localization/";
		public string luaPathRoot = "xxx/upgradeRes/priority/lua";

		public static string upgradeRes {
			get {
#if UNITY_EDITOR
				if (!CLCfgBase.self.isEditMode) {
					return "upgradeRes";
				}
				return "upgradeRes4Publish";
#else
            return "upgradeRes";
#endif
			}
		}

		public void resetPath (string basePath)
		{
			this.basePath = basePath;
			//实际的font路径
//			realFontPath = basePath + "/upgradeRes/priority/font/FontDynReal.unity3d";
			//页面数据存放路径
			panelDataPath = basePath + "/upgradeRes/priority/ui/panel/";
			cellDataPath = basePath + "/upgradeRes/priority/ui/cell/";
			//多语言文件路径
			localizationPath = basePath + "/upgradeRes/priority/localization/";
		
			luaPathRoot = basePath + "/upgradeRes/priority/lua/";
		}
	}
}
