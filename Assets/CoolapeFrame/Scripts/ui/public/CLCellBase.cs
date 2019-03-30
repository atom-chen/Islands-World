/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   ui列表元素
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;

namespace Coolape
{
	public abstract class CLCellBase : CLBehaviour4Lua
	{
		public abstract void init(object data, object onClick);
	}
}
