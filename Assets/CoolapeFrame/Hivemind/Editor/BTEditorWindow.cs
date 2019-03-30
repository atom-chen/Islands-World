using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using Coolape;
using System.Reflection;
using System;
using System.Linq;

namespace Hivemind
{

	public class MenuAction
	{

		public string nodeType;
		public Vector2 position;
		public Node node;

		public MenuAction (Node nodeVal)
		{
			node = nodeVal;
		}

		public MenuAction (Node nodeVal, Vector2 positionVal, string nodeTypeVal)
		{
			node = nodeVal;
			position = positionVal;
			nodeType = nodeTypeVal;
		}
	}

	public class BTEditorWindow : EditorWindow
	{

		public View view;
		private GUIStyle _noSelectionStyle;
		private bool _debugMode;

		[MenuItem ("Window/Behavior Tree Editor")]
		public static void ShowWindow ()
		{
			BTEditorWindow editor = EditorWindow.GetWindow<BTEditorWindow> ();
			editor.minSize = new Vector2 (480, 360);
			editor.title = "Behavior Tree";
		}

		void OnSelectionChange ()
		{
			Repaint ();
		}

		void OnEnable ()
		{
			_noSelectionStyle = new GUIStyle ();
			_noSelectionStyle.fontSize = 24;
			_noSelectionStyle.alignment = TextAnchor.MiddleCenter;
			if (BTEditorManager.Manager != null)
				BTEditorManager.Manager.editorWindow = this;
		}

		void OnDisable ()
		{
			if (BTEditorManager.Manager)
				BTEditorManager.Manager.editorWindow = null;
		}

		void OnDestroy ()
		{
			if (BTEditorManager.Manager)
				BTEditorManager.Manager.editorWindow = null;
		}

		void OnGUI ()
		{

			if (BTEditorManager.Manager != null && BTEditorManager.Manager.behaviorTree != null) {

				BTEditorManager.Manager.editorWindow = this;

				if (view == null)
					view = new View (this);

				if (view.nodeInspector != null)
					view.nodeInspector.OnInspectorGUI ();

				bool viewNeedsRepaint = view.Draw (position);
				if (viewNeedsRepaint)
					Repaint ();
				
				view.ResizeCanvas ();

			} else {
				GUI.Label (new Rect (0, 0, position.width, position.height), "No Behavior Tree selected", _noSelectionStyle);
			}

		}

