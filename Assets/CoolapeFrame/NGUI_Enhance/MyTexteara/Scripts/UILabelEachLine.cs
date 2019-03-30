using UnityEngine;
using System.Collections;

[RequireComponent (typeof(UILabel))]
public class UILabelEachLine : MonoBehaviour
{
	public UITexteara texteara;
	UILabel _label;

	public UILabel label {
		get {
			if (_label == null) {
				_label = GetComponent<UILabel> ();
			} 
			return _label;
		}
	}

	UITweener[] _tweeners;

	public UITweener[] tweeners {
		get {
			if (_tweeners == null) {
				_tweeners = GetComponents<UITweener> ();
			}
			return _tweeners;
		}
	}

	public string text {
		get {
			return label.text;
		}
		set {
			label.text = value;
			if (tweeners != null && tweeners.Length > 0) {
				for (int i = 0; i < tweeners.Length; i++) {
					tweeners [i].ResetToBeginning ();
					tweeners [i].Play (true);
				}
			}
		}
	}

	public float tweenDelay {
		set {
			if (tweeners != null && tweeners.Length > 0) {
				for (int i = 0; i < tweeners.Length; i++) {
					tweeners [i].delay = value;
				}
			}
		}
	}

	// Use this for initialization
	void Start ()
	{
		if (texteara == null) {
			texteara = GetComponentInParent<UITexteara> ();
		}
	}
	
	// Update is called once per frame
//	void Update ()
//	{
//		if (!label.isVisible) {
////			Debug.Log (label.text);
//		}
//	}
}
