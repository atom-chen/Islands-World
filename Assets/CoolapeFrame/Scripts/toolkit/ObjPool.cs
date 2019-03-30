/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  对象池基类
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Coolape
{
	/// ///////////////////////////////////////////////////////////////////////////////////////////////////
	public abstract class AbstractObjectPool<T>
	{
		public Queue<T> queue = new Queue<T>();

		public abstract T createObject(string key = null);

		public abstract T resetObject(T t);

		protected int max;

		public T borrowObject(string key = null)
		{
			if (queue.Count > 0)
				return queue.Dequeue();
		
			return createObject(key);
		}

		public void returnObject(T obj)
		{
			if (max > 0 && queue.Count > max)
				return;
		
			if (obj is T) {
				obj = resetObject(obj);
				queue.Enqueue(obj);
			}
		}

		public bool typeTrue(object obj)
		{
			return obj is T;
		}

		public void Clear()
		{
			queue.Clear();
		}

		public int Count()
		{
			return queue.Count;
		}
	}

	/// ///////////////////////////////////////////////////////////////////////////////////////////////////

	public class StringPool : AbstractObjectPool<StringBuilder>
	{
		public override StringBuilder createObject(string name = null)
		{
			return new StringBuilder();
		}

		public override StringBuilder resetObject(StringBuilder t)
		{
			t.Remove(0, t.Length);
			return t;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////

	public class MapPool : AbstractObjectPool<NewMap>
	{
		public override NewMap createObject(string name = null)
		{
			return new NewMap();
		}

		public override NewMap resetObject(NewMap t)
		{
			t.Clear();
			return t;
		}
	}

	/// ///////////////////////////////////////////////////////////////////////////////////////////////////

	public class ListPool : AbstractObjectPool<NewList>
	{
		public override NewList createObject(string name = null)
		{
			return new NewList();
		}

		public override NewList resetObject(NewList t)
		{
			t.Clear();
			return t;
		}
	}

	/// ///////////////////////////////////////////////////////////////////////////////////////////////////

	public class SetPool : AbstractObjectPool<NewSet>
	{
		public override NewSet createObject(string name = null)
		{
			return new NewSet();
		}

		public override NewSet resetObject(NewSet t)
		{
			t.Clear();
			return t;
		}
	}

	/// ///////////////////////////////////////////////////////////////////////////////////////////////////

	public class MemoryStreamPool : AbstractObjectPool<MemoryStream>
	{
		public override MemoryStream createObject(string name = null)
		{
			MemoryStream ret = new MemoryStream();
			return ret;
		}

		public override MemoryStream resetObject(MemoryStream t)
		{
			t.Position = 0;
			t.SetLength(0);
			return t;
		}
	}


	/// ///////////////////////////////////////////////////////////////////////////////////////////////////
	public static class ObjPool
	{
		public static StringPool strs = new StringPool();
		public static MapPool maps = new MapPool();
		public static SetPool sets = new SetPool();
		public static ListPool listPool = new ListPool ();
		//public static MemoryStreamPool mems = new MemoryStreamPool ();
	}
}
