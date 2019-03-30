using UnityEngine;
using System.Collections;

namespace Coolape
{
	[RequireComponent (typeof(HUDText))]
	public class CLAlert : MonoBehaviour
	{
		public static CLAlert self;
		UIPanel panel;
		HUDText hudText;
		public string hudBackgroundSpriteName = "public_empty";
		public UIBasicSprite.Type hudBackgroundSpriteType = UIBasicSprite.Type.Simple;
		public int bgAnchorLeft = -5;
		public int bgAnchorBottom = -5;
		public int bgAnchorTop = 5;
		public int bgAnchorRight = 5;
		public Color hudBackgroundColor = Color.white;
		public static SpriteHudPool pool = new SpriteHudPool ();

		public CLAlert ()
		{
			self = this;
		}

		bool isFinishInit = false;

		void Start ()
		{
			if (isFinishInit)
				return;
			isFinishInit = true;
			hudText = GetComponent<HUDText> ();
			panel = GetComponent<UIPanel> ();
		}

		static object beforeStr = "";
		static long beforeTime = 0;

		public static void add (object msg)
		{
			add (msg, Color.white, 0);
		}
		public static void add (object msg, Color color, float delayTime)
		{
			add (msg, color, delayTime, 1, true, Vector3.zero);
		}

		public static void add (object msg, Color color, float delayTime, float scaleOffset)
		{
			add (msg, color, delayTime, scaleOffset, true, Vector3.zero);
		}

		public static void add (object msg, Color color, float scaleOffset, Vector3 posOffset)
		{
			add (msg, color, 0, scaleOffset, true, posOffset);
		}

		public static void add (object msg, Color color, float delayTime, float scaleOffset, bool needBackGround , Vector3 posOffset)
		{
			if (msg == null)
				return;
			
			if (beforeStr.Equals (msg) && beforeTime - System.DateTime.Now.ToFileTime () / 10000 > 0) {
				// 如果内容一样，且要2秒内，都不再弹出
				return;
			}

			if (msg is string) {
				msg = StrEx.trimStr (msg.ToString());
			}
			self.Start ();
			self.panel.depth = CLUIInit.self.AlertRootDepth;
			beforeStr = msg;
			beforeTime = System.DateTime.Now.AddSeconds (2).ToFileTime () / 10000;
//		Debug.Log(self.hudText);
			UILabel label = self.hudText.Add (msg, color, delayTime, scaleOffset);

//			Debug.LogError (posOffset);
			self.hudText.transform.localPosition = posOffset;

			if (needBackGround) {
				if (label.transform.childCount > 0) {
					Transform sp = label.transform.GetChild (0);
					NGUITools.SetActive (sp.gameObject, true);
					sp.GetComponent<UISprite>().depth = label.depth - 1;
				} else {
					UISprite sp = pool.borrowObject (self.hudBackgroundSpriteName);
					if (sp != null) {
						sp.transform.parent = label.transform;
						sp.transform.localScale = Vector3.one;
						sp.transform.localPosition = Vector3.zero;
						sp.color = self.hudBackgroundColor;
						sp.type = self.hudBackgroundSpriteType;
						sp.depth = label.depth - 1;
						sp.SetAnchor (label.gameObject, self.bgAnchorLeft, self.bgAnchorBottom, self.bgAnchorRight, self.bgAnchorTop);
						NGUITools.SetActive (sp.gameObject, true);
						sp.ResetAndUpdateAnchors ();
					}
				}
			} else {
				if (label.transform.childCount > 0) {
					Transform sp = label.transform.GetChild (0);
					NGUITools.SetActive (sp.gameObject, false);
				}
			}
		}
	}

	public class SpriteHudPool : AbstractObjectPool<UISprite>
	{
		public override UISprite createObject (string name = null)
		{
			UISprite sp = NGUITools.AddSprite (HUDRoot.go, CLUIInit.self.emptAtlas, name);
			sp.type = UIBasicSprite.Type.Sliced;
			return sp;
		}

		public override UISprite resetObject (UISprite t)
		{
			return t;
		}
	}
}
