/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   ui工具类
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;
using XLua;

namespace Coolape
{
	public enum TweenType
	{
		position,
		scale,
		alpha,
	}

	/// <summary>
	/// Init cell delegate.列表单元的初始回调函数
	/// </summary>
	public delegate void InitCellDelegate (GameObject cell, object content);

	public class CLUIUtl
	{
		/// <summary>
		/// Appends the list.向列追加元素,注意每个元素都要求绑定CLCellBase
		/// </summary>
		/// <param name='parent'>
		/// Parent.
		/// </param>
		/// <param name='prefabChild'>
		/// Prefab child.
		/// </param>
		/// <param name='list'>
		/// List.
		/// </param>
		/// <param name='itype'>
		/// Itype.
		/// </param>
		/// <param name='beforCount'>
		/// Befor count.之前已经有的元素个数
		/// </param>
		/// <param name='initCallback'>
		/// Init callback.
		/// </param>
		public static void appendList4Lua (UIGrid parent, GameObject prefabChild, ArrayList list,
		                                   int beforCount, object initCallback)
		{
			appendList (parent, prefabChild, list, typeof(CLCellLua), beforCount, null, initCallback);
		}

		public static void appendList (UIGrid parent, GameObject prefabChild, ArrayList list,
		                               System.Type itype, int beforCount, object initCallback)
		{
			appendList (parent, prefabChild, list, itype, beforCount, null, initCallback);
		}

		public static void appendList (UIGrid parent, GameObject prefabChild, ArrayList list,
		                               System.Type itype, int beforCount, GameObject nextPage, object initCallback, float offset = 0)
		{
			if (list == null) {
				return;
			}
		
			if (parent == null) {
				return;
			}	
		
			parent.sorted = true;
			Transform go = null;
			string childName = "";
			for (int i = 0; i < list.Count; i++) {
				childName = NumEx.nStrForLen (beforCount + i, 5);
#if UNITY_5_6_OR_NEWER
				go = parent.transform.Find (childName);
#else
				go = parent.transform.FindChild (childName);
#endif

				if (go == null) {
					go = NGUITools.AddChild (parent.gameObject, prefabChild).transform;
					go.name = childName;
				}
			
				go.transform.localPosition = new Vector3 (0, -(beforCount + i) * parent.cellHeight + offset, 0);
				NGUITools.SetActive (go.gameObject, true);
				Utl.doCallback (initCallback, go.GetComponent<CLCellBase> (), list [i]);
			}
		
			if (nextPage != null && go != null) {
				nextPage.transform.localPosition = Vector3.zero;
				nextPage.transform.parent = go;
				nextPage.transform.localPosition = new Vector3 (0, -parent.cellHeight, 0);
			}
		}

		/// <summary>
		/// Resets the list.更新列表
		/// </summary>
		/// <param name='parent'>
		/// Parent.
		/// </param>
		/// <param name='prefabChild'>
		/// Prefab child.
		/// </param>
		/// <param name='list'>
		/// List.
		/// </param>
		/// <param name='itype'>
		/// Itype.
		/// </param>
		/// <param name='initCallback'>
		/// Init callback.
		/// </param>
		public static void resetList4Lua (object parent, GameObject prefabChild, object list, object initCallback)
		{
			resetList4Lua (parent, prefabChild, list, initCallback, true);
		}

		public static void resetList4Lua (object parent, GameObject prefabChild, object list, object initCallback, bool isReposition)
		{
			resetList (parent, prefabChild, list, typeof(CLCellLua), initCallback, isReposition, false, 0);
		}

		public static void resetList4Lua (object parent, GameObject prefabChild, object list, object initCallback, bool isReposition, bool isPlayTween, float tweenSpeed = 0.2f)
		{
			resetList (parent, prefabChild, list, typeof(CLCellLua), initCallback, isReposition, isPlayTween, tweenSpeed);
		}

