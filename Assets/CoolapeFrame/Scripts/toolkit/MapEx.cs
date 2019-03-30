/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  Map工具
  *Others:  
  *History:
*********************************************************************************
*/ 
using System.Collections;
using System;
using XLua;

namespace Coolape
{
	public class MapEx
	{
		public Hashtable map = null;

		public MapEx ()
		{
			this.map = new Hashtable ();
		}

		public MapEx (Hashtable map)
		{
			this.map = map;
		}

		public static MapEx builder ()
		{
			return new MapEx ();
		}

		public MapEx Add (object key, object value)
		{
			map.Add (key, value);
			return this;
		}

		public int Count ()
		{
			return map.Count;
		}

		public MapEx Clear ()
		{
			map.Clear ();
			return this;
		}

		public MapEx Set (object key, object value)
		{
			map [key] = value;
			return this;
		}

		public Hashtable ToMap ()
		{
			return map;
		}

		public ICollection Keys ()
		{
			return map.Keys;
		}

		public ArrayList KeysList ()
		{
			ArrayList list = new ArrayList ();
			list.AddRange (map.Keys);
			return list;
		}

		public ICollection Values ()
		{
			return map.Values;
		}

		public ArrayList ValuesList ()
		{
			ArrayList list = new ArrayList ();
			list.AddRange (map.Values);
			return list;
		}

		public bool ContainsKey (object key)
		{
			return map.ContainsKey (key);
		}

		public bool ContainsValue (object value)
		{
			return map.ContainsValue (value);
		}

		public object get (object key)
		{
			return getObject (map, key);
		}

		public bool getBool (object key)
		{
			return getBool (map, key);
		}

		public byte getByte (object key)
		{
			return getByte (map, key);
		}

		public int getInt (object key)
		{
			return getInt (map, key);
		}

		public long getLong (object key)
		{
			return getLong (map, key);
		}

		public double getDouble (object key)
		{
			return getDouble (map, key);
		}

		public string getString (object key)
		{
			return getString (map, key);
		}

		public ArrayList getList (object key)
		{
			return getList (map, key);
		}

		public Hashtable getMap (object key)
		{
			return getMap (map, key);
		}

		public static object get (Hashtable map, object key)
		{
			return getObject (map, key);
		}

		public static void set (Hashtable map, object key, object value)
		{
			if (map == null)
				return;
			map [key] = value;
		}

		public static object getObject (Hashtable map, object key)
		{
			if (map == null)
				return null;
			return map [key];
		}

		public static bool getBool (object map, object key)
		{
			if (map == null)
				return false;
			if (map is Hashtable) {
				return _getBool ((Hashtable)map, key);
			} else if (map.GetType () == typeof(LuaTable)) {
				return _getBool ((LuaTable)map, key);
			}
			return false;
		}

		static bool _getBool (LuaTable map, object key)
		{
			try {
				if (map == null)
					return false;
				object val = map [key];
				if (val == null)
					return false;
				
				if (val is bool)
					return (bool)val;
				else
					return Convert.ToBoolean (val);
			} catch {
				return false;
			}
		}

		static bool _getBool (Hashtable map, object key)
		{
			try {
				if (map == null)
					return false;
				if (!map.Contains (key))
					return false;
				
				object val = map [key];
				if (val == null)
					return false;
				
				if (val is bool)
					return (bool)val;
				else
					return Convert.ToBoolean (val);
			} catch {
				return false;
			}
		}

		public static byte getByte (Hashtable map, object key)
		{
			try {
				if (map == null)
					return 0;
				if (!map.Contains (key))
					return 0;

				object val = map [key];
				if (val == null)
					return 0;

				if (val is byte)
					return (byte)val;
				else
					return Convert.ToByte (val);
			} catch {
				return 0;
			}
		}

		public static byte[] getBytes (object map, object key)
		{
			if (map == null)
				return null;
			if (map is Hashtable) {
				return _getBytes ((Hashtable)map, key);
			} else if (map is LuaTable) {
				return _getBytes ((LuaTable)map, key);
			}
			return null;
		}

		static byte[] _getBytes (LuaTable map, object key)
		{
			try {
				if (map == null)
					return null;
				object val = map [key];
				if (val == null)
					return null;
				return map.GetInPath<byte[]> (key.ToString ());
//				return (byte[])val;
//				if (val is Byte[])
//					return (byte[])val;
//				else
//					return null;
			} catch {
				return null;
			}
		}

		static byte[] _getBytes (Hashtable map, object key)
		{
			try {
				if (map == null)
					return null;
				if (!map.Contains (key))
					return null;
				
				object val = map [key];
				if (val == null)
					return null;
				
				if (val is Byte[])
					return (byte[])val;
				else
					return null;
			} catch {
				return null;
			}
		}

		public static int getBytes2Int (object map, object key)
		{
			if (map == null || key == null)
				return 0;
			return NumEx.bio2Int (getBytes (map, key));
		}

		public static void setInt2Bytes (object map, object key, int val)
		{
			if (map == null || key == null)
				return;
			if (map is LuaTable) {
				((LuaTable)map) [key] = NumEx.int2Bio (val);
			} else {
				((Hashtable)map) [key] = NumEx.int2Bio (val);
			}
		}

		public static int getInt (object map, object key)
		{
			if (map == null || key == null)
				return 0;
			if (map is Hashtable) {
				return _getInt ((Hashtable)map, key);
			} else if (map.GetType () == typeof(LuaTable)) {
				return _getInt ((LuaTable)map, key);
			}
			return 0;
		}

