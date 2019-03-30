---@class Coolape.CLMaterialPool : Coolape.CLAssetsPoolBase`1[[UnityEngine.Material, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@field public pool Coolape.CLMaterialPool
---@field public materialTexRefCfgPath System.String
---@field public materialTexRefCfg System.Collections.Hashtable

local m = { }
---public CLMaterialPool .ctor()
---@return CLMaterialPool
function m.New() end
---public String getAssetPath(String name)
---@return String
---@param optional String name
function m:getAssetPath(name) end
---public Material _borrowObj(String name)
---@return Material
---@param optional String name
function m:_borrowObj(name) end
---public Boolean havePrefab(String name)
---@return bool
---@param optional String name
function m.havePrefab(name) end
---public Void clean()
function m.clean() end
---public Void setPrefab(String name, Object finishCallback)
---public Void setPrefab(String name, Object finishCallback, Object args)
---public Void setPrefab(String name, Object finishCallback, Object args, Object progressCB)
---@param String name
---@param Object finishCallback
---@param optional Object args
---@param optional Object progressCB
function m.setPrefab(name, finishCallback, args, progressCB) end
---public Material borrowObj(String name)
---@return Material
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
---public Void cleanTexRef(String name, Material mat)
---@param optional String name
---@param optional Material mat
function m.cleanTexRef(name, mat) end
---public Void sepcProc4Assets(Material mat, Object cb, Object args, Object progressCB)
---@param optional Material mat
---@param optional Object cb
---@param optional Object args
---@param optional Object progressCB
function m:sepcProc4Assets(mat, cb, args, progressCB) end
---public Void resetTexRef(String matName, Material mat, Object cb, Object args)
---@param optional String matName
---@param optional Material mat
---@param optional Object cb
---@param optional Object args
function m.resetTexRef(matName, mat, cb, args) end
---public Void onGetTexture(Object[] paras)
---@param optional Object[] paras
function m.onGetTexture(paras) end
---public Boolean getMaterialTexCfg(String matName, ArrayList& propNames, ArrayList& texNames, ArrayList& texPaths)
---@return bool
---@param optional String matName
---@param optional ArrayList& propNames
---@param optional ArrayList& texNames
---@param optional ArrayList& texPaths
function m.getMaterialTexCfg(matName, propNames, texNames, texPaths) end
---public Hashtable readMaterialTexRefCfg()
---@return Hashtable
function m.readMaterialTexRefCfg() end
Coolape.CLMaterialPool = m
return m
