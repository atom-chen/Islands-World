using UnityEngine;
using System.Collections;
using Coolape;

public class CLUIElement : MonoBehaviour {
	public string jsonKey = "";
	public string formatValue = "";
	public UILabel labeName;
	public string defaultName;
	public bool canNull = false;
	public bool checkIDCard = false;
	public int minLen = 0;
	public int maxLen = 0;
	public UIWidget spriteBg = null;
	public bool valueIsNumber = false;
	public Color inValidColor = Color.yellow;
	Color defaultColor = Color.white;

	void Start() {
		if(sprite != null) {
			defaultColor = sprite.color;
		}
	}

	public object value {
		get {
			object ret = getValue();
			if(valueIsNumber) {
				return NumEx.stringToDouble(ret.ToString());
			}
			return ret;
		}
		set {
			string val = value == null ? (valueIsNumber ? "0" :  "") : value.ToString();
			if(!string.IsNullOrEmpty(formatValue)) {
				val = string.Format(formatValue, val);
			}
			setValue(val);
			setInvalidColor (true);
		}
	}
	
	public object getValue() {
		UIInput input = GetComponent<UIInput>();
		if(input != null) {
			return input.value.Trim();
		}
		UIPopupList popList = GetComponent<UIPopupList>();
		if(popList != null) {
			if(popList.value == null) {
				return "";
			} else {
				return popList.value.Trim();
			}
		}
		
		UIToggle toggle = GetComponent<UIToggle>();
		if(toggle != null) {
			return toggle.value;
		}

		UILabel lable = GetComponent<UILabel>();
		if(lable != null) {
			return lable.text.Trim();
		}
		return "";
	}
	public void setValue(object obj) {
		string value = obj == null ? "" : obj.ToString();
		UIInput input = GetComponent<UIInput>();
		if(input != null) {
			input.value = value;
			return;
		}
		UIPopupList popList = GetComponent<UIPopupList>();
		if(popList != null) {
			if(popList.valueItems.Count == popList.items.Count) {
				int index = popList.valueItems.IndexOf(value);
				if(index >= 0) {
					popList.value = popList.items[index];
				} else {
					popList.value = value;
				}
			} else {
				popList.value = value;
			}
			return;
		}
		
		UIToggle toggle = GetComponent<UIToggle>();
		if(toggle != null) {
			try{
				toggle.value = bool.Parse(value);
			} catch {
				toggle.value = false;
			}
			return;
		}
		UILabel lable = GetComponent<UILabel>();
		if(lable != null) {
			lable.text = value;
			return;
		}
	}

	public UIWidget sprite {
		get {
			if(spriteBg != null) return spriteBg;
			UIInput input = GetComponent<UIInput>();
			if(input != null) {
				spriteBg = input.GetComponent<UIWidget>();
				if(spriteBg == null) {
					spriteBg = input.GetComponentInChildren<UIWidget>();
				}
				return spriteBg;
			}
			UIPopupList popList = GetComponent<UIPopupList>();
			if(popList != null) {
				spriteBg = popList.GetComponent<UIWidget>();
				if(spriteBg == null) {
					spriteBg = popList.GetComponentInChildren<UIWidget>();
				}
				return spriteBg;
			}
			return spriteBg;
		}
	}
	public void setInvalidColor(bool isValid) {
		if(sprite != null) {
			if(isValid) {
				spriteBg.color = defaultColor;
			} else {
				spriteBg.color = inValidColor;
			}
		}
	}

	public string checkValid() {
		string msg = _checkValid();
		if(!string.IsNullOrEmpty(msg)) {
			setInvalidColor(false);
		} else {
			setInvalidColor(true);
		}
		return msg;
	}
	public string name {
		get {
			if(labeName != null) {
				return labeName.text.Replace(":", "");
			}
			return defaultName;
		}
	}
	public string _checkValid() {
		string v = value.ToString();
		if(!canNull && string.IsNullOrEmpty(v)) {
			return PStr.b().a (name).a ("不能为空\n").e ();
		}
		
		if(minLen > 0 && v.Length < minLen) {
			return PStr.b().a (name).a ( "长度至少有").a (minLen).a("位\n").e ();
		}
		if(maxLen > 0 && v.Length > maxLen) {
			return PStr.b().a (name).a ("长度最长").a (maxLen).a ("位\n").e ();
		}
		if (checkIDCard) {
			string str = CLUIFormUtl.IdentityCodeValid(v);
			if(!string.IsNullOrEmpty(str)) {
				return  str;
			}
		}
		return "";
	}
}
