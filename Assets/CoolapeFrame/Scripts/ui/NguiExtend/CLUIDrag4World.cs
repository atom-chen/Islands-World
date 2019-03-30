/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   通过ui的drag来处理对3d场景中处理
 * 使用说明：
				1.在ui层绑定该脚本
				2.在3d场景中有主摄像机要绑定CLSmoothFollow脚本和MyMainCamera脚本
				3.CLSmoothFollow要有一个target
				4.再把2，3赋值给本脚本中的target变量及mainCamera变量等
				5.特别注意groundMask，这个可以处理能够看到的地表的大小，因此需要地表有boxCollider
  *Others:  
  *History:
*********************************************************************************
*/
using UnityEngine;
using System.Collections;

namespace Coolape
{
    [ExecuteInEditMode]
    public class CLUIDrag4World : MonoBehaviour
    {
        public enum DragEffect
        {
            None,
            Momentum,
            MomentumAndSpring,
        }

        public static CLUIDrag4World self;

        public CLUIDrag4World()
        {
            self = this;
        }

        public object onDragMoveDelegate = null;
        public object onDragScaleDelegate = null;
        public object onEndDragMoveDelegate = null;
        public MyMainCamera main3DCamera;
        /// <summary>
        /// Target object that will be dragged.
        /// </summary>

        public Transform target;
        public CLSmoothFollow scaleTarget;
        public bool canMove = true;
        public bool canRotation = true;
        public bool canRotationOneTouch = true;
        public bool canScale = true;
        [HideInInspector]
        public bool canDoInertance = true;

        //是否严格检测可视范围
        public bool isLimitCheckStrict = true;

        int _dragProcType = -1;
        //0:拖动 1:旋转 2:缩放
        int dragProcType
        {
            get
            {
                return _dragProcType;
            }
            set
            {
                if (_dragProcType != value)
                {
                    oldTowFingersDis = -1;
                }
                _dragProcType = value;
            }
        }

        /// <summary>
        /// Scale value applied to the drag delta. Set X or Y to 0 to disallow dragging in that direction.
        /// </summary>

        public Vector3 dragMovement { get { return scale; } set { scale = value; } }

        /// <summary>
        /// Momentum added from the mouse scroll wheel.
        /// </summary>

        public Vector3 scrollMomentum = Vector3.zero;

        /// <summary>
        /// Effect to apply when dragging.
        /// </summary>

        public DragEffect dragEffect = DragEffect.MomentumAndSpring;

        /// <summary>
        /// How much momentum gets applied when the press is released after dragging.
        /// </summary>

        public float momentumAmount = 35f;

        // Obsolete property. Use 'dragMovement' instead.
        [SerializeField]
        protected Vector3
            scale = new Vector3(1f, 1f, 0f);

        //====limit setting
        public LayerMask groundMask;
        public Vector3 viewCenter = Vector3.zero;
        public float viewRadius = 10;
        public float rotationMini = 0;
        public float rotationMax = 0;
        public float scaleMini = 0;
        public float scaleMax = 0;
        public float scaleHeightMini = 0;
        public float scaleHeightMax = 0;
        public float rotateSpeed = 1;
        public float scaleSpeed = 1;
        public float inertanceSpeed = 1;
        //惯性speed

        // Obsolete property. Use 'scrollMomentum' instead.
        [SerializeField]
        [HideInInspector]
        float
            scrollWheelFactor = 0f;
        Vector2 inputPos;
        Vector3 mLastPos;
        Vector3 mMomentum = Vector3.zero;
        Vector3 mScroll = Vector3.zero;
        int mTouchID = 0;
        bool mStarted = false;
        bool mPressed = false;
        Vector2 limitOffset = Vector2.zero;

        public float _scaleValue
        {
            get
            {
                return scaleTarget.height / 10f;
            }
        }

        float scaleValue = 1;

        /// <summary>
        /// Auto-upgrade the legacy data.
        /// </summary>

        float _scaleOffsetVal_ = 200;

        void Start()
        {
            float size = Screen.width < Screen.height ? Screen.height : Screen.width;
            _scaleOffsetVal_ = size / 5;
        }

        void OnEnable()
        {
            if (scrollWheelFactor != 0f)
            {
                scrollMomentum = scale * scrollWheelFactor;
                scrollWheelFactor = 0f;
            }
            dragProcType = -1;
        }

