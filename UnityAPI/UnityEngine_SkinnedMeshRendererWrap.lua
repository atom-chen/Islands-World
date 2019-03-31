---@class UnityEngine.SkinnedMeshRenderer : UnityEngine.Renderer
---@field public quality UnityEngine.SkinQuality
---@field public updateWhenOffscreen System.Boolean
---@field public forceMatrixRecalculationPerRender System.Boolean
---@field public rootBone UnityEngine.Transform
---@field public bones UnityEngine.Transform
---@field public sharedMesh UnityEngine.Mesh
---@field public skinnedMotionVectors System.Boolean
---@field public localBounds UnityEngine.Bounds

local m = { }
---public SkinnedMeshRenderer .ctor()
---@return SkinnedMeshRenderer
function m.New() end
---public Single GetBlendShapeWeight(Int32 index)
---@return number
---@param optional Int32 index
function m:GetBlendShapeWeight(index) end
---public Void SetBlendShapeWeight(Int32 index, Single value)
---@param optional Int32 index
---@param optional Single value
function m:SetBlendShapeWeight(index, value) end
---public Void BakeMesh(Mesh mesh)
---@param optional Mesh mesh
function m:BakeMesh(mesh) end
UnityEngine.SkinnedMeshRenderer = m
return m
