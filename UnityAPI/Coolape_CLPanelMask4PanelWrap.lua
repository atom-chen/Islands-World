---@class Coolape.CLPanelMask4Panel : Coolape.CLPanelLua
---@field public tweenAlpha TweenAlpha
---@field public sprite UISprite
---@field public label UILabel
---@field public self Coolape.CLPanelMask4Panel
---@field public defautSpriteNameList System.Collections.Generic.List1System.String
local m = { }
---public CLPanelMask4Panel .ctor()
---@return CLPanelMask4Panel
function m.New() end
---public Void init()
function m:init() end
---public Void _show(Object callback, List`1 list)
---@param optional Object callback
---@param optional List`1 list
function m:_show(callback, list) end
---public Void _hide(Object callback)
---@param optional Object callback
function m:_hide(callback) end
---public Void onTweenFinish(GameObject go)
---@param optional GameObject go
function m:onTweenFinish(go) end
---public Void doCallback()
function m:doCallback() end
---public Void show(Object callback, List`1 list)
---@param optional Object callback
---@param optional List`1 list
function m.show(callback, list) end
---public Void hide(Object callback)
---@param optional Object callback
function m.hide(callback) end
Coolape.CLPanelMask4Panel = m
return m
