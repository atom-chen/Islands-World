using System;
using System.Collections.Generic;
using SimpleFogOfWar.Renderers;
using UnityEngine;
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable ConvertToConstant.Local

namespace SimpleFogOfWar
{
    public class FogOfWarSystem : MonoBehaviour
    {

        public enum FogMode
        {
            Temporary,
            Persistent
        }

        public enum FogTexResolution
        {
            _256 = 256,
            _512 = 512,
            _1024 = 1024,
            _2048 = 2048
        }

        public enum FogVisibility
        {
            Undetermined,
            Visible,
            Invisible
        }

        RenderTexture stampTexture;
        [SerializeField, HideInInspector]
        public float size = 100;
        public FogMode mode = FogMode.Temporary;
        public FogTexResolution Resolution = FogTexResolution._256;
        [SerializeField, HideInInspector]
		public float edgeSoftness = 1f;
        [SerializeField, HideInInspector]
		public Color color = Color.black;
        [SerializeField, HideInInspector]
		public FOWRenderer fogRenderer;
        Texture2D viewStamp;
        Material viewStampMat;

        Texture2D snapShot;
		public float snapShotInterval = 0.5f;
		public float lastSnapShot;
		bool initialized;
		public int layer = 0;
		public int renderQueue = 0;
		public Camera camera;

        static readonly List<FogOfWarInfluence> influences = new List<FogOfWarInfluence>();

        /// <summary>
        /// The interval at which snapshots are taken that are used by <see cref="GetVisibility"/> 
        /// (higher numbers = slower updates = more inaccurate queries, but decreasing performance impact)
        /// </summary>
        public float VisibilitySnapshotInterval
        {
            get { return snapShotInterval; }
            set
            {
                snapShotInterval = value;
                if (snapShotInterval < 0) snapShotInterval = 0;
            }
        }

        /// <summary>
        /// The size of the fog covered area
        /// </summary>
        public float Size
        {
            get { return size; }
            set {
                if (Mathf.Abs(size - value) > 0.0001f)
                {
                    size = value;
                    OnDestroy();
                    initialized = false;
                    Awake();
                }
            }
        }

        /// <summary>
        /// Registers an <see cref="influence"/> to be able to contribute to the fog calculations
        /// </summary>
        public static void RegisterInfluence(FogOfWarInfluence influence)
        {
            if (influence == null) throw new ArgumentNullException("influence");
            if (!influences.Contains(influence)) influences.Add(influence);
        }

        /// <summary>
        /// Removes an <see cref="influence"/> from the fog calculations
        /// </summary>
        public static void UnregisterInfluence(FogOfWarInfluence influence)
        {
            if (influence == null) throw new ArgumentNullException("influence");
            influences.Remove(influence);
        }

        /// <summary>
        /// Sets or replaces the active fog renderer
        /// </summary>
        public void SetFogRenderer(FOWRenderer fRenderer)
        {
            if (fRenderer == null) throw new ArgumentNullException("fRenderer");
            if (fogRenderer != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(fogRenderer);
                }
                else
                {
                    DestroyImmediate(fogRenderer, false);
                }
            }
            fogRenderer = fRenderer;
            if (Application.isPlaying)
            {
                InitializeRenderer();
            }
        }

        /// <summary>
        /// Sets the edge softness of the rendered fog
        /// </summary>
        public void SetBlur(float value)
        {
            edgeSoftness = Mathf.Clamp01(value);
            fogRenderer.SetBlur(edgeSoftness);
        }

        /// <summary>
        /// Sets the color of the fog. Also controls transparency (black = opaque -> white = transparent)
        /// </summary>
        public void SetColor(Color col)
        {
            color = col;
            fogRenderer.SetColor(color);
        }

        /// <summary>
        /// Returns the fog-state of the queried position. Temporal accuracy determined by <see cref="VisibilitySnapshotInterval"/>
        /// </summary>
        public FogVisibility GetVisibility(Vector3 position)
        {
            if (Time.time - lastSnapShot > snapShotInterval | !snapShot)
            {
                SnapshotStampTexture();
                lastSnapShot = Time.time;
            }
            position = transform.InverseTransformPoint(position);
            var scaling = (int)Resolution / size;
            var x = (int)(position.x * scaling);
            var y = (int)(position.z * scaling);
            if (x < 0 || x > snapShot.width || y < 0 || y > snapShot.height) return FogVisibility.Undetermined;
            return snapShot.GetPixel(x, y).grayscale < 0.5f ? FogVisibility.Invisible : FogVisibility.Visible;
        }

        /// <summary>
        /// If <see cref="mode"/> is <see cref="FogMode.Persistent"/>, returns data that can be used to recreate the current fog state via <see cref="LoadPersistentData"/>
        /// </summary>
        public byte[] GetPersistentData()
        {
            SnapshotStampTexture();
            return snapShot.GetRawTextureData();
        }

