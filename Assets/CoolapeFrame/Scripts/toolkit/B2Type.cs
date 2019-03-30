using System;
using System.IO;
using System.Collections;

namespace Coolape
{
	public class B2Type
	{
		//null
		public const int NULL = 0;
		//bool
		public const int BOOLEAN_TRUE = 1;
		public const int BOOLEAN_FALSE = 2;
		// byte
		public const int BYTE_0 = 3;
		public const int BYTE = 4;
		// short
		public const int SHORT_0 = 5;
		public const int SHORT_8B = 6;
		public const int SHORT_16B = 7;
		//int b32 b24 b16 b8
		public const int INT_0 = 8;
		public const int INT_8B = 9;
		public const int INT_16B = 10;
		public const int INT_32B = 11;
		public const int INT_N1 = 12;
		public const int INT_1 = 13;
		public const int INT_2 = 14;
		public const int INT_3 = 15;
		public const int INT_4 = 16;
		public const int INT_5 = 17;
		public const int INT_6 = 18;
		public const int INT_7 = 19;
		public const int INT_8 = 20;
		public const int INT_9 = 21;
		public const int INT_10 = 22;
		public const int INT_11 = 23;
		public const int INT_12 = 24;
		public const int INT_13 = 25;
		public const int INT_14 = 26;
		public const int INT_15 = 27;
		public const int INT_16 = 28;
		public const int INT_17 = 29;
		public const int INT_18 = 30;
		public const int INT_19 = 31;
		public const int INT_20 = 32;
		public const int INT_21 = 33;
		public const int INT_22 = 34;
		public const int INT_23 = 35;
		public const int INT_24 = 36;
		public const int INT_25 = 37;
		public const int INT_26 = 38;
		public const int INT_27 = 39;
		public const int INT_28 = 40;
		public const int INT_29 = 41;
		public const int INT_30 = 42;
		public const int INT_31 = 43;
		public const int INT_32 = 44;
		//long b64 b56 b48 b40 b32 b24 b16 b8
		public const int LONG_0 = 45;
		public const int LONG_8B = 46;
		public const int LONG_16B = 47;
		public const int LONG_32B = 48;
		public const int LONG_64B = 49;
		//double b64 b56 b48 b40 b32 b24 b16 b8
		public const int DOUBLE_0 = 50;
		//	public const int DOUBLE_8B = 51;
		//	public const int DOUBLE_16B = 52;
		//	public const int DOUBLE_32B = 53;
		public const int DOUBLE_64B = 54;
		//STR [bytes]
		public const int STR_0 = 55;
		public const int STR = 56;
		public const int STR_1 = 57;
		public const int STR_2 = 58;
		public const int STR_3 = 59;
		public const int STR_4 = 60;
		public const int STR_5 = 61;
		public const int STR_6 = 62;
		public const int STR_7 = 63;
		public const int STR_8 = 64;
		public const int STR_9 = 65;
		public const int STR_10 = 66;
		public const int STR_11 = 67;
		public const int STR_12 = 68;
		public const int STR_13 = 69;
		public const int STR_14 = 70;
		public const int STR_15 = 71;
		public const int STR_16 = 72;
		public const int STR_17 = 73;
		public const int STR_18 = 74;
		public const int STR_19 = 75;
		public const int STR_20 = 76;
		public const int STR_21 = 77;
		public const int STR_22 = 78;
		public const int STR_23 = 79;
		public const int STR_24 = 80;
		public const int STR_25 = 81;
		public const int STR_26 = 82;
		//Bytes [int len, byte[]]
		public const int BYTES_0 = 83;
		public const int BYTES = 84;
		//VECTOR [int len, v...]
		public const int VECTOR_0 = 85;
		public const int VECTOR = 86;
		public const int VECTOR_1 = 87;
		public const int VECTOR_2 = 88;
		public const int VECTOR_3 = 89;
		public const int VECTOR_4 = 90;
		public const int VECTOR_5 = 91;
		public const int VECTOR_6 = 92;
		public const int VECTOR_7 = 93;
		public const int VECTOR_8 = 94;
		public const int VECTOR_9 = 95;
		public const int VECTOR_10 = 96;
		public const int VECTOR_11 = 97;
		public const int VECTOR_12 = 98;
		public const int VECTOR_13 = 99;
		public const int VECTOR_14 = 100;
		public const int VECTOR_15 = 101;
		public const int VECTOR_16 = 102;
		public const int VECTOR_17 = 103;
		public const int VECTOR_18 = 104;
		public const int VECTOR_19 = 105;
		public const int VECTOR_20 = 106;
		public const int VECTOR_21 = 107;
		public const int VECTOR_22 = 108;
		public const int VECTOR_23 = 109;
		public const int VECTOR_24 = 110;
		//HASHTABLE [int len, k,v...]
		public const int HASHTABLE_0 = 111;
		public const int HASHTABLE = 112;
		public const int HASHTABLE_1 = 113;
		public const int HASHTABLE_2 = 114;
		public const int HASHTABLE_3 = 115;
		public const int HASHTABLE_4 = 116;
		public const int HASHTABLE_5 = 117;
		public const int HASHTABLE_6 = 118;
		public const int HASHTABLE_7 = 119;
		public const int HASHTABLE_8 = 120;
		public const int HASHTABLE_9 = 121;
		public const int HASHTABLE_10 = 122;
		public const int HASHTABLE_11 = 123;
		public const int HASHTABLE_12 = 124;
		public const int HASHTABLE_13 = 125;
		public const int HASHTABLE_14 = 126;
		public const int HASHTABLE_15 = 127;
		// int[]
		public const int INT_ARRAY = -9;
		public const int INT_ARRAY_0 = -10;
		public const int INT_ARRAY_1 = -11;
		public const int INT_ARRAY_2 = -12;
		public const int INT_ARRAY_3 = -13;
		public const int INT_ARRAY_4 = -14;
		public const int INT_ARRAY_5 = -15;
		public const int INT_ARRAY_6 = -16;
		public const int INT_ARRAY_7 = -17;
		public const int INT_ARRAY_8 = -18;
		public const int INT_ARRAY_9 = -19;
		public const int INT_ARRAY_10 = -20;
		public const int INT_ARRAY_11 = -21;
		public const int INT_ARRAY_12 = -22;
		public const int INT_ARRAY_13 = -23;
		public const int INT_ARRAY_14 = -24;
		public const int INT_ARRAY_15 = -25;
		public const int INT_ARRAY_16 = -26;
		// int[][]
		public const int INT_2D_ARRAY = -29;
		public const int INT_2D_ARRAY_0 = -30;

