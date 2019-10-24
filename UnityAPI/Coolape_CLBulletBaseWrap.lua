---@class Coolape.CLBulletBase : UnityEngine.MonoBehaviour
---@field public curveSpeed UnityEngine.AnimationCurve
---@field public curveHigh UnityEngine.AnimationCurve
---@field public attr System.Object
---@field public data System.Object
---@field public isFireNow System.Boolean
---@field public isFollow System.Boolean
---@field public isMulHit System.Boolean
---@field public isStoped System.Boolean
---@field public needRotate System.Boolean
---@field public slowdownDistance System.Single
---@field public arriveDistance System.Single
---@field public turningSpeed System.Single
---@field public RefreshTargetMSec System.Int32
---@field public speed System.Single
---@field public high System.Single
---@field public angleOffset System.Int32
---@field public attacker Coolape.CLUnit
---@field public target Coolape.CLUnit
---@field public hitTarget Coolape.CLUnit
---@field public haveCollider System.Boolean
---@field public boxCollider UnityEngine.BoxCollider
---@field public transform UnityEngine.Transform
local m = { }
---public CLBulletBase .ctor()
---@return CLBulletBase
function m.New() end
---public Void doFire(CLUnit attacker, CLUnit target, Vector3 orgPos, Vector3 dir, Object attr, Object data, Object callbak)
---@param optional CLUnit attacker
---@param optional CLUnit target
---@param optional Vector3 orgPos
---@param optional Vector3 dir
---@param optional Object attr
---@param optional Object data
---@param optional Object callbak
function m:doFire(attacker, target, orgPos, dir, attr, data, callbak) end
---public Void RotateBullet()
function m:RotateBullet() end
---public Void FixedUpdate()
function m:FixedUpdate() end
---public Void resetTarget()
function m:resetTarget() end
---public Void timeOut()
function m:timeOut() end
---public Void stop()
function m:stop() end
---public Void onFinishFire(Boolean needRelease)
---@param optional Boolean needRelease
function m:onFinishFire(needRelease) end
---public CLBulletBase fire(CLUnit attacker, CLUnit target, Vector3 orgPos, Vector3 dir, Object attr, Object data, Object callbak)
---@return CLBulletBase
---@param optional CLUnit attacker
---@param optional CLUnit target
---@param optional Vector3 orgPos
---@param optional Vector3 dir
---@param optional Object attr
---@param optional Object data
---@param optional Object callbak
function m.fire(attacker, target, orgPos, dir, attr, data, callbak) end
Coolape.CLBulletBase = m
return m