        void OnDisable()
        {
            dragProcType = -1;
            mStarted = false;
        }

        static Hashtable canProcClickPanels = new Hashtable();

        public static void setCanClickPanel(string pName)
        {
            canProcClickPanels[pName] = true;
        }

        public static void removeCanClickPanel(string pName)
        {
            canProcClickPanels[pName] = null;
        }

        /// <summary>
        /// Create a plane on which we will be performing the dragging.
        /// </summary>

        void OnPress(bool pressed)
        {
            // Unity's physics seems to break when timescale is not quite zero. Raycasts start to fail completely.
            float ts = Time.timeScale;
            if (ts < 0.01f && ts != 0f)
                return;

            if (Input.touchCount > 1 && !main3DCamera.allowMultiTouch)
            {
                if (main3DCamera.enabled)
                {
                    main3DCamera.ProcessRelease();
                    main3DCamera.enabled = false;
                }
            }
            else
            {
                if (CLPanelManager.topPanel != null &&
                    canProcClickPanels[CLPanelManager.topPanel.name] != null)
                {
                    if (pressed && main3DCamera != null)
                    {//add by chenbin
                        main3DCamera.enabled = true;
                        main3DCamera.Update();
                        main3DCamera.LateUpdate();
                    }
                }
            }

            if (enabled && NGUITools.GetActive(gameObject) && target != null)
            {
                if (pressed)
                {
                    unDoInertance();
                    if (!mPressed)
                    {
                        scaleValue = _scaleValue;
                        inputPos = UICamera.currentTouch.pos;
                        // Remove all momentum on press
                        mTouchID = UICamera.currentTouchID;
                        mPressed = true;
                        mStarted = false;
                        CancelMovement();

                        // Disable the spring movement
                        CancelSpring();
                    }
                }
                else if (mPressed && mTouchID == UICamera.currentTouchID)
                {
                    mPressed = false;
                    if (dragEffect != DragEffect.MomentumAndSpring)
                    {
                        CancelMovement();
                        unDoInertance();
                    }
                    if (dragProcType == 0)
                    {
                        if (limitDisplayView())
                        {
                            if (canMove)
                            {
                                doInertance();
                            }
                            else
                            {
                                unDoInertance();
                            }
                        }
                        else
                        {
                            unDoInertance();
                        }
                    }
                    else
                    {
                        unDoInertance();
                    }
                }
            }
            //===========================
            dragProcType = -1;
        }

        Vector2 dragDelta = Vector2.zero;

        /// <summary>
        /// Drag the object along the plane.
        /// </summary>
        public void OnDrag(Vector2 delta)
        {
            dragDelta = delta;
            switch (dragProcType)
            {
                case 0:
                    if (canMove)
                    {
                        doOnDragMove(delta);
                    }
                    break;
                case 1:
                    if (canRotation)
                    {
                        procAngleView(delta);
                    }
                    break;
                case 2:
                    if (canScale)
                    {
                        procScalerSoft(delta);
                    }
                    break;
                case 3:
                    proc2TouchsDrag(delta);
                    break;
                default:
                    break;
            }
        }

        void doOnDragMove(Vector2 delta)
        {
            if (mPressed && mTouchID == UICamera.currentTouchID &&
                enabled && NGUITools.GetActive(gameObject) && target != null && main3DCamera != null)
            {
                Vector3 offset = getMoveOffset(delta);
                if (dragEffect != DragEffect.None)
                    mMomentum = Vector3.Lerp(mMomentum, mMomentum + offset * (0.01f * momentumAmount), 0.67f);

                // Adjust the position and bounds
                Move(offset, false);

                // Constrain the UI to the bounds, and if done so, immediately eliminate the momentum
                if (dragEffect != DragEffect.MomentumAndSpring)
                {
                    CancelMovement();
                }
            }
        }

        Vector3 getMoveOffset(Vector2 delta)
        {
            Vector2 offset = delta;
            offset.Scale(scale);

            Vector3 off = Vector3.zero;
            float angle = target.rotation.eulerAngles.y * Mathf.PI / 180;
            off.x = -((offset.x) * Mathf.Cos(angle) + (offset.y) * Mathf.Sin(angle));
            off.z = -((offset.y) * Mathf.Cos(angle) - (offset.x) * Mathf.Sin(angle));

            off = off * scaleValue * 0.015f;
            return off;
        }

        /// <summary>
        /// Move the dragged object by the specified amount.
        /// </summary>

