using UnityEngine;
using System.Collections;

public class HUDTest : UIEventListener {
	
	public Transform hudAnchor;
	public Camera mGameCamera;
	public Camera mUICamera;
	public HUDText prefHudtxt;
	
	HUDText hudtxt;
	
	// Use this for initialization
	void Start () {
		if(hudtxt == null) {
			hudtxt = Object.Instantiate(prefHudtxt) as HUDText;
			hudtxt.transform.parent = HUDRoot.go.transform;
			hudtxt.transform.localScale = Vector3.one;
			hudtxt.transform.localPosition = Vector3.zero;
			UIFollowTarget ft = hudtxt.gameObject.AddComponent<UIFollowTarget>();
			ft.target = hudAnchor;
			ft.setCamera(mGameCamera, mUICamera);
			hudtxt.Add("Please click the cube...", Color.white, -1);	//最后一个参数:-1 表示常驻
		}
	}
	
	
	void OnClick() {
		if(hudtxt == null) {
			Start ();
		}
		hudtxt.Add(1, Color.red, 1);
	}
	
	void OnDrag(Vector2 delta) {
		Vector3 off = new Vector3(delta.x, delta.y, 0);
		transform.localPosition += (off*0.01f);
	}
}
