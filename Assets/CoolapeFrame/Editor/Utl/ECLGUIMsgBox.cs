using UnityEngine;
using UnityEditor;
using System.Collections;
using Coolape;

public class ECLGUIMsgBox : EditorWindow
{
	Vector2 scrollPos = Vector2.zero;
	//	Rect windowRect = new Rect (0,0,Screen.width/2, Screen.height/2);
	public string msg;
	public Callback callback;
	public Callback callback2;
	bool is2Buttons = true;
	int eachLen = 10000;

	void OnGUI ()
	{
		// Begin Window
//		BeginWindows ();
		
		// All GUI.Window or GUILayout.Window must come inside here
		//所有GUI.Window 或 GUILayout.Window 必须在这里面
//		windowRect = GUILayout.Window (1, windowRect, DoWindow, title);
		
		// Collect all the windows between the two.
		//在这两者之间搜集所有窗口
//		EndWindows ();

		scrollPos = EditorGUILayout.BeginScrollView (scrollPos, GUILayout.Width (position.width), GUILayout.Height (position.height - 30));
		int pos = 0;
		while (pos < msg.Length) {
			GUILayout.TextArea (msg.Substring (pos, msg.Length - pos > eachLen ? eachLen : msg.Length - pos));
			pos += eachLen;
		}
		EditorGUILayout.EndScrollView ();
		
		EditorGUILayout.BeginHorizontal ();

		if (GUILayout.Button ("Okay")) {
			this.Close ();
			if (callback != null) {
				callback ();
			}
		}
		if (is2Buttons) {
			GUI.color = Color.yellow;
			if (GUILayout.Button ("Cancel")) {
				this.Close ();
				if (callback2 != null) {
					callback2 ();
				}
			}
		}
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal ();
	}
	
	//	void DoWindow (int index) {
	//
	//		if(GUILayout.Button ("Okay"))
	//			this.Close();
	//		GUI.DragWindow ();
	//	}


	public static void show (string title, string msg, Callback callback)
	{
		show (title, msg, callback, null, false);
	}

	public static void show (string title, string msg, Callback callback, Callback callback2, bool _is2Buttons = true)
	{
		ECLGUIMsgBox window = EditorWindow.GetWindow<ECLGUIMsgBox> (true, "GUIMsgBox", true);
		if (window == null) {
			window = new ECLGUIMsgBox ();
		}
		window.title = title;
		window.msg = msg;
		window.callback = callback;
		window.callback2 = callback2;
		window.is2Buttons = _is2Buttons;
		Rect rect = window.position;
//		rect = new Rect (-Screen.width/2, Screen.height / 2 - Screen.height / 4, Screen.width / 2, Screen.height / 2);
		rect.x = -Screen.width - Screen.width / 4;
		rect.y = Screen.height / 2 - Screen.height / 4;
		rect.width = Screen.width / 2;
		rect.height = Screen.height / 2;
		window.position = rect;

		window.Show ();
	}
}
