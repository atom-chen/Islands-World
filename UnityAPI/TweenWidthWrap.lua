---@class TweenWidth : UITweener
---@field public from System.Int32
---@field public to System.Int32
---@field public updateTable System.Boolean
---@field public cachedWidget UIWidget
---@field public value System.Int32

local m = { }
---public TweenWidth .ctor()
---@return TweenWidth
function m.New() end
---public TweenWidth Begin(UIWidget widget, Single duration, Int32 width)
---@return TweenWidth
---@param optional UIWidget widget
---@param optional Single duration
---@param optional Int32 width
function m.Begin(widget, duration, width) end
---public Void SetStartToCurrentValue()
function m:SetStartToCurrentValue() end
---public Void SetEndToCurrentValue()
function m:SetEndToCurrentValue() end
TweenWidth = m
return m
