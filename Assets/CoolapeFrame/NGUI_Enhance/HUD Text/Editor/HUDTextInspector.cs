using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(HUDText))]
public class HUDTextInspector:Editor  {
	HUDText hud;
	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();
		hud = target as HUDText;
		EditorGUILayout.BeginHorizontal ();{
			if (NGUIEditorTools.DrawPrefixButton ("Font", GUILayout.Width (64f))) {
				ComponentSelector.Show<UIFont> (OnNGUIFont);
			}
			NGUIEditorTools.DrawProperty ("Font Name", serializedObject, "fontName");		// add by chenbin
		}
		EditorGUILayout.EndHorizontal ();
		base.OnInspectorGUI ();

	}
	
	void OnNGUIFont (Object obj)
	{
		serializedObject.Update();
		SerializedProperty sp = serializedObject.FindProperty("font");
		sp.objectReferenceValue = obj;
		serializedObject.ApplyModifiedProperties();
		hud.fontName = obj.name;
		NGUISettings.ambigiousFont = obj;
	}
}
