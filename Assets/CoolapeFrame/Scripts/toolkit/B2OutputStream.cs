using System;
using System.IO;
using System.Collections;
using System.Text;

namespace Coolape
{
	public class B2OutputStream
	{
		public static byte[] toBytes (int v)
		{
			byte[] b;
			switch (v) {
			case -1:
				b = new byte[1];
				b [0] = B2Type.INT_N1;
				return b;
			case 0:
				b = new byte[1];
				b [0] = B2Type.INT_0;
				return b;
			case 1:
				b = new byte[1];
				b [0] = B2Type.INT_1;
				return b;
			case 2:
				b = new byte[1];
				b [0] = B2Type.INT_2;
				return b;
			case 3:
				b = new byte[1];
				b [0] = B2Type.INT_3;
				return b;
			case 4:
				b = new byte[1];
				b [0] = B2Type.INT_4;
				return b;
			case 5:
				b = new byte[1];
				b [0] = B2Type.INT_5;
				return b;
			case 6:
				b = new byte[1];
				b [0] = B2Type.INT_6;
				return b;
			case 7:
				b = new byte[1];
				b [0] = B2Type.INT_7;
				return b;
			case 8:
				b = new byte[1];
				b [0] = B2Type.INT_8;
				return b;
			case 9:
				b = new byte[1];
				b [0] = B2Type.INT_9;
				return b;
			case 10:
				b = new byte[1];
				b [0] = B2Type.INT_10;
				return b;
			case 11:
				b = new byte[1];
				b [0] = B2Type.INT_11;
				return b;
			case 12:
				b = new byte[1];
				b [0] = B2Type.INT_12;
				return b;
			case 13:
				b = new byte[1];
				b [0] = B2Type.INT_13;
				return b;
			case 14:
				b = new byte[1];
				b [0] = B2Type.INT_14;
				return b;
			case 15:
				b = new byte[1];
				b [0] = B2Type.INT_15;
				return b;
			case 16:
				b = new byte[1];
				b [0] = B2Type.INT_16;
				return b;
			case 17:
				b = new byte[1];
				b [0] = B2Type.INT_17;
				return b;
			case 18:
				b = new byte[1];
				b [0] = B2Type.INT_18;
				return b;
			case 19:
				b = new byte[1];
				b [0] = B2Type.INT_19;
				return b;
			case 20:
				b = new byte[1];
				b [0] = B2Type.INT_20;
				return b;
			case 21:
				b = new byte[1];
				b [0] = B2Type.INT_21;
				return b;
			case 22:
				b = new byte[1];
				b [0] = B2Type.INT_22;
				return b;
			case 23:
				b = new byte[1];
				b [0] = B2Type.INT_23;
				return b;
			case 24:
				b = new byte[1];
				b [0] = B2Type.INT_24;
				return b;
			case 25:
				b = new byte[1];
				b [0] = B2Type.INT_25;
				return b;
			case 26:
				b = new byte[1];
				b [0] = B2Type.INT_26;
				return b;
			case 27:
				b = new byte[1];
				b [0] = B2Type.INT_27;
				return b;
			case 28:
				b = new byte[1];
				b [0] = B2Type.INT_28;
				return b;
			case 29:
				b = new byte[1];
				b [0] = B2Type.INT_29;
				return b;
			case 30:
				b = new byte[1];
				b [0] = B2Type.INT_30;
				return b;
			case 31:
				b = new byte[1];
				b [0] = B2Type.INT_31;
				return b;
			case 32:
				b = new byte[1];
				b [0] = B2Type.INT_32;
				return b;
			default:
				//if (v >= Byte.MinValue && v <= Byte.MaxValue) {
				if (v >= -128 && v <= 127) {
					b = new byte[2];
					b [0] = B2Type.INT_8B;
					b [1] = (byte)((v) & 0xff);
					return b;
					//} else if (v >= Int16.MinValue && v <= Int16.MaxValue) {
				} else if (v >= -32768 && v <= 32767) {
					b = new byte[3];
					b [0] = B2Type.INT_16B;
					b [1] = (byte)((v >> 8) & 0xff);
					b [2] = (byte)((v >> 0) & 0xff);
					return b;
				} else {
					b = new byte[5];
					b [0] = B2Type.INT_32B;
					b [1] = (byte)((v >> 24) & 0xff);
					b [2] = (byte)((v >> 16) & 0xff);
					b [3] = (byte)((v >> 8) & 0xff);
					b [4] = (byte)((v >> 0) & 0xff);
					return b;
				}
			}
		}