        void Move(Vector3 worldDelta, bool isDoLimitView = false)
        {
            Vector3 before = target.position;
            Vector3 orgDelta = worldDelta;
            target.position = before + worldDelta;
            scaleTarget.LateUpdate();       //执行一次跟随
            bool inGround = true;
            if (isDoLimitView)
            {
                inGround = limitDisplayView(ref worldDelta);
            }
            else
            {
                inGround = isInviewBounds(ref worldDelta);
            }
            if (!inGround)
            {
                if (!isDoLimitView)
                {
                    //target.position = before + worldDelta;
                    target.position = before + orgDelta;
                    CancelMovement();
                }
            }
            else
            {
                Utl.doCallback(onDragMoveDelegate, worldDelta);
            }
        }

        RaycastHit hitOut;
        Ray veiwRay;

        bool getViewPoint2World(Vector3 pos, ref Vector3 outPos)
        {
            veiwRay = main3DCamera.cachedCamera.ScreenPointToRay(pos);
            outPos = Vector3.zero;
            if (Physics.Raycast(veiwRay, out hitOut, 2000, groundMask))
            {
                outPos = hitOut.point;
                return true;
            }
            return false;
        }

        Vector3 viewLeft = Vector3.zero;
        Vector3 viewRight = Vector3.zero;
        Vector3 viewTop = Vector3.zero;
        Vector3 viewBottom = Vector3.zero;
        bool inGroundLeft = false;
        bool inGroundRight = false;
        bool inGroundTop = false;
        bool inGroundBottom = false;

        bool isInviewBounds()
        {
            Vector3 offset = Vector3.zero;
            return isInviewBounds(ref offset);
        }

        bool isInviewBounds(ref Vector3 offset)
        {
            if (offset == null)
            {
                offset = Vector3.zero;
            }
            inGroundLeft = getViewPoint2World(new Vector3(0, Screen.height / 2, 0), ref viewLeft);
            inGroundRight = getViewPoint2World(new Vector3(Screen.width, Screen.height / 2, 0), ref viewRight);
            inGroundTop = getViewPoint2World(new Vector3(Screen.width / 2, Screen.height, 0), ref viewTop);
            inGroundBottom = getViewPoint2World(new Vector3(Screen.width / 2, 0, 0), ref viewBottom);
            if (isLimitCheckStrict)
            {
                if (!inGroundLeft || !inGroundRight || !inGroundTop || !inGroundBottom)
                {
                    if (!inGroundLeft || !inGroundRight)
                    {
                        offset.x = 0;
                    }
                    if (!inGroundTop || !inGroundBottom)
                    {
                        offset.z = 0;
                    }
                    return false;
                }
            }
            else
            {
                if (!inGroundLeft && !inGroundRight && !inGroundTop && !inGroundBottom)
                {
                    float dis1 = Vector3.Distance(target.position, viewCenter);
                    float dis2 = Vector3.Distance(target.position + offset, viewCenter);
                    if (dis1 < dis2)
                    {
                        offset = Vector3.zero;
                    }
                    return false;
                }
                else if (!inGroundLeft && !inGroundRight && !inGroundTop)
                {
                    offset = Vector3.zero;
                    return false;
                }
                else if (!inGroundLeft && !inGroundRight && !inGroundBottom)
                {
                    offset = Vector3.zero;
                    return false;
                }
                else if (!inGroundLeft && !inGroundTop && !inGroundBottom)
                {
                    offset = Vector3.zero;
                    return false;
                }
                else if (!inGroundRight && !inGroundTop && !inGroundBottom)
                {
                    offset = Vector3.zero;
                    return false;
                }
            }

            float disLeft = Vector3.Distance(viewCenter, viewLeft);
            float disRight = Vector3.Distance(viewCenter, viewRight);
            float disTop = Vector3.Distance(viewCenter, viewTop);
            float disBottom = Vector3.Distance(viewCenter, viewBottom);

            if ((disLeft > viewRadius || disRight > viewRadius)
            && (disTop > viewRadius || disBottom > viewRadius))
            {
                offset.x = 0;
                offset.y = 0;
                offset.z = 0;
                return false;
            }

            if (disLeft > viewRadius || disRight > viewRadius)
            {
                offset.x = 0;
                return false;
            }

            if (disTop > viewRadius || disBottom > viewRadius)
            {
                offset.z = 0;
                return false;
            }
            return true;
        }

