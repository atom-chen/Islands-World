---@class CameraMgr : Coolape.CLBaseLua
---@field public self CameraMgr
---@field public maincamera UnityEngine.Camera
---@field public subcamera UnityEngine.Camera
---@field public postprocessing UnityEngine.PostProcessing.PostProcessingBehaviour
---@field public subpostprocessing UnityEngine.PostProcessing.PostProcessingBehaviour
---@field public fieldOfView System.Single
---@field public postProcessingProfile UnityEngine.PostProcessing.PostProcessingProfile
---@field public postProcessingProfileSub UnityEngine.PostProcessing.PostProcessingProfile

local m = { }
---public CameraMgr .ctor()
---@return CameraMgr
function m.New() end
CameraMgr = m
return m
