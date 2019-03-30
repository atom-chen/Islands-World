using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hivemind;
using Coolape;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MyRoot:ActionBase
{
	public int fubenID;

	public override void Deserialize (Hashtable map)
	{
		fubenID = MapEx.getInt (map, "fubenID");
	}
	public override Hashtable Serialize ()
	{
		Hashtable m = new Hashtable ();
		m["fubenID"] = fubenID;
		return m;
	}

	#if UNITY_EDITOR
	public override void DrawInspector (Node node)
	{
		fubenID = EditorGUILayout.IntField (new GUIContent ("副本ID"), fubenID);	
	}
	#endif
}