		public static void WriteByte (Stream os, int v)
		{
			os.WriteByte ((byte)((v) & 0xff));
		}

		public static void WriteByte (Stream os, byte v)
		{
			os.WriteByte (v);
		}

		public static void writeNull (Stream os)
		{
			WriteByte (os, B2Type.NULL);
		}

		public static void writeBoolean (Stream os, bool v)
		{
			if (v)
				WriteByte (os, B2Type.BOOLEAN_TRUE);
			else
				WriteByte (os, B2Type.BOOLEAN_FALSE);
		}

		public static void writeByte (Stream os, int v)
		{
			if (v == 0)
				WriteByte (os, B2Type.BYTE_0);
			else {
				WriteByte (os, B2Type.BYTE);
				WriteByte (os, v);
			}
		}

		public static void writeShort (Stream os, int v)
		{
			if (v == 0)
				WriteByte (os, B2Type.SHORT_0);
			else if (v >= Byte.MinValue && v <= Byte.MaxValue) {
				WriteByte (os, B2Type.SHORT_8B);
				WriteByte (os, v);
			} else {
				WriteByte (os, B2Type.SHORT_16B);
				WriteByte (os, (byte)((v >> 8) & 0xff));
				WriteByte (os, (byte)((v >> 0) & 0xff));
			}
		}

		public static void writeB2Int (Stream os, B2Int v)
		{
			WriteByte (os, B2Type.INT_B2);
			writeInt (os, v.value);
		}

		public static void writeB2Int (Stream os, int v)
		{
			WriteByte (os, B2Type.INT_B2);
			writeInt (os, v);
		}

