using UnityEngine;
using System.Collections;
using System.IO;
using Coolape;
using System.Net;
using System;
using System.Text;

namespace Coolape
{
	public class FTP
	{
		public string host;
		public  string user;
		public  string password;
		public FtpWebRequest request;
		public float progress = 0;

		public FTP (string host, string user, string password)
		{
			this.host = host;
			this.user = user;
			this.password = password;
		}

		public bool Upload (string localFile, string remotePath)
		{
			return Upload (localFile, host, user, password, remotePath, ref  progress);
		}

		/// <summary>
		/// Uploads a file through FTP.
		/// </summary>
		/// <param name="filename">The path to the file to upload.</param>
		/// <param name="server">The server to use.</param>
		/// <param name="username">The username to use.</param>
		/// <param name="password">The password to use.</param>
		/// <param name="initialPath">The path on the server to upload to.</param>
		public bool Upload (string localFile, string server, string username, string password, string remotePath, ref float progress)
		{
			//		MakeFTPDir (server, remotePath, username, password, onlyCheckLastDir);
			var file = new FileInfo (localFile);
			string url = PStr.b ().a ("ftp://").a (server).a (Path.Combine (remotePath, file.Name)).e ();
			//		Debug.Log (localFile);
			Debug.Log (url);
			//		Debug.Log (username);
			//		Debug.Log (password);
			var address = new Uri (url);
			request = FtpWebRequest.Create (address) as FtpWebRequest;

			// Upload options:
			// Provide credentials
			request.Credentials = new NetworkCredential (username, password);

			// Set control connection to closed after command execution
			request.KeepAlive = false;

			// Specify command to be executed
			request.Method = WebRequestMethods.Ftp.UploadFile;

			// Specify data transfer type
			request.UseBinary = true;

			// Notify server about size of uploaded file
			request.ContentLength = file.Length;

			// Set buffer size to 2KB.
			var bufferLength = 2048;
			var buffer = new byte[bufferLength];
			var contentLength = 0;
			int countLen = 0;

			// Open file stream to read file
			var fs = file.OpenRead ();
			Stream stream = null;
			try {
				// Stream to which file to be uploaded is written.
				stream = request.GetRequestStream ();

				// Read from file stream 2KB at a time.
				contentLength = fs.Read (buffer, 0, bufferLength);

				// Loop until stream content ends.
				while (contentLength != 0) {
					//Debug.Log("Progress: " + ((fs.Position / fs.Length) * 100f));
					// Write content from file stream to FTP upload stream.
					stream.Write (buffer, 0, contentLength);
					contentLength = fs.Read (buffer, 0, bufferLength);
					countLen += contentLength;
					progress = ((float)countLen) / request.ContentLength;
				}
				// Close file and request streams
				stream.Close ();
				fs.Close ();
			} catch (Exception e) {
				if (stream != null) {
					stream.Close ();
				}
				if (fs != null) {
					fs.Close ();
				}
				Debug.LogError ("Error uploading file: " + e);
				return false;
			}
			Debug.Log ("Upload successful.");
			return true;
		}

		public void MakeFTPDir (string pathToCreate, bool onlyCheckLastDir)
		{
			Uri address = null;
			FtpWebRequest request = null;
			Stream ftpStream = null;

			string[] subDirs = pathToCreate.Split ('/');
			if (subDirs.Length == 0)
				return;
			string currentDir = PStr.b ().a ("ftp://").a (host).e ();
			string subDir = "";
			int i = 0;
			if (onlyCheckLastDir) {
				i = subDirs.Length - 1;
				currentDir = PStr.b ().a (currentDir).a (Path.GetDirectoryName (pathToCreate)).e ();
			} else {
				i = 0;
			}
			for (; i < subDirs.Length; i++) {
				subDir = subDirs [i];
				if (string.IsNullOrEmpty (subDir))
					continue;
				try {
					currentDir = PStr.b ().a (currentDir).a ("/").a (subDir).e ();
					//				Debug.Log("login==" + login);
					//				Debug.Log("password==" + password);
					//				Debug.Log("currentDir====" + currentDir);
					address = new Uri (currentDir);
					request = FtpWebRequest.Create (address) as FtpWebRequest;

					// Upload options:

					// Provide credentials
					request.Credentials = new NetworkCredential (user, password);

					// Set control connection to closed after command execution
					request.KeepAlive = false;

					// Specify command to be executed
					request.Method = WebRequestMethods.Ftp.MakeDirectory;

					// Specify data transfer type
					request.UseBinary = true;

					FtpWebResponse response = (FtpWebResponse)request.GetResponse ();
					ftpStream = response.GetResponseStream ();
					ftpStream.Close ();
					response.Close ();
				} catch (Exception ex) {
					//directory already exist I know that is weak but there is no way to check if a folder exist on ftp...
					//				Debug.LogError(ex);
					//				return false;
				}
			}
		}

