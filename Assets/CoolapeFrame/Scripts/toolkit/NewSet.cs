/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  hashSet 包装
  *Others:  
  *History:
*********************************************************************************
*/ 

using System.Collections;
using System;

namespace Coolape
{
	public class NewSet
	{
	
		private Hashtable data = new Hashtable();

		public static NewSet create()
		{
			NewSet ret = new NewSet();
			return ret;
		}

		public static NewSet create(params object[] vars)
		{
			NewSet ret = new NewSet();
			foreach (object v in vars) {
				ret.Add(v);
			}
			return ret;
		}

		public bool IsEmpty()
		{
			return data == null || data.Count <= 0;
		}

		public NewSet Add(object o)
		{
			lock (data) {
				if (!data.Contains(o))
					data [o] = 1;
		
				return this;
			}
		}

		public bool Contains(object o)
		{
			lock (data) {
				return data.Contains(o);
			}
		}

		public NewSet addAll(ICollection objs)
		{
			lock (data) {
				foreach (object o in objs)
					data [o] = 1;
		
				return this;
			}
		}

		public NewSet addAll(ArrayList objs)
		{
			lock (data) {
				foreach (object o in objs) {
					if (!data.Contains(o))
						data.Add(o, 1);
				}
				return this;
			}
		}

		public ICollection Values {
			get {
				return data.Keys;
			}
		}

		public NewSet Remove(object key)
		{
			lock (data) {
				data.Remove(key);
				return this;
			}
		}

		public int Count()
		{
			return data.Count;
		}

		public void Clear()
		{
			lock (data) {
				data.Clear();
			}
		}
	}
}