        /// <summary>
        /// Recreates a previous fog state captured via <see cref="GetPersistentData"/>, settings must not have changed in between!
        /// Only usable if <see cref="mode"/> is <see cref="FogMode.Persistent"/>
        /// </summary>
        public void LoadPersistentData(byte[] data)
        {
            if (mode != FogMode.Persistent) return;
            Initialize();
            var dataTex = new Texture2D((int)Resolution, (int)Resolution);
            dataTex.LoadRawTextureData(data);
            dataTex.Apply();
            ClearPersistenFog();
            Graphics.Blit(dataTex, stampTexture);
        }

        /// <summary>
        /// Removes persistent uncovered fog
        /// </summary>
        public void ClearPersistenFog()
        {
            if (mode != FogMode.Persistent) return;
            ClearStampTexture();
        }

        void OnValidate()
        {
            if (size < 0) size = 0;
            edgeSoftness = Mathf.Clamp01(edgeSoftness);
            if (snapShotInterval < 0) snapShotInterval = 0;
        }

        void Awake()
        {
            Initialize();
            if (fogRenderer) InitializeRenderer();
        }

        void OnDestroy()
        {
            if (viewStamp) Destroy(viewStamp);
            if (stampTexture) RenderTexture.ReleaseTemporary(stampTexture);
        }
		public void setRenderQueue(int val) {
			renderQueue = val;
			if (renderQueue > 0) {
				if (viewStampMat != null) {
					viewStampMat.renderQueue = renderQueue;
				}
				if (fogRenderer.material != null) {
					fogRenderer.material.renderQueue = renderQueue;
				}
			}
		}
        public void LateUpdate()
        {
            if (fogRenderer != null)
            {
				fogRenderer.Render(transform.position, layer, camera);
            }
        }

        void OnDrawGizmosSelected()
        {
            if (!fogRenderer) return;
            Gizmos.color = new Color(1, 1, 1, 0.5f);
            fogRenderer.DrawGizmos(this);
        }

        void Initialize()
        {
            if (initialized) return;
            const int viewTexSize = 256;
            viewStamp = new Texture2D(viewTexSize, viewTexSize);
            var c = new Color(1f, 1f, 1f, 0f);
            for (var x = -viewTexSize / 2; x < viewTexSize / 2; x++)
            {
                for (var y = -viewTexSize / 2; y < viewTexSize / 2; y++)
                {
					float dis = new Vector2 (x, y).magnitude;
					if(dis < viewTexSize * 0.5f) {
						float alpha = 1-dis/(viewTexSize * 0.5f);
						c.a = alpha;
					}  else {
						c.a = 0;
					}

		         	viewStamp.SetPixel(x + viewTexSize / 2, y + viewTexSize / 2, c);
//                    viewStamp.SetPixel(x + viewTexSize / 2, y + viewTexSize / 2, new Vector2(x, y).magnitude < viewTexSize * 0.5f ? Color.white : c);
                }
            }
            viewStamp.Apply();

            stampTexture = RenderTexture.GetTemporary((int)Resolution, (int)Resolution);
            ClearStampTexture();
            viewStampMat = new Material(Shader.Find("Hidden/FOWViewStampShader"));
			if (renderQueue > 0) {
				viewStampMat.renderQueue = renderQueue;
			}
            initialized = true;
        }

        public void ClearStampTexture()
        {
            var oldRT = RenderTexture.active;
            RenderTexture.active = stampTexture;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = oldRT;
        }

        void InitializeRenderer()
        {
			fogRenderer.Init(this, stampTexture, renderQueue);
            SetBlur(edgeSoftness);
            SetColor(color);
        }

        void SnapshotStampTexture()
        {
            var oldRT = RenderTexture.active;
            RenderTexture.active = stampTexture;
            if (snapShot == null) snapShot = new Texture2D(stampTexture.width, stampTexture.height);
            snapShot.ReadPixels(new Rect(0, 0, stampTexture.width, stampTexture.height), 0, 0);
            snapShot.Apply();
            RenderTexture.active = oldRT;
        }

        void OnRenderObject()
        {
			if (Camera.current != this.camera) {
				return;
			}
            var oldRTex = RenderTexture.active;
            RenderTexture.active = stampTexture;
            if (mode == FogMode.Temporary)
            {
                GL.Clear(true, true, Color.black);
            }
            GL.PushMatrix();
            var res = (int)Resolution;
            GL.LoadPixelMatrix(0, res, 0, res);
            for (var i = 0; i < influences.Count; i++)
            {
                if (influences[i].Suspended) continue;
                var abspos = transform.InverseTransformPoint(influences[i].transform.position);
                var scaling = (int)Resolution / size;
                var screenRect = new Rect(new Vector2(abspos.x, abspos.z) * scaling, new Vector2(influences[i].ViewDistance, influences[i].ViewDistance) * scaling);
                screenRect.position -= screenRect.size * 0.5f;
                Graphics.DrawTexture(screenRect, viewStamp, viewStampMat);
            }
            GL.PopMatrix();
            RenderTexture.active = oldRTex;
        }

    }
}
