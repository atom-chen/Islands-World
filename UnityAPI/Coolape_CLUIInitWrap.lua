---@class Coolape.CLUIInit : UnityEngine.MonoBehaviour
---@field public atlasAllRealName System.String
---@field public emptFont UIFont
---@field public emptAtlas UIAtlas
---@field public uiPublicRoot UnityEngine.Transform
---@field public self Coolape.CLUIInit
---@field public fontMap System.Collections.Generic.Dictionary2System.StringUIFont
---@field public atlasMap System.Collections.Generic.Dictionary2System.StringUIAtlas
---@field public PanelConfirmDepth System.Int32
---@field public PanelHotWheelDepth System.Int32
---@field public PanelWWWProgressDepth System.Int32
---@field public AlertRootDepth System.Int32
local m = { }
---public CLUIInit .ctor()
---@return CLUIInit
function m.New() end
---public Void clean()
function m:clean() end
---public Boolean init()
---@return bool
function m:init() end
---public Boolean initAtlas()
---@return bool
function m:initAtlas() end
---public UIFont getFontByName(String fontName)
---@return UIFont
---@param optional String fontName
function m:getFontByName(fontName) end
---public UIAtlas getAtlasByName(String atlasName)
---@return UIAtlas
---@param optional String atlasName
function m:getAtlasByName(atlasName) end
Coolape.CLUIInit = m
return m
