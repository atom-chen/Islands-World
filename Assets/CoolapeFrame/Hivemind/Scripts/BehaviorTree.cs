using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Best practices for serialization:
 * - Don't use the `new` constructor
 * - Instead use ScriptableObject.CreateInstance()
 * - For initialization, use OnEnable() instead of the constructor
 * 
 * Unity calls the constructor, deserializes the data (populating the object) and THEN calls OnEnable(),
 * so the data is guaranteed to be there in this method.
 * 
 */

namespace Hivemind
{

	public enum Status
	{
		Success,
		Failure,
		Running,
		Error
	}

	public class Context
	{
		private Dictionary<string, object> context = new Dictionary<string, object>();

		public Dictionary<string, object> All {
			get { return context; }
		}

		public bool ContainsKey(string key)
		{
			return context.ContainsKey(key);
		}

		public T Get<T>(string key)
		{
			if (!context.ContainsKey(key)) {
				throw new System.MissingMemberException(string.Format("Key {0} not found in the current context", key));
			}
			T value = (T)context [key];
			return value;
		}

		public T Get<T>(string key, T defaultValue)
		{
			if (!context.ContainsKey(key)) {
				Set<T>(key, defaultValue);
				return defaultValue;
			}
			T value = (T)context [key];
			return value;
		}

		public void Set<T>(string key, T value)
		{
			context [key] = value;
		}

		public void Unset(string key)
		{
			context.Remove(key);
		}
	}

	[System.Serializable]
	public class BehaviorTree : ScriptableObject
	{

		public static string[] NodeTypes = {
			"Action",					//动作
			"Branch",		 			//两分支
			"Together",				// 汇集
//			"Inverter", 
//			"Parallel",
//			"RandomSelector", 
//			"Repeater", 
//			"Selector",
//			"Sequence", 
//			"Succeeder", 
//			"UntilSucceed"
		};

		public int nodeIndex = 0;
		public Root rootNode;
		public List<Node> nodes = new List<Node>();

		public bool debugMode = false;
		public Node currentNode = null;

		public int TotalTicks { get; private set; }

		public delegate void NodeWillTick(Node node);

		public delegate void NodeDidTick(Node node,Status result);

		public NodeWillTick nodeWillTick;
		public NodeDidTick nodeDidTick;

		public Node getNodeByID(int id)
		{
			for (int i = 0; i < nodes.Count; i++) {
				if (nodes [i].id == id) {
					return nodes [i];
				}
			}
			return null;
		}

		public void SetRoot(Root root)
		{
			rootNode = root;
			nodes.Add(root);
			root.behaviorTree = this;
		}

		public T CreateNode<T>() where T : Node
		{
			T node = (T)ScriptableObject.CreateInstance<T>();
			if (rootNode != null) {
				rootNode.maxIndex++;
				node.index = rootNode.begainIndex + rootNode.maxIndex;
			}
			node.behaviorTree = this;
			return node;
		}
		
		// Lifecycle
		public void OnEnable()
		{
			hideFlags = HideFlags.HideAndDontSave;
		}

		public void OnDestroy()
		{
			foreach (Node node in nodes) {
				DestroyImmediate(node);
			}
			nodeIndex = 0;
		}

		public Status Tick(GameObject agent, Context context)
		{
			BehaviorTreeAgent btAgent = agent.GetComponent<BehaviorTreeAgent>();
			if (btAgent) {
				debugMode = btAgent.debugMode;
			}

			TotalTicks++;

			Status result = rootNode.Tick(agent, context);
			rootNode.lastStatus = result;
			rootNode.lastTick = TotalTicks;
			return result;
		}

		public Status Tick(Node node, GameObject agent, Context context)
		{

			if (nodeWillTick != null && node != currentNode)
				nodeWillTick(node);

			Status result = node.Tick(agent, context);

			if (nodeDidTick != null)
				nodeDidTick(node, result);

			currentNode = node;
			node.lastStatus = result;
			node.lastTick = TotalTicks;
			return result;
		}
	}
}