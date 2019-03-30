using UnityEngine;
using System.Collections;
using System;

namespace Coolape
{
	/// <summary>
	/// Push message local IO.本地推送
	/// </summary>
	public static class CLPushMsgLocal
	{
		public static string strClass = "com.coolape.pushmsg.CBPushServer";
		#if UNITY_ANDROID && !UNITY_EDITOR
		public static AndroidJavaClass jpushClass = new AndroidJavaClass(strClass);
		#endif

		public static void init (string uid,  string longTimeNotLoginMsg, string notifyAndroidPackageName)
		{
			try{
#if UNITY_ANDROID  && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.Android) {
			AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
		
			jpushClass.CallStatic("init", uid, longTimeNotLoginMsg, notifyAndroidPackageName, currActivity);
		}
#endif

#if UNITY_IOS
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				UnityEngine.iOS.NotificationServices.RegisterForNotifications (
					UnityEngine.iOS.NotificationType.Alert | 
					UnityEngine.iOS.NotificationType.Badge | 
					UnityEngine.iOS.NotificationType.Sound, true);
			}
#endif
			} catch(Exception e) {
				Debug.LogError (e);
			}
		}

		public static void cancelAll ()
		{
			#if UNITY_IOS
				if (Application.platform == RuntimePlatform.IPhonePlayer) {
					UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications ();
				}
			#elif UNITY_ANDROID  && !UNITY_EDITOR
				if (Application.platform == RuntimePlatform.Android) {
					jpushClass.CallStatic("cleanAllMsg");
				}
			#endif
		}

		public static void clearLocal ()
		{
			#if UNITY_IOS
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
			}
			#elif UNITY_ANDROID  && !UNITY_EDITOR
			if (Application.platform == RuntimePlatform.Android) {
			jpushClass.CallStatic("cleanAllMsg");
			}
			#endif

		}
		public static void clearRemote ()
		{
			#if UNITY_IOS
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				UnityEngine.iOS.NotificationServices.ClearRemoteNotifications();
			}
			#elif UNITY_ANDROID  && !UNITY_EDITOR
			if (Application.platform == RuntimePlatform.Android) {
			jpushClass.CallStatic("cleanAllMsg");
			}
			#endif
		}

		public static void cancelNotifyByMsg (string msg)
		{
			if (string.IsNullOrEmpty(msg)) {
				return;
			}
#if UNITY_IOS
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				UnityEngine.iOS.LocalNotification[] lns = UnityEngine.iOS.NotificationServices.localNotifications;
				UnityEngine.iOS.LocalNotification cell = null;
				for (int i = 0; i < lns.Length; i++) {
					cell = lns [i];
					if (cell.alertBody.Equals(msg)) {
						UnityEngine.iOS.NotificationServices.CancelLocalNotification (cell);
						return;
					}
				}
			}
#elif UNITY_ANDROID  && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.Android) {
		jpushClass.CallStatic("cancelNotification", msg);
		}
#endif
		}

		/// <summary>
		/// Schedules the local notification.创建一个本地定时通知
		/// </summary>
		/// <param name='msg'>
		/// Message.
		/// </param>
		/// <param name='fireSeconds'>
		/// Fire seconds.
		/// </param>
		public static void scheduleLocalNotification (string msg, long fireDelaySeconds)
		{
#if UNITY_IOS
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				// schedule notification to be delivered in 10 seconds
				UnityEngine.iOS.LocalNotification notif = new UnityEngine.iOS.LocalNotification ();
				notif.fireDate = DateTime.Now.AddSeconds (fireDelaySeconds);
				notif.alertBody = msg;
				notif.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
				UnityEngine.iOS.NotificationServices.ScheduleLocalNotification (notif);
			}
#elif UNITY_ANDROID  && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.Android) {
				jpushClass.CallStatic("setMsg", msg, fireDelaySeconds);
		}
#endif
		}

		public static byte[] deviceToken {
			get {
				#if UNITY_IOS
				return UnityEngine.iOS.NotificationServices.deviceToken;
				#endif
				return null;
			}
		}

		public static string deviceTokenStr2 {
			get {
				byte[] bytes = deviceToken;
				if (bytes != null) {
//					return System.Text.UTF8Encoding.Default.GetString (bytes);
					string hexToken = "%" + System.BitConverter.ToString(bytes).Replace('-', '%');
					return hexToken;
				}
				return "";
			}
		}

		public static string deviceTokenStr {
			get {
				byte[] bytes = deviceToken;
				if (bytes != null) {
					//					return System.Text.UTF8Encoding.Default.GetString (bytes);
					return System.BitConverter.ToString(bytes).Replace("-", "");
				}
				return "";
			}
		}
	}
}
