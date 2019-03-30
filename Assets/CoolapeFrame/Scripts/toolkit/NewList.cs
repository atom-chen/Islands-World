/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  List 安全包装
  *Others:  
  *History:
*********************************************************************************
*/ 

using System.Collections;
using System;
using UnityEngine;

namespace Coolape
{
	public class NewList : ArrayList
	{

		public static NewList create()
		{
			return new NewList();
		}

		public static NewList create(params object[] args)
		{
			NewList ret = new NewList();
			return ret.add(args);
		}

		public static NewList create(ArrayList list)
		{
			if (list is NewList)
				return (NewList)list;
		
			NewList ret = new NewList();
			ret.AddRange(list);
			return ret;
		}

		public NewList add(object val)
		{
			lock (this) {
				this.Add(val);
				return this;
			}
		}

		public NewList add(params object[] args)
		{
			lock (this) {
				foreach (object o in args)
					this.Add(o);
				return this;
			}
		}

		public override bool Contains(object o)
		{
			lock (this) {
				return base.Contains(o);
			}
		}

		public static object getObject(ArrayList list, int i)
		{
			lock (list) {
				return list [i];
			}
		}

		public object getObject(int i)
		{
			lock (this) {
				return getObject(this, i);
			}
		}

		public bool getBool(int i)
		{
			lock (this) {
				object val = getObject(i);
				if (val == null)
					return false;
				if (val is bool)
					return (bool)val;

				return Convert.ToBoolean(val);
			}
		}

		public byte getByte(int i)
		{
			lock (this) {
				object val = getObject(i);
				if (val == null)
					return 0;
				if (val is byte)
					return (byte)val;

				return Convert.ToByte(val);
			}
		}

		public int getInt(int i)
		{
			lock (this) {
				object val = getObject(i);
				if (val == null)
					return 0;
				if (val is int)
					return (int)val;

				return Convert.ToInt32(val);
			}
		}

		public double getLong(int i)
		{
			lock (this) {
				object val = getObject(i);
				if (val == null)
					return 0;
				if (val is double)
					return (double)val;
				
				return Convert.ToInt64(val);
			}
		}

		public double getDouble(int i)
		{
			lock (this) {
				object val = getObject(i);
				if (val == null)
					return 0;
				if (val is double)
					return (double)val;
				
				return Convert.ToDouble(val);
			}
		}

		public string getString(int i)
		{
			lock (this) {
				object val = getObject(i);
				if (val == null)
					return "";
				if (val is double)
					return (string)val;

				return Convert.ToString(val);
			}
		}

		public ArrayList getList(int i)
		{
			lock (this) {
				object val = getObject(i);
				if (val == null)
					return new ArrayList();
				if (val is ArrayList)
					return (ArrayList)val;

				return null;
			}
		}

		public NewList getNewList(int i)
		{
			lock (this) {
				object val = getObject(i);
				if (val == null)
					return new NewList();
				if (val is NewList)
					return (NewList)val;

				return null;
			}
		}

		public Hashtable getMap(int i)
		{
			lock (this) {
				object val = getObject(i);
				if (val == null)
					return new Hashtable();
				if (val is Hashtable)
					return (Hashtable)val;

				return null;
			}
		}

		public NewMap getNewMap(int i)
		{
			lock (this) {
				object val = getObject(i);
				if (val == null)
					return new NewMap();
				if (val is NewMap)
					return (NewMap)val;

				return null;
			}
		}

		public int pageCount(int pageSize)
		{
			int page = Count / pageSize;
			page = Count == page * pageSize ? page : page + 1;
			return page;
		}

		public ArrayList getPage(int page, int pageSize)
		{
			lock (this) {
				int begin = (page * pageSize);
				if (begin > Count || begin < 0)
					return new NewList();
				return this.GetRange(begin, pageSize);
			}
		}
	}
}
