using UnityEditor;
using UnityEngine;
using System.Collections;
using Coolape;

[CustomEditor (typeof(CombineChildren), true)]
public class CombineChildrenInspector : Editor
{
	CombineChildren combine;
	Object luaAsset = null;

	public override void OnInspectorGUI ()
	{
		combine = (CombineChildren)target;
		base.OnInspectorGUI ();
		if (GUILayout.Button ("Combine to Save")) {
			combine.combineChildren (true);
			EditorUtility.SetDirty (combine);
		}
		if (GUILayout.Button ("Combine")) {
			combine.combineChildren ();
		}
		
		if (GUILayout.Button ("Explain")) {
			combine.explainChildren ();
		}

	}
}