        void Update()
        {
            //处理旋转或缩放
            screenTouch();

            if (isLimitDisplayView)
            {
                canDoInertance = false;
                Move(getMoveOffset(limitOffset * 50), true);
            }
            else
            {
                if (canDoInertance)
                {
                    inertanceCount = inertanceCount + Time.unscaledDeltaTime;
                    if (inertanceCount > 1)
                    {
                        canDoInertance = false;
                    }
                    else
                    {
                        if (isInviewBounds())
                        {
                            Move(getMoveOffset(dragDelta * (1 - inertanceCount) * inertanceSpeed), false);
                        }
                        else
                        {
                            //							limitDisplayView ();
                            canDoInertance = false;
                        }
                    }
                }
            }
        }

        float inertanceCount = 0;

        public void doInertance()
        {
            inertanceCount = 0;
            canDoInertance = true;
        }

        public void unDoInertance()
        {
            dragDelta = Vector2.zero;
            inertanceCount = 0;
            canDoInertance = false;
        }

        bool isTouchMoved = false;
        bool isCameraRotation = false;
        bool isFirstInOneTouch = false;
        float touchClickThreshold = 5;
        long StationaryTime = 0;
        Touch touch1;
        Touch touch2;
        Vector2 totalDelta1 = Vector2.zero;
        Vector2 totalDelta2 = Vector2.zero;

        /// <summary>
        /// Screens the touch.处理旋转或缩放
        /// </summary>
        void screenTouch()
        {
            if (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer)
            {
                //两个手指滑动
                if (Input.touchCount == 2)
                {
                    touch1 = Input.touches[0];
                    touch2 = Input.touches[1];
                    isFirstInOneTouch = true;

                    if (Input.touches[0].phase == TouchPhase.Moved)
                    {
                        totalDelta1 += Input.touches[0].deltaPosition;
                    }
                    if (Input.touches[1].phase == TouchPhase.Moved)
                    {
                        totalDelta2 += Input.touches[1].deltaPosition;
                    }
                    if (Input.touches[0].phase == TouchPhase.Stationary)
                    {
                        totalDelta1 = Vector2.zero;
                    }
                    if (Input.touches[1].phase == TouchPhase.Stationary)
                    {
                        totalDelta2 = Vector2.zero;
                    }
                    //===============
                    if (touch1.phase == TouchPhase.Began ||
                        touch2.phase == TouchPhase.Began)
                    {
                        totalDelta1 = Vector2.zero;
                        totalDelta2 = Vector2.zero;
                        dragProcType = -1;
                        oldTowFingersDis = -1;
                        tempOldDis = -1;
                    }
                    else if (isTouchMoving(totalDelta1) ||
                             isTouchMoving(totalDelta2))
                    {
                        proc2TouchsDrag(dragDelta);
                        //					dragProcType = 3;
                    }
                    else if (touch1.phase == TouchPhase.Ended ||
                             touch2.phase == TouchPhase.Ended)
                    {
                        dragProcType = -1;
                        isCameraRotation = false;
                        StationaryTime = System.DateTime.Now.AddSeconds(0.5f).ToFileTime();
                        isFirstInOneTouch = true;
                    }
                }
                else if (Input.touchCount == 1)
                {   //一个手指
                    if (isFirstInOneTouch)
                    {
                        totalDelta1 = Vector2.zero;
                        //初始化视角转动参Number
                        StationaryTime = System.DateTime.Now.AddSeconds(0.5f).ToFileTime();
                        isCameraRotation = false;
                        isFirstInOneTouch = false;
                        isTouchMoved = false;
                    }
                    if (Input.touches[0].phase == TouchPhase.Stationary)
                    {
                        totalDelta1 = Vector2.zero;
                        if (canRotationOneTouch)
                        {
                            if (!isTouchMoved && !isCameraRotation && System.DateTime.Now.ToFileTime() - StationaryTime > 0)
                            {
                                isCameraRotation = true;
                                //							dragProcType = 1;
                                //TODO:显示一个图标，表示已经选中了点
                            }
                        }
                        else
                        {
                            isCameraRotation = false;
                            dragProcType = -1;
                        }
                    }
                    else if (Input.touches[0].phase == TouchPhase.Began)
                    {
                        totalDelta1 = Vector2.zero;
                        isTouchMoved = false;
                        //初始化视角转动参Number
                        StationaryTime = System.DateTime.Now.AddSeconds(0.5f).ToFileTime();
                        isCameraRotation = false;
                    }
                    else if (Input.touches[0].phase == TouchPhase.Ended)
                    {
                        isTouchMoved = false;
                        isCameraRotation = false;
                        totalDelta1 = Vector2.zero;
                        dragProcType = -1;
                    }
                    else if (Input.touches[0].phase == TouchPhase.Moved)
                    { //滑动
                        totalDelta1 += Input.touches[0].deltaPosition;
                        if (isTouchMoving(totalDelta1))
                        {
                            if (isCameraRotation)
                            {
                                dragProcType = 1;
                            }
                            else
                            {
                                isTouchMoved = true;
                                dragProcType = 0;
                            }
                        }
                    }
                }
            }
            else
            {
                float v = Input.GetAxis("Mouse ScrollWheel");
                if (Mathf.Abs(v) > 0.01f)
                {
                    dragProcType = 2;
                    OnDrag(new Vector2(v * Screen.width, 0));
                }
                else
                {
                    //视角转动处理
                    if (Input.GetMouseButtonDown(1))
                    {
                        dragProcType = 1;
                    }
                    else if (Input.GetMouseButtonDown(0))
                    {
                        dragProcType = 0;
                    }
                }
            }
        }

