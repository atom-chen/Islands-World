/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   循环列表，只能是单排或单列
 * 1、脚本放在Scroll View下面的UIGrid的那个物体上 
 * 2、Scroll View上面的UIPanel的Cull勾选上
 * 3、每一个Item都放上一个CLUILoopTableCell，调整到合适的大小（用它的Visiable） ，设置Dimensions
 * 4、需要提前把Item放到Grid下，不能动态加载进去
 * 5、注意元素的个数要多出可视范围至少4个
 * 6、特别注意，因为使用的是ngui的Widget来处理排列的，
 * 		因此在编辑cell里要保证通过计算后widget可以全部覆盖完整个cell（可以使用anchor来处理）
 * 7、特别注意Table.CellAlignment要设置正确，不然可能单元的位置计算有会问题
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
	[RequireComponent (typeof(UITable))]
	public class CLUILoopTable : MonoBehaviour
	{
		public int cellCount = 10;
		public Vector2 mOffset = Vector2.zero;
		private List<CLUILoopTableCell> itemList = new List<CLUILoopTableCell> ();
		private Transform cachedTransform;
		UITable table = null;
	
		bool isFinishInit = false;
		int times = 0;
		int RealCellCount = 0;
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
						oldClipOffset = _scrollView.panel.clipOffset;
						_scrollView.panel.cullWhileDragging = true;
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
			table = this.GetComponent<UITable> ();
			table.hideInactive = true;
			table.sorting = UITable.Sorting.Alphabetic;
			oldGridPosition = table.transform.localPosition;
			_scrollView = scrollView;

			Transform prefab = cachedTransform.GetChild (0);
			CLUILoopTableCell prefabCell = prefab.GetComponent<CLUILoopTableCell> ();
			table.cellAlignment = prefabCell.widget.pivot;

			if (cachedTransform.childCount < cellCount) {
				for (int i = cachedTransform.childCount; i < cellCount; i++) {
					NGUITools.AddChild (gameObject, prefab.gameObject);
				}
			}
			
			for (int i = 0; i < cachedTransform.childCount; ++i) {
				Transform t = cachedTransform.GetChild (i);
				CLUILoopTableCell uiw = t.GetComponent<CLUILoopTableCell> ();
//				uiw.name = string.Format ("{0:D5}", itemList.Count);
				uiw.name = NumEx.nStrForLen (itemList.Count, 6);
				itemList.Add (uiw);
			}
			RealCellCount = itemList.Count;
			table.Reposition ();
			isFinishInit = true;		
			if (itemList.Count < 3) {
				Debug.LogError ("The childCount < 3");
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
			scrollView.panel.clipOffset = oldClipOffset;
			scrollView.transform.localPosition = oldScrollViewPos;
			table.transform.localPosition = oldGridPosition;
		}

		object data = null;
		ArrayList list = null;
		object initCellCallback;
		object onEndListCallback;

		public void refreshContentOnly ()
		{
			refreshContentOnly (list);
		}

        public void refreshContentOnly(object data){
            refreshContentOnly(data, false);
        }
        public void refreshContentOnly (object data, bool isRePositionTable)
		{
			ArrayList _list = null;
			if (data == null) {
				_list = new ArrayList ();
			} else if (data is LuaTable) {
				_list = CLUtlLua.luaTableVals2List ((LuaTable)data);
			} else if (data is ArrayList) {
				_list = (ArrayList)data;
			} else if (data is object[]) {
				_list = new ArrayList ();
				_list.AddRange ((object[])data);
			} else {
				_list = new ArrayList ();
			}
			list = _list;
			if (RealCellCount > itemList.Count) {
				setList (list, this.initCellCallback);
			} else {
				Transform t = null;
				int tmpIndex = 0;
                CLUILoopTableCell cell = null;
                CLUILoopTableCell preCell = null;
                //for (int i = 0; i < cachedTransform.childCount; ++i) {
                for (int i = 0; i < itemList.Count; i++){
                    //t = cachedTransform.GetChild (i);
                    cell = itemList[i];
					if (!cell.gameObject.activeSelf)
						continue;
					tmpIndex = int.Parse (cell.name);
					//cell = t.GetComponent<CLUILoopTableCell> ();
					if (cell != null) {
						Utl.doCallback (this.initCellCallback, cell, list [tmpIndex]);
					}

                    if (cell.isSetWidgetSize)
                    {
                        BoxCollider box = cell.GetComponent<BoxCollider>();
                        if (box != null)
                        {
                            box.size = Vector2.zero;
                        }
                        cell.widget.SetDimensions(0, 0);
                    }
                    NGUITools.updateAll(cell.transform);

                    if (cell.isSetWidgetSize)
                    {
                        Bounds bound = NGUIMath.CalculateRelativeWidgetBounds(t, false);
                        cell.widget.SetDimensions((int)(bound.size.x), (int)(bound.size.y));
                    }

                    if (isRePositionTable)
                    {
                        if (preCell != null)
                        {
                            setPosition(cell, preCell, table.direction);
                        }
                    }
                    preCell = cell;
				}
			}
		}

		public void setList (object data, object initCellCallback)
		{
			setList (data, initCellCallback, null);
		}

		public void setList (object data, object initCellCallback, object onEndListCallback)
		{
			setList (data, initCellCallback, onEndListCallback, true);
		}

		public void setList (object data, object initCellCallback, object onEndListCallback, bool isNeedRePosition)
		{
			_setList (data, initCellCallback, onEndListCallback, isNeedRePosition);
		}

		public void setList (object data, object initCellCallback, object onEndListCallback, bool isNeedRePosition, bool isCalculatePosition)
		{
			_setList (data, initCellCallback, onEndListCallback, isNeedRePosition, isCalculatePosition);
		}

		void _setList (object data, object initCellCallback, object onEndListCallback, bool isNeedRePosition, bool isCalculatePosition = false)
		{
			ArrayList _list = null;
			if (data == null) {
				_list = new ArrayList ();
			} else if (data is LuaTable) {
				_list = CLUtlLua.luaTableVals2List ((LuaTable)data);
			} else if (data is ArrayList) {
				_list = (ArrayList)data;
			} else if (data is object[]) {
				_list = new ArrayList ();
				_list.AddRange ((object[])data);
			} else {
				_list = new ArrayList ();
			}
			try {
				this.data = data;
				this.list = _list;
				this.initCellCallback = initCellCallback;
				this.onEndListCallback = onEndListCallback;
				if (!isFinishInit) {
					init ();
				}
				int tmpIndex = 0;
				times = 0;
				itemList.Clear ();

				for (int i = 0; i < cachedTransform.childCount; ++i) {
					Transform t = cachedTransform.GetChild (i);
					CLUILoopTableCell uiw = t.GetComponent<CLUILoopTableCell> ();
				
					tmpIndex = i;
//					uiw.name = string.Format ("{0:D5}", tmpIndex);
					uiw.name = NumEx.nStrForLen (tmpIndex, 6);
					if (tmpIndex >= 0 && tmpIndex < this.list.Count) {
						NGUITools.SetActive (t.gameObject, true);
						Utl.doCallback (this.initCellCallback, t.GetComponent<CLCellBase> (), list [tmpIndex]);

						if (uiw.isSetWidgetSize) {
							BoxCollider box = uiw.GetComponent<BoxCollider> ();
							if (box != null) {
								box.size = Vector2.zero;
							}
							uiw.widget.SetDimensions (0, 0);
						}
						NGUITools.updateAll (uiw.transform);

						if (uiw.isSetWidgetSize) {
							Bounds bound = NGUIMath.CalculateRelativeWidgetBounds (t, false);
							uiw.widget.SetDimensions ((int)(bound.size.x), (int)(bound.size.y));
						}
						if (!isNeedRePosition) {
							if (itemList.Count > 0) {
								CLUILoopTableCell targetUIW = itemList [itemList.Count - 1];
								setPosition (uiw, targetUIW, table.direction);
							}
						}
						itemList.Add (uiw);
					} else {
						NGUITools.SetActive (t.gameObject, false);
					}
				}
				if (isNeedRePosition) {
					resetClip ();
					table.Reposition ();
					if (scrollView != null) {
						scrollView.ResetPosition ();
					}
				}
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}

		public void insertList (object data, bool isNeedRePosition, bool isCalculatePosition)
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

			times = (int)(_list.Count / RealCellCount);
			//------------------------------------------------
			CLUILoopTableCell uiw = null;
			CLUILoopTableCell targetUIW = null;
			int newDataCount = _list.Count;
			Transform t = null;
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

			//------------------------------------------------
			if (RealCellCount > itemList.Count) {
				Debug.Log ("说明之前加载的数据没有占用完所有单元");
				//说明之前加载的数据没有占用完所有单元
				_insertList (_list, isCalculatePosition);

				if (isNeedRePosition) {
					table.Reposition ();
				}
			}
			//------------------------------------------------
			if (list != null) {
				_list.AddRange (list);
			}
			list = _list;
		}

		void _insertList (ArrayList list, bool isCalculatePosition)
		{
			if (list.Count == 0)
				return;
			int dataIndex = list.Count - 1;
//			int tmpIndex = itemList.Count;
			CLUILoopTableCell uiw = null;
			CLUILoopTableCell targetUIW = null;
			Transform t = null;
			for (int i = 0; i < cachedTransform.childCount; i++) {
				if (dataIndex < 0) {
					break;
				}
				t = cachedTransform.GetChild (i);
				if (t.gameObject.activeSelf)
					continue;
				uiw = t.GetComponent<CLUILoopTableCell> ();
				if (uiw == null)
					continue;

				//				uiw.name = string.Format ("{0:D5}", (tmpIndex + dataIndex));
				uiw.name = NumEx.nStrForLen (dataIndex, 6);
				NGUITools.SetActive (t.gameObject, true);
				Utl.doCallback (this.initCellCallback, t.GetComponent<CLCellBase> (), list [dataIndex]);

				if (uiw.isSetWidgetSize) {
					BoxCollider box = uiw.GetComponent<BoxCollider> ();
					if (box != null) {
						box.size = Vector2.zero;
					}
					uiw.widget.SetDimensions (0, 0);
				}
				NGUITools.updateAll (uiw.transform);

				if (uiw.isSetWidgetSize) {
					Bounds bound = NGUIMath.CalculateRelativeWidgetBounds (t, false);
					uiw.widget.SetDimensions ((int)(bound.size.x), (int)(bound.size.y));
				}

				if (isCalculatePosition) {
					if (itemList.Count > 0) {
						targetUIW = itemList [0];
						UITable.Direction dir = table.direction;
						if (table.direction == UITable.Direction.Up) {
							dir = UITable.Direction.Down;
						} else {
							dir = UITable.Direction.Up;
						}
						setPosition (uiw, targetUIW, dir);
					}
				}
				itemList.Insert (0, uiw);
				dataIndex--;
			}
		}


		public void appendList (object data, bool isNeedRePosition, bool isCalculatePosition)
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
			if (_list != null) {
				list.AddRange (_list);
			}
			if (RealCellCount > itemList.Count) {
				//说明之前加载的数据没有占用完所有单元
				_appendList (_list, isCalculatePosition);
				if (isNeedRePosition) {
					table.Reposition ();
				}
			}
		}

		void _appendList (ArrayList list, bool isCalculatePosition)
		{
			if (list.Count == 0)
				return;
			int dataIndex = 0;
			int tmpIndex = itemList.Count;
			CLUILoopTableCell uiw = null;
			CLUILoopTableCell targetUIW = null;
			Transform t = null;
			for (int i = 0; i < cachedTransform.childCount; i++) {
				if (dataIndex >= list.Count) {
					break;
				}
				t = cachedTransform.GetChild (i);
				if (t.gameObject.activeSelf)
					continue;
				uiw = t.GetComponent<CLUILoopTableCell> ();
				if (uiw == null)
					continue;
				
//				uiw.name = string.Format ("{0:D5}", (tmpIndex + dataIndex));
				uiw.name = NumEx.nStrForLen (tmpIndex + dataIndex, 6);
				NGUITools.SetActive (t.gameObject, true);
				Utl.doCallback (this.initCellCallback, t.GetComponent<CLCellBase> (), list [dataIndex]);

				if (uiw.isSetWidgetSize) {
					BoxCollider box = uiw.GetComponent<BoxCollider> ();
					if (box != null) {
						box.size = Vector2.zero;
					}
					uiw.widget.SetDimensions (0, 0);
				}
				NGUITools.updateAll (uiw.transform);

				if (uiw.isSetWidgetSize) {
					Bounds bound = NGUIMath.CalculateRelativeWidgetBounds (t, false);
					uiw.widget.SetDimensions ((int)(bound.size.x), (int)(bound.size.y));
				}
				if (isCalculatePosition) {
					if (itemList.Count > 0) {
						targetUIW = itemList [itemList.Count - 1];
						setPosition (uiw, targetUIW, table.direction);
					}
				}
				itemList.Add (uiw);
				dataIndex++;
			}
		}

		void setPosition (CLUILoopTableCell movement, CLUILoopTableCell target, UITable.Direction direction)
		{
			Vector3 v3Offset = Vector3.zero;
			int flag = 1;
			if (table.columns == 1) {
				if (direction == UITable.Direction.Down) {
					flag = -1;
					switch (table.cellAlignment) {
					case UIWidget.Pivot.Bottom:
						v3Offset = new Vector3 (0, flag * (movement.widget.height + mOffset.y + (table.padding.y) * 2), 0);
						break;
					case UIWidget.Pivot.BottomLeft:
						v3Offset = new Vector3 (0, flag * (movement.widget.height + mOffset.y + (table.padding.y) * 2), 0);
						break;
					case UIWidget.Pivot.BottomRight:
						v3Offset = new Vector3 (0, flag * (movement.widget.height + mOffset.y + (table.padding.y) * 2), 0);
						break;
					case UIWidget.Pivot.Center:
						v3Offset = new Vector3 (0, flag * ((movement.widget.height + target.widget.height) / 2.0f + mOffset.y + (table.padding.y) * 2), 0);
						break;
					case UIWidget.Pivot.Left:
						v3Offset = new Vector3 (0, flag * ((movement.widget.height + target.widget.height) / 2.0f + mOffset.y + (table.padding.y) * 2), 0);
						break;
					case UIWidget.Pivot.Right:
						v3Offset = new Vector3 (0, flag * ((movement.widget.height + target.widget.height) / 2.0f + mOffset.y + (table.padding.y) * 2), 0);
						break;
					case UIWidget.Pivot.Top:
						v3Offset = new Vector3 (0, flag * (target.widget.height + mOffset.y + (table.padding.y) * 2), 0);
						break;
					case UIWidget.Pivot.TopLeft:
						v3Offset = new Vector3 (0, flag * (target.widget.height + mOffset.y + (table.padding.y) * 2), 0);
						break;
					case UIWidget.Pivot.TopRight:
						v3Offset = new Vector3 (0, flag * (target.widget.height + mOffset.y + (table.padding.y) * 2), 0);
						break;
					}
				} else {
					flag = 1;
					switch (table.cellAlignment) {
					case UIWidget.Pivot.Bottom:
						v3Offset = new Vector3 (0, flag * (target.widget.height + mOffset.y + (table.padding.y) * 2), 0);
						break;
					case UIWidget.Pivot.BottomLeft:
						v3Offset = new Vector3 (0, flag * (target.widget.height + mOffset.y + (table.padding.y) * 2), 0);
						break;
					case UIWidget.Pivot.BottomRight:
						v3Offset = new Vector3 (0, flag * (target.widget.height + mOffset.y + (table.padding.y) * 2), 0);
						break;
					case UIWidget.Pivot.Center:
						v3Offset = new Vector3 (0, flag * ((movement.widget.height + target.widget.height) / 2.0f + mOffset.y + (table.padding.y) * 2), 0);
						break;
					case UIWidget.Pivot.Left:
						v3Offset = new Vector3 (0, flag * ((movement.widget.height + target.widget.height) / 2.0f + mOffset.y + (table.padding.y) * 2), 0);
						break;
					case UIWidget.Pivot.Right:
						v3Offset = new Vector3 (0, flag * ((movement.widget.height + target.widget.height) / 2.0f + mOffset.y + (table.padding.y) * 2), 0);
						break;
					case UIWidget.Pivot.Top:
						v3Offset = new Vector3 (0, flag * (movement.widget.height + mOffset.y + (table.padding.y) * 2), 0);
						break;
					case UIWidget.Pivot.TopLeft:
						v3Offset = new Vector3 (0, flag * (movement.widget.height + mOffset.y + (table.padding.y) * 2), 0);
						break;
					case UIWidget.Pivot.TopRight:
						v3Offset = new Vector3 (0, flag * (movement.widget.height + mOffset.y + (table.padding.y) * 2), 0);
						break;
					}
				}
			} else if (table.columns == 0) {
				flag = 1;
				if (direction == UITable.Direction.Down) {
					switch (table.cellAlignment) {
					case UIWidget.Pivot.Bottom:
						v3Offset = new Vector3 (flag * ((target.widget.width + movement.widget.width) / 2.0f + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					case UIWidget.Pivot.BottomLeft:
						v3Offset = new Vector3 (flag * (target.widget.width + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					case UIWidget.Pivot.BottomRight:
						v3Offset = new Vector3 (flag * (movement.widget.width + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					case UIWidget.Pivot.Center:
						v3Offset = new Vector3 (flag * ((target.widget.width + movement.widget.width) / 2.0f + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					case UIWidget.Pivot.Left:
						v3Offset = new Vector3 (flag * (target.widget.width + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					case UIWidget.Pivot.Right:
						v3Offset = new Vector3 (flag * (movement.widget.width + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					case UIWidget.Pivot.Top:
						v3Offset = new Vector3 (flag * ((target.widget.width + movement.widget.width) / 2.0f + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					case UIWidget.Pivot.TopLeft:
						v3Offset = new Vector3 (flag * (target.widget.width + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					case UIWidget.Pivot.TopRight:
						v3Offset = new Vector3 (flag * (movement.widget.width + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					}
				} else {
					flag = -1;
					switch (table.cellAlignment) {
					case UIWidget.Pivot.Bottom:
						v3Offset = new Vector3 (flag * ((target.widget.width + movement.widget.width) / 2.0f + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					case UIWidget.Pivot.BottomLeft:
						v3Offset = new Vector3 (flag * (movement.widget.width + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					case UIWidget.Pivot.BottomRight:
						v3Offset = new Vector3 (flag * (target.widget.width + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					case UIWidget.Pivot.Center:
						v3Offset = new Vector3 (flag * ((target.widget.width + movement.widget.width) / 2.0f + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					case UIWidget.Pivot.Left:
						v3Offset = new Vector3 (flag * (movement.widget.width + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					case UIWidget.Pivot.Right:
						v3Offset = new Vector3 (flag * (target.widget.width + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					case UIWidget.Pivot.Top:
						v3Offset = new Vector3 (flag * ((target.widget.width + movement.widget.width) / 2.0f + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					case UIWidget.Pivot.TopLeft:
						v3Offset = new Vector3 (flag * (movement.widget.width + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					case UIWidget.Pivot.TopRight:
						v3Offset = new Vector3 (flag * (target.widget.width + mOffset.x + (table.padding.x) * 2), 0, 0);
						break;
					}
				}
			} else {
				Debug.LogError ("The loop table cannot suport this setting! columes must in (0, 1)");
			}

			movement.transform.localPosition = target.transform.localPosition + v3Offset;
		}

		int sourceIndex = -1;
		int targetIndex = -1;
		bool forward = true;
		bool firstVislable = false;
		bool lastVisiable = false;
		CLUILoopTableCell head;
		CLUILoopTableCell tail;
		CLUILoopTableCell checkHead;
		CLUILoopTableCell checkTail;
		bool isCanCallOnEndList = true;
		CLUILoopTableCell movedWidget = null;
		CLUILoopTableCell targetWidget = null;
		BoxCollider _boxCollidr;
		Bounds _bound;
		Vector3 _offset;
		//	void LateUpdate ()
		void Update ()
		{
			if (itemList.Count < 3) {
				return;
			}
			sourceIndex = -1;
			targetIndex = -1;
//			sign = 0;
			head = itemList [0];
			tail = itemList [itemList.Count - 1];
			checkHead = itemList [1];
			checkTail = itemList [itemList.Count - 2];
			firstVislable = checkHead.widget.isVisible;
			lastVisiable = checkTail.widget.isVisible;
			
			// if first and last both visiable or invisiable then return	
			if (firstVislable == lastVisiable) {
				return;
			}
			
			if (firstVislable && int.Parse (head.name) > 0) {
				isCanCallOnEndList = true;
				times--;
				// move last to first one
				sourceIndex = itemList.Count - 1;
				targetIndex = 0;
				forward = false;
			} else if (lastVisiable && int.Parse (tail.name) < list.Count - 1) {
				isCanCallOnEndList = true;
				times++;
				// move first to last one
				sourceIndex = 0;
				targetIndex = itemList.Count - 1;
				forward = true;
			} else if (lastVisiable && int.Parse (tail.name) == list.Count - 1) {
				//说明已经到最后了
				if (isCanCallOnEndList) {
					isCanCallOnEndList = false;
					#if UNITY_EDITOR
					Debug.Log ("说明已经到最后了");
					#endif
					Utl.doCallback (this.onEndListCallback);
				}
			} else {
				isCanCallOnEndList = true;
			}
			if (sourceIndex > -1) {
				movedWidget = itemList [sourceIndex];
				if (forward) {
//					movedWidget.name = string.Format ("{0:D5}",);
					movedWidget.name = NumEx.nStrForLen (NumEx.stringToInt (tail.name) + 1, 6);
//					movedWidget.name = NumEx.nStrForLen( ((times - 1) / RealCellCount + 1) * RealCellCount + int.Parse (movedWidget.name) % RealCellCount, 6);
				} else {
					movedWidget.name = NumEx.nStrForLen (NumEx.stringToInt (head.name) - 1, 6);
//					movedWidget.name = string.Format ("{0:D5}", ((times) / RealCellCount) * RealCellCount + int.Parse (movedWidget.name) % RealCellCount);
//					movedWidget.name = NumEx.nStrForLen(((times) / RealCellCount) * RealCellCount + int.Parse (movedWidget.name) % RealCellCount, 6);
				}

				int index = int.Parse (movedWidget.name);
				Utl.doCallback (this.initCellCallback, movedWidget.GetComponent<CLCellBase> (), this.list [index]);

				// ***after init call, then set the position***

				if (movedWidget.isSetWidgetSize) {
					_boxCollidr = movedWidget.GetComponent<BoxCollider> ();
					if (_boxCollidr != null) {
						_boxCollidr.size = Vector2.zero;
					}
					movedWidget.widget.SetDimensions (0, 0);
				}
				NGUITools.updateAll (movedWidget.transform);

				if (movedWidget.isSetWidgetSize) {
					_bound = NGUIMath.CalculateRelativeWidgetBounds (movedWidget.transform, false);
					movedWidget.widget.SetDimensions ((int)(_bound.size.x), (int)(_bound.size.y));
				}

				targetWidget = itemList [targetIndex];
				if (forward) {
					setPosition (movedWidget, targetWidget, table.direction);
				} else {
					UITable.Direction dir = table.direction;
					if (table.direction == UITable.Direction.Up) {
						dir = UITable.Direction.Down;
					} else {
						dir = UITable.Direction.Up;
					}
					setPosition (movedWidget, targetWidget, dir);
				}
				
				// change item index
				itemList.RemoveAt (sourceIndex);
				itemList.Insert (targetIndex, movedWidget);
			}
		}
	}
}