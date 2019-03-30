using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using Coolape;

//页面视图
using System.Collections.Generic;

[CustomEditor (typeof(CLPathCfg), true)]

public class CLPathCfgInspector : Editor
{
	CLPathCfg instance;

	public override void OnInspectorGUI ()
	{
		instance = target as CLPathCfg;

		NGUIEditorTools.BeginContents ();
		{
			GUILayout.Space (3);
			
			GUILayout.BeginHorizontal ();
			{
				GUILayout.Label ("Project Name");
				instance.basePath = EditorGUILayout.TextField (instance.basePath);
				if (GUILayout.Button ("Reset Path")) {
					resetPath ();
					EditorUtility.SetDirty (instance);
				}
			}
			GUILayout.EndHorizontal ();
		}
		NGUIEditorTools.EndContents ();

		DrawDefaultInspector ();
	}

	public void resetPath ()
	{
		instance.resetPath (instance.basePath);
	}

}
