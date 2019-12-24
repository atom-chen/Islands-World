/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  网格生成
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Coolape
{
	public class CLGridPoints : MonoBehaviour
	{
		public int width = 3;
		public int length = 3;
		public float widthInterval = 0.5f;
		public float lengthInterval = 0.5f;
		public bool needCreateObj = false;
		public Transform root;
		public Transform[] points;
		public Vector3[] vectors;
		public OffsetType offsetType = OffsetType.none;
		public float offsetValue = 0;

		public enum OffsetType
		{
			none,
			odd,
			even,
		}

		[ContextMenu ("Execute")]
		public void refreshPoint ()
		{
			if (root == null) {
				root = transform;
			}
			float offsetX = 0;
			float offsetZ = 0;
			offsetX = length / 2 * lengthInterval - ((length % 2 == 0) ? lengthInterval / 2.0f : 0);
			offsetZ = width / 2 * widthInterval - ((width % 2 == 0) ? widthInterval / 2.0f : 0);
			int count = width * length;
			if (needCreateObj) {
				List<Transform> list = new List<Transform> ();
				Transform child = null;
				for (int i = 0; i < root.childCount; i++) {
					child = root.GetChild (i);
					if (i < count) {
						child.name = i.ToString ();
						list.Add (child);
					} else {
						DestroyImmediate (child.gameObject);
					}
				}
				for (int i = root.childCount; i < count; i++) {
					GameObject go = new GameObject (i.ToString ());
					go.transform.parent = root;
					list.Add (go.transform);
				}
				points = list.ToArray ();
				vectors = null;
			} else {
				vectors = new Vector3[count];
				points = null;
			}

			int index = 0;
			Vector3 v3 = Vector3.zero;
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < length; j++) {
					v3.z = -i * widthInterval + offsetZ;
					if (offsetType != OffsetType.none) {
						if (j % 2 == 0 && offsetType == OffsetType.even) {
							v3.z += offsetValue;
						} else if (j % 2 != 0 && offsetType == OffsetType.odd) {
							v3.z += offsetValue;
						}
					}
					v3.x = j * lengthInterval - offsetX;
					index = i * length + j;
					if (needCreateObj) {
						points [index].localPosition = v3;
					} else {
                        v3 += root.localPosition; // root.position;
						vectors [index] = v3;
					}
				}
			}
		}

		//==============================
		#if UNITY_EDITOR
		void OnDrawGizmos ()
		{
			Gizmos.color = Color.red;
            if (points != null) {
                for (int i = 0; i < points.Length; i++) {
                    if (points[i] == null)
                        continue;
                    Gizmos.DrawWireCube(points[i].position, Vector3.one * 0.3f);
                }
            } else if(vectors != null)
            {
                for (int i = 0; i < vectors.Length; i++)
                {
                    if (vectors[i] == null)
                        continue;
                    Gizmos.DrawWireCube(vectors[i] + transform.position, Vector3.one * 0.3f);
                }
            }
            Gizmos.color = Color.white;
		}
		#endif
	}
}