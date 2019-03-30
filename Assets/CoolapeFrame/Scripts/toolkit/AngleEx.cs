/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  几何
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;

namespace Coolape
{
	public static class AngleEx
	{
		// xy平面上的两个点A(a,b),B(c,d),求C(e,f)沿A-->B的向量方向移动一定距离h;此时求所在的位置D的xy坐标
		// xy正切值是(d-b)/(c-a);
		//假设D(x,y)可以得两个方程:
		//(y-f)/(x-e) = (d-b)/(c-a);
		//(y-f)^2 + (x-e) ^2 = h ^2;
		//可求得x,y
		static public Vector2 getEndVector2(Vector2 directionalFrom, Vector2 directionalTo, Vector2 from, float toDistance)
		{
			float x = 0f;
			float y = 0f;
			//取得正切值
			float tanXY = (directionalTo.y - directionalFrom.y) / (directionalTo.x - directionalFrom.x);
			float xx = Mathf.Pow(toDistance, 2) / (Mathf.Pow(tanXY, 2) + 1);
			x = Mathf.Sqrt(xx) + from.x;
			y = tanXY * (x - from.x) + from.y;
			return new Vector2(x, y);
		}
	
		//求圆心p,半径r，逆时针角度angle，所得的圆上的点的坐标
		static public Vector2 getCirclePointV2(Vector2 p, float r, float angle)
		{
			//弧度
			float radian = angle * Mathf.PI / 180;
			float x = p.x + r * Mathf.Cos(radian);
			float y = p.y + r * Mathf.Sin(radian);
			return new Vector2(x, y);
		}

		static public Vector3 getCirclePointV3(Vector3 p, float r, float angle)
		{
			float y = p.y;
			Vector2 pos = getCirclePointV2(new Vector2(p.x, p.z), r, angle);
			return new Vector3(pos.x, y, pos.y);
		}

		static public Vector3 getCirclePointStartWithXV3(Vector3 p, float r, float angle)
		{
			return getCirclePointV3(p, r, angle);
		}

		static public Vector3 getCirclePointStartWithYV3(Vector3 p, float r, float angle)
		{
			return getCirclePointV3(p, r, 90 - angle);
		}
	}
}
