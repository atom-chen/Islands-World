using System.Collections;
using System.IO;

namespace Coolape
{
	public class ByteEx
	{
		public static byte[] readFully (Stream stream)
		{ 
			// 初始化一个32k的缓存 
			byte[] buff = new byte[32768]; 
			using (MemoryStream ms = new MemoryStream ()) { //返回结果后会自动回收调用该对象的Dispose方法释放内存 
				// 不停的读取 
				while (true) { 
					int read = stream.Read (buff, 0, buff.Length); 
					// 直到读取完最后的3M数据就可以返回结果了 
					if (read <= 0)
						return ms.ToArray (); 

					ms.Write (buff, 0, read); 
				} 
			} 
		}

		public static void readFully4BLength (Stream ns)
		{
			int len = NumEx.readInt (ns);
			byte[] b = new byte[len];
			readFully (ns, b);
		}

		public static void readFullyBIO2Length (Stream ns)
		{
			int len = B2InputStream.readInt (ns);
			byte[] b = new byte[len];
			readFully (ns, b);
		}

		public static void readFully (Stream ns, byte[] b)
		{
			int off = 0;
			int len = b.Length;
			readFully (ns, b, off, len);
		}

		public static void readFully (Stream ns, byte[] b, int off, int len)
		{
			if (len < 0)
				return;
			int n = 0;
			int trynum = 1000;
			while (n < len) {
				if (trynum-- <= 0)
					break;
				int count = ns.Read (b, off + n, len - n);
				if (count == 0)
					continue;
				if (count < 0)
					return;
				n += count;
			}
		}
	}
}
