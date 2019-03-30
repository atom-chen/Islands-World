/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   在ui中使用粒子时，需要绑定该脚本，
  *						主要功能是设置粒子的renderQueue,使粒子可以在两个页面之间显示，而不总是显示在最顶层
  *						另外会自动添加一个uisprite，目的是用这个sprite占一个renderqueue的位置
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;

namespace Coolape
{
	[RequireComponent (typeof(UISprite))]
	public class CLUIRenderQueue : MonoBehaviour
	{
		public UIPanel _panel;
		UISprite sprite;
		public int depth = 0;
		public int currRenderQueue = 0;
		public bool isSharedMaterial = true;
        public bool isForceUpdateSetRenderQueue = false;
		Renderer[] _renders;

		Renderer[] renders {
			get {
				if (_renders == null) {
					_renders = GetComponentsInChildren<Renderer> ();
				}
				return _renders;
			}
		}

		public UIPanel panel {
			get {
				if (_panel == null) {
					_panel = GetComponentInParent<UIPanel> ();
				}
				return _panel;
			}
		}
		// Use this for initialization
		public void Start ()
		{
			mWidget.depth = depth;
			#if UNITY_EDITOR
			//因为是通过assetebundle加载的，在真机上不需要处理，只有在pc上需要重设置shader
//			if (Application.isPlaying) {
//				Utl.setBodyMatEdit (transform);
//			}
			#endif

		}

        public void reset()
        {
            reset(false);
        }
        public void reset(bool forceUpdate)
        {
            Start();
            isForceUpdateSetRenderQueue = forceUpdate;
            _renders = null;
        }

        public int mDepth {
			get {
				return depth;
			}
			set {
				depth = value;
				mWidget.depth = depth;
			}
		}

		public UIWidget mWidget {
			get {
				if (sprite == null) {
					// 建议使用 _empty 的 UITextures
					sprite = GetComponent<UISprite> ();
					sprite.height = 1;
					sprite.width = 1;
					sprite.alpha = 0.004f;
					sprite.depth = depth;
					if (sprite.atlas == null) {
						sprite.atlas = CLUIInit.self.emptAtlas;
					}
					// 随便设置一个图给他，不然不能被渲染
					if (CLUIInit.self.emptAtlas != null && CLUIInit.self.emptAtlas.spriteList.Count > 0) {
						sprite.spriteName = (CLUIInit.self.emptAtlas.spriteList [0]).name;
					}
				}
				return sprite;
			}
		}

		// Update is called once per frame
		void LateUpdate ()
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying) {
				mWidget.depth = depth;
			}
			#endif
			setRenderQueue (isForceUpdateSetRenderQueue);
		}
		void OnEnable(){
			#if UNITY_EDITOR
			if (!Application.isPlaying) {
				mWidget.depth = depth;
			}
			#endif
			setRenderQueue (true);
		}
		[ContextMenu ("setRenderQueue")]
		public void setRenderQueueExe ()
		{
			setRenderQueue (true);
		}
		public void setRenderQueue (bool isForce = false)
		{
			int newRenderQ = currRenderQueue;
			Material mat = null;

			if (mWidget != null && NGUITools.GetActive (mWidget)) {
				if (mWidget.drawCall == null) {
					#if UNITY_EDITOR
//					Debug.LogWarning ("widget.drawCall == null");
					#endif
					mWidget.depth = depth;
				} else {
					newRenderQ = mWidget.drawCall.finalRenderQueue;
				}
			} else {
				newRenderQ = panel != null ? panel.startingRenderQueue + depth : depth;
			}

			if (isForce || newRenderQ != currRenderQueue) {
				for (int i = 0; i < renders.Length; i++) {
					if (isSharedMaterial) {
						mat = renders [i].sharedMaterial;
					} else {
						mat = renders [i].material;
					}
					if (mat != null) {
						mat.renderQueue = newRenderQ;
					}
				}
				currRenderQueue = newRenderQ;
			}
		}

	}
}
