/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  List工具
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.Collections.Generic;

namespace Coolape
{
	public class ListEx
	{
		public ArrayList list = null;

		public ListEx ()
		{
			list = new ArrayList ();
		}

		public ListEx (ArrayList list)
		{
			this.list = list;
		}

		public static ListEx builder ()
		{
			return new ListEx ();
		}

		public ListEx Add (object o)
		{
			list.Add (o);
			return this;
		}

		public ArrayList ToList ()
		{
			return list;
		}

		public int Count ()
		{
			return list.Count;
		}

		public ListEx Clear ()
		{
			list.Clear ();
			return this;
		}

		public ListEx set (int index, object obj)
		{
			list [index] = obj;
			return this;
		}

		public object get (int index)
		{
			return list [index];
		}

		public bool getBool (int index)
		{
			try {
				object obj = list [index];
				if (obj == null)
					return false;
				if (obj is bool)
					return (bool)obj;
				return Convert.ToBoolean (obj);
			} catch {
				return false;
			}
		}

		public byte getByte (int index)
		{
			try {
				object obj = list [index];
				if (obj == null)
					return 0;
				if (obj is byte)
					return (byte)obj;
				return Convert.ToByte (obj);
			} catch {
				return 0;
			}
		}

		public int getInt (int index)
		{
			try {
				object obj = list [index];
				if (obj == null)
					return 0;
				if (obj is int)
					return (int)obj;
				return Convert.ToInt32 (obj);
			} catch {
				return 0;
			}
		}

		public long getLong (int index)
		{
			try {
				object obj = list [index];
				if (obj == null)
					return 0;
				if (obj is long)
					return (long)obj;
				return Convert.ToInt64 (obj);
			} catch {
				return 0;
			}
		}

		public double getDouble (int index)
		{
			try {
				object obj = (double)list [index];
				if (obj == null)
					return 0;
				if (obj is double)
					return (double)obj;
				return Convert.ToDouble (obj);
			} catch {
				return 0;
			}
		}

		public string getString (int index)
		{
			try {
				object obj = list [index];
				if (obj == null)
					return "";
				if (obj is string)
					return (string)obj;
				return Convert.ToString (obj);
			} catch {
				return "";
			}
		}

		public ArrayList getList (int index)
		{
			try {
				object obj = list [index];
				if (obj == null)
					return new ArrayList ();
				if (obj is ArrayList)
					return (ArrayList)obj;
				return new ArrayList ();
			} catch {
				return new ArrayList ();
			}
		}

		public Hashtable getMap (int index)
		{
			try {
				object obj = list [index];
				if (obj == null)
					return new Hashtable ();
				if (obj is Hashtable)
					return (Hashtable)obj;
				return new Hashtable ();
			} catch {
				return new Hashtable ();
			}
		}

		//
		public static object get (ArrayList list, int index)
		{
			return list [index];
		}

		public static bool getBool (ArrayList list, int index)
		{
			try {
				object obj = list [index];
				if (obj == null)
					return false;
				if (obj is bool)
					return (bool)obj;
				return Convert.ToBoolean (obj);
			} catch {
				return false;
			}
		}

		public static byte getByte (ArrayList list, int index)
		{
			try {
				object obj = list [index];
				if (obj == null)
					return 0;
				if (obj is byte)
					return (byte)obj;
				return Convert.ToByte (obj);
			} catch {
				return 0;
			}
		}

		public static int getInt (ArrayList list, int index)
		{
			try {
				object obj = list [index];
				if (obj == null)
					return 0;
				if (obj is int)
					return (int)obj;
				return Convert.ToInt32 (obj);
			} catch {
				return 0;
			}
		}

		public static long getLong (ArrayList list, int index)
		{
			try {
				object obj = list [index];
				if (obj == null)
					return 0;
				if (obj is long)
					return (long)obj;
				return Convert.ToInt64 (obj);
			} catch {
				return 0;
			}
		}

		public static double getDouble (ArrayList list, int index)
		{
			try {
				object obj = list [index];
				if (obj == null)
					return 0;
				if (obj is double)
					return (double)obj;
				return Convert.ToDouble (obj);
			} catch {
				return 0;
			}
		}

