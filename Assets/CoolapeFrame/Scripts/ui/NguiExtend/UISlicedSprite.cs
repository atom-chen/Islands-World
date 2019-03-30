/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  wangkaiyuan
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  正常的九宫格sprite是四个角保持不变，拉伸中间。
  *						而该脚本正好反过来的效果，九宫格中间不变，四边接伸。
  *						可以达到很完善的遮罩效果.
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;

namespace Coolape
{
	public class UISlicedSprite : UISprite
	{

		[ContextMenu("Execute")]
		public void refreshCenter() {
			mChanged = true;
			MarkAsChanged();
		}

		// Type --> Sliced
		[SerializeField] 
		public UIWidget mCenter = null;

		/// <summary>
		/// Virtual function called by the UIPanel that fills the buffers.
		/// </summary>
		protected override void SlicedFill (BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
		{
			Vector4 br = border * pixelSize;
			if (br.x == 0f && br.y == 0f && br.z == 0f && br.w == 0f) {
				SimpleFill (verts, uvs, cols);
				return;
			}
			Color32 c = drawingColor;
			Vector4 v = drawingDimensions;

			if (mCenter != null) {
				Vector4 h = mCenter.drawingDimensions;
				/// X = left, Y = bottom, Z = right, W = top.
				Vector3 offset = mCenter.transform.localPosition;
//			print ("center pos = " + offset );
				h.x += offset.x;
				h.y += offset.y;
				h.z += offset.x;
				h.w += offset.y;

//			print ("h = " + h );
//			print ("v = " + v );

				br.x = -(v.x - h.x);
				br.y = -(v.y - h.y);
				br.z = (v.z - h.z);
				br.w = (v.w - h.w);
//			print ("br = " + br );
//			print ("br w= " + (br.z - br.x).ToString() );
//			print ("br h= " + (br.w - br.y).ToString() );

				if (br.x < 0f)
					br.x = 0f;
				if (br.y < 0f)
					br.y = 0f;
				if (br.z < 0f)
					br.z = 0f;
				if (br.w < 0f)
					br.w = 0f;
			}
			mTempPos [0].x = v.x;
			mTempPos [0].y = v.y;
			mTempPos [3].x = v.z;
			mTempPos [3].y = v.w;

			if (mFlip == Flip.Horizontally || mFlip == Flip.Both) {
				mTempPos [1].x = mTempPos [0].x + br.z;
				mTempPos [2].x = mTempPos [3].x - br.x;
			
				mTempUVs [3].x = mOuterUV.xMin;
				mTempUVs [2].x = mInnerUV.xMin;
				mTempUVs [1].x = mInnerUV.xMax;
				mTempUVs [0].x = mOuterUV.xMax;
			} else {
				mTempPos [1].x = mTempPos [0].x + br.x;
				mTempPos [2].x = mTempPos [3].x - br.z;
			
				mTempUVs [0].x = mOuterUV.xMin;
				mTempUVs [1].x = mInnerUV.xMin;
				mTempUVs [2].x = mInnerUV.xMax;
				mTempUVs [3].x = mOuterUV.xMax;
			}
		
			if (mFlip == Flip.Vertically || mFlip == Flip.Both) {
				mTempPos [1].y = mTempPos [0].y + br.w;
				mTempPos [2].y = mTempPos [3].y - br.y;
			
				mTempUVs [3].y = mOuterUV.yMin;
				mTempUVs [2].y = mInnerUV.yMin;
				mTempUVs [1].y = mInnerUV.yMax;
				mTempUVs [0].y = mOuterUV.yMax;
			} else {
				mTempPos [1].y = mTempPos [0].y + br.y;
				mTempPos [2].y = mTempPos [3].y - br.w;
			
				mTempUVs [0].y = mOuterUV.yMin;
				mTempUVs [1].y = mInnerUV.yMin;
				mTempUVs [2].y = mInnerUV.yMax;
				mTempUVs [3].y = mOuterUV.yMax;
			}
		
			for (int x = 0; x < 3; ++x) {
				int x2 = x + 1;
			
				for (int y = 0; y < 3; ++y) {
					if (centerType == AdvancedType.Invisible && x == 1 && y == 1)
						continue;
				
					int y2 = y + 1;
				
					verts.Add (new Vector3 (mTempPos [x].x, mTempPos [y].y));
					verts.Add (new Vector3 (mTempPos [x].x, mTempPos [y2].y));
					verts.Add (new Vector3 (mTempPos [x2].x, mTempPos [y2].y));
					verts.Add (new Vector3 (mTempPos [x2].x, mTempPos [y].y));
				
					uvs.Add (new Vector2 (mTempUVs [x].x, mTempUVs [y].y));
					uvs.Add (new Vector2 (mTempUVs [x].x, mTempUVs [y2].y));
					uvs.Add (new Vector2 (mTempUVs [x2].x, mTempUVs [y2].y));
					uvs.Add (new Vector2 (mTempUVs [x2].x, mTempUVs [y].y));
				
					cols.Add (c);
					cols.Add (c);
					cols.Add (c);
					cols.Add (c);
				}
			}
		}
	}
}
