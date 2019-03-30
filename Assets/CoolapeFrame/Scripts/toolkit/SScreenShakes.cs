/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  屏幕抖动
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class SScreenShakes : MonoBehaviour
	{
		public static SScreenShakes self;
		public TweenPosition twPos;
		public Vector3 offset;
		object finishCallback;
		float timeS = 0.2f;
		Transform oldParent;
		Vector3 oldPos = Vector3.zero;
		bool isShaking = false;

		float strength = 1;

		public SScreenShakes()
		{
			self = this;
		}
	
		// Use this for initialization
		void Start()
		{
			init();
		}

		void init()
		{
			if (twPos == null) {
				twPos = GetComponent<TweenPosition>();
			}
			if (twPos == null) {
				twPos = gameObject.AddComponent<TweenPosition>();
				twPos.ignoreTimeScale = false;
			}
		
			twPos.method = UITweener.Method.EaseInOut;
			twPos.style = UITweener.Style.PingPong;
			twPos.duration = 0.03f;
			
			oldPos = transform.position;
			oldParent = transform.parent;
//		oldPos = transform.localPosition;
			twPos.from = oldPos - offset * strength;
			twPos.to = oldPos + offset * strength;
			twPos.enabled = false;
		}

		public static void play(object finishCallback, float delay)
		{
			play(finishCallback, delay, 1, false);
		}

		public static void play(object finishCallback, float delay, float strength)
		{
			play(finishCallback, delay, strength, false);
		}

		public static void play(object finishCallback, float delay, float strength, bool loop)
		{
			self._play(finishCallback, delay, strength, loop);
		}

		public static void stop()
		{
			self.onFinish();
		}

		public void _play(object finishCallback, float delay, float strength, bool loop)
		{
			StartCoroutine(doShakes(finishCallback, delay, strength, loop));
		}

		public IEnumerator doShakes(object finishCallback, float delay, float strength, bool loop)
		{
			yield return new WaitForSeconds(delay);
			if (isShaking) {
				doFinishCallback();
			} else {
				isShaking = true;
				this.strength = strength;
				init();
				this.finishCallback = finishCallback;
				twPos.worldSpace = true;
				twPos.enabled = true;
				if (!loop) {
					CancelInvoke("onFinish");
					Invoke("onFinish", timeS);
				}
			}
		}

		public void doFinishCallback()
		{
			Utl.doCallback(finishCallback, this);
		}

		public void onFinish()
		{
			twPos.enabled = false;
			if (oldParent == transform.parent) {
				transform.localPosition = Vector3.zero;
			} else {
				transform.position = oldPos;
			}
			twPos.worldSpace = false;
			isShaking = false;
			doFinishCallback();
		}
	}
}
