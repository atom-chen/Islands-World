---@class Coolape.CLEjector : UnityEngine.MonoBehaviour
---@field public firePoints UnityEngine.Transform
---@field public transform UnityEngine.Transform
local m = { }
---public CLEjector .ctor()
---@return CLEjector
function m.New() end
---public Void fire(CLUnit attacker, CLUnit target, Object bulletAttr, Object data, Object callbak)
---public Void fire(Int32 numPoints, Int32 numEach, Single angle, CLUnit attacker, CLUnit target, Object bulletAttr, Object data, Object callbak)
---public Void fire(Int32 numPoints, Int32 numEach, Single angle, Single offsetTime, CLUnit attacker, CLUnit target, Object bulletAttr, Object data, Object callbak)
---public Void fire(Int32 firePointIndex, Int32 numPoints, Int32 numEach, Single angle, Single offsetTime, CLUnit attacker, CLUnit target, Object attr, Object data, Object callbak)
---@param Int32 firePointIndex
---@param Int32 numPoints
---@param Int32 numEach
---@param Single angle
---@param Single offsetTime
---@param optional CLUnit attacker
---@param optional CLUnit target
---@param optional Object attr
---@param optional Object data
---@param optional Object callbak
function m:fire(firePointIndex, numPoints, numEach, angle, offsetTime, attacker, target, attr, data, callbak) end
Coolape.CLEjector = m
return m
