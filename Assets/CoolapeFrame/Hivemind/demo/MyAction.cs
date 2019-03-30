using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hivemind;
using Coolape;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MyAction : ActionBase
{
	public int triggerType = 0;
	public bool hideSelf = false;
	public int talkId = 0;
	public string npcCfg = "";
	public string boxCfg = "";

	public override Hashtable Serialize ()
	{
		Hashtable m = new Hashtable ();
		m["triggerType"] = triggerType;
		m ["hideSelf"] = hideSelf;
		m ["talkId"] = talkId;
		m ["npcCfg"] = npcCfg.Trim ();
		m ["boxCfg"] = boxCfg;
		return m;
	}

	public override void Deserialize (Hashtable map)
	{
		triggerType = MapEx.getInt (map, "triggerType");
		hideSelf = MapEx.getBool (map, "hideSelf");
		talkId = MapEx.getInt (map, "talkId");
		npcCfg = MapEx.getString (map, "npcCfg");
		boxCfg = MapEx.getString (map, "boxCfg");
	}

	#if UNITY_EDITOR
	public override void DrawInspector (Node node)
	{
		triggerType = (EditorGUILayout.Toggle (new GUIContent ("主角碰撞触发"), (triggerType== 0?true:false))) ?0:1;
		triggerType = (EditorGUILayout.Toggle (new GUIContent ("自动触发"), (triggerType == 0 ? false : true ))) ? 1:0;
		EditorGUILayout.LabelField("---------------------------");
		EditorGUILayout.Space();
		hideSelf = EditorGUILayout.Toggle (new GUIContent ("Hide Self"), hideSelf);
		talkId = EditorGUILayout.IntField (new GUIContent ("Talking id"), talkId);

//		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField (new GUIContent ("Npc Config"));
		npcCfg = EditorGUILayout.TextArea (npcCfg);
//		EditorGUILayout.EndHorizontal ();

		boxCfg = EditorGUILayout.TextField (new GUIContent ("Box Config"), boxCfg);
	}
	#endif
}
