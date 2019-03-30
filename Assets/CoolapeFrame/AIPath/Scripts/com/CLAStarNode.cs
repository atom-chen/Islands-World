using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Coolape
{
    public class CLAStarNode
    {
        public CLAStarNode left;
        public CLAStarNode right;
        public CLAStarNode up;
        public CLAStarNode down;
        public CLAStarNode leftUp;
        public CLAStarNode leftDown;
        public CLAStarNode rightUp;
        public CLAStarNode rightDown;

        public List<CLAStarNode> aroundList = new List<CLAStarNode>();
        public int index;
        public Vector3 position;
        // 是障碍格子
        public bool isObstruct = false;
        //父节点，需要一个key作缓存，以免同时有多个寻路时parent值会冲突
        Dictionary<string, CLAStarNode> parentNodesMap = new Dictionary<string, CLAStarNode>();

        public CLAStarNode(int index, Vector3 pos)
        {
            this.index = index;
            this.position = pos;
        }

        public void init(
            CLAStarNode left,
            CLAStarNode right,
            CLAStarNode up,
            CLAStarNode down,
            CLAStarNode leftUp,
            CLAStarNode leftDown,
            CLAStarNode rightUp,
            CLAStarNode rightDown)
        {
            aroundList.Clear();
            this.left = left;
            if(left != null) {
                aroundList.Add(left);
            }
            this.right = right;
            if (right != null)
            {
                aroundList.Add(right);
            }
            this.up = up;
            if (up != null)
            {
                aroundList.Add(up);
            }
            this.down = down;
            if (down != null)
            {
                aroundList.Add(down);
            }
            this.leftUp = leftUp;
            if (leftUp != null)
            {
                aroundList.Add(leftUp);
            }
            this.leftDown = leftDown;
            if (leftDown != null)
            {
                aroundList.Add(leftDown);
            }
            this.rightUp = rightUp;
            if (rightUp != null)
            {
                aroundList.Add(rightUp);
            }
            this.rightDown = rightDown;
            if (rightDown != null)
            {
                aroundList.Add(rightDown);
            }
        }

        public void setParentNode(CLAStarNode preNode, string key)
        {
            parentNodesMap[key] = preNode;
        }
        public CLAStarNode getParentNode(string key)
        {
            CLAStarNode parent = null;
            if(parentNodesMap.TryGetValue(key, out parent)) {
                return parent;
            }
            return null;
        }
    }
}
