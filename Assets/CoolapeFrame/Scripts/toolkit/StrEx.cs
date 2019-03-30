/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  字符串工具类
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;
using System.Text;

namespace Coolape
{
	public class StrEx
	{
		public StringBuilder sb;

		public StrEx()
		{
			this.sb = new StringBuilder();
		}

		public StrEx(StringBuilder sb)
		{
			this.sb = sb;
		}

		public static StrEx builder()
		{
			return new StrEx();
		}

		public StrEx Append(bool v)
		{
			this.sb.Append(v);
			return this;
		}

		public StrEx Append(byte v)
		{
			this.sb.Append(v);
			return this;
		}

		public StrEx Append(short v)
		{
			this.sb.Append(v);
			return this;
		}

		public StrEx Append(int v)
		{
			this.sb.Append(v);
			return this;
		}

		public StrEx Append(long v)
		{
			this.sb.Append(v);
			return this;
		}

		public StrEx Append(double v)
		{
			this.sb.Append(v);
			return this;
		}

		public StrEx Append(string v)
		{
			this.sb.Append(v);
			return this;
		}

		public StrEx AppendLine()
		{
			this.sb.AppendLine();
			return this;
		}

		public StrEx AppendLine(string v)
		{
			this.sb.AppendLine(v);
			return this;
		}

		public StrEx Append(StringBuilder v)
		{
			this.sb.Append(v);
			return this;
		}

		public StrEx AppendFormat(string fmt, object o)
		{
			this.sb.AppendFormat(fmt, o);
			return this;
		}

		public int Length {
			get {
				return Count();
			}
		}

		public int Count()
		{
			return this.Count();
		}

		public StrEx Clear()
		{
			this.Clear();
			return this;
		}

		public override string ToString()
		{
			return sb.ToString();
		}

		public static string Left(string s, int len)
		{
			return s.Substring(0, len);
		}

		public string Left(int len)
		{
			return ToString().Substring(0, len);
		}

		public  static string Right(string s, int len)
		{
			int length = s.Length;
			return s.Substring(length - len, len);
		}

		public string Right(int len)
		{
			int length = ToString().Length;
			return ToString().Substring(length - len, len);
		}

		public  static string Mid(string s, int start, int len)
		{
			return s.Substring(start, len);
		}

		public  static string Mid(string s, int start)
		{
			return s.Substring(start);
		}

		public string Mid(int begin, int len)
		{
			return  ToString().Substring(begin, len);
		}

		public static string mapToString(Hashtable map)
		{
			StrEx builder = StrEx.builder();
			ICollection keys = map.Keys;
			IEnumerator e = keys.GetEnumerator();
			while (e.MoveNext()) {
				object key = e.Current;
				object varlue = map [key];
				builder.Append(key.ToString()).Append("=").Append(varlue.ToString()).Append("\n");
			}
			return builder.ToString();
		}

		public static string listToString(ArrayList list)
		{
			StrEx builder = StrEx.builder();
			foreach (object o in list) {
				builder.Append(o.ToString()).Append("\n");
			}
			return builder.ToString();
		}

		public StrEx ap(string s)
		{
			this.sb.Append(s);
			return this;
		}

		public StrEx ap(string fmt, params object[] args)
		{
			this.sb.Append(format(fmt, args));
			return this;
		}

		public StrEx pn()
		{
			this.sb.AppendLine();
			return this;
		}

		public StrEx pn(string s)
		{
			this.sb.AppendLine(s);
			return this;
		}

		public StrEx pn(string fmt, params object[] args)
		{
			this.sb.AppendLine(format(fmt, args));
			return this;
		}

		public static string format(string fmt, params object[] args)
		{
			Hashtable p = new Hashtable();
			int length = args.Length;
			for (int i = 1; i < length + 1; i++) {
				string key = i.ToString();
				string value = args [i - 1].ToString();
				p [key] = value;
			}
			return make(fmt, p);
		}

		public static string make(string s, Hashtable param)
		{
			if (s == null || s.Length <= 0 || param == null || param.Count <= 0)
				return s;
			if (s.IndexOf("${") < 0 && s.IndexOf("$[") < 0)
				return s;

			ArrayList keys = new ArrayList(param.Keys);
			foreach (object key in keys) {
				object v = param [key];
				string k = "${" + key + "}";
				string k2 = "$[" + key + "]";
				string var = v.ToString();
				while (s.Contains(k))
					s = s.Replace(k, var);
				while (s.Contains(k2))
					s = s.Replace(k2, "\"" + var + "\"");
			}
			return s;
		}

