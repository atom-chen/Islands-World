/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   包装NGUI播放音效,支持只保存音效名，方便在打包成ab时，不把音效文件打进去
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class CLUIPlaySound : UIPlaySound
	{
		[HideInInspector]
		public string soundFileName = "Tap.ogg";
		public string soundName = "Tap";

		public override void Play ()
		{
			SoundEx.playSound (soundName, volume, 1);
		}
	}
}