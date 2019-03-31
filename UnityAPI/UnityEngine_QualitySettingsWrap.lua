---@class UnityEngine.QualitySettings : UnityEngine.Object
---@field public pixelLightCount System.Int32
---@field public shadows UnityEngine.ShadowQuality
---@field public shadowProjection UnityEngine.ShadowProjection
---@field public shadowCascades System.Int32
---@field public shadowDistance System.Single
---@field public shadowResolution UnityEngine.ShadowResolution
---@field public shadowmaskMode UnityEngine.ShadowmaskMode
---@field public shadowNearPlaneOffset System.Single
---@field public shadowCascade2Split System.Single
---@field public shadowCascade4Split UnityEngine.Vector3
---@field public lodBias System.Single
---@field public anisotropicFiltering UnityEngine.AnisotropicFiltering
---@field public masterTextureLimit System.Int32
---@field public maximumLODLevel System.Int32
---@field public particleRaycastBudget System.Int32
---@field public softParticles System.Boolean
---@field public softVegetation System.Boolean
---@field public vSyncCount System.Int32
---@field public antiAliasing System.Int32
---@field public asyncUploadTimeSlice System.Int32
---@field public asyncUploadBufferSize System.Int32
---@field public asyncUploadPersistentBuffer System.Boolean
---@field public realtimeReflectionProbes System.Boolean
---@field public billboardsFaceCameraPosition System.Boolean
---@field public resolutionScalingFixedDPIFactor System.Single
---@field public blendWeights UnityEngine.BlendWeights
---@field public streamingMipmapsActive System.Boolean
---@field public streamingMipmapsMemoryBudget System.Single
---@field public streamingMipmapsRenderersPerFrame System.Int32
---@field public streamingMipmapsMaxLevelReduction System.Int32
---@field public streamingMipmapsAddAllCameras System.Boolean
---@field public streamingMipmapsMaxFileIORequests System.Int32
---@field public maxQueuedFrames System.Int32
---@field public names System.String
---@field public desiredColorSpace UnityEngine.ColorSpace
---@field public activeColorSpace UnityEngine.ColorSpace

local m = { }
---public Void IncreaseLevel()
---public Void IncreaseLevel(Boolean applyExpensiveChanges)
---@param Boolean applyExpensiveChanges
function m.IncreaseLevel(applyExpensiveChanges) end
---public Void DecreaseLevel()
---public Void DecreaseLevel(Boolean applyExpensiveChanges)
---@param Boolean applyExpensiveChanges
function m.DecreaseLevel(applyExpensiveChanges) end
---public Void SetQualityLevel(Int32 index)
---public Void SetQualityLevel(Int32 index, Boolean applyExpensiveChanges)
---@param Int32 index
---@param optional Boolean applyExpensiveChanges
function m.SetQualityLevel(index, applyExpensiveChanges) end
---public Int32 GetQualityLevel()
---@return number
function m.GetQualityLevel() end
UnityEngine.QualitySettings = m
return m
