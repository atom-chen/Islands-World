---@class UIRect+AnchorPoint
---@field public target UnityEngine.Transform
---@field public relative System.Single
---@field public absolute System.Int32
---@field public rect UIRect
---@field public targetCam UnityEngine.Camera
local m = { }
---public AnchorPoint .ctor()
---public AnchorPoint .ctor(Single relative)
---@return AnchorPoint
---@param Single relative
function m.New(relative) end
---public Void Set(Single relative, Single absolute)
---public Void Set(Transform target, Single relative, Single absolute)
---@param Transform target
---@param optional Single relative
---@param optional Single absolute
function m:Set(target, relative, absolute) end
---public Void SetToNearest(Single abs0, Single abs1, Single abs2)
---public Void SetToNearest(Single rel0, Single rel1, Single rel2, Single abs0, Single abs1, Single abs2)
---@param Single rel0
---@param Single rel1
---@param Single rel2
---@param optional Single abs0
---@param optional Single abs1
---@param optional Single abs2
function m:SetToNearest(rel0, rel1, rel2, abs0, abs1, abs2) end
---public Void SetHorizontal(Transform parent, Single localPos)
---@param optional Transform parent
---@param optional Single localPos
function m:SetHorizontal(parent, localPos) end
---public Void SetVertical(Transform parent, Single localPos)
---@param optional Transform parent
---@param optional Single localPos
function m:SetVertical(parent, localPos) end
---public Vector3[] GetSides(Transform relativeTo)
---@return table
---@param optional Transform relativeTo
function m:GetSides(relativeTo) end
UIRect+AnchorPoint = m
return m
