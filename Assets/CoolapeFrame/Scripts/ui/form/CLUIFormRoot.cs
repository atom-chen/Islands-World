using UnityEngine;
using System.Collections;
using Coolape;
using XLua;

public class CLUIFormRoot : MonoBehaviour
{
	public string jsonKey = "";

	public CLUIElement[] inputs {
		get {
			return GetComponentsInChildren<CLUIElement> ();
		}
	}

	public string checkValid ()
	{
		string msg = "";
		int count = inputs == null ? 0 : inputs.Length;
		for (int i = 0; i < count; i++) {
			msg += inputs [i].checkValid ();
		}
		return msg;
	}

	void setVal (object map, object key, object val)
	{
		if (map is LuaTable) {
			((LuaTable)map).SetInPath(key.ToString(), val);
		} else if (map is Hashtable) {
			((Hashtable)map) [key] = val;
		}
	}

	object getVal (object map, object key)
	{
		object ret = "";
		if (map is LuaTable) {
			ret = ((LuaTable)map).GetInPath<object>(key.ToString());
		} else if (map is Hashtable) {
			ret = ((Hashtable)map) [key];
		}
		return ret == null ? "" : ret;
	}

	public object getValue (bool isLuatable = false)
	{
		return getValue (null, isLuatable);
	}

	public object getValue (object map, bool isLuatable)
	{
		object r = getValue (transform, map, isLuatable);
#if UNITY_EDITOR
		if (r is Hashtable) {
			Debug.Log (Utl.MapToString (r as Hashtable));
		}
#endif
		return r;
	}

	public object getValue (Transform tr, object map, bool isLuaTable)
	{
		if (map == null) {
			if (isLuaTable) {
				map = CLBaseLua.mainLua.NewTable ();
			} else {
				map = new Hashtable ();
			}
		}
		CLUIElement cell = null;
		CLUIFormRoot root = null;
		int count = tr.childCount;
		for (int i = 0; i < count; i++) {
			cell = tr.GetChild (i).GetComponent<CLUIElement> ();
			if (cell != null && !string.IsNullOrEmpty (cell.jsonKey)) {
				setVal (map, cell.jsonKey, cell.value);
			}

			root = tr.GetChild (i).GetComponent<CLUIFormRoot> ();
			if (root != null && !string.IsNullOrEmpty (root.jsonKey)) {
				setVal (map, root.jsonKey, getValue (tr.GetChild (i), null, isLuaTable));
			} else {
				map = getValue (tr.GetChild (i), map, isLuaTable);
			}
		}
		return map;
	}

	public void setValue (object map)
	{
		if (map is LuaTable) {
			setValue (transform, map, true);
		} else {
			setValue (transform, map);
		}
	}

	public void setValue (Transform tr, object map, bool isLuatable = false)
	{
		if (map == null) {
			map = new Hashtable ();
		}
		
		CLUIElement cell = null;
		CLUIFormRoot root = null;
		int count = tr.childCount;
		Transform cellTr = null;
		for (int i = 0; i < count; i++) {
			cellTr = tr.GetChild (i);
			cell = cellTr.GetComponent<CLUIElement> ();
			if (cell != null && !string.IsNullOrEmpty (cell.jsonKey)) {
				if (cell.valueIsNumber) {
					cell.value = getVal (map, cell.jsonKey).ToString ();
//					cell.value = MapEx.getInt(map, cell.jsonKey).ToString();
				} else {
					cell.value = getVal (map, cell.jsonKey).ToString ();
				}
			}
			
			root = cellTr.GetComponent<CLUIFormRoot> ();
			if (root != null) {
				if (!string.IsNullOrEmpty (root.jsonKey)) {
					setValue (root.transform, getVal (map, root.jsonKey), isLuatable);
				} else {
					setValue (root.transform, map, isLuatable);
				}
			} else {
				if (cellTr.childCount > 0) {
					setValue (cellTr, map, isLuatable);
				}
			}
		}
	}
}
