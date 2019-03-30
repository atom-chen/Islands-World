/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  hashtable 包装
  *Others:  
  *History:
*********************************************************************************
*/ 

using System.Collections;
using System;

namespace Coolape
{
	public class NewMap : Hashtable
	{
		public static NewMap create()
		{
			return new NewMap();
		}

		public static NewMap create(Hashtable map)
		{
			if (map is NewMap)
				return (NewMap)map;
		
			NewMap ret = new NewMap();
			foreach (System.Collections.DictionaryEntry d in map) {
				object key = d.Key;
				object val = d.Value;
				ret [key] = val;
			}
			return ret;
		}

		public static NewMap create(params object[] args)
		{
			NewMap map = create();
			return MapEx.putKvs(map, args);
		}

		public NewMap put(object key, object val)
		{
			lock (this) {
				this [key] = val;
				return this;
			}
		}

		public NewMap put(params object[] args)
		{
			lock (this) {
				MapEx.putKvs(this, args);
				return this;
			}
		}

		public NewMap putPut(object key, object val)
		{
			lock (this) {
				this [key] = val;
				return this;
			}
		}

		public static object getObject(Hashtable map, object key)
		{
			lock (map) {
				return map [key];
			}
		}

		public object getObject(object key)
		{
			lock (this) {
				return this [key];
			}
		}

		public bool getBool(object key)
		{
			lock (this) {
				try {
					if (!this.Contains(key))
						return false;

					object val = this [key];
					if (val == null)
						return false;
					if (val is bool)
						return (bool)val;
					else
						return Convert.ToBoolean(val);
				} catch {
					return false;
				}
			}
		}

		public byte getByte(object key)
		{
			lock (this) {
				try {
					if (!this.Contains(key))
						return 0;

					object val = this [key];
					if (val == null)
						return 0;

					if (val is byte)
						return (byte)val;
					else
						return Convert.ToByte(val);
				} catch {
					return 0;
				}
			}
		}

		public short getShort(object key)
		{
			lock (this) {
				try {
					if (!this.Contains(key))
						return 0;

					object val = this [key];
					if (val == null)
						return 0;
					
					if (val is short)
						return (short)val;
					else
						return Convert.ToInt16(val);
				} catch {
					return 0;
				}
			}
		}

		public int getInt(object key)
		{
			lock (this) {
				try {
					if (!this.Contains(key))
						return 0;

					object val = this [key];
					if (val == null)
						return 0;
					if (val is int)
						return (int)val;
					else
						return Convert.ToInt32(val);
				} catch {
					return 0;
				}
			}
		}

		public long getLong(object key)
		{
			lock (this) {
				try {
					if (!this.Contains(key))
						return 0;

					object val = this [key];
					if (val == null)
						return 0;
					if (val is long)
						return (long)val;
					else
						return Convert.ToInt64(val);
				} catch {
					return 0;
				}
			}
		}

		public float getFloat(object key)
		{
			lock (this) {
				try {
					if (!this.Contains(key))
						return 0;

					object val = this [key];
					if (val == null)
						return 0;
					if (val is float)
						return (float)val;
					else
						return (float)Convert.ToDouble(val);
				} catch {
					return 0;
				}
			}
		}

		public double getDouble(object key)
		{
			lock (this) {
				try {
					if (!this.Contains(key))
						return 0;

					object val = this [key];
					if (val == null)
						return 0;
					if (val is double)
						return (double)val;
					else
						return Convert.ToDouble(val);
				} catch {
					return 0;
				}
			}
		}

		public byte[] getBytes(object key)
		{
			lock (this) {
				try {
					if (!this.Contains(key))
						return null;

					object val = this [key];
					if (val == null)
						return null;
					if (val is byte[])
						return (byte[])val;
					else
						return null;
				} catch {
					return null;
				}
			}
		}

		public string getString(object key)
		{
			lock (this) {
				try {
					if (!this.Contains(key))
						return "";

					object val = this [key];
					if (val == null)
						return "";
					if (val is string)
						return (string)val;
					else
						return Convert.ToString(val);
				} catch {
					return "";
				}
			}
		}

		public ArrayList getList(object key)
		{
			lock (this) {
				try {
					if (!this.Contains(key))
						return new ArrayList();

					object val = this [key];
					if (val == null)
						return null;
					if (val is ArrayList)
						return (ArrayList)val;

					return null;
				} catch {
					return new ArrayList();
				}
			}
		}

		public NewList getNewList(object key)
		{
			lock (this) {
				try {
					if (!this.Contains(key))
						return new NewList();

					object val = this [key];
					if (val == null)
						return null;
					//if (val is NewMap)
					return (NewList)val;
				} catch {
					return new NewList();
				}
			}
		}

		public Hashtable getMap(object key)
		{
			lock (this) {
				try {
					if (!this.Contains(key))
						return new Hashtable();

					object val = this [key];
					if (val == null)
						return null;
					if (val is Hashtable)
						return (Hashtable)val;

					return null;
				} catch {
					return new Hashtable();
				}
			}
		}

		public NewMap getNewMap(object key)
		{
			lock (this) {
				try {
					if (!this.Contains(key))
						return new NewMap();

					object val = this [key];
					if (val == null)
						return null;
					if (val is NewMap)
						return (NewMap)val;

					return null;
				} catch {
					return new NewMap();
				}
			}
		}

		public NewSet getNewSet(object key)
		{
			lock (this) {
				try {
					if (!this.Contains(key))
						return new NewSet();

					object val = this [key];
					if (val == null)
						return null;
					if (val is NewSet)
						return (NewSet)val;

					return null;
				} catch {
					return new NewSet();
				}
			}
		}

	}
}