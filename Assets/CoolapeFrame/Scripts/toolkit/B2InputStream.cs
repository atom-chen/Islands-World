using System;
using System.IO;
using System.Collections;
using System.Text;

namespace Coolape
{
	public class Ref
	{
		public int val;
	}

	public class B2InputStream
	{
		public static int ReadByte (Stream s)
		{
//			return s.ReadByte ();
			sbyte v = (sbyte)(s.ReadByte ());
			return (int)v;
		}

		public static int readInt (byte[] b, Ref rf)
		{
			int tag = (sbyte)b [0];
			switch (tag) {
			case B2Type.INT_N1:
				return -1;
			case B2Type.INT_0:
				return 0;
			case B2Type.INT_1:
				return 1;
			case B2Type.INT_2:
				return 2;
			case B2Type.INT_3:
				return 3;
			case B2Type.INT_4:
				return 4;
			case B2Type.INT_5:
				return 5;
			case B2Type.INT_6:
				return 6;
			case B2Type.INT_7:
				return 7;
			case B2Type.INT_8:
				return 8;
			case B2Type.INT_9:
				return 9;
			case B2Type.INT_10:
				return 10;
			case B2Type.INT_11:
				return 11;
			case B2Type.INT_12:
				return 12;
			case B2Type.INT_13:
				return 13;
			case B2Type.INT_14:
				return 14;
			case B2Type.INT_15:
				return 15;
			case B2Type.INT_16:
				return 16;
			case B2Type.INT_17:
				return 17;
			case B2Type.INT_18:
				return 18;
			case B2Type.INT_19:
				return 19;
			case B2Type.INT_20:
				return 20;
			case B2Type.INT_21:
				return 21;
			case B2Type.INT_22:
				return 22;
			case B2Type.INT_23:
				return 23;
			case B2Type.INT_24:
				return 24;
			case B2Type.INT_25:
				return 25;
			case B2Type.INT_26:
				return 26;
			case B2Type.INT_27:
				return 27;
			case B2Type.INT_28:
				return 28;
			case B2Type.INT_29:
				return 29;
			case B2Type.INT_30:
				return 30;
			case B2Type.INT_31:
				return 31;
			case B2Type.INT_32:
				return 32;
			case B2Type.INT_8B:
				{
					rf.val = 1;
					return 0;
				}
			case B2Type.INT_16B:
				{
					rf.val = 2;
					return 0;
				}
			case B2Type.INT_32B:
				{
					rf.val = 4;
					return 0;
				}
			default:
				//throw new IOException("read int tag error:" + tag);
				break;
			}
			return 0;
		}

		public static int byte2Int (byte[] b, Ref rf)
		{
			if (rf.val == 1) {
				sbyte v = (sbyte)b [0];
				return (int)v;
			} else if (rf.val == 2) {
				Int16 v = (Int16)(((b [0] & 0xff) << 8) + ((b [1] & 0xff) << 0));
				return v;
			} else if (rf.val == 3) {
				Int32 value1 = b [0];
				Int32 value2 = b [1];
				Int32 value3 = b [2];
				Int32 value4 = b [3];
	
				Int32 v = (Int32)(((value1 & 0xff) << 24) + ((value2 & 0xff) << 16)
				          + ((value3 & 0xff) << 8) + ((value4 & 0xff) << 0));
				return v;
			}
			return 0;
		}

