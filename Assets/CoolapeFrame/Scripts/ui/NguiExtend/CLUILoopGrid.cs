/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   循环列表，只能是单排或单列
 * 1、脚本放在Scroll View下面的UIGrid的那个物体上 
 * 2、Scroll View上面的UIPanel的Cull勾选上
 * 3、每一个Item都放上一个UIWidget，调整到合适的大小（用它的Visiable） ，设置Dimensions
 * 4、需要提前把Item放到Grid下，不能动态加载进去
 * 5、注意元素的个数要多出可视范围至少4个
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;

namespace Coolape
{
	[RequireComponent (typeof(UIGrid))]
	public class CLUILoopGrid : MonoBehaviour
	{
		public int cellCount = 10;
		[HideInInspector]
		public bool
			isPlayTween = true;
		public TweenType twType = TweenType.position;
		public  float tweenSpeed = 0.01f;
		public  float twDuration = 0.5f;
		public UITweener.Method twMethod = UITweener.Method.EaseOut;
		public List<UIWidget> itemList = new List<UIWidget> ();
		private Vector4 posParam;
		private Transform cachedTransform;
		public UIGrid grid = null;
		public UIPanel panel;
		//	void Awake ()
		//	{
		//		cachedTransform = this.transform;
		//		grid = this.GetComponent<UIGrid> ();
		//		float cellWidth = grid.cellWidth;
		//		float cellHeight = grid.cellHeight;
		//		posParam = new Vector4 (cellWidth, cellHeight,
		//			grid.arrangement == UIGrid.Arrangement.Horizontal ? 1 : 0,
		//			grid.arrangement == UIGrid.Arrangement.Vertical ? 1 : 0);
		//	}
	
		bool isFinishInit = false;
		int times = 0;
		int RealCellCount = 0;
		int wrapLineNum = 0;
		Vector3 oldGridPosition = Vector3.zero;
		Vector3 oldScrollViewPos = Vector3.zero;
		Vector2 oldClipOffset = Vector2.zero;
		UIScrollView _scrollView;

		public UIScrollView scrollView {
			get {
				if (_scrollView == null) {
					_scrollView = NGUITools.FindInParents<UIScrollView> (transform);
					if (_scrollView != null) {
						oldScrollViewPos = _scrollView.transform.localPosition;
						panel = _scrollView.panel;
						if(panel == null) {
							panel = _scrollView.GetComponent<UIPanel>();
						}
						if (panel != null) {
							oldClipOffset = panel.clipOffset;
						}
						panel.cullWhileDragging = true;
					}
				}
				return _scrollView;
			}
		}

		public void init ()
		{
			if (isFinishInit)
				return;

			cachedTransform = this.transform;
			grid = this.GetComponent<UIGrid> ();
			grid.sorting = UIGrid.Sorting.Alphabetic;
			grid.hideInactive = true;
			oldGridPosition = grid.transform.localPosition;
			_scrollView = scrollView;

			Transform prefab = cachedTransform.GetChild (0);
			if (cachedTransform.childCount < cellCount) {
				for (int i = cachedTransform.childCount; i < cellCount; i++) {
					NGUITools.AddChild (gameObject, prefab.gameObject);
				}
			}

			float cellWidth = grid.cellWidth;
			float cellHeight = grid.cellHeight;
			posParam = new Vector4 (cellWidth, cellHeight, 
				grid.arrangement == UIGrid.Arrangement.Horizontal ? -1 : 0,
				grid.arrangement == UIGrid.Arrangement.Vertical ? 1 : 0);
			
			wrapLineNum = grid.maxPerLine;
			if (wrapLineNum > 0) {
				posParam.z = -1;
				posParam.w = 1;
			}
		
			for (int i = 0; i < cachedTransform.childCount; ++i) {
				Transform t = cachedTransform.GetChild (i);
				UIWidget uiw = t.GetComponent<UIWidget> ();
//				uiw.name = string.Format("{0:D5}", itemList.Count);
				uiw.name = NumEx.nStrForLen (itemList.Count, 6);
				itemList.Add (uiw);
			}
			RealCellCount = itemList.Count;
			grid.Reposition ();
			isFinishInit = true;		
			if (itemList.Count < 3) {
				Debug.Log ("The childCount < 3");
			}
		}

