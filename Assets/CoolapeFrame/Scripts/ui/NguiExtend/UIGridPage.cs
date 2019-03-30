/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  wangkaiyuan
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   滑动页面，比如可以用在关卡地图页面
  *						注意使用时，每个page都要继承UIDragPageContents(如果是lua时可以直接使用UIDragPage4Lua)
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;
using XLua;

namespace Coolape
{
	[ExecuteInEditMode]
	[RequireComponent (typeof(UIMoveToCell))]
	public class UIGridPage : UIGrid
	{
		int cacheNum = 3;

		public bool isLimitless = false;
		//缓存数
		public bool isReverse = false;
		public float dragSensitivity = 10f;
		//拖动敏感度
		public UIDragPageContents page1;
		public UIDragPageContents page2;
		public UIDragPageContents page3;
		[HideInInspector]
		public int currCell = 0;
		int oldCurrCell = -1;
		UIMoveToCell _moveToCell;
		public LoopPage currPage;
		LoopPage pages;
		bool canDrag = true;
		int flag = 1;
		UIScrollView _scrollView;

		public UIScrollView scrollView {
			get {
				if (_scrollView == null) {
					_scrollView = GetComponent<UIScrollView> ();
					if (_scrollView == null) {
						_scrollView = transform.parent.GetComponent<UIScrollView> ();
					}
				}
				return _scrollView;
			}
		}

		public class LoopPage
		{
			public UIDragPageContents data;
			public LoopPage prev;
			public LoopPage next;
		}

		bool isFinishInit = false;

		public override void Start ()
		{
			if (isFinishInit) {
				return;
			}
			base.Start ();
			_init ();
		}

		void _init ()
		{
			if (isFinishInit) {
				return;
			}
			isFinishInit = true;
			dragSensitivity = dragSensitivity <= 0 ? 1 : dragSensitivity;
			pages = new LoopPage ();
			pages.data = page1;
			pages.next = new LoopPage ();
			pages.next.data = page2;
			pages.next.next = new LoopPage ();
			pages.next.next.data = page3;
			pages.next.next.next = pages;
		
			pages.prev = pages.next.next;
			pages.next.prev = pages;
			pages.next.next.prev = pages.next;
			currPage = pages;
		}

		public UIMoveToCell moveToCell {
			get {
				if (_moveToCell == null) {
					_moveToCell = GetComponent<UIMoveToCell> ();
				}
				return _moveToCell;
			}
		}

		object data;
		object[] dataList;
		object onRefreshCurrentPage;

		/// <summary>
		/// Init the specified pageList, initPage, initCell and defalt.初始化滑动页面
		/// </summary>
		/// <param name="pageList">Page list.
		/// 数据：是list里面套list，外层list是页面的数据，内层的数据是每个页面里面的数据
		/// </param>
		/// <param name="onRefreshCurrentPage">onRefreshCurrentPage.
		/// 当显示当前page时的回调
		/// </param>
		/// <param name="defaltPage">defaltPage.
		/// 初始化时默认页（0表示第一页，1表示第2页）
		/// </param>
		public void init (object pageList, object onRefreshCurrentPage, int defaltPage)
		{
			if (pageList == null) {
				Debug.LogError ("Data is null");
				return;
			}
			_initList (pageList, onRefreshCurrentPage, defaltPage);
		}

		void _initList (object data, object onRefreshCurrentPage, int defaltPage)
		{
			Start ();
			canDrag = true;
			if (isReverse) {
				flag = -1;
			}

			object[] list = null;
			if (data is LuaTable) {
				ArrayList _list = CLUtlLua.luaTableVals2List ((LuaTable)data);
				list = _list.ToArray ();
				_list.Clear ();
				_list = null;
			} else if (data is ArrayList) {
				list = ((ArrayList)data).ToArray ();
			}

			this.data = data;
			this.dataList = list;

			this.onRefreshCurrentPage = onRefreshCurrentPage;
			if (defaltPage >= list.Length || defaltPage < 0) {
				return;
			}
			currCell = defaltPage;
			initCellPos (currCell);
		}