		public static void writeInt (Stream os, int v)
		{
			switch (v) {
			case -1:
				WriteByte (os, B2Type.INT_N1);
				break;
			case 0:
				WriteByte (os, B2Type.INT_0);
				break;
			case 1:
				WriteByte (os, B2Type.INT_1);
				break;
			case 2:
				WriteByte (os, B2Type.INT_2);
				break;
			case 3:
				WriteByte (os, B2Type.INT_3);
				break;
			case 4:
				WriteByte (os, B2Type.INT_4);
				break;
			case 5:
				WriteByte (os, B2Type.INT_5);
				break;
			case 6:
				WriteByte (os, B2Type.INT_6);
				break;
			case 7:
				WriteByte (os, B2Type.INT_7);
				break;
			case 8:
				WriteByte (os, B2Type.INT_8);
				break;
			case 9:
				WriteByte (os, B2Type.INT_9);
				break;
			case 10:
				WriteByte (os, B2Type.INT_10);
				break;
			case 11:
				WriteByte (os, B2Type.INT_11);
				break;
			case 12:
				WriteByte (os, B2Type.INT_12);
				break;
			case 13:
				WriteByte (os, B2Type.INT_13);
				break;
			case 14:
				WriteByte (os, B2Type.INT_14);
				break;
			case 15:
				WriteByte (os, B2Type.INT_15);
				break;
			case 16:
				WriteByte (os, B2Type.INT_16);
				break;
			case 17:
				WriteByte (os, B2Type.INT_17);
				break;
			case 18:
				WriteByte (os, B2Type.INT_18);
				break;
			case 19:
				WriteByte (os, B2Type.INT_19);
				break;
			case 20:
				WriteByte (os, B2Type.INT_20);
				break;
			case 21:
				WriteByte (os, B2Type.INT_21);
				break;
			case 22:
				WriteByte (os, B2Type.INT_22);
				break;
			case 23:
				WriteByte (os, B2Type.INT_23);
				break;
			case 24:
				WriteByte (os, B2Type.INT_24);
				break;
			case 25:
				WriteByte (os, B2Type.INT_25);
				break;
			case 26:
				WriteByte (os, B2Type.INT_26);
				break;
			case 27:
				WriteByte (os, B2Type.INT_27);
				break;
			case 28:
				WriteByte (os, B2Type.INT_28);
				break;
			case 29:
				WriteByte (os, B2Type.INT_29);
				break;
			case 30:
				WriteByte (os, B2Type.INT_30);
				break;
			case 31:
				WriteByte (os, B2Type.INT_31);
				break;
			case 32:
				WriteByte (os, B2Type.INT_32);
				break;
			default:
				//if (v >= Byte.MinValue && v <= Byte.MaxValue) {
				if (v >= -128 && v <= 127) {
					WriteByte (os, B2Type.INT_8B);
					WriteByte (os, v);
					//} else if (v >= Int16.MinValue && v <= Int16.MaxValue) {
				} else if (v >= -32768 && v <= 32767) {
					WriteByte (os, B2Type.INT_16B);
					WriteByte (os, (byte)((v >> 8) & 0xff));
					WriteByte (os, (byte)((v >> 0) & 0xff));
				} else {
					WriteByte (os, B2Type.INT_32B);
					WriteByte (os, (byte)((v >> 24) & 0xff));
					WriteByte (os, (byte)((v >> 16) & 0xff));
					WriteByte (os, (byte)((v >> 8) & 0xff));
					WriteByte (os, (byte)((v >> 0) & 0xff));
				}
				break;
			}
		}

		public static void writeIntArray (Stream os, int[] v)
		{
			int len = v.Length;
			switch (len) {
			case 0:
				WriteByte (os, B2Type.INT_ARRAY_0);
				break;
			case 1:
				WriteByte (os, B2Type.INT_ARRAY_1);
				break;
			case 2:
				WriteByte (os, B2Type.INT_ARRAY_2);
				break;
			case 3:
				WriteByte (os, B2Type.INT_ARRAY_3);
				break;
			case 4:
				WriteByte (os, B2Type.INT_ARRAY_4);
				break;
			case 5:
				WriteByte (os, B2Type.INT_ARRAY_5);
				break;
			case 6:
				WriteByte (os, B2Type.INT_ARRAY_6);
				break;
			case 7:
				WriteByte (os, B2Type.INT_ARRAY_7);
				break;
			case 8:
				WriteByte (os, B2Type.INT_ARRAY_8);
				break;
			case 9:
				WriteByte (os, B2Type.INT_ARRAY_9);
				break;
			case 10:
				WriteByte (os, B2Type.INT_ARRAY_10);
				break;
			case 11:
				WriteByte (os, B2Type.INT_ARRAY_11);
				break;
			case 12:
				WriteByte (os, B2Type.INT_ARRAY_12);
				break;
			case 13:
				WriteByte (os, B2Type.INT_ARRAY_13);
				break;
			case 14:
				WriteByte (os, B2Type.INT_ARRAY_14);
				break;
			case 15:
				WriteByte (os, B2Type.INT_ARRAY_15);
				break;
			case 16:
				WriteByte (os, B2Type.INT_ARRAY_16);
				break;
			default:
				WriteByte (os, B2Type.INT_ARRAY);
				writeInt (os, len);
				break;
			}
			for (int i = 0; i < len; i++) {
				writeInt (os, v [i]);
			}
		}

