---@class Coolape.CLTexturePool : Coolape.CLAssetsPoolBase`1[[UnityEngine.Texture, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@field public pool Coolape.CLTexturePool
---@field public isAutoReleaseAssetBundle System.Boolean
local m = { }
---public CLTexturePool .ctor()
---@return CLTexturePool
function m.New() end
---public String getAssetPath(String name)
---@return String
---@param optional String name
function m:getAssetPath(name) end
---public Texture _borrowObj(String name)
---@return Texture
---@param optional String name
function m:_borrowObj(name) end
---public Boolean havePrefab(String path)
---@return bool
---@param optional String path
function m.havePrefab(path) end
---public Void clean()
function m.clean() end
---public Void setPrefab(String path, Object finishCallback)
---public Void setPrefab(String path, Object finishCallback, Object args)
---public Void setPrefab(String path, Object finishCallback, Object args, Object progressCB)
---@param String path
---@param Object finishCallback
---@param optional Object args
---@param optional Object progressCB
function m.setPrefab(path, finishCallback, args, progressCB) end
---public Texture borrowObj(String path)
---@return Texture
---@param optional String path
function m.borrowObj(path) end
---public Void borrowObjAsyn(String path, Object onGetCallbak)
---public Void borrowObjAsyn(String path, Object onGetCallbak, Object orgs)
---public Void borrowObjAsyn(String path, Object onGetCallbak, Object orgs, Object progressCB)
---@param String path
---@param Object onGetCallbak
---@param optional Object orgs
---@param optional Object progressCB
function m.borrowObjAsyn(path, onGetCallbak, orgs, progressCB) end
---public Void returnObj(String path)
---@param optional String path
function m.returnObj(path) end
Coolape.CLTexturePool = m
return m
