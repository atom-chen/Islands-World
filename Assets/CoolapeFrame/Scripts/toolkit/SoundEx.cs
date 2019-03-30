/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   播放音效工具,需要绑定到某个对像上
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class SoundEx : MonoBehaviour
	{
		public static SoundEx self;
		//主音效，音乐audio
		public AudioSource mainAudio;
		//同时只能放一个音效
		public AudioSource singletonAudio;

        public CLDelegate OnSwitchChangeCallbacks = new CLDelegate();

		public SoundEx ()
		{
			self = this;
		}

		/// <summary>
		/// Plaies the sound with callback.播放音效，带完成回调方法
		/// </summary>
		/// <param name='go'>
		/// Go.
		/// </param>
		/// <param name='clip'>
		/// Clip.
		/// </param>
		/// <param name='volume'>
		/// Volume.
		/// </param>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		public static void PlaySoundWithCallback (AudioClip clip, float volume, object callback)
		{
			if (clip == null)
				return;
			NGUITools.PlaySound (clip, volume);
			self.StartCoroutine (DelayedCallback (clip, clip.length, callback));
		}

		static IEnumerator DelayedCallback (AudioClip clip, float time, object callback)
		{
			string cName = "";
			if (clip != null) {
				cName = clip.name;
			}
			yield return new WaitForSeconds (time);
			CLSoundPool.returnObj (cName);
			if (clip != null) {
				Utl.doCallback (callback, clip);
			}
		}

		static Hashtable playSoundCount = new Hashtable ();

        public static void playSound(string name)
        {
            playSound(name, 1, 1);
        }
		/// <summary>
		/// Plaies the sound.播放音效，可指定同时最大播放次数
		/// </summary>
		/// <param name='soundPath'>
		/// Sound path.
		/// </param>
		/// <param name='volume'>
		/// Volume.
		/// </param>
		/// <param name='maxTimes'>
		/// Max times.同时最大播放次数
		/// </param>
		public static void playSound (string name, float volume, int maxTimes = 1)
		{
			if (!soundEffectSwitch)
				return;
			if (self == null) {
				Debug.LogError ("Need Attack [SoundEx] to a gameObject");
				return;
			}
			if (!string.IsNullOrEmpty (name)) {
				CLSoundPool.borrowObjAsyn (name, (Callback)onFinishSetAudio, maxTimes, null);
//				AudioClip clip = CLSoundPool.borrowObj (name);
//				if (clip != null) {
//					doPlaySound (clip, volume, maxTimes);
//				} else {
//					CLSoundPool.borrowObjAsyn (name, (Callback)onFinishSetAudio, maxTimes, null);
//				}
			}
		}

		public static void doPlaySound (AudioClip clip, float volume, int maxTimes = 1)
		{
			if (clip == null)
				return;
			try {
				if (playSoundCount [clip.name] == null || (int)(playSoundCount [clip.name]) < maxTimes) {
					playSoundCount [clip.name] = (playSoundCount [clip.name] == null ? 1 : (int)(playSoundCount [clip.name]) + 1);
					Callback cb = finishPlaySound;
					PlaySoundWithCallback (clip, volume, cb);
				} else {
					CLSoundPool.returnObj (clip.name);
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public static void onFinishSetAudio (params object[] args)
		{
			if (args == null || args.Length == 0)
				return;

			AudioClip ac = ((AudioClip)args [1]); 
			int maxTimes = (int)(args [2]);
			if (ac != null) {
				string name = ac.name;
				doPlaySound (ac, 1, maxTimes);
			}
		}

		//只能同时播一个音乐/音效
		public static void playSoundSingleton (string name, float volume)
		{
			if (!soundEffectSwitch)
				return;
			if (self == null) {
				Debug.LogError ("Need Attack [SoundEx] to a gameObject");
				return;
			}
			if (self.singletonAudio == null) {
				Debug.LogError ("singletonAudio is Null");
				return;
			}
			try {
				self.singletonAudio.loop = false;
				if (self.singletonAudio.clip != null && self.singletonAudio.clip.name == name) {
					return;
				}
				self.singletonAudio.Stop ();
				if (!string.IsNullOrEmpty (name)) {
					CLSoundPool.borrowObjAsyn (name, (Callback)onFinishSetAudio4Singleton);
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		static void onFinishSetAudio4Singleton (params object[] args)
		{
			try {
				AudioClip ac = ((AudioClip)args [1]); 
				if (ac != null) {
					string name = ac.name;
					self.singletonAudio.clip = ac;
					self.singletonAudio.volume = 1;
					self.singletonAudio.Play ();
					self.StartCoroutine (DelayedCallback (ac, ac.length, (Callback)onFinishPlayAudio4Singleton));
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		static void onFinishPlayAudio4Singleton (params object[] args)
		{
			try {
				AudioClip ac = ((AudioClip)args [0]); 
				if (ac == null)
					return;
				if (self.singletonAudio.clip != null && self.singletonAudio.clip.name == ac.name) {
					self.singletonAudio.clip = null;
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

        public static void addCallbackOnSoundEffectSwitch(object callback)
        {
            self.OnSwitchChangeCallbacks.add("soundEffectSwitch", callback, null);
        }

        public static void removeCallbackOnSoundEffectSwitch(object callback)
        {
            self.OnSwitchChangeCallbacks.remove("soundEffectSwitch", callback);
        }
        public static void addCallbackOnMusicBgSwitch(object callback)
        {
            self.OnSwitchChangeCallbacks.add("musicBgSwitch", callback, null);
        }

        public static void removeCallbackOnMusicBgSwitch(object callback)
        {
            self.OnSwitchChangeCallbacks.remove("musicBgSwitch", callback);
        }

        public static void clean()
        {
            self.OnSwitchChangeCallbacks.removeDelegates("soundEffectSwitch");
            self.OnSwitchChangeCallbacks.removeDelegates("musicBgSwitch");
        }

        public static bool soundEffectSwitch {
			get {
				int f = PlayerPrefs.GetInt ("soundEffectSwitch", 0);
				return f == 0 ? true : false;
			}
			set {
                int oldVal = PlayerPrefs.GetInt("soundEffectSwitch", 0);
                int f = value ? 0 : 1;
				PlayerPrefs.SetInt ("soundEffectSwitch", f);
                if(oldVal != f) {
                    ArrayList list = self.OnSwitchChangeCallbacks.getDelegates("soundEffectSwitch");
                    NewList cell = null;
                    for (int i = 0; i < list.Count; i++) {
                        cell = list[i] as NewList;
                        if (cell != null && cell.Count > 0)
                        {
                            Utl.doCallback(cell[0], value);
                        }
                    }
                }
			}
		}

		public static bool musicBgSwitch {
			get {
				int f = PlayerPrefs.GetInt ("musicBgSwitch", 0);
				return f == 0 ? true : false;
			}
			set {
                int oldVal = PlayerPrefs.GetInt("musicBgSwitch", 0);
                int f = value ? 0 : 1;
				PlayerPrefs.SetInt ("musicBgSwitch", f);
                if(oldVal != f) {
                    ArrayList list = self.OnSwitchChangeCallbacks.getDelegates("musicBgSwitch");
                    NewList cell= null;
                    for (int i = 0; i < list.Count; i++)
                    {
                        cell = list[i] as NewList;
                        if (cell != null && cell.Count > 0)
                        {
                            Utl.doCallback(cell[0], value);
                        }
                    }
                }
			}
		}

		public static void playSound (AudioClip clip, float volume, int maxTimes = 1)
		{
			if (!soundEffectSwitch)
				return;
			
			if (self == null) {
				Debug.LogError ("Need Attack [SoundEx] to a gameObject");
				return;
			}

			if (clip == null)
				return;
			if (playSoundCount [clip.name] == null || (int)(playSoundCount [clip.name]) < maxTimes) {
				playSoundCount [clip.name] = (playSoundCount [clip.name] == null ? 1 : (int)(playSoundCount [clip.name]) + 1);
				Callback cb = finishPlaySound;
				PlaySoundWithCallback (clip, volume, cb);
			}
		}

		public static void playSound2 (string clipName, float volume)
		{
			AudioClip clip = Resources.Load (clipName) as AudioClip;
			NGUITools.PlaySound (clip, volume);
		}

		static void finishPlaySound (params object[] obj)
		{
			AudioClip clip = (AudioClip)(obj [0]);
			if (clip != null) {
				playSoundCount [clip.name] = (playSoundCount [clip.name] == null ? 0 : (int)(playSoundCount [clip.name])) - 1;
				playSoundCount [clip.name] = (int)(playSoundCount [clip.name]) < 0 ? 0 : playSoundCount [clip.name];
				//Resources.UnloadAsset(clip);
			}
		}

		//		-- 播放背景音乐---------------
		public static AudioClip mainClip;

		public static void onGetMainMusic (params object[] args)
		{
			string path = (string)(args [0]);
			mainClip = (AudioClip)(args [1]);
			if (mainClip == null) {
				return;
			}
			doPlayMainMusic (mainClip);
		}

		public static void stopMainMusic ()
		{
			if (self == null) {
				Debug.LogError ("Need Attack [SoundEx] to a gameObject");
				return;
			}
			if (self.mainAudio.isPlaying) {
                self.mainAudio.Pause ();
			}
			if (self.mainAudio.clip != null) {
				CLSoundPool.returnObj (self.mainAudio.clip.name);
				self.mainAudio.clip = null;
			}
		}

		public static void doPlayMainMusic (AudioClip clip)
		{
			try {
				if (self.mainAudio.clip != clip) {
					if (self.mainAudio.clip != null) {
						CLSoundPool.returnObj (self.mainAudio.clip.name);
					}
					self.mainAudio.Stop ();
					self.mainAudio.clip = clip;
					self.mainAudio.Play ();
				} else {
					if (!self.mainAudio.isPlaying) {
						self.mainAudio.Play ();
					}
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public static void playMainMusic (string soundName)
		{
			if (self == null) {
				Debug.LogError ("Need Attack [SoundEx] to a gameObject");
				return;
			}
			if (self.mainAudio.clip != null && self.mainAudio.clip.name == soundName) {
				if (SoundEx.musicBgSwitch) {
					if (!self.mainAudio.isPlaying) {
						self.mainAudio.Play ();
					}
				} else {
                    self.mainAudio.Pause ();
				}
				return;
			}
			if (SoundEx.musicBgSwitch) {
				CLSoundPool.borrowObjAsyn (soundName, (Callback)onGetMainMusic);
			}
		}
	}
}
