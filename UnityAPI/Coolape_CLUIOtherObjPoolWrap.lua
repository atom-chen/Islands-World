---@class Coolape.CLUIOtherObjPool : Coolape.CLAssetsPoolBase`1[[UnityEngine.GameObject, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@field public pool Coolape.CLUIOtherObjPool
---@field public isAutoReleaseAssetBundle System.Boolean

local m = { }
---public CLUIOtherObjPool .ctor()
---@return CLUIOtherObjPool
function m.New() end
---public String getAssetPath(String name)
---@return String
---@param optional String name
function m:getAssetPath(name) end
---public Void _returnObj(String name, GameObject unit, Boolean inActive, Boolean setParent)
---@param optional String name
---@param optional GameObject unit
---@param optional Boolean inActive
---@param optional Boolean setParent
function m:_returnObj(name, unit, inActive, setParent) end
---public Void clean()
function m.clean() end
---public Boolean havePrefab(String name)
---@return bool
---@param optional String name
function m.havePrefab(name) end
---public Void setPrefab(String name, Object finishCallback, Object args)
---public Void setPrefab(String name, Object finishCallback, Object args, Object progressCB)
---@param String name
---@param optional Object finishCallback
---@param optional Object args
---@param optional Object progressCB
function m.setPrefab(name, finishCallback, args, progressCB) end
---public GameObject _borrowObj(String name)
---@return GameObject
---@param optional String name
function m:_borrowObj(name) end
---public GameObject borrowObj(String name)
---@return GameObject
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
---public Void returnObj(GameObject go)
---public Void returnObj(String name, GameObject go)
---public Void returnObj(String name, GameObject go, Boolean inActive, Boolean setParent)
---@param String name
---@param GameObject go
---@param Boolean inActive
---@param optional Boolean setParent
function m.returnObj(name, go, inActive, setParent) end
Coolape.CLUIOtherObjPool = m
return m