		/// <summary>
		/// 获取文件大小
		/// </summary>
		/// <param name="file">ip服务器下的相对路径</param>
		/// <returns>文件大小</returns>
		public int GetFileSize (string file, string host, string username, string password)
		{
			StringBuilder result = new StringBuilder ();
			try {
				string uri = PStr.b ().a ("ftp://").a (host).a (file).e ();
				request = (FtpWebRequest)FtpWebRequest.Create (new Uri (uri));
				request.UseBinary = true;
				request.Credentials = new NetworkCredential (username, password);//设置用户名和密码
				request.Method = WebRequestMethods.Ftp.GetFileSize;

				int dataLength = (int)request.GetResponse ().ContentLength;
				return dataLength;
			} catch (Exception ex) {
				Console.WriteLine ("获取文件大小出错：" + ex.Message);
				return -1;
			}
		}


		/* Download File */
		public bool download (string server, string username, string password, string remoteFile, string localFile)
		{
			try {
				/* Create an FTP Request */
				request = (FtpWebRequest)FtpWebRequest.Create ("ftp://" + Path.Combine (server, remoteFile));
				/* Log in to the FTP Server with the User Name and Password Provided */
				request.Credentials = new NetworkCredential (username, password);
				/* When in doubt, use these options */
				request.UseBinary = true;
				request.UsePassive = true;
				request.KeepAlive = false;
				/* Specify the Type of FTP Request */
				request.Method = WebRequestMethods.Ftp.DownloadFile;
				/* Establish Return Communication with the FTP Server */
				FtpWebResponse ftpResponse = (FtpWebResponse)request.GetResponse ();
				/* Get the FTP Server's Response Stream */
				Stream ftpStream = ftpResponse.GetResponseStream ();
				/* Open a File Stream to Write the Downloaded File */
				FileStream localFileStream = new FileStream (localFile, FileMode.Create);
				/* Buffer for the Downloaded Data */
				int bufferSize = 2048;
				byte[] byteBuffer = new byte[bufferSize];
				int bytesRead = ftpStream.Read (byteBuffer, 0, bufferSize);
				/* Download the File by Writing the Buffered Data Until the Transfer is Complete */
				try {
					while (bytesRead > 0) {
						localFileStream.Write (byteBuffer, 0, bytesRead);
						bytesRead = ftpStream.Read (byteBuffer, 0, bufferSize);
					}
				} catch (Exception ex) {
					Console.WriteLine (ex.ToString ());
				}
				/* Resource Cleanup */
				localFileStream.Close ();
				ftpStream.Close ();
				ftpResponse.Close ();
				request = null;
			} catch (Exception ex) {
				Console.WriteLine (ex.ToString ());
				return false;
			}
			return true;
		}

		public void Abort ()
		{
			if (request != null) {
				request.Abort ();
			}
		}

		public static bool UploadDir (string localDir, string server, string username, string password, string remotePath, bool onlyCheckLastDir = false)
		{
			if (!Directory.Exists (localDir)) {
				Debug.LogError ("There is no directory exist!");
				return false;
			}
			FTP ftp = new FTP (server, username, password);
			ftp.MakeFTPDir (remotePath, onlyCheckLastDir);
			string[] files = Directory.GetFiles (localDir);
			string file = "";
			if (files != null) {
				FtpWebRequest request = null;
				for (int i = 0; i < files.Length; i++) {
					file = files [i];
					//				Debug.Log (file);
					ftp = new FTP (server, username, password);
					if (!ftp.Upload (file, remotePath)) {
						return false;
					}
				}
			}

			string[] dirs = Directory.GetDirectories (localDir);
			if (dirs != null) {
				for (int i = 0; i < dirs.Length; i++) {
					//				Debug.Log (PStr.b ().a (remotePath).a ("/").a (Path.GetFileName (dirs [i])).e ());
					if (!UploadDir (dirs [i], server, username, password, PStr.b ().a (remotePath).a ("/").a (Path.GetFileName (dirs [i])).e (), true)) {
						return false;
					}
				}
			}
			return true;
		}
	}
}
