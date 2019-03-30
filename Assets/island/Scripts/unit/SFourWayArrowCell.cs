using UnityEngine;
using System.Collections;

public class SFourWayArrowCell : UIEventListener
{
	public BoxCollider boxCollider;
	public SFourWayArrow mParent;
	TweenPosition _tweenPos;
	public TweenPosition tweenPos {
		get {
			if(_tweenPos == null ) {
				_tweenPos = GetComponent<TweenPosition>();
			}
			return _tweenPos;
		}
	}

	public void init(int size) {
		boxCollider.size = new Vector3(size*0.012f, 0.02f, 0.005f);
	}
	
	void OnClick() {
		if(mParent != null) {
			mParent.OnClick();
		}
	}
	
	void OnPress( bool isPressed) {
		if(mParent != null) {
			mParent.OnPress(isPressed);
		}
	}
	
	void OnDrag(Vector2 delta) {
		if(mParent != null) {
			mParent.OnDrag(delta);
		}
	}
}
