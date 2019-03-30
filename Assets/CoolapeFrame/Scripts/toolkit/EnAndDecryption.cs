/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  加解密,注意：只能对短字符串进么加密
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;

namespace Coolape
{
	public static class EnAndDecryption
	{
		/// <summary>
		/// Bgnuadd the specified nu1 and nu2.大数相加
		/// </summary>
		/// <param name='nu1'>
		/// Nu1.
		/// </param>
		/// <param name='nu2'>
		/// Nu2.
		/// </param>
		static string bgnuadd (string nu1, string nu2)
		{
			if (string.IsNullOrEmpty (nu2))
				return nu1;
			if (string.IsNullOrEmpty (nu1))
				return nu2;
			string result = "";
//    	'nu1为负、nu2为正
			if (nu1 [0] == '-' && nu2 [0] != '-') {
				result = bignumbersubduct (nu2, nu1.Substring (1));
//    'nu1为负、nu2为负
			} else if (nu1 [0] == '-' && nu2 [0] == '-') {
				result = "-" + bignumberadditive (nu1.Substring (1), nu2.Substring (1));
//    'nu1为正、nu2为正
			} else if (nu1 [0] != '-' && nu2 [0] != '-') {
				result = bignumberadditive (nu1, nu2); 
//    'nu1为正、nu2为负
			} else if (nu1 [0] != '-' && nu2 [0] == '-') {
				result = bignumbersubduct (nu1, nu2.Substring (1));
			}
			return result;
		}

		/// <summary>
		/// Bgnusub the specified nu1 and nu2.大数相减
		/// </summary>
		/// <param name='nu1'>
		/// Nu1.
		/// </param>
		/// <param name='nu2'>
		/// Nu2.
		/// </param>
		public static string bgnusub (string nu1, string nu2)
		{
			if (string.IsNullOrEmpty (nu2))
				return nu1;
			if (string.IsNullOrEmpty (nu1))
				return nu2;
			string result = "";
//    'nu1为负、nu2为正
			if (nu1 [0] == '-' && nu2 [0] != '-') {
				result = "-" + bignumberadditive (nu1.Substring (1), nu2);
//    'nu1为负、nu2为负
			} else if (nu1 [0] == '-' && nu2 [0] == '-') {
				result = bignumbersubduct (nu2.Substring (1), nu1.Substring (1));
//    'nu1为正、nu2为正
			} else if (nu1 [0] != '-' && nu2 [0] != '-') {
				result = bignumbersubduct (nu1, nu2);
//    'nu1为正、nu2为负
			} else if (nu1 [0] != '-' && nu2 [0] == '-') {
				result = bignumberadditive (nu1, nu2.Substring (1));
			}
			return result;
		}

		/// <summary>
		/// Bignumberadditive the specified nu1 and nu2.
		/// 大数相加，以4位长的数字分段计算两个参数是不代符号的
		/// </summary>
		/// <param name='nu1'>
		/// Nu1.
		/// </param>
		/// <param name='nu2'>
		/// Nu2.
		/// </param>
		static string bignumberadditive (string nu1, string nu2)
		{
			string result = "";
			string a = "";
			string b = "";
			int sizea = 0;
			int sizeb = 0;
			string tmpstr;
			int i = 0;
			a = nu1;
			b = nu2;
	
			if (a.Length < b.Length) {
				tmpstr = a;
				a = b;
				b = tmpstr;
			}
		
			if (a.Length % 4 == 0) {
				sizea = a.Length / 4;
			} else {
				sizea = a.Length / 4 + 1;
			}
		
			if (b.Length % 4 == 0) {
				sizeb = b.Length / 4;
			} else {
				sizeb = b.Length / 4 + 1;
			}
			string[] lista = new string[sizea];
			string[] tmpresult = new string[sizea];
			string[] listb = new string[sizeb];
                                            
			for (i = 0; i < sizea; i++) {
				if (a.Length > 4) {
					lista [i] = StrEx.Right (a, 4);
					a = StrEx.Left (a, a.Length - 4);
				} else {
					lista [i] = StrEx.Right (a, a.Length);
					a = StrEx.Left (a, a.Length);
				}
			}
			for (i = 0; i < sizeb; i++) {
				if (b.Length > 4) {
					listb [i] = StrEx.Right (b, 4);
					b = StrEx.Left (b, b.Length - 4);
				} else {
					listb [i] = StrEx.Right (b, b.Length);
					b = StrEx.Left (b, b.Length);
				}
			}
                                            
			for (i = 0; i < sizea; i++) {
				if (i < sizeb) {
					tmpresult [i] = (NumEx.stringToInt (lista [i]) + NumEx.stringToInt (listb [i])).ToString ();
				} else {
					tmpresult [i] = lista [i];
				}
				if (i != 0) {
					if ((tmpresult [i - 1]).Length == 5) {
						tmpresult [i] = (NumEx.stringToInt (tmpresult [i]) + 1).ToString ();
					}
				}
				if (i != sizea - 1) {
					int tmpN = 0;
					if (tmpresult [i].Length >= 4) {
						tmpN = NumEx.stringToInt (StrEx.Right (tmpresult [i], 4));
					} else {
						tmpN = NumEx.stringToInt (tmpresult [i]);
					} 
					result = NumEx.nStrForLen (tmpN, 4) + result;
				} else {
					result = tmpresult [i] + result;
				}
			}
			return result;
		}

