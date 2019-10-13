---@class UnityEngine.Keyframe : System.ValueType
---@field public time System.Single
---@field public value System.Single
---@field public inTangent System.Single
---@field public outTangent System.Single
---@field public inWeight System.Single
---@field public outWeight System.Single
---@field public weightedMode UnityEngine.WeightedMode
local m = { }
---public Keyframe .ctor(Single time, Single value)
---public Keyframe .ctor(Single time, Single value, Single inTangent, Single outTangent)
---public Keyframe .ctor(Single time, Single value, Single inTangent, Single outTangent, Single inWeight, Single outWeight)
---@return Keyframe
---@param Single time
---@param Single value
---@param Single inTangent
---@param Single outTangent
---@param optional Single inWeight
---@param optional Single outWeight
function m.New(time, value, inTangent, outTangent, inWeight, outWeight) end
UnityEngine.Keyframe = m
return m
