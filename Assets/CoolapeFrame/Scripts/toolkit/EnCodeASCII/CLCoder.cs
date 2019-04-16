using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coolape
{
    public static class CLCoder
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
        static string bgNumAdd(string nu1, string nu2)
        {
            if (string.IsNullOrEmpty(nu2))
                return nu1;
            if (string.IsNullOrEmpty(nu1))
                return nu2;
            string result = "";

            if (nu1[0] == '-' && nu2[0] != '-')// 'nu1为负、nu2为正
            {
                result = bignumbersubduct(nu2, nu1.Substring(1));
            }
            else if (nu1[0] == '-' && nu2[0] == '-')// 'nu1为负、nu2为负
            {
                result = "-" + bignumberadditive(nu1.Substring(1), nu2.Substring(1));
            }
            else if (nu1[0] != '-' && nu2[0] != '-')// 'nu1为正、nu2为正
            {
                result = bignumberadditive(nu1, nu2);
            }
            else if (nu1[0] != '-' && nu2[0] == '-')// 'nu1为正、nu2为负
            {
                result = bignumbersubduct(nu1, nu2.Substring(1));
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
        public static string bgNumSub(string nu1, string nu2)
        {
            if (string.IsNullOrEmpty(nu2))
                return nu1;
            if (string.IsNullOrEmpty(nu1))
                return nu2;
            string result = "";

            if (nu1[0] == '-' && nu2[0] != '-')         // 'nu1为负、nu2为正
            {
                result = "-" + bignumberadditive(nu1.Substring(1), nu2);
            }
            else if (nu1[0] == '-' && nu2[0] == '-')    // 'nu1为负、nu2为负
            {
                result = bignumbersubduct(nu2.Substring(1), nu1.Substring(1));
            }
            else if (nu1[0] != '-' && nu2[0] != '-')    // 'nu1为正、nu2为正
            {
                result = bignumbersubduct(nu1, nu2);
            }
            else if (nu1[0] != '-' && nu2[0] == '-')    // 'nu1为正、nu2为负
            {
                result = bignumberadditive(nu1, nu2.Substring(1));
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
        static string bignumberadditive(string nu1, string nu2)
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

            if (a.Length < b.Length)
            {
                tmpstr = a;
                a = b;
                b = tmpstr;
            }

            if (a.Length % 4 == 0)
            {
                sizea = a.Length / 4;
            }
            else
            {
                sizea = a.Length / 4 + 1;
            }

            if (b.Length % 4 == 0)
            {
                sizeb = b.Length / 4;
            }
            else
            {
                sizeb = b.Length / 4 + 1;
            }
            string[] lista = new string[sizea];
            string[] tmpresult = new string[sizea];
            string[] listb = new string[sizeb];

            for (i = 0; i < sizea; i++)
            {
                if (a.Length > 4)
                {
                    lista[i] = StrEx.Right(a, 4);
                    a = StrEx.Left(a, a.Length - 4);
                }
                else
                {
                    lista[i] = StrEx.Right(a, a.Length);
                    a = StrEx.Left(a, a.Length);
                }
            }
            for (i = 0; i < sizeb; i++)
            {
                if (b.Length > 4)
                {
                    listb[i] = StrEx.Right(b, 4);
                    b = StrEx.Left(b, b.Length - 4);
                }
                else
                {
                    listb[i] = StrEx.Right(b, b.Length);
                    b = StrEx.Left(b, b.Length);
                }
            }

            for (i = 0; i < sizea; i++)
            {
                if (i < sizeb)
                {
                    tmpresult[i] = (NumEx.stringToInt(lista[i]) + NumEx.stringToInt(listb[i])).ToString();
                }
                else
                {
                    tmpresult[i] = lista[i];
                }
                if (i != 0)
                {
                    if ((tmpresult[i - 1]).Length == 5)
                    {
                        tmpresult[i] = (NumEx.stringToInt(tmpresult[i]) + 1).ToString();
                    }
                }
                if (i != sizea - 1)
                {
                    int tmpN = 0;
                    if (tmpresult[i].Length >= 4)
                    {
                        tmpN = NumEx.stringToInt(StrEx.Right(tmpresult[i], 4));
                    }
                    else
                    {
                        tmpN = NumEx.stringToInt(tmpresult[i]);
                    }
                    result = NumEx.nStrForLen(tmpN, 4) + result;
                }
                else
                {
                    result = tmpresult[i] + result;
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
        static string bignumbersubduct(string nu1, string nu2)
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
            if (a.Length < b.Length)
            {
                tmpstr = a;
                a = b;
                b = tmpstr;
                flag = "-";
            }
            else if (a.Length == b.Length)
            {
                if (a.CompareTo(b) == -1)
                {
                    tmpstr = a;
                    a = b;
                    b = tmpstr;
                    flag = "-";
                }
            }

            if (a.Length % 4 == 0)
            {
                sizea = a.Length / 4;
            }
            else
            {
                sizea = a.Length / 4 + 1;
            }

            if (b.Length % 4 == 0)
            {
                sizeb = b.Length / 4;
            }
            else
            {
                sizeb = b.Length / 4 + 1;
            }
            string[] lista = new string[sizea];
            string[] tmpresult = new string[sizea];
            string[] listb = new string[sizeb];
            for (i = 0; i < sizea; i++)
            {
                if (a.Length > 4)
                {
                    lista[i] = StrEx.Right(a, 4);
                    a = StrEx.Left(a, a.Length - 4);
                }
                else
                {
                    lista[i] = StrEx.Right(a, a.Length);
                    a = StrEx.Left(a, a.Length);
                }
            }

            for (i = 0; i < sizeb; i++)
            {
                if (b.Length > 4)
                {
                    listb[i] = StrEx.Right(b, 4);
                    b = StrEx.Left(b, b.Length - 4);
                }
                else
                {
                    listb[i] = StrEx.Right(b, b.Length);
                    b = StrEx.Left(b, b.Length);
                }
            }
            for (i = 0; i < sizea; i++)
            {
                if (i < sizeb)
                {
                    if (i != sizea - 1)
                    {
                        tmpresult[i] = (NumEx.stringToInt("1" + lista[i]) - NumEx.stringToInt(listb[i])).ToString();
                    }
                    else
                    {
                        tmpresult[i] = (NumEx.stringToInt(lista[i]) - NumEx.stringToInt(listb[i])).ToString();
                    }
                }
                else
                {
                    if (i != sizea - 1)
                    {
                        tmpresult[i] = "1" + lista[i];
                    }
                    else
                    {
                        tmpresult[i] = lista[i];
                    }
                }
                if (i != 0)
                {
                    if (tmpresult[i - 1].Length < 5)
                    {
                        tmpresult[i] = (NumEx.stringToInt(tmpresult[i]) - 1).ToString();
                    }
                }
                if (i != sizea - 1)
                {
                    int tempN = 0;
                    if (tmpresult[i].Length >= 4)
                    {
                        tempN = NumEx.stringToInt(StrEx.Right(tmpresult[i], 4));
                    }
                    else
                    {
                        tempN = NumEx.stringToInt(tmpresult[i]);
                    }

                    result = NumEx.nStrForLen(tempN, 4) + result;
                }
                else
                {
                    result = tmpresult[i] + result;
                }
            }
            result = flag + result;
            return result;
        }

    }
}