/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  摄像机视图FieldView的变化
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class TweenCameraZoom : MonoBehaviour
	{
		public float speed = 1;
		public Camera camera;
		public Camera subCamera;
		public bool ignoreTimeScale = true;
	
		float midFieldOfView = 60;
		float toFieldOfView = 60;
		bool isPlaying = false;
		float element = 1;
		object onFinsihCallback;
		bool needBack = false;
		float staySeconds = 0;
		bool isPlayBack = false;

		// Use this for initialization
		void Start ()
		{
			enabled = false;
		}

		float tmpField = 0;
		bool isFinished = false;
		float toFiled = 0;
		float deltaTime = 0;

		void Update ()
		{
			if (!isPlaying) {
				return;
			}
			deltaTime = ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
			tmpField = camera.fieldOfView;
			tmpField += element * deltaTime * speed * 50;
			toFiled = toFieldOfView;
			if (needBack && !isPlayBack) {
				toFiled = midFieldOfView;
			}
			if ((element > 0 && tmpField - toFiled > 0)
			    ||	(element < 0 && tmpField - toFiled < 0)) {
				tmpField = toFiled;
				isFinished = true;
			}
			camera.fieldOfView = tmpField;
			if (subCamera != null) {
				subCamera.fieldOfView = tmpField;
			}
			if (isFinished) {
				if (needBack) {
					if (!isPlayBack) {
						isPlayBack = true;
						isPlaying = false;
						Invoke ("playBack", staySeconds);
					} else {
						onFinish ();
					}
				} else {
					onFinish ();
				}
			}
		}

		void playBack ()
		{
			zoom (toFieldOfView, speed, onFinsihCallback);
		}

		void onFinish ()
		{
			isPlaying = false;
			enabled = false;
			Utl.doCallback (onFinsihCallback, this);
		}

		public void zoom (float zoomValue, float speed, object callback)
		{
			this.speed = speed;
			toFieldOfView = zoomValue;
			onFinsihCallback = callback;
			element = zoomValue - camera.fieldOfView > 0 ? 1 : (zoomValue - camera.fieldOfView == 0 ? 0 : -1);

			needBack = false;
			isFinished = false;
			enabled = true;
			isPlaying = true;
		}

		public void zoomFromTo (float mid, float to, float staySeconds, float speed, object callback)
		{
			midFieldOfView = mid;
			toFieldOfView = to;
			this.staySeconds = staySeconds;
			this.speed = speed;
			onFinsihCallback = callback;
			element = mid - camera.fieldOfView > 0 ? 1 : (mid - camera.fieldOfView == 0 ? 0 : -1);

			needBack = true;
			isFinished = false;
			isPlaying = true;
			enabled = true;
			isPlayBack = false;
		}
	}
}