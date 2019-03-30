/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  canyou
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   进度条动态显示效果，比如一个slider的值是0，当设置成1时，有一个从0增到1的过程
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;
using System;

namespace Coolape
{
	[RequireComponent (typeof(UISlider))]
	public class TweenProgress : UITweener
	{

        [Range(0f, 1f)] public float from = 1f;
        [Range(0f, 1f)] public float to = 1f;
        UISlider _slider;
        public object finishCallback;


        public UISlider slider {
			get {
				if (_slider == null) {
					_slider = gameObject.GetComponent<UISlider> ();
				}
				return _slider;
			}
		}

        public float value
        {
            set{
                slider.value = value;
            }

            get
            {
                return slider.value;
            }
        }

        protected override void OnUpdate(float factor, bool isFinished)
        {
            value = Mathf.Lerp(from, to, factor);
            if(isFinished)
            {
                Utl.doCallback(finishCallback);
            }
        }

        public void Play(bool forward, object finishCallback)
        {
            this.finishCallback = finishCallback;
            base.Play(forward);
        }
        public void Play(float from, float to, object finishCallback)
        {
            this.from = from;
            this.to = to;
            this.finishCallback = finishCallback;
            ResetToBeginning();
            Play(true);
        }
    }
}