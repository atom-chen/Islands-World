/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  Thread工具类型
  *Others:  
  *History:
*********************************************************************************
*/ 

using System;
using System.Collections;
using System.Threading;

namespace Coolape
{
	public class ThreadEx
	{
		public static void exec(ThreadStart fn)
		{
			Thread t = new Thread(fn);
			t.Start();
		}

		public static void exec(ParameterizedThreadStart fn, object obj)
		{
			Thread t = new Thread(fn);
			t.Start(obj);
		}

		public static void exec2(System.Threading.WaitCallback fn)
		{
			ThreadPool.QueueUserWorkItem(fn);
		}

		public static void exec2(System.Threading.WaitCallback fn, object x)
		{
			ThreadPool.QueueUserWorkItem(fn, x);
		}
	}
}
