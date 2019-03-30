---@class UnityEngine.PostProcessing.PostProcessingBehaviour : UnityEngine.MonoBehaviour
---@field public profile UnityEngine.PostProcessing.PostProcessingProfile
---@field public jitteredMatrixFunc System.Func2UnityEngine.Vector2UnityEngine.Matrix4x4

local m = { }
---public PostProcessingBehaviour .ctor()
---@return PostProcessingBehaviour
function m.New() end
---public Void ResetTemporalEffects()
function m:ResetTemporalEffects() end
UnityEngine.PostProcessing.PostProcessingBehaviour = m
return m
