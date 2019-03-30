/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  为了解决lua中map的key为int时传给服务器无法取值的问题
  *Others:  
  *History:
*********************************************************************************
*/ 
using System;
using System.Collections;

namespace Coolape
{
	public class B2Int
	{
		public int value = 0;
		//	public B2Int() {
		//		value = 0;
		//	}
		public B2Int (int v)
		{
			value = v;
		}
	}
}