using UnityEngine;
using System.Collections;
using Coolape;

[RequireComponent (typeof(UIPopupList))]
public class CLUIElementPopList : UIEventListener
{
	UIPopupList _poplist;

	public UIPopupList poplist {
		get {
			if (_poplist == null) {
				_poplist = GetComponent<UIPopupList> ();
				_poplist.enabled = false;
			}
			return _poplist;
		}
	}

	public void OnClick ()
	{
		ArrayList values = new ArrayList ();
		values.AddRange (poplist.valueItems);
		if (poplist.items.Count == 0) {
			CLAlert.add (Localization.Get ("EmpyContent"), Color.white, 1, 1, false, Vector3.zero);
		}
		CLUIPopListPanel.show (poplist.items, values, (Callback)onSelectedValue);
	}

	void onSelectedValue (params object[] orgs)
	{
		string val = orgs [1].ToString ();
		poplist.value = val;
	}
}
