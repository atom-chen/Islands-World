/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  设置render的shader。因为有引起assetbundle加载后，需要重新设置下shader才可以正常显示
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class ResetMat : MonoBehaviour
	{
		public bool isRunOnlyEditerMode = false;

		// Use this for initialization
		void Start()
		{
			if (!isRunOnlyEditerMode ||
			    Application.platform == RuntimePlatform.OSXEditor ||
			    Application.platform == RuntimePlatform.WindowsEditor) {
				resetMat();
			}
		}

		public void resetMat()
		{
			Utl.setBodyMatEdit(transform);
		}
	}
}
