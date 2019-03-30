/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  xxtea 一个非常快速小巧的加解密算法
  *						http://en.wikipedia.org/wiki/XXTEA
  *Others:  
  *History:
*********************************************************************************
*/ 
using System.Text;
using System;

namespace Coolape
{
	public class XXTEA
	{
		public static string key = "coolape";
		static Byte[] _defaultKey;

		public static Byte[] defaultKey {
			get {
				if (_defaultKey == null) {
					_defaultKey = Encoding.Default.GetBytes (key);
				}
				return _defaultKey;
			}
		}

		public static Byte[] encodeStr (string Data, string Key = null)
		{
			if (string.IsNullOrEmpty (Data)) {
				return null;
			}
			Byte[] _key = null;
			if (string.IsNullOrEmpty (Key)) {
				_key = defaultKey;
			} else {
				_key = Encoding.Default.GetBytes (key);
			}
			Byte[] databytes = Encoding.UTF8.GetBytes (Data);
			return Encrypt (databytes, _key);
		}

		public static string decodeStr (Byte[] Data, string Key = null)
		{
			if (Data == null) {
				return "";
			}
			Byte[] _key = null;
			if (string.IsNullOrEmpty (Key)) {
				_key = defaultKey;
			} else {
				_key = Encoding.Default.GetBytes (key);
			}
			Byte[] databytes = Decrypt (Data, _key);
			if (databytes != null) {
				return Encoding.UTF8.GetString (databytes);
			} else {
				return "";
			}
		}

		public static Byte[] Encrypt (Byte[] Data, Byte[] Key)
		{
			if (Data.Length == 0) {
				return Data;
			}
			return ToByteArray (Encrypt (ToUInt32Array (Data, true), ToUInt32Array (Key, false)), false);
		}

		public static Byte[] Decrypt (Byte[] Data, Byte[] Key)
		{
			if (Data.Length == 0) {
				return Data;
			}
			return ToByteArray (Decrypt (ToUInt32Array (Data, false), ToUInt32Array (Key, false)), true);
		}

		public static UInt32[] Encrypt (UInt32[] v, UInt32[] k)
		{
			Int32 n = v.Length - 1;
			if (n < 1) {
				return v;
			}
			if (k.Length < 4) {
				UInt32[] Key = new UInt32[4];
				k.CopyTo (Key, 0);
				k = Key;
			}
			UInt32 z = v [n], y = v [0], delta = 0x9E3779B9, sum = 0, e;
			Int32 p, q = 6 + 52 / (n + 1);
			while (q-- > 0) {
				sum = unchecked(sum + delta);
				e = sum >> 2 & 3;
				for (p = 0; p < n; p++) {
					y = v [p + 1];
					z = unchecked(v [p] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k [p & 3 ^ e] ^ z));
				}
				y = v [0];
				z = unchecked(v [n] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k [p & 3 ^ e] ^ z));
			}
			return v;
		}

		public static UInt32[] Decrypt (UInt32[] v, UInt32[] k)
		{
			Int32 n = v.Length - 1;
			if (n < 1) {
				return v;
			}
			if (k.Length < 4) {
				UInt32[] Key = new UInt32[4];
				k.CopyTo (Key, 0);
				k = Key;
			}
			UInt32 z = v [n], y = v [0], delta = 0x9E3779B9, sum, e;
			Int32 p, q = 6 + 52 / (n + 1);
			sum = unchecked((UInt32)(q * delta));
			while (sum != 0) {
				e = sum >> 2 & 3;
				for (p = n; p > 0; p--) {
					z = v [p - 1];
					y = unchecked(v [p] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k [p & 3 ^ e] ^ z));
				}
				z = v [n];
				y = unchecked(v [0] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k [p & 3 ^ e] ^ z));
				sum = unchecked(sum - delta);
			}
			return v;
		}

		private static UInt32[] ToUInt32Array (Byte[] Data, Boolean IncludeLength)
		{
			Int32 n = (((Data.Length & 3) == 0) ? (Data.Length >> 2) : ((Data.Length >> 2) + 1));
			UInt32[] Result;
			if (IncludeLength) {
				Result = new UInt32[n + 1];
				Result [n] = (UInt32)Data.Length;
			} else {
				Result = new UInt32[n];
			}
			n = Data.Length;
			for (Int32 i = 0; i < n; i++) {
				Result [i >> 2] |= (UInt32)Data [i] << ((i & 3) << 3);
			}
			return Result;
		}

		private static Byte[] ToByteArray (UInt32[] Data, Boolean IncludeLength)
		{
			Int32 n;
			if (IncludeLength) {
				n = (Int32)Data [Data.Length - 1];
			} else {
				n = Data.Length << 2;
			}
			Byte[] Result = new Byte[n];
			for (Int32 i = 0; i < n; i++) {
				Result [i] = (Byte)(Data [i >> 2] >> ((i & 3) << 3));
			}
			return Result;
		}
	}
}