		public const int JAVA_DATE = -31;
		
		public const int JAVA_OBJECT = -32;
		//b2int
		public const int INT_B2 = -33;

		///////////////////////////////////////////////////////////////// 
		public static bool isMap (Object obj)
		{
			return obj is Hashtable;
		}

		public static bool isInt (Object obj)
		{
			return obj is int || obj is Int16 || obj is Int32;
		}

		public static bool isString (Object obj)
		{
			return obj is string || obj is String;
		}

		public static bool isBool (Object obj)
		{
			return obj is bool || obj is Boolean;
		}

		public static bool isByte (Object obj)
		{
			return obj is byte || obj is Byte;
		}

		public static bool isBytes (Object obj)
		{
			return obj is byte[];
		}

		public static bool isList (Object obj)
		{
			return obj is ArrayList;
		}

		public static bool isShort (Object obj)
		{
			return obj is short || obj is Int16;
		}

		public static bool isLong (Object obj)
		{
			return obj is long || obj is Int64;
		}

		public static bool isDouble (Object obj)
		{
			return obj is double || obj is Double;
		}

		public static bool isIntArray (Object obj)
		{
			return obj is int[];
		}

		public static bool isB2Int (Object obj)
		{
			return obj is B2Int;
		}
		/////////////////////////////////////////////////////////////////
		
	}
}

