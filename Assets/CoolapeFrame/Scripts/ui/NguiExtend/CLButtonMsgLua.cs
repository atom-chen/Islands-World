/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   按键事件绑定到lua
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;

namespace Coolape
{
	[AddComponentMenu("NGUI/Button Message 4 Lua")]
	public class CLButtonMsgLua : UIEventListener
	{
		public enum Trigger
		{
			OnClick,
			OnMouseOver,
			OnMouseOut,
			OnPress,
			OnRelease,
			OnDoubleClick,
			OnDrag,
			OnDrop,
			OnKey,
		}

		public CLPanelLua target;
		public CLCellLua target2;
		public Trigger trigger = Trigger.OnClick;
		public string functionName = "";

		void OnClick()
		{
			if (target != null && trigger == Trigger.OnClick) {
				target.onClick4Lua(gameObject, functionName);
			}
			if (target2 != null && trigger == Trigger.OnClick) {
				target2.onClick4Lua(gameObject, functionName);
			}
		}

		void OnDoubleClick()
		{
			if (target != null && trigger == Trigger.OnDoubleClick)
				target.onDoubleClick4Lua(gameObject, functionName);
			if (target2 != null && trigger == Trigger.OnDoubleClick)
				target2.onDoubleClick4Lua(gameObject, functionName);
		}

		void OnHover(bool isOver)
		{
			if (target != null) {
				if (((isOver && trigger == Trigger.OnMouseOver) ||
				    (!isOver && trigger == Trigger.OnMouseOut))) {
					target.onHover4Lua(gameObject, functionName, isOver);
				}
			}
			if (target2 != null) {
				if (((isOver && trigger == Trigger.OnMouseOver) ||
				    (!isOver && trigger == Trigger.OnMouseOut))) {
					target2.onHover4Lua(gameObject, functionName, isOver);
				}
			}
		}

		void OnPress(bool isPressed)
		{
			if (target != null) {
				if (((isPressed && trigger == Trigger.OnPress) ||
				    (!isPressed && trigger == Trigger.OnRelease)))
					target.onPress4Lua(gameObject, functionName, isPressed);
			}
			if (target2 != null) {
				if (((isPressed && trigger == Trigger.OnPress) ||
				    (!isPressed && trigger == Trigger.OnRelease)))
					target2.onPress4Lua(gameObject, functionName, isPressed);
			}
		}

		void OnSelect(bool isSelected)
		{
			if (target != null) {
				if (enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
					OnHover(isSelected);
			}
		}

		void OnDrag(Vector2 delta)
		{
			if (target != null && trigger == Trigger.OnDrag)
				target.onDrag4Lua(gameObject, functionName, delta);
			if (target2 != null && trigger == Trigger.OnDrag)
				target2.onDrag4Lua(gameObject, functionName, delta);
		}

		void OnDrop(GameObject go)
		{
			if (target != null && trigger == Trigger.OnDrop)
				target.onDrop4Lua(gameObject, functionName, go);
			if (target2 != null && trigger == Trigger.OnDrop)
				target2.onDrop4Lua(gameObject, functionName, go);
		}

		void OnKey(KeyCode key)
		{
			if (target != null && trigger == Trigger.OnDrop)
				target.onKey4Lua(gameObject, functionName, key);
			if (target2 != null && trigger == Trigger.OnDrop)
				target2.onKey4Lua(gameObject, functionName, key);
		}
	}
}
