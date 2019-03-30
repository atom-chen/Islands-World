---@class Coolape.CLRolePool : Coolape.CLAssetsPoolBase`1[[Coolape.CLUnit, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@field public pool Coolape.CLRolePool

local m = { }
---public CLRolePool .ctor()
---@return CLRolePool
function m.New() end
---public String getAssetPath(String name)
---@return String
---@param optional String name
function m:getAssetPath(name) end
---public Void _returnObj(String name, CLUnit unit, Boolean inActive, Boolean setParent)
---@param optional String name
---@param optional CLUnit unit
---@param optional Boolean inActive
---@param optional Boolean setParent
function m:_returnObj(name, unit, inActive, setParent) end
---public Void clean()
function m.clean() end
---public Boolean havePrefab(String name)
---@return bool
---@param optional String name
function m.havePrefab(name) end
---public Boolean isNeedDownload(String roleName)
---@return bool
---@param optional String roleName
function m.isNeedDownload(roleName) end
---public Void setPrefab(String name, Object finishCallback)
---public Void setPrefab(String name, Object finishCallback, Object args)
---public Void setPrefab(String name, Object finishCallback, Object args, Object progressCB)
---@param String name
---@param Object finishCallback
---@param optional Object args
---@param optional Object progressCB
function m.setPrefab(name, finishCallback, args, progressCB) end
---public CLUnit borrowObj(String name)
---@return CLUnit
---@param optional String name
function m.borrowObj(name) end
---public Void borrowObjAsyn(String name, Object onGetCallbak)
---public Void borrowObjAsyn(String name, Object onGetCallbak, Object orgs)
---public Void borrowObjAsyn(String name, Object onGetCallbak, Object orgs, Object progressCB)
---@param String name
---@param Object onGetCallbak
---@param optional Object orgs
---@param optional Object progressCB
function m.borrowObjAsyn(name, onGetCallbak, orgs, progressCB) end
---public Void returnObj(CLUnit unit)
---public Void returnObj(String name, CLUnit unit)
---public Void returnObj(String name, CLUnit unit, Boolean inActive, Boolean setParent)
---@param String name
---@param CLUnit unit
---@param Boolean inActive
---@param optional Boolean setParent
function m.returnObj(name, unit, inActive, setParent) end
Coolape.CLRolePool = m
return m
