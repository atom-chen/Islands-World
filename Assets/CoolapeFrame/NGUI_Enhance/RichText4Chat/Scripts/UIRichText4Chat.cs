/// <summary>
/// User interface rich text4 chat.
/// add by chenbin
/// 2016-03-22
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// User interface rich text4 chat.
/// </summary>
public class UIRichText4Chat : MonoBehaviour
{
	public UILabel _label;

	public UILabel label {
		get {
			if (_label == null) {
				_label = GetComponent<UILabel> ();
			}
			return _label;
		}
		set {
			_label = value;
		}
	}

	[HideInInspector]
	public UIAtlas
		faceAtlas;
	//表情所在的图集
	public string atlasName;
	public int faceSize = 30;
	public float faceScaleOffset = 1;
	//表情图片的大小（最好是font大小的整数倍，当与字体是一样大小时为最宜）
	//	public static List<string> faceList = new List<string> ();
	public string faceHead = "";
	public bool isFullSpace = true;
	//用一个全角空格来占表情的位置,***因次使用的font中需要包含全角空格***
	//	[HideInInspector]
	public float
		spaceSize = -1;
	public static string
		_FaceChar_ = "　";
	[HideInInspector]
	public int
		spaceNumber = -1;
	[HideInInspector]
	public string
		faceStr = "";
	public static SpritePool pool = new SpritePool ();
	public List<UISprite> spList = new List<UISprite> ();

	[ContextMenu ("init")]
	public void init ()
	{
		if (isFullSpace) {
			_FaceChar_ = "　";		//用一个全角空格来占表情的位置,***因次使用的font中需要包含全角空格***
		} else {
			_FaceChar_ = ".";		//用半角空格来占位不知道为何有问题，因此用一个“.“来占位
		}
		calculateSpaceSize ();
	}

	public void calculateSpaceSize ()
	{
		string oldStr = label.text;
		mTempVerts.Clear ();
		mTempIndices.Clear ();
		
		label.text = _FaceChar_;
		string text = label.processedText;
		label.UpdateNGUIText ();
		NGUIText.PrintExactCharacterPositions (text, mTempVerts, mTempIndices);
		if (mTempVerts.size > 1) {
			spaceSize = mTempVerts [mTempVerts.size - 1].x - mTempVerts [0].x;
			spaceNumber = Mathf.CeilToInt (faceSize / spaceSize);
		}
		mTempVerts.Clear ();
		mTempIndices.Clear ();
		label.text = oldStr;
		faceStr = "";
		for (int i = 0; i < spaceNumber; i++) {
			faceStr += _FaceChar_;
		}
	}

	string mText = "";

	public string value {
		get {
			return mText;
		}
		set {
			if (mText != value) {
				mText = value;
				onTextChanged (gameObject);
			}
		}
	}

	public string wrapFaceName (string faceName)
	{
		return string.IsNullOrEmpty (faceHead) ? faceName : faceHead + faceName;
	}

	public void onInputChanged (GameObject go)
	{
		if (go == null)
			return;
		UIInput input = go.GetComponent<UIInput> ();
		if (input == null)
			return;
		value = input.value;
	}

	static BetterList<Vector3> mTempVerts = new BetterList<Vector3> ();
	static BetterList<int> mTempIndices = new BetterList<int> ();
	bool isFinishInit = false;

	[ContextMenu ("Execute Text Changed")]
	public void onTextChanged (GameObject go)
	{
		if (!isFinishInit) {
			isFinishInit = true;
			init ();
		}
		
		clean ();
		mTempVerts.Clear ();
		mTempIndices.Clear ();
		string str = findFace ();
		label.text = str;
		string text = label.processedText;
		
		label.UpdateNGUIText ();
		NGUIText.PrintExactCharacterPositions (label.text, mTempVerts, mTempIndices);
		//		for (int i=0; i < mTempVerts.size; i++) {
		//			Debug.Log (mTempVerts [i]);
		//		}
		ArrayList keyList = new ArrayList ();
		keyList.AddRange (facesMap.Keys);
		int count = keyList.Count;
		int index = 0;
		string faceName = "";
		Vector3 pos = Vector3.zero;
		for (int i = 0; i < count; i++) {
			index = (int)(keyList [i]);
			if (index * 2+1 < mTempVerts.size) {
				faceName = facesMap [index].ToString ();
				//			Debug.Log ("index==" + index);
				//			Debug.Log("faceName==" + faceName);
				pos = calculatePos (mTempVerts [index * 2], mTempVerts [index * 2 + 1]);
				showFace (faceName, pos);
			}
		}
		keyList.Clear ();
		mTempVerts.Clear ();
		mTempIndices.Clear ();
	}