        bool isTouchMoving(Vector2 totalDelta)
        {
            float threshold = Mathf.Max(touchClickThreshold, Screen.height * 0.005f);
            if (totalDelta.magnitude > threshold)
            {
                return true;
            }
            return false;
        }

        float tempOldDis = 0;

        void proc2TouchsDrag(Vector2 iDelta)
        {
            if (Input.touchCount != 2)
            {
                dragProcType = -1;
                return;
            }

            Vector2 pos1 = Input.touches[0].position;
            Vector2 pos2 = Input.touches[1].position;
            Vector2 cachDelta1 = Input.touches[0].deltaPosition;
            Vector2 cachDelta2 = Input.touches[1].deltaPosition;
            Vector2 dragDelta = iDelta;
            if (touch2.position.x < touch1.position.x)
            {
                dragDelta = iDelta * -1;
                pos1 = touch2.position;
                pos2 = touch1.position;
                cachDelta1 = touch2.deltaPosition;
                cachDelta2 = touch1.deltaPosition;
            }
            Vector2 delta1 = cachDelta1;
            Vector2 delta2 = cachDelta2;

            delta1 = Mathf.Abs(delta1.x) > Mathf.Abs(delta1.y) ? new Vector2(delta1.x, 0) : new Vector2(0, delta1.y);
            delta2 = Mathf.Abs(delta2.x) > Mathf.Abs(delta2.y) ? new Vector2(delta2.x, 0) : new Vector2(0, delta2.y);
            if (
                (delta1.x < 0 && delta2.x < 0) ||
                (delta1.x > 0 && delta2.x > 0) ||
                (delta1.y < 0 && delta2.y < 0) ||
                (delta1.y > 0 && delta2.y > 0))
            {       //两个手指向同一方向移动
                dragProcType = -1;
            }
            else if ((
                         (Mathf.Abs(pos1.y - pos2.y) < _scaleOffsetVal_ &&
                         ((delta1.x <= 0 && delta2.x > 0) ||
                         (delta1.x < 0 && delta2.x >= 0) ||
                         (delta1.x >= 0 && delta2.x < 0) ||
                         (delta1.x > 0 && delta2.x <= 0)
                         )
                         )) || (
                         (Mathf.Abs(pos1.x - pos2.x) < _scaleOffsetVal_ &&
                         ((delta1.y <= 0 && delta2.y > 0) ||
                         (delta1.y < 0 && delta2.y >= 0) ||
                         (delta1.y >= 0 && delta2.y < 0) ||
                         (delta1.y > 0 && delta2.y <= 0))
                         )))
            {   //缩放
                //				dragProcType = 2;
                procScalerSoft(Vector2.zero);
            }
            else
            {
                float angle1 = getAngle(new Vector3(cachDelta1.x, 0, cachDelta1.y));
                float angle2 = getAngle(new Vector3(-cachDelta2.x, 0, -cachDelta2.y));

                Vector2 dir = Vector2.zero;
                float dis = Vector2.Distance(pos1, pos2);
                if (tempOldDis == -1)
                {
                    tempOldDis = dis;
                    return;
                }
                if (tempOldDis > dis)
                {
                    dir = pos2 - pos1;
                }
                else
                {
                    dir = pos1 - pos2;
                }
                tempOldDis = dis;
                float angle3 = getAngle(new Vector3(dir.x, 0, dir.y));
                if (Mathf.Abs(angle3 - angle1) < 50f &&
                    Mathf.Abs(angle3 - angle2) < 50f)
                {
                    //缩放
                    //					dragProcType = 2;
                    procScalerSoft(Vector2.zero);
                }
                else
                {
                    if (
                        (delta1.y <= 0 && delta2.y > 0) ||
                        (delta1.y < 0 && delta2.y >= 0))
                    {
                        procAngleView(dragDelta);
                        dragProcType = -1;
                    }
                    else if (
                      (delta1.y >= 0 && delta2.y < 0) ||
                      (delta1.y > 0 && delta2.y <= 0))
                    {
                        procAngleView(dragDelta);
                        dragProcType = -1;
                    }
                    if (
                        (delta1.x <= 0 && delta2.x > 0) ||
                        (delta1.x < 0 && delta2.x >= 0))
                    {
                        procAngleView(dragDelta);
                        dragProcType = -1;
                    }
                    else if (
                      (delta1.x >= 0 && delta2.x < 0) ||
                      (delta1.x > 0 && delta2.x <= 0))
                    {
                        procAngleView(dragDelta);
                        dragProcType = -1;
                    }
                }
            }
        }