		public static void resetList (object parent, GameObject prefabChild, object list,
			System.Type itype, object initCallback, bool isReposition, bool isPlayTween, float tweenSpeed = 0.2f)
		{
			resetList (parent, prefabChild, list, itype, null, true, initCallback, isReposition, isPlayTween, tweenSpeed);
		}
		public static void resetList (object parent, GameObject prefabChild,
		                              object data, System.Type itype, GameObject nextPage, bool isShowNoneContent,
		                              object initCallback, bool isReposition, bool isPlayTween, float tweenSpeed = 0.2f)
		{
			object[] list = null;
			if (data is LuaTable) {
				ArrayList _list = CLUtlLua.luaTableVals2List ((LuaTable)data);
				list = _list.ToArray ();
				_list.Clear ();
				_list = null;
			} else if (data is ArrayList) {
				list = ((ArrayList)data).ToArray ();
			}
			if ((list == null || list.Length == 0) && isShowNoneContent) {
				//mtoast = NGUIPublic.toast (mtoast, USWarnMsg.warnMsgNoContent ());
			}
			if (parent == null) {
				return;
			}	
			bool isTable = false;
			if (typeof(UIGrid) == parent.GetType ()) {
				isTable = false;
			} else if (typeof(UITable) == parent.GetType ()) {
				isTable = true;
			} else {
				return;
			}
			
			Transform parentTf = null;
			if (isTable) {
				//((UITable)parent).sorting = UITable.Sorting.Alphabetic;
				parentTf = ((UITable)parent).transform;
			} else {
				((UIGrid)parent).sorted = true;
				parentTf = ((UIGrid)parent).transform;
			}
			Transform go;
			int i = 0, j = 0;
//			bool isNeedReposition = false;
			string childName = "";
			for (i = 0; i < parentTf.childCount && list != null && j < list.Length; i++) {
				childName = NumEx.nStrForLen (i, 5);
#if UNITY_5_6_OR_NEWER
				go = parentTf.Find (childName);
#else
				go = parentTf.FindChild (childName);
#endif
				if (go != null) {
					if (go.GetComponent (itype) != null) {
						NGUITools.SetActive (go.gameObject, true);
						Utl.doCallback (initCallback, go.GetComponent<CLCellBase> (), list [j]);
						NGUITools.updateAll(go.transform);

						if (isPlayTween) { 
							resetCellTween (i, parent, go.gameObject, tweenSpeed);
						}
					
						if ((j + 1) == list.Length && nextPage != null) {
							nextPage.transform.localPosition = Vector3.zero;
							nextPage.transform.parent = go;
							if (!isTable) {
								nextPage.transform.localPosition = new Vector3 (0, -((UIGrid)parent).cellHeight, 0);
							}
						}
						j++;
					}
				}
			}
		
			while (i < parentTf.childCount) {
				childName = NumEx.nStrForLen (i, 5);
#if UNITY_5_6_OR_NEWER
				go = parentTf.Find (childName);
#else
				go = parentTf.FindChild (childName);
#endif
				if (go != null && go.gameObject.activeSelf) {
					if (go.GetComponent (itype) != null) {
						NGUITools.SetActive (go.gameObject, false);
//						isNeedReposition = true;
					}
				}
				i++;
			}
			while (list != null && j < list.Length) {
				go = NGUITools.AddChild (parentTf.gameObject, prefabChild).transform;
//				isNeedReposition = true;
				childName = NumEx.nStrForLen (j, 5);
				go.name = childName;
				Utl.doCallback (initCallback, go.GetComponent<CLCellBase> (), list [j]);
				NGUITools.updateAll(go.transform);

				if (isPlayTween) { 
					resetCellTween (j, parent, go.gameObject, tweenSpeed);
				}
			
				if ((j + 1) == list.Length && nextPage != null) {
					nextPage.transform.localPosition = Vector3.zero;
					nextPage.transform.parent = go;
					if (!isTable) {
						nextPage.transform.localPosition = new Vector3 (0, -((UIGrid)parent).cellHeight, 0);
					}
				}
				j++;
			}
		