		public static void writeInt2DArray (Stream os, int[][] v)
		{
			int len = v.Length;
			if (len <= 0) {
				WriteByte (os, B2Type.INT_2D_ARRAY_0);
				return;
			}
			WriteByte (os, B2Type.INT_2D_ARRAY);
			writeInt (os, len);
			for (int i = 0; i < len; i++) {
				writeIntArray (os, v [i]);
			}
		}

		public static void writeLong (Stream os, long v)
		{
			if (v == 0) {
				WriteByte (os, B2Type.LONG_0);
				//} else if (v >= Byte.MinValue && v <= Byte.MaxValue) {
			} else if (v >= -128 && v <= 127) {
				WriteByte (os, B2Type.LONG_8B);
				WriteByte (os, (int)v);
				//} else if (v >= Int16.MinValue && v <= Int16.MaxValue) {
			} else if (v >= -32768 && v <= 32767) {
				WriteByte (os, B2Type.LONG_16B);
				WriteByte (os, (byte)((v >> 8) & 0xff));
				WriteByte (os, (byte)((v >> 0) & 0xff));
				//} else if (v >= Int32.MinValue && v <= Int32.MaxValue) {
			} else if (v >= -2147483648 && v <= 2147483647) {
				WriteByte (os, B2Type.LONG_32B);
				WriteByte (os, (byte)((v >> 24) & 0xff));
				WriteByte (os, (byte)((v >> 16) & 0xff));
				WriteByte (os, (byte)((v >> 8) & 0xff));
				WriteByte (os, (byte)((v >> 0) & 0xff));
			} else {
				WriteByte (os, B2Type.LONG_64B);
				WriteByte (os, (byte)((v >> 56) & 0xff));
				WriteByte (os, (byte)((v >> 48) & 0xff));
				WriteByte (os, (byte)((v >> 40) & 0xff));
				WriteByte (os, (byte)((v >> 32) & 0xff));
				WriteByte (os, (byte)((v >> 24) & 0xff));
				WriteByte (os, (byte)((v >> 16) & 0xff));
				WriteByte (os, (byte)((v >> 8) & 0xff));
				WriteByte (os, (byte)((v >> 0) & 0xff));
			}
		}

		public static void writeDouble (Stream os, double val)
		{
			long v = NumEx.DoubleToInt64Bits (val);
			//long v = Double.doubleToLongBits(var);
			if (v == 0) {
				WriteByte (os, B2Type.DOUBLE_0);
				//		} else if (v >= Byte.MIN_VALUE && v <= Byte.MAX_VALUE) {
				//			WriteByte(os, B2Type.DOUBLE_8B);
				//			WriteByte(os, (int) v);
				//		} else if (v >= Short.MIN_VALUE && v <= Short.MAX_VALUE) {
				//			WriteByte(os, B2Type.DOUBLE_16B);
				//			WriteByte(os, (byte) ((v >> 8) & 0xff));
				//			WriteByte(os, (byte) ((v >> 0) & 0xff));
				//		} else if (v >= Integer.MIN_VALUE && v <= Integer.MAX_VALUE) {
				//			WriteByte(os, B2Type.DOUBLE_32B);
				//			WriteByte(os, (byte) ((v >> 24) & 0xff));
				//			WriteByte(os, (byte) ((v >> 16) & 0xff));
				//			WriteByte(os, (byte) ((v >> 8) & 0xff));
				//			WriteByte(os, (byte) ((v >> 0) & 0xff));
			} else {
				WriteByte (os, B2Type.DOUBLE_64B);
				WriteByte (os, (byte)((v >> 56) & 0xff));
				WriteByte (os, (byte)((v >> 48) & 0xff));
				WriteByte (os, (byte)((v >> 40) & 0xff));
				WriteByte (os, (byte)((v >> 32) & 0xff));
				WriteByte (os, (byte)((v >> 24) & 0xff));
				WriteByte (os, (byte)((v >> 16) & 0xff));
				WriteByte (os, (byte)((v >> 8) & 0xff));
				WriteByte (os, (byte)((v >> 0) & 0xff));
			}
		}

