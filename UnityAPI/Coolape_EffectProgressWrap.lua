---@class Coolape.EffectProgress : UnityEngine.MonoBehaviour
---@field public isGui System.Boolean
---@field public speedCurve UnityEngine.AnimationCurve
---@field public speed System.Single
---@field public slider UISlider

local m = { }
---public EffectProgress .ctor()
---@return EffectProgress
function m.New() end
---public Void effectStart(Single to, Object back, Single delayTime)
---public Void effectStart(Single from, Single to, Object back, Single delayTime)
---@param Single from
---@param optional Single to
---@param optional Object back
---@param optional Single delayTime
function m:effectStart(from, to, back, delayTime) end
Coolape.EffectProgress = m
return m
