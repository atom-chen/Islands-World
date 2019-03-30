using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITexteara : MonoBehaviour
{
	public enum EffectMode
	{
		none,
		positon1,
		positon2,
		positon3,
		scale1,
		scale2,
		rotate1,
		rotate2,
		alpha,
	}

	public UILabel mLabel;
	public UIScrollView scrollView;
	[HideInInspector]
	public EffectMode effectMode = EffectMode.none;
	[HideInInspector]
	public UITweener.Method method = UITweener.Method.Linear;
	[HideInInspector]
	public float duration = 1;
	[HideInInspector]
	public float delay = 0.1f;
	//----------------------------------------------------------------
	Vector2 mSize = Vector2.zero;
	public List<UILabelEachLine> labelList = new List<UILabelEachLine> ();

	string oldContent = "";
	void Start ()
	{
		if (mLabel == null) {
			Debug.LogError ("The label is null, please drag a UILabel into here");
			return;
		}
		mLabel.onChange += onLabelChange;
		mLabel.alpha = 0;

		if (scrollView == null) {
			scrollView = GetComponentInParent<UIScrollView> ();
		}
	}

	// Update is called once per frame
	void Update ()
	{
	
	}

	public void refresh ()
	{
		refresh (true);
	}

	public void refresh (bool force)
	{
		if (force) {
			getProcText ();
		}
		show ();
	}

	string[] _textLines;

	public string[] textLines {
		get {
			return _textLines;
		}
	}

	[ContextMenu ("Refresh")]
	public void onLabelChange ()
	{
		if (!oldContent.Equals (mLabel.text)) {
			oldContent = mLabel.text;
			refresh (true);
		}
	}

	[ContextMenu ("show Proc Text")]
	public string[] getProcText ()
	{
		string text = mLabel.processedText;
		string[] strs = text.Split ('\n');
//		for (int i = 0; i < strs.Length; i++) {
//			Debug.Log (strs [i]);
//		}
		_textLines = strs;
		mSize = mLabel.printedSize;
		return strs;
	}

	public void setEffect (UILabel label)
	{
		switch (effectMode) {
		case EffectMode.positon1:
		case EffectMode.positon2:
		case EffectMode.positon3:
			addTweenPosition (label.gameObject, Vector3.zero, Vector3.zero);
			break;
		case EffectMode.scale1:
			addTweenScrale (label.gameObject, new Vector3 (1, 0, 1), Vector3.one);
			break;
		case EffectMode.scale2:
			addTweenScrale (label.gameObject, Vector3.one * 2, Vector3.one);
			addTweenAlpha (label.gameObject, 0, 1);
			break;
		case EffectMode.rotate1:
			addTweenRotate (label.gameObject, new Vector3 (90, 45, 0), Vector3.zero);
			addTweenAlpha (label.gameObject, 0, 1);
			break;
		case EffectMode.rotate2:
			addTweenRotate (label.gameObject, new Vector3 (-180, 45, 0), Vector3.zero);
			addTweenAlpha (label.gameObject, 0, 1);
			break;
		case EffectMode.alpha:
			addTweenAlpha (label.gameObject, 0, 1);
			break;
		}
	}

	public TweenPosition addTweenPosition (GameObject go, Vector3 from, Vector3 to)
	{
		TweenPosition twPosition = go.AddComponent<TweenPosition> ();
		twPosition.enabled = false;
		twPosition.from = from;
		twPosition.to = to;
		twPosition.method = method;
		twPosition.duration = duration;	
//		twPosition.ResetToBeginning ();
		return twPosition;
	}

	public TweenPosition setTweenPosition (GameObject go, Vector3 from, Vector3 to)
	{
		TweenPosition twPosition = go.GetComponent<TweenPosition> ();
		if (twPosition == null) {
			twPosition = addTweenPosition (go, from, to);
		}
		twPosition.enabled = false;
		twPosition.from = from;
		twPosition.to = to;
		twPosition.method = method;
		twPosition.duration = duration;	
//		twPosition.ResetToBeginning ();
		return twPosition;
	}

	public TweenScale addTweenScrale (GameObject go, Vector3 from, Vector3 to)
	{
		TweenScale twScale = go.AddComponent<TweenScale> ();
		twScale.enabled = false;
		twScale.from = from;
		twScale.to = to;
		twScale.method = method;
		twScale.duration = duration;	
//		twScale.ResetToBeginning ();
		return twScale;
	}

	public TweenRotation addTweenRotate (GameObject go, Vector3 from, Vector3 to)
	{
		TweenRotation twRotate = go.AddComponent<TweenRotation> ();
		twRotate.enabled = false;
		twRotate.from = from;
		twRotate.to = to;
		twRotate.method = method;
		twRotate.duration = duration;
//		twRotate.ResetToBeginning ();
		return twRotate;
	}

	public TweenAlpha addTweenAlpha (GameObject go, float from, float to)
	{
		TweenAlpha twAlpha = go.AddComponent<TweenAlpha> ();
		twAlpha.enabled = false;
		twAlpha.from = from;
		twAlpha.to = to;
		twAlpha.method = method;
		twAlpha.duration = duration;
		twAlpha.ResetToBeginning ();
		return twAlpha;
	}

	public void show ()
	{
		int count = textLines.Length;
		UILabelEachLine eachLine = null;
		for (int i = labelList.Count; i < count; i++) {
			UILabel label = NGUITools.AddChild (gameObject, mLabel.gameObject).GetComponent<UILabel> ();
			setEffect (label);
			label.alpha = 1;
			label.overflowMethod = UILabel.Overflow.ResizeFreely;
			eachLine = label.gameObject.AddComponent<UILabelEachLine> ();
			eachLine.texteara = this;
			NGUITools.SetActive (label.gameObject, false);
			labelList.Add (eachLine);
		}
		float heightOffset = mSize.y / count;
		int labelCount = labelList.Count;
		Vector3 pos = mLabel.transform.localPosition;
		Vector3 fromPos = Vector3.zero;
		Vector3 toPos = Vector3.zero;
		float flag = -1;
		if (mLabel.pivot == UIWidget.Pivot.Center || mLabel.pivot == UIWidget.Pivot.Left || mLabel.pivot == UIWidget.Pivot.Right) {
			pos.y += (mSize.y - mLabel.fontSize - mLabel.spacingY) / 2;
		} else if (mLabel.pivot == UIWidget.Pivot.Bottom || mLabel.pivot == UIWidget.Pivot.BottomLeft || mLabel.pivot == UIWidget.Pivot.BottomRight) {
			pos.y += (mSize.y - mLabel.fontSize - mLabel.spacingY);
		}

		for (int i = 0; i < labelCount; i++) {
			eachLine = labelList [i];
			if (i < count) {
				toPos = pos + new Vector3 (0, i * flag * heightOffset, 0);
				if (effectMode == EffectMode.positon1) {
					fromPos = toPos;
					fromPos.x -= (mLabel.localSize.x + 40);
					setTweenPosition (eachLine.gameObject, fromPos, toPos);
				} else if (effectMode == EffectMode.positon2) {
					fromPos = toPos;
					fromPos.x += (mLabel.localSize.x + 40);
					setTweenPosition (eachLine.gameObject, fromPos, toPos);
				} else if (effectMode == EffectMode.positon3) {
					fromPos = toPos;
					if (i % 2 == 0) {
						fromPos.x += (mLabel.localSize.x + 40);
					} else {
						fromPos.x -= (mLabel.localSize.x + 40);
					}
					setTweenPosition (eachLine.gameObject, fromPos, toPos);
				} else {
					eachLine.transform.localPosition = toPos;
				}
				eachLine.tweenDelay = i * delay;
				eachLine.text = textLines [i];
				NGUITools.SetActive (eachLine.gameObject, true);
			} else {
				NGUITools.SetActive (eachLine.gameObject, false);
			}
		}
	}

	[ContextMenu ("Clean")]
	public void clean ()
	{
		int labelCount = labelList.Count;
		UILabelEachLine label = null;
		for (int i = 0; i < labelCount; i++) {
			label = labelList [i];
			if (label != null)
				GameObject.DestroyImmediate (label.gameObject);
		}
		labelList.Clear ();
		oldContent = "";
	}
}