			if (!isPlayTween) {
				if (isReposition) {
					if (!isTable) {
						((UIGrid)parent).enabled = true;
						((UIGrid)parent).Start ();
						((UIGrid)parent).Reposition ();
						((UIGrid)parent).repositionNow = true;
						UIScrollView sv = ((UIGrid)parent).transform.parent.GetComponent<UIScrollView> ();
						if (sv != null) {
							sv.ResetPosition ();
						}
					} else {
						((UITable)parent).enabled = true;
						((UITable)parent).Start ();
						((UITable)parent).Reposition ();
						((UITable)parent).repositionNow = true;
						UIScrollView sv = ((UITable)parent).transform.parent.GetComponent<UIScrollView> ();
						if (sv != null) {
							sv.ResetPosition ();
						}
					}
				}
			}
		}

		/// <summary>
		/// Resets the cell tween.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="gridObj">Grid object.</param>
		/// <param name="cell">Cell.</param>
		public static void resetCellTween (int index, object gridObj, GameObject cell, 
		                                   float tweenSpeed, float duration = 0.5f, 
		                                   UITweener.Method method = UITweener.Method.EaseInOut, 
		                                   TweenType twType = TweenType.position)
		{
			switch (twType) {
			case TweenType.position:
				resetCellTweenPosition (index, gridObj, cell, tweenSpeed, duration, method);
				break;
			case TweenType.scale:
				resetCellTweenScale (index, gridObj, cell, tweenSpeed, duration, method);
				break;
			case TweenType.alpha:
				resetCellTweenAlpha (index, gridObj, cell, tweenSpeed, duration, method);
				break;
			}
		}

		public static void resetCellTweenPosition (int index, object gridObj, GameObject cell, 
		                                           float tweenSpeed, float duration = 0.5f, 
		                                           UITweener.Method method = UITweener.Method.EaseInOut)
		{
			if (gridObj.GetType () != typeof(UIGrid)) {
				Debug.LogWarning ("The cell tween must have grid!");
				return;
			}
			UIGrid grid = (UIGrid)gridObj;
			if (grid.maxPerLine > 1) {
				#if UNITY_EDITOR
				Debug.LogWarning ("The grid must have one line!");
				#endif
				return;
			}
			UIPanel panel = grid.transform.parent.GetComponent<UIPanel> ();
			float clippingWidth = panel == null ? 100 : panel.baseClipRegion.z;
			float clippingHeight = panel == null ? 100 : panel.baseClipRegion.w;
			Vector3 pos1 = Vector3.zero;
			Vector3 pos2 = Vector3.zero;
			if ((grid.arrangement == UIGrid.Arrangement.Horizontal &&
			    grid.maxPerLine == 0) ||
			    (grid.arrangement == UIGrid.Arrangement.Vertical &&
			    grid.maxPerLine == 1)) {
				pos1 = new Vector3 (index * grid.cellWidth, 0, 0);
				pos2 = new Vector3 (index * grid.cellWidth, -clippingHeight, 0);
			} else if ((grid.arrangement == UIGrid.Arrangement.Horizontal &&
			           grid.maxPerLine == 1) ||
			           (grid.arrangement == UIGrid.Arrangement.Vertical &&
			           grid.maxPerLine == 0)) {
				pos1 = new Vector3 (0, -index * grid.cellHeight, 0);
				pos2 = new Vector3 (-clippingWidth, -index * grid.cellHeight, 0);
			}

			TweenPosition tween = cell.GetComponent<TweenPosition> ();
			tween = tween == null ? cell.AddComponent<TweenPosition> () : tween;
			tween.method = method;
			tween.enabled = false;
			tween.from = pos2;
			tween.to = pos1;
			tween.duration = duration;
			tween.ResetToBeginning ();
			tween.delay = index * tweenSpeed;
			tween.Play ();
		}

	
		public static void resetCellTweenScale (int index, object gridObj, GameObject cell, 
		                                        float tweenSpeed, float duration = 0.5f, 
		                                        UITweener.Method method = UITweener.Method.EaseInOut)
		{
			if (gridObj.GetType () != typeof(UIGrid)) {
				Debug.LogWarning ("The cell tween must have grid!");
				return;
			}
			UIGrid grid = (UIGrid)gridObj;
			if (grid.maxPerLine > 1) {
				Debug.LogWarning ("The grid must have one line!");
				return;
			}
//		UIPanel panel = grid.transform.parent.GetComponent<UIPanel>();
//		float clippingWidth = panel == null ? 100 : panel.baseClipRegion.z;
//		float clippingHeight = panel == null ? 100 : panel.baseClipRegion.w;
//		Vector3 pos1 = Vector3.zero;
//		Vector3 pos2 = Vector3.zero;
//		if ((grid.arrangement == UIGrid.Arrangement.Horizontal &&
//		     grid.maxPerLine == 0) ||
//		    (grid.arrangement == UIGrid.Arrangement.Vertical &&
//		 grid.maxPerLine == 1)) {
//			pos1 = new Vector3(index * grid.cellWidth, 0, 0);
//		} else if ((grid.arrangement == UIGrid.Arrangement.Horizontal &&
//		            grid.maxPerLine == 1) ||
//		           (grid.arrangement == UIGrid.Arrangement.Vertical &&
//		 grid.maxPerLine == 0)) {
//			pos1 = new Vector3(0, -index * grid.cellHeight, 0);
//		}
//		cell.transform.localPosition = pos1;

			TweenScale tween = cell.GetComponent<TweenScale> ();
			tween = tween == null ? cell.AddComponent<TweenScale> () : tween;
			tween.method = method;
			tween.enabled = false;
			tween.from = Vector3.zero;
			tween.to = Vector3.one;
			tween.duration = duration;
			tween.ResetToBeginning ();
			tween.delay = index * tweenSpeed;
			tween.Play ();
		}

