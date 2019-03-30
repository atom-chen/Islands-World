/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  文件工具类
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;

namespace Coolape
{
	/*
    On a desktop computer (Mac OS or Windows) the location of the files can be obtained with the following code:-
      path = Application.dataPath + "/StreamingAssets";
    On iOS, you should use:-
      path = Application.dataPath + "/Raw";
    ...while on Android, you should use:-
      path = "jar:file://" + Application.dataPath + "!/assets/";
    */
	public class FileEx
	{
		public static bool FileExists (string fn)
		{
			try {
				return File.Exists (fn);
			} catch (Exception e) {
				Debug.Log (e.Message);
			}
			return false;
		}

		public static void WriteAllBytes (string fn, byte[] bytes)
		{
			try {
				File.WriteAllBytes (fn, bytes);
			} catch (Exception e) {
				Debug.Log (e.Message);
			}
		}

		public static byte[] ReadAllBytes (string fn)
		{
			try {
				if (fn.StartsWith ("jar:file:")) {
					return FileEx.readBytesFromStreamingAssetsPath (fn);
				} else {
					return File.ReadAllBytes (fn);
				}
			} catch (Exception e) {
				Debug.Log (e.Message);
			}
			return null;
		}

		public static void WriteAllText (string fn, string str)
		{
			try {
				File.WriteAllText (fn, str);
			} catch (Exception e) {
				Debug.Log (e.Message);
			}
		}

		public static void AppendAllText (string fn, string str)
		{
			try {
				File.AppendAllText (fn, str);
			} catch (Exception e) {
				Debug.Log (e.Message);
			}
		}

		public static string ReadAllText (string fn)
		{
			try {
				if (fn.StartsWith ("jar:file:")) {
					return FileEx.readTextFromStreamingAssetsPath (fn);
				} else {
					if (!FileEx.FileExists (fn))
						return null;
					return File.ReadAllText (fn);
				}
			} catch (Exception e) {
				Debug.Log (e.Message);
			}
		
			return "";
		}

		public static void Delete (string fn)
		{
			try {
				File.Delete (fn);
			} catch (Exception e) {
				Debug.Log (e.Message);
			}
		}

		public static bool DirectoryExists (string path)
		{
			return Directory.Exists (path);
		}

		public static bool CreateDirectory (string path)
		{
			if (DirectoryExists (path))
				return true;
			
			DirectoryInfo di = Directory.CreateDirectory (path);
			return di.Exists;
		}

		public static string[] GetFiles (string fn)
		{
			try {
				return Directory.GetFiles (fn);
			} catch (Exception e) {
				Debug.Log (e.Message);
			}
		
			return new string[0];
		}

		public static string[] GetFiles ()
		{
			return GetFiles ("");
		}

		public static void SaveTexture2D (string fn, byte[] data)
		{
			try { 
				if (fn == null || fn.Length <= 0 || data == null || data.Length <= 0)
					return;
				string path = fn;
				File.WriteAllBytes (path, data);
			} catch (Exception e) {
				Debug.Log (e.Message);
			}		
		}

		public static Texture2D LoadTexture2D (int w, int h, string fn)
		{
			try { 
				string path = fn;
				if (!File.Exists (path))
					return null;
		
				byte[] bytes = File.ReadAllBytes (path);
				if (bytes == null || bytes.Length <= 10)
					return null;
				Texture2D r2 = new Texture2D (w, h);
				bool succ = r2.LoadImage (bytes);
				if (!succ)
					return null;
				return r2;
			} catch (Exception e) {
				Debug.Log (e.Message);
			}
			return null;
		}

#if UNITY_ANDROID && !UNITY_EDITOR
		static AndroidJavaClass _jcAssetMgr;

		static AndroidJavaClass jcAssetMgr {
			get {
				if (_jcAssetMgr == null) {
					_jcAssetMgr = new AndroidJavaClass ("com.coolape.u3dPlugin.AssetMgr");
				}
				return _jcAssetMgr;
			}
		}
#endif

		public static string readTextFromStreamingAssetsPath (string filepath)
		{
			string buff = null;
			try {
				#if UNITY_ANDROID && !UNITY_EDITOR
				string tempPath = filepath.Replace(PStr.b().a(Application.streamingAssetsPath).a("/").e(), "");
				buff =  jcAssetMgr.CallStatic<string>("getString", tempPath);
				#else
				if (File.Exists (filepath)) {
					buff = File.ReadAllText (filepath);
				}
				#endif
			} catch (Exception e) {
				Debug.LogError (e);
			}
			return buff;
		}

		public static byte[] readBytesFromStreamingAssetsPath (string filepath)
		{
			byte[] buff = null;
			try {
				#if UNITY_ANDROID && !UNITY_EDITOR
				string tempPath = filepath.Replace(PStr.b().a(Application.streamingAssetsPath).a("/").e(), "");

				AndroidJavaObject obj =  jcAssetMgr.CallStatic<AndroidJavaObject>("getBytes", tempPath);
				if (obj != null && obj.GetRawObject().ToInt32() != 0) {
					buff = AndroidJNIHelper.ConvertFromJNIArray<byte[]>(obj.GetRawObject());
				}
				if(obj != null) {
					obj.Dispose();
					obj = null;
				}
				#else
				if (File.Exists (filepath)) {
					buff = File.ReadAllBytes (filepath);
				}
				#endif
			} catch (Exception e) {
				Debug.LogError (e);
			}
			return buff;
		}

