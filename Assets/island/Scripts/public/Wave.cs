using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public float speed = 1;
    public float singleX = 1;
    public float singleY = 1;
    public Material _material;
    public Material material
    {
        get
        {
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

    public bool isStop = false;

    private float offsetX = 0.0f;
    private float offsetY = 0.0f;
    // Update is called once per frame
    void Update()
    {
        if (isStop || material == null) return;
        offsetY += Time.fixedDeltaTime * speed*0.1f;
        material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }
}