        public float getAngle(Vector3 dir)
        {
            if (dir.magnitude < 0.001f)
            {
                return 0;
            }
            Quaternion toTarget = Quaternion.LookRotation(dir);
            float angle = 0;
            Vector3 axis = Vector3.zero;
            toTarget.ToAngleAxis(out angle, out axis);
            return angle;
        }

        //视角处理
        void procAngleView(Vector2 delta)
        {
            if (!canRotation)
            {
                return;
            }
            //		float angle = getAngle (new Vector3(delta.x, 0, delta.y));
            float offset = 0;
#if UNITY_EDITOR
            if (UIRoot.list.Count > 0 && Input.mousePosition.y > UIRoot.list[0].manualHeight/2) {
                offset = -delta.x;
            } else {
                offset = delta.x;
            }
#else
            offset = Mathf.Abs(delta.x) > Mathf.Abs(delta.y) ? delta.x : delta.y;
#endif
            target.Rotate (Vector3.up, rotateSpeed * Time.deltaTime * offset * 10);
			Vector3 ea = target.localEulerAngles;
			if (rotationMax != 0 && rotationMini != 0) {
				if (ea.y >= rotationMax) {
					ea.y = rotationMax;
					target.localEulerAngles = ea;
				}
				if (ea.y <= rotationMini) {
					ea.y = rotationMini;
					target.localEulerAngles = ea;
				}
			}
			limitDisplayView ();
		}

		float oldTowFingersDis = -1;

		void procScalerSoft (Vector2 delta)
		{
			if (!canScale) {
				return;
			}
			float offset = 0;
			if (Input.touchCount == 2) {
				float dis = Vector2.Distance (Input.touches [0].position, Input.touches [1].position);
				if (oldTowFingersDis == -1) {
					oldTowFingersDis = dis;
					return;
				}
				offset = (dis - oldTowFingersDis);
				oldTowFingersDis = dis;
			} else {
				offset = Mathf.Abs (delta.y) > Mathf.Abs (delta.x) ? delta.y : delta.x;
			}
			if (onDragScaleDelegate != null) {
				Utl.doCallback (onDragScaleDelegate, delta, offset);
			} else {
				procScaler (offset);
			}
		}

		public void procScaler (float delta)
        {
            CancelMovement();
            unDoInertance();
            if (!canScale) {
				return;
			}

			if (scaleTarget == null) {
				return;
			}
			float offset = Time.deltaTime * delta * scaleSpeed;
			scaleTarget.distance -= offset;
			scaleTarget.height -= offset;
			if (scaleMax != 0 && scaleMini != 0) {
				if (scaleTarget.distance >= scaleMax) {
					scaleTarget.distance = scaleMax;
				} else if (scaleTarget.distance <= scaleMini) {
					scaleTarget.distance = scaleMini;
				} 
			} 
			if (scaleHeightMax != 0 && scaleHeightMini != 0) {
				if (scaleTarget.height >= scaleHeightMax) {
					scaleTarget.height = scaleHeightMax;
				} else if (scaleTarget.height <= scaleHeightMini) {
					scaleTarget.height = scaleHeightMini;
				} 
			}

			limitDisplayView ();
		}

