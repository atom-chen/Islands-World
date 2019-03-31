---@class UnityEngine.AnimationClip : UnityEngine.Motion
---@field public events UnityEngine.AnimationEvent
---@field public length System.Single
---@field public frameRate System.Single
---@field public wrapMode UnityEngine.WrapMode
---@field public localBounds UnityEngine.Bounds
---@field public legacy System.Boolean
---@field public humanMotion System.Boolean
---@field public empty System.Boolean
---@field public hasGenericRootTransform System.Boolean
---@field public hasMotionFloatCurves System.Boolean
---@field public hasMotionCurves System.Boolean
---@field public hasRootCurves System.Boolean

local m = { }
---public AnimationClip .ctor()
---@return AnimationClip
function m.New() end
---public Void AddEvent(AnimationEvent evt)
---@param optional AnimationEvent evt
function m:AddEvent(evt) end
---public Void SampleAnimation(GameObject go, Single time)
---@param optional GameObject go
---@param optional Single time
function m:SampleAnimation(go, time) end
---public Void SetCurve(String relativePath, Type t, String propertyName, AnimationCurve curve)
---@param optional String relativePath
---@param optional Type t
---@param optional String propertyName
---@param optional AnimationCurve curve
function m:SetCurve(relativePath, type, propertyName, curve) end
---public Void EnsureQuaternionContinuity()
function m:EnsureQuaternionContinuity() end
---public Void ClearCurves()
function m:ClearCurves() end
UnityEngine.AnimationClip = m
return m
