---@class UIAnchor : UnityEngine.MonoBehaviour
---@field public uiCamera UnityEngine.Camera
---@field public container UnityEngine.GameObject
---@field public side UIAnchor.Side
---@field public runOnlyOnce System.Boolean
---@field public relativeOffset UnityEngine.Vector2
---@field public pixelOffset UnityEngine.Vector2
---@field public screenRect UnityEngine.Rect
local m = { }
---public UIAnchor .ctor()
---@return UIAnchor
function m.New() end
---public Vector3 top()
---@return Vector3
function m.top() end
---public Vector3 bottom()
---@return Vector3
function m.bottom() end
---public Vector3 left()
---@return Vector3
function m.left() end
---public Vector3 right()
---@return Vector3
function m.right() end
---public Vector3 topLeft()
---@return Vector3
function m.topLeft() end
---public Vector3 topRight()
---@return Vector3
function m.topRight() end
---public Vector3 bottomLeft()
---@return Vector3
function m.bottomLeft() end
---public Vector3 bottomRight()
---@return Vector3
function m.bottomRight() end
UIAnchor = m
return m
