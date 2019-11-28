/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  number 工具
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System;
using System.IO;
using System.Collections;

namespace Coolape
{
	public class NumEx
	{
		// byte
		public const sbyte BYTE_MIN_VALUE = -128;
		public const sbyte BYTE_MAX_VALUE = 127;
		// short
		public const short SHORT_MIN_VALUE = -32768;
		public const short SHORT_MAX_VALUE = 32767;
		// int
		public const long INT_MIN_VALUE = 0x80000000;
		public const long INT_MAX_VALUE = 0x7fffffff;
		// long
		public const double LONG_MIN_VALUE = 0x8000000000000000;
		public const double LONG_MAX_VALUE = 0x7fffffffffffffff;
		public const long KB = 1024;
		public const long MB = 1024 * KB;
		public const long GB = 1024 * MB;
		public const long TB = 1024 * GB;
		public const long PB = 1024 * TB;
		static System.Random __rnd = null;

		public static System.Random _rnd()
		{
			if (__rnd == null)
				__rnd = new System.Random((int)DateTime.Now.Ticks);
			return __rnd;
		}

		public static bool stringToBool(string s)
		{
			try {
				return bool.Parse(s);
			} catch {
				return false;
			}
		}

		public static int stringToInt(string s)
		{
			try {
				if (string.IsNullOrEmpty(s))
					return 0;
				return int.Parse(s);
			} catch {
				return 0;
			}
		}

		public static long stringToLong(string s)
		{
			try {
				return long.Parse(s);
			} catch {
				return 0;
			}
		}

		public static double stringToDouble(string s)
		{
			try {
				return double.Parse(s);
			} catch {
				return 0.0;
			}
		}

		public static int NextInt(int max)
		{
			try {
				return _rnd().Next(max);
			} catch {
				return 0;
			}
		}

        /// <summary>
        /// Nexts the int.注意左包右不包
        /// </summary>
        /// <returns>The int.</returns>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
		public static int NextInt(int min, int max)
		{
			try {
				return _rnd().Next(min, max);
			} catch {
				return 0;
			}

		}

		public static bool NextBool()
		{
			try {
				int r = _rnd().Next(100);
				return r < 50;
			} catch {
				return false;
			}
		}

		public static bool NextBool(double probability)
		{
			try {
				return NextBool(probability * 100, 100);
			} catch {
				return false;
			}
		}

		public static bool NextBool(int v, int max)
		{
			try {
				int r = _rnd().Next(max);
				return r < v;
			} catch {
				return false;
			}
		}

		public static bool NextBool(double v, int max)
		{
			try {
				int r = _rnd().Next(max);
				return r < v;
			} catch {
				return false;
			}
		}

		public static object Next(ArrayList list)
		{
			try {
				int index = NextInt(list.Count);
				return list [index];
			} catch {
				return null;
			}
		}

		public static int Next(int[] arrays)
		{
			try {
				int index = NextInt(arrays.Length);
				return arrays [index];
			} catch {
				return 0;
			}
		}

		public static string Next(string[] arrays)
		{
			try {
				int index = NextInt(arrays.Length);
				return arrays [index];
			} catch {
				return "";
			}
		}
	
		// ///////////////////////////////////////////////////
		// 计算百分率
		public static int percent(double v, double max)
		{
			try {
				if (v <= 0 || max <= 0)
					return 0;
				int r = (int)(v * 100 / max);
				return r > 100 ? 100 : r;
			} catch {
				return 0;
			}
		}

		public static int Min(int[] arrays)
		{
			try {
				int min = arrays [0];
				foreach (int i in arrays) {
					min = min < i ? min : i;
				}
				return min;
			} catch {
				return 0;
			}

		}

		public static int Max(int[] arrays)
		{
			try {
				int max = arrays [0];
				foreach (int i in arrays) {
					max = max > i ? max : i;
				}
				return max;
			} catch {
				return 0;
			}

		}

		public static int Min(ArrayList arrays)
		{
			try {
				int min = (int)arrays [0];
				foreach (int i in arrays) {
					min = min < i ? min : i;
				}
				return min;
			} catch {
				return 0;
			}

		}

		public static int Max(ArrayList arrays)
		{
			try {
				int max = (int)arrays [0];
				foreach (int i in arrays) {
					max = max > i ? max : i;
				}
				return max;
			} catch {
				return 0;
			}

		}

		public static double Min(double[] arrays)
		{
			try {
				double min = arrays [0];
				foreach (int i in arrays)
					min = min < i ? min : i;
				return min;
			} catch {
				return 0.0;
			}

		}

		public static double Max(double[] arrays)
		{
			try {
				double max = arrays [0];
				foreach (int i in arrays)
					max = max > i ? max : i;
				return max;
			} catch {
				return 0.0;
			}
		}

