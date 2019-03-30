//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Plays the specified sound.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Play Sound")]
public class UIPlaySound : MonoBehaviour
{
	public enum Trigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
		Custom,
		OnEnable,
		OnDisable,
	}

	[HideInInspector] // add by chenbin
	public AudioClip audioClip;
	public Trigger trigger = Trigger.OnClick;

	[Range(0f, 1f)] public float volume = 1f;
	[Range(0f, 2f)] public float pitch = 1f;

	bool mIsOver = false;

	bool canPlay
	{
		get
		{
			if (!enabled) return false;
			UIButton btn = GetComponent<UIButton>();
			return (btn == null || btn.isEnabled);
		}
	}

	void OnEnable ()
	{
		if (trigger == Trigger.OnEnable)
			Play();	// modify by chenbin
	}

	void OnDisable ()
	{
		if (trigger == Trigger.OnDisable)
			Play();	// modify by chenbin
	}

	void OnHover (bool isOver)
	{
		if (trigger == Trigger.OnMouseOver)
		{
			if (mIsOver == isOver) return;
			mIsOver = isOver;
		}

		if (canPlay && ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut)))
			Play();	// modify by chenbin
	}

	void OnPress (bool isPressed)
	{
		if (trigger == Trigger.OnPress)
		{
			if (mIsOver == isPressed) return;
			mIsOver = isPressed;
		}

		if (canPlay && ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease)))
			Play();	// modify by chenbin
	}

	void OnClick ()
	{
		if (canPlay && trigger == Trigger.OnClick)
			Play();	// modify by chenbin
	}

	void OnSelect (bool isSelected)
	{
		if (canPlay && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
			OnHover(isSelected);
	}

	public virtual void Play ()
	{
		NGUITools.PlaySound(audioClip, volume, pitch);
	}
}
