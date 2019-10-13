---@class Coolape.CLPathCfg : UnityEngine.MonoBehaviour
---@field public self Coolape.CLPathCfg
---@field public basePath System.String
---@field public panelDataPath System.String
---@field public cellDataPath System.String
---@field public localizationPath System.String
---@field public luaPathRoot System.String
---@field public persistentDataPath System.String
---@field public platform System.String
---@field public runtimePlatform System.String
---@field public _cellDataPath System.String
---@field public upgradeRes System.String
local m = { }
---public CLPathCfg .ctor()
---@return CLPathCfg
function m.New() end
---public Void resetPath(String basePath)
---@param optional String basePath
function m:resetPath(basePath) end
Coolape.CLPathCfg = m
return m