		public static string msToTime(long ms)
		{// 将毫秒数换算成x天x时x分x秒
			int ss = 1000;
			int mi = ss * 60;
			int hh = mi * 60;
			int dd = hh * 24;

			long day = ms / dd;
			long hour = (ms - day * dd) / hh;
			long minute = (ms - day * dd - hour * hh) / mi;
			long second = (ms - day * dd - hour * hh - minute * mi) / ss;
			//long milliSecond = ms - day * dd - hour * hh - minute * mi - second * ss;

			string strDay = day < 10 ? "0" + day : "" + day;
			string strHour = hour < 10 ? "0" + hour : "" + hour;
			string strMinute = minute < 10 ? "0" + minute : "" + minute;
			string strSecond = second < 10 ? "0" + second : "" + second;
			//String strMilliSecond = milliSecond < 10 ? "0" + milliSecond : "" + milliSecond;
			//strMilliSecond = milliSecond < 100 ? "0" + strMilliSecond : ""+ strMilliSecond;

			StringBuilder sb = new StringBuilder();
			if (!strDay.Equals("00"))
				sb.Append(strDay + "天");
			if (!strHour.Equals("00") || sb.Length > 1)
				sb.Append(strHour + "时");
			if (!strMinute.Equals("00") || sb.Length > 1)
				sb.Append(strMinute + "分");
			if (!strSecond.Equals("00") || sb.Length > 1)
				sb.Append(strSecond + "秒");
			return sb.ToString();
		}

		public static bool isIpv4(string ip)
		{
			if (ip == null || ip.Length < 7 || ip.Length > 17)
				return false;
			
			int p1 = ip.IndexOf('.');
			if (p1 < 1)
				return false;
			int p2 = ip.IndexOf('.', p1 + 1);
			if (p2 < 1)
				return false;
			int p3 = ip.IndexOf('.', p2 + 1);
			if (p3 < 1)
				return false;

			return true;
		}

		public static int[] ipv4(string ipv4)
		{
			int[] r2 = new int[4];
			if (ipv4 == null || ipv4.Length < 7 || ipv4.Length > 17)
				return r2;
			
			// 127.0.0.1
			int p1 = ipv4.IndexOf('.');
			if (p1 < 1)
				return r2;
			int p2 = ipv4.IndexOf('.', p1 + 1);
			if (p2 < 1)
				return r2;
			int p3 = ipv4.IndexOf('.', p2 + 1);
			if (p3 < 1)
				return r2;

			string s1 = Mid(ipv4, 0, p1);
			string s2 = Mid(ipv4, p1 + 1, p2);
			string s3 = Mid(ipv4, p2 + 1, p3);
			string s4 = Mid(ipv4, p3 + 1, ipv4.Length);
			r2 [0] = NumEx.stringToInt(s1);
			r2 [1] = NumEx.stringToInt(s2);
			r2 [2] = NumEx.stringToInt(s3);
			r2 [3] = NumEx.stringToInt(s4);
			return r2;
		}

		public static bool isMailAddr(string mail)
		{
			if (mail == null || mail.Length < 6)
				return false;
			
			int p1 = mail.IndexOf('@');
			if (p1 < 1)
				return false;
			int p2 = mail.IndexOf('.', p1 + 1);
			if (p2 < 1)
				return false;

			return true;
		}

		static public int getStrLen(string str)
		{
			if (string.IsNullOrEmpty(str))
				return 0;
			return str.Length;
		}

		static public int getStrLen4Trim(string str)
		{
			if (string.IsNullOrEmpty(str))
				return 0;
			str = str.Trim();
			return str.Length;
		}

		static public string trimStr(string str)
		{
			if (string.IsNullOrEmpty(str))
				return "";
			return str.Trim();
		}

		static public string appendSpce(string str, int totalLen)
		{
			int i = 0;
			if (str == null || str == "") {
				str = "";
			}
			i = str.Length;
			PStr ps = PStr.b(str);
			for (; i < totalLen; i++) {
				ps.a(" ");
			}
			return ps.e();
		}
	}
}
