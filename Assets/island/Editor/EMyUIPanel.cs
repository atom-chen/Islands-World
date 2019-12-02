using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Coolape;
using System.IO;

[CustomEditor(typeof(MyUIPanel), true)]
public class EMyUIPanel : CLPanelLuaInspector
{
    private MyUIPanel instance;
    string title = "";
    bool isFinishInited = false;
    void init()
    {
        if (isFinishInited) return;
        isFinishInited = true;
        instance = target as MyUIPanel;
        string language = "Chinese";
        if (!Localization.language.Equals(language))
        {
            byte[] buff = null;
            string languageFile = PStr.b(
                CLPathCfg.self.localizationPath,
                language, ".txt").e();
#if UNITY_EDITOR
            if (CLCfgBase.self.isEditMode)
            {
                languageFile = PStr.b().a(CLPathCfg.persistentDataPath).a("/").a(languageFile).e();
                languageFile = languageFile.Replace("/upgradeRes/", "/upgradeRes4Dev/");
                buff = File.ReadAllBytes(languageFile);
            }
            else
            {
                buff = FileEx.readNewAllBytes(languageFile);
            }
#else
                buff = FileEx.readNewAllBytes(languageFile);
#endif
            Localization.Load(language, buff);
        }
        title = Localization.Get(instance.titleKeyName);
    }

    public override void OnInspectorGUI()
    {
        init();
        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("页面Title", ECLEditorUtl.width80);
            title = EditorGUILayout.TextField(title);

            #region add by chenbin
            if (GUILayout.Button("Select"))
            {
                ECLLocalizeSelection.open((ECLLocalizeSelection.OnSlecteCallback)OnSlecteLocalize);
            }
            #endregion
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("页面Title Key", ECLEditorUtl.width80);
            instance.titleKeyName = EditorGUILayout.TextField(instance.titleKeyName);
        }
        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }

    public void OnSlecteLocalize(string key, string val)
    {
        instance.titleKeyName = key;
        title = val;
    }

}
