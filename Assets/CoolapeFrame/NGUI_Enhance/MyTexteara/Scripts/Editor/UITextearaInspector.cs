using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections;

[CustomEditor (typeof(UITexteara), true)]
public class UITextearaInspector : Editor
{
	UITexteara instance;

	void OnEnable ()
	{
		instance = target as UITexteara; 
	}

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		if (instance == null)
			return;
		using (new UnityEditorHelper.HighlightBox ()) {
			instance.effectMode = (UITexteara.EffectMode)EditorGUILayout.EnumPopup ("Effect Type", instance.effectMode);
			if (instance.effectMode == UITexteara.EffectMode.none)
				return;
			instance.method = (UITweener.Method)EditorGUILayout.EnumPopup ("Effect Method", instance.method);

			GUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField ("Duration");
				instance.duration = EditorGUILayout.FloatField (instance.duration);
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField ("Delay");
				instance.delay = EditorGUILayout.FloatField (instance.delay);
			}
			GUILayout.EndHorizontal ();
		}

		GUILayout.BeginHorizontal ();
		{
			if(GUILayout.Button ("Refresh")) {
				instance.refresh (true);
			}

			if (GUILayout.Button ("Clean")) {
				instance.clean ();
			}
		}
		GUILayout.EndHorizontal ();
		if (!Application.isPlaying) {
			EditorUtility.SetDirty (instance);
			EditorSceneManager.MarkAllScenesDirty ();
		}
	}
}
