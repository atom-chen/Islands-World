---@class MyUIPanel : Coolape.CLPanelLua
---@field public frameName System.String
---@field public frameObj Coolape.CLCellLua

local m = { }
---public MyUIPanel .ctor()
---@return MyUIPanel
function m.New() end
---public Void init()
function m:init() end
---public Void showFrame()
function m:showFrame() end
---public Void releaseFrame()
function m:releaseFrame() end
---public Void show()
function m:show() end
---public Void hide()
function m:hide() end
MyUIPanel = m
return m