		public static int readInt (Stream s)
		{
			int tag = ReadByte (s);
			switch (tag) {
			case B2Type.INT_N1:
				return -1;
			case B2Type.INT_0:
				return 0;
			case B2Type.INT_1:
				return 1;
			case B2Type.INT_2:
				return 2;
			case B2Type.INT_3:
				return 3;
			case B2Type.INT_4:
				return 4;
			case B2Type.INT_5:
				return 5;
			case B2Type.INT_6:
				return 6;
			case B2Type.INT_7:
				return 7;
			case B2Type.INT_8:
				return 8;
			case B2Type.INT_9:
				return 9;
			case B2Type.INT_10:
				return 10;
			case B2Type.INT_11:
				return 11;
			case B2Type.INT_12:
				return 12;
			case B2Type.INT_13:
				return 13;
			case B2Type.INT_14:
				return 14;
			case B2Type.INT_15:
				return 15;
			case B2Type.INT_16:
				return 16;
			case B2Type.INT_17:
				return 17;
			case B2Type.INT_18:
				return 18;
			case B2Type.INT_19:
				return 19;
			case B2Type.INT_20:
				return 20;
			case B2Type.INT_21:
				return 21;
			case B2Type.INT_22:
				return 22;
			case B2Type.INT_23:
				return 23;
			case B2Type.INT_24:
				return 24;
			case B2Type.INT_25:
				return 25;
			case B2Type.INT_26:
				return 26;
			case B2Type.INT_27:
				return 27;
			case B2Type.INT_28:
				return 28;
			case B2Type.INT_29:
				return 29;
			case B2Type.INT_30:
				return 30;
			case B2Type.INT_31:
				return 31;
			case B2Type.INT_32:
				return 32;
			case B2Type.INT_8B:
				{
					return ReadByte (s);
				}
			case B2Type.INT_16B:
				{
					Int16 v = (Int16)(((ReadByte (s) & 0xff) << 8) + ((ReadByte (s) & 0xff) << 0));
					return v;
				}
			case B2Type.INT_32B:
				{
					Int32 value1 = ReadByte (s);
					Int32 value2 = ReadByte (s);
					Int32 value3 = ReadByte (s);
					Int32 value4 = ReadByte (s);

					Int32 v = (Int32)(((value1 & 0xff) << 24) + ((value2 & 0xff) << 16)
					          + ((value3 & 0xff) << 8) + ((value4 & 0xff) << 0));
					return v;
				}
			default:
                    //throw new IOException("read int tag error:" + tag);
				break;
			}
			return 0;
		}

		private static int[] readIntArray (Stream s, int len)
		{
			int[] ret = new int[len];
			for (int i = 0; i < len; i++) {
				int v = readInt (s);
				ret [i] = v;
			}
			return ret;
		}

		private static NewList readList (Stream s, int len)
		{
			NewList ret = new NewList ();
			for (int i = 0; i < len; i++) {
				Object o = readObject (s);
				// ret.addElement(o);
				ret.Add (o);
			}
			return ret;
		}

		private static NewMap readMap (Stream s, int len)
		{
			NewMap ret = new NewMap ();
			for (int i = 0; i < len; i++) {
				Object key = readObject (s);
				Object val = readObject (s);
				ret [key] = val;
			}
			return ret;
	
		}