		public static void writeString (Stream os, String v)
		{
			if (v == null) {
				writeNull (os);
			} else {
				byte[] b = Encoding.UTF8.GetBytes (v);
				int len = b.Length;
				switch (len) {
				case 0:
					WriteByte (os, B2Type.STR_0);
					break;
				case 1:
					WriteByte (os, B2Type.STR_1);
					printString (os, b);
					break;
				case 2:
					WriteByte (os, B2Type.STR_2);
					printString (os, b);
					break;
				case 3:
					WriteByte (os, B2Type.STR_3);
					printString (os, b);
					break;
				case 4:
					WriteByte (os, B2Type.STR_4);
					printString (os, b);
					break;
				case 5:
					WriteByte (os, B2Type.STR_5);
					printString (os, b);
					break;
				case 6:
					WriteByte (os, B2Type.STR_6);
					printString (os, b);
					break;
				case 7:
					WriteByte (os, B2Type.STR_7);
					printString (os, b);
					break;
				case 8:
					WriteByte (os, B2Type.STR_8);
					printString (os, b);
					break;
				case 9:
					WriteByte (os, B2Type.STR_9);
					printString (os, b);
					break;
				case 10:
					WriteByte (os, B2Type.STR_10);
					printString (os, b);
					break;
				case 11:
					WriteByte (os, B2Type.STR_11);
					printString (os, b);
					break;
				case 12:
					WriteByte (os, B2Type.STR_12);
					printString (os, b);
					break;
				case 13:
					WriteByte (os, B2Type.STR_13);
					printString (os, b);
					break;
				case 14:
					WriteByte (os, B2Type.STR_14);
					printString (os, b);
					break;
				case 15:
					WriteByte (os, B2Type.STR_15);
					printString (os, b);
					break;
				case 16:
					WriteByte (os, B2Type.STR_16);
					printString (os, b);
					break;
				case 17:
					WriteByte (os, B2Type.STR_17);
					printString (os, b);
					break;
				case 18:
					WriteByte (os, B2Type.STR_18);
					printString (os, b);
					break;
				case 19:
					WriteByte (os, B2Type.STR_19);
					printString (os, b);
					break;
				case 20:
					WriteByte (os, B2Type.STR_20);
					printString (os, b);
					break;
				case 21:
					WriteByte (os, B2Type.STR_21);
					printString (os, b);
					break;
				case 22:
					WriteByte (os, B2Type.STR_22);
					printString (os, b);
					break;
				case 23:
					WriteByte (os, B2Type.STR_23);
					printString (os, b);
					break;
				case 24:
					WriteByte (os, B2Type.STR_24);
					printString (os, b);
					break;
				case 25:
					WriteByte (os, B2Type.STR_25);
					printString (os, b);
					break;
				case 26:
					WriteByte (os, B2Type.STR_26);
					printString (os, b);
					break;
				default:
					WriteByte (os, B2Type.STR);
					writeInt (os, len);
					printString (os, b);
					break;
				}
			}
		}

		public static void writeBytes (Stream os, byte[] v)
		{
			if (v == null) {
				writeNull (os);
			} else {
				int len = v.Length;
				if (len == 0) {
					WriteByte (os, B2Type.BYTES_0);
				} else {
					WriteByte (os, B2Type.BYTES);
					writeInt (os, len);
					os.Write (v, 0, len);
				}
			}
		}

