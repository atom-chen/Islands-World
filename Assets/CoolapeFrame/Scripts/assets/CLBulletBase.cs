/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   子弹对象基类
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class CLBulletBase : MonoBehaviour
	{
		public AnimationCurve curveSpeed = new AnimationCurve (new Keyframe (0, 0, 0, 1), new Keyframe (1, 1, 1, 0));
		public AnimationCurve curveHigh = new AnimationCurve (new Keyframe (0, 0, 0, 4), new Keyframe (0.5f, 1, 0, 0), new Keyframe (1, 0, -4, 0));
		BoxCollider _boxCollider;

		public BoxCollider boxCollider {
			get {
				if (_boxCollider == null) {
					_boxCollider = gameObject.GetComponent<BoxCollider> ();
				}
				return _boxCollider;
			}
		}

		public object attr;
		//子弹悔恨
		public object data = null;
		//可以理解为透传参数
		public bool isFireNow = false;
		public bool isFollow = false;
		public bool isMulHit = false;
		public bool isStoped = false;
		public bool needRotate = false;
		//	public bool isCheckTrigger = true;
		public float slowdownDistance = 0;
		public float arriveDistance = 0.3f;
		public float turningSpeed = 1;
        public int RefreshTargetMSec = 0;

        float minMoveScale = 0.05F;
		float curveTime = 0;
		float curveTime2 = 0;
		Vector3 v3Diff = Vector3.zero;
		Vector3 v3Diff2 = Vector3.zero;
		Vector3 subDiff = Vector3.zero;
		Vector3 subDiff2 = Vector3.zero;
        long lastResetTargetTime = 0;
        long lastResetToPosTime = 0;
        public float speed = 1;
		public float high = 0;
		Vector3 highV3 = Vector3.zero;
		//角度偏移量
		public int angleOffset = 0;

		Vector3 origin = Vector3.zero;
		object onFinishCallback;
		Vector3 targetDirection = Vector3.zero;
		public CLUnit attacker;
		public CLUnit target;
		public CLUnit hitTarget;
	
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

		public bool haveCollider = true;

		#if UNITY_EDITOR
//		void Start ()
//		{
//			Utl.setBodyMatEdit (transform);
//		}
		#endif

		void OnTriggerEnter (Collider collider)
		{
			CLUnit unit = collider.gameObject.GetComponent<CLUnit> ();
			if (unit != null && unit.isOffense != attacker.isOffense && !unit.isDead) {
				hitTarget = unit;
				onFinishFire (!isMulHit);
			}
		}

        public virtual void doFire(CLUnit attacker, CLUnit target, Vector3 orgPos, Vector3 dir, object attr, object data, object callbak)
        {
            this.attr = attr;
            this.data = data;
            this.attacker = attacker;
            this.target = target;
            onFinishCallback = callbak;

            int SpeedRandomFactor = MapEx.getBytes2Int(attr, "SpeedRandomFactor");
            //			int SpeedRandomFactor = NumEx.bio2Int (MapEx.getBytes (attr, "SpeedRandomFactor"));
            speed = MapEx.getBytes2Int(attr, "Speed") / 10.0f;
            //			speed = (NumEx.bio2Int (MapEx.getBytes (attr, "Speed"))) / 10.0f;
            if (SpeedRandomFactor > 0) {
                speed = speed + attacker.fakeRandom(-SpeedRandomFactor, SpeedRandomFactor) / 100.0f;
            }
            high = MapEx.getBytes2Int(attr, "High") / 10.0f;
            //			high = NumEx.bio2Int (MapEx.getBytes (attr, "High"));
            if (MapEx.getBool(attr, "IsHighOffset")) {
                high = high * (1.0f + attacker.fakeRandom(-200, 200) / 1000.0f);
            }
            bool isZeroY = high > 0 ? true : false;

            float dis = MapEx.getBytes2Int(attr, "Range") / 10.0f;
            //			float dis = NumEx.bio2Int (MapEx.getBytes (attr, "Range")) / 10.0f;
            isFollow = MapEx.getBool(attr, "IsFollow");
            isMulHit = MapEx.getBool(attr, "IsMulHit");
            needRotate = MapEx.getBool(attr, "NeedRotate");
            RefreshTargetMSec = MapEx.getBytes2Int(attr, "RefreshTargetMSec");
            lastResetTargetTime = DateEx.nowMS;
            lastResetToPosTime = DateEx.nowMS;
            //dir.y = 0;
            Utl.RotateTowards(transform, dir);

            origin = orgPos;
            transform.position = origin;
            Vector3 toPos = Vector3.zero;
            if (target != null && dis <= 0) {
                toPos = target.transform.position;
            } else {
                toPos = origin + dir.normalized * dis;
                //toPos.y = 0;
            }
            int PosRandomFactor = MapEx.getBytes2Int(attr, "PosRandomFactor");
            //			int PosRandomFactor = NumEx.bio2Int (MapEx.getBytes (attr, "PosRandomFactor"));
            if (PosRandomFactor > 0) {
                toPos.x += attacker.fakeRandom(-PosRandomFactor, PosRandomFactor) / 100.0f;
                toPos.y += attacker.fakeRandom(-PosRandomFactor, PosRandomFactor) / 100.0f;
            }

            //if (isZeroY) {
            //    toPos.y = 0;
            //}

            if (boxCollider != null) { 
                if (MapEx.getBool(attr, "CheckTrigger")) {
                    boxCollider.enabled = true;
                } else {
                    boxCollider.enabled = false;
                }
            }
			haveCollider = (boxCollider != null && boxCollider.enabled);

			v3Diff = toPos - origin;

			if (angleOffset != 0) {
				Vector3 center = origin + v3Diff / 2.0f;
//				transform.position = center + new Vector3 (0, high, 0);
				Vector3 _v3 = Utl.RotateAround(center + new Vector3 (0, high, 0), center, v3Diff, angleOffset * Mathf.Sin (Mathf.Deg2Rad * Utl.getAngle (v3Diff).y));
//				transform.RotateAround (center, v3Diff, angleOffset * Mathf.Sin (Mathf.Deg2Rad * Utl.getAngle (v3Diff).y));
				highV3 = _v3 - center;
			} else {
				highV3 = new Vector3 (0, high, 0);
			}

			magnitude = v3Diff.magnitude <= 0.00001f ? 1 : 1.0f / v3Diff.magnitude;

			hitTarget = null;
			curveTime = 0;
			curveTime2 = 0;
			isStoped = false;
			isFireNow = true;
			RotateBullet ();
			CancelInvoke ("timeOut");
			int stayTime = MapEx.getBytes2Int (attr, "MaxStayTime");
//			int stayTime = NumEx.bio2Int (MapEx.getBytes (attr, "MaxStayTime"));
			if (stayTime > 0.00001) {
				Invoke ("timeOut", stayTime / 10.0f);
			}
		}

		RaycastHit hitInfor;
		float magnitude = 1f;

		public virtual void RotateBullet ()
		{
			if (needRotate) {
				curveTime2 += Time.fixedDeltaTime * speed * 10 * magnitude;
				subDiff2 = v3Diff * curveSpeed.Evaluate (curveTime2);
				//				subDiff.y += high * curveHigh.Evaluate (curveTime);
				subDiff2 += highV3 * curveHigh.Evaluate (curveTime2);

				if (subDiff2.magnitude > 0.01) {
					Utl.RotateTowards (transform, origin + subDiff2 - transform.position);
				}
			}
		}
		// Update is called once per frame
		public virtual void FixedUpdate ()
		{
			if (!isFireNow) {
				return;
			}
			if (!isFollow) {
				curveTime += Time.fixedDeltaTime * speed * 10 * magnitude;
				subDiff = v3Diff * curveSpeed.Evaluate (curveTime);
				//				subDiff.y += high * curveHigh.Evaluate (curveTime);
				subDiff += highV3 * curveHigh.Evaluate (curveTime);
				if (!isMulHit && haveCollider) {
					if (Physics.Raycast (transform.position, v3Diff, out hitInfor, 1f)) {
						OnTriggerEnter (hitInfor.collider);
					}
				}

				if (needRotate && subDiff.magnitude > 0.001f) {
					Utl.RotateTowards (transform, origin + subDiff - transform.position);
				}
				transform.position = origin + subDiff;
				if (curveTime >= 1f) {
					hitTarget = null;
					onFinishFire (true);
				}
			} else {
				if (target == null || target.isDead ||
                    (RefreshTargetMSec > 0 && 
                    (DateEx.nowMS - lastResetTargetTime >= RefreshTargetMSec))
                    ){
                    lastResetTargetTime = DateEx.nowMS;
                    resetTarget ();
				}
				subDiff = CalculateVelocity (transform.position);
				if (!isMulHit) {
					if (Physics.Raycast (transform.position, v3Diff, out hitInfor, 1f)) {
                        OnTriggerEnter (hitInfor.collider);
					}
				}
				//Rotate towards targetDirection (filled in by CalculateVelocity)
				if (targetDirection != Vector3.zero) {
					Utl.RotateTowards (transform, targetDirection, turningSpeed);
				}
				transform.Translate (subDiff.normalized * Time.fixedDeltaTime * speed * 10, Space.World);
			}
		}

		/// <summary>
		/// Resets the target. 当子弹是跟踪弹时，如果目标死亡，则重新设置攻击目标
		/// </summary>
		public virtual void resetTarget ()
		{
//			if (attacker == null) {
//				return;
//			}
//			object[] list = null;
//			if (attacker.isOffense) {
//				list = CLBattle.self.defense.ToArray ();
//			} else {
//				list = CLBattle.self.offense.ToArray ();
//			}
//			int count = list.Length;
//			if (count == 0) {
//				return;
//			}
//			int index = attacker.fakeRandom (0, count);
//			target = (CLUnit)(list [index]);
//			list = null;
		}

		Vector3 mToPos = Vector3.zero;
		Vector3 dir = Vector3.zero;
		float targetDist = 0;
		Vector3 forward = Vector3.zero;
		float dot = 0;
		float sp = 0;

		Vector3 CalculateVelocity (Vector3 fromPos)
		{
			//mToPos = Vector3.zero;
			if (isFollow){
                if (target != null)
                {
                    mToPos = target.transform.position;
                }
                else
                {
                    if (RefreshTargetMSec > 0 &&
                    (DateEx.nowMS - lastResetToPosTime >= RefreshTargetMSec)
                    ){
                        lastResetToPosTime = DateEx.nowMS;
                        int x = attacker.fakeRandom(-10, 10);
                        int z = attacker.fakeRandom2(-10, 10);
                        mToPos = transform.position + new Vector3(x, 0, z);
                    }
                    else
                    {
                        mToPos = Vector3.zero;
                    }
                }
            }
			dir = mToPos - fromPos;
			targetDist = dir.magnitude;
			this.targetDirection = dir;
            if (targetDist <= arriveDistance) {
				if (!isStoped) {
                    onFinishFire (true);
				}
				//Send a move request, this ensures gravity is applied
				return Vector3.zero;
			}
			//forward = Vector3.zero;
			forward = transform.forward;//  + dir.y * Vector3.up;
		
			dot = Vector3.Dot (dir.normalized, forward);
			sp = speed * Mathf.Max (dot, minMoveScale);//* slowdown;
		
			if (Time.fixedDeltaTime > 0) {
				sp = Mathf.Clamp (sp, 0, targetDist / (Time.fixedDeltaTime));
			}
			return  forward * sp;// + dir.y * Vector3.up * sp;
		}

		public void timeOut ()
		{
			onFinishFire (true);
		}

		public virtual void stop ()
		{
			if (isStoped) {
				return;
			}
			CancelInvoke ("timeOut");
			isStoped = true;
			isFireNow = false;
			NGUITools.SetActive (gameObject, false);
			CLBulletPool.returnObj (this);
		}

		public void onFinishFire (bool needRelease)
		{
			if (needRelease) {
				isFireNow = false;
				stop ();
			}
			Utl.doCallback (onFinishCallback, this);
		}

		public static CLBulletBase fire (CLUnit attacker, CLUnit target, Vector3 orgPos, 
		                                 Vector3 dir, object attr, object data, object callbak)
		{
			if (attr == null || attacker == null) {
				Debug.LogError ("bullet attr is null");
				return null;
			}
        
			string bulletName = MapEx.getString (attr, "PrefabName");
			if (!CLBulletPool.havePrefab (bulletName)) {
				ArrayList list = new ArrayList ();
				list.Add (attacker);
				list.Add (target);
				list.Add (orgPos);
				list.Add (dir);
				list.Add (attr);
				list.Add (data);
				list.Add (callbak);
				CLBulletPool.borrowObjAsyn (bulletName, (Callback)onFinishBorrowBullet, list, null);
				return null;
			}

			CLBulletBase bullet = CLBulletPool.borrowObj (bulletName);
			if (bullet == null) {
				return null;
			}

			bullet.doFire (attacker, target, orgPos, dir, attr, data, callbak);
			NGUITools.SetActive (bullet.gameObject, true);
//		bullet.FixedUpdate();
			return bullet;
		}

		static void onFinishBorrowBullet (params object[] args)
		{
			CLBulletBase bullet = (CLBulletBase)(args [1]);
			if (bullet != null) {
				ArrayList list = (ArrayList)(args [2]);
				CLUnit attacker = (CLUnit)(list [0]);
				CLUnit target = (CLUnit)(list [1]);
				Vector3 orgPos = (Vector3)(list [2]);
				Vector3 dir = (Vector3)(list [3]);
				object attr = (list [4]);
				object data = (list [5]);
				object callbak = list [6];
//				fire (attacker, target, orgPos, dir, attr, data, callbak);
				bullet.doFire (attacker, target, orgPos, dir, attr, data, callbak);
				NGUITools.SetActive (bullet.gameObject, true);
			}
		}
	}
}
