/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  定时器
  *Others:  
  *History:
*********************************************************************************
*/ 
using System;

namespace Coolape
{
	public class TimerEx
	{
		/// /////////////////////////////2/////////////////////////////////////////////////
		
		/// <summary>
		/// 定延时执行
		/// </summary>
		/// <param name="fn">执行函Number void fn(object o)</param>
		/// <param name="v">fn 的传入参Number</param>
		/// <param name="t">执行时间点</param>
		/// <returns></returns>
		public static System.Threading.Timer schedule(System.Threading.TimerCallback fn, object v, DateTime t)
		{
			DateTime now = DateTime.Now;
			int c = now.CompareTo(t);
			long t2 = 1;
			if (c <= 0) {
				TimeSpan ts = t.Subtract(now);
				double ms = ts.TotalMilliseconds;
				t2 = Convert.ToInt64(ms);
				t2 = t2 < 0 ? 2 : t2;
			}
			return schedule(fn, v, t2);
		}

		/// <summary>
		/// 定延时执行
		/// </summary>
		/// <param name="fn">执行函Number void fn(object o)</param>
		/// <param name="v">fn 的传入参Number</param>
		/// <param name="initialDelay">执行时间:从curr时间延后执行</param>
		/// <returns></returns>
		public static System.Threading.Timer schedule(System.Threading.TimerCallback fn, object v, long initialDelay)
		{
			System.Threading.Timer timer = null;
			System.Threading.TimerCallback tc = new System.Threading.TimerCallback((o) => {
				fn(v);
				timer.Dispose();
			});	
			timer = new System.Threading.Timer(tc, null, initialDelay, System.Threading.Timeout.Infinite);
			return timer;
		}

		public static System.Threading.Timer schedule(System.Threading.TimerCallback fn, object v, DateTime t, long delay)
		{
			DateTime now = DateTime.Now;
			int c = now.CompareTo(t);
			long t2 = 1;
			if (c <= 0) {
				TimeSpan ts = t.Subtract(now);
				double ms = ts.TotalMilliseconds;
				t2 = Convert.ToInt64(ms);
				t2 = t2 < 0 ? 2 : t2;
			}
			return schedule(fn, v, t2, delay);
		}

		/// <summary>
		/// 定时执行
		/// </summary>
		/// <param name="fn">执行函Number void fn(object o)</param>
		/// <param name="v">fn 的传入参Number</param>
		/// <param name="initialDelay">执行时间:从curr时间延后执行</param>
		/// <param name="delay">间隔执行时间</param>
		/// <returns></returns>
		public static System.Threading.Timer schedule(System.Threading.TimerCallback fn, object v, long initialDelay, long delay)
		{
			System.Threading.Timer timer = null;
			System.Threading.TimerCallback tc = new System.Threading.TimerCallback((o) => {
				fn(v);
			});				
			timer = new System.Threading.Timer(tc, null, initialDelay, delay);
			return timer;
		}

		/// <summary>
		/// 定时执行
		/// </summary>
		/// <param name="fn">执行函Number void fn(object o)</param>
		/// <param name="v">fn 的传入参Number</param>
		/// <param name="initialDelay">执行时间:从curr时间延后执行</param>
		/// <param name="delay">间隔执行时间</param>
		/// <param name="t">执行t次后停止</param>
		/// <returns></returns>
		public static System.Threading.Timer schedule(System.Threading.TimerCallback fn, object v, long initialDelay, long delay, int t)
		{
			System.Threading.Timer timer = null;
			System.Threading.TimerCallback tc = new System.Threading.TimerCallback((o) => {
				fn(v);
				if (--t <= 0) {
					timer.Dispose();
				}
			});
			timer = new System.Threading.Timer(tc, null, initialDelay, delay);
			return timer;
		}

		/// //////////////////////////////////////////////////////////////////////////////
		
		public static void sample()
		{
			schedule(func, "date time", DateTime.Now.Add(new TimeSpan(0, 0, 10)));
			//schedule(func, "initialDelay", 15 * 1000);
			//schedule(func, "data time, delay", DateTime.Now.Add(new TimeSpan(0,0,10)), 2000);
			//schedule(func, "initialDelay delay", 10000 , 3000);
			//schedule(func, "init delay, t", 1000, 1000, 10);
			         
		}

		public static void func(object o)
		{
			Console.WriteLine(o);
		}
	}
}
