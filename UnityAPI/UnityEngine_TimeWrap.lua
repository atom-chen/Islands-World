---@class UnityEngine.Time
---@field public time System.Single
---@field public timeSinceLevelLoad System.Single
---@field public deltaTime System.Single
---@field public fixedTime System.Single
---@field public unscaledTime System.Single
---@field public fixedUnscaledTime System.Single
---@field public unscaledDeltaTime System.Single
---@field public fixedUnscaledDeltaTime System.Single
---@field public fixedDeltaTime System.Single
---@field public maximumDeltaTime System.Single
---@field public smoothDeltaTime System.Single
---@field public maximumParticleDeltaTime System.Single
---@field public timeScale System.Single
---@field public frameCount System.Int32
---@field public renderedFrameCount System.Int32
---@field public realtimeSinceStartup System.Single
---@field public captureDeltaTime System.Single
---@field public captureFramerate System.Int32
---@field public inFixedTimeStep System.Boolean
local m = { }
---public Time .ctor()
---@return Time
function m.New() end
UnityEngine.Time = m
return m
