
// Root ------------------------------------------------------------------------------------------------------------------------------------------------------
using Hivemind;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Coolape;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace Hivemind
{
	[System.Serializable]
	public class Root : Node
	{
		public int begainIndex = 0;
		public int maxIndex = 0;

		public ActionBase attr;
		#if UNITY_EDITOR
		MonoScript _monoScript;
		public MonoScript monoScript{
			get {
				return _monoScript;
			}
			set {
				if(_monoScript != value && value != null) {
					Type tp = value.GetClass ();
					attr = Activator.CreateInstance (tp) as ActionBase;
				}
				_monoScript = value;
			}
		}
		#endif

		public virtual void OnEnable() {
			#if UNITY_EDITOR
			if (monoScript == null) {
				monoScript = AssetDatabase.LoadAssetAtPath ("Assets/CoolapeFrame/Hivemind/Test/MyRoot.cs", typeof(MonoScript)) as MonoScript;
			}
			#endif
		}

		// Child connections
		[SerializeField]
		Node _child;

		public override void ConnectChild (Node child)
		{
			if (_child == null) {
				_child = child;
				child.setParent(this);
			} else {
				throw new System.InvalidOperationException (string.Format ("{0} already has a connected child, cannot connect {1}", this, child));
			}
		}

		public override void DisconnectChild (Node child)
		{
			if (_child == child) {
				_child = null;
				child.Unparent(this);
			} else {
				throw new System.InvalidOperationException (string.Format ("{0} is not a child of {1}", child, this));
			}
		}

		public override List<Node> Children {
			get {
				List<Node> nodeList = new List<Node> ();
				nodeList.Add (_child);
				return nodeList;
			}
		}

		public override int ChildCount {
			get { return _child != null ? 1 : 0; }
		}

		public override bool CanConnectChild {
			get { return _child == null; }
		}

		public override bool ContainsChild (Node child)
		{
			return _child == child;
		}

		public override Hashtable Serialize ()
		{
			Hashtable map = base.Serialize ();
			#if UNITY_EDITOR
			map ["begainIndex"] = begainIndex;
			map ["maxIndex"] = maxIndex;

			if (attr != null) {
				map ["attr"] = attr.Serialize ();
			}

			if (monoScript != null) {
				string actionClass = AssetDatabase.GetAssetPath (monoScript);
				map ["actionClass"] = actionClass;
			}

			#endif
			return map;
		}

		public override void Deserialize (Hashtable map)
		{
			#if UNITY_EDITOR
			base.Deserialize (map);
			begainIndex = MapEx.getInt (map, "begainIndex");
			maxIndex = MapEx.getInt (map, "maxIndex");

			string actionClass = MapEx.getString (map, "actionClass");
			if (string.IsNullOrEmpty (actionClass)) {
				monoScript = null;
			} else {
				monoScript = AssetDatabase.LoadAssetAtPath (actionClass, typeof(MonoScript)) as MonoScript;
				if (monoScript != null) {
					Type tp = monoScript.GetClass ();
					attr = Activator.CreateInstance (tp) as ActionBase;
					if (attr != null) {
						attr.Deserialize (MapEx.getMap (map, "attr"));
					}
				}
			}
			#endif
		}

		// Runtime

		public override Status Tick (GameObject agent, Context context)
		{
			Status result = Status.Error;
			if (_child != null) {
				result = behaviorTree.Tick (_child, agent, context);
			}
			return result;
		}
	}
}