		void initCellPos (int index)
		{
			NGUITools.SetActive (page1.gameObject, true);
			NGUITools.SetActive (page2.gameObject, true);
			NGUITools.SetActive (page3.gameObject, true);
				
			Update ();
			repositionNow = true;
			Update ();
		
			currPage = pages;
				
			int pageCount = dataList.Length;
			if (pageCount <= 3) {
				#region
				//此段处理是为了让ngpc这种三个页面显示的数据不同的那种，让三个页面的位置固定
				switch (currCell) { 
				case 0:
					currPage = pages;
					NGUITools.SetActive (page1.gameObject, true);
					break;
				case 1:
					currPage = pages.next;
					break;
				case 2:
					currPage = pages.prev;
					break;
				default:
					break;
				}
			
				switch (pageCount) {
				case 1:
					NGUITools.SetActive (page1.gameObject, true);
					NGUITools.SetActive (page2.gameObject, false);
					NGUITools.SetActive (page3.gameObject, false);
					break;
				case 2:
					NGUITools.SetActive (page1.gameObject, true);
					NGUITools.SetActive (page2.gameObject, true);
					NGUITools.SetActive (page3.gameObject, false);
					break;
				case 3:
					NGUITools.SetActive (page1.gameObject, true);
					NGUITools.SetActive (page2.gameObject, true);
					NGUITools.SetActive (page3.gameObject, true);
					break;
				default:
					NGUITools.SetActive (page1.gameObject, false);
					NGUITools.SetActive (page2.gameObject, false);
					NGUITools.SetActive (page3.gameObject, false);
					break;
				}
				#endregion
			} else {
				Vector3 toPos = Vector3.zero;
				if (arrangement == UIGrid.Arrangement.Horizontal) {
					toPos.x = flag * cellWidth * (index);
					currPage.data.transform.localPosition = toPos;
					toPos.x = flag * cellWidth * (index - 1);
					currPage.prev.data.transform.localPosition = toPos;
					toPos.x = flag * cellWidth * (index + 1);
					currPage.next.data.transform.localPosition = toPos;
				} else {
					toPos.y = -flag * cellHeight * index;
					currPage.data.transform.localPosition = toPos;
					toPos.y = -flag * cellHeight * (index - 1);
					currPage.prev.data.transform.localPosition = toPos;
					toPos.y = -flag * cellHeight * (index + 1);
					currPage.next.data.transform.localPosition = toPos;
				}
			}
			oldCurrCell = currCell;
			moveToCell.moveTo (index, isReverse, false);
		
			//刷新数据
			if (currCell <= 0) {
				NGUITools.SetActive (currPage.prev.data.gameObject, false);
			}

			if (currCell - 1 >= 0 && currCell - 1 < pageCount) {
				currPage.prev.data.init (dataList [currCell - 1], currCell - 1);//刷新数据
			} else {
				currPage.prev.data.init (null, currCell - 1);//刷新数据
			}
			if (currCell + 1 < pageCount && (currCell + 1) >= 0) {
				currPage.next.data.init (dataList [currCell + 1], currCell + 1);//刷新数据
			} else {
				currPage.next.data.init (null, currCell + 1);//刷新数据
			}

			if(dataList.Length > currCell && currCell >= 0) { 
				currPage.data.refreshCurrent (currCell, dataList [currCell]);//刷新数据(放在最后)
				doCallback (dataList [currCell]);
			} else {
				currPage.data.refreshCurrent (currCell, null);//刷新数据(放在最后)
				doCallback (null);
			}
		}

		void onFinishMoveto (params object[] paras)
		{
			canDrag = true;
		}

		void doCallback (object data)
		{
			Utl.doCallback (onRefreshCurrentPage, currCell, data, currPage.data);
		}


