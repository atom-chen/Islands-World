/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  处理animaiton的播放及完成回调
  *Others:  
  *History:
*********************************************************************************
*/ 
using System;
using UnityEngine;

namespace Coolape
{
	public class AnimationProc : MonoBehaviour
	{
		public object onFinish;
		public object callbackPara;
		//回调数
		bool canFixedUpdate = false;
		public bool isLoop = false;
		public bool isDestroySelf;
		public float timeOut = 0;
		ParticleSystem[] particleSystems;
		public bool ignoreTimeScale = false;

		long frameCounter = 0;
		long hideFrame = 0;
		[HideInInspector]
		public int id;
		Animation _animation;

		Animation animation {
			get {
				if (_animation != null) {
					_animation = GetComponent<Animation> ();
				}
				return _animation;
			}
		}

		// Use this for initialization
		void Start ()
		{
			particleSystems = GetComponentsInChildren<ParticleSystem> ();
			if (ignoreTimeScale) {
				Animator[] animators = GetComponentsInChildren<Animator> ();
				if (animators != null) {
					for (int i = 0; i < animators.Length; i++) {
						animators [i].updateMode = AnimatorUpdateMode.UnscaledTime;
					}
				}
			}
		}

		public void init (object  finishCallback, object callbackPara)
		{
			onFinish = finishCallback;
			this.callbackPara = callbackPara;
		}

		void OnEnable ()
		{
			frameCounter = 0;
			if (timeOut > 0) {
				hideFrame = Mathf.FloorToInt (timeOut / Time.fixedDeltaTime);
			}
			if (animation != null) {
				animation.Play ();
			}
			canFixedUpdate = true;
		}

		float curtTime = 0;

		void FixedUpdate ()
		{
			if (!canFixedUpdate)
				return;
			frameCounter++;
			if (timeOut > 0) {
				if (isDestroySelf) {
					if (frameCounter - hideFrame >= 0) {
						onFinishPlay ();
						Destroy (gameObject);
					}
				} else {
					if (frameCounter - hideFrame >= 0) {
						gameObject.SetActive (false);
						onFinishPlay ();
					}
				}
			} else {
				if (isDestroySelf) {
					if (animation != null && !animation.isPlaying) {
						onFinishPlay ();
						Destroy (gameObject);
					}
				} else {
					if (animation != null && !animation.isPlaying) {
						gameObject.SetActive (false);
						onFinishPlay ();
					}
				}
			}
		}

		void Update ()
		{
			if (!ignoreTimeScale)
				return;
			if (Time.timeScale < 0.001f) {
				if (particleSystems != null) {
					for (int i = 0; i < particleSystems.Length; i++) {
						particleSystems [i].Simulate (Time.unscaledDeltaTime, true, false);
					}
				}
				FixedUpdate ();
			}
		}

		void onFinishPlay ()
		{
			canFixedUpdate = false;
			Utl.doCallback (onFinish, this);
		}
	}
}