		public void setOldClip (Vector2 oldClipOffset, Vector3 oldScrollViewPos, Vector3 oldGridPosition)
		{
			this.oldClipOffset = oldClipOffset;
			this.oldScrollViewPos = oldScrollViewPos;
			this.oldGridPosition = oldGridPosition;
		}

		public void resetClip ()
		{
			if (scrollView != null) {
				scrollView.ResetPosition ();	
				if (panel != null) {
					panel.clipOffset = oldClipOffset;
				}
				scrollView.transform.localPosition = oldScrollViewPos;
			}
			grid.transform.localPosition = oldGridPosition;
		}

		object data = null;
		public ArrayList list = null;
		object initCellCallback;
		object onEndListCallback;
		object onHeadListCallback;

		public void refreshContentOnly ()
		{
			refreshContentOnly (list);
		}

		public void refreshContentOnly (object data)
		{
			refreshContentOnly (data, true);
		}

		public void refreshContentOnly (object data, bool UpdatePosition)
		{
			list = wrapList (data);
			UIWidget t = null;
			int tmpIndex = 0;
			CLCellBase cell = null;
			int maxIndex = -1;
			bool isActivedAllItems = true;
			bool needScrollReposiont = false;
			for (int i = 0; i < itemList.Count; ++i) {
				t = itemList [i];
				if (!t.gameObject.activeSelf) {
					isActivedAllItems = false;
					continue;
				}
				tmpIndex = int.Parse (t.name);
				maxIndex = (maxIndex < tmpIndex) ? tmpIndex : maxIndex;
				cell = t.GetComponent<CLCellBase> ();
				if (cell != null) {
					if (tmpIndex >= list.Count) {
						NGUITools.SetActive (cell.gameObject, false);
						needScrollReposiont = true;
					} else {
						NGUITools.SetActive (cell.gameObject, true);
						Utl.doCallback (this.initCellCallback, cell, list [tmpIndex]);
					}
				}
			}

			if (maxIndex < list.Count && !isActivedAllItems) {
				tmpIndex = maxIndex;
				for (int i = 0; i < itemList.Count; ++i) {
					t = itemList [i];
					if (!t.gameObject.activeSelf) {
						tmpIndex++;
						if (tmpIndex < list.Count) {
							cell = t.GetComponent<CLCellBase> ();
							if (cell != null) {
								cell.name = NumEx.nStrForLen (tmpIndex, 6);
								NGUITools.SetActive (cell.gameObject, true);
								Utl.doCallback (this.initCellCallback, cell, list [tmpIndex]);
							}
						}
					}
				}
//				grid.Reposition ();
//
//				if (scrollView != null) {
//					scrollView.ResetPosition ();
//				}
			}
			if (UpdatePosition && scrollView != null) {
				scrollView.RestrictWithinBounds (true);
				scrollView.UpdateScrollbars ();
				scrollView.UpdatePosition ();
			}
//			if (needScrollReposiont && scrollView != null) {
//				grid.Reposition ();
//				scrollView.ResetPosition ();
//			}
		}

		ArrayList wrapList (object data)
		{
			ArrayList _list = null;
			if (data is LuaTable) {
				_list = CLUtlLua.luaTableVals2List ((LuaTable)data);
			} else if (data is ArrayList) {
				_list = (ArrayList)data;
			} else if (data is object[]) {
				_list = new ArrayList ();
				_list.AddRange ((object[])data);
			}
			if (_list == null) {
				_list = new ArrayList ();
			}
			return _list;
		}

