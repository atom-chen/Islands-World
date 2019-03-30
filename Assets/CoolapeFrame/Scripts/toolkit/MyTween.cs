/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  一个简单的位置，支持高度单独控制，及影子的处理，因此可以处理击飞之类的位移
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class MyTween : MonoBehaviour
	{
		public float speed = 1;
		public float turningSpeed = 1;
		public float high = 0;
		Vector3 highV3 = Vector3.zero;
		public LayerMask obstrucLayer;
		public float obsDistance = 0.5f;
		public Transform shadow;
		//影子，当移动时，影子的高度不变
		public float shadowHeight = 0;
		public AnimationCurve curveSpeed = new AnimationCurve (new Keyframe (0, 0, 0, 1), new Keyframe (1, 1, 1, 0));
		public AnimationCurve curveHigh = new AnimationCurve (new Keyframe (0, 0, 0, 4), new Keyframe (0.5f, 1, 0, 0), new Keyframe (1, 0, -4, 0));
		public object orgParams;
		public bool ignoreTimeScale = false;
		//透传参数
		object onFinishCallback;
		object onMovingCallback;
		Vector3 origin = Vector3.zero;
		Vector3 v3Diff = Vector3.zero;
		float curveTime = 0;
		public bool isMoveNow = false;
		bool isWoldPos = true;
		bool isTurning = false;
		Vector3 subDiff = Vector3.zero;
		Vector3 subDiff4Shadow = Vector3.zero;

		public bool runOnStart = false;
		public UITweener.Style style = UITweener.Style.Once;
		public Vector3 from = Vector3.zero;
		public Vector3 to = Vector3.zero;
		int flag = 1;
		bool isMoveForward = false;

		// cach transform
		Transform _transform;

		public Transform transform {
			get {
				if (_transform == null) {
					_transform = gameObject.transform;
				}
				return _transform;
			}
		}

	
		void Start ()
		{
			//		isMoveNow = false;
			//		enabled = false;
		
			if (runOnStart) {
				doTween ();
			}
		}

		void doTween ()
		{
			float dis = Vector3.Distance (from, to);
			object onFinishCallback = null;
			if (style == UITweener.Style.Loop) {
				flag = 1;
				transform.position = from;
				flyout (from, to, dis, speed, high, 0, null, onFinishCallback, true);
			} else if (style == UITweener.Style.PingPong) {
				if (flag > 0) {
					transform.position = from;
					flyout (from, to, dis, speed, high, 0,null, onFinishCallback, true);
				} else {
					transform.position = to;
					flyout (to, from, dis, speed, high, 0,null, onFinishCallback, true);
				}
			} else if (style == UITweener.Style.Once) {
				return;
			} else {
				return;
			}
		}

		public void flyout (Vector3 dirFrom, Vector3 dirTo, float distance, float speed, float hight, float angleOffset, 
		                    object onMovingCallback, object finishCallback, bool isWoldPos)
		{
			flyout (dirTo - dirFrom, distance, speed, hight, angleOffset, onMovingCallback, finishCallback, isWoldPos);
		}

		public void flyout (Vector3 dir, float distance, float speed, float hight, float angleOffset,
		                    object onMovingCallback, object finishCallback, bool isWoldPos)
		{
			Vector3 v2 = Vector3.zero;
			if (isWoldPos) {
				v2 = transform.position;
			} else {
				v2 = transform.localPosition;
			}
//			dir.y = 0;
			Vector3 v3 = v2 + dir.normalized * distance;
			flyout (v3, speed, hight, angleOffset, onMovingCallback, finishCallback, isWoldPos);
		}

		//弹出
		public  void flyout (Vector3 toPos, float speed, float ihight, 
		                     object onMovingCallback, object finishCallback, bool isWoldPos)
		{
			flyout (toPos, speed, ihight, 0, onMovingCallback, finishCallback, null, isWoldPos);
		}

		public  void flyout (Vector3 toPos, float speed, float ihight, float angleOffset,
		                     object onMovingCallback, object finishCallback, bool isWoldPos)
		{
			flyout (toPos, speed, ihight, angleOffset, onMovingCallback, finishCallback, null, isWoldPos);
		}

		//弹出
		public  void flyout (Vector3 toPos, float speed, float ihight, float angleOffset,
		                     object onMovingCallback, object finishCallback, object orgs, bool isWoldPos)
		{
			this.orgParams = orgs;
			this.onFinishCallback = finishCallback;
			this.onMovingCallback = onMovingCallback;
			this.speed = speed;
			this.high = ihight;
			this.isWoldPos = isWoldPos;
			isMoveForward = false;

			if (isWoldPos) {
				origin = transform.position;
				v3Diff = toPos - transform.position;
			} else {
				origin = transform.localPosition;
				v3Diff = toPos - transform.localPosition;
			}

			if (angleOffset > -0.00001f && angleOffset < 0.00001f) {
				highV3 = new Vector3 (0, high, 0);
			} else {
				Vector3 center = origin + v3Diff / 2.0f;
				Vector3 _v3 = Utl.RotateAround (center + new Vector3 (0, high, 0), center, v3Diff, angleOffset * Mathf.Sin (Mathf.Deg2Rad * Utl.getAngle (v3Diff).y));
				highV3 = _v3 - center;
			}

			curveTime = 0;
		
			isMoveNow = true;
			enabled = true;
		}

		public void refreshToPos(Vector3 toPos, float angleOffset = 0) {
			if (isWoldPos) {
				v3Diff = toPos - origin;
			} else {
				v3Diff = toPos - origin;
			}
			if (angleOffset > -0.00001f && angleOffset < 0.00001f) {
				highV3 = new Vector3 (0, high, 0);
			} else {
				Vector3 center = origin + v3Diff / 2.0f;
				Vector3 _v3 = Utl.RotateAround (center + new Vector3 (0, high, 0), center, v3Diff, angleOffset * Mathf.Sin (Mathf.Deg2Rad * Utl.getAngle (v3Diff).y));
				highV3 = _v3 - center;
			}
		}

		public void onFinishTween ()
		{
			enabled = false;
			isMoveNow = false;
			Utl.doCallback (onFinishCallback, this);
		}

		public void stop ()
		{
			isMoveNow = false;
		}

		public void Update ()
		{
			if (ignoreTimeScale && Time.deltaTime < 0.0001f) {
				FixedUpdate ();
			}
		}

		RaycastHit hitInfor;
		Vector3 dis = Vector3.zero;
		Vector3 shadowPos = Vector3.zero;
		float fixedDeltaTime = 0;
		// Update is called once per frame
		public void FixedUpdate ()
		{
			fixedDeltaTime = ignoreTimeScale ? Time.unscaledDeltaTime : Time.fixedDeltaTime;
			if (isMoveForward) {
				if (!Physics.Raycast (transform.position, transform.forward, out hitInfor, obsDistance, obstrucLayer.value)) {
					transform.Translate (Vector3.forward.x * 0.017f * speed, 0, Vector3.forward.z * 0.017f * speed);
					Utl.doCallback (onMovingCallback, this);
				}
			} else {
				if (!isMoveNow) {
					return;
				}
				curveTime += fixedDeltaTime * speed;
				subDiff = 					v3Diff * curveSpeed.Evaluate (curveTime);//*SCfg.self.fps.fpsRate;
				subDiff4Shadow = 	v3Diff * curveSpeed.Evaluate (curveTime);
				//		subDiff = v3Diff.normalized*curveSpeed.Evaluate(curveTime)*Time.deltaTime;
				subDiff += highV3 * curveHigh.Evaluate (curveTime);//*SCfg.self.fps.fpsRate;
				if (isWoldPos) {
					dis = origin + subDiff - transform.position;
				} else {
					dis = origin + subDiff - transform.localPosition;
				}
				//		Debug.DrawLine(transform.position, transform.position + v3Diff.normalized*(dis.magnitude + obsDistance));
				if (!Physics.Raycast (transform.position, v3Diff, out hitInfor, dis.magnitude + obsDistance, obstrucLayer.value)) {
					if (isWoldPos) {
						transform.position = origin + subDiff;
						if (shadow != null) {
							shadow.position = origin + subDiff4Shadow;
						}
						//			transform.Translate(subDiff, Space.World);
					} else {
						transform.localPosition = origin + subDiff;
					}
				} else {
					// 虽然遇到了障碍，但是高度还是需要计算，不然就会变成人越来越高
					subDiff.x = 0;
					subDiff.z = 0;
					subDiff4Shadow.x = 0;
					subDiff4Shadow.z = 0;
					if (isWoldPos) {
						transform.position = transform.position + subDiff;
						if (shadow != null) {
							shadow.position = shadow.position + subDiff4Shadow;
						}
					} else {
						transform.localPosition = transform.localPosition + subDiff;
					}
				}

				if (shadow != null) {
					shadowPos = shadow.position;
					shadowPos.y = shadowHeight;
					shadow.position = shadowPos;
				}

				Utl.doCallback (onMovingCallback, this);

				if (isTurning) {
					Utl.RotateTowards (transform, v3Diff, turningSpeed);
				}
				if (curveTime >= 1) {
					onFinishTween ();
				}
			}
		}


		public void moveForward (float speed)
		{
			moveForward (speed, null);
		}

		public void moveForward (float speed, object onMovingCallback)
		{
			this.onMovingCallback = onMovingCallback;
			this.speed = speed;
			isMoveForward = true;
			enabled = true;
		}

		public void stopMoveForward ()
		{
			isMoveForward = false;
		}
	}
}
