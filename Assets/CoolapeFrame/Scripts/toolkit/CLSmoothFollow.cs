/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  软跟随
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;

namespace Coolape
{
	public class CLSmoothFollow : MonoBehaviour
	{
		// The target we are following
		public Transform target;

		// The distance in the x-z plane to the target
		public float distance = 10.0f;
		// the height we want the camera to be above the target
		public float height = 5.0f;
		// How much we
		public float heightDamping = 2.0f;
		public float rotationDamping = 3.0f;
//		public float moveDamping = 2.0f;
		public Vector3 offset = Vector3.zero;
		public bool isCanRotate = true;
		public bool isRole = false;
		float wantedRotationAngle = 0;
		float wantedHeight = 0;
		float currentRotationAngle = 0;
		float currentHeight = 0;
		Quaternion currentRotation;
		Vector3 pos = Vector3.zero;
		Vector3 localAngle = Vector3.zero;

		//		public void Update ()
		public void LateUpdate ()
		{
			// Early out if we don't have a target
			if (!target)
				return;
	
			// Calculate the current rotation angles
			wantedRotationAngle = target.eulerAngles.y;
			wantedHeight = target.position.y + height;
		
			currentRotationAngle = transform.eulerAngles.y;
			currentHeight = transform.position.y;
	
			if (Mathf.Abs (wantedRotationAngle - currentRotationAngle) < 160 || !isRole) {
				// Damp the rotation around the y-axis
				currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
			}

			// Damp the height
			currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);

			// Convert the angle into a rotation
			currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
	
			// Set the position of the camera on the x-z plane to:
			// distance meters behind the target
			if (isCanRotate) {
				var newPos = target.position;
				newPos -= currentRotation * Vector3.forward * distance;
				transform.position = newPos;//Vector3.Lerp (transform.position, newPos, Time.deltaTime*moveDamping);
			} else {
				var newPos = target.position;
				newPos.y -= distance;
				newPos.z -= distance;
				//newPos.x -= 5;
				transform.position = newPos;
			}

			// Set the height of the camera
			pos = transform.position;
			pos.y = currentHeight;
			transform.position = pos + offset;
	
			// Always look at the target
			if (isCanRotate) {
				if (distance > -0.00001f && distance < 0.00001f) {
					transform.LookAt (target);
					localAngle = transform.localEulerAngles;
					localAngle.y = target.localEulerAngles.y;
					transform.localEulerAngles = localAngle;
				} else {
					transform.LookAt (target);
				}
			}
		}

		//===============================
		Vector2 diff4Tween = Vector2.zero;
		Vector2 from4Tween = Vector2.zero;
		Vector2 to4Tween = Vector2.zero;
		Vector2 tmpTo = Vector2.zero;
		float speed4Tween = 1;
		bool isDoTween = false;
		object finishTweenCallback = null;
        object progressCallback = null;
        float totalDeltaVal = 0;

		void FixedUpdate ()
		{
			if (!isDoTween)
				return;
			totalDeltaVal += Time.deltaTime * speed4Tween * 0.33f;
			if (totalDeltaVal >= 1) {
				totalDeltaVal = 1;
			}
			tmpTo = from4Tween + diff4Tween * totalDeltaVal;
			distance = tmpTo.x;
			height = tmpTo.y;
            Utl.doCallback(progressCallback, tmpTo);
            if (totalDeltaVal >= 1) {
				isDoTween = false;
				Utl.doCallback (finishTweenCallback, this);
			}
		}

        /// <summary>
        /// Tween the specified from, to, speed and callback.
        /// </summary>
        /// <param name="from">from.x 是distance的开始值；from.y是heigh的开始值 </param>
        /// <param name="to">to.x 是distance的结束值；to.y是heigh的结束值.</param>
        /// <param name="speed">Speed.</param>
        /// <param name="callback">Callback.</param>
		public void tween (Vector2 from, Vector2 to, float speed, object callback, object progressCallback)
		{
			from4Tween = from;
			to4Tween = to;
			speed4Tween = speed; 
			diff4Tween = to - from;
			finishTweenCallback = callback;
            this.progressCallback = progressCallback;
            distance = from.x;
			height = from.y;
			totalDeltaVal = 0;
			isDoTween = true;
		}
	}
}
