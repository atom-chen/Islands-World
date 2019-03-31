---@class UnityEngine.Rendering.PostProcessing.PostProcessLayer : UnityEngine.MonoBehaviour
---@field public volumeTrigger UnityEngine.Transform
---@field public volumeLayer UnityEngine.LayerMask
---@field public stopNaNPropagation System.Boolean
---@field public finalBlitToCameraTarget System.Boolean
---@field public antialiasingMode UnityEngine.Rendering.PostProcessing.PostProcessLayer.Antialiasing
---@field public temporalAntialiasing UnityEngine.Rendering.PostProcessing.TemporalAntialiasing
---@field public subpixelMorphologicalAntialiasing UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing
---@field public fastApproximateAntialiasing UnityEngine.Rendering.PostProcessing.FastApproximateAntialiasing
---@field public fog UnityEngine.Rendering.PostProcessing.Fog
---@field public debugLayer UnityEngine.Rendering.PostProcessing.PostProcessDebugLayer
---@field public breakBeforeColorGrading System.Boolean
---@field public sortedBundles System.Collections.Generic.Dictionary2UnityEngine.Rendering.PostProcessing.PostProcessEventSystem.Collections.Generic.List1UnityEngine.Rendering.PostProcessing.PostProcessLayer.SerializedBundleRef
---@field public haveBundlesBeenInited System.Boolean

local m = { }
---public PostProcessLayer .ctor()
---@return PostProcessLayer
function m.New() end
---public Void Init(PostProcessResources resources)
---@param optional PostProcessResources resources
function m:Init(resources) end
---public Void InitBundles()
function m:InitBundles() end
---public PostProcessBundle GetBundle(Type settingsType)
---@return PostProcessBundle
---@param optional Type settingsType
function m:GetBundle(settingsType) end
---public Void BakeMSVOMap(CommandBuffer cmd, Camera camera, RenderTargetIdentifier destination, Nullable`1 depthMap, Boolean invert, Boolean isMSAA)
---@param optional CommandBuffer cmd
---@param optional Camera camera
---@param optional RenderTargetIdentifier destination
---@param optional Nullable`1 depthMap
---@param optional Boolean invert
---@param optional Boolean isMSAA
function m:BakeMSVOMap(cmd, camera, destination, depthMap, invert, isMSAA) end
---public Void ResetHistory()
function m:ResetHistory() end
---public Boolean HasOpaqueOnlyEffects(PostProcessRenderContext context)
---@return bool
---@param optional PostProcessRenderContext context
function m:HasOpaqueOnlyEffects(context) end
---public Boolean HasActiveEffects(PostProcessEvent evt, PostProcessRenderContext context)
---@return bool
---@param optional PostProcessEvent evt
---@param optional PostProcessRenderContext context
function m:HasActiveEffects(evt, context) end
---public Void UpdateVolumeSystem(Camera cam, CommandBuffer cmd)
---@param optional Camera cam
---@param optional CommandBuffer cmd
function m:UpdateVolumeSystem(cam, cmd) end
---public Void RenderOpaqueOnly(PostProcessRenderContext context)
---@param optional PostProcessRenderContext context
function m:RenderOpaqueOnly(context) end
---public Void Render(PostProcessRenderContext context)
---@param optional PostProcessRenderContext context
function m:Render(context) end
UnityEngine.Rendering.PostProcessing.PostProcessLayer = m
return m
