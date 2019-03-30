//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Attaching this script to an object will let you trigger remote functions using NGUI events.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Event Trigger")]
public class UIEventTrigger : MonoBehaviour
{
	static public UIEventTrigger current;

	public List<EventDelegate> onHoverOver = new List<EventDelegate>();
	public List<EventDelegate> onHoverOut = new List<EventDelegate>();
	public List<EventDelegate> onPress = new List<EventDelegate>();
	public List<EventDelegate> onRelease = new List<EventDelegate>();
	public List<EventDelegate> onSelect = new List<EventDelegate>();
	public List<EventDelegate> onDeselect = new List<EventDelegate>();
	public List<EventDelegate> onClick = new List<EventDelegate>();
	public List<EventDelegate> onDoubleClick = new List<EventDelegate>();
	public List<EventDelegate> onDragStart = new List<EventDelegate>();
	public List<EventDelegate> onDragEnd = new List<EventDelegate>();
	public List<EventDelegate> onDragOver = new List<EventDelegate>();
	public List<EventDelegate> onDragOut = new List<EventDelegate>();
	public List<EventDelegate> onDrag = new List<EventDelegate>();
	#region add by chenbin
	public List<EventDelegate> onBecameVisible = new List<EventDelegate>();
	public List<EventDelegate> onBecameInvisible = new List<EventDelegate>();
	#endregion

	void OnHover (bool isOver)
	{
		if (current != null) return;
		current = this;
		if (isOver) EventDelegate.Execute(onHoverOver, gameObject); //modify by chenbin
		else EventDelegate.Execute(onHoverOut, gameObject); // modify by chenbin
		current = null;
	}

	void OnPress (bool pressed)
	{
		if (current != null) return;
		current = this;
		if (pressed) EventDelegate.Execute(onPress, gameObject); // modify by chenbin
		else EventDelegate.Execute(onRelease, gameObject); // modify by chenbin
		current = null;
	}

	void OnSelect (bool selected)
	{
		if (current != null) return;
		current = this;
		if (selected) EventDelegate.Execute(onSelect, gameObject); // modify by chenbin
		else EventDelegate.Execute(onDeselect, gameObject); // modify by chenbin
		current = null;
	}

	void OnClick ()
	{
		if (current != null) return;
		current = this;
		EventDelegate.Execute(onClick, gameObject); // modify by chenbin
		current = null;
	}

	void OnDoubleClick ()
	{
		if (current != null) return;
		current = this;
		EventDelegate.Execute(onDoubleClick, gameObject); // modify by chenbin
		current = null;
	}

	void OnDragStart ()
	{
		if (current != null) return;
		current = this;
		EventDelegate.Execute(onDragStart, gameObject); // modify by chenbin
		current = null;
	}

	void OnDragEnd ()
	{
		if (current != null) return;
		current = this;
		EventDelegate.Execute(onDragEnd, gameObject); // modify by chenbin
		current = null;
	}

	void OnDragOver (GameObject go)
	{
		if (current != null) return;
		current = this;
		EventDelegate.Execute(onDragOver, go); // modify by chenbin
		current = null;
	}

	void OnDragOut (GameObject go)
	{
		if (current != null) return;
		current = this;
		EventDelegate.Execute(onDragOut, go); // modify by chenbin
		current = null;
	}

	void OnDrag (Vector2 delta)
	{
		if (current != null) return;
		current = this;
		EventDelegate.Execute(onDrag, gameObject); // modify by chenbin
		current = null;
	}

	#region add by chenbin
	public  void OnBecameInvisible ()
	{
		EventDelegate.Execute(onBecameInvisible, gameObject); // modify by chenbin
	}

	public void OnBecameVisible ()
	{
		EventDelegate.Execute(onBecameVisible, gameObject); // modify by chenbin
	}
	#endregion
}
