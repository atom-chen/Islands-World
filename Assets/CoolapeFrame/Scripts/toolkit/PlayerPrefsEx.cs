using UnityEngine;
using System.Collections;
using System;

public class PlayerPrefsEx
{
	public static string UUID_KEY = "player_perfs_uuid_key";
	public static object oLock = new object ();
	
	//生成一次uuid。并且自动保存在本地。
	//只要不删游戏。uuid不变。
	public static string generatorUUID ()
	{
		lock (oLock) {
			if (HasKey (UUID_KEY)) {
				return GetString (UUID_KEY);
			} else {
				string uuid_value = "guid_" + System.Guid.NewGuid ().ToString ();
				SetValue (UUID_KEY, uuid_value);
				Save ();
				return uuid_value;
			}
		}
	}
	
	public static string changeUUID (string uuid_value)
	{
		lock (oLock) {
			if (HasKey (UUID_KEY)) {
				DeleteKey (UUID_KEY);
			}
			SetValue (UUID_KEY, uuid_value);
			Save ();
			return uuid_value;
		}
	}
	
	public static bool HasKey (string key)
	{
		return PlayerPrefs.HasKey (key);
	}
	
	public static void SetValue (string key, byte[] val)
	{
		string base64 = Convert.ToBase64String (val);
		SetValue (key, base64);
	}

	public static void SetValue (string key, string val)
	{
		PlayerPrefs.SetString (key, val);
	}

	public static void SetValue (string key, int val)
	{
		PlayerPrefs.SetInt (key, val);
	}

	public static void SetValue (string key, float val)
	{
		PlayerPrefs.SetFloat (key, val);
	}
	
	public static void Save ()
	{
		PlayerPrefs.Save ();
	}
	
	public static byte[] GetBytes (string key)
	{
		string base64 = GetString (key);
		return Convert.FromBase64String (base64);
	}
	
	public static string GetString (string key)
	{
		return PlayerPrefs.GetString (key);
	}

	public static int GetInt (string key)
	{
		return PlayerPrefs.GetInt (key);
	}

	public static float GetFloat (string key)
	{
		return PlayerPrefs.GetFloat (key);
	}
	
	public static void DeleteAll ()
	{
		PlayerPrefs.DeleteAll ();
	}
	
	public static void DeleteKey (string key)
	{
		PlayerPrefs.DeleteKey (key);
	}
}