		public static string nStr(long n, long lMax = 9000000)
		{
			if (UnityEngine.Mathf.Abs(n) < lMax)
				return n.ToString();
			if (n < -1000000)
				return ((int)(n / 1000000)) + "M";
			if (n < -1000)
				return ((int)(n / 1000)) + "K";
			if (n < 0)
				return "" + n;
			else if (n < 1000)
				return "" + n;
			else if (n < 1000000)
				return ((int)(n / 1000)) + "K";
			else
				return ((int)(n / 1000000)) + "M";
		}

		//		public static string nStr(int n, int iMax = 1000)
		//		{
		//			return nStr((long)n, (long)iMax);
		//		}

		public static string nStrForLen(int n, int len)
		{
			try {
				string str = n.ToString();
				string ret = "";
				for (int i = 0; i < len - str.Length; i++) {
					ret = PStr.b().a(ret).a("0").e();
				}
				return PStr.b().a(ret).a(str).e();
			} catch {
				return "0";
			}

		}

		public static string nStrForLen(string str, int len)
		{
			try {
				string ret = "";
				for (int i = 0; i < len - str.Length; i++) {
					ret = PStr.b().a(ret).a("0").e();
				}
				return PStr.b().a(ret).a(str).e();
			} catch {
				return "0";
			}
		}

		public static int readByte(Stream input)
		{
			try {
				return input.ReadByte();
			} catch {
				return 0;
			}

		}

		public static bool readBool(Stream input)
		{
			try {
				return readByte(input) == 1 ? true : false;
			} catch {
				return false;
			}

		}

		public static char readChar(Stream input)
		{
			try {
				int ch1 = readByte(input);
				int ch2 = readChar(input);
				if ((ch1 | ch2) < 0)
					throw new IOException("End Of Stream.");
				return (char)((ch1 << 8) + ch2 << 0);
			} catch {
				return '0';
			}
		}

		public static short readShort(Stream input)
		{
			try {
				int ch1 = readByte(input);
				int ch2 = readByte(input);
				if ((ch1 | ch2) < 0)
					throw new IOException("End Of Stream.");
				return (short)((ch1 << 8) + (ch2 << 0));
			} catch {
				return (short)0;
			}
		}

		public static int readInt(Stream input)
		{
			try {
				int ch1 = readByte(input);
				int ch2 = readByte(input);
				int ch3 = readByte(input);
				int ch4 = readByte(input);
				if ((ch1 | ch2 | ch3 | ch4) < 0)
					throw new IOException("End Of Stream.");
				return ((ch1 << 24) + (ch2 << 16) + (ch3 << 8) + (ch4 << 0));
			} catch {
				return 0;
			}
		}

		public static long readLong(Stream input)
		{
			try {
				int ch1 = readByte(input);
				int ch2 = readByte(input);
				int ch3 = readByte(input);
				int ch4 = readByte(input);
				int ch5 = readByte(input);
				int ch6 = readByte(input);
				int ch7 = readByte(input);
				int ch8 = readByte(input);
				if ((ch1 | ch2 | ch3 | ch4 | ch5 | ch6 | ch7 | ch8) < 0)
					throw new IOException("End Of Stream.");
				return (((long)ch1 << 56) +
				((long)(ch2 & 0xFF) << 48) +
				((long)(ch3 & 0xFF) << 40) +
				((long)(ch4 & 0xFF) << 32) +
				((long)(ch5 & 0xFF) << 24) +
				((long)(ch6 & 0xFF) << 16) +
				((long)(ch7 & 0xFF) << 8) +
				((long)(ch8 & 0xFF) << 0));
			} catch {
				return 0;
			}
		}

		public static double Int64BitsToDouble(long v)
		{
			try {
				return BitConverter.Int64BitsToDouble(v);
			} catch {
				return 0;
			}
		}

		public static long DoubleToInt64Bits(double v)
		{
			try {
				return  BitConverter.DoubleToInt64Bits(v);
			} catch {
				return 0;
			}
		}

		public static int kb(int nb)
		{
			return (int)(nb / KB);
		}

		public static int mb(int nb)
		{
			return (int)(nb / MB);
		}

		public static int gb(int nb)
		{
			return (int)(nb / GB);
		}

		public static int tb(int nb)
		{
			return (int)(nb / TB);
		}

		public static int pb(int nb)
		{
			return (int)(nb / PB);
		}

		public static int kb(long nb)
		{
			return (int)(nb / KB);
		}

		public static int mb(long nb)
		{
			return (int)(nb / MB);
		}

		public static int gb(long nb)
		{
			return (int)(nb / GB);
		}

