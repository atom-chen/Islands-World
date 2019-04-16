using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Coolape
{
    public static class CLASCIICode
    {

        static Dictionary<char, string> _AsciiCodeMap = null;
        public static Dictionary<char, string> AsciiCodeMap
        {
            get
            {
                if (_AsciiCodeMap == null)
                {
                    _AsciiCodeMap = new Dictionary<char, string>();
                    //string keyboardStrs = "`1234567890-=~!@#$%^&*()_+\tqwertyuiop[]\\asdfghjkl;'zxcvbnm,./QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>? ";
                    //string str = "";
                    //Debug.LogError(keyboardStrs.Length);
                    //for (int i = 0; i < keyboardStrs.Length; i++)
                    //{
                    //    _AsciiCodeMap[keyboardStrs[i]] = NumEx.nStrForLen((int)(keyboardStrs[i]), 4);
                    //    str = PStr.b().a(str).a("_AsciiCodeMap.Add(\'").a(keyboardStrs[i]).a("', \"").a(NumEx.nStrForLen((int)(keyboardStrs[i]), 4)).a("\");\n").e();

                    //}
                    //Debug.LogError(str);

                    _AsciiCodeMap.Add('`', "0096");
                    _AsciiCodeMap.Add('1', "0049");
                    _AsciiCodeMap.Add('2', "0050");
                    _AsciiCodeMap.Add('3', "0051");
                    _AsciiCodeMap.Add('4', "0052");
                    _AsciiCodeMap.Add('5', "0053");
                    _AsciiCodeMap.Add('6', "0054");
                    _AsciiCodeMap.Add('7', "0055");
                    _AsciiCodeMap.Add('8', "0056");
                    _AsciiCodeMap.Add('9', "0057");
                    _AsciiCodeMap.Add('0', "0048");
                    _AsciiCodeMap.Add('-', "0045");
                    _AsciiCodeMap.Add('=', "0061");
                    _AsciiCodeMap.Add('~', "0126");
                    _AsciiCodeMap.Add('!', "0033");
                    _AsciiCodeMap.Add('@', "0064");
                    _AsciiCodeMap.Add('#', "0035");
                    _AsciiCodeMap.Add('$', "0036");
                    _AsciiCodeMap.Add('%', "0037");
                    _AsciiCodeMap.Add('^', "0094");
                    _AsciiCodeMap.Add('&', "0038");
                    _AsciiCodeMap.Add('*', "0042");
                    _AsciiCodeMap.Add('(', "0040");
                    _AsciiCodeMap.Add(')', "0041");
                    _AsciiCodeMap.Add('_', "0095");
                    _AsciiCodeMap.Add('+', "0043");
                    _AsciiCodeMap.Add('\t', "0009");
                    _AsciiCodeMap.Add('q', "0113");
                    _AsciiCodeMap.Add('w', "0119");
                    _AsciiCodeMap.Add('e', "0101");
                    _AsciiCodeMap.Add('r', "0114");
                    _AsciiCodeMap.Add('t', "0116");
                    _AsciiCodeMap.Add('y', "0121");
                    _AsciiCodeMap.Add('u', "0117");
                    _AsciiCodeMap.Add('i', "0105");
                    _AsciiCodeMap.Add('o', "0111");
                    _AsciiCodeMap.Add('p', "0112");
                    _AsciiCodeMap.Add('[', "0091");
                    _AsciiCodeMap.Add(']', "0093");
                    _AsciiCodeMap.Add('\\', "0092");
                    _AsciiCodeMap.Add('a', "0097");
                    _AsciiCodeMap.Add('s', "0115");
                    _AsciiCodeMap.Add('d', "0100");
                    _AsciiCodeMap.Add('f', "0102");
                    _AsciiCodeMap.Add('g', "0103");
                    _AsciiCodeMap.Add('h', "0104");
                    _AsciiCodeMap.Add('j', "0106");
                    _AsciiCodeMap.Add('k', "0107");
                    _AsciiCodeMap.Add('l', "0108");
                    _AsciiCodeMap.Add(';', "0059");
                    _AsciiCodeMap.Add('\'', "0039");
                    _AsciiCodeMap.Add('z', "0122");
                    _AsciiCodeMap.Add('x', "0120");
                    _AsciiCodeMap.Add('c', "0099");
                    _AsciiCodeMap.Add('v', "0118");
                    _AsciiCodeMap.Add('b', "0098");
                    _AsciiCodeMap.Add('n', "0110");
                    _AsciiCodeMap.Add('m', "0109");
                    _AsciiCodeMap.Add(',', "0044");
                    _AsciiCodeMap.Add('.', "0046");
                    _AsciiCodeMap.Add('/', "0047");
                    _AsciiCodeMap.Add('Q', "0081");
                    _AsciiCodeMap.Add('W', "0087");
                    _AsciiCodeMap.Add('E', "0069");
                    _AsciiCodeMap.Add('R', "0082");
                    _AsciiCodeMap.Add('T', "0084");
                    _AsciiCodeMap.Add('Y', "0089");
                    _AsciiCodeMap.Add('U', "0085");
                    _AsciiCodeMap.Add('I', "0073");
                    _AsciiCodeMap.Add('O', "0079");
                    _AsciiCodeMap.Add('P', "0080");
                    _AsciiCodeMap.Add('{', "0123");
                    _AsciiCodeMap.Add('}', "0125");
                    _AsciiCodeMap.Add('|', "0124");
                    _AsciiCodeMap.Add('A', "0065");
                    _AsciiCodeMap.Add('S', "0083");
                    _AsciiCodeMap.Add('D', "0068");
                    _AsciiCodeMap.Add('F', "0070");
                    _AsciiCodeMap.Add('G', "0071");
                    _AsciiCodeMap.Add('H', "0072");
                    _AsciiCodeMap.Add('J', "0074");
                    _AsciiCodeMap.Add('K', "0075");
                    _AsciiCodeMap.Add('L', "0076");
                    _AsciiCodeMap.Add(':', "0058");
                    _AsciiCodeMap.Add('"', "0034");
                    _AsciiCodeMap.Add('Z', "0090");
                    _AsciiCodeMap.Add('X', "0088");
                    _AsciiCodeMap.Add('C', "0067");
                    _AsciiCodeMap.Add('V', "0086");
                    _AsciiCodeMap.Add('B', "0066");
                    _AsciiCodeMap.Add('N', "0078");
                    _AsciiCodeMap.Add('M', "0077");
                    _AsciiCodeMap.Add('<', "0060");
                    _AsciiCodeMap.Add('>', "0062");
                    _AsciiCodeMap.Add('?', "0063");
                    _AsciiCodeMap.Add(' ', "0032");

                }
                return _AsciiCodeMap;
            }
        }

        public static string getCode(char key)
        {
            if (AsciiCodeMap.ContainsKey(key))
            {
                return AsciiCodeMap[key];
            } else {
                return null;
            }
        }
    }
}
