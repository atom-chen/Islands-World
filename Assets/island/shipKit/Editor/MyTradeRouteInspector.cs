using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// My trade route inspector.路径生成器
/// </summary>
[CustomEditor(typeof(MyTradeRoute))]
public class MyTradeRouteInspector : Editor
{
	MyTradeRoute tradeToute;
	bool isEditor = false;
	Vector3 newPoint = Vector3.zero;

	public override void OnInspectorGUI ()
	{
		
		tradeToute = target as MyTradeRoute;
		DrawDefaultInspector ();
		NGUIEditorTools.DrawSeparator ();
		
		RaycastHit hitt;
		
		if (tradeToute.KeyTradeRouteNodes != null) {
			for (int i = 0; i < tradeToute.KeyTradeRouteNodes.Count -1; i++) {
				Debug.DrawLine (tradeToute.KeyTradeRouteNodes [i], tradeToute.KeyTradeRouteNodes [i + 1]);
			}
		}
		
		EditorGUILayout.BeginHorizontal ();
		{
			EditorGUILayout.LabelField ("Editor Trade Route");
			if (GUILayout.Button ("Editor")) {
				tradeToute.isEditMode = !tradeToute.isEditMode;
				isEditor = tradeToute.isEditMode;
//				tradeToute.Update();
			}
		}
		EditorGUILayout.EndHorizontal ();
		if (isEditor) {
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField ("在运行模式下移动该对象到某个点，点击Add即可以添加该点到路径中");
			}
			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField ("路径关键点");
			}
			EditorGUILayout.EndHorizontal ();
			if (tradeToute.KeyTradeRouteNodes == null) {
				tradeToute.KeyTradeRouteNodes = new System.Collections.Generic.List<UnityEngine.Vector3> ();
			}
			for (int i =0; i < tradeToute.KeyTradeRouteNodes.Count; i++) {
				Vector3 keyPoint = tradeToute.KeyTradeRouteNodes [i];
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.Vector3Field ("Key Point." + i, keyPoint);
					if (GUILayout.Button ("Location")) {
						tradeToute.transform.position = keyPoint;
						return;
					}
					if (GUILayout.Button ("Delecte")) {
						tradeToute.KeyTradeRouteNodes.RemoveAt (i);
						tradeToute.Update ();
						return;
					}
				}
				EditorGUILayout.EndHorizontal ();
			}
			
			EditorGUILayout.BeginHorizontal ();
			{
				newPoint = EditorGUILayout.Vector3Field ("New Key Point", tradeToute.transform.position);
				GUI.color = Color.green;
				if (GUILayout.Button ("Add")) {
					tradeToute.KeyTradeRouteNodes.Add (newPoint);
					tradeToute.Update ();
				}
				GUI.color = Color.white;
			}
			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.BeginHorizontal ();
			{
				if (GUILayout.Button ("Apply")) {
				}
			}
			EditorGUILayout.EndHorizontal ();
		}
	}
}
