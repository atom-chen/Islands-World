---@class Coolape.EffectNum : UnityEngine.MonoBehaviour
---@field public isGui System.Boolean
---@field public speedCurve UnityEngine.AnimationCurve

local m = { }
---public EffectNum .ctor()
---@return EffectNum
function m.New() end
---public Void effectStart(Int32 to, Object back, Single delayTime)
---public Void effectStart(Int32 from, Int32 to, Object back, Single delayTime)
---@param Int32 from
---@param optional Int32 to
---@param optional Object back
---@param optional Single delayTime
function m:effectStart(from, to, back, delayTime) end
Coolape.EffectNum = m
return m
