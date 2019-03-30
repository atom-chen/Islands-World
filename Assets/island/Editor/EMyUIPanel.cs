using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Coolape;
using System.IO;

[CustomEditor(typeof(MyUIPanel), true)]
public class EMyUIPanel : CLPanelLuaInspector {

    Object frameObj;
    MyUIPanel instance;

    bool isFinishInit = false;

    public override void OnInspectorGUI()
    {

        instance = target as MyUIPanel;
        init();

        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("背景框", GUILayout.Width(100));
            frameObj = EditorGUILayout.ObjectField(frameObj, typeof(UnityEngine.Object), GUILayout.Width(125));
        }
        GUILayout.EndHorizontal();
        string path = AssetDatabase.GetAssetPath(frameObj);
        instance.frameName = Path.GetFileNameWithoutExtension(path);
        EditorUtility.SetDirty(instance);

        base.OnInspectorGUI();
        if(GUILayout.Button("增加渐显缩放效果")) {
            //instance.EffectList.Count
            TweenScale ts = instance.gameObject.AddComponent<TweenScale>();
            ts.from = Vector3.one * 1.5f;
            ts.to = Vector3.one;
            ts.duration = 0.5f;
            ts.method = UITweener.Method.EaseInOut;
            ts.enabled = false;
            instance.EffectList.Add(ts);

            TweenAlpha ta = instance.gameObject.AddComponent<TweenAlpha>();
            ta.from = 0.1f;
            ta.to = 1;
            ta.duration = 0.5f;
            ta.method = UITweener.Method.EaseInOut;
            ta.enabled = false;
            instance.EffectList.Add(ta);
            instance.effectType = CLPanelBase.EffectType.synchronized;
            EditorUtility.SetDirty(instance);
        }

        if (GUILayout.Button("增加渐显左移效果"))
        {
            //instance.EffectList.Count
            TweenPosition ts = instance.gameObject.AddComponent<TweenPosition>();
            ts.from = Vector3.left * 1920;
            ts.to = Vector3.zero;
            ts.duration = 0.5f;
            ts.method = UITweener.Method.EaseInOut;
            ts.enabled = false;
            instance.EffectList.Add(ts);

            TweenAlpha ta = instance.gameObject.AddComponent<TweenAlpha>();
            ta.from = 0.1f;
            ta.to = 1;
            ta.duration = 0.5f;
            ta.method = UITweener.Method.EaseInOut;
            ta.enabled = false;
            instance.EffectList.Add(ta);
            instance.effectType = CLPanelBase.EffectType.synchronized;
            EditorUtility.SetDirty(instance);
        }

        if (GUILayout.Button("增加渐显下移效果"))
        {
            //instance.EffectList.Count
            TweenPosition ts = instance.gameObject.AddComponent<TweenPosition>();
            ts.from = Vector3.up * 1080;
            ts.to = Vector3.zero;
            ts.duration = 0.5f;
            ts.method = UITweener.Method.EaseInOut;
            ts.enabled = false;
            instance.EffectList.Add(ts);

            TweenAlpha ta = instance.gameObject.AddComponent<TweenAlpha>();
            ta.from = 0.1f;
            ta.to = 1;
            ta.duration = 0.5f;
            ta.method = UITweener.Method.EaseInOut;
            ta.enabled = false;
            instance.EffectList.Add(ta);
            instance.effectType = CLPanelBase.EffectType.synchronized;
            EditorUtility.SetDirty(instance);
        }
    }

    void init()
    {
        if (!isFinishInit || frameObj == null)
        {
            isFinishInit = true;

            if (!string.IsNullOrEmpty(instance.frameName))
            {
                string tmpPath = PStr.b().a("Assets/").a(CLPathCfg.self.basePath).a("/upgradeRes4Dev/priority/ui/other/").a(instance.frameName).a(".prefab").e();
                frameObj = AssetDatabase.LoadMainAssetAtPath(tmpPath);
            }
        }
    }
}
