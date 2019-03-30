using UnityEngine;

namespace Coolape
{
	public class uvAn:MonoBehaviour
    {
        public Material _material;
        public bool isSharedMaterial = true;
        public float scrollSpeed = 5;
		public int countX = 4;
		public int countY = 4;
		public float singleX = 0.0f;
		public float singleY = 0.0f;

		private float offsetX = 0.0f;
		private float offsetY = 0.0f;
		public bool isSmooth = true;
        //private var singleTexSize;
        public bool isStop = false;

        public Material material
        {
            get
            {
                if (_material == null)
                {
                    Renderer renderer = GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.mainTextureScale = new Vector2(singleX, singleY);
                        if (isSharedMaterial)
                        {
                            _material = renderer.sharedMaterial;
                        }
                        else
                        {
                            _material = renderer.material;
                        }
                    }
                }
                return _material;
            }
            set
            {
                _material = value;
                if (_material != null)
                {
                    _material.mainTextureScale = new Vector2(singleX, singleY);
                }
            }
        }

		float frame = 0;
		public void Update ()
		{
            if (isStop) return;
            if(material == null)
            {
                return;
            }
            if (isSmooth) {
				frame += Time.deltaTime * scrollSpeed;
			} else {
				frame = Mathf.Floor(Time.time * scrollSpeed);
			}
			offsetX = frame / countX;
			offsetY = -(1.0f / countY) - (frame - frame % countX) / countY / countX;
			material.SetTextureOffset ("_MainTex", new Vector2 (offsetX, offsetY));
		}
	}
}
