using UnityEditor;
using UnityEngine;
using System.Collections;
using Coolape;
using System.Collections.Generic;


[CustomEditor (typeof(CLRoleAction), true)]
public class CLRoleActionInspector :  Editor
{
	
	CLRoleAction roleAction;
	CLEffect effect;
	CLRoleAction.Action action = CLRoleAction.Action.idel;
	static List<int> pausePersent = new List<int> ();
	static int onePersent = 100;
	static int index = 0;

	void onActionCallback (params object[] args)
	{
		roleAction.pause ();
	}

	void onFinishActionCallback (params object[] args)
	{
		roleAction.regain ();
		roleAction.setAction (CLRoleAction.Action.idel, null);
	}

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		NGUIEditorTools.BeginContents ();
		{
			roleAction = (CLRoleAction)target;

			GUILayout.BeginHorizontal ();
			{
				action = (CLRoleAction.Action)EditorGUILayout.EnumPopup ("Action", action);
				if (GUILayout.Button ("Play")) {
					roleAction.regain ();
					index = 0;
					if (pausePersent.Count == 0)
						return;
					Callback cb = onActionCallback;
					Hashtable cbs = new Hashtable ();
					for (int i = 0; i < pausePersent.Count; i++) {
						cbs [pausePersent [i]] = cb;
					}
					cbs [100] = (Callback)onFinishActionCallback;
					roleAction.setAction (action, cbs);
					if (effect != null) {
						effect.gameObject.SetActive (true);
					}
				}
				if (GUILayout.Button ("Continue")) {
					index++;
					roleAction.regain ();
					if (index > pausePersent.Count) {
						index = 0;
						roleAction.setAction (CLRoleAction.Action.idel, null);
						return;
					}
				}
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField ("Effect object");
				effect = (CLEffect)(EditorGUILayout.ObjectField (effect, typeof(CLEffect)));
			}
			GUILayout.EndHorizontal ();
			
			for (int i = 0; i < pausePersent.Count; i++) {
				GUILayout.BeginHorizontal ();
				{
					pausePersent [i] = EditorGUILayout.IntField (pausePersent [i], GUILayout.Width (100));
					if (GUILayout.Button ("-")) {
						pausePersent.RemoveAt (i);
						break;
					}
				}
				GUILayout.EndHorizontal ();
			}
			
			GUILayout.BeginHorizontal ();
			{
				onePersent = EditorGUILayout.IntField (onePersent, GUILayout.Width (100));
				if (GUILayout.Button ("+")) {
					onePersent = 100;
					pausePersent.Add (onePersent);
					return;
				}
			}
			GUILayout.EndHorizontal ();
		}
		NGUIEditorTools.EndContents ();
	}
}
