---@class Coolape.CLSharedAssets+CLMaterialInfor
---@field public render UnityEngine.Renderer
---@field public index System.Int32
---@field public materialName System.String
local m = { }
---public CLMaterialInfor .ctor()
---@return CLMaterialInfor
function m.New() end
---public Void returnMaterial()
function m:returnMaterial() end
---public Void setMaterial(Callback onFinishCallback, Object orgs)
---@param optional Callback onFinishCallback
---@param optional Object orgs
function m:setMaterial(onFinishCallback, orgs) end
Coolape.CLSharedAssets+CLMaterialInfor = m
return m
