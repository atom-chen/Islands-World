using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Coolape;

public class CLPrefabLightmapDataEditor : Editor
{
	// 把renderer上面的lightmap信息保存起来，以便存储到prefab上面
	public static void SaveLightmapInfo ()
	{
		GameObject go = Selection.activeGameObject;
		if (go == null) {
			EditorUtility.DisplayDialog("Alert", "Please select a gameObject!", "Okey");
			return;
		}

		CLPrefabLightmapData data = go.GetComponent<CLPrefabLightmapData>();
		if (data == null) {
			data = go.AddComponent<CLPrefabLightmapData>();
		}
		
		data.SaveLightmap();
		EditorUtility.SetDirty(go);
	}
	
	// 把保存的lightmap信息恢复到renderer上面
	public static void LoadLightmapInfo()
	{
		GameObject go = Selection.activeGameObject;
		if (go == null) {
			EditorUtility.DisplayDialog("Alert", "Please select a gameObject!", "Okey");
			return;
		}

		CLPrefabLightmapData data = go.GetComponent<CLPrefabLightmapData>();
		if (data == null) {
			EditorUtility.DisplayDialog("Alert", "Can't find [CLPrefabLightmapData] component!", "Okey");
			return;
		}
		
		data.LoadLightmap();
		EditorUtility.SetDirty(go);
	}

	public static void ClearLightmapInfo()
	{
		GameObject go = Selection.activeGameObject;
		if (go == null) {
			EditorUtility.DisplayDialog("Alert", "Please select a gameObject!", "Okey");
			return;
		}
		
		CLPrefabLightmapData data = go.GetComponent<CLPrefabLightmapData>();
		if (data == null) {
			EditorUtility.DisplayDialog("Alert", "Can't find [CLPrefabLightmapData] component!", "Okey");
			return;
		}

		data.m_RendererInfo.Clear();
		data.m_Lightmaps = null;
		EditorUtility.SetDirty(go);
	}
}