		public static void resetCellTweenAlpha (int index, object gridObj, GameObject cell, 
		                                        float tweenSpeed, float duration = 0.5f, 
		                                        UITweener.Method method = UITweener.Method.EaseInOut)
		{
			TweenAlpha tween = cell.GetComponent<TweenAlpha> ();
			tween = tween == null ? cell.AddComponent<TweenAlpha> () : tween;
			tween.method = method;
			tween.enabled = false;
			tween.from = 0;
			tween.to = 1;
			tween.duration = duration;
			tween.ResetToBeginning ();
			tween.delay = index * tweenSpeed;
			tween.Play ();
		}

		/// <summary>
		/// Resets the chat list.聊天列表
		/// </summary>
		public static void resetChatList (GameObject grid, GameObject prefabChild, ArrayList list,
		                                  System.Type itype, float offsetY, object initCallback)
		{
			if (list == null) {
				return;
			}
		
			//NGUITools.SetActive(grid, true);
			int cellObjCount = grid.transform.childCount; //grid.GetComponentsInChildren(itype).Length;
			GameObject go = null;
			BoxCollider bc = null;
			float anchor = 0;
			Vector3 pos = Vector3.zero;
			int i = 0;
			for (i = list.Count - 1; i >= 0; i--) {
				if (i < cellObjCount) {
#if UNITY_5_6_OR_NEWER
					go = grid.transform.Find (i.ToString ()).gameObject;
#else
					go = grid.transform.FindChild (i.ToString ()).gameObject;
#endif
					NGUITools.SetActive (go, true);
				} else {
					go = NGUITools.AddChild (grid.gameObject, prefabChild);
					go.name = i.ToString ();
				}
				Utl.doCallback (initCallback, go.GetComponent<CLCellBase> (), list [i]);
			
				NGUITools.AddWidgetCollider (go);		//设置collider是为了得到元素的高度以便计算，同时也会让碰撞区适合元素大小
				bc = go.GetComponent (typeof(BoxCollider)) as  BoxCollider;
				pos = go.transform.localPosition;
				anchor += (bc.size.y + offsetY);
				pos.y = anchor;
				go.transform.localPosition = pos;
			}
			for (i = list.Count; i < cellObjCount; i++) {
#if UNITY_5_6_OR_NEWER
				go = grid.transform.Find (i.ToString ()).gameObject;
#else
				go = grid.transform.FindChild (i.ToString ()).gameObject;
#endif
				NGUITools.SetActive (go, false);
			}
		}