		static int _getInt (LuaTable map, object key)
		{
			try {
				if (map == null || key == null)
					return 0;
				object val = map.GetInPath<object> (key.ToString ());// [key];
				if (val == null)
					return 0;
				
				if (val is int)
					return (int)val;
				else if (val is byte[])
					return getBytes2Int (map, key);
				else
					return Convert.ToInt32 (val);
			} catch {
				return 0;
			}
		}

		static int _getInt (Hashtable map, object key)
		{
			try {
				if (map == null)
					return 0;
				if (!map.Contains (key))
					return 0;

				object val = map [key];
				if (val == null)
					return 0;
				
				if (val is int)
					return (int)val;
				else if (val is byte[])
					return getBytes2Int (map, key);
				else
					return Convert.ToInt32 (val);
			} catch {
				return 0;
			}
		}

		public static long getLong (Hashtable map, object key)
		{
			try {
				if (map == null)
					return 0;
				if (!map.Contains (key))
					return 0;

				object val = map [key];
				if (val == null)
					return 0;

				if (val is long)
					return (long)val;
				else
					return Convert.ToInt64 (val);
			} catch {
				return 0;
			}
		}

		public static double getDouble (Hashtable map, object key)
		{
			try {
				if (map == null)
					return 0;
				if (!map.Contains (key))
					return 0;

				object val = map [key];
				if (val == null)
					return 0;

				if (val is double)
					return (double)val;
				else
					return Convert.ToDouble (val);
			} catch {
				return 0;
			}
		}

		public static string getString (object map, object key)
		{
			if (map == null)
				return "";

			if (map is Hashtable) {
				return _getString ((Hashtable)map, key);
			} else if (map.GetType () == typeof(LuaTable)) {
				return _getString ((LuaTable)map, key);
			}
			return "";
		}

		static string _getString (LuaTable map, object key)
		{
			try {
				if (map == null)
					return "";
				object val = map.GetInPath<object> (key.ToString ());
				if (val == null)
					return "";
				
				if (val is string)
					return (string)val;
				else
					return Convert.ToString (val);
			} catch {
				return "";
			}
		}

		static string _getString (Hashtable map, object key)
		{
			try {
				if (map == null)
					return "";
				if (!map.Contains (key))
					return "";

				object val = map [key];
				if (val == null)
					return "";

				if (val is string)
					return (string)val;
				else
					return Convert.ToString (val);
			} catch {
				return "";
			}
		}

		public static ArrayList getList (Hashtable map, object key)
		{
			try {
				if (map == null)
					return new ArrayList ();
				if (!map.Contains (key))
					return new ArrayList ();

				object val = map [key];
				if (val == null)
					return null;
				
				if (val is ArrayList)
					return (ArrayList)val;

				return null;
			} catch {
				return null;
			}
		}

		public static Hashtable getMap (Hashtable map, object key)
		{
			try {
				if (map == null)
					return new Hashtable ();
				if (!map.Contains (key))
					return new Hashtable ();

				object val = map [key];
				if (val == null)
					return null;

				if (val is Hashtable)
					return (Hashtable)val;

				return null;
			} catch {
				return null;
			}
		}

		public static void setIntKey (Hashtable map, int key, object val)
		{
			if (map == null) {
				return;
			}
			map [key] = val;
		}

		public static  object getByIntKey (Hashtable map, int key)
		{
			if (map == null)
				return null;
			return map [key];
		}

		public static Hashtable newMap ()
		{
			return new Hashtable ();
		}

		public static Hashtable ToMap (ArrayList list)
		{
			Hashtable map = new Hashtable ();
			int count = list.Count;
			for (int i = 0; i < count; i++)
				map [i] = list [i];
			return map;
		}

		public static Hashtable createKvs (params object[] kv)
		{
			Hashtable map = new Hashtable ();
			return putKvs (map);
		}

		public static Hashtable putKvs (Hashtable map, params object[] kv)
		{
			for (int n = 0; n < kv.Length; n++) {
				object key = kv [n];
				n++;
				if (n >= kv.Length)
					return map;
				
				object val = kv [n];
				map [key] = val;
			}
			return map;
		}

		public static NewMap putKvs (NewMap map, params object[] kv)
		{
			for (int n = 0; n < kv.Length; n++) {
				object key = kv [n];
				n++;
				if (n >= kv.Length)
					return map;
				
				object val = kv [n];
				map [key] = val;
			}
			return map;
		}

		// method map
		public static bool isNull (Hashtable map)
		{
			return (map == null);
		}

		public static bool isNullOrEmpty (Hashtable map)
		{
			bool r = isNull (map);
			if (!r)
				r = (map.Count <= 0);
			return r;
		}

		public static void clearMap (Hashtable map)
		{
			if (isNull (map))
				return;
			map.Clear ();
		}

		public static void clearNullMap (Hashtable map)
		{
			clearMap (map);
			map = null;
		}

		public static ArrayList keys2List (Hashtable map)
		{
			ArrayList list = new ArrayList ();
			if (isNullOrEmpty (map))
				return list;
			list.AddRange (map.Keys);
			return list;
		}

		public static ArrayList vals2List (Hashtable map)
		{
			ArrayList list = new ArrayList ();
			if (isNullOrEmpty (map))
				return list;

			list.AddRange (map.Values);
			return list;
		}

		public static Hashtable cloneMap (Hashtable old, Hashtable nwMap)
		{
			if (nwMap == null)
				nwMap = new Hashtable ();

			nwMap.Clear ();

			if (isNullOrEmpty (old))
				return nwMap;

			foreach (object key in old.Keys) {
				nwMap [key] = old [key];
			}
			return nwMap;
		}

		public static bool isHashtable (object obj)
		{
			if (obj.GetType () == typeof(Hashtable)) {
				return true;
			}
			return false;
		}
	}
}
