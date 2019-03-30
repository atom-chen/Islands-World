using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Coolape;

namespace Coolape
{
    /// <summary>
    /// CLAIP ath by ray simple.寻路通过射线[[只能处理简单的寻路,特别在凹形地形时可能无法寻到路]]
    /// </summary>
    [ExecuteInEditMode]
    public class CLSeekerByRay : CLSeeker
    {
        public LayerMask obstructMask;         //障碍
        public float rayDistance = 1f;            //射线长度
        //public float selfSize = 1;
        public float rayHeight = 0.5f;
        public SearchDirs rayDirs = SearchDirs._8;
        public int maxSearchTimes = 100;        //最大寻路次数

        //柔化路径
        public bool isSoftenPath = true;
        public CLAIPathUtl.SoftenPathType softenPathType = CLAIPathUtl.SoftenPathType.Line;
        public int softenFactor = 3;
        //---------------------------------------------
        List<Vector3> pointsDirLeft = new List<Vector3>();  //左边的方向的点
        List<Vector3> pointsDirRight = new List<Vector3>(); //右边的方向的点
        bool canLeftSearch = true;
        bool canRightSearch = true;
        //---------------------------------------------
        //几个方向
        public enum SearchDirs
        {
            _4,
            _8,
            _16,
            _32,
        }

        public enum _Dir
        {
            up,
            bottom,
            left,
            right,
        }

        public int dirNum
        {
            get
            {
                switch (rayDirs)
                {
                    case SearchDirs._4:
                        return 4;
                    case SearchDirs._8:
                        return 8;
                    case SearchDirs._16:
                        return 16;
                    case SearchDirs._32:
                        return 32;
                }
                return 8;
            }
        }

        /// <summary>
        /// Gets the origin position.起点
        /// </summary>
        /// <value>The origin position.</value>
        public Vector3 originPosition
        {
            get
            {
                Vector3 pos = mTransform.position;
                pos.y = rayHeight;
                return pos;
            }
        }

        public override void Start()
        {
            base.Start();
            //resetCache();
        }

        public override float cellSize
        {
            get
            {
                return rayDistance;
            }
        }

        public void resetCache()
        {
            canLeftSearch = true;
            canRightSearch = true;
            pathList = null;
            tmpVectorList1.Clear();
            tmpVectorList2.Clear();
            pointsDirLeft.Clear();
            pointsDirRight.Clear();
            getPoints(originPosition, mTransform.eulerAngles, rayDistance, ref pointsDirLeft, ref pointsDirRight);
        }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <param name="center">From position.中心点</param>
        /// <param name="eulerAngles">Euler angles.朝向</param>
        /// <param name="r">The red component.半径</param>
        /// <param name="left">Left.左半点列表</param>
        /// <param name="right">Right.右半点列表</param>
        public void getPoints(Vector3 center, Vector3 eulerAngles, float r, ref List<Vector3> left, ref List<Vector3> right)
        {
            //		float angle = 360.0f / (dirNum + 1);
            float angle = 360.0f / (dirNum);
            int half = dirNum / 2;
            Vector3 pos = Vector3.zero;
            for (int i = 0; i <= half; i++)
            {
                pos = AngleEx.getCirclePointStartWithYV3(center, r, eulerAngles.y - i * angle);
                pos.y = rayHeight;
                left.Add(pos);

                pos = AngleEx.getCirclePointStartWithYV3(center, r, eulerAngles.y - (dirNum - i) * angle);
                pos.y = rayHeight;
                right.Add(pos);
            }
        }


