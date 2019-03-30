---@class TweenScale : UITweener
---@field public from UnityEngine.Vector3
---@field public to UnityEngine.Vector3
---@field public updateTable System.Boolean
---@field public cachedTransform UnityEngine.Transform
---@field public value UnityEngine.Vector3

local m = { }
---public TweenScale .ctor()
---@return TweenScale
function m.New() end
---public TweenScale Begin(GameObject go, Single duration, Vector3 scale)
---@return TweenScale
---@param optional GameObject go
---@param optional Single duration
---@param optional Vector3 scale
function m.Begin(go, duration, scale) end
---public Void SetStartToCurrentValue()
function m:SetStartToCurrentValue() end
---public Void SetEndToCurrentValue()
function m:SetEndToCurrentValue() end
TweenScale = m
return m
