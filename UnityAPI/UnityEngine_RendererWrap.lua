---@class UnityEngine.Renderer : UnityEngine.Component
---@field public bounds UnityEngine.Bounds
---@field public enabled System.Boolean
---@field public isVisible System.Boolean
---@field public shadowCastingMode UnityEngine.Rendering.ShadowCastingMode
---@field public receiveShadows System.Boolean
---@field public motionVectorGenerationMode UnityEngine.MotionVectorGenerationMode
---@field public lightProbeUsage UnityEngine.Rendering.LightProbeUsage
---@field public reflectionProbeUsage UnityEngine.Rendering.ReflectionProbeUsage
---@field public renderingLayerMask System.UInt32
---@field public sortingLayerName System.String
---@field public sortingLayerID System.Int32
---@field public sortingOrder System.Int32
---@field public allowOcclusionWhenDynamic System.Boolean
---@field public isPartOfStaticBatch System.Boolean
---@field public worldToLocalMatrix UnityEngine.Matrix4x4
---@field public localToWorldMatrix UnityEngine.Matrix4x4
---@field public lightProbeProxyVolumeOverride UnityEngine.GameObject
---@field public probeAnchor UnityEngine.Transform
---@field public lightmapIndex System.Int32
---@field public realtimeLightmapIndex System.Int32
---@field public lightmapScaleOffset UnityEngine.Vector4
---@field public realtimeLightmapScaleOffset UnityEngine.Vector4
---@field public materials UnityEngine.Material
---@field public material UnityEngine.Material
---@field public sharedMaterial UnityEngine.Material
---@field public sharedMaterials UnityEngine.Material

local m = { }
---public Renderer .ctor()
---@return Renderer
function m.New() end
---public Boolean HasPropertyBlock()
---@return bool
function m:HasPropertyBlock() end
---public Void SetPropertyBlock(MaterialPropertyBlock properties)
---public Void SetPropertyBlock(MaterialPropertyBlock properties, Int32 materialIndex)
---@param MaterialPropertyBlock properties
---@param optional Int32 materialIndex
function m:SetPropertyBlock(properties, materialIndex) end
---public Void GetPropertyBlock(MaterialPropertyBlock properties)
---public Void GetPropertyBlock(MaterialPropertyBlock properties, Int32 materialIndex)
---@param MaterialPropertyBlock properties
---@param optional Int32 materialIndex
function m:GetPropertyBlock(properties, materialIndex) end
---public Void GetMaterials(List`1 m)
---@param optional List`1 m
function m:GetMaterials(m) end
---public Void GetSharedMaterials(List`1 m)
---@param optional List`1 m
function m:GetSharedMaterials(m) end
---public Void GetClosestReflectionProbes(List`1 result)
---@param optional List`1 result
function m:GetClosestReflectionProbes(result) end
UnityEngine.Renderer = m
return m
