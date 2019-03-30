/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  fps
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class CLFPS : MonoBehaviour
	{
		public bool isDispalyFps = true;
		public Rect displayRect = new Rect (0, 0, Screen.width, Screen.height * 2 / 100);
		public Color fontColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
		float deltaTime = 0.0f;
		[System.NonSerialized]
		public float fps = 0;
		float msec = 0;
		string text = "";
		GUIStyle style = new GUIStyle ();

		void Start ()
		{
			style.alignment = TextAnchor.UpperLeft;
			style.fontSize = (int)(displayRect.height);
			style.normal.textColor = fontColor;
		}

		void Update ()
		{
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		}

		void OnGUI ()
		{
			if (!isDispalyFps)
				return;
			msec = deltaTime * 1000.0f;
			fps = 1.0f / deltaTime;
			text = string.Format ("{0:0.} fps", fps);
			// text = string.Format ("{0:0.0} ms ({1:0.} fps)", msec, fps);
			GUI.Label (displayRect, text, style);
		}
	}
}
