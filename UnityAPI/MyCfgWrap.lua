---@class MyCfg : Coolape.CLCfgBase
---@field public mode System.Int32
---@field public default_UID System.String
---@field public lookAtTarget UnityEngine.Transform
---@field public directionalLight UnityEngine.Light
---@field public mainCamera UnityEngine.Camera
---@field public uiCamera UnityEngine.Camera
---@field public buildingSize UnityEngine.GameObject
---@field public hud3dRoot UnityEngine.Transform

local m = { }
---public MyCfg .ctor()
---@return MyCfg
function m.New() end
MyCfg = m
return m