		/// <summary>
		/// Reads the new all text. 同步读取。优先从persistentDataPath目录取得，再从streamingAssetsPath读书
		/// </summary>
		/// <returns>The new all text.</returns>
		/// <param name="fName">F name.</param>
		public static string readNewAllText (string fName)
		{
			try {
				string buff = "";
				string fPath = CLPathCfg.persistentDataPath + "/" + fName;
				if (File.Exists (fPath)) {
					buff = File.ReadAllText (fPath);
				} else {
					fPath = Application.streamingAssetsPath + "/" + fName;
					buff = ReadAllText (fPath);
				}
				#if UNITY_EDITOR
				if (buff == null) {
					Debug.LogError ("Get null content == " + fPath);
				}
				#endif
				return buff;
			} catch (Exception e) {
				Debug.LogError (e);
				return "";
			}
		}

		/// <summary>
		/// Reads the new all text. 异步读取。优先从persistentDataPath目录取得，再从streamingAssetsPath读书
		/// </summary>
		/// <returns>The new all text.</returns>
		/// <param name="fName">F name.</param>
		public static IEnumerator readNewAllTextAsyn (string fName, object OnGet)
		{
			string buff = "";
			string fPath = CLPathCfg.persistentDataPath + "/" + fName;
			if (File.Exists (fPath)) {
				yield return null;
				buff = File.ReadAllText (fPath);
			} else {
				fPath = Application.streamingAssetsPath + "/" + fName;
				if (Application.platform == RuntimePlatform.Android) {
					WWW www = new WWW (Utl.urlAddTimes (fPath));
					yield return www;
					buff = www.text;
					www.Dispose ();
					www = null;
				} else {
					yield return null;
					if (File.Exists (fPath)) {
						buff = File.ReadAllText (fPath);
					}
				}
			}
#if UNITY_EDITOR
			if (buff == null) {
				Debug.LogError ("Get null content == " + fPath);
			}
#endif
			Utl.doCallback (OnGet, buff);
		}

		/// <summary>
		/// Reads the new all text. 同步步读取。优先从persistentDataPath目录取得，再从streamingAssetsPath读书
		/// </summary>
		/// <returns>The new all text.</returns>
		/// <param name="fName">F name.</param>
		public static byte[] readNewAllBytes (string fName)
		{
			try {
				byte[] buff = null;
				string fPath = CLPathCfg.persistentDataPath + "/" + fName;
				if (File.Exists (fPath)) {
					buff = FileEx.ReadAllBytes (fPath);
				} else {
					fPath = Application.streamingAssetsPath + "/" + fName;
					buff = FileEx.ReadAllBytes (fPath);
				}
				#if UNITY_EDITOR
				if (buff == null) {
					Debug.LogError ("Get null content == " + fPath);
				}
				#endif
				return buff;
			} catch (Exception e) {
				Debug.LogError (e);
				return null;
			}
		}

		/// <summary>
		/// Reads the new all text. 异步读取。优先从persistentDataPath目录取得，再从streamingAssetsPath读书
		/// </summary>
		/// <returns>The new all text.</returns>
		/// <param name="fName">F name.</param>
		public static IEnumerator readNewAllBytesAsyn (string fName, object OnGet)
		{
			byte[] buff = null;
			string fPath = CLPathCfg.persistentDataPath + "/" + fName;
			if (File.Exists (fPath)) {
				yield return null;
				buff = File.ReadAllBytes (fPath);
			} else {
				fPath = Application.streamingAssetsPath + "/" + fName;
				if (Application.platform == RuntimePlatform.Android) {
					WWW www = new WWW (Utl.urlAddTimes (fPath));
					yield return www;
					if (string.IsNullOrEmpty (www.error)) {
						buff = www.bytes;
						www.Dispose ();
						www = null;
					}
				} else {
					yield return null;
					if (File.Exists (fPath)) {
						buff = File.ReadAllBytes (fPath);
					}
				}
			}
#if UNITY_EDITOR
			if (buff == null) {
				Debug.LogWarning ("Get null content == " + fPath);
			}
#endif
			Utl.doCallback (OnGet, buff);
		}

		public static Hashtable FileTextMap = new Hashtable ();
		public static Hashtable FileBytesMap = new Hashtable ();

		public static string getTextFromCache (string path)
		{
			if (string.IsNullOrEmpty (path))
				return null;
			string ret = MapEx.getString (FileTextMap, path);
			if (string.IsNullOrEmpty (ret)) {
				ret = FileEx.ReadAllText (path);
				FileTextMap [path] = ret;
			}
			return ret;
		}

		public static byte[] getBytesFromCache (string path)
		{
			if (string.IsNullOrEmpty (path))
				return null;
			byte[] ret = MapEx.getBytes (FileBytesMap, path);
			if (ret == null) {
				ret = FileEx.ReadAllBytes (path);
				FileBytesMap [path] = ret;
			}
			return ret;
		}

		public static void cleanCache ()
		{
			FileTextMap.Clear ();
			FileBytesMap.Clear ();
		}
	}
}
