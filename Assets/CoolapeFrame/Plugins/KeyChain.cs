using UnityEngine;
using System.Runtime.InteropServices;

public class KeyChain
{
	#if UNITY_IOS || UNITY_IPHONE || UNITY_STANDALONE_OSX
	
	[DllImport ("__Internal")]
	private static extern string getKeyChainUser ();

	public static string BindGetKeyChainUser ()
	{
		return getKeyChainUser ();
	}

	[DllImport ("__Internal")]
	private static extern void setKeyChainUser (string userId, string uuid);

	public static void BindSetKeyChainUser (string userId, string uuid)
	{
		setKeyChainUser (userId, uuid);
	}

	[DllImport ("__Internal")]
	private static extern void deleteKeyChainUser ();

	public static void BindDeleteKeyChainUser ()
	{
		deleteKeyChainUser ();
	}

	[DllImport ("__Internal")]
	private static extern void setKeyChain (string key, string  val);

	public static void BindSetKeyChain (string key, string  val)
	{
		setKeyChain (key, val);
	}

	[DllImport ("__Internal")]
	private static extern string getKeyChain (string key);

	public static string BindGetKeyChain (string key)
	{
		return getKeyChain (key);
	}

	[DllImport ("__Internal")]
	private static extern void deleteKeyChain (string key);

	public static void BindDeleteKeyChain (string  key)
	{
		deleteKeyChain (key);
	}

	[DllImport ("__Internal")]
	private static extern void setShareKeyChain (string _key, string _val, string _group);

	public static void BindSetShareKeyChain (string _key, string _val, string _group)
	{
		setShareKeyChain (_key, _val, _group);
	}

	[DllImport ("__Internal")]
	private static extern string getShareKeyChain (string key, string group);

	public static  string BindGetShareKeyChain (string key, string group)
	{
		return getShareKeyChain (key, group);
	}

	[DllImport ("__Internal")]
	private static extern void deleteShareKeyChain (string key, string group);

	public static  void BindDeleteShareKeyChain (string key, string group)
	{
		deleteShareKeyChain (key, group);
	}

			#endif
}
