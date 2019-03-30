using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using Coolape;

[CustomEditor (typeof(CLBehaviourWithUpdate4Lua), true)]
public class CLBehaviourWithUpdate4LuaInspector :CLBaseLuaInspector
{
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
	}
}
