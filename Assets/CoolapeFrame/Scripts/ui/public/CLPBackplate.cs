/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   页面后面的档板
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;

//档板
namespace Coolape
{
	public class CLPBackplate : CLPanelLua
	{
		public static CLPBackplate self;
		//	public UITexture textureBg;
		//	public Camera camera;
		public CLPBackplate ()
		{
			self = this;
		}

		public override void show ()
		{
			base.show ();
		}

		public void procOtherPanel ()
		{
			if (CLPanelManager.panelRetainLayer.Count > 0) {
				CLPanelBase[] ps = CLPanelManager.panelRetainLayer.ToArray ();
				if (ps != null) {
					for (int i = 0; i < ps.Length; i++) {
						if (ps [i].isNeedBackplate && ps [i].isActive) {
							_proc (ps [i]);
							ps = null;
							return;
						}
					}
					hide ();
					ps = null;
				} else {
					hide ();
				}
			} else {
				hide ();
			}
		}

		public void proc (CLPanelBase clpanel)
		{
			if (clpanel == null) {
				procOtherPanel ();
				return;
			}
			if (clpanel.isNeedBackplate) {
				_proc (clpanel);
			} else {
				procOtherPanel ();
			}
		}

		public void _proc (CLPanelBase clpanel)
		{
			setData (clpanel);
			show ();
			this.panel.depth = clpanel.panel.depth - 1;
			Vector3 pos = transform.localPosition;
			this.panel.renderQueue = UIPanel.RenderQueue.StartAt;
			// 设置startingRenderQueue是为了可以在ui中使用粒子效果，注意在粒子中要绑定CLUIParticle角本
			this.panel.startingRenderQueue = CLPanelManager.Const_RenderQueue + this.panel.depth;
			pos.z = -180;
			transform.localPosition = pos;
		}
	}
}
