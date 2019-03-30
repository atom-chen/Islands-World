/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  移动列表显示指定的单元
  *						注意：将该脚本绑在grid层
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class UIMoveToCell : MonoBehaviour
	{
		public float speed = 1.5f;
		public AnimationCurve moveCurve = new AnimationCurve (new Keyframe (0, 0), new Keyframe (1, 1));
		Vector3 fromListPos = Vector3.zero;
		//	Vector4 fromClipRage = Vector4.zero;
		Vector2 fromOffset = Vector2.zero;
		Vector3 toListPos = Vector3.zero;
		//	Vector4 toClipRage = Vector4.zero;
		Vector2 toOffset = Vector2.zero;
		Vector3 diffListPos = Vector3.zero;
		Vector2 diffOffset = Vector4.zero;
		bool isMoveToNow = false;
		object finishCallback;
		UIGrid _grid;

		UIGrid grid {
			get {
				if (_grid == null) {
					_grid = GetComponent<UIGrid> ();
				}
				return _grid;
			}
		}

		UIPanel _panelList;

		UIPanel panelList {
			get {
				if (_panelList == null && grid != null) {
					_panelList = grid.transform.parent.GetComponent<UIPanel> ();
				}
				return _panelList;
			}
		}

		float times = 0;
		// Update is called once per frame
		void FixedUpdate ()
		{
			if (isMoveToNow) {
				times += speed * Time.unscaledDeltaTime;
				if (times > 1) {
					times = 1;
					isMoveToNow = false;
					Utl.doCallback (finishCallback, this);
				}
				panelList.transform.localPosition = fromListPos + diffListPos * moveCurve.Evaluate (times);
				panelList.clipOffset = fromOffset + diffOffset * moveCurve.Evaluate (times);
			}
		}

		/// <summary>
		/// Moves the list show specific cell.
		/// </summary>移动列表显示指定的单元
		/// <param name='parentOfCell'>
		/// Parent of cell.
		/// </param>
		/// <param name='specificCell'>
		/// Specific cell.指定的单元
		/// </param>
		/// <param name='arrangeMent'>
		/// Arrange ment.  1：水平滑动 2：垂直滑动
		/// </param>
		public void moveTo (GameObject specificCell, bool isReverse)
		{
			if (grid == null || panelList == null || specificCell == null) {
				return;
			}
			int index = 0;
			try {
				index = int.Parse (specificCell.name);
			} catch {
				return;
			}
			moveTo (index, isReverse, false);
		}


		public void moveTo (int index, bool isReverse, bool reset, object finishCallback = null)
		{
			this.finishCallback = finishCallback;
			int flag = 1;
			if (isReverse) {
				flag = -1;
			}
			if (grid == null || panelList == null) { // || index < 0) {
				return;
			}
			if (reset)
				grid.resetPosition ();

			Vector4 clip = panelList.baseClipRegion;
			Vector2 newOffset = panelList.clipOffset;
			Vector3 newListPos = panelList.transform.localPosition;
			fromOffset = newOffset;
			fromListPos = newListPos;
			newOffset = grid.oldParentClipOffset;
			newListPos = grid.oldParentPos;
			float cellSize = 0;
			int cellMaxPerLine = grid.maxPerLine;
			cellMaxPerLine = cellMaxPerLine == 0 ? 1 : cellMaxPerLine;

			if ((grid.arrangement == UIGrid.Arrangement.Horizontal && cellMaxPerLine == 1) || 
				(grid.arrangement == UIGrid.Arrangement.Vertical && cellMaxPerLine > 1)) {
				
				cellSize = grid.cellWidth;
				float x = flag * (index / cellMaxPerLine) * cellSize;
				if ((grid.Count () / cellMaxPerLine) * cellSize < clip.z) {
					return;
				}
				newListPos.x = grid.oldParentPos.x - x;
				newOffset.x = grid.oldParentClipOffset.x + x;
			} else {
				cellSize = grid.cellHeight;
				float y = flag * (index / cellMaxPerLine) * cellSize;
				if ((grid.Count () / cellMaxPerLine) * cellSize < clip.w) {
					return;
				}
//			newListPos.y += y;
//			newClipRage.y -= y;
				newListPos.y = grid.oldParentPos.y + y;
				newOffset.y = grid.oldParentClipOffset.y - y;
			}
			toListPos = newListPos;
			toOffset = newOffset;
			diffListPos = toListPos - fromListPos;
			diffOffset = toOffset - fromOffset;
			times = 0;
			isMoveToNow = true;
		}
	}
}
