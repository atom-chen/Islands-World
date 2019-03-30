using Tamir.SharpSsh.jsch;
using System.Collections;
using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using Coolape;

public class SFTPHelper
{
	private Session m_session;
	private Channel m_channel;
	private ChannelSftp m_sftp;

	//host:sftp地址   user：用户名   pwd：密码
	public SFTPHelper (string host, int _port, string user, string pwd)
	{
		string[] arr = host.Split (':');
		string ip = arr [0];
//		int port = 22;
		int port = _port;
		if (arr.Length > 1)
			port = Int32.Parse (arr [1]);

		JSch jsch = new JSch ();
		m_session = jsch.getSession (user, ip, port);
		MyUserInfo ui = new MyUserInfo ();
		ui.setPassword (pwd);
		m_session.setUserInfo (ui);

	}

	//SFTP连接状态
	public bool Connected { get { return m_session.isConnected (); } }

	//连接SFTP
	public bool Connect ()
	{
		try {
			if (!Connected) {
				m_session.connect ();
				m_channel = m_session.openChannel ("sftp");
				m_channel.connect ();
				m_sftp = (ChannelSftp)m_channel;
			}
			return true;
		} catch (Exception e) {
			Debug.Log ("connect failed!!" + e);
			return false;
		}
	}

	//断开SFTP
	public void Disconnect ()
	{
		if (Connected) {
			m_channel.disconnect ();
			m_session.disconnect ();
		}
	}

	public bool PutDir (string localDir, string remoteDir, Callback onProgressCallback, Callback onFinishCallback)
	{
		bool ret = false;
		if (!Directory.Exists (localDir)) {
			Debug.LogError ("There is no directory exist!");
			Utl.doCallback (onFinishCallback, false);
			return false;
		}
		Mkdir (remoteDir);
		string[] files = Directory.GetFiles (localDir);
		string file = "";
		string[] dirs = Directory.GetDirectories (localDir);
		if (files != null) {
			for (int i = 0; i < files.Length; i++) {
				file = files [i];
				Debug.Log (file);
				ret = Put (file, remoteDir, onProgressCallback, null);
				if (!ret) {
					Utl.doCallback (onFinishCallback, false);
					return false;
				}
			}
		}

		if (dirs != null) {
			for (int i = 0; i < dirs.Length; i++) {
				//				Debug.Log (PStr.b ().a (remotePath).a ("/").a (Path.GetFileName (dirs [i])).e ());
				ret = PutDir (dirs [i], PStr.b ().a (remoteDir).a ("/").a (Path.GetFileName (dirs [i])).e (), onProgressCallback, null);
				if (!ret) {
					Utl.doCallback (onFinishCallback, false);
					return false;
				}
			}
		}
		Utl.doCallback (onFinishCallback, true);
		return ret;
	}

	public void Mkdir (string dir)
	{
		try {
			m_sftp.mkdir (new Tamir.SharpSsh.java.String (dir));
		} catch (Exception e) {
//			Debug.LogError (e);
		}
	}
	//SFTP存放文件
	public bool Put (string localPath, string remotePath, Callback onProgressCallback, Callback onFinishCallback)
	{
		try {
			Tamir.SharpSsh.java.String src = new Tamir.SharpSsh.java.String (localPath);
			Tamir.SharpSsh.java.String dst = new Tamir.SharpSsh.java.String (remotePath);
			ProgressMonitor progressMonitor = new ProgressMonitor (onProgressCallback, onFinishCallback);
			m_sftp.put (src, dst, progressMonitor, ChannelSftp.OVERWRITE);
			return true;
		} catch (Exception e) {
			Debug.LogError (e);
			return false;
		}
	}

	//SFTP获取文件
	public bool Get (string remotePath, string localPath)
	{
		try {
			Tamir.SharpSsh.java.String src = new Tamir.SharpSsh.java.String (remotePath);
			Tamir.SharpSsh.java.String dst = new Tamir.SharpSsh.java.String (localPath);
			m_sftp.get (src, dst);
			return true;
		} catch (Exception e) {
			Debug.LogError (e);
			return false;
		}
	}
	//删除SFTP文件
	public bool Delete (string remoteFile)
	{
		try {
			m_sftp.rm (remoteFile);
			return true;
		} catch {
			return false;
		}
	}

	public void Exit ()
	{
		try {
			if (m_sftp != null) {
				m_sftp.disconnect ();
				m_sftp.quit ();
				m_sftp.exit ();
			}
		} catch (Exception e) {
			Debug.LogError (e);
		}
	}
	//获取SFTP文件列表
	public ArrayList GetFileList (string remotePath, string fileType)
	{
		try {
			Tamir.SharpSsh.java.util.Vector vvv = m_sftp.ls (remotePath);
			ArrayList objList = new ArrayList ();
			foreach (Tamir.SharpSsh.jsch.ChannelSftp.LsEntry qqq in vvv) {
				string sss = qqq.getFilename ();
				if (sss.Length > (fileType.Length + 1) && fileType == sss.Substring (sss.Length - fileType.Length)) {
					objList.Add (sss);
				} else {
					continue;
				}
			}

			return objList;
		} catch {
			return null;
		}
	}


	//登录验证信息
	public class MyUserInfo : UserInfo
	{
		String passwd;

		public String getPassword ()
		{
			return passwd;
		}

		public void setPassword (String passwd)
		{
			this.passwd = passwd;
		}

		public String getPassphrase ()
		{
			return null;
		}

		public bool promptPassphrase (String message)
		{
			return true;
		}

		public bool promptPassword (String message)
		{
			return true;
		}

		public bool promptYesNo (String message)
		{
			return true;
		}

		public void showMessage (String message)
		{
		}
	}

}

public class ProgressMonitor : SftpProgressMonitor
{
	private long max = 0;
	private long mCount = 0;
	private float percent = 0;
	Callback onProgress;
	Callback onFinish;

	// If you need send something to the constructor, change this method
	public ProgressMonitor (Callback onProgress, Callback onFinish)
	{
		this.onProgress = onProgress;
		this.onFinish = onFinish;
	}

	public override void init (int op, string src, string dest, long max)
	{
		this.max = max;
//		System.out.println("starting");
//		System.out.println(src); // Origin destination
//		System.out.println(dest); // Destination path
//		System.out.println(max); // Total filesize
	}

	public override bool count (long bytes)
	{
		mCount += bytes;
		float percentNow = mCount / (float)max;
		if (percentNow > this.percent) {
			this.percent = percentNow;
			Utl.doCallback (onProgress, percentNow);
//			Debug.Log("progress=="+ this.percent); // Progress 0,0
//			System.out.println(max); //Total ilesize
//			System.out.println(this.count); // Progress in bytes from the total
		}

		return(true);
	}

	public override void end ()
	{
		Utl.doCallback (onFinish, true);
//		System.out.println("finished");// The process is over
//		System.out.println(this.percent); // Progress
//		System.out.println(max); // Total filesize
//		System.out.println(this.count); // Process in bytes from the total
	}
}
