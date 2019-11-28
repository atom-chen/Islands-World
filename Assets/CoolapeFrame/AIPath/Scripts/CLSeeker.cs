using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coolape
{
    public class CLSeeker : MonoBehaviour
    {
        public CLAStarPathSearch mAStarPathSearch;
        public Transform target;
        public Vector3 targetPos;
        //移动速度
        public float speed = 1;
        //转向速度（当为负=硬转向）
        public float turningSpeed = 5;
        //离目标多远就停止移动
        public float endReachedDistance = 0.2f;
        public bool canSearchPath = true;
        public bool canMove = false;
        public bool autoMoveOnFinishSeek = true;
        public MovingBy movingBy = MovingBy.Update;
        public bool unscaledTime = false;

        public bool showPath = true;
        [System.NonSerialized]
        public List<Vector3> pathList = new List<Vector3>();
        public object onFinishSeekCallback;
        public object onMovingCallback;
        public object onArrivedCallback;
        [HideInInspector]
        public bool isSeekTargetLoop = false;
        public float seekTargetLoopIntvalSec = 1;
        public float nextSeekTargetTime = 0;

        Transform _mTransform;
        public Transform mTransform
        {
            get
            {
                if (_mTransform == null)
                {
                    _mTransform = gameObject.transform;
                }
                return _mTransform;
            }
        }

        // Use this for initialization
        public virtual void Start()
        {
            if (mAStarPathSearch == null)
            {
                mAStarPathSearch = CLAStarPathSearch.current;
            }
        }

        public virtual float cellSize
        {
            get
            {
                return mAStarPathSearch.cellSize;
            }
        }

        /// <summary>
        /// Init the specified onFinishSeekCallback, onMovingCallback and onArrivedCallback.
        /// </summary>
        /// <param name="onFinishSeekCallback">On finish seek callback.完成寻路后的回（其实是同步模式）</param>
        /// <param name="onMovingCallback">On moving callback.移动时的回调</param>
        /// <param name="onArrivedCallback">On arrived callback.到达目的地的回调</param>
        public virtual void init(object onFinishSeekCallback, object onMovingCallback, object onArrivedCallback)
        {
            this.onFinishSeekCallback = onFinishSeekCallback;
            this.onMovingCallback = onMovingCallback;
            this.onArrivedCallback = onArrivedCallback;
        }

        public virtual List<Vector3> seekTarget(Transform target)
        {
            return seekTarget(target, 1);
        }

        /// <summary>
        /// Seeks the target.寻找目标对
        /// </summary>
        /// <returns>The target.</returns>
        /// <param name="target">Target.目标对象</param>
        /// <param name="searchIntvalSec">Search intval sec.每隔xx秒重新寻路</param>
        public virtual List<Vector3> seekTarget(Transform target, float searchIntvalSec)
        {
            if (target == null) return null;
            this.target = target;
            canSearchPath = true;
            //fixedInvoke((Callback)seekTargetLoop, searchIntvalSec, searchIntvalSec);
            seekTargetLoopIntvalSec = searchIntvalSec;
            isSeekTargetLoop = true;
            nextSeekTargetTime = Time.realtimeSinceStartup + seekTargetLoopIntvalSec;
            return seek(target.position);
        }

        /// <summary>
        /// Cancels the seek target.取消对目标的寻路
        /// </summary>
        public virtual void cancelSeekTarget()
        {
            isSeekTargetLoop = false;
            //cancelFixedInvoke4Lua((Callback)seekTargetLoop);
        }

        void seekTargetLoop()
        {
            if (!canSearchPath || target == null)
            {
                return;
            }
            //float searchIntvalSec = (float)(objs[0]);
            seek(target.position);
            nextSeekTargetTime = Time.realtimeSinceStartup + seekTargetLoopIntvalSec;
            //fixedInvoke((Callback)seekTargetLoop, searchIntvalSec, searchIntvalSec);
        }

        public virtual void seekAsyn(Vector3 toPos)
        {
            targetPos = toPos;
            canMove = false;
            stopMove();
            //pathList.Clear();
            mAStarPathSearch.searchPathAsyn(mTransform.position, toPos, (Callback)onSeekAsynCallback);
        }

        void onSeekAsynCallback(params object[] objs)
        {
            bool canReach = (bool)(objs[0]);
            pathList = objs[1] as List<Vector3>;

            //回调的第一个参数是路径，第二个参数是能否到达目标点
            Utl.doCallback(onFinishSeekCallback, pathList, canReach);

            if (autoMoveOnFinishSeek)
            {
                //开始移动
                startMove();
            }
        }

        public virtual List<Vector3> seek(Vector3 toPos, float endReachDis)
        {
            endReachedDistance = endReachDis;
            return seek(toPos);
        }
        /// <summary>
        /// Seek the specified toPos.寻路
        /// </summary>
        /// <returns>The seek.路径列表</returns>
        /// <param name="toPos">To position.</param>
        public virtual List<Vector3> seek(Vector3 toPos)
        {
            targetPos = toPos;
            canMove = false;
            stopMove();
            pathList.Clear();
            bool canReach = mAStarPathSearch.searchPath(mTransform.position, toPos, ref pathList);

            //回调的第一个参数是路径，第二个参数是能否到达目标点
            Utl.doCallback(onFinishSeekCallback, pathList, canReach);

            if (autoMoveOnFinishSeek)
            {
                //开始移动
                startMove();
            }
            return pathList;
        }

        // Update is called once per frame
        public virtual void Update()
        {
            if(isSeekTargetLoop) {
                if(Time.realtimeSinceStartup >= nextSeekTargetTime) {
                    seekTargetLoop();
                }
            }
            if (canMove && movingBy == MovingBy.Update)
            {
                if (unscaledTime)
                {
                    moving(Time.unscaledDeltaTime);
                }
                else
                {
                    moving(Time.deltaTime);
                }
            }
        }

        public virtual void FixedUpdate()
        {
            if (canMove && movingBy == MovingBy.FixedUpdate)
            {
                if (unscaledTime)
                {
                    moving(Time.fixedUnscaledDeltaTime);
                }
                else
                {
                    moving(Time.fixedDeltaTime);
                }
            }
        }

        int nextPahtIndex = 0;
        float movePersent = 0;
        bool finishOneSubPath = false;
        float offsetSpeed = 0;
        Vector3 diff4Moving = Vector3.zero;
        Vector3 fromPos4Moving = Vector3.zero;
        /// <summary>
        /// Begains the move.
        /// </summary>
        public virtual void startMove()
        {
            canMove = false;
            if (pathList == null || pathList.Count < 2)
            {
                Debug.LogError("Path list error!");
                return;
            }
            if (Vector3.Distance(mTransform.position, pathList[0]) < 0.001f)
            {
                //说明是在原点
                movePersent = 0;
                finishOneSubPath = false;
                fromPos4Moving = pathList[0];
                diff4Moving = pathList[1] - pathList[0];
                offsetSpeed = 1.0f / Vector3.Distance(pathList[0], pathList[1]);
                nextPahtIndex = 1;
                rotateTowards(diff4Moving, true);
                canMove = true;
            }
            else if (Vector3.Distance(mTransform.position, pathList[pathList.Count - 1]) <= endReachedDistance)
            {
                //到达目标点
                Utl.doCallback(onArrivedCallback);
                return;
            }
            else
            {
                float dis = 0;
                float dis1 = 0;
                float dis2 = 0;
                for (int i = 1; i < pathList.Count; i++)
                {
                    dis = Vector3.Distance(pathList[i - 1], pathList[i]);
                    dis1 = Vector3.Distance(mTransform.position, pathList[i - 1]);
                    dis2 = Vector3.Distance(mTransform.position, pathList[i]);
                    if (Mathf.Abs(dis - (dis1 + dis2)) < 0.01f)
                    {
                        movePersent = dis1 / dis;
                        finishOneSubPath = false;
                        nextPahtIndex = i;
                        fromPos4Moving = pathList[i - 1];
                        diff4Moving = pathList[i] - pathList[i - 1];
                        offsetSpeed = 1.0f / Vector3.Distance(pathList[i - 1], pathList[i]);
                        rotateTowards(diff4Moving, true);
                        canMove = true;
                        break;
                    }
                }
                if(!canMove)
                {
                    Debug.LogWarning("说明没有找到起点，但是还是强制设置成可以移动");
                    canMove = true;
                }
            }
        }

        public virtual void stopMove()
        {
            canMove = false;
        }

        /// <summary>
        /// Moving the specified deltaTime.处理移动
        /// </summary>
        /// <param name="deltaTime">Delta time.会根据不同的情况传入</param>
        protected void moving(float deltaTime)
        {
            if (pathList == null || nextPahtIndex >= pathList.Count)
            {
                Debug.LogError("moving error");
                return;
            }

            movePersent += (deltaTime * speed * 0.3f * offsetSpeed);
            if (movePersent >= 1)
            {
                movePersent = 1;
                finishOneSubPath = true;
            }
            if (turningSpeed > 0)
            {
                rotateTowards(diff4Moving, false);
            }
            mTransform.position = fromPos4Moving + diff4Moving * movePersent;

            Utl.doCallback(onMovingCallback);

            if (nextPahtIndex == pathList.Count - 1)
            {
                //最后一段路径，即将要到达目的地
                if (finishOneSubPath ||
                    Vector3.Distance(mTransform.position, pathList[pathList.Count - 1]) <= endReachedDistance)
                {
                    stopMove();
                    Utl.doCallback(onArrivedCallback);
                }
            }
            else
            {
                if (finishOneSubPath)
                {
                    //移动完成一段路径，进入下一段路径的准备（一些变量的初始化）
                    nextPahtIndex++;
                    movePersent = 0;
                    finishOneSubPath = false;
                    fromPos4Moving = pathList[nextPahtIndex - 1];
                    diff4Moving = pathList[nextPahtIndex] - pathList[nextPahtIndex - 1];
                    offsetSpeed = 1.0f / Vector3.Distance(pathList[nextPahtIndex - 1], pathList[nextPahtIndex]);
                    if (turningSpeed <= 0)
                    {
                        rotateTowards(diff4Moving, true);
                    }
                }
            }
        }

        /// <summary>
        /// Rotates the towards.转向
        /// </summary>
        /// <param name="dir">Dir.方向</param>
        /// <param name="immediately">If set to <c>true</c> immediately.立即转向，否则为慢慢转</param>
        protected void rotateTowards(Vector3 dir, bool immediately)
        {
            if (dir == Vector3.zero) return;

            Quaternion rot = mTransform.rotation;
            Quaternion toTarget = Quaternion.LookRotation(dir);

            if (immediately)
            {
                rot = toTarget;
            }
            else
            {
                rot = Quaternion.Slerp(rot, toTarget, turningSpeed * Time.deltaTime);
            }
            Vector3 euler = rot.eulerAngles;
            euler.z = 0;
            euler.x = 0;
            rot = Quaternion.Euler(euler);

            mTransform.rotation = rot;
        }

        //====================================================================
        //====================================================================
#if UNITY_EDITOR
        void OnDrawGizmos()
        {

            if (showPath && pathList != null && pathList.Count > 0)
            {
                for (int i = 0; i < pathList.Count - 1; i++)
                {
                    Debug.DrawLine(pathList[i], pathList[i + 1], Color.green);
                }
            }
        }
#endif


        //====================================================================
        //====================================================================
        public enum MovingBy
        {
            Update,
            FixedUpdate
        }
    }
}
