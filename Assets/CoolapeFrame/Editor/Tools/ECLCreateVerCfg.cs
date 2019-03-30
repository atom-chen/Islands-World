/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  工程中资源文件成生md5配置文件
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System;
using Coolape;

//生成版本配置文件
public static class ECLCreateVerCfg
{
	static string basePath = "";
	static ArrayList replaces = new ArrayList ();

	static public  Hashtable create2Map (string path)
	{	
		basePath = Application.dataPath + "/";
		basePath = basePath.Replace ("/Assets/", "/");
		replaces.Clear ();
		replaces.Add (basePath + "Assets/StreamingAssets/");
		replaces.Add (basePath + "Assets/Resources/");
		replaces.Add (basePath + "Assets/");
		
		path = basePath + path;

		string lastVerPath = Application.dataPath + "/" + ECLProjectManager.ver4DevelopeMd5;
		Hashtable lastVerMap = Utl.fileToMap (lastVerPath);
		if (lastVerMap == null) {
			lastVerMap = new Hashtable ();
		}

		Hashtable lastOtherVer = MapEx.getMap (lastVerMap, "other");
		Hashtable lastPriorityVer = MapEx.getMap (lastVerMap, "priority");
		Hashtable lastCfgdataVer = MapEx.getMap (lastVerMap, "cfgData");

		Hashtable outMap = new Hashtable ();
		doCreate (path, lastOtherVer, lastPriorityVer, lastCfgdataVer, ref outMap);
		return outMap;
	}

	static public  void create (string path, string outPath)
	{	
		Hashtable outMap = create2Map (path);
		saveMap (outMap, outPath);
	}

	static public void saveMap (Hashtable map, string outPath)
	{
		MemoryStream ms = new MemoryStream ();
		B2OutputStream.writeMap (ms, map);
		Directory.CreateDirectory (Path.GetDirectoryName (outPath));
		File.WriteAllBytes (outPath, ms.ToArray ());
	}

	static void doCreate (string path, Hashtable lastOtherVer, Hashtable lastPriorityVer, Hashtable lastCfgdataVer, ref Hashtable outMap)
	{
		string[] fileEntries = Directory.GetFiles (path);
		string extension = "";
		string filePath = "";
		string md5Str = "";

		foreach (string fileName in fileEntries) {
			extension = Path.GetExtension (fileName);
			if (ECLEditorUtl.isIgnoreFile (fileName)) {
				continue;
			}
			filePath = filter (fileName);
			filePath = filePath.Replace ("\\", "/");
			filePath = filePath.Replace ("/upgradeRes4Publish/", "/upgradeRes/");

			if (ECLProjectManager.isModified (fileName)) {
				md5Str = Utl.MD5Encrypt (File.ReadAllBytes (fileName));
			} else {
				md5Str = MapEx.getString (lastOtherVer, filePath);
				if (string.IsNullOrEmpty (md5Str)) {
					md5Str = MapEx.getString (lastPriorityVer, filePath);
				} else if (string.IsNullOrEmpty (md5Str)) {
					md5Str = MapEx.getString (lastCfgdataVer, filePath);
				}
			}
			outMap [filePath] = md5Str;
		}
		
		string[] dirEntries = Directory.GetDirectories (path);
		foreach (string dir in dirEntries) {
			//跳过不同平台的资源
			#if UNITY_ANDROID
			if (Path.GetFileName (dir).Equals ("IOS") || Path.GetFileName(dir).Equals("Standalone") || Path.GetFileName(dir).Equals("StandaloneOSX")) {
				continue;
			}
#elif UNITY_IOS
			if(Path.GetFileName(dir).Equals("Android") || Path.GetFileName(dir).Equals("Standalone") || Path.GetFileName(dir).Equals("StandaloneOSX")) {
				continue;
			}
#elif UNITY_STANDALONE_WIN
            if(Path.GetFileName(dir).Equals("Android") || Path.GetFileName(dir).Equals("IOS") || Path.GetFileName(dir).Equals("StandaloneOSX")) {
                continue;
            }
#elif UNITY_STANDALONE_OSX
            if(Path.GetFileName(dir).Equals("Android") || Path.GetFileName(dir).Equals("IOS") || Path.GetFileName(dir).Equals("Standalone")) {
                continue;
            }
#endif
            doCreate(dir, lastOtherVer, lastPriorityVer, lastCfgdataVer, ref outMap);
		}
	}

	static string filter (string path)
	{
		string str = "";
		for (int i = 0; i < replaces.Count; i++) {
			str = replaces [i].ToString ();
			path = path.Replace (str, "");
		}
		return path;
	}
}