		public void moveTo (bool force = false)
		{
			canDrag = false;
			Callback cb = onFinishMoveto;
			moveToCell.moveTo (currCell, isReverse, false, cb);
			if (oldCurrCell != currCell || force) {
				int diff = currCell - oldCurrCell;
				int absDiff = Mathf.Abs (diff);
				int _flag = diff / absDiff;
				for (int i = 0; i < absDiff; i++) {
					resetCell (force);
					oldCurrCell += _flag;
				}
				oldCurrCell = currCell;

				//刷新数据==================
				int pageCount = dataList.Length;
				if (currCell - 1 >= 0 && currCell - 1 < pageCount) {
					currPage.prev.data.init (dataList [currCell - 1], currCell - 1);//刷新数据
				} else {
					currPage.prev.data.init (null, currCell - 1);//刷新数据
				}
				if (currCell + 1 < pageCount && (currCell+1) >= 0) {
					currPage.next.data.init (dataList [currCell + 1], currCell + 1);//刷新数据
				} else {
					currPage.next.data.init (null, currCell + 1);//刷新数据
				}
				if(dataList.Length > currCell && currCell >= 0) { 
					currPage.data.refreshCurrent (currCell, dataList [currCell]);//刷新数据(放在最后)
					doCallback (dataList [currCell]);
				} else {
					currPage.data.refreshCurrent (currCell, null);//刷新数据(放在最后)
					doCallback (null);
				}
			}
		}

		void resetCell (bool isForce)
		{
			if (currCell > 0) {
				NGUITools.SetActive (currPage.prev.data.gameObject, true);
			}
			//处理边界
			int pageCount = dataList.Length;

			//移动位置
			UIDragPageContents cell;
			Vector3 toPos = Vector3.zero;
			if (oldCurrCell < currCell) {
				cell = currPage.prev.data;
				if (arrangement == UIGrid.Arrangement.Horizontal) {
					toPos = currPage.data.transform.localPosition;
					toPos.x += flag * cellWidth * 2;
				} else {
					toPos = currPage.data.transform.localPosition;
					toPos.y -= flag * cellHeight * 2;
				}
			} else {
				cell = currPage.next.data;
				if (arrangement == UIGrid.Arrangement.Horizontal) {
					toPos = currPage.data.transform.localPosition;
					toPos.x -= flag * cellWidth * 2;
				} else {
					toPos = currPage.data.transform.localPosition;
					toPos.y += flag * cellHeight * 2;
				}
			}
			cell.transform.localPosition = toPos;
			if (isLimitless || (!isLimitless && (oldCurrCell != -1 || currCell != 0))) {
				if (oldCurrCell < currCell) {
					currPage = currPage.next;
				} else {
					currPage = currPage.prev;
				}
			}
		}

		public void onPress (bool isPressed)
		{
			//===============
			if (!isPressed) {
				if (canDrag) {
					procMoveCell ();
				}
			} else {
				totalDelta = Vector2.zero;
			}
		}

		Vector2 totalDelta = Vector2.zero;

		public void onDrag (Vector2 delta)
		{
			totalDelta += delta;
		}
	
		//处理移动单元
		public void procMoveCell ()
		{
			int index = currCell;
			if (dataList == null || dataList.Length <= 0) {
				return;
			}
			float delta = 0;

			float sensitivity = dragSensitivity <= 0 ? 1 : dragSensitivity;
			if (arrangement == Arrangement.Horizontal) {
				delta = totalDelta.x;
				if (Mathf.Abs (delta) >= cellWidth / dragSensitivity) {
					if (flag * delta > 0) {
						index--;
					} else {
						index++;
					}
				}
			} else {
				delta = totalDelta.y;
				if (Mathf.Abs (delta) >= cellHeight / dragSensitivity) {
					if (flag * delta > 0) {
						index++;
					} else {
						index--;
					}
				}
			}
			if (scrollView.dragEffect == UIScrollView.DragEffect.Momentum) {
				if ((index < 0 || index >= dataList.Length) && !isLimitless) {
					return;
				}
			}
			moveTo (index);
		}

		public void moveTo (int index)
		{
			currCell = index;
			if(!isLimitless) {
				currCell = currCell < 0 ? 0 : currCell;
				currCell = currCell >= dataList.Length ? dataList.Length - 1 : currCell;
			}
			moveTo ();
		}
	}
}