		public void ShowContextMenu (Vector2 point, Node node)
		{

//			if (Application.isPlaying) {
//				return;
//			}

			var menu = new GenericMenu ();

			if (node == null || node.CanConnectChild) {

				// Add new node
				string addMsg = (node == null ? "Add" : "Add Child") + "/";

				// List all available node subclasses
				int length = BehaviorTree.NodeTypes.Length;
				for (int i = 0; i < length; i++) {
					menu.AddItem (
						new GUIContent (addMsg + BehaviorTree.NodeTypes [i]),
						false,
						Add,
						new MenuAction (node, point, BehaviorTree.NodeTypes [i])
					);
				}


			} else {
				menu.AddDisabledItem (new GUIContent ("Add"));
			}
			int menuType = 0;
			int selectedNodesCount = view.selectedNodes.Count;
			if (selectedNodesCount == 0) {
				menuType = 0;
			} else if (selectedNodesCount == 1) {
				menuType = 0;
			} else if (selectedNodesCount == 2) {
				menuType = 2;
			} else {
				menuType = 3;
			}

			if (menuType == 2) {
				menu.AddItem (new GUIContent ("Disconnect From Selected Nodes"), false, DisconnectSelectedNodes, new MenuAction (node));
				menu.AddSeparator ("");
				menu.AddItem (new GUIContent ("Delete Selected Nodes"), false, DeleteSelectNodes, new MenuAction (node));
			} else if (menuType == 3) {
				//empty
				menu.AddItem (new GUIContent ("Delete Selected Nodes"), false, DeleteSelectNodes, new MenuAction (node));
			} else {
				if (node == null) {
					menu.AddSeparator ("");
					menu.AddItem (new GUIContent ("Save"), false, Save, null);
					menu.AddItem (new GUIContent ("Refresh Nodes Index"), false, RefreshNodesIndex, null);
					menu.AddItem (new GUIContent ("Genrate Node State"), false, GenerateNodeState, null);
					menu.AddItem (new GUIContent ("Export Data"), false, exportData, null);
				}

				menu.AddSeparator ("");

				// Node actions
				if (node != null) {

					// Connect/Disconnect Parent
					if (!(node is Root)) {
						menu.AddItem (new GUIContent ("Disconnect from All Parent"), false, Unparent, new MenuAction (node));
						menu.AddItem (new GUIContent ("Disconnect All"), false, DisconnectAll, new MenuAction (node));
						menu.AddItem (new GUIContent ("Connect to Parent"), false, ConnectParent, new MenuAction (node));
					}

					menu.AddSeparator ("");

					// Connect Child
					if (node.CanConnectChild) {
						if (node is NodeBranch) {
							menu.AddItem (new GUIContent ("Connect to Left Child"), false, ConnectLeftChild, new MenuAction (node));
							menu.AddItem (new GUIContent ("Connect to Left Kill  Child"), false, ConnectLeftKillChild, new MenuAction (node));
							menu.AddItem (new GUIContent ("Connect to Right Child"), false, ConnectRightChild, new MenuAction (node));
							menu.AddItem (new GUIContent ("Connect to Right Kill  Child"), false, ConnectRightKillChild, new MenuAction (node));
						} else {
							menu.AddItem (new GUIContent ("Connect to Child"), false, ConnectChild, new MenuAction (node));
							menu.AddItem (new GUIContent ("Connect to Kill Child"), false, ConnectKillChild, new MenuAction (node));
						}
					} else {
						menu.AddDisabledItem (new GUIContent ("Connect to Child"));
					}

					menu.AddSeparator ("");
				
					// Deleting
					if (node is Root)
						menu.AddDisabledItem (new GUIContent ("Delete"));
					else
						menu.AddItem (new GUIContent ("Delete"), false, Delete, new MenuAction (node));
				}
			}

			menu.DropDown (new Rect (point.x, point.y, 0, 0));
		}

		// Context Menu actions

		public void Add (object userData)
		{
			MenuAction menuAction = userData as MenuAction;
			BTEditorManager.Manager.Add (menuAction.node, menuAction.position, menuAction.nodeType);
			view.ResizeCanvas ();
			Repaint ();
		}

		public void Unparent (object userData)
		{
			MenuAction menuAction = userData as MenuAction;
			BTEditorManager.Manager.Unparent (menuAction.node);
			Repaint ();
		}

		public void DisconnectAll (object userData)
		{
			MenuAction menuAction = userData as MenuAction;
			BTEditorManager.Manager.DisconnectAll (menuAction.node);
			Repaint ();
		}

		public void ConnectParent (object userData)
		{
			MenuAction menuAction = userData as MenuAction;
			view.ConnectParent (menuAction.node);
		}

		public void ConnectChild (object userData)
		{
			MenuAction menuAction = userData as MenuAction;
			view.ConnectChild (menuAction.node);
		}

		public void ConnectKillChild (object userData)
		{
			MenuAction menuAction = userData as MenuAction;
			view.ConnectKillChild (menuAction.node);
		}

		public void ConnectLeftChild (object userData)
		{
			MenuAction menuAction = userData as MenuAction;
			view.ConnectLeftChild (menuAction.node);
		}

		public void ConnectLeftKillChild (object userData)
		{
			MenuAction menuAction = userData as MenuAction;
			view.ConnectLeftKillChild (menuAction.node);
		}

		public void ConnectRightChild (object userData)
		{
			MenuAction menuAction = userData as MenuAction;
			view.ConnectRightChild (menuAction.node);
		}

		public void ConnectRightKillChild (object userData)
		{
			MenuAction menuAction = userData as MenuAction;
			view.ConnectRightKillChild (menuAction.node);
		}

		public void Delete (object userData)
		{
			MenuAction menuAction = userData as MenuAction;
			BTEditorManager.Manager.Delete (menuAction.node);
			Repaint ();
		}

		public void Save (object userData)
		{
			if (BTEditorManager.Manager != null
			    && BTEditorManager.Manager.btAsset != null
			    && BTEditorManager.Manager.behaviorTree != null) {
				BTEditorManager.Manager.btAsset.Serialize (BTEditorManager.Manager.behaviorTree);
				EditorUtility.SetDirty (BTEditorManager.Manager.btAsset);
			}
			AssetDatabase.Refresh ();
			AssetDatabase.SaveAssets ();
			Debug.Log ("Save");
		}

