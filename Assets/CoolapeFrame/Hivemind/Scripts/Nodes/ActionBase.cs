using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hivemind;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class  ActionBase
{
	public abstract  Hashtable Serialize ();

	public abstract void Deserialize (Hashtable map);

	public static ActionBase newAction (Hashtable map)
	{
		if (map == null) {
			return null;
		}
		return null;
	}

	#if UNITY_EDITOR
	public abstract void DrawInspector (Node node);
	#endif
}
