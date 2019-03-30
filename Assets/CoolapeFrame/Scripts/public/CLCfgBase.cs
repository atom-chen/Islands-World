/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  配置基础
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class CLCfgBase : MonoBehaviour
	{
		public static CLCfgBase self;

		public CLCfgBase()
		{
			self = this;
		}
		// 应用的唯一id，内部管理用，非渠道的appid
		public int appUniqueID = 0;
		public bool isContBorrowSpriteTimes = false;

		// 在编辑器下模拟真机
		public bool isNotEditorMode = false;

		// 是不事需要加密lua
		public bool isEncodeLua = true;
		//新手引导模式
		public bool isGuidMode = false;
		// 签名的md5码
		public string singinMd5Code = "";
		//每个服务器单独控制热更新
		public bool hotUpgrade4EachServer = false;

		//直接进入
		public bool isDirectEntry = false;

		//是否是在编辑器下
		public bool isEditMode {
			get {
				#if UNITY_EDITOR
				if (self.isNotEditorMode) {
					return false;
				} else {
					return true;
				}
				#else
				return false;
				#endif
			}
		}

        public bool isUnityEditor
        {
            get
            {
#if UNITY_EDITOR
                return true;
#endif 
                return false;
            }
        }
    }
}
