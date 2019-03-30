using UnityEngine;
using UnityEditor;
using System.Collections;

#if UNITY_3_5
[CustomEditor(typeof(UIRichText4Chat))]
#else
[CustomEditor(typeof(UIRichText4Chat), true)]
#endif
public class UIRichText4ChatInspector : Editor {

	UIRichText4Chat richText4Chat;
	public override void OnInspectorGUI ()
	{
		richText4Chat = target as UIRichText4Chat;
		GUILayout.BeginHorizontal();
		if (NGUIEditorTools.DrawPrefixButton ("Face Atlas")) {
			ComponentSelector.Show<UIAtlas> (OnSelectAtlas);
		}
		SerializedProperty atlas = NGUIEditorTools.DrawProperty("", serializedObject, "faceAtlas", GUILayout.MinWidth(20f));
		if (atlas != null) {
			UIAtlas atl = atlas.objectReferenceValue as UIAtlas;
			if(atl != richText4Chat.faceAtlas) {
				Debug.Log(atl.name);
				richText4Chat.faceAtlas = atl;
				richText4Chat.atlasName = atl.name;
			}
		}

		if (GUILayout.Button("Edit", GUILayout.Width(40f)))
		{
			if (atlas != null)
			{
				UIAtlas atl = atlas.objectReferenceValue as UIAtlas;
				NGUISettings.atlas = atl;
				NGUIEditorTools.Select(atl.gameObject);
			}
		}
		GUILayout.EndHorizontal();

		DrawDefaultInspector ();
	}
	
	void OnSelectAtlas (Object obj)
	{
		Debug.Log (obj.name);
		serializedObject.Update();
		SerializedProperty sp = serializedObject.FindProperty("faceAtlas");
		sp.objectReferenceValue = obj;
		serializedObject.ApplyModifiedProperties();
		
		sp = serializedObject.FindProperty("atlasName");		// add by chenbin
		sp.stringValue = obj.name;		// add by chenbin
		serializedObject.ApplyModifiedProperties();		// add by chenbin
		
		NGUITools.SetDirty(serializedObject.targetObject);
		NGUISettings.atlas = obj as UIAtlas;
	}
}
