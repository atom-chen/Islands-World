---@class Coolape.TweenSpriteFill : UITweener
---@field public from System.Single
---@field public to System.Single
---@field public value System.Single
local m = { }
---public TweenSpriteFill .ctor()
---@return TweenSpriteFill
function m.New() end
---public TweenSpriteFill Begin(GameObject go, Single duration, Single amount)
---@return TweenSpriteFill
---@param optional GameObject go
---@param optional Single duration
---@param optional Single amount
function m.Begin(go, duration, amount) end
---public Void SetStartToCurrentValue()
function m:SetStartToCurrentValue() end
---public Void SetEndToCurrentValue()
function m:SetEndToCurrentValue() end
Coolape.TweenSpriteFill = m
return m
