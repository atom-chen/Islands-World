---@class UIFollowTarget : UnityEngine.MonoBehaviour
---@field public target UnityEngine.Transform
---@field public mGameCamera UnityEngine.Camera
---@field public mUICamera UnityEngine.Camera
---@field public offsetPos UnityEngine.Vector3
---@field public disableIfInvisible System.Boolean

local m = { }
---public UIFollowTarget .ctor()
---@return UIFollowTarget
function m.New() end
---public Void Start()
function m:Start() end
---public Void setCamera(Camera mCamera, Camera uiCamera)
---@param optional Camera mCamera
---@param optional Camera uiCamera
function m:setCamera(mCamera, uiCamera) end
---public Void LateUpdate()
function m:LateUpdate() end
---public Vector3 getViewPos()
---@return Vector3
function m:getViewPos() end
---public Void setTarget(Transform target, Vector3 offset)
---@param optional Transform target
---@param optional Vector3 offset
function m:setTarget(target, offset) end
UIFollowTarget = m
return m
