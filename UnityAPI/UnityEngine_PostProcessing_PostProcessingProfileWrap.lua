---@class UnityEngine.PostProcessing.PostProcessingProfile : UnityEngine.ScriptableObject
---@field public debugViews UnityEngine.PostProcessing.BuiltinDebugViewsModel
---@field public fog UnityEngine.PostProcessing.FogModel
---@field public antialiasing UnityEngine.PostProcessing.AntialiasingModel
---@field public ambientOcclusion UnityEngine.PostProcessing.AmbientOcclusionModel
---@field public screenSpaceReflection UnityEngine.PostProcessing.ScreenSpaceReflectionModel
---@field public depthOfField UnityEngine.PostProcessing.DepthOfFieldModel
---@field public motionBlur UnityEngine.PostProcessing.MotionBlurModel
---@field public eyeAdaptation UnityEngine.PostProcessing.EyeAdaptationModel
---@field public bloom UnityEngine.PostProcessing.BloomModel
---@field public colorGrading UnityEngine.PostProcessing.ColorGradingModel
---@field public userLut UnityEngine.PostProcessing.UserLutModel
---@field public chromaticAberration UnityEngine.PostProcessing.ChromaticAberrationModel
---@field public grain UnityEngine.PostProcessing.GrainModel
---@field public vignette UnityEngine.PostProcessing.VignetteModel
---@field public dithering UnityEngine.PostProcessing.DitheringModel
---@field public monitors UnityEngine.PostProcessing.PostProcessingProfile.MonitorSettings

local m = { }
---public PostProcessingProfile .ctor()
---@return PostProcessingProfile
function m.New() end
UnityEngine.PostProcessing.PostProcessingProfile = m
return m
