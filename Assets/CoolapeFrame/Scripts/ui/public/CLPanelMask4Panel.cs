/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   页面的遮羞布
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Coolape
{
	public class CLPanelMask4Panel : CLPanelLua
	{
		public TweenAlpha tweenAlpha;
		public UISprite sprite;
		public UILabel label;
		object finishCallback;
		bool isShowing = false;
		public static CLPanelMask4Panel self;

		public List<string> defautSpriteNameList = new List<string>();

		public CLPanelMask4Panel()
		{
			self = this;
		}
		public override void init ()
		{
			tweenAlpha.ResetToBeginning ();
			base.init ();
		}
		public void _show(object callback, List<string> list)
		{
			NGUITools.SetActive(gameObject, true);
			List<string> tmplist = defautSpriteNameList;
			if (list != null && list.Count > 0) {
				tmplist = list;
			}
			if (tmplist != null && tmplist.Count > 0) {
				int index = NumEx.NextInt(0, tmplist.Count);
				CLUIUtl.setSpriteFit(sprite, tmplist [index]);
			}

			finishCallback = callback;
//		NGUITools.SetActive (label.gameObject, false);
			tweenAlpha.Play(true);
		
			CLPanelBase p = CLPanelManager.topPanel;
			panel.depth = p == null ? 3000 : p.panel.depth + CLPanelManager.depthOffset + 100;
			panel.renderQueue = UIPanel.RenderQueue.StartAt;
			// 设置startingRenderQueue是为了可以在ui中使用粒子效果，注意在粒子中要绑定CLUIParticle角本
			panel.startingRenderQueue = CLPanelManager.Const_RenderQueue + this.panel.depth;
		}

		public void _hide(object callback)
		{
			finishCallback = callback;
			tweenAlpha.Play(false);
		}

		public void onTweenFinish(GameObject go)
		{
			isShowing = !isShowing;
			if (isShowing) {
				NGUITools.SetActive(label.gameObject, true);
			} else {
				NGUITools.SetActive(gameObject, false);
			}

			Invoke ("doCallback", 0.1f);
		}

		public void doCallback()
		{
			Utl.doCallback(finishCallback, this);
		}

		public static void show(object callback, List<string> list)
		{
			self._show(callback, list);
		}

		public static void hide(object callback)
		{
			self._hide(callback);
		}
	}
}
