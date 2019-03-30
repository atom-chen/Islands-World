using UnityEngine;
using System.Collections;
using UnityEditor;
using Coolape;

[CustomEditor (typeof(UIGridPage), true)]
public class CLUIGridPageInspector : UIGridEditor
{
	public override void OnInspectorGUI ()
	{
//		base.OnInspectorGUI ();
		DrawDefaultInspector ();
	}
}