		public void insertList (object data)
		{
			ArrayList _list = null;
			_list = wrapList (data);

			UIWidget uiw = null;
			int newDataCount = _list.Count;
			int _startIndex = 0;
			if (itemList != null && itemList.Count > 0) {
				_startIndex = NumEx.stringToInt (itemList [0].name);
				for (int i = 0; i < itemList.Count; i++) {
					uiw = itemList [i];
					if (uiw == null)
						continue;
					uiw.name = NumEx.nStrForLen (newDataCount + _startIndex + i, 6);	
				}
			}

			if (list != null) {
				_list.AddRange (list);
			}
			list = _list;
			refreshContentOnly ();
		}

		public void setListData (object data, object initCellCallback, bool isFirst)
		{
			if (isFirst) {
				setList (data, initCellCallback, null, null, true);
			} else if (data != null) {
				refreshContentOnly (data);
			} else {
				refreshContentOnly ();
			}
		}

		public void setList (object data, object initCellCallback)
		{
			setList (data, initCellCallback, null, null, true);
		}

		public void setList (object data, object initCellCallback, object onEndListCallback)
		{
			setList (data, initCellCallback, null, onEndListCallback, true);
		}

		public void setList (object data, object initCellCallback, object onHeadListCallback, object onEndListCallback)
		{
			setList (data, initCellCallback, onHeadListCallback, onEndListCallback, true);
		}

		public void setList (object data, object initCellCallback, object onHeadListCallback, object onEndListCallback, bool isNeedRePosition)
		{
			setList (data, initCellCallback, onHeadListCallback, onEndListCallback, isNeedRePosition, isPlayTween);
		}

		public void setList (object data, object initCellCallback, object onHeadListCallback, object onEndListCallback, bool isNeedRePosition, bool isPlayTween)
		{
			setList (data, initCellCallback, onHeadListCallback, onEndListCallback, isNeedRePosition, isPlayTween, tweenSpeed);
		}

		public void setList (object data, object initCellCallback, object onHeadListCallback, object onEndListCallback, bool isNeedRePosition, bool isPlayTween, 
		                     float tweenSpeed)
		{
			setList (data, initCellCallback, onHeadListCallback, onEndListCallback, isNeedRePosition, isPlayTween, tweenSpeed, twDuration);
		}

		public void setList (object data, object initCellCallback, object onHeadListCallback, object onEndListCallback, bool isNeedRePosition, bool isPlayTween, 
		                     float tweenSpeed, float twDuration)
		{
			setList (data, initCellCallback, onHeadListCallback, onEndListCallback, isNeedRePosition, isPlayTween, tweenSpeed, twDuration, twMethod);
		}

		public void setList (object data, object initCellCallback, object onHeadListCallback, object onEndListCallback, bool isNeedRePosition, bool isPlayTween, 
		                     float tweenSpeed, float twDuration, UITweener.Method twMethod)
		{
			setList (data, initCellCallback, onHeadListCallback, onEndListCallback, isNeedRePosition, isPlayTween, tweenSpeed, twDuration, twMethod, twType);
		}

		public void appendList (object data)
		{
			ArrayList _list = wrapList (data);
			if (_list != null) {
				if (list == null) {
					list = new ArrayList ();
				}
				list.AddRange (_list);
			}
//			if (itemList.Count < RealCellCount) {
//				_appendList (_list);
//				grid.Reposition ();
//			}
			refreshContentOnly ();
		}

		void _appendList (ArrayList list)
		{
			if (list.Count == 0)
				return;
			int dataIndex = 0;
			int tmpIndex = itemList.Count;
			UIWidget uiw = null;
			Transform t = null;
			for (int i = 0; i < cachedTransform.childCount; i++) {
				if (dataIndex >= list.Count) {
					break;
				}
				t = cachedTransform.GetChild (i);
				if (t.gameObject.activeSelf)
					continue;
				uiw = t.GetComponent<UIWidget> ();
				if (uiw == null)
					continue;
				uiw.name = NumEx.nStrForLen (tmpIndex + dataIndex, 6);
				NGUITools.SetActive (t.gameObject, true);
				Utl.doCallback (this.initCellCallback, t.GetComponent<CLCellBase> (), list [dataIndex]);
				NGUITools.updateAll (t);
//				itemList.Add (uiw);
				dataIndex++;
			}
		}