		bool isLimitDisplayView = false;

		/// <summary>
		/// Limits the display view.
		/// </summary>
		/// <returns><c>true</c>, if display view was limited, <c>false</c> otherwise.</returns>
		/*
	 * left->Rrigh:(-1,0)
	 * Rrigh->left:(1,0)
	 * bottom->up:(0, -1);
	 * up->bottom:(0, 1);
	 */ 

		public bool limitDisplayView ()
		{
			Vector3 offset = Vector3.zero;
			return limitDisplayView (ref offset);
		}

		public bool limitDisplayView (ref Vector3 offset)
		{
			if (offset == null) {
				offset = Vector3.zero;
			}
			bool ret = true;
			isLimitDisplayView = false;
			Vector3 pos = target.position;
			Vector3 newPos = pos;
			limitOffset = Vector2.zero;
			inGroundLeft = getViewPoint2World (new Vector3 (0, Screen.height / 2, 0), ref viewLeft);
			inGroundRight = getViewPoint2World (new Vector3 (Screen.width, Screen.height / 2, 0), ref viewRight);
			inGroundTop = getViewPoint2World (new Vector3 (Screen.width / 2, Screen.height, 0), ref viewTop);
			inGroundBottom = getViewPoint2World (new Vector3 (Screen.width / 2, 0, 0), ref viewBottom);
			if (!inGroundLeft && !inGroundRight && !inGroundTop && !inGroundBottom) {
				offset = Vector3.zero;
//				target.position = viewCenter;
				return false;
			}

			if (isLimitCheckStrict) {
				if (!inGroundLeft && !inGroundRight) {
					offset.x = 0;
					newPos.x = viewCenter.x;
					target.position = newPos;
				} else {
					if (!inGroundLeft) {
						offset.x = 0;
						limitOffset.x = -1;
						isLimitDisplayView = true;
						ret = false;
					} else if (!inGroundRight) {
						offset.x = 0;
						limitOffset.x = 1;
						isLimitDisplayView = true;
						ret = false;
					}
				}
				if (!inGroundTop && !inGroundBottom) {
					offset.z = 0;
					newPos.z = viewCenter.z;
					target.position = newPos;
				} else {
					if (!inGroundTop) {
						offset.z = 0;
						limitOffset.y = 1;
						isLimitDisplayView = true;
						ret = false;
					} else if (!inGroundBottom) {
						offset.z = 0;
						limitOffset.y = -1;
						isLimitDisplayView = true;
						ret = false;
					}
				}
			} else {
				if (!inGroundLeft && !inGroundRight && !inGroundTop && inGroundBottom) {
					offset = Vector3.zero;
					limitOffset.y = 1;
					isLimitDisplayView = true;
					ret = false;
				} else if (!inGroundLeft && !inGroundRight && inGroundTop && !inGroundBottom) {
					offset = Vector3.zero;
					limitOffset.y = -1;
					isLimitDisplayView = true;
					ret = false;
				} else if (!inGroundLeft && inGroundRight && !inGroundTop && !inGroundBottom) {
					offset = Vector3.zero;
					limitOffset.x = -1;
					isLimitDisplayView = true;
					ret = false;
				} else if (inGroundLeft && !inGroundRight && !inGroundTop && !inGroundBottom) {
					offset = Vector3.zero;
					limitOffset.x = 1;
					isLimitDisplayView = true;
					ret = false;
				}
			}

			//=======================================
			float disLeft = Vector3.Distance (viewCenter, viewLeft);
			float disRight = Vector3.Distance (viewCenter, viewRight);
			float disTop = Vector3.Distance (viewCenter, viewTop);
			float disBottom = Vector3.Distance (viewCenter, viewBottom);
            float screenWidth = Vector3.Distance(viewLeft, viewRight);
            float screenHeigh = Vector3.Distance(viewBottom, viewTop);

            if (disLeft > viewRadius && disRight > viewRadius 
            && (disLeft + disRight) < screenWidth + viewRadius/2)
            {
                //说明中心点还在屏内，那说是表示x轴已经不在在拖动
                offset.x = 0;
                newPos.x = viewCenter.x;
                target.position = newPos;
                limitOffset.x = 0;
            }
            else
            {
                if (disLeft > viewRadius && disRight > viewRadius
                    && (disLeft + disRight) > screenWidth + viewRadius
                    && disLeft > disRight)
                {
                    offset.x = 0;
                    limitOffset.x = -1;
                    isLimitDisplayView = true;
                    ret = false;
                } else if (disLeft > viewRadius && disRight > viewRadius
                    && (disLeft + disRight) > screenWidth + viewRadius
                    && disLeft <= disRight)
                {
                    offset.x = 0;
                    limitOffset.x = 1;
                    isLimitDisplayView = true;
                    ret = false;
                } else if(disLeft > viewRadius)
                {
                    offset.x = 0;
                    limitOffset.x = -1;
                    isLimitDisplayView = true;
                    ret = false;
                } else if (disRight > viewRadius)
                {
                    offset.x = 0;
                    limitOffset.x = 1;
                    isLimitDisplayView = true;
                    ret = false;
                }
            }

            if (disTop > viewRadius && disBottom > viewRadius
            && (disTop + disBottom) < screenHeigh+ viewRadius / 2)
            {
                //说明中心点还在屏内，那说是表示x轴已经不在在拖动
                offset.z = 0;
                newPos.z = viewCenter.z;
                target.position = newPos;
                limitOffset.y = 0;
            }
            else
            {
                if (disTop > viewRadius && disBottom > viewRadius
                    && (disTop + disBottom) > screenHeigh + viewRadius
                    && disTop > disBottom)
                {
                    offset.z = 0;
                    limitOffset.y = 1;
                    isLimitDisplayView = true;
                    ret = false;
                }
                else if (disTop > viewRadius && disBottom > viewRadius
                  && (disTop + disBottom) > screenHeigh + viewRadius
                  && disTop <= disBottom)
                {
                    offset.z = 0;
                    limitOffset.y = -1;
                    isLimitDisplayView = true;
                    ret = false;
                }
                else if (disTop > viewRadius)
                {
                    offset.z = 0;
                    limitOffset.y = 1;
                    isLimitDisplayView = true;
                    ret = false;
                }
                else if (disBottom > viewRadius)
                {
                    offset.z = 0;
                    limitOffset.y = -1;
                    isLimitDisplayView = true;
                    ret = false;
                }
            }

            if (!isLimitDisplayView) {
				LateUpdate ();
			}
			return ret;
		}