		public static void writeVector (Stream os, ArrayList v)
		{
			if (v == null) {
				writeNull (os);
			} else {
				int len = v.Count;
				switch (len) {
				case 0:
					WriteByte (os, B2Type.VECTOR_0);
					break;
				case 1:
					WriteByte (os, B2Type.VECTOR_1);
					break;
				case 2:
					WriteByte (os, B2Type.VECTOR_2);
					break;
				case 3:
					WriteByte (os, B2Type.VECTOR_3);
					break;
				case 4:
					WriteByte (os, B2Type.VECTOR_4);
					break;
				case 5:
					WriteByte (os, B2Type.VECTOR_5);
					break;
				case 6:
					WriteByte (os, B2Type.VECTOR_6);
					break;
				case 7:
					WriteByte (os, B2Type.VECTOR_7);
					break;
				case 8:
					WriteByte (os, B2Type.VECTOR_8);
					break;
				case 9:
					WriteByte (os, B2Type.VECTOR_9);
					break;
				case 10:
					WriteByte (os, B2Type.VECTOR_10);
					break;
				case 11:
					WriteByte (os, B2Type.VECTOR_11);
					break;
				case 12:
					WriteByte (os, B2Type.VECTOR_12);
					break;
				case 13:
					WriteByte (os, B2Type.VECTOR_13);
					break;
				case 14:
					WriteByte (os, B2Type.VECTOR_14);
					break;
				case 15:
					WriteByte (os, B2Type.VECTOR_15);
					break;
				case 16:
					WriteByte (os, B2Type.VECTOR_16);
					break;
				case 17:
					WriteByte (os, B2Type.VECTOR_17);
					break;
				case 18:
					WriteByte (os, B2Type.VECTOR_18);
					break;
				case 19:
					WriteByte (os, B2Type.VECTOR_19);
					break;
				case 20:
					WriteByte (os, B2Type.VECTOR_20);
					break;
				case 21:
					WriteByte (os, B2Type.VECTOR_21);
					break;
				case 22:
					WriteByte (os, B2Type.VECTOR_22);
					break;
				case 23:
					WriteByte (os, B2Type.VECTOR_23);
					break;
				case 24:
					WriteByte (os, B2Type.VECTOR_24);
					break;
				default:
					WriteByte (os, B2Type.VECTOR);
					writeInt (os, len);
					break;
				}
	
				for (int i = 0; i < len; i++) {
					//Object object = v.elementAt(i);
					Object obj = v [i];
					writeObject (os, obj);
				}
			}
		}

		public static void writeMap (Stream os, Hashtable v)
		{
			if (v == null) {
				writeNull (os);
			} else {
				int len = v.Count;
				switch (len) {
				case 0:
					WriteByte (os, B2Type.HASHTABLE_0);
					break;
				case 1:
					{
						WriteByte (os, B2Type.HASHTABLE_1);
						break;
					}
				case 2:
					{
						WriteByte (os, B2Type.HASHTABLE_2);
						break;
					}
				case 3:
					{
						WriteByte (os, B2Type.HASHTABLE_3);
						break;
					}
				case 4:
					{
						WriteByte (os, B2Type.HASHTABLE_4);
						break;
					}
				case 5:
					{
						WriteByte (os, B2Type.HASHTABLE_5);
						break;
					}
				case 6:
					{
						WriteByte (os, B2Type.HASHTABLE_6);
						break;
					}
				case 7:
					{
						WriteByte (os, B2Type.HASHTABLE_7);
						break;
					}
				case 8:
					{
						WriteByte (os, B2Type.HASHTABLE_8);
						break;
					}
				case 9:
					{
						WriteByte (os, B2Type.HASHTABLE_9);
						break;
					}
				case 10:
					{
						WriteByte (os, B2Type.HASHTABLE_10);
						break;
					}
				case 11:
					{
						WriteByte (os, B2Type.HASHTABLE_11);
						break;
					}
				case 12:
					{
						WriteByte (os, B2Type.HASHTABLE_12);
						break;
					}
				case 13:
					{
						WriteByte (os, B2Type.HASHTABLE_13);
						break;
					}
				case 14:
					{
						WriteByte (os, B2Type.HASHTABLE_14);
						break;
					}
				case 15:
					{
						WriteByte (os, B2Type.HASHTABLE_15);
						break;
					}
				default:
					WriteByte (os, B2Type.HASHTABLE);
					writeInt (os, len);
					break;
				}
				
				
				foreach (System.Collections.DictionaryEntry e in v) {
					Object key = e.Key;
					Object val = e.Value;
					writeObject (os, key);
					writeObject (os, val);
				}
					
				//Set<Entry> entrys = v.entrySet();
				
//				for (Entry e : entrys) {
//					Object key = e.getKey();
//					Object var = e.getValue();
//					writeObject(os, key);
//					writeObject(os, var);
//				}
				
				//			Enumeration keys = v.keys();
				//			Iterator keys = v.keySet().iterator();
				//			while (keys.hasNext()) {
				//				Object key = keys.next();
				//				Object var = v.get(key);
				//				writeObject(os, key);
				//				writeObject(os, var);
				//			}
			}
		}

