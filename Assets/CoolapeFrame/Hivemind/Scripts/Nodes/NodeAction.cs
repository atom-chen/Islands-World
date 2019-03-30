using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hivemind;

#if UNITY_EDITOR
using UnityEditor;
#endif
using Coolape;
using System;

namespace Hivemind
{
	public class NodeAction : Node
	{
		public ActionBase action;

		#if UNITY_EDITOR
		MonoScript _monoScript;
		public MonoScript monoScript{
			get {
				return _monoScript;
			}
			set {
				if(_monoScript != value && value != null) {
					Type tp = value.GetClass ();
					action = Activator.CreateInstance (tp) as ActionBase;
				}
				_monoScript = value;
			}
		}
		#endif

		public virtual void OnEnable() {
			#if UNITY_EDITOR
			if (monoScript == null) {
				monoScript = AssetDatabase.LoadAssetAtPath ("Assets/CoolapeFrame/Hivemind/Test/MyAction.cs", typeof(MonoScript)) as MonoScript;
			}
			#endif
		}

		public override Hashtable Serialize ()
		{
			Hashtable m = base.Serialize ();
			if (action != null) {
				m ["action"] = action.Serialize ();
			}
			#if UNITY_EDITOR
			if (monoScript != null) {
				string actionClass = AssetDatabase.GetAssetPath (monoScript);
				m ["actionClass"] = actionClass;
			}
			#endif
			return m;
		}

		public override void Deserialize (Hashtable map)
		{
			#if UNITY_EDITOR
			base.Deserialize (map);
			string actionClass = MapEx.getString (map, "actionClass");
			if (string.IsNullOrEmpty (actionClass)) {
				monoScript = null;
			} else {
				monoScript = AssetDatabase.LoadAssetAtPath (actionClass, typeof(MonoScript)) as MonoScript;
				if (monoScript != null) {
					Type tp = monoScript.GetClass ();
					action = Activator.CreateInstance (tp) as ActionBase;
					if (action != null) {
						action.Deserialize (MapEx.getMap (map, "action"));
					}
				}
			}
			#endif
		}
	}
}
