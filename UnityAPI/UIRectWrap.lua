---@class UIRect : UnityEngine.MonoBehaviour
---@field public leftAnchor UIRect.AnchorPoint
---@field public rightAnchor UIRect.AnchorPoint
---@field public bottomAnchor UIRect.AnchorPoint
---@field public topAnchor UIRect.AnchorPoint
---@field public updateAnchors UIRect.AnchorUpdate
---@field public finalAlpha System.Single
---@field public cachedGameObject UnityEngine.GameObject
---@field public cachedTransform UnityEngine.Transform
---@field public anchorCamera UnityEngine.Camera
---@field public isFullyAnchored System.Boolean
---@field public isAnchoredHorizontally System.Boolean
---@field public isAnchoredVertically System.Boolean
---@field public canBeAnchored System.Boolean
---@field public parent UIRect
---@field public root UIRoot
---@field public isAnchored System.Boolean
---@field public alpha System.Single
---@field public localCorners UnityEngine.Vector3
---@field public worldCorners UnityEngine.Vector3
local m = { }
---public Single CalculateFinalAlpha(Int32 frameID)
---@return number
---@param optional Int32 frameID
function m:CalculateFinalAlpha(frameID) end
---public Void Invalidate(Boolean includeChildren)
---@param optional Boolean includeChildren
function m:Invalidate(includeChildren) end
---public Vector3[] GetSides(Transform relativeTo)
---@return table
---@param optional Transform relativeTo
function m:GetSides(relativeTo) end
---public Void Update()
function m:Update() end
---public Void UpdateAnchors()
function m:UpdateAnchors() end
---public Void SetAnchor(Transform t)
---public Void SetAnchor(GameObject go)
---public Void SetAnchor(GameObject go, Int32 left, Int32 bottom, Int32 right, Int32 top)
---@param GameObject go
---@param Int32 left
---@param Int32 bottom
---@param Int32 right
---@param optional Int32 top
function m:SetAnchor(go, left, bottom, right, top) end
---public Void ResetAnchors()
function m:ResetAnchors() end
---public Void ResetAndUpdateAnchors()
function m:ResetAndUpdateAnchors() end
---public Void SetRect(Single x, Single y, Single width, Single height)
---@param optional Single x
---@param optional Single y
---@param optional Single width
---@param optional Single height
function m:SetRect(x, y, width, height) end
---public Void ParentHasChanged()
function m:ParentHasChanged() end
UIRect = m
return m