		public static void writeObject (Stream os, Object obj)
		{
			if (obj == null) {
				writeNull (os);
			} else if (B2Type.isMap (obj)) {
				Hashtable v = (Hashtable)obj;
				writeMap (os, v);
			} else if (B2Type.isInt (obj)) {
				int v = ((Int32)obj);
				writeInt (os, v);
			} else if (B2Type.isString (obj)) {
				String v = (String)obj;
				writeString (os, v);
			} else if (B2Type.isBool (obj)) {
				bool v = ((Boolean)obj);
				writeBoolean (os, v);
			} else if (B2Type.isByte (obj)) {
				int v = ((Byte)obj);
				writeByte (os, v);
			} else if (B2Type.isBytes (obj)) {
				byte[] v = (byte[])obj;
				writeBytes (os, v);
			} else if (B2Type.isList (obj)) {
				ArrayList v = (ArrayList)obj;
				writeVector (os, v);
			} else if (B2Type.isShort (obj)) {
				int v = (Int16)obj;
				writeShort (os, v);
			} else if (B2Type.isLong (obj)) {
				long v = ((Int64)obj);
				writeLong (os, v);
			} else if (B2Type.isDouble (obj)) {
				double v = ((Double)obj);
				writeDouble (os, v);
			} else if (B2Type.isIntArray (obj)) {
				int[] v = (int[])obj;
				writeIntArray (os, v);
//			} else if(obj instanceof int[][]){
//				int[][] v = (int[][]) obj;
//				writeInt2DArray(os, v);
			} else if (B2Type.isB2Int (obj)) {
				B2Int v = (B2Int)obj;
				writeB2Int (os, v);
			} else {
				//throw new IOException("unsupported obj:" + obj);
				UnityEngine.Debug.LogError ("B2IO unsupported error: type=[" + obj.GetType () + "] val=[" + obj + "]");
			}
		}

		protected static void printString (Stream os, byte[] v)
		{
			os.Write (v, 0, v.Length);
			//printString(os, v, 0, v.Length);
		}

		protected static void printString2 (Stream os, byte[] v)
		{
			for (int n = 0; n < v.Length; n++)
				v [n] = (byte)(v [n] ^ B2Type.STR);

			os.Write (v, 0, v.Length);
			//printString(os, v, 0, v.Length);
		}
		/*
		protected static void printString(Stream os, String v) {
			printString(os, v, 0, v.Length);
		}
	
		protected static void printString(Stream os, String v, int offset, int length) {
			for (int i = 0; i < length; i++) {
				char ch = v[i + offset];
	
				if (ch < 0x80)
					WriteByte(os, ch);
				else if (ch < 0x800) {
					WriteByte(os, 0xc0 + ((ch >> 6) & 0x1f));
					WriteByte(os, 0x80 + (ch & 0x3f));
				} else {
					WriteByte(os, 0xe0 + ((ch >> 12) & 0xf));
					WriteByte(os, 0x80 + ((ch >> 6) & 0x3f));
					WriteByte(os, 0x80 + (ch & 0x3f));
				}
			}
		}
		*/
		


	}
}

