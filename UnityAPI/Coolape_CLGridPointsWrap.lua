---@class Coolape.CLGridPoints : UnityEngine.MonoBehaviour
---@field public width System.Int32
---@field public length System.Int32
---@field public widthInterval System.Single
---@field public lengthInterval System.Single
---@field public needCreateObj System.Boolean
---@field public root UnityEngine.Transform
---@field public points UnityEngine.Transform
---@field public vectors UnityEngine.Vector3
---@field public offsetType Coolape.CLGridPoints.OffsetType
---@field public offsetValue System.Single

local m = { }
---public CLGridPoints .ctor()
---@return CLGridPoints
function m.New() end
---public Void refreshPoint()
function m:refreshPoint() end
Coolape.CLGridPoints = m
return m
