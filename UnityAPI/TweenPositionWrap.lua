---@class TweenPosition : UITweener
---@field public from UnityEngine.Vector3
---@field public to UnityEngine.Vector3
---@field public worldSpace System.Boolean
---@field public cachedTransform UnityEngine.Transform
---@field public value UnityEngine.Vector3
local m = { }
---public TweenPosition .ctor()
---@return TweenPosition
function m.New() end
---public TweenPosition Begin(GameObject go, Single duration, Vector3 pos)
---public TweenPosition Begin(GameObject go, Single duration, Vector3 pos, Boolean worldSpace)
---@return TweenPosition
---@param GameObject go
---@param optional Single duration
---@param optional Vector3 pos
---@param optional Boolean worldSpace
function m.Begin(go, duration, pos, worldSpace) end
---public Void SetStartToCurrentValue()
function m:SetStartToCurrentValue() end
---public Void SetEndToCurrentValue()
function m:SetEndToCurrentValue() end
TweenPosition = m
return m
