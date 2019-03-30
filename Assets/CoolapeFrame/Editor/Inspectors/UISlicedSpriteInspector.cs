using UnityEngine;
using UnityEditor;
using System.Collections;
using Coolape;

[CanEditMultipleObjects]
[CustomEditor(typeof(UISlicedSprite), true)]
public class UISlicedSpriteInspector : UISpriteInspector
{
	
	
	/// <summary>
	/// Draw the atlas and sprite selection fields.
	/// </summary>
	
	protected override bool ShouldDrawProperties ()
	{
		GUILayout.BeginHorizontal();
		SerializedProperty atlas = NGUIEditorTools.DrawProperty("Center rect", serializedObject, "mCenter", GUILayout.MinWidth(20f));
		GUILayout.EndHorizontal();

		base.ShouldDrawProperties();
		

		return true;
	}
	
}