		public static string getString (ArrayList list, int index)
		{
			try {
				object obj = list [index];
				if (obj == null)
					return "";
				if (obj is string)
					return (string)obj;
				return Convert.ToString (obj);
			} catch {
				return "";
			}
		}

		public static ArrayList getList (ArrayList list, int index)
		{
			try {
				object obj = list [index];
				if (obj == null)
					return new ArrayList ();
				if (obj is ArrayList)
					return (ArrayList)obj;
				return new ArrayList ();
			} catch {
				return new ArrayList ();
			}
		}

		public static Hashtable getMap (ArrayList list, int index)
		{
			try {
				object obj = list [index];
				if (obj == null)
					return new Hashtable ();
				if (obj is Hashtable)
					return (Hashtable)obj;
				return new Hashtable ();
			} catch {
				return new Hashtable ();
			}
		}
		//

		public static ArrayList toList (params object[] args)
		{
			ArrayList list = new ArrayList ();
			foreach (object o in args)
				list.Add (o);
			return list;
		}

		public static ArrayList ToList (object[] arrays)
		{
			ArrayList list = new ArrayList ();
			foreach (object o in arrays)
				list.Add (o);
			return list;
		}

		public static ArrayList newList ()
		{
			return new ArrayList ();
		}

		public static bool containsIntVal(ArrayList list, int v)
		{
			if (list == null)
				return false;
			return list.Contains (v);
		}

		public static bool withIn (ArrayList list, object v)
		{
			return list.Contains (v);
		}

		public static bool withIn (int[] list, int v)
		{
			foreach (int v1 in list) {
				if (v1 == v)
					return true;
			}
			return false;
		}

		public static object Next (ArrayList list)
		{
			int index = NumEx.NextInt (list.Count);
			return list [index];
		}

		public static int Next (int[] arrays)
		{
			int index = NumEx.NextInt (arrays.Length);
			return arrays [index];
		}

		public static string Next (string[] arrays)
		{
			int index = NumEx.NextInt (arrays.Length);
			return arrays [index];
		}

		public static ArrayList Next2 (ArrayList list)
		{
			ArrayList list2 = new ArrayList ();
			ArrayList list1 = Copy (list);
			while (list1.Count > 0) {
				object o = Next (list1);
				list1.Remove (o);
				list2.Add (o);
			}
			return list2;
		}

		public static ArrayList Copy (ArrayList list)
		{
			ArrayList list2 = new ArrayList ();
			list2.AddRange (list);
			return list2;
		}

		public override string ToString ()
		{
			StringBuilder sb = new StringBuilder ();
			int count = list.Count;
			int i = 0;
			foreach (object o in list) {
				i++;
				sb.Append (o.ToString ());
				if (i < count)
					sb.Append (",");
			}
			return sb.ToString ();
		}

		public static string ToString (ArrayList list)
		{
			StringBuilder sb = new StringBuilder ();
			int count = list.Count;
			int i = 0;
			foreach (object o in list) {
				i++;
				sb.Append (o.ToString ());
				if (i < count)
					sb.Append (",");
			}
			return sb.ToString ();
		}

		public static ArrayList shuffleRnd (ArrayList src)
		{
			System.Random random = new System.Random ();
			ArrayList newList = new ArrayList ();
			foreach (object item in src) {
				newList.Insert (random.Next (newList.Count), item);
			}
			return newList;
		}

		public static ArrayList sort (ArrayList list, IComparer c)
		{
			list.Sort (c);
			return list;
		}

		public static List<Hashtable> sortHashtable (List<Hashtable> list, string key)
		{
			list.Sort (new HashtableComparer (key));
			return list;
		}

		public static List<NewMap> sortNewMap (List<NewMap> list, string key)
		{
			list.Sort (new NewMapComparer (key));
			return list;
		}


