---@class Coolape.CLPBackplate : Coolape.CLPanelLua
---@field public self Coolape.CLPBackplate
local m = { }
---public CLPBackplate .ctor()
---@return CLPBackplate
function m.New() end
---public Void show()
function m:show() end
---public Void procOtherPanel()
function m:procOtherPanel() end
---public Void proc(CLPanelBase clpanel)
---@param optional CLPanelBase clpanel
function m:proc(clpanel) end
---public Void _proc(CLPanelBase clpanel)
---@param optional CLPanelBase clpanel
function m:_proc(clpanel) end
Coolape.CLPBackplate = m
return m
