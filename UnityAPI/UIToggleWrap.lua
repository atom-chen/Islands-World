---@class UIToggle : UIWidgetContainer
---@field public list BetterList1UIToggle
---@field public current UIToggle
---@field public group System.Int32
---@field public activeSprite UIWidget
---@field public activeAnimation UnityEngine.Animation
---@field public animator UnityEngine.Animator
---@field public startsActive System.Boolean
---@field public instantTween System.Boolean
---@field public optionCanBeNone System.Boolean
---@field public onChange System.Collections.Generic.List1EventDelegate
---@field public validator UIToggle.Validate
---@field public value System.Boolean

local m = { }
---public UIToggle .ctor()
---@return UIToggle
function m.New() end
---public UIToggle GetActiveToggle(Int32 group)
---@return UIToggle
---@param optional Int32 group
function m.GetActiveToggle(group) end
---public Void Set(Boolean state)
---@param optional Boolean state
function m:Set(state) end
UIToggle = m
return m
