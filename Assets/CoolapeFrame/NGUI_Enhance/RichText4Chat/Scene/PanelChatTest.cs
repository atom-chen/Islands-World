using UnityEngine;
using System.Collections;

public class PanelChatTest : MonoBehaviour {
	public TweenPosition twPosFaceList;
	public UIInput input;
	public UIRichText4Chat richText;
	public UIRichText4Chat labelContent;

	public void onClickSend(GameObject go) {
		if (string.IsNullOrEmpty (input.value))
			return;
		labelContent.value += "\n" + input.value;
		input.value = "";
	}

	public void onClick4ShowFaceList(GameObject go) {
		twPosFaceList.Play (true);
	}

	public void onSelectedFace(GameObject go) {
		twPosFaceList.Play (false);
		UISprite sp = go.GetComponent<UISprite> ();
		string[] strs = sp.spriteName.Split ('_');
		string faceName = "";
		if (strs.Length > 1) {
			faceName = "#" + strs[1] + "#";
		} else {
			faceName = "#" + sp.spriteName + "#";
		}
		input.Insert (faceName);
	}

}