		public static Object readObject (Stream s)
		{
			int tag = ReadByte (s);
			switch (tag) {
			case B2Type.NULL:
				{
					return null;
				}
			case B2Type.HASHTABLE_0:
				{
					return new NewMap ();
				}
			case B2Type.HASHTABLE_1:
				{
					return readMap (s, 1);
				}
			case B2Type.HASHTABLE_2:
				{
					return readMap (s, 2);
				}
			case B2Type.HASHTABLE_3:
				{
					return readMap (s, 3);
				}
			case B2Type.HASHTABLE_4:
				{
					return readMap (s, 4);
				}
			case B2Type.HASHTABLE_5:
				{
					return readMap (s, 5);
				}
			case B2Type.HASHTABLE_6:
				{
					return readMap (s, 6);
				}
			case B2Type.HASHTABLE_7:
				{
					return readMap (s, 7);
				}
			case B2Type.HASHTABLE_8:
				{
					return readMap (s, 8);
				}
			case B2Type.HASHTABLE_9:
				{
					return readMap (s, 9);
				}
			case B2Type.HASHTABLE_10:
				{
					return readMap (s, 10);
				}
			case B2Type.HASHTABLE_11:
				{
					return readMap (s, 11);
				}
			case B2Type.HASHTABLE_12:
				{
					return readMap (s, 12);
				}
			case B2Type.HASHTABLE_13:
				{
					return readMap (s, 13);
				}
			case B2Type.HASHTABLE_14:
				{
					return readMap (s, 14);
				}
			case B2Type.HASHTABLE_15:
				{
					return readMap (s, 15);
				}
			case B2Type.HASHTABLE:
				{
					int len = readInt (s);
					return readMap (s, len);
				}
			case B2Type.INT_N1:
				{
					return (int)-1;
				}
			case B2Type.INT_0:
				{
					return (int)0;
				}
			case B2Type.INT_1:
				{
					return (int)1;
				}
			case B2Type.INT_2:
				{
					return (int)2;
				}
			case B2Type.INT_3:
				{
					return (int)3;
				}
			case B2Type.INT_4:
				{
					return (int)4;
				}
			case B2Type.INT_5:
				{
					return (int)5;
				}
			case B2Type.INT_6:
				{
					return (int)6;
				}
			case B2Type.INT_7:
				{
					return (int)7;
				}
			case B2Type.INT_8:
				{
					return (int)8;
				}
			case B2Type.INT_9:
				{
					return (int)9;
				}
			case B2Type.INT_10:
				{
					return (int)10;
				}
			case B2Type.INT_11:
				{
					return (int)11;
				}
			case B2Type.INT_12:
				{
					return (int)12;
				}
			case B2Type.INT_13:
				{
					return (int)13;
				}
			case B2Type.INT_14:
				{
					return (int)14;
				}
			case B2Type.INT_15:
				{
					return (int)15;
				}
			case B2Type.INT_16:
				{
					return (int)16;
				}
			case B2Type.INT_17:
				{
					return (int)17;
				}
			case B2Type.INT_18:
				{
					return (int)18;
				}
			case B2Type.INT_19:
				{
					return (int)19;
				}
			case B2Type.INT_20:
				{
					return (int)20;
				}
			case B2Type.INT_21:
				{
					return (int)21;
				}
			case B2Type.INT_22:
				{
					return (int)22;
				}
			case B2Type.INT_23:
				{
					return (int)23;
				}
			case B2Type.INT_24:
				{
					return (int)24;
				}
			case B2Type.INT_25:
				{
					return (int)25;
				}
			case B2Type.INT_26:
				{
					return (int)26;
				}
			case B2Type.INT_27:
				{
					return (int)27;
				}
			case B2Type.INT_28:
				{
					return (int)28;
				}
			case B2Type.INT_29:
				{
					return (int)29;
				}
			case B2Type.INT_30:
				{
					return (int)30;
				}
			case B2Type.INT_31:
				{
					return (int)31;
				}
			case B2Type.INT_32:
				{
					return (int)32;
				}
			case B2Type.INT_8B:
				{
					sbyte v = (sbyte)ReadByte (s);
					return (int)v;
				}
			case B2Type.INT_16B:
				{
					short v = (short)(((ReadByte (s) & 0xff) << 8) + ((ReadByte (s) & 0xff) << 0));
					return (int)v;
				}
			case B2Type.INT_32B:
				{
					int v1 = ReadByte (s);
					int v2 = ReadByte (s);
					int v3 = ReadByte (s);
					int v4 = ReadByte (s);
					int v = ((v1 & 0xff) << 24) + ((v2 & 0xff) << 16)
					        + ((v3 & 0xff) << 8) + ((v4 & 0xff) << 0);
					return (int)v;
				}
			case B2Type.STR_0:
				{
					return "";
				}
			case B2Type.STR_1:
				{
					return readStringImpl (s, 1);
				}
			case B2Type.STR_2:
				{
					return readStringImpl (s, 2);
				}
			case B2Type.STR_3:
				{
					return readStringImpl (s, 3);
				}
			case B2Type.STR_4:
				{
					return readStringImpl (s, 4);
				}
			case B2Type.STR_5:
				{
					return readStringImpl (s, 5);
				}
			case B2Type.STR_6:
				{
					return readStringImpl (s, 6);
				}
			case B2Type.STR_7:
				{
					return readStringImpl (s, 7);
				}
			case B2Type.STR_8:
				{
					return readStringImpl (s, 8);
				}
			case B2Type.STR_9:
				{
					return readStringImpl (s, 9);
				}
			case B2Type.STR_10:
				{
					return readStringImpl (s, 10);
				}
			case B2Type.STR_11:
				{
					return readStringImpl (s, 11);
				}
			case B2Type.STR_12:
				{
					return readStringImpl (s, 12);
				}
			case B2Type.STR_13:
				{
					return readStringImpl (s, 13);
				}
			case B2Type.STR_14:
				{
					return readStringImpl (s, 14);
				}
			case B2Type.STR_15:
				{
					return readStringImpl (s, 15);
				}
			case B2Type.STR_16:
				{
					return readStringImpl (s, 16);
				}
			case B2Type.STR_17:
				{
					return readStringImpl (s, 17);
				}
			case B2Type.STR_18:
				{
					return readStringImpl (s, 18);
				}
			case B2Type.STR_19:
				{
					return readStringImpl (s, 19);
				}
			case B2Type.STR_20:
				{
					return readStringImpl (s, 20);
				}
			case B2Type.STR_21:
				{
					return readStringImpl (s, 21);
				}
			case B2Type.STR_22:
				{
					return readStringImpl (s, 22);
				}
			case B2Type.STR_23:
				{
					return readStringImpl (s, 23);
				}
			case B2Type.STR_24:
				{
					return readStringImpl (s, 24);
				}
			case B2Type.STR_25:
				{
					return readStringImpl (s, 25);
				}
			case B2Type.STR_26:
				{
					return readStringImpl (s, 26);
				}
			case B2Type.STR:
				{
					int len = readInt (s);
					return readStringImpl (s, len);
				}
			case B2Type.BOOLEAN_TRUE:
				{
					return true;
				}
			case B2Type.BOOLEAN_FALSE:
				{
					return false;
				}
			case B2Type.BYTE_0:
				{
					byte v = 0;
					return v;
				}
			case B2Type.BYTE:
				{
					byte v = (byte)ReadByte (s);
					return v;
				}
			case B2Type.BYTES_0:
				{
					return new byte[0];
				}
			case B2Type.BYTES:
				{
					int len = readInt (s);
					byte[] b = new byte[len];
					s.Read (b, 0, len);
					return b;
				}
			case B2Type.VECTOR_0:
				{
					return new NewList ();
				}
			case B2Type.VECTOR_1:
				{
					return readList (s, 1);
				}
			case B2Type.VECTOR_2:
				{
					return readList (s, 2);
				}
			case B2Type.VECTOR_3:
				{
					return readList (s, 3);
				}
			case B2Type.VECTOR_4:
				{
					return readList (s, 4);
				}
			case B2Type.VECTOR_5:
				{
					return readList (s, 5);
				}
			case B2Type.VECTOR_6:
				{
					return readList (s, 6);
				}
			case B2Type.VECTOR_7:
				{
					return readList (s, 7);
				}
			case B2Type.VECTOR_8:
				{
					return readList (s, 8);
				}
			case B2Type.VECTOR_9:
				{
					return readList (s, 9);
				}
			case B2Type.VECTOR_10:
				{
					return readList (s, 10);
				}
			case B2Type.VECTOR_11:
				{
					return readList (s, 11);
				}
			case B2Type.VECTOR_12:
				{
					return readList (s, 12);
				}
			case B2Type.VECTOR_13:
				{
					return readList (s, 13);
				}
			case B2Type.VECTOR_14:
				{
					return readList (s, 14);
				}
			case B2Type.VECTOR_15:
				{
					return readList (s, 15);
				}
			case B2Type.VECTOR_16:
				{
					return readList (s, 16);
				}
			case B2Type.VECTOR_17:
				{
					return readList (s, 17);
				}
			case B2Type.VECTOR_18:
				{
					return readList (s, 18);
				}
			case B2Type.VECTOR_19:
				{
					return readList (s, 19);
				}
			case B2Type.VECTOR_20:
				{
					return readList (s, 20);
				}
			case B2Type.VECTOR_21:
				{
					return readList (s, 21);
				}
			case B2Type.VECTOR_22:
				{
					return readList (s, 22);
				}
			case B2Type.VECTOR_23:
				{
					return readList (s, 23);
				}
			case B2Type.VECTOR_24:
				{
					return readList (s, 24);
				}
			case B2Type.VECTOR:
				{
					int len = readInt (s);
					return readList (s, len);
				}
			case B2Type.SHORT_0:
				{
					short v = 0;
					return v;
				}
			case B2Type.SHORT_8B:
				{
					short v = (short)ReadByte (s);
					return v;
				}
			case B2Type.SHORT_16B:
				{
					short v = (short)(((ReadByte (s) & 0xff) << 8) + ((ReadByte (s) & 0xff) << 0));
					return v;
				}
			case B2Type.LONG_0:
				{
					int v = 0;
					return (long)v;
				}
			case B2Type.LONG_8B:
				{
					int v = ReadByte (s);
					return (long)v;
				}
			case B2Type.LONG_16B:
				{
					int v = (((ReadByte (s) & 0xff) << 8) + ((ReadByte (s) & 0xff) << 0));
					if (v > 32767) {
						v = v - 65536;
					}
					return (long)(v);
				}
			case B2Type.LONG_32B:
				{
					int v1 = ReadByte (s);
					int v2 = ReadByte (s);
					int v3 = ReadByte (s);
					int v4 = ReadByte (s);
					long v = ((v1 & 0xff) << 24) + ((v2 & 0xff) << 16)
					        + ((v3 & 0xff) << 8) + ((v4 & 0xff) << 0);
					if (v > 2147483647) {
						v = v - 4294967296;
					}
					return (long)(v);
				}
			case B2Type.LONG_64B:
				{
					byte[] b = new byte[8];
					for (int i = 0; i < 8; i++) {
						b [i] = (byte)ReadByte (s);
					}
					long high = ((b [0] & 0xff) << 24) + ((b [1] & 0xff) << 16)
					            + ((b [2] & 0xff) << 8) + ((b [3] & 0xff) << 0);
					long low = ((b [4] & 0xff) << 24) + ((b [5] & 0xff) << 16)
					           + ((b [6] & 0xff) << 8) + ((b [7] & 0xff) << 0);
					long v = (high << 32) + (0xffffffffL & low);
					return (long)(v);
				}
			case B2Type.DOUBLE_0:
				{
					// int v = 0;
					// double ret = Double.longBitsToDouble(v);
					return (double)(0);
					// }case B2Type.DOUBLE_8B: {
					// int v = is.read();
					// double ret = Double.longBitsToDouble(v);
					// return new Double(ret);
					// }case B2Type.DOUBLE_16B: {
					// int v = (((is.read() & 0xff) << 8) + ((is.read() & 0xff) << 0));
					// double ret = Double.longBitsToDouble(v);
					// return new Double(ret);
					// }case B2Type.DOUBLE_32B: {
					// int v1 = is.read();
					// int v2 = is.read();
					// int v3 = is.read();
					// int v4 = is.read();
					//
					// int v = ((v1 & 0xff) << 24) + ((v2 & 0xff) << 16)
					// + ((v3 & 0xff) << 8) + ((v4 & 0xff) << 0);
					// double ret = Double.longBitsToDouble(v);
					// return new Double(ret);
				}
			case B2Type.DOUBLE_64B:
				{
					byte[] b = new byte[8];
					for (int i = 0; i < 8; i++) {
						b [i] = (byte)ReadByte (s);
					}
					long high = ((b [0] & 0xff) << 24) + ((b [1] & 0xff) << 16)
					            + ((b [2] & 0xff) << 8) + ((b [3] & 0xff) << 0);
					long low = ((b [4] & 0xff) << 24) + ((b [5] & 0xff) << 16)
					           + ((b [6] & 0xff) << 8) + ((b [7] & 0xff) << 0);
					long v = (high << 32) + (0xffffffffL & low);
					double ret = NumEx.Int64BitsToDouble (v);
					return (double)(ret);
				}
			case B2Type.INT_ARRAY_0:
				{
					return new int[0];
				}
			case B2Type.INT_ARRAY_1:
				{
					return readIntArray (s, 1);
				}
			case B2Type.INT_ARRAY_2:
				{
					return readIntArray (s, 2);
				}
			case B2Type.INT_ARRAY_3:
				{
					return readIntArray (s, 3);
				}
			case B2Type.INT_ARRAY_4:
				{
					return readIntArray (s, 4);
				}
			case B2Type.INT_ARRAY_5:
				{
					return readIntArray (s, 5);
				}
			case B2Type.INT_ARRAY_6:
				{
					return readIntArray (s, 6);
				}
			case B2Type.INT_ARRAY_7:
				{
					return readIntArray (s, 7);
				}
			case B2Type.INT_ARRAY_8:
				{
					return readIntArray (s, 8);
				}
			case B2Type.INT_ARRAY_9:
				{
					return readIntArray (s, 9);
				}
			case B2Type.INT_ARRAY_10:
				{
					return readIntArray (s, 10);
				}
			case B2Type.INT_ARRAY_11:
				{
					return readIntArray (s, 11);
				}
			case B2Type.INT_ARRAY_12:
				{
					return readIntArray (s, 12);
				}
			case B2Type.INT_ARRAY_13:
				{
					return readIntArray (s, 13);
				}
			case B2Type.INT_ARRAY_14:
				{
					return readIntArray (s, 14);
				}
			case B2Type.INT_ARRAY_15:
				{
					return readIntArray (s, 15);
				}
			case B2Type.INT_ARRAY_16:
				{
					return readIntArray (s, 16);
				}
			case B2Type.INT_ARRAY:
				{
					int len = readInt (s);
					return readIntArray (s, len);
					//}case B2Type.INT_2D_ARRAY_0: {
					//	return new int[0][0];
					//}case B2Type.INT_2D_ARRAY: {
					//	int len = readInt(s);
					//	return readInt2DArray(s,  len);
				}
			case B2Type.INT_B2:
				{
					return readInt (s);
				}
			default:
							//throw new IOException("unknow tag error:" + tag);
				UnityEngine.Debug.LogError ("bio2 unknon type:" + tag);
				break;
			}
			return 0;
		}

		
		// //////////////////////////////////
		private static String readStringImpl (Stream s, int length)
		{
			byte[] b = new byte[length];
			s.Read (b, 0, length);
			return Encoding.UTF8.GetString (b);
		}

		private static String readStringImpl2 (Stream s, int length)
		{
			byte[] b = new byte[length];
			s.Read (b, 0, length);
			for (int n = 0; n < length; n++)
				b [n] = (byte)(b [n] ^ B2Type.STR);
			return Encoding.UTF8.GetString (b);
		}

		/*
		private static String readStringImpl(Stream s, int length) {
			StringBuilder sb = new StringBuilder();
	
			for (int i = 0; i < length; i++) {
				int ch = ReadByte(s);
	
				if (ch < 0x80)
					sb.Append((char) ch);
				else if ((ch & 0xe0) == 0xc0) {
					int ch1 = ReadByte(s);
					int v = ((ch & 0x1f) << 6) + (ch1 & 0x3f);
	
					sb.Append((char) v);
				} else if ((ch & 0xf0) == 0xe0) {
					int ch1 = ReadByte(s);
					int ch2 = ReadByte(s);
					int v = ((ch & 0x0f) << 12) + ((ch1 & 0x3f) << 6)
							+ (ch2 & 0x3f);
	
					sb.Append((char) v);
				} else{
					//throw new IOException("bad utf-8 encoding");
				}
			}
	
			return sb.ToString();
		}
         * */
	}
}

