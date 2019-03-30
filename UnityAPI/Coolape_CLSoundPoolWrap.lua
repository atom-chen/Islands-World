---@class Coolape.CLSoundPool : Coolape.CLAssetsPoolBase`1[[UnityEngine.AudioClip, UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@field public pool Coolape.CLSoundPool

local m = { }
---public CLSoundPool .ctor()
---@return CLSoundPool
function m.New() end
---public String getAssetPath(String name)
---@return String
---@param optional String name
function m:getAssetPath(name) end
---public AudioClip _borrowObj(String name)
---@return AudioClip
---@param optional String name
function m:_borrowObj(name) end
---public Boolean havePrefab(String name)
---@return bool
---@param optional String name
function m.havePrefab(name) end
---public Void clean()
function m.clean() end
---public Void setPrefab(String name, Object finishCallback)
---public Void setPrefab(String name, Object finishCallback, Object orgs)
---public Void setPrefab(String name, Object finishCallback, Object orgs, Object progressCB)
---@param String name
---@param Object finishCallback
---@param optional Object orgs
---@param optional Object progressCB
function m.setPrefab(name, finishCallback, orgs, progressCB) end
---public AudioClip borrowObj(String name)
---@return AudioClip
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
---public Void returnObj(String name)
---@param optional String name
function m.returnObj(name) end
Coolape.CLSoundPool = m
return m