	public Vector3 calculatePos (Vector3 pos1, Vector3 pos2)
	{
		float offsetX = label.printedSize.x;
		if (label.overflowMethod == UILabel.Overflow.ResizeHeight) {
			offsetX = label.localSize.x;
		}
		
		Vector3 pos = pos1;
		Vector3 diff = pos2 - pos;
		switch (label.pivot) {
		case UIWidget.Pivot.Center:
			pos.x -= offsetX / 2.0f;
			pos.y += label.printedSize.y / 2.0f;
			pos.y += diff.y / 2.0f;
			break;
		case UIWidget.Pivot.TopLeft:
			pos.y += diff.y / 2.0f;
			break;
		case UIWidget.Pivot.BottomLeft:
			pos.y += label.printedSize.y;
			pos.y += diff.y / 2.0f;
			break;
		case UIWidget.Pivot.Left:
			pos.y += label.printedSize.y / 2.0f;
			pos.y += diff.y / 2.0f;
			break;
		case UIWidget.Pivot.Right:
			pos.x -= offsetX;
			pos.y += label.printedSize.y / 2.0f;
			pos.y += diff.y / 2.0f;
			break;
		case UIWidget.Pivot.TopRight:
			pos.x -= offsetX;
			pos.y += diff.y / 2.0f;
			break;
		case UIWidget.Pivot.BottomRight:
			pos.x -= offsetX;
			pos.y += label.printedSize.y;
			pos.y += diff.y / 2.0f;
			break;
		case UIWidget.Pivot.Top:
			pos.x -= offsetX / 2.0f;
			pos.y += diff.y / 2.0f;
			break;
		case UIWidget.Pivot.Bottom:
			pos.x -= offsetX / 2.0f;
			pos.y += label.printedSize.y;
			pos.y += diff.y / 2.0f;
			break;
		}
		return pos;
	}

	public void showFace (string faceName, Vector3 pos)
	{
		UISprite sp = pool.getSprite ();
		spList.Add (sp);
		NGUITools.SetLayer (sp.gameObject, gameObject.layer);
		sp.pivot = UIWidget.Pivot.Left;
		sp.atlas = faceAtlas;
		sp.spriteName = wrapFaceName (faceName);
		sp.SetDimensions (faceSize, faceSize);
		sp.transform.parent = transform;
		sp.transform.localScale = Vector3.one*faceScaleOffset;
		if (sp.transform.parent == label.transform) {
			sp.transform.localPosition = pos;
		} else {
			sp.transform.localPosition = pos + label.transform.localPosition;
		}
		sp.depth = label.depth + 1;
		NGUITools.SetActive (sp.gameObject, true);
	}

	Hashtable facesMap = new Hashtable ();

	public string findFace ()
	{
		facesMap.Clear ();
		if (string.IsNullOrEmpty (value))
			return "";
		
		int len = value.Length;
		string str = "";
		int offset = 0;
		string faceName = "";
		for (int i = 0; i < len; i++) {
			//			Debug.Log("val==[" + value[i] + "]");
			if (value [i] == '\n') {
				offset += 1;
			} else if (value [i] == '\\' && (i + 1 < len) && value [i + 1] == 'n') {
				offset += 2;
			} else if(value [i] == '[' && i+7 < len && value[i+7] == ']') {
				offset += 8;
			} else if(value[i] == '[' && i+2 < len && value[i+1] == '-' && value[i+2] == ']') {
				offset += 3;
			}
			//			Debug.Log ("val==[" + value [i] + "]==" + offset);
			if (value [i] == '#') {
				faceName = "";
				for (int j = i + 1; j < len; j++) {
					if (value [j] == '\n') {
						offset += 1;
					} else if (value [j] == '\\' && (j + 1 < len) && value [j + 1] == 'n') {
						offset += 2;
					} else if(value [i] == '[' && i+7 < len && value[i+7] == ']') {
						offset += 8;
					} else if(value[i] == '[' && i+2 < len && value[i+1] == '-' && value[i+2] == ']') {
						offset += 3;
					}
					//					Debug.Log ("val==[" + value [j] + "]==" + offset);
					if (value [j] == '#') {
						if (faceAtlas.spriteMap.Contains (wrapFaceName (faceName))) {
							facesMap [str.Length - offset] = faceName;
							str += faceStr;
							faceName = "";
							i = j;
						} else {
							str += ("#" + faceName);
							faceName = "";
							i = j - 1;
						}
						break;
					} else {
						faceName += value [j];
						if (j == len - 1) {
							i = j;
						}
					}
				}
			} else {
				str += value [i];
			}
		}
		if (!string.IsNullOrEmpty (faceName)) {
			str += "#" + faceName;
		}
		return str;
	}

	[ContextMenu ("Clean")]
	public void clean ()
	{
		int count = spList.Count;
		for (int i = 0; i < count; i++) {
			pool.retSprite (spList [i]);
			//			spList [i].transform.parent = null;
//			NGUITools.SetActive (spList [i].gameObject, false);
		}
		spList.Clear ();
	}

	public void OnDisable ()
	{
		clean ();
	}

	public void OnEnable ()
	{
		onTextChanged (gameObject);
	}
	
	
	//图表对像池
	public class SpritePool
	{
		Queue<UISprite> queue = new Queue<UISprite> ();

		public void clean ()
		{
			queue.Clear ();
		}

		public UISprite getSprite ()
		{
			if (queue.Count <= 0) {
				return newSprite ();
			} else {
				UISprite sp = queue.Dequeue ();
				try {
					if (sp == null && sp.gameObject == null) {
						return getSprite ();
					} else {
						return sp;
					}
				} catch (System.Exception e) {
//					Debug.LogError (e);
					return getSprite ();
				}
			}
		}

		public void retSprite (UISprite sp)
		{
			try {
				queue.Enqueue (sp);
				sp.gameObject.transform.parent = null;
				NGUITools.SetActive (sp.gameObject, false);
			} catch (System.Exception e) {
				Debug.LogWarning (e);
			}
		}

		public UISprite newSprite ()
		{
			GameObject go = new GameObject ("Sprite");
			go.transform.parent = null;
			UISprite sp = go.AddComponent<UISprite> ();
			NGUITools.SetActive (go, false);
			return sp;
		}
	}
}
