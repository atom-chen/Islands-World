---@class Coolape.UIMoveToCell : UnityEngine.MonoBehaviour
---@field public speed System.Single
---@field public moveCurve UnityEngine.AnimationCurve

local m = { }
---public UIMoveToCell .ctor()
---@return UIMoveToCell
function m.New() end
---public Void moveTo(GameObject specificCell, Boolean isReverse)
---public Void moveTo(Int32 index, Boolean isReverse, Boolean reset, Object finishCallback)
---@param Int32 index
---@param Boolean isReverse
---@param optional Boolean reset
---@param optional Object finishCallback
function m:moveTo(index, isReverse, reset, finishCallback) end
Coolape.UIMoveToCell = m
return m
