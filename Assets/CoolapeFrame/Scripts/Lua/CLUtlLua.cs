/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  lua相关处理工具类
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;
using XLua;
using System.IO;

namespace Coolape
{
	public class CLUtlLua
	{
		//public static bool isFinishAddLoader = false;
		public static Hashtable FileBytesCacheMap = new Hashtable ();

		/// <summary>
		/// Adds the lua loader.
		/// </summary>
		/// <param name="lua">Lua.</param>
		public static void addLuaLoader (LuaEnv lua)
		{
            LuaEnv.CustomLoader loader = (LuaEnv.CustomLoader)(myLuaLoader);
            if (!lua.customLoaders.Contains(loader))
            {
                lua.AddLoader(loader);
            }
			//isFinishAddLoader = true;
		}

		public static void cleanFileBytesCacheMap ()
		{
			FileBytesCacheMap.Clear ();
		}

		public static byte[] myLuaLoader (ref string filepath)
		{
			byte[] bytes = null;
			string luaPath = "";
			string strs = "";
			try {
				if (!filepath.StartsWith (CLPathCfg.self.basePath)) {
					//说明是通过require进来的
					filepath = filepath.Replace (".", "/");
					filepath = PStr.b ().a (CLPathCfg.self.basePath).a ("/upgradeRes/priority/lua/").a (filepath).a (".lua").e ();
				}
				#if UNITY_EDITOR
				if (CLCfgBase.self.isEditMode) {
					filepath = filepath.Replace ("/upgradeRes/", "/upgradeRes4Dev/");
					luaPath = PStr.b ().a (Application.dataPath).a ("/").a (filepath).e ();
					bytes = MapEx.getBytes (FileBytesCacheMap, luaPath);
					if (bytes != null) {
						filepath = luaPath;
						return bytes;
					}
					if (File.Exists (luaPath)) {
						strs = FileEx.getTextFromCache (luaPath);
						bytes = System.Text.Encoding.UTF8.GetBytes (strs);
						filepath = luaPath;
						return bytes;
					}
				}
				#endif

				//=======================================================
				//1.first  load from CLPathCfg.persistentDataPath;
				luaPath = PStr.b ().a (CLPathCfg.persistentDataPath).a ("/").a (filepath).e ();
				bytes = MapEx.getBytes (FileBytesCacheMap, luaPath);
				if (bytes != null) {
					filepath = luaPath;
					return bytes;
				}
				if (File.Exists (luaPath)) {
					bytes = FileEx.getBytesFromCache (luaPath); 
					if (bytes != null) {
//					bytes = System.Text.Encoding.UTF8.GetBytes(strs);
						bytes = deCodeLua (bytes);
						FileBytesCacheMap [luaPath] = bytes;
						filepath = luaPath;
						return bytes;
					}
				}
				//=======================================================
				//2.second load from  Application.streamingAssetsPath;
				luaPath = PStr.b ().a (Application.streamingAssetsPath).a ("/").a (filepath).e ();
				bytes = MapEx.getBytes (FileBytesCacheMap, luaPath);
				if (bytes != null) {
					filepath = luaPath;
					return bytes;
				}

				bytes = FileEx.getBytesFromCache (luaPath);
				if (bytes != null) {
//				bytes = System.Text.Encoding.UTF8.GetBytes(strs);
					bytes = deCodeLua (bytes);
					FileBytesCacheMap [luaPath] = bytes;
					filepath = luaPath;
					return bytes;
				}
				//=======================================================
				//3.third load from Resources.Load ();
				luaPath = filepath;
				bytes = MapEx.getBytes (FileBytesCacheMap, luaPath);
				if (bytes != null) {
					filepath = luaPath;
					return bytes;
				}

				TextAsset text = Resources.Load<TextAsset> (filepath);
				if (text != null) {
					bytes = text.bytes;// System.Text.Encoding.UTF8.GetBytes(text.text);
					if (bytes != null) {
						bytes = deCodeLua (bytes);
						FileBytesCacheMap [luaPath] = bytes;
						filepath = luaPath;
						return bytes;
					}
				}
				//==========================
				return bytes;
			} catch (System.Exception e) {
				Debug.LogError (luaPath + ":" + e);
				return null;
			}
		}

		// lua文件解密
		public static byte[] deCodeLua (byte[] buff)
		{
			if (buff == null) {
				return null;
			}
			if (CLCfgBase.self.isEncodeLua) {
				return XXTEA.Decrypt (buff, XXTEA.defaultKey);
			}
			return buff;
		}

		public static string getLua (string fn)
		{
			string path = fn;
			string str = "";
			byte[] buff = myLuaLoader (ref path);
			if (buff != null) {
				str = System.Text.Encoding.UTF8.GetString (buff);
			}
			return str;
		}

		public static object[] doLua (LuaEnv lua, string _path)
		{
			try {
				string path = _path.Replace ("\\", "/");
				string filebase = Path.GetFileName(path);
				path = path.Replace ("/upgradeRes4Publish/", "/upgradeRes/");
				string luaContent = "";
#if UNITY_EDITOR
				if (CLCfgBase.self.isEditMode) {
					string tmpPath = path.Replace ("/upgradeRes/", "/upgradeRes4Dev/");
					luaContent = getLua (tmpPath);
				} else {
					luaContent = getLua (path);
				}
#else
				luaContent = getLua (path);
#endif
				if(string.IsNullOrEmpty(luaContent)) {
					Debug.LogError (_path + " get content is null!");
					return null;
				} else {
					return lua.DoString(luaContent, filebase);
				}
			} catch (System.Exception e) {
				Debug.LogError (_path + "," + e);
				return null;
			}
		}

		public static ArrayList luaTableKeys2List (LuaTable table)
		{
			ArrayList list = new ArrayList ();
			foreach (object key in table.GetKeys<object>()) {
				list.Add (key);
			}
			return list;
		}

		public static ArrayList luaTableVals2List (LuaTable table)
		{
			ArrayList list = new ArrayList ();
            foreach (object key in table.GetKeys<object>()) {
				list.Add (table.Get<object> (key));
			}
			return list;
		}
	}
}
