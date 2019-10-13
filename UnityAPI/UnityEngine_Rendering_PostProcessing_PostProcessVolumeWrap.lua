---@class UnityEngine.Rendering.PostProcessing.PostProcessVolume : UnityEngine.MonoBehaviour
---@field public sharedProfile UnityEngine.Rendering.PostProcessing.PostProcessProfile
---@field public isGlobal System.Boolean
---@field public blendDistance System.Single
---@field public weight System.Single
---@field public priority System.Single
---@field public profile UnityEngine.Rendering.PostProcessing.PostProcessProfile
local m = { }
---public PostProcessVolume .ctor()
---@return PostProcessVolume
function m.New() end
---public Boolean HasInstantiatedProfile()
---@return bool
function m:HasInstantiatedProfile() end
UnityEngine.Rendering.PostProcessing.PostProcessVolume = m
return m
