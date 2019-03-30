/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  Http工具
  *Others:  
  *History:
*********************************************************************************
*/ 

using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Security;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Net.Sockets;
using System.Collections;

namespace Coolape
{
	public class HttpEx
	{
		public static string readString (string url, int? timeout)
		{
			HttpWebResponse res = HttpEx.CreateGetHttpResponse (url, timeout);
			string str = HttpEx.readString (res);
			res.Close ();
			return str;
		}

		public static string readString (string url, byte[] data, int? timeout)
		{
			HttpWebResponse res = HttpEx.CreatePostHttpResponse (url, data, timeout);
			string str = HttpEx.readString (res);
			res.Close ();
			return str;
		}

		public static string readString (HttpWebResponse response)
		{
			if (response == null)
				return "";
			HttpStatusCode code = response.StatusCode;
			if (HttpStatusCode.OK != code)
				return "";

			StringBuilder sb = new StringBuilder ();
			Stream stream = response.GetResponseStream ();
			StreamReader srdPreview = new StreamReader (stream);
			while (srdPreview.Peek () > -1) {
				String input = srdPreview.ReadLine ();
				sb.AppendLine (input);
			}
			return sb.ToString ();
		}

		public static byte[] readBytes (string url, int? timeout)
		{
			HttpWebResponse res = HttpEx.CreateGetHttpResponse (url, timeout);
			byte[] r2 = HttpEx.readBytes (res);
			res.Close ();
			return r2;
		}

		public static byte[] readBytes (string url, byte[] data, int? timeout)
		{
			HttpWebResponse res = HttpEx.CreatePostHttpResponse (url, data, timeout);
			byte[] r2 = HttpEx.readBytes (res);
			res.Close ();
			return r2;
		}

		private static byte[] readBytes (HttpWebResponse response)
		{
			if (response == null)
				return new byte[0];
			
			HttpStatusCode code = response.StatusCode;
			if (HttpStatusCode.OK != code)
				return new byte[0];

			int length = (int)response.ContentLength;
			if (length < 0)
				return new byte[0];
			byte[] buff = new byte[length];
			
			Stream stream = response.GetResponseStream ();
			int n = 0;
			int off = 0;
			while (n < length) {
				int count = stream.Read (buff, off + n, length - n);
				if (count < 0)
					break;
				n += count;
			}
			//stream.Read (buff, 0, length);
			return buff;
		}
		
		//
		public static HttpWebResponse CreateGetHttpResponse (string url, int? timeout)
		{  
			if (string.IsNullOrEmpty (url)) {  
				throw new ArgumentNullException ("url");  
			}  
			HttpWebRequest request = WebRequest.Create (url) as HttpWebRequest;  
			request.Method = "GET";  
			//request.UserAgent = "u3d";
			//request.ContentType = "audio/ogg";
			if (timeout.HasValue) {  
				request.Timeout = timeout.Value;  
			}  
			return request.GetResponse () as HttpWebResponse;  
		}

		//
		public static HttpWebResponse CreatePostHttpResponse (string url, IDictionary<string,object> parameters, int? timeout, Encoding requestEncoding)
		{  
			if (string.IsNullOrEmpty (url)) {  
				throw new ArgumentNullException ("url");  
			}  
			if (requestEncoding == null) {  
				throw new ArgumentNullException ("requestEncoding");  
			}  

			byte[] data = null;
			//如果需要POST数据  
			if (!(parameters == null || parameters.Count == 0)) {  
				StringBuilder buffer = new StringBuilder ();  
				int i = 0;  
				foreach (string key in parameters.Keys) {  
					if (i > 0) {  
						buffer.AppendFormat ("&{0}={1}", key, parameters [key]);  
					} else {  
						buffer.AppendFormat ("{0}={1}", key, parameters [key]);  
					}  
					i++;  
				}  
				data = requestEncoding.GetBytes (buffer.ToString ());  
			}  
			return CreatePostHttpResponse (url, data, timeout);  
		}

		public static HttpWebResponse CreatePostHttpResponse (string url, byte[] data, int? timeout)
		{  
			if (string.IsNullOrEmpty (url)) {  
				throw new ArgumentNullException ("url");  
			}  
			HttpWebRequest request = null;  
			//如果是发送HTTPS请求  
			if (url.StartsWith ("https", StringComparison.OrdinalIgnoreCase)) {  
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback (CheckValidationResult);  
				request = WebRequest.Create (url) as HttpWebRequest;  
				request.ProtocolVersion = HttpVersion.Version10;  
			} else {  
				request = WebRequest.Create (url) as HttpWebRequest;  
			}  
			request.Method = "POST";  
			//request.UserAgent = "u3d";  
			//request.ContentType = "audio/ogg";  

			if (timeout.HasValue) {  
				request.Timeout = timeout.Value;  
			}  
			//如果需要POST数据  
			if (data != null && data.Length > 0) {  
				using (Stream stream = request.GetRequestStream ()) {  
					if (!stream.CanWrite)
						throw new Exception ("stream isClosed");
					stream.Write (data, 0, data.Length);  
				}  
			}  
			return request.GetResponse () as HttpWebResponse;  
		}