		public static int compareTo (object o1, object o2)
		{
			try {
				if (o1 == null || o2 == null)
					return 0;
				if (!Types.Equals (o1, o1))
					return 0;

				if (o1 is bool) {
					bool v1 = (bool)o1;
					bool v2 = (bool)o2;
					return v1.CompareTo (v2);
				} else if (o1 is Boolean) {
					Boolean v1 = (Boolean)o1;
					bool v2 = (Boolean)o2;
					return v1.CompareTo (v2);
				} else if (o1 is byte) {
					byte v1 = (byte)o1;
					byte v2 = (byte)o2;
					return v1.CompareTo (v2);
				} else if (o1 is Byte) {
					Byte v1 = (Byte)o1;
					Byte v2 = (Byte)o2;
					return v1.CompareTo (v2);
				} else if (o1 is short) {
					short v1 = (short)o1;
					short v2 = (short)o2;
					return v1.CompareTo (v2);
				} else if (o1 is Int16) {
					Int16 v1 = (Int16)o1;
					Int16 v2 = (Int16)o2;
					return v1.CompareTo (v2);
				} else if (o1 is UInt16) {
					UInt16 v1 = (UInt16)o1;
					UInt16 v2 = (UInt16)o2;
					return v1.CompareTo (v2);
				} else if (o1 is int) {
					int v1 = (int)o1;
					int v2 = (int)o2;
					return v1.CompareTo (v2);
				} else if (o1 is Int32) {
					Int32 v1 = (Int32)o1;
					Int32 v2 = (Int32)o2;
					return v1.CompareTo (v2);
				} else if (o1 is UInt32) {
					UInt32 v1 = (UInt32)o1;
					UInt32 v2 = (UInt32)o2;
					return v1.CompareTo (v2);
				} else if (o1 is long) {
					long v1 = (long)o1;
					long v2 = (long)o2;
					return v1.CompareTo (v2);
				} else if (o1 is Int64) {
					Int64 v1 = (Int64)o1;
					Int64 v2 = (Int64)o2;
					return v1.CompareTo (v2);
				} else if (o1 is UInt64) {
					UInt64 v1 = (UInt64)o1;
					UInt64 v2 = (UInt64)o2;
					return v1.CompareTo (v2);
				} else if (o1 is float) {
					float v1 = (float)o1;
					float v2 = (float)o2;
					return v1.CompareTo (v2);
				} else if (o1 is double) {
					double v1 = (double)o1;
					double v2 = (double)o2;
					return v1.CompareTo (v2);
				} else if (o1 is string) {
					string v1 = (string)o1;
					string v2 = (string)o2;
					return v1.CompareTo (v2);
				} else if (o1 is String) {
					String v1 = (String)o1;
					String v2 = (String)o2;
					return v1.CompareTo (v2);
				} else if (o1 is DateTime) {
					DateTime v1 = (DateTime)o1;
					DateTime v2 = (DateTime)o2;
					return v1.CompareTo (v2);
				}
			} catch (Exception) {

			}
			return 0;
		}

		// method list
		public static bool isNull (IList list)
		{
			return (list == null);
		}

		public static bool isNullOrEmpty (IList list)
		{
			bool r = isNull (list);
			if (!r)
				r = (list.Count <= 0);
			return r;
		}

		public static void clearList (IList list)
		{
			if (isNull (list))
				return;
			list.Clear ();
		}

		public static void clearNullList (IList list)
		{
			clearList (list);
			list = null;
		}

		public static NewList getListNone (int allLen, int curLen)
		{
			NewList reLst = NewList.create ();
			int dif = allLen - curLen;
			for (int i = 0; i < dif; i++) {
				reLst.Add (i);
			}
			return reLst;
		}
	}

	public class HashtableComparer : IComparer<Hashtable>
	{
		protected object _key;

		public HashtableComparer (object key)
		{
			this._key = key;
		}

		public int Compare (Hashtable x, Hashtable y)
		{
			object o1 = x [_key];
			object o2 = y [_key];
			return ListEx.compareTo (o1, o2);
		}
	}

	public class NewMapComparer : IComparer<NewMap>
	{
		protected object _key;

		public NewMapComparer (object key)
		{
			this._key = key;
		}

		public int Compare (NewMap x, NewMap y)
		{
			object o1 = x [_key];
			object o2 = y [_key];
			return ListEx.compareTo (o1, o2);
		}
	}
}