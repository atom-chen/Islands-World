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
		public float tilingX = 1.0f;
		public float tilingY = 1.0f;

        float offsetX = 0.0f;
        float offsetY = 0.0f;
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
                        if (isSharedMaterial)
                        {
                            renderer.sharedMaterial.mainTextureScale = new Vector2(tilingX, tilingY);
                            _material = renderer.sharedMaterial;
                        }
                        else
                        {
                            renderer.material.mainTextureScale = new Vector2(tilingX, tilingY);
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
                    _material.mainTextureScale = new Vector2(tilingX, tilingY);
                }
            }
        }

		float frame = 0;
        Vector2 texOffset = new Vector2();
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
            texOffset.x = offsetX;
            texOffset.y = offsetY;
            material.SetTextureOffset ("_MainTex", texOffset);
        }
	}
}
