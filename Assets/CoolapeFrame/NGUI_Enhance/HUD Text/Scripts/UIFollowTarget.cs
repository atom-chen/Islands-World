//--------------------------------------------
//            NGUI: HUD Text
// Copyright © 2012 Tasharen Entertainment
//--------------------------------------------

using UnityEngine;

/// <summary>
/// Attaching this script to an object will make it visibly follow another object, even if the two are using different cameras to draw them.
/// </summary>

[AddComponentMenu("NGUI/Examples/Follow Target")]
public class UIFollowTarget : MonoBehaviour
{
	/// <summary>
	/// 3D target that this object will be positioned above.
	/// </summary>

	public Transform target;
    public Vector3 targetPosition = Vector3.zero;
    public Camera mGameCamera;
	public Camera mUICamera;
	
	public Vector3 offsetPos = Vector3.zero;

	/// <summary>
	/// Whether the children will be disabled when this object is no longer visible.
	/// </summary>

	public bool disableIfInvisible = true;
	
	Transform _mTrans;
	Transform mTrans {
		get {
			if(_mTrans == null) {
				_mTrans = transform;
			}
			return _mTrans;
		}
	}
	bool mIsVisible = false;
	bool isInitFinish = false;

	/// <summary>
	/// Find both the UI camera and the game camera so they can be used for the position calculations
	/// </summary>

	public void Start() // modify by chenbin
	{
		
		if(isInitFinish) return;
		if (target != null)
		{
			mGameCamera = NGUITools.FindCameraForLayer(target.gameObject.layer);
			mUICamera = NGUITools.FindCameraForLayer(gameObject.layer);
			SetVisible(false);
		}
		else
		{
			Debug.LogError("Expected to have 'target' set to a valid transform", this);
			enabled = false;
		}
	}
	
	public void setCamera(Camera mCamera, Camera uiCamera){
		mGameCamera = mCamera;
		mUICamera = uiCamera;
		isInitFinish = true;
	}

	/// <summary>
	/// Enable or disable child objects.
	/// </summary>

	void SetVisible (bool val)
	{
		mIsVisible = val;

		for (int i = 0, imax = mTrans.childCount; i < imax; ++i)
		{
			NGUITools.SetActive(mTrans.GetChild(i).gameObject, val);
		}
	}

    /// <summary>
    /// Update the position of the HUD object every frame such that is position correctly over top of its real world object.
    /// </summary>
    Vector3 pos;
    bool isVisible = false;
    public void LateUpdate ()
	{
        if (!isInitFinish)
        {
            return;
        }
        if(target != null)
        {
            pos = mGameCamera.WorldToViewportPoint(target.position + offsetPos);
        }
        else
        {
            pos = mGameCamera.WorldToViewportPoint(targetPosition+ offsetPos);
        }
        //Debug.Log("pos============" + pos);

        // Determine the visibility and the target alpha
        isVisible = (pos.z > 0f && pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f);
		// Update the visibility flag
		if (disableIfInvisible && mIsVisible != isVisible) SetVisible(isVisible);

		// If visible, update the position
		if (isVisible)
		{
			transform.position = mUICamera.ViewportToWorldPoint(pos);
			//Debug.Log("transform.position============" + transform.position);
			pos = mTrans.localPosition;
			pos.x = Mathf.RoundToInt(pos.x);
			pos.y = Mathf.RoundToInt(pos.y);
			pos.z = 0f;
			mTrans.localPosition = pos;
			//Debug.Log("mTrans.localPosition============" + mTrans.localPosition);
		} else {
			mTrans.localPosition = new Vector3(20000, 20000, 0);
		}
	}
	
	public Vector3 getViewPos() {
		if(!isInitFinish) return Vector3.zero;
		Vector3 pos = mGameCamera.WorldToViewportPoint(target.position + offsetPos);
		pos.z = 0;
		return pos;
	}
	
	public void setTarget(Transform target, Vector3 offset) {
		this.target = target;
        this.targetPosition = Vector3.zero;
        this.offsetPos = offset;
		LateUpdate ();
	}

    public void setTargetPosition(Vector3 targetPosition, Vector3 offset)
    {
        this.target = null;
        this.targetPosition = targetPosition;
        this.offsetPos = offset;
        LateUpdate();
    }
}