		public void DisconnectSelectedNodes (object userData)
		{
			if (view.selectedNodes.Count != 2)
				return;
			Node node1 = view.selectedNodes [0];
			Node node2 = view.selectedNodes [1];
			node1.DisconnectChild (node2);
			node1.DisconnectKillChild (node2);
			node1.Unparent (node2);
			node2.DisconnectChild (node1);
			node2.DisconnectKillChild (node1);
			node2.Unparent (node1);
			if (node1 is NodeBranch) {
				((NodeBranch)node1).DisconnectLeftChild (node2);
				((NodeBranch)node1).DisconnectRightChild (node2);
				((NodeBranch)node1).DisconnectKillLeftChild (node2);
				((NodeBranch)node1).DisconnectKillRightChild (node2);
			}
			if (node2 is NodeBranch) {
				((NodeBranch)node2).DisconnectLeftChild (node1);
				((NodeBranch)node2).DisconnectRightChild (node1);
				((NodeBranch)node2).DisconnectKillLeftChild (node1);
				((NodeBranch)node2).DisconnectKillRightChild (node1);
			}
			Repaint ();
//			Save (userData);
		}

		public void DeleteSelectNodes (object userData)
		{
			for (int i = view.selectedNodes.Count - 1; i >= 0; i--) {
				BTEditorManager.Manager.Delete (view.selectedNodes [i]);
			}
			view.selectedNodes.Clear ();
			Repaint ();
//			Save (userData);
		}

		public void RefreshNodesIndex (object userData)
		{
			Root node = BTEditorManager.Manager.behaviorTree.rootNode;
			node.index = 0;
			node.maxIndex = 0;
			foreach (Node n in node.behaviorTree.nodes) {
				if (n != node) {
					node.maxIndex++;
					n.index = node.begainIndex + node.maxIndex;
				}
			}
//			Save (userData);
		}

		public void GenerateNodeState (object userData)
		{
			List<Node> nodes = BTEditorManager.Manager.behaviorTree.nodes;
			Hashtable procedMap = new Hashtable ();
			for (int i = 0; i < nodes.Count; i++) {
				doGenerateNodeState (nodes [i], ref procedMap);
			}
		}

		public void doGenerateNodeState (Node node, ref Hashtable map)
		{
			if (MapEx.getBool (map, node) || node == node.behaviorTree.rootNode) {
				return;
			}
			map [node] = true;

			int mainType = 14;
			string pubStr = "";
			if (node.behaviorTree.rootNode.attr != null) {
				pubStr = mainType + "/" + ((MyRoot)(node.behaviorTree.rootNode.attr)).fubenID + "/";
			} else {
				pubStr = mainType + "/" + 0 + "/";
			}

			//Condition
			if (node.parents != null && node.parents.Count > 0) {
				node.condition = "{";
				if ((node.parents.Count == 1 && (node.parents [0]) is Root)) {
					node.condition += "{\"" + pubStr + node.index + "/" + 0 + "\"}";
				} else if (node.parents.Contains (node.behaviorTree.rootNode)) {
					node.condition += "{\"" + pubStr + node.index + "/" + 0 + "\",\"" + pubStr + node.index + "/" + 1 + "\"}";
				} else {
					node.condition += "{\"" + pubStr + node.index + "/" + 1 + "\"}";
				}

				if (node is NodeTogether) {
					Node parent = null;
					for (int i = 0; i < node.parents.Count; i++) {
						parent = node.parents [i];
						if (parent is  Root) {
						} else if (parent is NodeBranch) {
							if (((NodeBranch)parent).inLeft (node)) {
								node.condition += ",{\"" + pubStr + (-node.parents [i].index) + "/" + 1 + "\"}";
							} 
							if (((NodeBranch)parent).inRight (node)) {
								node.condition += ",{\"" + pubStr + (-node.parents [i].index) + "/" + 2 + "\"}";
							}
						} else {
							node.condition += ",{\"" + pubStr + (-node.parents [i].index) + "/" + 1 + "\"}";
						}
					}
				}
				node.condition += "}";
			} else {
				node.condition = "{{\"" + pubStr + node.index + "/" + 0 + "\"}}";
			}
			//result
			if (node is NodeBranch) {
				NodeBranch branch = node as NodeBranch;
				node.result1 = getResultWithSubNodes (branch, branch.ChildrenLeft, branch.KillLeftNodes);
				node.result2 = getResultWithSubNodes (branch, branch.ChildrenRight, branch.KillRightNodes);
			} else {
				node.result1 = getResultWithSubNodes (node, node.Children, node.KillNodes);
				node.result2 = node.result1;
			}
		}

