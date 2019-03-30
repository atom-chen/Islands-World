using UnityEngine;
using Hivemind;
using System.Collections.Generic;
using System.Collections;
using Coolape;

namespace Hivemind
{
	[System.Serializable]
	public class Node : ScriptableObject, System.IComparable
	{
		public string condition = "";
		public string result1 = "";
		public string result2 = "";

		// Editor settings
		[SerializeField]
		public Vector2 editorPosition;
		public int id;
		public int index = 0;
		public bool canMultTimes = false;
		public string desc = "";

		// The Behavior Tree this node belongs too
		public BehaviorTree behaviorTree;

		// Used by the debugger to visually display the last status returned
		public Status? lastStatus;
		public int lastTick = 0;

		public Node()
		{
			id = GetInstanceID();
		}

		// 要结束的节点
		public List<Node> KillNodes = new List<Node>();
		public List<Node> ParentKillNodes = new List<Node>();

		public virtual void ConnectKillChild(Node child)
		{
			if (!KillNodes.Contains(child) && !Children.Contains(child)) {
				KillNodes.Add(child);
				child.ParentKillNodes.Add(this);
			}
		}

		public virtual void DisconnectKillChild(Node child)
		{
			if (KillNodes.Contains(child)) {
				KillNodes.Remove(child);
				child.ParentKillNodes.Remove(this);
			}
		}

		// Child connections
		public virtual void ConnectChild(Node child)
		{
			if (child == null)
				return;
			if (!Children.Contains(child) && !Children.Contains(child)) {
				Children.Add(child);
				child.setParent(this);
			}
		}

		public virtual void DisconnectChild(Node child)
		{
			if (Children.Contains(child)) {
				Children.Remove(child);
				child.Unparent(this);
			}
		}

		[SerializeField]
		List<Node> _children = new List<Node>();

		public virtual List<Node> Children { get { return _children; } }

		public virtual int ChildCount { get { return Children.Count; } }

		public virtual bool CanConnectChild { get { return true; } }

		public virtual bool ContainsChild(Node child)
		{
			if (child == null)
				return false;
			return Children.Contains(child);
		}

		// IComparable for sorting left-to-right in the visual editor
		public int CompareTo(object other)
		{
			Node otherNode = other as Node;
			return editorPosition.x < otherNode.editorPosition.x ? -1 : 1;
		}

		// Parent connections
		[SerializeField]
		List<Node> _parents = new List<Node>();

		public virtual List<Node> parents {
			get { return _parents; }
		}

		public virtual void setParent(Node parent)
		{
			if (!parents.Contains(parent)) {
				parents.Add(parent);
			}
		}

		public virtual void Unparent()
		{
			for (int i = parents.Count - 1; i >= 0; i--) {
				Unparent(parents [i]);
			}
		}

		public virtual void Unparent(Node parent)
		{
			if (parent == null)
				return;
			if (!parents.Contains(parent)) {
				return;
			}
			if (parent != null) {
				parents.Remove(parent);
				parent.DisconnectChild(this);
			} else {
				Debug.LogWarning(string.Format("Attempted unparenting {0} while it has no parent"));
			}
		}

		// All connections
		public virtual void Disconnect()
		{
			// Disconnect parent
			Node parent = null;
			for (int i = 0; i < parents.Count; i++) {
				parent = parents [i];
				if (parent != null) {
					Unparent(parent);
				}
			}
			parents.Clear();

			// Disconnect children
			if (ChildCount > 0) {
				for (int i = ChildCount - 1; i >= 0; i--) {
					DisconnectChild(Children [i]);
				}
			}

			for (int i = KillNodes.Count - 1; i >= 0; i--) {
				DisconnectKillChild(KillNodes [i]);
			}
			Node killNode = null;
			for (int i = ParentKillNodes.Count - 1; i >= 0; i--) {
				killNode = ParentKillNodes [i];
				killNode.DisconnectKillChild(this);
				if(killNode is NodeBranch) {
					NodeBranch branch = killNode as NodeBranch;
					branch.DisconnectKillLeftChild(this);
					branch.DisconnectKillRightChild(this);
				}
			}
		}

		// Lifecycle
		public void OnEnable()
		{
			hideFlags = HideFlags.HideAndDontSave;
		}

		// Runtime
		public virtual Status Tick(GameObject agent, Context context)
		{
			return Status.Error;
		}

		public virtual void Deserialize(Hashtable map)
		{
			float x = float.Parse(map ["editorx"].ToString());
			float y = float.Parse(map ["editory"].ToString());
			editorPosition = new Vector2(x, y);
			id = MapEx.getInt(map, "id");
			desc = MapEx.getString(map, "desc");
			index = MapEx.getInt(map, "index");
			canMultTimes = MapEx.getBool(map, "canMultTimes");
		}

		public virtual void DeserializeChildren(Hashtable map)
		{
			ArrayList list = MapEx.getList(map, "Children");
			Node node = null;
			for (int i = 0; i < list.Count; i++) {
				int id = int.Parse(list [i].ToString());
				node = behaviorTree.getNodeByID(id);
				ConnectChild(node);
			}

			list = MapEx.getList(map, "KillNodes");
			node = null;
			for (int i = 0; i < list.Count; i++) {
				int id = int.Parse(list [i].ToString());
				node = behaviorTree.getNodeByID(id);
				ConnectKillChild(node);
			}
		}

		public virtual Hashtable Serialize()
		{
			Hashtable m = new Hashtable();
			m ["id"] = id;
			m ["editorx"] = editorPosition.x.ToString();
			m ["editory"] = editorPosition.y.ToString();
			ArrayList subNodes = new ArrayList();
			for (int i = 0; i < ChildCount; i++) {
				subNodes.Add(Children [i].id);
			}
			m ["Children"] = subNodes;

			ArrayList subkillNodes = new ArrayList();
			for (int i = 0; i < KillNodes.Count; i++) {
				subkillNodes.Add(KillNodes [i].id);
			}
			m ["KillNodes"] = subkillNodes;

			m ["type"] = GetType().ToString();
			m ["desc"] = desc;
			m ["index"] = index;
			m ["canMultTimes"] = canMultTimes;
			return m;
		}
	}
}
