---@class Coolape.CLSharedAssets : UnityEngine.MonoBehaviour
---@field public OnFinishSetCallbacks Coolape.CLDelegate
---@field public materials System.Collections.Generic.List1Coolape.CLSharedAssets.CLMaterialInfor
---@field public meshs System.Collections.Generic.List1Coolape.CLSharedAssets.CLMeshInfor
---@field public isDonnotResetAssets System.Boolean
---@field public isSkipModel System.Boolean
---@field public progressCallback System.Object
---@field public onFinshLoad System.Collections.Generic.List1EventDelegate
---@field public progress System.Single

local m = { }
---public CLSharedAssets .ctor()
---@return CLSharedAssets
function m.New() end
---public Void reset()
function m:reset() end
---public Boolean isEmpty()
---@return bool
function m:isEmpty() end
---public Void cleanRefOnly()
function m:cleanRefOnly() end
---public Void cleanRefAssets()
function m:cleanRefAssets() end
---public Void init(Object finishCallback, Object orgs)
---public Void init(Object finishCallback, Object orgs, Object progressCallback)
---@param Object finishCallback
---@param optional Object orgs
---@param optional Object progressCallback
function m:init(finishCallback, orgs, progressCallback) end
---public Void resetAssets()
function m:resetAssets() end
---public Void setMaterial()
function m:setMaterial() end
---public Void setMesh()
function m:setMesh() end
---public Void OnDestroy()
function m:OnDestroy() end
---public Void returnAssets()
function m:returnAssets() end
Coolape.CLSharedAssets = m
return m
