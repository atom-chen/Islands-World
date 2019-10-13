---@class Coolape.CLSmoothFollow : UnityEngine.MonoBehaviour
---@field public target UnityEngine.Transform
---@field public distance System.Single
---@field public height System.Single
---@field public heightDamping System.Single
---@field public rotationDamping System.Single
---@field public offset UnityEngine.Vector3
---@field public isCanRotate System.Boolean
---@field public isRole System.Boolean
local m = { }
---public CLSmoothFollow .ctor()
---@return CLSmoothFollow
function m.New() end
---public Void LateUpdate()
function m:LateUpdate() end
---public Void tween(Vector2 from, Vector2 to, Single speed, Object callback, Object progressCallback)
---@param optional Vector2 from
---@param optional Vector2 to
---@param optional Single speed
---@param optional Object callback
---@param optional Object progressCallback
function m:tween(from, to, speed, callback, progressCallback) end
Coolape.CLSmoothFollow = m
return m