		public string getNodeResult (Node node, Node child)
		{
			string ret = "";
			if (node is NodeBranch) {
				if (((NodeBranch)node).inLeft (child)) {
					if (child is NodeTogether) {
						ret = -node.index + "/" + 1;
					} else {
						ret = child.index + "/" + 1;
					}
				}

				if (((NodeBranch)node).inRight (child)) {
					if (child is NodeTogether) {
						ret = -node.index + "/" + 2;
					} else {
						ret = child.index + "/" + 1;
					}
				}
			} else {
				if (child is NodeTogether) {
					ret = -node.index + "/" + 1;
				} else {
					ret = child.index + "/" + 1;
				}
			}
			return ret;
		}

		public string getResultWithSubNodes (Node node, List<Node>children, List<Node>killChildren)
		{
			int mainType = 14;
			string pubStr = "";
			if (node.behaviorTree.rootNode.attr != null) {
				pubStr = mainType + "/" + ((MyRoot)(node.behaviorTree.rootNode.attr)).fubenID + "/";
			} else {
				pubStr = mainType + "/" + 0 + "/";
			}
			string result = "{";
			if (!node.canMultTimes) {
				result = result + "\"" + pubStr + node.index + "/" + 9 + "\"";
			}
			if ((node is NodeAction) && ((((NodeAction)node).action) is MyAction)) {
				int triggerType = ((MyAction)(((NodeAction)node).action)).triggerType;
				if (node.canMultTimes && triggerType == 1) {
					result = result + "\"" + pubStr + node.index + "/" + 9 + "\"";
					Debug.LogError ("Node index=["+ node.index +"] have logic error, beacause it can multTimes and it triggered auto");
				}
			}

			Node child = null;
			for (int i = 0; i < children.Count; i++) {
				child = children [i];
				if (result.Length == 1) {
					result = result + "\"" + pubStr + children [i].index + "/" + 1 + "\"";
				} else {
					result = result + ",\"" + pubStr + children [i].index + "/" + 1 + "\"";
				}
				if (children [i] is NodeTogether) {
					result = result + ",\"" + pubStr + getNodeResult (node, children [i]) + "\"";
				}
			}

			for (int i = 0; i < killChildren.Count; i++) {
				if (killChildren [i] == null)
					continue;
				if (result.Length == 1) {
					result = result + "\"" + pubStr + killChildren [i].index + "/" + 9 + "\"";
				} else {
					result = result + ",\"" + pubStr + killChildren [i].index + "/" + 9 + "\"";
				}
				result = result + ",\"" + pubStr + (-killChildren [i].index) + "/" + 9 + "\"";
			}
			result = result + "}";
			if (result.Equals ("{}")) {
				result = "";
			}
			return result;
		}

		//会调用加了【ExportHiveData】的函数
		public void exportData (object userData)
		{
			GenerateNodeState (userData);
			List<Node> nodes = BTEditorManager.Manager.behaviorTree.nodes;
			callCustomExportHiveData (nodes);

			EditorUtility.DisplayDialog ("Alert", "Finished!", "Okay");
		}

		static IEnumerable<MethodInfo> exportHiveDataMethodList = (from type in XLua.Utils.GetAllTypes ()
		                                                           from method in type.GetMethods (BindingFlags.Static | BindingFlags.Public)
		                                                           where method.IsDefined (typeof(ExportHiveDataAttribute), false)
		                                                           select method);

		static void callCustomExportHiveData (List<Node> nodes)
		{
			if (exportHiveDataMethodList.Count<MethodInfo> () == 0) {
				Debug.LogError ("There is no customer export data function");
				return;
			}
			foreach (var method in exportHiveDataMethodList) {
				method.Invoke (null, new object[] { nodes });
			}
		}
	}

	public class ExportHiveDataAttribute : Attribute
	{
	}
}
