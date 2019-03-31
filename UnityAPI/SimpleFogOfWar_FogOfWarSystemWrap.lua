---@class SimpleFogOfWar.FogOfWarSystem : UnityEngine.MonoBehaviour
---@field public size System.Single
---@field public mode SimpleFogOfWar.FogOfWarSystem.FogMode
---@field public Resolution SimpleFogOfWar.FogOfWarSystem.FogTexResolution
---@field public edgeSoftness System.Single
---@field public color UnityEngine.Color
---@field public fogRenderer SimpleFogOfWar.Renderers.FOWRenderer
---@field public snapShotInterval System.Single
---@field public lastSnapShot System.Single
---@field public layer System.Int32
---@field public renderQueue System.Int32
---@field public camera UnityEngine.Camera
---@field public VisibilitySnapshotInterval System.Single
---@field public Size System.Single

local m = { }
---public FogOfWarSystem .ctor()
---@return FogOfWarSystem
function m.New() end
---public Void RegisterInfluence(FogOfWarInfluence influence)
---@param optional FogOfWarInfluence influence
function m.RegisterInfluence(influence) end
---public Void UnregisterInfluence(FogOfWarInfluence influence)
---@param optional FogOfWarInfluence influence
function m.UnregisterInfluence(influence) end
---public Void SetFogRenderer(FOWRenderer fRenderer)
---@param optional FOWRenderer fRenderer
function m:SetFogRenderer(fRenderer) end
---public Void SetBlur(Single value)
---@param optional Single value
function m:SetBlur(value) end
---public Void SetColor(Color col)
---@param optional Color col
function m:SetColor(col) end
---public FogVisibility GetVisibility(Vector3 position)
---@return number
---@param optional Vector3 position
function m:GetVisibility(position) end
---public Byte[] GetPersistentData()
---@return table
function m:GetPersistentData() end
---public Void LoadPersistentData(Byte[] data)
---@param optional Byte[] data
function m:LoadPersistentData(data) end
---public Void ClearPersistenFog()
function m:ClearPersistenFog() end
---public Void setRenderQueue(Int32 val)
---@param optional Int32 val
function m:setRenderQueue(val) end
---public Void LateUpdate()
function m:LateUpdate() end
---public Void ClearStampTexture()
function m:ClearStampTexture() end
SimpleFogOfWar.FogOfWarSystem = m
return m
