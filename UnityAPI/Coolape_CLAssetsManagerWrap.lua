---@class Coolape.CLAssetsManager : UnityEngine.MonoBehaviour
---@field public self Coolape.CLAssetsManager
---@field public isPuase System.Boolean
---@field public debugKey System.String
---@field public timeOutSec4Realse System.Int32
---@field public assetsMap System.Collections.Hashtable
---@field public realseTime System.Int32

local m = { }
---public CLAssetsManager .ctor()
---@return CLAssetsManager
function m.New() end
---public Void pause()
function m:pause() end
---public Void regain()
function m:regain() end
---public Void addAsset(String key, String name, AssetBundle asset, Callback onRealse)
---@param optional String key
---@param optional String name
---@param optional AssetBundle asset
---@param optional Callback onRealse
function m:addAsset(key, name, asset, onRealse) end
---public Void useAsset(String key)
---@param optional String key
function m:useAsset(key) end
---public Void unUseAsset(String key)
---@param optional String key
function m:unUseAsset(key) end
---public Object getAsset(String key)
---@return Object
---@param optional String key
function m:getAsset(key) end
---public Void releaseAsset()
---public Void releaseAsset(Boolean isForceRelease)
---@param Boolean isForceRelease
function m:releaseAsset(isForceRelease) end
Coolape.CLAssetsManager = m
return m
