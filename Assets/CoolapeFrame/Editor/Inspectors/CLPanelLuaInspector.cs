using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using Coolape;

//页面视图
[CustomEditor(typeof(CLPanelLua), true)]
public class CLPanelLuaInspector : CLBehaviour4LuaInspector
{
	CLPanelLua panel;
	Object panelData;

	public override void OnInspectorGUI()
	{
		panel = target as CLPanelLua;

		panel.isNeedMask4Init = EditorGUILayout.Toggle ("Is Need Mask 4 Show", panel.isNeedMask4Init);
		if (panel.isNeedMask4Init) {
			panel.isNeedMask4InitOnlyOnce = EditorGUILayout.Toggle ("    Only First Show", panel.isNeedMask4InitOnlyOnce);
		}

		base.OnInspectorGUI();
		NGUIEditorTools.BeginContents();
		{
			GUILayout.Space(3);
//			if (GUILayout.Button("Reload Lua")) {
//				reloadLua();
//			}
			if (GUILayout.Button("Reset Atlas & Font")) {
				if (panel.isNeedResetAtlase) {
//					CLUIInit.self.init ();
					CLUIUtl.resetAtlasAndFont(panel.transform, false);
//					CLUIInit.self.clean();
				}
			}
			if (GUILayout.Button("Save Panel 2 U3dType")) {
				saveToU3d();
			}
		}
		NGUIEditorTools.EndContents();
		GUILayout.Space(5);
	}

	void reloadLua()
	{
		panel.setLua();
	}

	void saveToU3d()
	{
		doSaveAsset(panel.gameObject);
		EditorUtility.DisplayDialog("success", "cuccess!", "Okey");
	}

	public static void doSaveAsset(GameObject go)
	{
		CLPanelBase panel = go.GetComponent<CLPanelBase>();
		if (panel == null)
			return;
		Debug.Log(panel.name);
		if (panel.isNeedResetAtlase) {
			CLUIUtl.resetAtlasAndFont(panel.transform, true);
		}
		string dir = Application.dataPath + "/" + ECLEditorUtl.getPathByObject(go);
		dir = Path.GetDirectoryName(dir);
		ECLCreatAssetBundle4Update.createAssets4Upgrade(dir, panel.gameObject, true);

		// 必须再取一次，好像执行了上面一句方法后，cell就会变成null
		panel = go.GetComponent<CLPanelBase>();
		if (panel != null && panel.isNeedResetAtlase) {
			CLUIUtl.resetAtlasAndFont(panel.transform, false);
		}
	}
}
