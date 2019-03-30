using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Coolape;
using System;
using System.IO;

public class ECLCurve2Code : EditorWindow
{
	AnimationCurve curve = new AnimationCurve ();

	void OnGUI ()
	{
		if (GUILayout.Button ("Gen C#")) {
			Debug.Log (getCode ());
		}
		if (GUILayout.Button ("Gen Lua")) {
			Debug.Log (getCodeLua ());
		}
		GUILayout.Space (5);
		ECLEditorUtl.BeginContents ();
		{
			EditorGUILayout.BeginHorizontal ();
			{

				curve = EditorGUILayout.CurveField ("Curve", curve, GUILayout.Height (200));
			}
			EditorGUILayout.EndHorizontal ();
		}
		ECLEditorUtl.EndContents ();
	}

	string getCode ()
	{
		
		PStr p = PStr.b ().a ("AnimationCurve curve = new AnimationCurve ();\n");
		if (curve == null)
			return "";
		Keyframe kf = new Keyframe ();
		for (int i = 0; i < curve.length; i++) {
			kf = curve [i];
			p.a ("curve.AddKey(new Keyframe(").a (kf.time).a ("f, ").a (kf.value).a ("f, ").a (kf.inTangent).a ("f, ").a (kf.outTangent).a ("f));\n");
		}
		return p.e ();
	}

	string getCodeLua ()
	{
		PStr p = PStr.b ().a ("local curve = AnimationCurve ();\n");
		if (curve == null)
			return "";
		Keyframe kf = new Keyframe ();
		for (int i = 0; i < curve.length; i++) {
			kf = curve [i];
			p.a ("curve:AddKey(Keyframe(").a (kf.time).a (", ").a (kf.value).a (", ").a (kf.inTangent).a (", ").a (kf.outTangent).a ("));\n");
		}
		return p.e ();
	}
}
