/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  wangkaiyuan
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   拖动滑动一页，比如可以用在关卡地图页面，绑定lua 
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;
using XLua;

namespace Coolape
{
	[RequireComponent (typeof(CLCellLua))]
	public class UIDragPage4Lua : UIDragPageContents
	{
		public CLCellLua uiLua;
	
		bool isFinishInit = false;
		LuaFunction lfInit = null;
		LuaFunction lfrefresh = null;
		LuaFunction lfrefreshCurrent = null;

		public override void init (object obj, int index)
		{
			if (!isFinishInit) {
				isFinishInit = true;
				if (uiLua != null) {
					uiLua.setLua ();
					lfInit = uiLua.getLuaFunction ("init");
					lfrefresh = uiLua.getLuaFunction ("refresh");
					lfrefreshCurrent = uiLua.getLuaFunction ("refreshCurrent");
				}
				if (lfInit != null) {
					lfInit.Call (uiLua);
				}
			}
		
			if (lfrefresh != null) {
				lfrefresh.Call (obj, index);
			}
		}

		public override void refreshCurrent (int pageIndex, object obj)
		{
			init (obj, pageIndex);
			if (lfrefreshCurrent != null) {
				lfrefreshCurrent.Call (pageIndex, obj);
			}
		}
	}
}
