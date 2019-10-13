---@class MyCfg : Coolape.CLCfgBase
---@field public mode System.Int32
---@field public default_UID System.String
---@field public lookAtTarget UnityEngine.Transform
---@field public directionalLight UnityEngine.Light
---@field public mainCamera UnityEngine.Camera
---@field public uiCamera UnityEngine.Camera
---@field public hud3dRoot UnityEngine.Transform
---@field public shadowRoot UnityEngine.Transform
---@field public fogOfWar SimpleFogOfWar.FogOfWarSystem
---@field public _isEditScene System.Boolean
---@field public isEditScene System.Boolean
local m = { }
---public MyCfg .ctor()
---@return MyCfg
function m.New() end
MyCfg = m
return m
