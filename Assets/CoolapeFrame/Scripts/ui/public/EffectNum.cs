/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  canyou
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   数字动态显示效果，比如一个label的值是0，当设置成100时，有一个从0增到100的过程
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;
using System;

namespace Coolape
{
	[RequireComponent (typeof(UILabel))]
	public class EffectNum : MonoBehaviour
	{

		UILabel labSelf;
		[HideInInspector]
		protected Transform myTransform;

		public bool isGui = false;
		public AnimationCurve speedCurve = new AnimationCurve (new Keyframe (0f, 0f, 0f, 1f), new Keyframe (1f, 1f, 1f, 0f));
		private float timeAdd = 0.05f;
		private float timeVal = 0f;
	
		private int fromVal = 0;
		private int toVal = 0;

		private bool isAdd = true;
		private int speed = 0;
		private float etime = 0.02f;
		//每次执行时间
		private bool isFirst = true;
		// 首次执行
		private float firstTime = 0.15f;
		//首次执行等待时间

		object callFun;

		void Awake ()
		{
			labSelf = gameObject.GetComponent<UILabel> ();
		}

		// Use this for initialization
		void Start ()
		{
			myTransform = gameObject.transform;
		}
	
		// Update is called once per frame
		//	void Update () {
		//
		//	}
		
		void changeNum (int valNum)
		{
			fromVal = fromVal + valNum;
			if ((isAdd && fromVal > toVal) || (!isAdd && fromVal < toVal)) {
				fromVal = toVal;
			}
			labSelf.text = "" + fromVal;
		}

		float timeGet {
			get {
				timeVal += timeAdd;
				float tmp = timeVal % 2;
				if (tmp > 1) {
					return 2 - tmp;
				}
				return tmp;
			}
		}

		IEnumerator effect ()
		{
			bool isNull = true;
			if (isFirst) {
				isFirst = false;
				labSelf.text = "" + fromVal;
				yield return new WaitForSeconds (firstTime);
			}
			if (toVal != fromVal) {
				isNull = false;
				float vCurve = speedCurve.Evaluate (timeGet);
				int chng = Mathf.CeilToInt (vCurve * speed);

				changeNum (chng);

				yield return new WaitForSeconds (etime);

				StartCoroutine (effect ());
			}

			if (isNull) {
				StopCoroutine (effect ());
				doCallback ();
			}
			yield return null;
		}

		void doCallback ()
		{
			Utl.doCallback (callFun, this);
		}
		//重新计算速度
		void reckonSpeed ()
		{
			float absSpeed = Mathf.Abs (speed);
			if (absSpeed < 100) {
				return;
			}
			if (absSpeed > 100000) {
				speed = speed / 12;
			} else if (absSpeed > 50000) {
				speed = speed / 10;
			} else if (absSpeed > 10000) {
				speed = speed / 8;
			} else if (absSpeed > 5000) {
				speed = speed / 6;
			} else if (absSpeed > 1000) {
				speed = speed / 4;
			} else {
				speed = speed / 2;
			}
		}

		public void effectStart (int to, object back, float delayTime = 0)
		{
			string vStr = labSelf.text;
			int from = NumEx.toInt (vStr);
			effectStart (from, to, back, delayTime);
		}

		public void effectStart (int from, int to, object back, float delayTime = 0)
		{
			speed = 0;
			fromVal = from;
			timeVal = 0f;
			isAdd = to > from;
			speed = (to - from);
			float absSpeed = Mathf.Abs (speed);
			bool isSimilar = absSpeed <= 5;
			isFirst = true;
			callFun = back;
			toVal = to;

			labSelf.text = fromVal.ToString ();
			if (isSimilar) {
				labSelf.text = toVal.ToString ();
				doCallback ();
			} else {
				reckonSpeed ();
				CancelInvoke ("doEffect");
				Invoke ("doEffect", delayTime);
			}
		}

		void doEffect ()
		{
			if (gameObject.activeInHierarchy) {
				StartCoroutine (effect ());
			}
		}

		void OnDisable ()
		{
			StopAllCoroutines ();
			CancelInvoke ();
		}

		#if UNITY_EDITOR
		void OnGUI ()
		{
			if (isGui) {
				Callback cb = backFunTest;
				bool is1 = GUI.Button (new Rect (20, 10, 200, 30), "10000to100");
				if (is1) {
					effectStart (10000, 100, cb);
				}

				bool is2 = GUI.Button (new Rect (20, 50, 200, 30), "1000to10000");
				if (is2) {
					effectStart (1000, 10000, cb);
				}

				bool is3 = GUI.Button (new Rect (20, 90, 200, 30), "10000to10000");
				if (is3) {
					effectStart (10000, 10000, cb);
				}

				bool is4 = GUI.Button (new Rect (20, 130, 200, 30), "0to100000");
				if (is4) {
					effectStart (0, 100000, cb);
				}

				bool is5 = GUI.Button (new Rect (20, 170, 200, 30), "100to200");
				if (is5) {
					effectStart (100, 200, cb);
				}

				bool is6 = GUI.Button (new Rect (20, 210, 200, 30), "100t500");
				if (is6) {
					effectStart (100, 500, cb);
				}
			}
		}

		void backFunTest (params object[] objs)
		{
			Debug.LogWarning ("timeVal:" + timeVal);
		}
		#endif
	}
}
