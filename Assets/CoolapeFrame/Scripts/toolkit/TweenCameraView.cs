/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  摄像机视图rect的变化
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class TweenCameraView : MonoBehaviour
	{
		public float speed = 1;
		public Camera camera;
		public bool ignoreTimeScale = true;
		Vector2 tmppos;
		Vector2 tmpsize;
		float deltaTime = 0;
		float dt = 0;

		public Camera mCamera {
			get {
				if (camera == null) {
					GetComponent<Camera> ();
				}
				return camera;
			}
		}

		// Update is called once per frame
		void Update ()
		{
			if (isPlayNow) {
				deltaTime = ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
				float dt = deltaTime * speed;
				offset += dt;
				tmppos += diffPos * dt;
				tmpsize += diffSize * dt;
				mCamera.rect = new Rect (tmppos.x, tmppos.y, tmpsize.x, tmpsize.y);
				if (offset >= 1) {
					isPlayNow = false;
					tmppos = oldpos + diffPos;
					tmpsize = oldsize + diffSize;
					mCamera.rect = new Rect (tmppos.x, tmppos.y, tmpsize.x, tmpsize.y);
				}
			}
		}

		Vector2 diffPos;
		Vector2 diffSize;
		float offset = 0;
		Vector2 oldpos;
		Vector2 oldsize;
		bool isPlayNow = false;

		public void play (Rect toRect, float speed)
		{
			if (mCamera == null) {
				Debug.LogError ("can not find camera, this script muct binding a camera");
				return;
			}
			this.speed = speed;
			oldpos = new Vector2 (mCamera.rect.x, mCamera.rect.y);
			oldsize = new Vector2 (mCamera.rect.width, mCamera.rect.height);
			tmppos = oldpos;
			tmpsize = oldsize;
		
			diffPos = new Vector2 (toRect.x, toRect.y) - oldpos;
			diffSize = new Vector2 (toRect.width, toRect.height) - oldsize;
			offset = 0;
			isPlayNow = true;
		}
	}
}