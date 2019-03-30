using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Diagnostics;
using Coolape;

/*
 * 1.获取LuaJIT！地址http://luajit.org/ 
  注意！！！！！============>>>>> uLua uses LuaJIT 2.0.2 which can be obtained from http://luajit.org/ 
  一定要下载LuaJIT 2.0.2 （当前release version 2.0.3） 
   
2.解压下载的压缩包 
  在terminal依次输入 
  cd /Users/YourAccount/Download/LuaJIT-2.0.2 
  make 
  sudo make install 
  luajit 
  如果能看到luajit的版本号，到此mac上luajit环境就ok了！！！ 
   
3.通过luajit对所有lua文件进行编译，生成bytecode二进制文件 
  find . -name "*.lua" -exec luajit -b {} {}.out \; 
  {}.out  至于这个后缀，本人其实建议还是用lua 
   
4.最后就是既关键！又简单！的一步！ 
  大家使用ulua，一般调用的是Lua.cs里的LuaState实例的DoString、DoFile 
  如果想使用LuaJIT的bytecode文件，只需要调用LuaDLL.luaL_dofile(IntPtr luaState, string fileName) 
   
本人亲测win、mac、android、ios均通过测试！！！解决了lua加密的问题！嘿嘿！ 


本次测试正好确认了一件事！那就是ulua插件提供的库里已经把LuaJIT打进去了！ 
*/
public class ECLLuaEncodeTool
{
	
	/// <summary>
	/// Luajits the encode.
	/// </summary>
	public static void luajitEncode ()
	{
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);//Selection表示你鼠标选择激活的对象
		if (string.IsNullOrEmpty (path) || !File.Exists (path)) {
			UnityEngine.Debug.LogWarning ("请选择lua 文件!");
			return;
		}
//		luajitEncode (path);
		xxteaEncode (path);
		EditorUtility.DisplayDialog ("success", "Encode cuccess!", "Okey");
	}

	public static void luajitEncodeAll ()
	{
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);//Selection表示你鼠标选择激活的对象
		if (string.IsNullOrEmpty (path) || !Directory.Exists (path)) {
			UnityEngine.Debug.LogWarning ("请选择目录!");
			return;
		}
		luajitEncodeAll (path);
		EditorUtility.DisplayDialog ("success", "Encode cuccess!", "Okey");
	}

	public static void luajitEncodeAll (string dir)
	{
		string[] fileEntries = Directory.GetFiles (dir);//因为Application.dataPath得到的是型如 "工程名称/Assets"
		string extension = "";
		foreach (string f in fileEntries) {
			extension = Path.GetExtension (f);
			extension = extension.ToLower ();
			if (extension != ".lua") {
				continue;
			}
//			luajitEncode (f);
			xxteaEncode (f);
		}
		string[] dirEntries = Directory.GetDirectories (dir);
		foreach (string d in dirEntries) {
			luajitEncodeAll (d);
		}
		
	}

	public static void luajitEncode (string path)
	{
		string basePath = Application.dataPath;
		path = path.Replace ("Assets/", "/");
		string dir = Path.GetDirectoryName (path);
		string fname = Path.GetFileNameWithoutExtension (path);
		string fext = Path.GetExtension (path);
		string outPath = basePath + dir + "/" + fname + fext;
		outPath = outPath.Replace ("/upgradeRes4Dev", "/upgradeRes4Publish");
		Directory.CreateDirectory (Path.GetDirectoryName (outPath));
		#if UNITY_IOS
		byte[] bytes = XXTEA.Encrypt(File.ReadAllBytes(basePath + path), XXTEA.defaultKey);
		File.WriteAllBytes(outPath, bytes);
		#elif UNITY_ANDROID
		//		string exeName = "/usr/local/bin/luajit";
		string exeName = "/usr/local/bin/luajit-2.0.4";
		//		string exeName = "/usr/local/bin/luajit-2.0.3";
		//		string exeName = "/usr/local/bin/luajit-2.1.0-alpha";
		if (!File.Exists (exeName)) {
			UnityEngine.Debug.LogError ("Ecode lua failed. file not fou" + exeName);
			return;
		}
		string arguments = "-b " + basePath + path + " " + outPath;
		Process.Start (exeName, arguments);
		//#else
		//		byte[] bytes = XXTEA.Encrypt(File.ReadAllBytes(basePath + path), XXTEA.defaultKey);
		//		File.WriteAllBytes(outPath, bytes);
		#endif
	}

	public static void xxteaEncode (string path)
	{
		string basePath = Application.dataPath;
		path = path.Replace ("Assets/", "/");
		string dir = Path.GetDirectoryName (path);
		string fname = Path.GetFileNameWithoutExtension (path);
		string fext = Path.GetExtension (path);
		string outPath = basePath + dir + "/" + fname + fext;
		outPath = outPath.Replace ("/upgradeRes4Dev", "/upgradeRes4Publish");
		Directory.CreateDirectory (Path.GetDirectoryName (outPath));
		byte[] bytes = XXTEA.Encrypt (System.Text.Encoding.UTF8.GetBytes (File.ReadAllText (basePath + path)), XXTEA.defaultKey);
		File.WriteAllBytes (outPath, bytes);
	}
}
