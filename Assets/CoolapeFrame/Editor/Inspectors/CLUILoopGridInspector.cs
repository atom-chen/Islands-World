using UnityEngine;
using System.Collections;
using UnityEditor;
using Coolape;

[CustomEditor (typeof(CLUILoopGrid), true)]
public class CLUILoopGridInspector : Editor
{
	CLUILoopGrid loopGrid;

	public override void OnInspectorGUI ()
	{
		loopGrid = (CLUILoopGrid)target;
		EditorGUILayout.BeginHorizontal ();
		{
			EditorGUILayout.LabelField ("Is Play Tween");
			loopGrid.isPlayTween = EditorGUILayout.Toggle (loopGrid.isPlayTween);
		}
		EditorGUILayout.EndHorizontal ();
		if (loopGrid.isPlayTween) {
			base.OnInspectorGUI ();
		} else  {
			loopGrid.cellCount = EditorGUILayout.IntField("Cell Count", loopGrid.cellCount);
		}
	}
}
