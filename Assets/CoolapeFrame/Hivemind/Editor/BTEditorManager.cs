using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using Coolape;

namespace Hivemind
{

	/* 
	 * This manager is created by the BTInspector, the custom inspector for BehaviorTree assets.
	 * Every time the inspector receives the OnEnable message, a fresh manager is created, and
	 * destroyed when the inspector receives the OnDisable message.
	 * 
	 * Its main responsibility is to serve as the sole route for BehaviorTree manipulation.
	 * 
	 * It achieves that by exposing a selected BehaviorTree (determined by the current BehaviorTree
	 * being inspected in BTInspector) to the BTEditorWindow
	 * 
	 * BTEditorWindow manages its own sub systems to provide editing functionality, and forwards
	 * all actual manipulations to this manager.
	 * 
	 * Creation of new BehaviorTrees are also handled here.
	 */

	public class BTEditorManager : ScriptableObject
	{

		public Editor btInspector;
		public Editor nodeInspector;
		public BehaviorTreeAgent inspectedAgent;
		public BTEditorWindow editorWindow;

		public Node selectedNode;

		public BehaviorTree _behaviorTree;

		public BehaviorTree behaviorTree {
			get { return _behaviorTree; }
			set {
				_behaviorTree = value;
				if (_behaviorTree != null) {
					_behaviorTree.nodeWillTick = OnNodeWillTick;
					_behaviorTree.nodeDidTick = OnNodeDidTick;
				}
			}
		}

		public BTAsset btAsset;

		public void OnNodeWillTick (Node node)
		{
			if (inspectedAgent.debugMode) {
				Debug.Log (node.GetType ().Name);
			}
		}

		public void OnNodeDidTick (Node node, Status result)
		{
			if (editorWindow != null)
				editorWindow.Repaint ();
		}

		public static BTEditorManager Manager { get; private set; }

		public static BTEditorManager CreateInstance (BehaviorTree bt, BTAsset asset)
		{
			if (Manager == null) {
				Manager = (BTEditorManager)ScriptableObject.CreateInstance (typeof(BTEditorManager));
				Manager.behaviorTree = bt;
				Manager.btAsset = asset;
			}
			return Manager;
		}

		// Lifecycle

		public void OnEnable ()
		{
			hideFlags = HideFlags.HideAndDontSave;
		}

		public void OnDestroy ()
		{
			Manager = null;
			DestroyImmediate (behaviorTree);
		}

		// Asset management ------------------------------------------------------------------------------------------------------------------------------------

		[MenuItem ("Coolape/New Behavior Tree", false, 3)]
		static void CreateNewBehaviorTree (MenuCommand menuCommand)
		{
//			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
//			if (path == "")
//				path = "Assets";
//			else if (Path.GetExtension(path) != "")
//				path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");


			string basePath = PStr.b ().a ("Assets/").a (CLPathCfg.self.basePath).a ("/xRes/carbonEvent/").e ();
			string fullPath = EditorUtility.SaveFilePanel ("Save Carbon Event", basePath, "New Behavior Tree", "asset");
			if (string.IsNullOrEmpty (fullPath)) {
				return;
			}
//			fullPath = AssetDatabase.GenerateUniqueAssetPath (fullPath);
			fullPath = fullPath.Substring(fullPath.IndexOf("/Assets/") + 1);
			BehaviorTree bt = ScriptableObject.CreateInstance<BehaviorTree> ();
			Root root = ScriptableObject.CreateInstance<Root> ();
			root.editorPosition = new Vector2 (0, 0);
			bt.SetRoot (root);
			BTAsset btAsset = ScriptableObject.CreateInstance<BTAsset> ();
			btAsset.Serialize (bt);

			AssetDatabase.CreateAsset (btAsset, fullPath);
			AssetDatabase.Refresh ();
			EditorUtility.FocusProjectWindow ();
			Selection.activeObject = btAsset;
		}

		public void Dirty ()
		{
			if (editorWindow != null)
				editorWindow.Repaint ();
			btAsset.Serialize (behaviorTree);
			EditorUtility.SetDirty (btAsset);
		}

		// Behavior Tree manipulation --------------------------------------------------------------------------------------------------------------------------

