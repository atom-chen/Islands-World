using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hivemind;
using Coolape;

namespace Hivemind
{
	public class NodeBranch : NodeAction
	{

		// 要结束的节点
		public List<Node> KillLeftNodes = new List<Node>();
		public List<Node> KillRightNodes = new List<Node>();

		public virtual void ConnectKillLeftChild(Node child)
		{
			if (!KillLeftNodes.Contains(child) && !ChildrenLeft.Contains(child)) {
				KillLeftNodes.Add(child);
				child.ParentKillNodes.Add(this);
			}
		}

		public virtual void DisconnectKillLeftChild(Node child)
		{
			if (KillLeftNodes.Contains(child)) {
				KillLeftNodes.Remove(child);
				child.ParentKillNodes.Remove(this);
			}
		}

		public virtual void ConnectKillRightChild(Node child)
		{
			if (!KillRightNodes.Contains(child) && !ChildrenRight.Contains(child)) {
				KillRightNodes.Add(child);
			}
		}

		public virtual void DisconnectKillRightChild(Node child)
		{
			if (KillRightNodes.Contains(child)) {
				KillRightNodes.Remove(child);
			}
		}


		[SerializeField]
		public List<Node> _childrenLeft = new List<Node>();
		[SerializeField]
		public List<Node> _childrenRight = new List<Node>();

		public virtual List<Node> ChildrenLeft { get { return _childrenLeft; } }

		public virtual List<Node> ChildrenRight { get { return _childrenRight; } }

		public virtual bool inLeft(Node node)
		{
			if (node == null)
				return false;
			return ChildrenLeft.Contains(node);
		}

		public virtual bool inRight(Node node)
		{
			if (node == null)
				return false;
			return ChildrenRight.Contains(node);
		}

		public virtual void ConnectLeftChild(Node child)
		{
			if (child == null)
				return;
			if (!ChildrenLeft.Contains(child) && !KillLeftNodes.Contains(child)) {
				ChildrenLeft.Add(child);
				child.setParent(this);
			}
		}

		public virtual void ConnectRightChild(Node child)
		{
			if (child == null)
				return;
			if (!ChildrenRight.Contains(child) && !KillRightNodes.Contains(child)) {
				ChildrenRight.Add(child);
				child.setParent (this);
			}
		}

		public virtual void DisconnectLeftChild(Node child)
		{
			if (child == null)
				return;
			if (ChildrenLeft.Contains(child)) {
				ChildrenLeft.Remove(child);
				child.Unparent(this);
			}
		}

		public virtual void DisconnectRightChild(Node child)
		{
			if (child == null)
				return;
			if (ChildrenRight.Contains(child)) {
				ChildrenRight.Remove(child);
				child.Unparent(this);
			}
		}

		public override void DisconnectChild(Node child)
		{
			if (child == null)
				return;
			base.DisconnectChild(child);
			DisconnectLeftChild(child);
			DisconnectRightChild(child);
		}

		public override void Disconnect()
		{
			base.Disconnect();

			// Disconnect children
			for (int i = ChildrenLeft.Count - 1; i >= 0; i--) {
				DisconnectLeftChild(ChildrenLeft [i]);
			}
			for (int i = ChildrenRight.Count - 1; i >= 0; i--) {
				DisconnectRightChild(ChildrenRight [i]);
			}

			for (int i = KillLeftNodes.Count - 1; i >= 0; i--) {
				DisconnectKillLeftChild(KillLeftNodes [i]);
			}
			for (int i = KillRightNodes.Count - 1; i >= 0; i--) {
				DisconnectKillRightChild(KillRightNodes [i]);
			}
		}

		public override Hashtable Serialize()
		{
			Hashtable map = base.Serialize();
			ArrayList lefts = new ArrayList();
			for (int i = 0; i < ChildrenLeft.Count; i++) {
				lefts.Add(ChildrenLeft [i].id);
			}
			map ["ChildrenLeft"] = lefts;

			ArrayList rights = new ArrayList();
			for (int i = 0; i < ChildrenRight.Count; i++) {
				rights.Add(ChildrenRight [i].id);
			}
			map ["ChildrenRight"] = rights;

			ArrayList killlefts = new ArrayList();
			for (int i = 0; i < KillLeftNodes.Count; i++) {
				killlefts.Add(KillLeftNodes [i].id);
			}
			map ["KillLeftNodes"] = killlefts;

			ArrayList killrights = new ArrayList();
			for (int i = 0; i < KillRightNodes.Count; i++) {
				if (KillRightNodes [i] == null)
					continue;
				killrights.Add(KillRightNodes [i].id);
			}
			map ["KillRightNodes"] = killrights;

			return map;
		}

		public override void DeserializeChildren(Hashtable map)
		{
			ArrayList children = MapEx.getList(map, "ChildrenLeft");
			Node node = null;
			for (int i = 0; i < children.Count; i++) {
				int id = int.Parse(children [i].ToString());
				node = behaviorTree.getNodeByID(id);
				ConnectLeftChild(node);
			}

			children = MapEx.getList(map, "ChildrenRight");
			for (int i = 0; i < children.Count; i++) {
				int id = int.Parse(children [i].ToString());
				node = behaviorTree.getNodeByID(id);
				ConnectRightChild(node);
			}

			children = MapEx.getList(map, "KillLeftNodes");
			for (int i = 0; i < children.Count; i++) {
				int id = int.Parse(children [i].ToString());
				node = behaviorTree.getNodeByID(id);
				ConnectKillLeftChild(node);
			}

			children = MapEx.getList(map, "KillRightNodes");
			for (int i = 0; i < children.Count; i++) {
				int id = int.Parse(children [i].ToString());
				node = behaviorTree.getNodeByID(id);
				ConnectKillRightChild(node);
			}
		}
	}
}
