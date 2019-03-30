/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   角色动作
/// 注意：	通过tag来判断当前正在执行状态是否为设置的状态，
/// 			因此在创建控制器中要把状态的tag的值与该文件中的ActionValue保持一至
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Coolape
{
	public class CLRoleAction : MonoBehaviour
	{
		Animator _animator = null;

		public Animator animator {
			get {
				if (_animator == null) {
					_animator = GetComponent<Animator>();
				}
				return _animator;
			}
		}

		public enum Action
		{
			idel,
			//0 空闲
			idel2,
			//1 空闲
			walk,
			//2 走
			run,
			//3 跑
			jump,
			//4 跳
			slide,
			//5 滑行，滚动,
			drop,
			//6 下落,击飞
			attack,
			//7 攻击
			attack2,
			//8 攻击2
			skill,
			//9 技能
			skill2,
			//10 技能2
			skill3,
			//11 技能3
			skill4,
			//12 技能4
			hit,
			//13 受击
			dead,
			//14 死亡
			happy,
			//15 高兴
			sad,
			//16 悲伤
			up,
			//17 起立
			down,
			//18 倒下
			biggestAK,
			//19 最大的大招
			dizzy,
			//20 晕
			stiff,
			//21 僵硬
			idel3,
			//22 空闲
		}

		public static Action getActByName(string name)
		{
			try {
				return (Action)(Enum.Parse(typeof(Action), name));
			} catch (Exception e) {
				return Action.idel;
			}
		}

		//	public void Start()
		//	{
		//		setAction (Action.walk, null);
		//	}
	
		public void pause()
		{
			animator.enabled = false;
			enabled = false;
		}

		public void regain()
		{
			animator.enabled = true;
			enabled = true;
		}

		public Action currAction = Action.idel;
		public int currActionValue = -1;
		int cbCount = 1;

		public void setSpeedAdd(float addSpeed)
		{
			animator.speed = 1 + addSpeed * 0.5f;
		}

		public void setAction(Action action, object onCompleteMotion)
		{
			setAction((int)action, onCompleteMotion);
		}

		Coroutine coroutineAction;

		public void setAction(int actionValue, object onCompleteMotion)
		{
			__setAction(actionValue, onCompleteMotion);		//为了做回放，不能用StartCoroutine

			/*
		#if UNITY_4_6 || UNITY_5  || UNITY_5_6_OR_NEWER
		if(coroutineAction != null) {
		    StopCoroutine(coroutineAction);
			coroutineAction = null;
		}
		coroutineAction  = StartCoroutine(_setAction(actionValue, onCompleteMotion, 0.01f));
		#else
		Debug.LogError ("This function cannot surport current version unity!!!");
		#endif
		*/
		}

		IEnumerator _setAction(int actionValue, object onCompleteMotion, float sec)
		{
			yield return new WaitForSeconds(sec);
			__setAction(actionValue, onCompleteMotion);
		}

		void __setAction(int actionValue, object onCompleteMotion)
		{
			if (onCompleteMotion != null && onCompleteMotion.GetType() == typeof(ArrayList)) {
				doSetActionWithCallback(actionValue, (ArrayList)onCompleteMotion);
			} else {
				ArrayList list = null;
				if (onCompleteMotion != null) {
					list = new ArrayList();
					list.Add(100);
					list.Add(onCompleteMotion);
				}
				doSetActionWithCallback(actionValue, list);
			}
		}

		ArrayList progressPoints = new ArrayList();
		//检测动作过程的点（百分比）
		ArrayList progressCallback = new ArrayList();
		//动作过程的回调
		Hashtable callbackMap = new Hashtable();
		int progressIndex = 0;

		/// <summary>
		/// Sets the action.
		/// </summary>
		/// <param name="actionValue">Action value.动作对应的value</param>
		/// <param name="callbackInfor">Callback infor. 是一个key:value的键值对
		///								key：是0～100的整数，表示动作播放到百分之多少时执行回调，
		///								而回调方法就是该key所对应的value
		/// </param>

		public void doSetActionWithCallback(int actionValue, ArrayList progressCallbackInfor)
		{
//		if (currActionValue == actionValue) {
//			return;
//		}
			//////////////////////////////////////////////////////////////////
			progressPoints.Clear();
			progressCallback.Clear();
			callbackMap.Clear();
			if (progressCallbackInfor != null) {
				int count = progressCallbackInfor.Count;
				for (int i = 0; i < count; i++) {
					if (i % 2 == 0) {
						progressPoints.Add(NumEx.stringToInt(progressCallbackInfor [i].ToString()) / 100.0f);
					} else {
						progressCallback.Add(progressCallbackInfor [i]);
					}
				}
			
				progressCallbackInfor.Clear();
				progressCallbackInfor = null;
			}
			//////////////////////////////////////////////////////////////////
			currActionValue = actionValue;
			currAction = (Action)(Enum.ToObject(typeof(Action), actionValue));
			if (!animator.isInitialized) {
				return;
			}
			animator.SetInteger("Action", actionValue);
			if (progressPoints.Count > 0) {
				progressIndex = 0;
				isCheckProgress = true; // place the code after setAction, beacuse in setAction function ,set isCheckProgress = false;
			} else {
				isCheckProgress = false;
			}
		}

		AnimatorStateInfo currentState;
		int oldMotionTime = 0;
		//old动作播放次数
		int MotionTime = 0;
		//动作播放次数
		bool isCheckProgress = false;
		//	object callback = null;

		void FixedUpdate()		//用FixedUpdate是为可以回放
		{
			if (!isCheckProgress)
				return;
			if (animator.layerCount <= 0) {
				return;
			}
			currentState = animator.GetCurrentAnimatorStateInfo(0);
			/*
			 * 通过tag来判断当前正在执行状态是否为设置的状态，
			 * 因此在创建控制器中要把状态的tag的值与该文件中的ActionValue保持一至
		*/
			if (!currentState.IsTag(currActionValue.ToString())) {
				return;
			}
			if (currentState.loop) {
				isCheckProgress = false;
//				//normalizedTime的整数部分为：该动作循环了几次，小数部分为：该动作的进程
//				MotionTime = (int)(currentState.normalizedTime);
//				if (MotionTime > oldMotionTime) {
//					oldMotionTime = MotionTime;
//					//完成一次动作
//					if (onCompleteMotion != null) {
//						if (typeof(LuaFunction) == onCompleteMotion.GetType()) {
//							((LuaFunction)onCompleteMotion).Call(this);
//						} else if (typeof(Callback) == onCompleteMotion.GetType()) {
//							((Callback)onCompleteMotion)(this);
//						}
//					}
//					if (onWillCompleteMotion != null) {
//						isCheckWillFinish = true;
//					}
//				} 
			} else {
				while (true) {
					if (progressIndex < progressPoints.Count) {
						if (currentState.normalizedTime + 0.009f > (float)(progressPoints [progressIndex])) {
							StartCoroutine(exeCallback(progressCallback [progressIndex]));
							progressIndex++;
						} else {
							break;
						}
					} else {
						isCheckProgress = false;
						break;
					}
				}
			}
		}

		public IEnumerator exeCallback(object cbFunc)
		{
			yield return null;
			if (cbFunc != null) {
				Utl.doCallback(cbFunc, this);
			}
		}
	}
}