		/// <summary>
		/// Bignumbersubduct the specified nu1 and nu2.
		/// 大数相减，以4位长的数字分段计算
		/// 两个参数是不代符号的
		/// </summary>
		/// <param name='nu1'>
		/// Nu1.
		/// </param>
		/// <param name='nu2'>
		/// Nu2.
		/// </param>
		static string bignumbersubduct (string nu1, string nu2)
		{
			string result = "";
			string a;
			string b;
			string tmpstr;
			int sizea = 0;
			int sizeb = 0;
		
			int i = 0;
			string flag = "";
			a = nu1;
			b = nu2;
			if (a.Length < b.Length) {
				tmpstr = a;
				a = b;
				b = tmpstr;
				flag = "-";
			} else if (a.Length == b.Length) {
				if (a.CompareTo (b) == -1) {
					tmpstr = a;
					a = b;
					b = tmpstr;
					flag = "-";
				}
			}
    
			if (a.Length % 4 == 0) {
				sizea = a.Length / 4;
			} else {
				sizea = a.Length / 4 + 1;
			}
		
			if (b.Length % 4 == 0) {
				sizeb = b.Length / 4;
			} else {
				sizeb = b.Length / 4 + 1;
			}
			string[] lista = new string[sizea];
			string[] tmpresult = new string[sizea];
			string[] listb = new string[sizeb];
			for (i = 0; i < sizea; i++) {
				if (a.Length > 4) {
					lista [i] = StrEx.Right (a, 4);
					a = StrEx.Left (a, a.Length - 4);
				} else {
					lista [i] = StrEx.Right (a, a.Length);
					a = StrEx.Left (a, a.Length);
				}
			}
		
			for (i = 0; i < sizeb; i++) {
				if (b.Length > 4) {
					listb [i] = StrEx.Right (b, 4);
					b = StrEx.Left (b, b.Length - 4);
				} else {
					listb [i] = StrEx.Right (b, b.Length);
					b = StrEx.Left (b, b.Length);
				}
			}
			for (i = 0; i < sizea; i++) {
				if (i < sizeb) {
					if (i != sizea - 1) {
						tmpresult [i] = (NumEx.stringToInt ("1" + lista [i]) - NumEx.stringToInt (listb [i])).ToString ();
					} else {
						tmpresult [i] = (NumEx.stringToInt (lista [i]) - NumEx.stringToInt (listb [i])).ToString ();
					}
				} else {
					if (i != sizea - 1) {
						tmpresult [i] = "1" + lista [i];
					} else {
						tmpresult [i] = lista [i];
					}
				}
				if (i != 0) {
					if (tmpresult [i - 1].Length < 5) {
						tmpresult [i] = (NumEx.stringToInt (tmpresult [i]) - 1).ToString ();
					}
				}
				if (i != sizea - 1) {
					int tempN = 0;
					if (tmpresult [i].Length >= 4) {
						tempN = NumEx.stringToInt (StrEx.Right (tmpresult [i], 4));
					} else {
						tempN = NumEx.stringToInt (tmpresult [i]);
					} 
				
					result = NumEx.nStrForLen (tempN, 4) + result;
				} else {
					result = tmpresult [i] + result;
				}
			}
			result = flag + result;
			return result;
		}

