---@class Coolape.CLRoleAction : UnityEngine.MonoBehaviour
---@field public currAction Coolape.CLRoleAction.Action
---@field public currActionValue System.Int32
---@field public animator UnityEngine.Animator
local m = { }
---public CLRoleAction .ctor()
---@return CLRoleAction
function m.New() end
---public Action getActByName(String name)
---@return number
---@param optional String name
function m.getActByName(name) end
---public Void pause()
function m:pause() end
---public Void regain()
function m:regain() end
---public Void setSpeedAdd(Single addSpeed)
---@param optional Single addSpeed
function m:setSpeedAdd(addSpeed) end
---public Void setAction(Action action, Object onCompleteMotion)
---public Void setAction(Int32 actionValue, Object onCompleteMotion)
---@param optional Int32 actionValue
---@param optional Object onCompleteMotion
function m:setAction(actionValue, onCompleteMotion) end
---public Void doSetActionWithCallback(Int32 actionValue, ArrayList progressCallbackInfor)
---@param optional Int32 actionValue
---@param optional ArrayList progressCallbackInfor
function m:doSetActionWithCallback(actionValue, progressCallbackInfor) end
---public IEnumerator exeCallback(Object cbFunc)
---@return IEnumerator
---@param optional Object cbFunc
function m:exeCallback(cbFunc) end
Coolape.CLRoleAction = m
return m