        public override List<Vector3> seek(Vector3 toPos)
        {
            canMove = false;
            resetCache();
            begainTime = Time.time;
            bool isCanReach = false;
            List<Vector3> leftPath = trySearchPathLeft(toPos);
            List<Vector3> rightPath = trySearchPathRight(toPos);
            if (leftPath != null && leftPath.Count > 0 && (rightPath == null || rightPath.Count == 0))
            {
                isCanReach = true;
                pathList = leftPath;
            }
            else if ((leftPath == null || leftPath.Count == 0) && rightPath != null && rightPath.Count > 0)
            {
                isCanReach = true;
                pathList = rightPath;
            }
            else if (leftPath != null && leftPath.Count > 0 && rightPath != null && rightPath.Count > 0)
            {
                isCanReach = true;
                pathList = getShortList(leftPath, rightPath);
            }
            else
            {
                filterPath(ref tmpVectorList1);
                leftPath = tmpVectorList1;
                leftPath.Insert(0, mTransform.position);
                filterPath(ref tmpVectorList2);
                rightPath = tmpVectorList2;
                rightPath.Insert(0, mTransform.position);
                float dis1 = Vector3.Distance(leftPath[leftPath.Count - 1], toPos);
                float dis2 = Vector3.Distance(rightPath[rightPath.Count - 1], toPos);
                if (dis1 < dis2)
                {
                    pathList = leftPath;
                }
                else
                {
                    pathList = rightPath;
                }

                //计算离目标点最近的点
                float minDis = -1;
                float tmpMinDis = 0;
                int index = -1;
                for (int i = 0; i < pathList.Count; i++)
                {
                    tmpMinDis = Vector3.Distance(pathList[i], toPos);
                    if(minDis < 0 || tmpMinDis < minDis) {
                        minDis = tmpMinDis;
                        index = i;
                    }
                }
                for (int i = index + 1; i < pathList.Count; i++) {
                    pathList.RemoveAt(i);
                }

                isCanReach = false;
#if UNITY_EDITOR
                Debug.LogWarning("Cannot search path");
#endif
            }

            if (isSoftenPath)
            {
                CLAIPathUtl.softenPath(ref pathList, softenPathType, softenFactor, cellSize);
            }

            //回调的第一个参数是路径，第二个参数是能否到达目标点
            Utl.doCallback(onFinishSeekCallback, pathList, isCanReach);

            if (autoMoveOnFinishSeek)
            {
                //开始移动
                startMove();
            }
            return pathList;
        }

        public List<Vector3> trySearchPathLeft(Vector3 toPos)
        {
            //searchTime = 0;
            canLeftSearch = true;
            canRightSearch = false;
            tmpVectorList1.Clear();
            return doSearchPath(toPos);
        }

        public List<Vector3> trySearchPathRight(Vector3 toPos)
        {
            //searchTime = 0;
            canLeftSearch = false;
            canRightSearch = true;
            tmpVectorList2.Clear();
            return doSearchPath(toPos);
        }

        List<Vector3> tmpVectorList1 = new List<Vector3>();
        List<Vector3> tmpVectorList2 = new List<Vector3>();
        float begainTime = 0;
        public List<Vector3> doSearchPath(Vector3 toPos)
        {
            List<Vector3> list = null;

            if (canReach(originPosition, toPos, endReachedDistance))
            {
                list = new List<Vector3>();
                list.Add(mTransform.position);
                list.Add(toPos);
                return list;
            }
            int searchTime = 0;
            list = searchPathByRay(originPosition, originPosition, toPos, Utl.getAngle(mTransform, toPos), ref searchTime);
            filterPath(ref list);
            if (list != null && list.Count > 0)
            {
                list.Insert(0, mTransform.position);
            }
            return list;
        }

