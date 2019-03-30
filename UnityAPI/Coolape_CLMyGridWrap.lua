---@class Coolape.CLMyGrid : UnityEngine.MonoBehaviour
---@field public width System.Int32
---@field public length System.Int32
---@field public widthInterval System.Single
---@field public lengthInterval System.Single
---@field public needCreateObj System.Boolean
---@field public root UnityEngine.Transform
---@field public points UnityEngine.Transform
---@field public vectors UnityEngine.Vector3
---@field public offsetType Coolape.CLMyGrid.OffsetType
---@field public offsetValue System.Single

local m = { }
---public CLMyGrid .ctor()
---@return CLMyGrid
function m.New() end
---public Void refreshPoint()
function m:refreshPoint() end
Coolape.CLMyGrid = m
return m
