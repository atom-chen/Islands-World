using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using Coolape;

//页面视图
[CustomEditor (typeof(CLCellLua), true)]
public class CLCellLuaInspector : CLBehaviour4LuaInspector
{
	CLCellLua cell;

	public override void OnInspectorGUI ()
	{
		cell = target as CLCellLua;
		base.OnInspectorGUI ();
		if (GUILayout.Button ("Reset Atlas & Font")) {
			if (cell.isNeedResetAtlase) {
//				CLUIInit.self.init ();
				CLUIUtl.resetAtlasAndFont (cell.transform, false);
//				CLUIInit.self.clean ();
			}
		}
	}
}
