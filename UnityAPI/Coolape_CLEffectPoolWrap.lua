---@class Coolape.CLEffectPool : Coolape.CLAssetsPoolBase`1[[Coolape.CLEffect, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@field public pool Coolape.CLEffectPool
---@field public isAutoReleaseAssetBundle System.Boolean
local m = { }
---public CLEffectPool .ctor()
---@return CLEffectPool
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
---public Void setPrefab(String name, Object finishCallback, Object args)
---public Void setPrefab(String name, Object finishCallback, Object args, Object progressCB)
---@param String name
---@param Object finishCallback
---@param optional Object args
---@param optional Object progressCB
function m.setPrefab(name, finishCallback, args, progressCB) end
---public CLEffect borrowObj(String name)
---@return CLEffect
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
---public Void returnObj(CLEffect effect)
---public Void returnObj(String name, CLEffect effect)
---public Void returnObj(String name, CLEffect effect, Boolean inActive, Boolean setParent)
---@param String name
---@param CLEffect effect
---@param Boolean inActive
---@param optional Boolean setParent
function m.returnObj(name, effect, inActive, setParent) end
Coolape.CLEffectPool = m
return m
