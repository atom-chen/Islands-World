using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLSetRenderQueue : MonoBehaviour
{
	public Renderer render;
	public int defaultRenderQueue = 0;
	public int settingRenderQueue = 3000;
	public bool isSharedMaterial = true;
	bool isInited = false;
	// Use this for initialization

	public void Start ()
	{
		try {
			defaultRenderQueue = renderQueue;
			renderQueue = settingRenderQueue;
		} catch (System.Exception e) {
			Debug.LogError (e);
		}
	}

	public void OnBecameVisible ()
	{
		try {
			renderQueue = settingRenderQueue;
		} catch (System.Exception e) {
			Debug.LogError (e);
		}
	}


	public int renderQueue {
		set {
			settingRenderQueue = value;
			if (isSharedMaterial) {
				if (render.sharedMaterial != null) {
					render.sharedMaterial.renderQueue = value;
				}
			} else {
				if (render.material != null) {
					render.material.renderQueue = value;
				}
			}
		}
		get {
			if (isSharedMaterial) {
				if (render.sharedMaterial != null) {
					return render.sharedMaterial.renderQueue;
				} else {
					return 0;
				}
			} else {
				if (render.material != null) {
					return render.material.renderQueue;
				} else {
					return 0;
				}
			}
		}
	}

    /// <summary>
    /// Ons the finish load assets.配合CLSharedAssets使用
    /// </summary>
    /// <param name="go">Go.</param>
    public void onFinishLoadAssets(GameObject go)
    {
        resetRenderQueue(go);
    }
    public void resetRenderQueue (GameObject go)
	{
		reset ();
	}

	[ContextMenu ("resetRenderQueue")]
	public void reset ()
	{
		try {
			renderQueue = settingRenderQueue;
		} catch (System.Exception e) {
			Debug.LogError (e);
		}
	}
}