		/// <summary>
		/// Apply the dragging momentum.
		/// </summary>
		void LateUpdate ()
		{
#if UNITY_EDITOR
			if (!Application.isPlaying)
				return;
#endif
			if (target == null)
				return;
			float delta = RealTime.deltaTime;

			mMomentum -= mScroll;
			mScroll = NGUIMath.SpringLerp (mScroll, Vector3.zero, 20f, delta);

			// No momentum? Exit.
			if (mMomentum.magnitude > 0.0001f) {
				if (!mPressed) {
					// Apply the momentum
					Move (NGUIMath.SpringDampen (ref mMomentum, 9f, delta));

					if (dragEffect == DragEffect.None) {
						CancelMovement ();
					} else {
						CancelSpring ();
					}

					// Dampen the momentum
					NGUIMath.SpringDampen (ref mMomentum, 9f, delta);

					// Cancel all movement (and snap to pixels) at the end
					if (mMomentum.magnitude < 0.0001f) {
						CancelMovement ();
						Utl.doCallback (onEndDragMoveDelegate);
					}
				} else {
					NGUIMath.SpringDampen (ref mMomentum, 9f, delta);
				}
			}
		}

		/// <summary>
		/// Cancel all movement.
		/// </summary>

		public void CancelMovement ()
		{
			mMomentum = Vector3.zero;
			mScroll = Vector3.zero;
		}

		/// <summary>
		/// Cancel the spring movement.
		/// </summary>
		public void CancelSpring ()
		{
			SpringPosition sp = target.GetComponent<SpringPosition> ();
			if (sp != null)
				sp.enabled = false;
		}

		/// <summary>
		/// If the object should support the scroll wheel, do it.
		/// </summary>
		void OnScroll (float delta)
		{
			if (enabled && NGUITools.GetActive (gameObject))
				mScroll -= scrollMomentum * (delta * 0.05f);
		}
	}
}
