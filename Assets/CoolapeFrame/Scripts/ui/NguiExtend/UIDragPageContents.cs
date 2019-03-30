/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  wangkaiyuan
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   拖动滑动一页，比如可以用在关卡地图页面，绑定lua 
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class UIDragPageContents : UIDragScrollView
	{
		Transform tr;

		public Transform transform {
			get {
				if (tr == null) {
					tr = gameObject.transform;
				}
				return tr;
			}
		}

		public UIGridPage _gridPage;

		public UIGridPage gridPage {
			get {
				if (_gridPage == null) {
					_gridPage = transform.parent.GetComponent<UIGridPage> ();
				}
				return _gridPage;
			}
		}

		public void OnPress (bool isPressed)
		{
			if (!enabled || !NGUITools.GetActive(this))
				return;
			if (isPressed) {
				base.OnPress (isPressed);
			}
			gridPage.onPress (isPressed);
		}

		public void OnDrag (Vector2 delta)
		{
			if (!enabled || !NGUITools.GetActive(this))
				return;
			base.OnDrag (delta);
			gridPage.onDrag (delta);
		}

		/// <summary>
		/// Init the specified obj.初始化页面数据
		/// </summary>
		/// <param name="obj">Object.</param>
		public virtual void init (object obj, int index){}

		/// <summary>
		/// Refreshs the current.初始化当前页面数据
		/// </summary>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="obj">Object.</param>
		public virtual void refreshCurrent (int pageIndex, object obj){}
	}
}