		private static bool CheckValidationResult (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{  
			return true; //总是接受  
		}

		// ***************************************************************
		const string GET_HEAD = "GET /{0} HTTP/1.1\r\nContent-Type: text/html; \r\n\r\n";
		const string POST_HEAD = "POST /{0} HTTP/1.1\r\nHost: {1}:{2}\r\nContent-Type: text/html;\r\nContent-Length: {3:G} \r\n\r\n";
		const int TIMEOUT = 50000;

		public static Hashtable get2json (string url)
		{
			string str = get2str (url);
			if (str == null || str.Length <= 0 || !str.StartsWith ("{"))
				return new Hashtable ();
			UnityEngine.Debug.Log (str);
			return (Hashtable)JSON.JsonDecode (str);
		}

		public static string get2str (string url)
		{
			byte[] buff = get2bytes (url);
			return Encoding.UTF8.GetString (buff);
		}

		public static byte[] get2bytes (string url)
		{
			Uri u = new Uri (url);
			return get2 (u.Host, u.Port, u.PathAndQuery);
		}

		public static byte[] get2 (string host, int port, string path)
		{
			return get2 (host, port, path, TIMEOUT);
		}

		public static byte[] get2 (string host, int port, string path, int timeout)
		{
			TcpClient client = new TcpClient ();
			client.SendTimeout = timeout;
			client.ReceiveTimeout = timeout;
			client.Connect (host, port);
			// create request post data
			string str = string.Format (GET_HEAD, path);
			byte[] headBytes = System.Text.Encoding.Default.GetBytes (str);
			// send  request data
			NetworkStream ns = client.GetStream ();
			ns.Write (headBytes, 0, headBytes.Length);
			//ns.Write (buf, 0, buf.Length);
			ns.Flush ();
			// read http header
			int length = 0;
			for (int n = 0; n < 100; n++) {
				string line = readLine (ns);
				bool isLen = line.StartsWith ("Content-Length");
				if (isLen) {
					string sub = line.Substring (line.IndexOf (":") + 1);
					length = NumEx.stringToInt (sub);
				}
				if (line == null || line.Length <= 1) {
					break;
				}
			}
			
			// read response body
			byte[] r2 = new byte[length];
			ns.Read (r2, 0, length);
			ns.Close ();
			client.Close ();
			
			return r2;
		}

		public static byte[] post2 (string host, int port, string path, byte[] buf)
		{
			return post2 (host, port, path, buf, TIMEOUT);
		}

		public static byte[] post2 (string host, int port, string path, byte[] buf, int timeout)
		{
			TcpClient client = new TcpClient ();
			client.SendTimeout = timeout;
			client.ReceiveTimeout = timeout;
			client.Connect (host, port);
			// create request post data
			//const string POST_HEAD = "POST /{0} HTTP/1.1\r\nHost: {1}:{2}\r\nContent-Type: text/html;\r\nContent-Length: {3:G} \r\n\r\n";
			string str = string.Format (POST_HEAD, path, host, port, buf.Length);
			byte[] headBytes = System.Text.Encoding.Default.GetBytes (str);
			// send  request data
			NetworkStream ns = client.GetStream ();
			ns.Write (headBytes, 0, headBytes.Length);
			ns.Write (buf, 0, buf.Length);
			ns.Flush ();
			// read http header
			int length = 0;
			for (int n = 0; n < 100; n++) {
				string line = readLine (ns);
				bool isLen = line.StartsWith ("Content-Length");
				if (isLen) {
					string sub = line.Substring (line.IndexOf (":") + 1);
					length = NumEx.stringToInt (sub);
				}
				if (line == null || line.Length <= 1) {
					break;
				}
			}
			
			// read response body
			byte[] r2 = new byte[length];
			ns.Read (r2, 0, length);
			ns.Close ();
			client.Close ();
			
			return r2;
		}

		public static string readLine (Stream stream)
		{
			StringBuilder sb = ObjPool.strs.borrowObject ("");
			try {
				for (int i = 0; i < 1024; i++) {
					int b = stream.ReadByte ();
					if (b != '\r') {
						sb.Append ((char)b);
					} else if (b == '\n') {
						return sb.ToString ();
					} else {
						stream.ReadByte ();
						return sb.ToString ();
					}
				}
			} finally {
				ObjPool.strs.returnObject (sb);	
			}
			return sb.ToString ();
		}
		// ***************************************************************
	}
}