        /// <summary>
        /// Ises the in circle. 判断pos是不是正好在回路中
        /// </summary>
        /// <returns><c>true</c>, if in circle was ised, <c>false</c> otherwise.</returns>
        /// <param name="list">List.</param>
        /// <param name="pos">Position.</param>
        public bool isInCircle(Vector3 pos, List<Vector3> list)
        {
            if (list.Count >= 4)
            {
                float minDis = cellSize / 10.0f;
                if (Vector3.Distance(list[list.Count - 4], pos) < minDis)
                {
                    return true;
                }
                for (int i = list.Count - 1; i > 0; i--)
                {
                    if (Vector3.Distance(list[i], pos) < minDis)
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        /// <summary>
        /// Searchs the path by ray.
        /// </summary>
        /// <returns>The path by ray.</returns>
        /// <param name="prefromPos">Prefrom position.前一个起始点</param>
        /// <param name="fromPos">From position.当前的起始点</param>
        /// <param name="toPos">To position.目标点</param> 
        /// <param name="angle">Angle.当前点的朝向角度</param>
        List<Vector3> searchPathByRay(Vector3 prefromPos, Vector3 fromPos, Vector3 toPos, Vector3 angle, ref int searchTime)
        {
            if (maxSearchTimes > 0 && (searchTime + 1) >= maxSearchTimes)
            {
                Debug.LogWarning("search times at the maxtimes=" + maxSearchTimes);
                return null;
            }
            //Vector3 dir = toPos - fromPos;
            List<Vector3> vetorList = null;
            if (canReach(fromPos, toPos, 0))
            {
                //可以到达目标点，把该点加到路径中返回
                vetorList = new List<Vector3>();
                vetorList.Add(toPos);
                return vetorList;
            }
            else
            {
                //起始点不能到达目标点，则从起始点的周围开始重新寻路
                Vector3 angle2 = angle;
                Vector3 from = fromPos;
                //left
                List<Vector3> left = new List<Vector3>();
                //right
                List<Vector3> right = new List<Vector3>();
                getPoints(from, angle2, rayDistance, ref left, ref right);
                List<Vector3> leftResult = null;
                List<Vector3> rightResult = null;
                //------------------------------------------------------------------
                int count = left.Count;
                Vector3 oldPos = Vector3.zero;
                bool isFirstLeft = true;
                bool isFirstRight = true;
                _Dir curTargetDir = getTargetDir(from, prefromPos);
                _Dir toPosTargetDir = getTargetDir(from, toPos);
                for (int i = 0; i < count; i++)
                {
                    if (canLeftSearch && searchTime < maxSearchTimes)
                    {
                        if (canReach(from, left[i], 0) && !isInCircle(left[i], tmpVectorList1))
                        {
                            _Dir nextTargetDir = getTargetDir(left[i], from);

                            if ((curTargetDir == nextTargetDir)
                               || (curTargetDir == _Dir.up && (nextTargetDir == _Dir.left || nextTargetDir == _Dir.right))
                               || (curTargetDir == _Dir.bottom && (nextTargetDir == _Dir.left || nextTargetDir == _Dir.right))
                               || (curTargetDir == _Dir.left && (nextTargetDir == _Dir.up || nextTargetDir == _Dir.bottom))
                               || (curTargetDir == _Dir.right && (nextTargetDir == _Dir.up || nextTargetDir == _Dir.bottom))
                               )
                            {
                                //Debug.Log("left:" + searchTime + "," + curTargetDir + "," + nextTargetDir);
                                Vector3 tmpPos1 = left[i];//from;
                                Vector3 tmpPos2 = toPos;
                                if (isFirstLeft)
                                {
                                    isFirstLeft = false;
                                    //oldPos = left[i];
                                }
                                else
                                {
                                    tmpPos2 = left[i - 1];
                                    tmpPos1 = left[i];
                                }
                                _Dir _targetDir = getTargetDir(left[i], toPos);
                                if (_targetDir == _Dir.left ||
                                    _targetDir == _Dir.right)
                                {
                                    tmpPos1.z = 0;
                                    tmpPos2.z = 0;
                                }
                                else
                                {
                                    tmpPos1.x = 0;
                                    tmpPos2.x = 0;
                                }

                                tmpPos1.y = 0;
                                tmpPos2.y = 0;
                                angle2 = Utl.getAngle(tmpPos1, tmpPos2);


                                //加入到临时点列表中
                                tmpVectorList1.Add(left[i]);
                                //把新的起始点当作起始点重新寻路
                                searchTime++;
                                leftResult = searchPathByRay(from, left[i], toPos, angle2, ref searchTime);
                            }
                        }
                    }
                    if (canRightSearch && searchTime < maxSearchTimes)
                    {
                        if (canReach(from, right[i], 0) && !isInCircle(right[i], tmpVectorList2))
                        {
                            _Dir nextTargetDir = getTargetDir(right[i], from);

                            if ((curTargetDir == nextTargetDir)
                               || (curTargetDir == _Dir.up && (nextTargetDir == _Dir.left || nextTargetDir == _Dir.right))
                               || (curTargetDir == _Dir.bottom && (nextTargetDir == _Dir.left || nextTargetDir == _Dir.right))
                               || (curTargetDir == _Dir.left && (nextTargetDir == _Dir.up || nextTargetDir == _Dir.bottom))
                               || (curTargetDir == _Dir.right && (nextTargetDir == _Dir.up || nextTargetDir == _Dir.bottom))
                               )
                            {
                                //Debug.Log("right:" + searchTime + "," + curTargetDir + "," + nextTargetDir);
                                Vector3 tmpPos1 = right[i];//from;
                                Vector3 tmpPos2 = toPos;
                                if (isFirstRight)
                                {
                                    isFirstRight = false;
                                    oldPos = right[i];
                                }
                                else
                                {
                                    tmpPos2 = oldPos;//right[i - 1];//;
                                    tmpPos1 = right[i];
                                }
                                _Dir _targetDir = getTargetDir(right[i], toPos);
                                if (_targetDir == _Dir.left ||
                                   _targetDir == _Dir.right)
                                {
                                    tmpPos1.z = 0;
                                    tmpPos2.z = 0;
                                }
                                else
                                {
                                    tmpPos1.x = 0;
                                    tmpPos2.x = 0;
                                }

                                tmpPos1.y = 0;
                                tmpPos2.y = 0;
                                angle2 = Utl.getAngle(tmpPos1, tmpPos2);


                                tmpVectorList2.Add(right[i]);
                                searchTime++;
                                rightResult = searchPathByRay(from, right[i], toPos, angle2, ref searchTime);
                            }
                        }
                    }

                    if (leftResult != null && rightResult == null)
                    {
                        leftResult.Insert(0, left[i]);
                        leftResult.Insert(0, from);
                        vetorList = leftResult;
                        break;
                    }
                    else if (leftResult == null && rightResult != null)
                    {
                        rightResult.Insert(0, right[i]);
                        rightResult.Insert(0, from);
                        vetorList = rightResult;
                        break;
                    }
                    else if (leftResult != null && rightResult != null)
                    {
                        leftResult.Insert(0, left[i]);
                        rightResult.Insert(0, right[i]);
                        vetorList = getShortList(leftResult, rightResult);
                        vetorList.Insert(0, from);
                        break;
                    }
                }
            }
            return vetorList;
        }

        /// <summary>
        /// Cans the reach.能否到达
        /// </summary>
        /// <returns><c>true</c>, if reach was caned, <c>false</c> otherwise.</returns>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public bool canReach(Vector3 from, Vector3 to, float endReachedDis)
        {
            Vector3 _to = to;
            _to.y = rayHeight;
            Vector3 dir = _to - from;
            float dis = Vector3.Distance(from, to) - endReachedDis;
            dis = dis < 0 ? 0 : dis;
            if (!Physics.Raycast(from, dir, dis, obstructMask))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the short list.取得最短路径
        /// </summary>
        /// <returns>The short list.</returns>
        /// <param name="list1">List1.</param>
        /// <param name="list2">List2.</param>
        public List<Vector3> getShortList(List<Vector3> list1, List<Vector3> list2)
        {
            int count = list1.Count;
            float dis1 = 0;
            float dis2 = 0;
            for (int i = 0; i < count - 1; i++)
            {
                dis1 += Vector3.Distance(list1[i], list1[i + 1]);
            }
            count = list2.Count;
            for (int i = 0; i < count - 1; i++)
            {
                dis2 += Vector3.Distance(list2[i], list2[i + 1]);
            }
            return dis1 > dis2 ? list2 : list1;
        }

        public void filterPath(ref List<Vector3> list)
        {
            if (list == null || list.Count < 3)
            {
                return;
            }
            Vector3 from = list[0];
            float dis = 0;
            int i = 2;
            while (i < list.Count)
            {
                dis = Vector3.Distance(from, list[i]);
                if (Physics.Raycast(from, list[i] - from, dis, obstructMask))
                {
                    from = list[i - 1];
                    i++;
                }
                else
                {
                    list.RemoveAt(i - 1);
                }
            }
        }

        //到目标点的方向
        public _Dir getTargetDir(Vector3 fromPos, Vector3 toPos)
        {
            Vector3 euAngle = Utl.getAngle(fromPos, toPos);
            float angle = euAngle.y;
            if (angle < 0)
            {
                angle = 360 + angle;
            }
            if ((angle >= 0 && angle <= 45) ||
                (angle >= 315 && angle <= 360))
            {
                return _Dir.up;
            }
            else if ((angle >= 45 && angle <= 135))
            {
                return _Dir.right;
            }
            else if ((angle >= 135 && angle <= 225))
            {
                return _Dir.bottom;
            }
            else if ((angle >= 225 && angle <= 315))
            {
                return _Dir.left;
            }
            else
            {
                Debug.LogError("This angle not in switch case!!!! angle===" + angle);
            }
            return _Dir.up;
        }

#if UNITY_EDITOR
        float oldRayDis = 0;
        SearchDirs oldDirs = SearchDirs._8;
        float oldRayHeight = 0;
        Vector3 oldeulerAngles = Vector3.zero;
        Vector3 oldPosition = Vector3.zero;
        Matrix4x4 boundsMatrix;
        float nowTime = 0;

        void OnDrawGizmos()
        {
            if (!showPath) return;
            nowTime = Time.time;

            //		if(center == null) return;

            //		boundsMatrix.SetTRS (center.position, Quaternion.Euler (girdRotaion),new Vector3 (aspectRatio,1,1));
            //		AstarPath.active.astarData.gridGraph.SetMatrix();
            //		Gizmos.matrix =  AstarPath.active.astarData.gridGraph.boundsMatrix;
            //		Gizmos.matrix = boundsMatrix;
            Gizmos.color = Color.red;

            //for (int i = 0; canLeftSearch && i < pointsDirLeft.Count; i++)
            //{
            //    Gizmos.DrawWireCube(pointsDirLeft[i], Vector3.one * (0.04f + i * 0.005f));
            //    Debug.DrawLine(originPosition, pointsDirLeft[i]);
            //}

            //for (int i = 0; canRightSearch && i < pointsDirRight.Count; i++)
            //{
            //    Gizmos.DrawWireCube(pointsDirRight[i], Vector3.one * (0.04f + i * 0.005f));
            //    Debug.DrawLine(originPosition, pointsDirRight[i]);
            //}

            List<Vector3> list = tmpVectorList1;// tmpVectorList1;  //vectorList
            if (list != null && list.Count > 1)
            {
                int max = ((int)(nowTime - begainTime)) % list.Count;
                int i = 0;
                for (i = 0; i < list.Count - 1 && i <= max; i++)
                {
                    Gizmos.DrawWireCube(list[i], Vector3.one * 0.06f);
                    Debug.DrawLine(list[i], list[i + 1], Color.red);
                }
                Gizmos.DrawWireCube(list[i], Vector3.one * 0.06f);
            }

            list = tmpVectorList2;  //vectorList
            if (list != null && list.Count > 1)
            {
                int max = ((int)(nowTime - begainTime)) % list.Count;
                int i = 0;
                for (i = 0; i < list.Count - 1 && i <= max; i++)
                {
                    Gizmos.DrawWireCube(list[i], Vector3.one * 0.06f);
                    Debug.DrawLine(list[i], list[i + 1], Color.red);
                }
                Gizmos.DrawWireCube(list[i], Vector3.one * 0.06f);
            }

            //List<object> list2 = tmpPointsList1;  //vectorList
            //if (list2 != null && list2.Count > 1)
            //{
            //    int max = ((int)(nowTime - begainTime)) % list2.Count;
            //    int i = 0;
            //    for (i = 0; i < list2.Count - 1 && i <= max; i++)
            //    {
            //        list = list2[i] as List<Vector3>;
            //        for (int j = 0; canLeftSearch && j < list.Count; j++)
            //        {
            //            Gizmos.DrawWireCube(list[j], Vector3.one * (0.04f + j * 0.005f));
            //            //Debug.DrawLine(originPosition, list[i]);
            //        }
            //    }
            //}


            list = pathList;  //vectorList
            if (list != null && list.Count > 1)
            {
                int i = 0;
                for (i = 0; i < list.Count - 1; i++)
                {
                    //				Gizmos.DrawWireCube(list[i], Vector3.one*0.04f);
                    Debug.DrawLine(list[i], list[i + 1], Color.green);
                }
                Gizmos.DrawWireCube(list[i], Vector3.one * 0.04f);
            }
            //		Gizmos.matrix = Matrix4x4.identity;

            //		Gizmos.matrix = Matrix4x4.identity;
            //		Gizmos.color = Color.white;
        }
#endif
    }
}
