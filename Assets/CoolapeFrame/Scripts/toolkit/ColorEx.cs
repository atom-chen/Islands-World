/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  Color Ex
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;

namespace Coolape
{
	public static class ColorEx
	{
		public static Color GetJetColor (float val)
		{
			float fourValue = 4.0f * val;
			float red = Mathf.Min (fourValue - 1.5f, -fourValue + 4.5f);
			float green = Mathf.Min (fourValue - 0.5f, -fourValue + 3.5f);
			float blue = Mathf.Min (fourValue + 0.5f, -fourValue + 2.5f);
			Color newColor = new Color ();
			newColor.r = Mathf.Clamp01 (red);                
			newColor.g = Mathf.Clamp01 (green);
			newColor.b = Mathf.Clamp01 (blue);
			newColor.a = 1;
			return newColor;
		}

		public static Color getGrayColor ()
		{
			Color r2 = getColor (255, 255, 255, 160);
			return r2;
		}

		public static Color getGrayColor2 ()
		{
			Color r2 = getColor (104, 104, 104, 100);
			return r2;
		}

		public static Color getColor (int r, int g, int b)
		{
			return getColor (r, g, b, 255);
		}

		public static Color getColor (int r, int g, int b, int a)
		{
			float rf = (float)r / 255;
			float gf = (float)g / 255;
			float bf = (float)b / 255;
			float af = (float)a / 255;
			Color r2 = new Color (rf, gf, bf, af);
			return r2;
		}
	}
}