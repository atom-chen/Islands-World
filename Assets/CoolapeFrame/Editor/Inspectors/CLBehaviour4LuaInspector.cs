using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using Coolape;

[CustomEditor (typeof(CLBehaviour4Lua), true)]
public class CLBehaviour4LuaInspector :CLBaseLuaInspector
{
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
	}
}