		public static int tb(long nb)
		{
			return (int)(nb / TB);
		}

		public static  int pb(long nb)
		{
			return (int)(nb / PB);
		}

		public static int toInt(object v)
		{
			try {
				return System.Convert.ToInt32(v);
			} catch (System.Exception e) {
				//Debug.LogError (e);
				return 0;
			}
		}

		static MemoryStreamPool msPool = new MemoryStreamPool();
		// bio to int
		public static int bio2Int(byte[] buff)
		{
			if (buff == null)
				return 0;
			MemoryStream ms = null;
			try {
				if (buff == null)
					return 0;
				ms = msPool.borrowObject("");
				ms.Write(buff, 0, buff.Length);
				ms.Position = 0;
				int ret = B2InputStream.readInt(ms);
				msPool.returnObject(ms);
				return ret;
			} catch (System.Exception e) {
				//Debug.LogError (e);
				if (ms != null) {
					msPool.returnObject(ms);
				}
				return 0;
			}
		}

		public static byte[] int2Bio(int v)
		{
			MemoryStream ms = null;
			try {
				ms = msPool.borrowObject("");
				B2OutputStream.writeInt(ms, v);
				ms.SetLength(ms.Position);
				byte[] ret = ms.ToArray();
//				Debug.Log("ret.Length===" + ret.Length);
//				byte[] outbs = new byte[ms.Position];
//				ret.CopyTo(outbs, 0);
				msPool.returnObject(ms);
				return ret;
			} catch (System.Exception e) {
				//Debug.LogError (e);
				if (ms != null) {
					msPool.returnObject(ms);
				}
				return new byte[0];
			}
		}
		//bio to long
		public static long bio2Long(byte[] buff)
		{
			if (buff == null)
				return 0;
			MemoryStream ms = null;
			try {
				if (buff == null)
					return 0;
				ms = msPool.borrowObject("");
				ms.Write(buff, 0, buff.Length);
				ms.Position = 0;
				long ret = (long)(B2InputStream.readObject(ms));
				msPool.returnObject(ms);
				return ret;
			} catch (System.Exception e) {
				//Debug.LogError (e);
				if (ms != null) {
					msPool.returnObject(ms);
				}
				return 0;
			}
		}

		public static byte[] Long2Bio(long v)
		{
			MemoryStream ms = null;
			try {
				ms = msPool.borrowObject("");
				B2OutputStream.writeLong(ms, v);
				ms.SetLength(ms.Position);
				byte[] ret = ms.ToArray();
//				byte[] outbs = new byte[ms.Position];
//				ret.CopyTo(outbs, 0);
				msPool.returnObject(ms);
				return ret;
			} catch (System.Exception e) {
				//Debug.LogError (e);
				if (ms != null) {
					msPool.returnObject(ms);
				}
				return new byte[0];
			}
		}

		public static double bio2Double(byte[] buff)
		{
			if (buff == null)
				return 0;
			MemoryStream ms = null;
			try {
				if (buff == null)
					return 0;
				ms = msPool.borrowObject("");
				ms.Write(buff, 0, buff.Length);
				ms.Position = 0;
				double ret = (double)(B2InputStream.readObject(ms));
				msPool.returnObject(ms);
				return ret;
			} catch (System.Exception e) {
				//Debug.LogError (e);
				if (ms != null) {
					msPool.returnObject(ms);
				}
				return 0;
			}
		}

		public static byte[] Double2Bio(double v)
		{
			try {
				MemoryStream ms = msPool.borrowObject("");
				B2OutputStream.writeDouble(ms, v);
				ms.SetLength(ms.Position);
				byte[] ret = ms.ToArray();
//				byte[] outbs = new byte[ms.Position];
//				ret.CopyTo(outbs, 0);
				msPool.returnObject(ms);
				return ret;
			} catch (System.Exception e) {
				//Debug.LogError (e);
				return new byte[0];
			}
		}

		public static byte[] getB2Int(int v)
		{
			try {
				MemoryStream ms = msPool.borrowObject("");
				B2OutputStream.writeB2Int(ms, v);
				ms.SetLength(ms.Position);
				byte[] ret = ms.ToArray();
//				byte[] outbs = new byte[ms.Position];
//				ret.CopyTo(outbs, 0);
				msPool.returnObject(ms);
				return ret;
			} catch (System.Exception e) {
				//Debug.LogError (e);
				return new byte[0];
			}
		}


		// 取一个数的整数部分
		public static int getIntPart(float x)
		{
			return (int)x;
//			int flag = 1;
//			int ret = 0;
//			if (x < 0) {
//				flag = -1;
//			}
//			ret = Mathf.Abs (x);
//			x = Mathf.FloorToInt (x);
//			return flag * x;
		}
	}
}
