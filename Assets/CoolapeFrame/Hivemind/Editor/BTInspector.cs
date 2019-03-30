using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Hivemind
{

	public class HivemindInspector : Editor
	{

		private static GUIStyle _titleStyle;
		private static GUIStyle _subtitleStyle;

		public static GUIStyle TitleStyle {
			get {
				if (_titleStyle == null) {
					_titleStyle = new GUIStyle ();
					_titleStyle.fontSize = 18;
				}
				return _titleStyle;
			}
		}

		public static GUIStyle SubtitleStyle {
			get {
				if (_subtitleStyle == null) {
					_subtitleStyle = new GUIStyle ();
					_subtitleStyle.fontSize = 15;
				}
				return _subtitleStyle;
			}
		}
	}

	[CustomEditor (typeof(BTAsset))]
	public class BTInspector : HivemindInspector
	{
		private BTEditorManager manager;

		public void OnEnable ()
		{
			BTAsset btAsset = (BTAsset)serializedObject.targetObject;
			BehaviorTree bt = btAsset.Deserialize ();
			manager = BTEditorManager.CreateInstance (bt, btAsset);
			manager.btInspector = this;
		}

		public void OnDisable ()
		{
			DestroyImmediate (manager);
		}

		public override void OnInspectorGUI ()
		{

			if (BTEditorManager.Manager.nodeInspector == null) {
				
				EditorGUILayout.LabelField ("Behavior Tree", TitleStyle);

				if (manager.behaviorTree.nodes.Count > 2)
					EditorGUILayout.LabelField (string.Format ("{0} nodes", manager.behaviorTree.nodes.Count - 1));
				else if (manager.behaviorTree.nodes.Count == 2)
					EditorGUILayout.LabelField ("Empty");
				else
					EditorGUILayout.LabelField ("1 node");

				EditorGUILayout.Space ();

				if (GUILayout.Button ("Show Behavior Tree editor")) {
					BTEditorWindow.ShowWindow ();
				}
				
			} else {
				manager.nodeInspector.OnInspectorGUI ();
			}
			
			if (GUI.changed) {
				manager.Dirty ();
			}
		}
		
	}

	[CustomEditor (typeof(Node), true)]
	public class BTNodeInspector : HivemindInspector
	{

		public override void OnInspectorGUI ()
		{

			Node node = (Node)serializedObject.targetObject;

			if (node is Root) {
				DrawInspector ((Root)node);
			} else if (node is NodeBranch) {
				DrawInspector ((NodeBranch)node);
			} else if (node is NodeAction) {
				DrawInspector ((NodeAction)node);
			}
//			else if (node is Selector) { DrawInspector ((Selector) node); }
//			else if (node is Sequence) { DrawInspector ((Sequence) node); }
//			else if (node is Parallel) { DrawInspector ((Parallel) node); }
//			else if (node is Repeater) { DrawInspector ((Repeater) node); }
//			else if (node is RandomSelector) { DrawInspector ((RandomSelector) node); }
//			else if (node is UntilSucceed) { DrawInspector ((UntilSucceed) node); }
//			else if (node is Inverter) { DrawInspector ((Inverter) node); }
//			else if (node is Succeeder) { DrawInspector ((Succeeder) node); }

			if (GUI.changed) {
				BTEditorManager.Manager.Dirty ();
			}
		}

		//		private int IndexOf (string[] array, string target)
		//		{
		//			int length = array.Length;
		//			for (var i = 0; i < length; i++) {
		//				if (array [i] == target)
		//					return i;
		//			}
		//			return 0;
		//		}


		public void DrawInspectorBase (Node node)
		{
			ECLEditorUtl.BeginContents ();
			{
				EditorGUILayout.LabelField ("State values");
				EditorGUILayout.TextField (new GUIContent ("Conditon"), node.condition);
				EditorGUILayout.TextField (new GUIContent ("Result1"), node.result1);
				EditorGUILayout.TextField (new GUIContent ("Result2"), node.result2);
			}
			ECLEditorUtl.EndContents ();
		}

		public void DrawInspector (Root node)
		{
			EditorGUILayout.LabelField (new GUIContent ("Root"), TitleStyle);
			EditorGUILayout.Space ();

			node.begainIndex = EditorGUILayout.IntField (new GUIContent ("Begain Index"), node.begainIndex);

			node.monoScript = (MonoScript)EditorGUILayout.ObjectField ("Root Script", node.monoScript, typeof(MonoScript), false);
			if (node.monoScript != null) {
				if (!node.monoScript.GetClass ().IsSubclassOf (typeof(ActionBase))) {
					EditorGUILayout.HelpBox (string.Format ("{0} is not a subclass of Hivemind.ActionBase", node.monoScript.GetClass ().ToString ()), MessageType.Warning);
					//					action.monoScript = null;
				} else {
				}
			}
			if (node.attr != null) {
				ECLEditorUtl.BeginContents ();
				{
					EditorGUILayout.LabelField ("Event Content");
					node.attr.DrawInspector (node);
				}
				ECLEditorUtl.EndContents ();
			}
			EditorGUILayout.Space ();

			if (GUILayout.Button ("Refresh Nodes Index")) {
				node.index = 0;
				node.maxIndex = 0;
				foreach (Node n in node.behaviorTree.nodes) {
					if (n != node) {
						node.maxIndex++;
						n.index = node.begainIndex + node.maxIndex;
					}
				}
			}
		}

		public void DrawInspector (NodeAction node)
		{
			EditorGUILayout.LabelField (new GUIContent ("Action"), TitleStyle);

			EditorGUILayout.Space ();
			node.desc = EditorGUILayout.TextField (new GUIContent ("Desc"), node.desc);
			node.canMultTimes = EditorGUILayout.Toggle("Run MultTimes", node.canMultTimes);

			// MonoScript selection field
			NodeAction action = (NodeAction)serializedObject.targetObject;
			action.monoScript = (MonoScript)EditorGUILayout.ObjectField ("Action Script", action.monoScript, typeof(MonoScript), false);
			if (action.monoScript != null) {
				if (!action.monoScript.GetClass ().IsSubclassOf (typeof(ActionBase))) {
					EditorGUILayout.HelpBox (string.Format ("{0} is not a subclass of Hivemind.ActionBase", action.monoScript.GetClass ().ToString ()), MessageType.Warning);
//					action.monoScript = null;
				} else {
				}
			}
			if (action.action != null) {
				ECLEditorUtl.BeginContents ();
				{
					EditorGUILayout.LabelField ("Event Content");
					action.action.DrawInspector (node);
				}
				ECLEditorUtl.EndContents ();
			}
			EditorGUILayout.Space ();

			DrawInspectorBase (node);

			serializedObject.ApplyModifiedProperties ();
		}

		//		private object DrawParamControl(System.Type type, string label, object value) {
		//			if (type == typeof(string)) {
		//				return EditorGUILayout.TextField(label, (string) value);
		//			}
		//			else if (type == typeof(float)) {
		//				return EditorGUILayout.FloatField(label, (float) value);
		//			}
		//			return null;
		//		}

		//		public void DrawInspector(Selector node) {
		//			EditorGUILayout.LabelField(new GUIContent("Selector"), TitleStyle);
		//			EditorGUILayout.Space ();
		//			node.rememberRunning = EditorGUILayout.Toggle("Remember running child", node.rememberRunning);
		//			string message = "The Selector node ticks its children sequentially from left to right, until one of them returns SUCCESS, RUNNING or ERROR, at which point it returns that state. If all children return the failure state, the priority also returns FAILURE.";
		//			EditorGUILayout.HelpBox(message, MessageType.Info);
		//			message = "If \"Remember running chikd\" is on, when a child returns RUNNING the Selector will remember that child, and in future ticks it will skip directly to that child until it returns something other than RUNNING.";
		//			EditorGUILayout.HelpBox(message, MessageType.Info);
		//		}
		//
		//		public void DrawInspector(Sequence node) {
		//			EditorGUILayout.LabelField(new GUIContent("Sequence"), TitleStyle);
		//			EditorGUILayout.Space ();
		//			node.rememberRunning = EditorGUILayout.Toggle("Remember running child", node.rememberRunning);
		//			string message = "The Sequence node ticks its children sequentially from left to right, until one of them returns FAILURE, RUNNING or ERROR, at which point the Sequence returns that state. If all children return the success state, the sequence also returns SUCCESS.";
		//			EditorGUILayout.HelpBox(message, MessageType.Info);
		//			message = "If \"Remember running child\" is on, when a child returns RUNNING the Sequence will remember that child, and in future ticks it will skip directly to that child until it returns something other than RUNNING.";
		//			EditorGUILayout.HelpBox(message, MessageType.Info);
		//		}
		//
		//		public void DrawInspector(Parallel node) {
		//			EditorGUILayout.LabelField(new GUIContent("Parallel"), TitleStyle);
		//			EditorGUILayout.Space ();
		//
		//
		//			node.strategy = (Parallel.ResolutionStrategy) EditorGUILayout.EnumPopup("Return Strategy", node.strategy);
		//			string message = "The parallel node ticks all children sequentially from left to right, regardless of their return states. It returns SUCCESS if the number of succeeding children is larger than a local constant S, FAILURE if the number of failing children is larger than a local constant F or RUNNING otherwise.";
		//			EditorGUILayout.HelpBox(message, MessageType.Info);
		//			EditorGUILayout.HelpBox("Not yet implemented!", MessageType.Error);
		//		}
		//
		//		public void DrawInspector(Repeater node) {
		//			EditorGUILayout.LabelField(new GUIContent("Repeater"), TitleStyle);
		//			EditorGUILayout.Space ();
		//			string message = "Repeater decorator sends the tick signal to its child every time that its child returns a SUCCESS or FAILURE.";
		//			EditorGUILayout.HelpBox(message, MessageType.Info);
		//			EditorGUILayout.HelpBox("Not yet implemented!", MessageType.Error);
		//		}
		//
		//		public void DrawInspector(Inverter node) {
		//			EditorGUILayout.LabelField(new GUIContent("Inveter"), TitleStyle);
		//			EditorGUILayout.Space ();
		//			string message = "Like the NOT operator, the inverter decorator negates the result of its child node, i.e., SUCCESS state becomes FAILURE, and FAILURE becomes SUCCESS. RUNNING or ERROR states are returned as is.";
		//			EditorGUILayout.HelpBox(message, MessageType.Info);
		//			EditorGUILayout.HelpBox("Not yet implemented!", MessageType.Error);
		//		}
		//
		//		public void DrawInspector(Succeeder node) {
		//			EditorGUILayout.LabelField(new GUIContent("Succeeder"), TitleStyle);
		//			EditorGUILayout.Space ();
		//			string message = "Succeeder always returns a SUCCESS, no matter what its child returns.";
		//			EditorGUILayout.HelpBox(message, MessageType.Info);
		//			EditorGUILayout.HelpBox("Not yet implemented!", MessageType.Error);
		//		}
		//
		//		public void DrawInspector(UntilSucceed node) {
		//			EditorGUILayout.LabelField(new GUIContent("Repeat Until Succeed"), TitleStyle);
		//			EditorGUILayout.Space ();
		//			string message = "This decorator keeps calling its child until the child returns a SUCCESS value. When this happen, the decorator return a SUCCESS state.";
		//			EditorGUILayout.HelpBox(message, MessageType.Info);
		//			EditorGUILayout.HelpBox("Not yet implemented!", MessageType.Error);
		//		}
		//
		//		public void DrawInspector(RandomSelector node) {
		//			EditorGUILayout.LabelField(new GUIContent("Random Selector"), TitleStyle);
		//			EditorGUILayout.Space ();
		//			if (node.ChildCount > 0) {
		//				float chance = 100f / node.ChildCount;
		//				EditorGUILayout.LabelField(new GUIContent("Each child has a " + chance.ToString ("F1") + "% chance of being selected."), SubtitleStyle);
		//			}
		//			string message = "The Random Selector sends the tick signal to one of its children, selected at random, and returns the state returned by that child.";
		//			EditorGUILayout.HelpBox(message, MessageType.Info);
		//			EditorGUILayout.HelpBox("Not yet implemented!", MessageType.Error);
		//		}

		public void DrawError ()
		{
			EditorGUILayout.LabelField ("Selected node is invalid");
		}

	}

}