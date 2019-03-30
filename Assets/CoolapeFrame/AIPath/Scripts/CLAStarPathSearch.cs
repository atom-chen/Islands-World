using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Coolape
{

    /// <summary>
    /// CLAS tar path search. 我写的A星寻路,只支持2D
    /// </summary>
    public class CLAStarPathSearch : MonoBehaviour
    {
        public static CLAStarPathSearch current;
        public int numRows = 10;
        public int numCols = 10;
        public float cellSize = 1;
        //寻路是4方向还是8方向
        public NumNeighbours numNeighbours = NumNeighbours.Eight;
        //扫描类型
        public ScanType scanType = ScanType.ObstructNode;
        //障碍
        public LayerMask obstructMask;
        //可以通行的layer
        public LayerMask passableMask;
        //检测障碍时用
        public float rayDis4Scan = 1;
        //检测障碍时用
        public RayDirection rayDirection = RayDirection.Up;
        //自动扫描一次障碍，init后自己用调用
        public bool isAutoScan = true;
        //过滤掉多余的节(当为true时，障碍物的collider尽量和障碍物保持一至大小，因为是通过射线来检测过滤多余的节点)
        public bool isFilterPathByRay = false;

        //柔化路径
        public bool isSoftenPath = true;
        public CLAIPathUtl.SoftenPathType softenPathType = CLAIPathUtl.SoftenPathType.Line;
        public int softenFactor = 3;

        //网格基础数据
        public GridBase grid = new GridBase();
        //节点map
        public Dictionary<int, CLAStarNode> nodesMap = new Dictionary<int, CLAStarNode>();
        public bool showGrid = true;
        public bool showObstruct = true;
        public bool isIninted = false;
        public ArrayList OnGridStateChgCallbacks = new ArrayList();
        //当ray检测后，再检测一次Sphere以保当节点在障碍内部时也可能检测成功
        float radius4CheckSphere = 1;

        //异步的
        static ListPool listPool = new ListPool();
        Queue<ArrayList> searchQueue = new Queue<ArrayList>();
        Queue<ArrayList> finishSearchQueue = new Queue<ArrayList>();

        public CLAStarPathSearch()
        {
            current = this;
        }

        // Use this for initialization
        public void Start()
        {
            if (isAutoScan)
            {
                init();
            }
        }

        /// <summary>
        /// Init this instance.初始化网格
        /// </summary>
        public void init()
        {
            radius4CheckSphere = cellSize / 4;
            grid.Awake(transform.position, numRows, numCols, cellSize, false);

            for (int i = 0; i < grid.NumberOfCells; i++)
            {
                nodesMap[i] = new CLAStarNode(i, grid.GetCellCenter(i));
            }

            //设置每个节点的左右上下一周的节点
            for (int i = 0; i < grid.NumberOfCells; i++)
            {
                CLAStarNode left = null;
                nodesMap.TryGetValue(grid.LeftIndex(i), out left);
                CLAStarNode right = null;
                nodesMap.TryGetValue(grid.RightIndex(i), out right);
                CLAStarNode up = null;
                nodesMap.TryGetValue(grid.UpIndex(i), out up);
                CLAStarNode down = null;
                nodesMap.TryGetValue(grid.DownIndex(i), out down);
                CLAStarNode leftUp = null;
                nodesMap.TryGetValue(grid.LeftUpIndex(i), out leftUp);
                CLAStarNode leftDown = null;
                nodesMap.TryGetValue(grid.LeftDownIndex(i), out leftDown);
                CLAStarNode rightUp = null;
                nodesMap.TryGetValue(grid.RightUpIndex(i), out rightUp);
                CLAStarNode rightDown = null;
                nodesMap.TryGetValue(grid.RightDownIndex(i), out rightDown);
                if (nodesMap[i] != null)
                {
                    nodesMap[i].init(left, right, up, down, leftUp, leftDown, rightUp, rightDown);
                }
            }
            isIninted = true;
            if (isAutoScan)
            {
                scan();
            }
        }

        /// <summary>
        /// Scan this instance.扫描网格哪些是障碍格
        /// </summary>
        public void scan()
        {
            if (!isIninted)
            {
                init();
            }
            for (int i = 0; i < grid.NumberOfCells; i++)
            {
                scanOne(i);
            }
            onGridStateChg();
        }

        void scanOne(int index)
        {
            if(!nodesMap.ContainsKey(index))
            {
                return;
            }
            Vector3 position = nodesMap[index].position;
            if (scanType == ScanType.ObstructNode)
            {
                nodesMap[index].isObstruct = raycastCheckCell(position, obstructMask);
            }
            else
            {
                bool ispass = raycastCheckCell(position, passableMask);
                bool ishit = raycastCheckCell(position, obstructMask);
                nodesMap[index].isObstruct = ishit || !ispass;
            }
        }

        bool raycastCheckCell(Vector3 cellPos, LayerMask mask)
        {
            bool ishit = false;
            if (rayDirection == RayDirection.Both)
            {
                ishit = Physics.Raycast(cellPos, Vector3.up, rayDis4Scan, mask)
                              || Physics.Raycast(cellPos, -Vector3.up, rayDis4Scan, mask);
            }
            else if (rayDirection == RayDirection.Up)
            {
                ishit = Physics.Raycast(cellPos, Vector3.up, rayDis4Scan, mask);
            }
            else
            {
                ishit = Physics.Raycast(cellPos, -Vector3.up, rayDis4Scan, mask);
            }

            if (!ishit)
            {
                ishit = Physics.CheckSphere(cellPos, radius4CheckSphere, mask);
            }
            return ishit;
        }

        /// <summary>
        /// Refreshs the range.刷新坐标center，半径为r的网格的障碍状态
        /// </summary>
        /// <param name="center">Center.</param>
        /// <param name="r">The red component.半径格子数</param>
        public void scanRange(Vector3 center, int r)
        {
            int centerIndex = grid.GetCellIndex(center);
            scanRange(centerIndex, r);
        }

        public void scanRange(int centerIndex, int r)
        {
            List<int> cells = grid.getCells(centerIndex, r * 2);
            for (int i = 0; i < cells.Count; i++)
            {
                scanOne(cells[i]);
            }
            onGridStateChg();
        }

        /// <summary>
        /// Adds the grid state callback. 添加当网格有变化时的回调
        /// </summary>
        /// <param name="callback">Callback.</param>
        public void addGridStateChgCallback(object callback)
        {
            if (!OnGridStateChgCallbacks.Contains(callback))
            {
                OnGridStateChgCallbacks.Add(callback);
            }
        }

        /// <summary>
        /// Removes the grid state callback.移除当网格有变化时的回调
        /// </summary>
        /// <param name="callback">Callback.</param>
        public void removeGridStateChgCallback(object callback)
        {
            OnGridStateChgCallbacks.Remove(callback);
        }

        void onGridStateChg()
        {
            for (int i = 0; i < OnGridStateChgCallbacks.Count; i++)
            {
                Utl.doCallback(OnGridStateChgCallbacks[i]);
            }
        }

        /// <summary>
        /// Revises from node.修正寻路开始节点
        /// </summary>
        /// <returns>The from node.</returns>
        /// <param name="orgFromNode">Org from node.</param>
        CLAStarNode reviseFromNode(Vector3 fromPos, CLAStarNode orgFromNode)
        {
            if (!orgFromNode.isObstruct) return orgFromNode;
            int count = orgFromNode.aroundList.Count;
            CLAStarNode node = null;
            CLAStarNode fromNode = null;
            float dis = -1;
            float tmpDis = 0;
            for (int i = 0; i < count; i++)
            {
                node = orgFromNode.aroundList[i];
                if (node != null && !node.isObstruct)
                {
                    tmpDis = Vector3.Distance(node.position, fromPos);
                    if (dis < 0 || tmpDis < dis)
                    {
                        dis = tmpDis;
                        fromNode = orgFromNode.aroundList[i];
                    }
                }
            }
            return fromNode;
        }

        /// <summary>
        /// Searchs the path. 异步的寻路
        /// </summary>
        /// <returns><c>true</c>, if path was searched,可以到达 <c>false</c> otherwise.不可到过</returns>
        /// <param name="from">From.出发点坐标</param>
        /// <param name="to">To.目标点坐标</param>
        /// <param name="finishSearchCallback">finish Search Callback</param>
        public void searchPathAsyn(Vector3 from, Vector3 to, object finishSearchCallback)
        {
            ArrayList list = listPool.borrow();
            list.Add(from);
            list.Add(to);
            list.Add(finishSearchCallback);
            searchQueue.Enqueue(list);
            if(searchQueue.Count == 1)
            {
                ThreadEx.exec2(new System.Threading.WaitCallback(doSearchPathAsyn));
            }
        }

        void doSearchPathAsyn(object obj)
        {
            if (searchQueue.Count == 0) return;
            ArrayList list = searchQueue.Dequeue();
            Vector3 from = (Vector3)(list[0]);
            Vector3 to = (Vector3)(list[1]);
            object callback = list[2];
            List<Vector3> outPath = new List<Vector3>();
            bool canReach = searchPath(from, to, ref outPath, true);
            list.Clear();
            list.Add(callback);
            list.Add(canReach);
            list.Add(outPath);
            finishSearchQueue.Enqueue(list);

            doSearchPathAsyn(null);
        }

        /// <summary>
        /// Searchs the path.寻路
        /// </summary>
        /// <returns><c>true</c>, if path was searched,可以到达 <c>false</c> otherwise.不可到过</returns>
        /// <param name="from">From.出发点坐标</param>
        /// <param name="to">To.目标点坐标</param>
        /// <param name="vectorList">Vector list.路径点坐标列表</param>
        public bool searchPath(Vector3 from, Vector3 to, ref List<Vector3> vectorList, bool notPocSoftenPath = false)
        {
            if (!isIninted)
            {
                init();
            }

            if (vectorList == null)
            {
                vectorList = new List<Vector3>();
            }
            else
            {
                vectorList.Clear();
            }
            int fromIndex = grid.GetCellIndex(from);
            int toIndex = grid.GetCellIndex(to);
            if (fromIndex < 0 || toIndex < 0)
            {
                Debug.LogWarning("Can not reach");
                return false;
            }
            if (fromIndex == toIndex)
            {
                //就在目标点，直接判断为到达
                vectorList.Add(from);
                vectorList.Add(to);
                return true;
            }

            CLAStarNode fromNode = nodesMap[fromIndex];
            if (fromNode.isObstruct)
            {
                fromNode = reviseFromNode(from, fromNode);
                if (fromNode == null)
                {
                    Debug.LogWarning("无法到达");
                    //无法到达
                    return false;
                }
            }
            CLAStarNode toNode = nodesMap[toIndex];

            // 本次寻路的唯一key，（并发同时处理多个寻路时会用到）
            string key = fromNode.index + "_" + toNode.index;

            List<CLAStarNode> openList = new List<CLAStarNode>();
            Dictionary<int, bool> closedList = new Dictionary<int, bool>();
            // F值缓存
            Dictionary<int, float> fValueMap = new Dictionary<int, float>();

            //先把开始点加入到closedList
            closedList[fromIndex] = true;
            //计算一次open点列表
            calculationOpenList(key, fromNode, toNode, ref fValueMap, ref openList, closedList);

            //离目标点最近的节点
            CLAStarNode nodeNearest = fromNode;
            CLAStarNode node = null;
            float dis4Target = -1;
            float tmpdis4Target = 0;
            int count = openList.Count;
            bool canReach = false;
            while (count > 0)
            {
                node = openList[count - 1];
                openList.RemoveAt(count - 1); //从openlist中移除
                closedList[node.index] = true;//设为closed

                if (node.index == toNode.index)
                {
                    //reached
                    nodeNearest = node;
                    canReach = true;
                    break;
                }
                // 设置离目标点最新的点
                tmpdis4Target = distance(node, toNode);
                if (dis4Target < 0 || tmpdis4Target < dis4Target)
                {
                    dis4Target = tmpdis4Target;
                    nodeNearest = node;
                }
                //重新处理新的open点
                calculationOpenList(key, node, toNode, ref fValueMap, ref openList, closedList);
                count = openList.Count;
            }
            //回溯路径＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
            CLAStarNode parentNode = null;
            if (canReach)
            {
                vectorList.Insert(0, to);
                parentNode = nodeNearest.getParentNode(key);
            }
            else
            {
                parentNode = nodeNearest;
            }

            while (parentNode != null && parentNode != fromNode)
            {
                vectorList.Insert(0, parentNode.position);
                parentNode = parentNode.getParentNode(key);
            }
            vectorList.Insert(0, from);
            if (!notPocSoftenPath)
            {
                softenPath(ref vectorList);
            }

            return canReach;
        }

        public void softenPath(ref List<Vector3> vectorList)
        {
            if (isFilterPathByRay)
            {
                filterPath(ref vectorList);
            }
            if (isSoftenPath)
            {
                CLAIPathUtl.softenPath(ref vectorList, softenPathType, softenFactor, cellSize);
            }
        }

        /// <summary>
        /// Filters the path.过滤掉多余的节(障碍物的collider尽量和障碍物保持一至大小，因为是通过射线来检测过滤多余的节点)
        /// </summary>
        /// <param name="list">List.</param>
        public void filterPath(ref List<Vector3> list)
        {
            if (list == null || list.Count < 4)
            {
                return;
            }
            Vector3 from = list[0];
            int i = 2;
            int fromIndex = grid.GetCellIndex(from);
            CLAStarNode fromNode = nodesMap[fromIndex];
            if (fromNode.isObstruct)
            {
                //因为list[0]有可能正好在障碍物的里面，这时射线是检测不到的
                from = list[1];
                i = 3;
            }

            float dis = 0;
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

        /// <summary>
        /// Calculations the open list.计算可行的点
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="openList">Open list.</param>
        void calculationOpenList(string key, CLAStarNode from, CLAStarNode to, ref Dictionary<int, float> fValueMap, ref List<CLAStarNode> openList, Dictionary<int, bool> closedList)
        {
            if (isNewOpenNode(from.up, openList, closedList))
            {
                addOpenNode(key, from.up, from, to, ref fValueMap, ref openList);
            }
            if (isNewOpenNode(from.down, openList, closedList))
            {
                addOpenNode(key, from.down, from, to, ref fValueMap, ref openList);
            }
            if (isNewOpenNode(from.left, openList, closedList))
            {
                addOpenNode(key, from.left, from, to, ref fValueMap, ref openList);
            }
            if (isNewOpenNode(from.right, openList, closedList))
            {
                addOpenNode(key, from.right, from, to, ref fValueMap, ref openList);
            }
            if (numNeighbours == NumNeighbours.Eight)
            {
                if (isNewOpenNode(from.leftUp, openList, closedList))
                {
                    addOpenNode(key, from.leftUp, from, to, ref fValueMap, ref openList);
                }
                if (isNewOpenNode(from.leftDown, openList, closedList))
                {
                    addOpenNode(key, from.leftDown, from, to, ref fValueMap, ref openList);
                }
                if (isNewOpenNode(from.rightUp, openList, closedList))
                {
                    addOpenNode(key, from.rightUp, from, to, ref fValueMap, ref openList);
                }
                if (isNewOpenNode(from.rightDown, openList, closedList))
                {
                    addOpenNode(key, from.rightDown, from, to, ref fValueMap, ref openList);
                }
            }
        }

        /// <summary>
        /// Adds the open node.新加入node，list的最后一位是f值最小的
        /// </summary>
        /// <param name="node">Node.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="fValueMap">F value map.</param>
        /// <param name="openList">Open list.</param>
        void addOpenNode(string key, CLAStarNode node, CLAStarNode from, CLAStarNode to, ref Dictionary<int, float> fValueMap, ref List<CLAStarNode> openList)
        {
            float fval = distance(node, from) + distance(node, to);
            fValueMap[node.index] = fval;
            int i = openList.Count - 1;
            for (; i >= 0; i--)
            {
                float fval2 = fValueMap[openList[i].index];
                if (fval <= fval2)
                {
                    break;
                }
            }
            //列表最后是F值最小的
            openList.Insert(i + 1, node);
            //设置该点的父节点，以便路径回溯时用
            node.setParentNode(from, key);
        }

        /// <summary>
        /// Ises the new open node. 节点是否需要新加入open
        /// </summary>
        /// <returns><c>true</c>, if new open node was ised, <c>false</c> otherwise.</returns>
        /// <param name="node">Node.</param>
        /// <param name="openList">Open list.</param>
        /// <param name="closedList">Closed list.</param>
        bool isNewOpenNode(CLAStarNode node, List<CLAStarNode> openList, Dictionary<int, bool> closedList)
        {
            if (node == null
               || node.isObstruct
               || openList.Contains(node)
               || closedList.ContainsKey(node.index)
              )
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Distance the specified node1 and node2.两个节点之间的距离
        /// </summary>
        /// <returns>The distance.</returns>
        /// <param name="node1">Node1.</param>
        /// <param name="node2">Node2.</param>
        public float distance(CLAStarNode node1, CLAStarNode node2)
        {
            return Vector3.Distance(node1.position, node2.position);
        }

        public virtual void Update()
        {
            if(finishSearchQueue.Count > 0)
            {
                ArrayList list = finishSearchQueue.Dequeue();
                object callback = list[0];
                bool canReach = (bool)(list[1]);
                List<Vector3> outPath = list[2] as List<Vector3>;
                softenPath(ref outPath);
                Utl.doCallback(callback, canReach, outPath);
                listPool.release(list);
            }
        }
        //=============================================================
        //=============================================================
        //=============================================================
#if UNITY_EDITOR
        Camera sceneCamera;
        //List<Vector3> _pathList;
        //public void showPath(List<Vector3> path) {
        //    _pathList = path;
        //}

        void OnDrawGizmos()
        {
            if (showGrid)
            {
                GridBase.DebugDraw(transform.position, numRows, numCols, cellSize, Color.white);
            }
            if (showObstruct)
            {
                Vector3 pos;
                for (int i = 0; i < grid.NumberOfCells; i++)
                {
                    CLAStarNode node = nodesMap[i];
                    if (node.isObstruct)
                    {
                        //显示障碍格子
                        pos = node.position;
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(pos, Vector3.one * cellSize);
                        Gizmos.color = Color.white;
                    }
                }
            }

            //===========================================
            //显示cell的周围格子============================
            /*
            Vector3 mousePosition = Event.current.mousePosition;
            sceneCamera = SceneView.lastActiveSceneView.camera;
            mousePosition.y = sceneCamera.pixelHeight - mousePosition.y;
            mousePosition = sceneCamera.ScreenToWorldPoint(mousePosition);
            //mousePosition.y = -mousePosition.y;
            mousePosition.y = 0;
            int index = grid.GetCellIndex(mousePosition);
            if (index >= 0)
            {
                CLAStarNode node = nodesMap[index];
                if (node != null)
                {
                    for (int j = 0; j < node.aroundList.Count; j++)
                    {
                        pos = node.aroundList[j].position;
                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(pos, Vector3.one * cellSize);
                        Gizmos.color = Color.white;
                    }
                }
            }
            */
            //===========================================
            //===========================================
        }
#endif


        //==============================================
        //==============================================
        public enum RayDirection
        {
            Up,     /**< Casts the ray from the bottom upwards */
            Down,   /**< Casts the ray from the top downwards */
            Both    /**< Casts two rays in either direction */
        }
        public enum NumNeighbours
        {
            Four,
            Eight
        }
        public enum ScanType
        {
            ObstructNode,
            PassableNode
        }

        public class ListPool
        {
            Queue queue = new Queue();
            public ArrayList borrow()
            {
                if (queue.Count == 0)
                {
                    newList();
                }
                return queue.Dequeue() as ArrayList;
            }

            public void release(ArrayList list)
            {
                list.Clear();
                queue.Enqueue(list);
            }

            void newList()
            {
                queue.Enqueue(new ArrayList());
            }
        }
    }
}
