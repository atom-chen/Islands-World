---@class TweenRotation : UITweener
---@field public from UnityEngine.Vector3
---@field public to UnityEngine.Vector3
---@field public quaternionLerp System.Boolean
---@field public cachedTransform UnityEngine.Transform
---@field public value UnityEngine.Quaternion

local m = { }
---public TweenRotation .ctor()
---@return TweenRotation
function m.New() end
---public TweenRotation Begin(GameObject go, Single duration, Quaternion rot)
---@return TweenRotation
---@param optional GameObject go
---@param optional Single duration
---@param optional Quaternion rot
function m.Begin(go, duration, rot) end
---public Void SetStartToCurrentValue()
function m:SetStartToCurrentValue() end
---public Void SetEndToCurrentValue()
function m:SetEndToCurrentValue() end
TweenRotation = m
return m