		public void Add (Node parent, Vector2 position, string nodeType)
		{
			
			Node node = null;

			switch (nodeType) {
			case "Action":
				node = behaviorTree.CreateNode<NodeAction> ();
				break;
			case "Branch":
				node = behaviorTree.CreateNode<NodeBranch> ();
				break;
			case "Together":
				node = behaviorTree.CreateNode<NodeTogether> ();
				break;
//			case "Inverter":
//				node = behaviorTree.CreateNode<Inverter>();
//				break;
//			case "Parallel":
//				node = behaviorTree.CreateNode<Parallel>();
//				break;
//			case "RandomSelector":
//				node = behaviorTree.CreateNode<RandomSelector>();
//				break;
//			case "Repeater":
//				node = behaviorTree.CreateNode<Repeater>();
//				break;
//			case "Selector":
//				node = behaviorTree.CreateNode<Selector>();
//				break;
//			case "Sequence":
//				node = behaviorTree.CreateNode<Sequence>();
//				break;
//			case "Succeeder":
//				node = behaviorTree.CreateNode<Succeeder>();
//				break;
//			case "UntilSucceed":
//				node = behaviorTree.CreateNode<UntilSucceed>();
//				break;
			}

			behaviorTree.nodes.Add (node);
			if (parent != null && parent.CanConnectChild) {
				if (parent.ChildCount > 0) {
					Node lastSibling = parent.Children [parent.ChildCount - 1];
					node.editorPosition = lastSibling.editorPosition + new Vector2 (GridRenderer.step.x * 10, 0);
				} else {
					node.editorPosition = new Vector2 (parent.editorPosition.x, parent.editorPosition.y + GridRenderer.step.y * 10);
				}
				parent.ConnectChild (node);
//				SortChildren(parent);
			} else {
				float xOffset = position.x % GridRenderer.step.x;
				float yOffset = position.y % GridRenderer.step.y;
				float xoffset1 = editorWindow.view.scrollPoint.x;
				float yoffset1 = editorWindow.view.scrollPoint.y;
				node.editorPosition = new Vector2 (position.x - xOffset +xoffset1, position.y - yOffset+yoffset1);
			}
			Dirty ();

			// Select the newly added node
			if (editorWindow != null)
				editorWindow.view.SelectNode (node);
		}

		public void Connect (Node parent, Node child)
		{
			if (parent == child)
				return;
			if (parent.CanConnectChild) {
				parent.ConnectChild (child);
//				SortChildren(parent);
				Dirty ();
			} else {
				Debug.LogWarning (string.Format ("{0} can't accept child {1}", parent, child));
			}
		}

		public void ConnectKill (Node parent, Node child)
		{
			if (parent == child)
				return;
			parent.ConnectKillChild (child);
			Dirty ();
		}

		public void ConnectLeft (NodeBranch parent, Node child)
		{
			if (parent == child)
				return;
			if (parent.CanConnectChild) {
				parent.ConnectLeftChild (child);
//				SortChildren(parent);
				Dirty ();
			} else {
				Debug.LogWarning (string.Format ("{0} can't accept child {1}", parent, child));
			}
		}

		public void ConnectLeftKill (NodeBranch parent, Node child)
		{
			if (parent == child)
				return;
			parent.ConnectKillLeftChild (child);
			Dirty ();
		}

		public void ConnectRight (NodeBranch parent, Node child)
		{
			if (parent == child)
				return;
			if (parent.CanConnectChild) {
				parent.ConnectRightChild (child);
//				SortChildren(parent);
				Dirty ();
			} else {
				Debug.LogWarning (string.Format ("{0} can't accept child {1}", parent, child));
			}
		}

		public void ConnectRightKill (NodeBranch parent, Node child)
		{
			if (parent == child)
				return;
			parent.ConnectKillRightChild (child);
			Dirty ();
		}

		public void Unparent (Node node)
		{
			node.Unparent ();
			Dirty ();
		}

		public void DisconnectAll (Node node)
		{
			node.Disconnect ();
			Dirty ();
		}

		public void Delete (Node node)
		{
			node.Disconnect ();
			behaviorTree.nodes.Remove (node);
			DestroyImmediate (node, true);
			Dirty ();
		}

		public void SetEditorPosition (Node node, Vector2 position)
		{
			node.editorPosition = position;
//			SortChildren(node.parent);
			Dirty ();
		}

		//		private void SortChildren(Node parent) {
		//			Composite parentComposite = parent as Composite;
		//			if (parentComposite != null)
		//				parentComposite.SortChildren();
		//		}
	}

}