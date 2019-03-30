//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Coolape;

/// <summary>
/// Inspector class used to edit UITextures.
/// </summary>

[CanEditMultipleObjects]
[CustomEditor(typeof(CLUITextureUseAtlas), true)]
public class CLUITextureUseAtlasInspector : UITextureInspector
{
	CLUITextureUseAtlas mTex;

	protected override void OnEnable ()
	{
		base.OnEnable();
		mTex = target as CLUITextureUseAtlas;
	}

	protected override bool ShouldDrawProperties ()
	{
		if (target == null) return false;

		GUILayout.BeginHorizontal();
		if (NGUIEditorTools.DrawPrefixButton("Atlas"))
			ComponentSelector.Show<UIAtlas>(OnSelectAtlas);
		

		EditorGUILayout.ObjectField (mTex.mAtlas, typeof(UIAtlas), GUILayout.Width (100));
		GUILayout.EndHorizontal();

		SerializedProperty isSharedMaterial = NGUIEditorTools.DrawProperty("isSharedMaterial", serializedObject, "isSharedMaterial", GUILayout.MinWidth(20f));

		base.ShouldDrawProperties ();
		return true;
	}


	void OnSelectAtlas (Object obj)
	{
		serializedObject.Update();
//		SerializedProperty sp = serializedObject.FindProperty("mAtlas");
//		sp.objectReferenceValue = obj;
//		serializedObject.ApplyModifiedProperties();

		SerializedProperty sp = serializedObject.FindProperty("atlasName");		// add by chenbin
		sp.stringValue = obj.name;		// add by chenbin
		serializedObject.ApplyModifiedProperties();		// add by chenbin

		NGUITools.SetDirty(serializedObject.targetObject);
		NGUISettings.atlas = obj as UIAtlas;
	}
}
