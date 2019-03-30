using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IPHONE || UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

namespace Coolape
{
	public class CLGameCenter
	{
		public static bool authenticated = false;
		public static string userInfo = "";

		/// <summary>  
		/// 初始化 GameCenter 登陆  
		/// </summary>  
		public static void authenticate (object callback, object orgs)
		{
			#if UNITY_IPHONE|| UNITY_IOS
			Social.localUser.Authenticate (success => {
				if (success) {
					authenticated = Social.localUser.authenticated;
					Debug.Log ("Authentication successful");
					Hashtable m = new Hashtable ();
					m ["Username"] = Social.localUser.userName;
					m ["UserID"] = Social.localUser.id;
					m ["IsUnderage"] = Social.localUser.underage;
					m ["state"] = Social.localUser.state.ToString ();
					userInfo = JSON.JsonEncode (m);
					Debug.Log (userInfo);
					Utl.doCallback (callback, success, userInfo, orgs);
				} else {
					Debug.Log ("Authentication failed");
					userInfo = "";
					Utl.doCallback (callback, success, userInfo, orgs);
				}
			});
			#endif
		}

		/// <summary>
		/// Shows the achievements U.打开成就
		/// </summary>
		public static void showAchievementsUI ()
		{
			#if UNITY_IPHONE|| UNITY_IOS
			if (Social.localUser.authenticated) {  
				Social.ShowAchievementsUI ();
			} else {
				Debug.LogWarning ("Social.localUser.authenticated==" + Social.localUser.authenticated);
			}
			#endif
		}

		/// <summary>
		/// Shows the leaderboard U.打开排行榜
		/// </summary>
		public static void showLeaderboardUI ()
		{
			#if UNITY_IPHONE|| UNITY_IOS
			if (Social.localUser.authenticated) {
				Social.ShowLeaderboardUI ();
			} else {
				Debug.LogWarning ("Social.localUser.authenticated==" + Social.localUser.authenticated);
			}
			#endif
		}

		/// <summary>
		/// Reports the score.排行榜设置分数
		/// </summary>
		/// <param name="sore">Sore.</param>
		/// <param name="board">Board.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="orgs">Orgs.</param>
		public static void reportScore (long sore, string board, object callback, object orgs)
		{
			#if UNITY_IPHONE|| UNITY_IOS
			if (Social.localUser.authenticated) {
				Social.ReportScore (sore, board, success => {
					Utl.doCallback (callback, success, orgs);
				});
			} else {
				Debug.LogWarning ("Social.localUser.authenticated==" + Social.localUser.authenticated);
				Utl.doCallback (callback, false, orgs, "Social.localUser.authenticated==" + Social.localUser.authenticated);
			}
			#endif
		}

		/// <summary>
		/// Reports the progress.设置成就
		/// </summary>
		/// <param name="activeId">Active identifier.</param>
		/// <param name="progress">Progress.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="orgs">Orgs.</param>
		public static void reportProgress (string activeId, double progress, object callback, object orgs)
		{
			#if UNITY_IPHONE|| UNITY_IOS
			if (Social.localUser.authenticated) {
				Social.ReportProgress (activeId, progress, success => {
					Utl.doCallback (callback, success, orgs);
				});
			} else {
				Debug.LogWarning ("Social.localUser.authenticated==" + Social.localUser.authenticated);
				Utl.doCallback (callback, false, orgs, "Social.localUser.authenticated==" + Social.localUser.authenticated);
			}
			#endif
		}
	}
}