		public static void showConfirm (string msg, object callback)
		{
			showConfirm (msg, true, Localization.Get ("Okay"), callback, "", null);
		}

		public static void showConfirm (string msg, object callback1, object callback2)
		{
			showConfirm (msg, false, Localization.Get ("Okay"), callback1, Localization.Get ("Cancel"), callback2);
		}

		public static void showConfirm (string msg, bool isShowOneButton, string button1, 
		                                object callback1, string button2, object callback2)
		{
		
			CLPanelBase p = CLPanelManager.getPanel ("PanelConfirm");
			if (p == null) {
				return;
			}
			ArrayList list = new ArrayList ();
			list.Add (msg);
			list.Add (isShowOneButton);
			list.Add (button1);
			list.Add (callback1);
			list.Add (button2);
			list.Add (callback2);
			p.setData (list);
			CLPanelManager.showPanel (p);
		}

		/// <summary>
		/// Sets the sprite fit.设置ngui图片的原始大小
		/// </summary>
		/// <param name='sprite'>
		/// Sprite.
		/// </param>
		/// <param name='sprName'>
		/// Spr name.
		/// </param>
		public static void setSpriteFit (UISprite sprite, string sprName)
		{
            setSpriteFit(sprite, sprName, -1);
		}

		public static void setSpriteFit (UISprite sprite, string sprName, int maxSize)
		{
            if(sprite == null || sprite.atlas == null || string.IsNullOrEmpty(sprName)) {
                Debug.LogError("setSpriteFit is error!");
                return;
            }
			if (sprite.atlas.isBorrowSpriteMode) {
                if (sprite.atlas.getSpriteBorrowMode(sprName) != null)
                {
                    onGetSprite(sprite, sprName, maxSize, false);
                }
                else
                {
                    Callback cb = onGetSprite;
                    sprite.atlas.borrowSpriteByname(sprName, sprite, cb, maxSize);
                }
			} else {
				sprite.spriteName = sprName;
				UISpriteData sd = sprite.GetAtlasSprite ();
				if (sd == null) {
					return;
				}
				float x = (float)(sd.width);
				float y = (float)(sd.height);
				float size = x > y ? x : y;
				float rate = 1;
				if (size > maxSize) {
					rate = maxSize / size;
				}
//				      sprite.MakePixelPerfect();
				sprite.SetDimensions ((int)(sd.width * rate), (int)(sd.height * rate));
			}
		}

		public static void onGetSprite (params object[] paras)
		{
			UISprite sprite = (UISprite)(paras [0]);
			if (sprite == null || sprite.atlas == null) {
				return;
			}

            string sprName = paras [1].ToString ();
            sprite.spriteName = sprName;
            UISpriteData sd = sprite.atlas.getSpriteBorrowMode(sprite.spriteName);
            if (sd == null) return;
            int maxSize = NumEx.stringToInt (paras [2].ToString ());
            if (maxSize > 0)
            {
                if (sd == null)
                {
                    return;
                }
                float x = (float)(sd.width);
                float y = (float)(sd.height);
                float size = x > y ? x : y;
                float rate = 1;
                if (size > maxSize)
                {
                    rate = maxSize / size;
                }
                //		sprite.MakePixelPerfect();
                sprite.SetDimensions((int)(sd.width * rate), (int)(sd.height * rate));
            } else {
                sprite.SetDimensions(sd.width, sd.height);
            }
		}

