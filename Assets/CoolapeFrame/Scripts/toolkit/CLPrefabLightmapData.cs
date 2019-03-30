/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  prefab lightmap data save & load. for unity 5.x version 
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

/// <summary>
/// CL prefab lightmap data. for unity 5.x version
/// </summary>
namespace Coolape
{
	public class CLPrefabLightmapData : MonoBehaviour
	{
		[System.Serializable]
		public struct RendererInfo
		{
			public Renderer renderer;
			public int lightmapIndex;
			public Vector4 lightmapOffsetScale;
		}

		[SerializeField]
		public LightmapsMode lightmapsMode;

		[SerializeField]
		public LightProbes lightProbes;

		[SerializeField]
		public List<RendererInfo>	m_RendererInfo = new List<RendererInfo> ();

		[SerializeField]
		public Texture2D[] m_Lightmaps;

		void Awake ()
		{
			LoadLightmap ();
		}

		#if UNITY_EDITOR
		public void SaveLightmap ()
		{
			m_RendererInfo.Clear ();
			lightmapsMode = LightmapSettings.lightmapsMode;
			lightProbes = LightmapSettings.lightProbes;

			var renderers = GetComponentsInChildren<MeshRenderer> ();
			Texture2D lightmapTexture;
			LightmapData lightMapData;
			m_Lightmaps = new Texture2D[LightmapSettings.lightmaps.Length];
			foreach (MeshRenderer r in renderers) {
				if (r.lightmapIndex != -1) {
					RendererInfo info = new RendererInfo ();
					info.renderer = r;
					info.lightmapOffsetScale = r.lightmapScaleOffset;
					info.lightmapIndex = r.lightmapIndex;
					//=====================================
//					Debug.Log (r.lightmapIndex);
					lightMapData = LightmapSettings.lightmaps [r.lightmapIndex];

#if UNITY_5_6_OR_NEWER
					lightmapTexture = lightMapData.lightmapColor;
#else
					lightmapTexture = lightMapData.lightmapLight;
#endif
//				info.lightmapIndex = m_Lightmaps.IndexOf (lightmapTexture);
					if (m_Lightmaps [r.lightmapIndex] == null) {
						info.lightmapIndex = r.lightmapIndex;
//					m_Lightmaps.Add (lightmapTexture);
						m_Lightmaps [r.lightmapIndex] = lightmapTexture;
					}
					//=====================================
					m_RendererInfo.Add (info);
				}
			}
		}
		#endif

		public void LoadLightmap ()
		{
			if (m_RendererInfo == null || m_RendererInfo.Count <= 0)
				return;

			var lightmaps = LightmapSettings.lightmaps;
			int preIndex = lightmaps.Length;
			var combinedLightmaps = new LightmapData[preIndex + m_Lightmaps.Length];
			lightmaps.CopyTo (combinedLightmaps, 0);
			LightmapData lightMapData;
			for (int i = 0; i < m_Lightmaps.Length; i++) {
				lightMapData = new LightmapData ();
#if UNITY_5_6_OR_NEWER
				lightMapData.lightmapColor = m_Lightmaps [i];
#else
				lightMapData.lightmapLight = m_Lightmaps [i];
#endif
				lightMapData.lightmapDir = m_Lightmaps [i];
				combinedLightmaps [preIndex + i] = lightMapData;
			}
			LightmapSettings.lightmapsMode = lightmapsMode;
			LightmapSettings.lightmaps = combinedLightmaps;

			foreach (var item in m_RendererInfo) {
				item.renderer.lightmapIndex = preIndex + item.lightmapIndex;
				item.renderer.lightmapScaleOffset = item.lightmapOffsetScale;
			}

		}
	}
}
