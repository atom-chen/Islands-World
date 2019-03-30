using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coolape
{
    public static class CLAIPathUtl
    {
        /// <summary>
        /// Softens the path.软化路径
        /// </summary>
        /// <param name="list">List.路径列表</param>
        /// <param name="softenPathType">Soften path type.软化类型</param>
        /// <param name="slerpFactor">Soften factor.曲面差值次数</param>
        /// <param name="cellSize">Cell size.单元格大小</param>
        public static void softenPath(ref List<Vector3> list, SoftenPathType softenPathType, int slerpFactor, float cellSize)
        {
            if (list == null || list.Count < 3)
            {
                return;
            }
            int i = 1;

            float factor = 0.25f;
            float slerpOffset = 0;
            if (softenPathType == SoftenPathType.Slerp)
            {
                factor = 0.5f;
                slerpFactor = slerpFactor == 0 ? 1 : slerpFactor;
                slerpOffset = 1.0f / slerpFactor;
            }
            while (i < list.Count - 1)
            {
                Vector3 mid = list[i];
                Vector3 left = list[i - 1];
                Vector3 right = list[i + 1];

                Vector3 angle1 = Utl.getAngle(mid, left);
                Vector3 angle2 = Utl.getAngle(mid, right);
                if (Mathf.Abs(Mathf.Abs(angle1.y - angle2.y) - 180) <= 20)
                {
                    //基本在一条线上，直接跳过
                    i++;
                    continue;
                }

                Vector3 leftOffset = Vector3.zero;
                float leftDis = Vector3.Distance(mid, left);
                if (leftDis >= cellSize)
                {
                    leftOffset = (left - mid).normalized * cellSize * factor;
                }
                else
                {
                    leftOffset = (left - mid).normalized * leftDis * factor;
                }
                left = mid + leftOffset;

                Vector3 rightOffset = Vector3.zero;
                float rightDis = Vector3.Distance(mid, right);
                if (rightDis >= cellSize)
                {
                    rightOffset = (right - mid).normalized * cellSize * factor;
                }
                else
                {
                    rightOffset = (right - mid).normalized * rightDis * factor;
                }
                right = mid + rightOffset;

                Vector3 center = (left + right) / 2.0f;
                Vector3 mid2 = center + (center - mid).normalized * Vector3.Distance(center, mid) * 4f;

                list.RemoveAt(i); //把硬转角的点去掉
                                  //接下来加入一些新的点
                list.Insert(i, left);
                i++;
                if (softenPathType == SoftenPathType.Slerp)
                {
                    //加入球面插值的点
                    Vector3 v1 = left - mid2;
                    Vector3 v2 = right - mid2;
                    for (int j = 0; j < slerpFactor; j++)
                    {
                        Vector3 pos = Vector3.Slerp(v1, v2, slerpOffset * j);
                        list.Insert(i, (pos + mid2));
                        i++;
                    }
                }
                list.Insert(i, right);
                i++;
            }
        }

        //============================================
        //============================================
        //============================================
        public enum SoftenPathType
        {
            Line,
            Slerp
        }
    }
}