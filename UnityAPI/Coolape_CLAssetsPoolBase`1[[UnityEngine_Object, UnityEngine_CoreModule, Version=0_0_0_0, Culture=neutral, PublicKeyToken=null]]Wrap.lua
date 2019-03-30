---@class Coolape.CLAssetsPoolBase`1[[UnityEngine.Object, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@field public OnSetPrefabCallbacks4Borrow Coolape.CLDelegate
---@field public OnSetPrefabCallbacks Coolape.CLDelegate
---@field public isFinishInitPool System.Boolean
---@field public poolMap System.Collections.Hashtable
---@field public prefabMap System.Collections.Hashtable
---@field public isSettingPrefabMap System.Collections.Hashtable

local m = { }
---public Void _clean()
function m:_clean() end
---public Void initPool()
function m:initPool() end
---public String wrapPath(String basePath, String thingName)
---@return String
---@param optional String basePath
---@param optional String thingName
function m.wrapPath(basePath, thingName) end
---public Boolean _havePrefab(String name)
---@return bool
---@param optional String name
function m:_havePrefab(name) end
---public String getAssetPath(String name)
---@return String
---@param optional String name
function m:getAssetPath(name) end
---public Void _setPrefab(String name, Object finishCallback, Object orgs, Object progressCB)
---@param optional String name
---@param optional Object finishCallback
---@param optional Object orgs
---@param optional Object progressCB
function m:_setPrefab(name, finishCallback, orgs, progressCB) end
---public Void onFinishSetPrefab(Object[] paras)
---@param optional Object[] paras
function m:onFinishSetPrefab(paras) end
---public Void doSetPrefab(String path, String name, Object finishCallback, Object args, Object progressCB)
---@param optional String path
---@param optional String name
---@param optional Object finishCallback
---@param optional Object args
---@param optional Object progressCB
function m:doSetPrefab(path, name, finishCallback, args, progressCB) end
---public Void finishSetPrefab(Object unit)
---@param optional Object unit
function m:finishSetPrefab(unit) end
---public Void onGetAssetsBundle(Object[] paras)
---@param optional Object[] paras
function m:onGetAssetsBundle(paras) end
---public Void sepcProc4Assets(Object unit, Object cb, Object args, Object progressCB)
---@param optional Object unit
---@param optional Object cb
---@param optional Object args
---@param optional Object progressCB
function m:sepcProc4Assets(unit, cb, args, progressCB) end
---public Void onGetSharedAssets(Object[] param)
---@param optional Object[] param
function m:onGetSharedAssets(param) end
---public Void realseAsset(Object[] paras)
---@param optional Object[] paras
function m:realseAsset(paras) end
---public ObjsPubPool getObjPool(String name)
---@return ObjsPubPool
---@param optional String name
function m:getObjPool(name) end
---public Object _borrowObj(String name)
---public Object _borrowObj(String name, Boolean isSharedResource)
---@return Object
---@param String name
---@param optional Boolean isSharedResource
function m:_borrowObj(name, isSharedResource) end
---public Void _borrowObjAsyn(String name, Object onGetCallbak)
---public Void _borrowObjAsyn(String name, Object onGetCallbak, Object orgs)
---public Void _borrowObjAsyn(String name, Object onGetCallbak, Object orgs, Object progressCB)
---@param String name
---@param Object onGetCallbak
---@param optional Object orgs
---@param optional Object progressCB
function m:_borrowObjAsyn(name, onGetCallbak, orgs, progressCB) end
---public Void onFinishSetPrefab4Borrow(Object[] paras)
---@param optional Object[] paras
function m:onFinishSetPrefab4Borrow(paras) end
---public Void _returnObj(String name, Object unit, Boolean inActive, Boolean setParent)
---@param optional String name
---@param optional Object unit
---@param optional Boolean inActive
---@param optional Boolean setParent
function m:_returnObj(name, unit, inActive, setParent) end
Coolape.CLAssetsPoolBase`1[[UnityEngine.Object, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]] = m
return m
