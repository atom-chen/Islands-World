---@class Coolape.CLBulletPool : Coolape.CLAssetsPoolBase`1[[Coolape.CLBulletBase, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@field public pool Coolape.CLBulletPool

local m = { }
---public CLBulletPool .ctor()
---@return CLBulletPool
function m.New() end
---public String getAssetPath(String name)
---@return String
---@param optional String name
function m:getAssetPath(name) end
---public Void clean()
function m.clean() end
---public Boolean havePrefab(String name)
---@return bool
---@param optional String name
function m.havePrefab(name) end
---public Void setPrefab(String name, Object finishCallback)
---public Void setPrefab(String name, Object finishCallback, Object orgs)
---public Void setPrefab(String name, Object finishCallback, Object orgs, Object progressCB)
---@param String name
---@param Object finishCallback
---@param optional Object orgs
---@param optional Object progressCB
function m.setPrefab(name, finishCallback, orgs, progressCB) end
---public CLBulletBase borrowObj(String name)
---@return CLBulletBase
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
---public Void returnObj(CLBulletBase unit)
---public Void returnObj(CLBulletBase unit, Boolean inActive, Boolean setParent)
---@param CLBulletBase unit
---@param Boolean inActive
---@param optional Boolean setParent
function m.returnObj(unit, inActive, setParent) end
Coolape.CLBulletPool = m
return m