		public void setList (object data, object initCellCallback, object onHeadListCallback, object onEndListCallback, bool isNeedRePosition, bool isPlayTween, 
		                     float tweenSpeed, float twDuration, UITweener.Method twMethod, TweenType twType)
		{
			_setList (data, initCellCallback, onHeadListCallback, onEndListCallback, isNeedRePosition, isPlayTween, tweenSpeed, twDuration, twMethod, twType);
		}

		void _setList (object data, object initCellCallback, object onHeadListCallback, object onEndListCallback, bool isNeedRePosition, bool isPlayTween, float tweenSpeed, 
		               float twDuration, UITweener.Method twMethod, TweenType twType)
		{
			try {
				this.data = data;
				this.list = wrapList (data);
				this.initCellCallback = initCellCallback;
				this.onEndListCallback = onEndListCallback;
				this.onHeadListCallback = onHeadListCallback;
				if (!isFinishInit) {
					init ();
				} else {
					for (int i = 0; i < itemList.Count; i++) {
						itemList [i].name = NumEx.nStrForLen (i, 6);
					}
				}
				int tmpIndex = 0;
				times = 0;
//				itemList.Clear ();

				for (int i = 0; i < itemList.Count; i++) {
//					Transform t = cachedTransform.GetChild (i);
					UIWidget uiw = itemList [i];
					tmpIndex = i;
					uiw.name = NumEx.nStrForLen (tmpIndex, 6);
					if (tmpIndex >= 0 && tmpIndex < this.list.Count) {
						NGUITools.SetActive (uiw.gameObject, true);
						Utl.doCallback (this.initCellCallback, uiw.GetComponent<CLCellBase> (), list [tmpIndex]);
						NGUITools.updateAll (uiw.transform);
//						itemList.Add (uiw);
					} else {
						NGUITools.SetActive (uiw.gameObject, false);
					}
				}
				if (isNeedRePosition) {
					resetClip ();
					if (!isPlayTween || twType == TweenType.alpha || twType == TweenType.scale) {
						grid.Reposition ();
//						scrollView.ResetPosition();	
					}
					if (isPlayTween) {
						for (int i = 0; i < itemList.Count; i++) {
							CLUIUtl.resetCellTween (i, grid, itemList [i].gameObject, tweenSpeed, twDuration, twMethod, twType);
						}
					}
				}

				isCanCallOnEndList = true;
				isCanCallOnHeadList = true;
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		int sourceIndex = -1;
		int targetIndex = -1;
		int sign = 0;
		bool firstVislable = false;
		bool lastVisiable = false;
		UIWidget head;
		UIWidget tail;
		UIWidget checkHead;
		UIWidget checkTail;
		bool isCanCallOnEndList = true;
		bool isCanCallOnHeadList = true;
		int checkLineSize = 1;
		int tmpIndex = 0;
	
		//	void LateUpdate ()
		public void Update ()
		{
			if (!isFinishInit || itemList.Count < 3) {
				return;
			}
			sourceIndex = -1;
			targetIndex = -1;
			sign = 0;
			head = itemList [0];
			tail = itemList [itemList.Count - 1];
			checkHead = itemList [wrapLineNum * checkLineSize];
			if (list.Count > 0 && int.Parse (tail.name) > list.Count) {
				tail = transform.Find (NumEx.nStrForLen (list.Count - 1, 6)).GetComponent<UIWidget> ();// itemList [list.Count - 1];
				tmpIndex = itemList.IndexOf (tail) - (wrapLineNum * checkLineSize);
				tmpIndex = tmpIndex < 0 ? 0 : tmpIndex;
				checkTail = itemList [tmpIndex];
			} else {
				tail = itemList [itemList.Count - 1];
				checkTail = itemList [itemList.Count - 1 - (wrapLineNum * checkLineSize)];
			}
			firstVislable = checkHead.isVisible;
			lastVisiable = checkTail.isVisible;
			
			// if first and last both visiable or invisiable then return	
//			if (firstVislable == lastVisiable) {
//				return;
//			}

//			Debug.Log (int.Parse (head.name) + "=11===" + (list.Count - 1) + "===" + firstVislable);
//			Debug.Log (int.Parse (tail.name) + "=22===" + (list.Count - 1) + "===" + lastVisiable);
			if (firstVislable && int.Parse (head.name) > 0) {
				isCanCallOnEndList = true;
				isCanCallOnHeadList = true;
				times--;
				// move last to first one
				sourceIndex = itemList.Count - 1;
				targetIndex = 0;
				sign = 1;
			} else if (lastVisiable && int.Parse (tail.name) < list.Count - 1) {
				isCanCallOnEndList = true;
				isCanCallOnHeadList = true;
				times++;
				// move first to last one
				sourceIndex = 0;
				targetIndex = itemList.Count - 1;
				sign = -1;
			} else {
				if (firstVislable && int.Parse (head.name) == 0) {
					if (isCanCallOnHeadList) {
						isCanCallOnHeadList = false;

						Utl.doCallback (this.onHeadListCallback);
					}
				} else {
					isCanCallOnHeadList = true;
				}
				if (lastVisiable && int.Parse (tail.name) == list.Count - 1) {
					//说明已经到最后了
					if (isCanCallOnEndList) {
						isCanCallOnEndList = false;

						Utl.doCallback (this.onEndListCallback);
					}
				} else {
					isCanCallOnEndList = true;
				}
			}
			if (sourceIndex > -1) {
				int lineNum = wrapLineNum <= 0 ? 1 : wrapLineNum;
				for (int j = 0; j < lineNum; j++) {
					UIWidget movedWidget = itemList [sourceIndex];

					int oldIndex = int.Parse (movedWidget.name);
					int newIndex = 0;
					if (sign < 0) {
						newIndex = oldIndex + RealCellCount;
					} else {
						newIndex = oldIndex - RealCellCount;
					}
					movedWidget.name = NumEx.nStrForLen (newIndex, 6);
					moveCellPos (movedWidget, itemList [targetIndex], newIndex, newIndex + sign);

					itemList.RemoveAt (sourceIndex);
					itemList.Insert (targetIndex, movedWidget);
					if (newIndex >= list.Count) {
						NGUITools.SetActive (movedWidget.gameObject, false);
					} else {
						NGUITools.SetActive (movedWidget.gameObject, true);
						Utl.doCallback (this.initCellCallback, movedWidget.GetComponent<CLCellBase> (), this.list [newIndex]);
					}
				}
			}
		}
		// 从原顺序位置移动到指定位置
		void moveCellPos (UIWidget moved, UIWidget target, int newIdx, int targetIdx)
		{
			int offx = 0;
			int offy = 0;
			if (wrapLineNum > 0) {
				if (grid.arrangement == UIGrid.Arrangement.Vertical) {
					offx = (targetIdx / wrapLineNum) - (newIdx / wrapLineNum);
					offy = (targetIdx % wrapLineNum) - (newIdx % wrapLineNum);
				} else {
					offx = (targetIdx % wrapLineNum) - (newIdx % wrapLineNum);
					offy = (targetIdx / wrapLineNum) - (newIdx / wrapLineNum);
				}
			} else {
				offx = targetIdx - newIdx;
				offy = offx;
			}
			Vector3 offset = new Vector3 (offx * posParam.x * posParam.z, offy * posParam.y * posParam.w, 0);
			moved.cachedTransform.localPosition = target.cachedTransform.localPosition + offset;
		}
	}
}