		/// <summary>
		/// Encoder the specified str and scrtkey.加密
		/// </summary>
		/// <param name='str'>要加密的串
		/// String.
		/// </param>
		/// <param name='scrtkey'>密钥secretkey
		/// Scrtkey.
		/// </param>
		public static string encoder (string str, string scrtkey)
		{
			if (string.IsNullOrEmpty (str))
				return "";
			string unicodestr = "";
			string posstr = "";
			string tmpstr = "";
			string uniscrtkey = "";
			string ret = "";
			int i;
			int[] poslist = new int[str.Length];
			for (i = 0; i < str.Length; i++) {
				unicodestr = unicodestr + (int)(str [i]); 
				poslist [i] = unicodestr.Length;
			}
			for (i = 0; i < str.Length; i++) {
				tmpstr = StrEx.Mid (unicodestr, poslist [i] - 1, 1);
				unicodestr = tmpstr + StrEx.Left (unicodestr, poslist [i] - 1) + StrEx.Mid (unicodestr, poslist [i]);
				posstr = posstr + NumEx.nStrForLen (poslist [i], 4); //每4位表示一个位置
			}
			for (i = 0; i < scrtkey.Length; i++) {
				uniscrtkey = uniscrtkey + (int)(scrtkey [i]);
			}
			string flag = "+";
			posstr = trimIntZero (posstr);
			string sub = bgnusub (uniscrtkey, posstr);
			if (!string.IsNullOrEmpty (sub) && sub.Length > 0 && sub [0] == '-') {
				sub = StrEx.Mid (sub, 1);
				flag = "-";
			}
			//每四位中把前面为0的用+号代表
			string enSub = "";
			int tmpN = 0;
			for (i = sub.Length - 4; i >= 0; i = i - 4) {
				tmpN = NumEx.stringToInt (StrEx.Mid (sub, i, 4));
				enSub = (tmpN.ToString ().Length < 4 ? "+" : "") + tmpN + enSub;
			}
			if (i != -4) {
				tmpN = NumEx.stringToInt (StrEx.Left (sub, i + 4));
				enSub = (tmpN.ToString ().Length < 4 ? "+" : "") + tmpN + enSub;
			}
		
			ret = unicodestr + flag + enSub;
			return ret;
		}

	
		/// <summary>
		/// Decoder the specified encodestr and scrtkey.解密
		/// </summary>
		/// <param name='encodestr'>要解密的串
		/// Encodestr.
		/// </param>
		/// <param name='scrtkey'>密钥secretkey
		/// Scrtkey.
		/// </param>
		public static string decoder (string encodestr, string scrtkey)
		{
			if (string.IsNullOrEmpty (encodestr) || string.IsNullOrEmpty (scrtkey))
				return "";
			string result = "";
			string unicodestr = "";
			string posstr = "";
			string tmpstr = "";
			string uniscrtkey = "";
			int sizepos = 0;
			int i = 0;
			char splitChar = '-';
			int splitPos = encodestr.IndexOf ('-');
			if (splitPos < 0) {
				splitChar = '+';
				splitPos = encodestr.IndexOf ('+');
			}
			if (splitPos < 0)
				return "";
			unicodestr = StrEx.Left (encodestr, splitPos);
			posstr = StrEx.Right (encodestr, encodestr.Length - splitPos - 1);
			string[] ss = posstr.Split ('+');
			posstr = "";
			for (i = 0; i < ss.Length; i++) {
				int j = 0;
				tmpstr = "";
				for (j = ss [i].Length - 4; j >= 0; j = j - 4) {
					tmpstr = StrEx.Mid (ss [i], j, 4) + tmpstr;
				}
				if (j != -4) {
					int tmpN = NumEx.stringToInt (StrEx.Mid (ss [i], 0, j + 4));
					tmpstr = NumEx.nStrForLen (tmpN, 4) + tmpstr;
				}
				posstr += tmpstr;
			}
			//去掉面前的0
			posstr = trimIntZero (posstr);
		
			if (splitChar == '-') {
				posstr = "-" + posstr; 
			}
			for (i = 0; i < scrtkey.Length; i++) {
				uniscrtkey = uniscrtkey + (int)(scrtkey [i]);
			}
			posstr = bgnusub (uniscrtkey, posstr);
			if (posstr.Length % 4 == 0) {
				sizepos = posstr.Length / 4;
			} else {
				sizepos = posstr.Length / 4 + 1;
			}
			int[] poslist = new int[sizepos];
			for (i = 0; i < sizepos; i++) {
				int tmpN = 0;
				if (posstr.Length >= 4) {
					tmpN = NumEx.stringToInt (StrEx.Right (posstr, 4));
				} else {
					tmpN = NumEx.stringToInt (posstr);
				}
				if (tmpN == 0)
					break;
				poslist [i] = tmpN;
				if (posstr.Length > 4) {
					posstr = StrEx.Left (posstr, posstr.Length - 4);
				}
			}
			sizepos = i;
			for (i = 0; i < sizepos; i++) {
				unicodestr = StrEx.Left (unicodestr, poslist [i]) + StrEx.Mid (unicodestr, 0, 1) + StrEx.Mid (unicodestr, poslist [i]);
				unicodestr = StrEx.Mid (unicodestr, 1);
			}
			for (i = 0; i < sizepos; i++) {
				if (i != sizepos - 1) {
					result = (char)(NumEx.stringToInt (StrEx.Mid (unicodestr, poslist [i + 1], poslist [i] - poslist [i + 1]))) + result;
				} else {
					result = (char)(NumEx.stringToInt (StrEx.Mid (unicodestr, 0, poslist [i]))) + result;
				}
			}
			return result;
		}

		public static string trimIntZero (string nStr)
		{
			if (string.IsNullOrEmpty (nStr))
				return "";
			string flag = StrEx.Left (nStr, 1);
			string tmpStr = nStr;
			if (flag == "+" || flag == "-") {
				tmpStr = StrEx.Mid (nStr, 1);
			} else {
				flag = "";
			}
			int len = tmpStr.Length;
			int index = 0;
			for (int i = 0; i < len; i++) {
				if (tmpStr [i] != '0') {
					break;
				}
				index++;
			}
		
			return flag + StrEx.Mid (tmpStr, index);
		}
	}
}
