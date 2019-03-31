---@class CameraMgr : Coolape.CLBaseLua
---@field public self CameraMgr
---@field public maincamera UnityEngine.Camera
---@field public subcamera UnityEngine.Camera
---@field public postprocessing UnityEngine.Rendering.PostProcessing.PostProcessVolume
---@field public subpostprocessing UnityEngine.Rendering.PostProcessing.PostProcessVolume
---@field public fieldOfView System.Single
---@field public postProcessingProfile UnityEngine.Rendering.PostProcessing.PostProcessProfile
---@field public postProcessingProfileSub UnityEngine.Rendering.PostProcessing.PostProcessProfile

local m = { }
---public CameraMgr .ctor()
---@return CameraMgr
function m.New() end
---public Boolean isInCameraView(Camera cam, Bounds bounds)
---@return bool
---@param optional Camera cam
---@param optional Bounds bounds
function m.isInCameraView(cam, bounds) end
---public Void showCameraView()
function m:showCameraView() end
CameraMgr = m
return m
