---@class TweenAlpha : UITweener
---@field public from System.Single
---@field public to System.Single
---@field public value System.Single

local m = { }
---public TweenAlpha .ctor()
---@return TweenAlpha
function m.New() end
---public TweenAlpha Begin(GameObject go, Single duration, Single alpha)
---@return TweenAlpha
---@param optional GameObject go
---@param optional Single duration
---@param optional Single alpha
function m.Begin(go, duration, alpha) end
---public Void SetStartToCurrentValue()
function m:SetStartToCurrentValue() end
---public Void SetEndToCurrentValue()
function m:SetEndToCurrentValue() end
TweenAlpha = m
return m
