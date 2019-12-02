using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using Coolape;

//页面视图
[CustomEditor(typeof(CLPanelLua), true)]
public class CLPanelLuaInspector : CLBehaviour4LuaInspector
{
	private CLPanelLua panel;
	Object panelData;
    Object frameObj;
    bool _isFinishInit = false;

    public override void OnInspectorGUI()
	{
		panel = target as CLPanelLua;

		panel.isNeedMask4Init = EditorGUILayout.Toggle ("Is Need Mask 4 Show", panel.isNeedMask4Init);
		if (panel.isNeedMask4Init) {
			panel.isNeedMask4InitOnlyOnce = EditorGUILayout.Toggle ("    Only First Show", panel.isNeedMask4InitOnlyOnce);
		}

		base.OnInspectorGUI();
        init();

        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("背景框", GUILayout.Width(100));
            frameObj = EditorGUILayout.ObjectField(frameObj, typeof(UnityEngine.Object), GUILayout.Width(125));
        }
        GUILayout.EndHorizontal();
        string path = AssetDatabase.GetAssetPath(frameObj);
        panel.frameName = Path.GetFileNameWithoutExtension(path);
        EditorUtility.SetDirty(panel);

        if (GUILayout.Button("增加渐显缩放效果"))
        {
            //instance.EffectList.Count
            TweenScale ts = panel.gameObject.AddComponent<TweenScale>();
            ts.from = Vector3.one * 1.5f;
            ts.to = Vector3.one;
            ts.duration = 0.5f;
            ts.method = UITweener.Method.EaseInOut;
            ts.enabled = false;
            panel.EffectList.Add(ts);

            TweenAlpha ta = panel.gameObject.AddComponent<TweenAlpha>();
            ta.from = 0.1f;
            ta.to = 1;
            ta.duration = 0.5f;
            ta.method = UITweener.Method.EaseInOut;
            ta.enabled = false;
            panel.EffectList.Add(ta);
            panel.effectType = CLPanelBase.EffectType.synchronized;
            EditorUtility.SetDirty(panel);
        }

        if (GUILayout.Button("增加渐显左移效果"))
        {
            //instance.EffectList.Count
            TweenPosition ts = panel.gameObject.AddComponent<TweenPosition>();
            ts.from = Vector3.left * 1920;
            ts.to = Vector3.zero;
            ts.duration = 0.5f;
            ts.method = UITweener.Method.EaseInOut;
            ts.enabled = false;
            panel.EffectList.Add(ts);

            TweenAlpha ta = panel.gameObject.AddComponent<TweenAlpha>();
            ta.from = 0.1f;
            ta.to = 1;
            ta.duration = 0.5f;
            ta.method = UITweener.Method.EaseInOut;
            ta.enabled = false;
            panel.EffectList.Add(ta);
            panel.effectType = CLPanelBase.EffectType.synchronized;
            EditorUtility.SetDirty(panel);
        }

        if (GUILayout.Button("增加渐显下移效果"))
        {
            //instance.EffectList.Count
            TweenPosition ts = panel.gameObject.AddComponent<TweenPosition>();
            ts.from = Vector3.up * 1080;
            ts.to = Vector3.zero;
            ts.duration = 0.5f;
            ts.method = UITweener.Method.EaseInOut;
            ts.enabled = false;
            panel.EffectList.Add(ts);

            TweenAlpha ta = panel.gameObject.AddComponent<TweenAlpha>();
            ta.from = 0.1f;
            ta.to = 1;
            ta.duration = 0.5f;
            ta.method = UITweener.Method.EaseInOut;
            ta.enabled = false;
            panel.EffectList.Add(ta);
            panel.effectType = CLPanelBase.EffectType.synchronized;
            EditorUtility.SetDirty(panel);
        }

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
        GameObject go = AssetDatabase.LoadAssetAtPath("Assets/" + CLPathCfg.self.basePath + "/upgradeRes4Dev/priority/ui/panel/" + panel.name + ".prefab", typeof(GameObject)) as GameObject;

        doSaveAsset(go);
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
            PrefabUtility.SavePrefabAsset(go);
		}
		string dir = Application.dataPath + "/" + ECLEditorUtl.getPathByObject(go);
		dir = Path.GetDirectoryName(dir);
		ECLCreatAssetBundle4Update.createAssets4Upgrade(dir, panel.gameObject, true);

		// 必须再取一次，好像执行了上面一句方法后，cell就会变成null
		panel = go.GetComponent<CLPanelBase>();
		if (panel != null && panel.isNeedResetAtlase) {
			CLUIUtl.resetAtlasAndFont(panel.transform, false);
            PrefabUtility.SavePrefabAsset(go);
        }
	}


    void init()
    {
        if (!_isFinishInit || frameObj == null)
        {
            _isFinishInit = true;

            if (!string.IsNullOrEmpty(panel.frameName))
            {
                string tmpPath = PStr.b().a("Assets/").a(CLPathCfg.self.basePath).a("/upgradeRes4Dev/priority/ui/other/").a(panel.frameName).a(".prefab").e();
                frameObj = AssetDatabase.LoadMainAssetAtPath(tmpPath);
            }
        }
    }
}
