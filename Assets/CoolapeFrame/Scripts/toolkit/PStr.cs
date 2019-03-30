/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  字符串拼接工具
  *						例：string str = Pstr.b().a("Helo").a("world").e();
  *Others:  
  *History:
*********************************************************************************
*/ 

using System;
using System.Collections;
using System.Text;

namespace Coolape
{
	public class PStr
	{
		public StringBuilder sb = null;

		private PStr()
		{
			this.sb = ObjPool.strs.borrowObject();
		}

		public static PStr b()
		{
			return begin();
		}

		public static PStr b(string s)
		{
			return begin(s);
		}

		public static PStr b(params object[] objs)
		{
			return begin(objs);
		}

		public static PStr begin()
		{
			PStr ret = new PStr();
			return ret;
		}

		public static PStr begin(params object[] objs)
		{
			PStr r2 = begin();
			return r2.a(objs);
		}

		public static PStr begin(string s)
		{
			PStr ret = new PStr();
			ret.sb.Append(s);
			return ret;
		}

		public PStr a(string value)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(char[] value)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(bool value)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(byte value)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(decimal value)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(double value)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(short value)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(int value)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(params object[] objs)
		{
			foreach (object o in objs) {
				a(o);
			}
			return this;
		}

		public PStr a(long value)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(object value)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(sbyte value)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(float value)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(ushort value)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(uint value)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(ulong value)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(char value)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(byte[] value)
		{
			this.sb.Append(Convert.ToString(value));
			return this;
		}

		public PStr a(char value, int repeatCount)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(char[] value, int startIndex, int charCount)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr a(string value, int startIndex, int count)
		{
			this.sb.Append(value);
			return this;
		}

		public PStr fmt(string fmt, params object[] args)
		{
			this.sb.AppendFormat(fmt, args);
			return this;
		}

		public PStr a_kv(string fmt, params object[] args)
		{
			NewMap p = new NewMap();
			int length = args.Length;
			for (int i = 0; i < length; i = i + 2) {
				string key = args [i].ToString();
				string value = args [i + 1].ToString();
				
				p [key] = value;
			}
			return a(fmt, p);
		}

		public PStr a(string s, params object[] args)
		{
			this.sb.Append(s);
			int i = 0;
			foreach (object obj in args) {
				string skey1 = "${" + i + "}";
				string sval1 = obj.ToString();
			
				sb.Replace(skey1, sval1);
			
				string skey2 = "$[" + i + "]";
				string sval2 = "\"" + obj.ToString() + "\"";
				sb.Replace(skey2, sval2);
			}
			return this;
		}

		public PStr a(string fmt, NewMap map)
		{
			//StringBuilder sb = ObjPool.strs.borrowObject ();
			this.sb.Append(fmt);
			ICollection keys = map.Keys;
			foreach (string key in keys) {
				object val = map [key];
			
				string skey1 = "${" + key + "}";
				string sval1 = val.ToString();
			
				sb.Replace(skey1, sval1);
			
				string skey2 = "$[" + key + "]";
				string sval2 = "\"" + val.ToString() + "\"";
				sb.Replace(skey2, sval2);
			}
		
			return this;
		}

		public PStr an(params object[] objs)
		{
			foreach (object o in objs) {
				a(o);
			}
			this.sb.AppendLine();
			return this;
		}

		public PStr an(string s)
		{
			this.sb.AppendLine(s);
			return this;
		}

		public int Length()
		{
			return this.sb.Length;
		}

		public string e(string s)
		{
			return end(s);
		}

		public string e()
		{
			return end();
		}

		public string end(string s)
		{
			this.sb.Append(s);
			String ret = this.sb.ToString();
			ObjPool.strs.returnObject(this.sb);
			this.sb = null;
			return ret;
		}

		public string end()
		{
			String ret = this.sb.ToString();
			ObjPool.strs.returnObject(this.sb);
			this.sb = null;
			return ret;
		}


		public string str()
		{
			return end();
		}
	}
}
