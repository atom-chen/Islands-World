/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   摇杆
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;

/// <summary>
/// CL joystick.
/// </summary>
namespace Coolape
{
	[RequireComponent(typeof(BoxCollider))]
	public class CLJoystick : UIEventListener
	{
		public Transform joystickUI;
		public float joystickMoveDis = 10;
		object onPressCallback;
		object onDragCallback;
		object onClickCallback;
		bool isCanMove = false;
		Vector2 orgPos = Vector2.zero;
		Vector2 dragDetla = Vector2.zero;
		Vector3 joystickUIPos = Vector3.zero;
		//	GameObject empty = null;

		bool isFinishStart = false;

		void Start()
		{
			if (isFinishStart)
				return;
			isFinishStart = true;
//		empty = new GameObject();
			if (joystickUI != null) {
				joystickUI.transform.parent.localScale = Vector3.one * 0.95f;
				joystickUIPos = joystickUI.transform.parent.localPosition;
//			empty.transform.parent = joystickUI.transform.parent;
				orgPos = joystickUI.localPosition;
//			empty.transform.localPosition = joystickUI.localPosition;
			}
		}

		public void init(object onPress, object onClick, object onDrag)
        {
            onPressCallback = onPress;
			onDragCallback = onDrag;
			onClickCallback = onClick;
			Start();
			OnPress(false);
		}

        //	RaycastHit lastHit;
        MyMainCamera _mainCamera;
        public MyMainCamera mainCamera
        {
            get {
                if(_mainCamera == null)
                {
                    _mainCamera = MyMainCamera.current;
                }
                return _mainCamera;
            }
            set
            {
                _mainCamera = value;
            }
        }

        void OnClick()
		{
            if (mainCamera == null) return;
			mainCamera.enabled = true;
			mainCamera.Update();
			mainCamera.LateUpdate();
//	#if UNITY_EDITOR
//		mainCamera.ProcessMouse();
//		#else
//		mainCamera.ProcessTouches();
//#endif
			if (MyMainCamera.lastHit.collider != null) {
			}

			Utl.doCallback(onClickCallback);
		}

		void OnPress(bool isPressed)
		{
			if (!isPressed) {
//            if(checkPressedJoy()) return;
				CancelInvoke("doOnPress");
				if (isCanMove) {
					callOnPressCallback(isPressed);
				}
				isCanMove = false;
				dragDetla = Vector2.zero;
				if (joystickUI != null) {
					joystickUI.localPosition = orgPos;
					joyPosition = orgPos;
					joystickUI.transform.parent.localPosition = joystickUIPos;
					joystickUI.transform.parent.localScale = Vector3.one * 0.95f;
				}
			} else {
				joyPosition = orgPos;
				if (joystickUI != null) {
					joystickUI.transform.parent.localScale = Vector3.one * 1.1f;
				}
//			Invoke ("doOnPress", 0.2f);
				doOnPress();
			}
		}

		void callOnPressCallback(bool isPressed)
		{
			Utl.doCallback(onPressCallback, isPressed);
		}

		void doOnPress()
		{
			//		isCanMove = true;
			if (joystickUI != null) {
				joystickUI.transform.parent.position = UICamera.lastHit.point;
			}
			callOnPressCallback(true);
		}

		Vector3 joyPosition = Vector3.zero;

		void OnDrag(Vector2 delta)
		{
			isCanMove = true;
			joyPosition += new Vector3(delta.x, delta.y, 0);
			if (joystickUI != null) {
				if (joyPosition.magnitude > joystickMoveDis) {
					joystickUI.transform.localPosition = Vector3.ClampMagnitude(joyPosition, joystickMoveDis);
				} else {
					joystickUI.transform.localPosition = joyPosition;
				}
				dragDetla = new Vector2((joystickUI.transform.localPosition.x - orgPos.x) / joystickMoveDis, (joystickUI.transform.localPosition.y - orgPos.y) / joystickMoveDis);
			}
		}


		//	void  OnDragOver (GameObject draggedObject) //is sent to a game object when another object is dragged over its area.
		//	{
		//		Debug.LogError("OnDragOver");
		//		OnPress(false);
		//	}
		//	void  OnDragOut (GameObject draggedObject) //is sent to a game object when another object is dragged out of its area.
		//	{
		//		Debug.LogError("OnDragOut");
		//		OnPress(false);
		//	}

		void OnDragEnd()
		{// is sent to a dragged object when the drag event finishes.
			OnPress(false);
		}

		void OnDoubleClick()
		{

		}

		void Update()
		{
			if (isCanMove) {
				Utl.doCallback(onDragCallback, dragDetla);
			}
		
#if UNITY_EDITOR || UNITY_STANDALONE
			if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) {
				isCanMove = true;
				dragDetla = new Vector2(1, 1);
			} else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) {
				isCanMove = true;
				dragDetla = new Vector2(-1, 1);
			} else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) {
				isCanMove = true;
				dragDetla = new Vector2(0, 1);
			} else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) {
				isCanMove = true;
				dragDetla = new Vector2(1, -1);
			} else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) {
				isCanMove = true;
				dragDetla = new Vector2(-1, -1);
			} else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) {
				isCanMove = true;
				dragDetla = new Vector2(1, 0);
			} else if (Input.GetKey(KeyCode.W)) {
				isCanMove = true;
				dragDetla = new Vector2(0, 1);
			} else if (Input.GetKey(KeyCode.S)) {
				isCanMove = true;
				dragDetla = new Vector2(0, -1);
			} else if (Input.GetKey(KeyCode.A)) {
				isCanMove = true;
				dragDetla = new Vector2(-1, 0);
			} else if (Input.GetKey(KeyCode.D)) {
				isCanMove = true;
				dragDetla = new Vector2(1, 0);
			}

			if (Input.GetKeyUp(KeyCode.A) ||
			    Input.GetKeyUp(KeyCode.D) ||
			    Input.GetKeyUp(KeyCode.W) ||
			    Input.GetKeyUp(KeyCode.S)) {
				isCanMove = false;
			
				Utl.doCallback(onPressCallback, false);
			}

//			if (Input.GetKeyDown(KeyCode.J)) {
//				if (lfonPressAttack != null) {
//					lfonPressAttack.Call(null, true);
//				}
//			} else if (Input.GetKeyUp(KeyCode.J)) {
//				if (lfonPressAttack != null) {
//					lfonPressAttack.Call(null, false);
//				}
//			}

#endif
		}
	}
}
