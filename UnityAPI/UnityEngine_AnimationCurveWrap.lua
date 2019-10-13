---@class UnityEngine.AnimationCurve
---@field public keys UnityEngine.Keyframe
---@field public Item UnityEngine.Keyframe
---@field public length System.Int32
---@field public preWrapMode UnityEngine.WrapMode
---@field public postWrapMode UnityEngine.WrapMode
local m = { }
---public AnimationCurve .ctor()
---public AnimationCurve .ctor(Keyframe[] keys)
---@return AnimationCurve
---@param Keyframe[] keys
function m.New(keys) end
---public Single Evaluate(Single time)
---@return number
---@param optional Single time
function m:Evaluate(time) end
---public Int32 AddKey(Keyframe key)
---public Int32 AddKey(Single time, Single value)
---@return number
---@param Single time
---@param optional Single value
function m:AddKey(time, value) end
---public Int32 MoveKey(Int32 index, Keyframe key)
---@return number
---@param optional Int32 index
---@param optional Keyframe key
function m:MoveKey(index, key) end
---public Void RemoveKey(Int32 index)
---@param optional Int32 index
function m:RemoveKey(index) end
---public Void SmoothTangents(Int32 index, Single weight)
---@param optional Int32 index
---@param optional Single weight
function m:SmoothTangents(index, weight) end
---public AnimationCurve Constant(Single timeStart, Single timeEnd, Single value)
---@return AnimationCurve
---@param optional Single timeStart
---@param optional Single timeEnd
---@param optional Single value
function m.Constant(timeStart, timeEnd, value) end
---public AnimationCurve Linear(Single timeStart, Single valueStart, Single timeEnd, Single valueEnd)
---@return AnimationCurve
---@param optional Single timeStart
---@param optional Single valueStart
---@param optional Single timeEnd
---@param optional Single valueEnd
function m.Linear(timeStart, valueStart, timeEnd, valueEnd) end
---public AnimationCurve EaseInOut(Single timeStart, Single valueStart, Single timeEnd, Single valueEnd)
---@return AnimationCurve
---@param optional Single timeStart
---@param optional Single valueStart
---@param optional Single timeEnd
---@param optional Single valueEnd
function m.EaseInOut(timeStart, valueStart, timeEnd, valueEnd) end
---public Boolean Equals(Object o)
---public Boolean Equals(AnimationCurve other)
---@return bool
---@param optional AnimationCurve other
function m:Equals(other) end
---public Int32 GetHashCode()
---@return number
function m:GetHashCode() end
UnityEngine.AnimationCurve = m
return m