		//设置所有图片是否灰色
		static public void setAllSpriteGray (GameObject gobj, bool isGray)
		{
			if (gobj == null) {
				return;
			}
			UISprite sprSelf = gobj.GetComponent<UISprite> ();
			setSpriteGray (sprSelf, isGray);

			UISprite[] sprs = gobj.GetComponentsInChildren<UISprite> ();
			if (sprs == null || sprs.Length == 0) {
				return;
			}
			int len = sprs.Length;
			for (int i = 0; i < len; i++) {
				setSpriteGray (sprs [i], isGray);
			}
		}

		static public void setSpriteGray (UISprite spr, bool isGray)
		{
			if (spr == null) {
				return;
			}
			if (isGray) {
				spr.setGray ();
			} else {
				spr.unSetGray ();
			}
		}

		public static void resetAtlasAndFont (Transform tr, bool isClean)
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying) {
				if (isClean) {
					CLUIInit.self.clean ();
				} else {
					CLUIInit.self.init ();
				}
			}
			#endif
			_resetAtlasAndFont (tr, isClean); 
		}
		public static void _resetAtlasAndFont (Transform tr, bool isClean)
		{
			UILabel lb = tr.GetComponent<UILabel> ();
			if (lb != null) {
				if (isClean) {
					lb.bitmapFont = null;
				} else {
					if (string.IsNullOrEmpty (lb.fontName)) {
						lb.bitmapFont = CLUIInit.self.emptFont;
					} else {
						lb.bitmapFont = CLUIInit.self.getFontByName (lb.fontName);//font;
					}
				}
			}

			HUDText hud = tr.GetComponent<HUDText> ();
			if (hud != null) {
				//			hud.font = font;
				if (isClean) {
					hud.font = null;
				} else {
					if (string.IsNullOrEmpty (hud.fontName)) {
						hud.font = CLUIInit.self.emptFont;
					} else {
						hud.font = CLUIInit.self.getFontByName (hud.fontName);//font;
					}
				}
			}

			UISprite sp = tr.GetComponent<UISprite> ();
			if (sp != null) {
				//			sp.atlas = atlas;
				if (isClean) {
					sp.atlas = null;
				} else {
					if (string.IsNullOrEmpty (sp.atlasName)) {
						sp.atlas = CLUIInit.self.emptAtlas;
					} else {
						sp.atlas = CLUIInit.self.getAtlasByName (sp.atlasName);
					}
				}
			}

			UIRichText4Chat rtc = tr.GetComponent<UIRichText4Chat> ();
			if (rtc != null) {
				if (isClean) {
					rtc.faceAtlas = null;
				} else {
					if (string.IsNullOrEmpty (rtc.atlasName)) {
						rtc.faceAtlas = CLUIInit.self.emptAtlas;
					} else {
						rtc.faceAtlas = CLUIInit.self.getAtlasByName (rtc.atlasName);
					}
				}
			}

			UIPopupList pop = tr.GetComponent<UIPopupList> ();
			if (pop != null) {
				//			pop.atlas = atlas;
				if (isClean) {
					pop.atlas = null;
				} else {
					if (string.IsNullOrEmpty (pop.atlasName)) {
						pop.atlas = CLUIInit.self.emptAtlas;
					} else {
						pop.atlas = CLUIInit.self.getAtlasByName (pop.atlasName);
					}
				}

				//			pop.bitmapFont = font;
				if (isClean) {
					pop.bitmapFont = null;
				} else {
					if (string.IsNullOrEmpty (pop.fontName)) {
						pop.bitmapFont = CLUIInit.self.emptFont;
					} else {
						pop.bitmapFont = CLUIInit.self.getFontByName (pop.fontName);//font;
					}
				}

				if (pop.bitmapFont == null) {
					pop.trueTypeFont = null;
				}
			}

			for (int i = 0; i < tr.childCount; i++) {
				_resetAtlasAndFont (tr.GetChild (i), isClean);
			}
		}
	}
}
