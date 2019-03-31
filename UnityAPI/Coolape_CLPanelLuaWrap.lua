---@class Coolape.CLPanelLua : Coolape.CLPanelBase

local m = { }
---public CLPanelLua .ctor()
---@return CLPanelLua
function m.New() end
---public Void reLoadLua()
function m:reLoadLua() end
---public Void setLua()
function m:setLua() end
---public Void OnApplicationPause(Boolean isPause)
---@param optional Boolean isPause
function m:OnApplicationPause(isPause) end
---public Void OnDestroy()
function m:OnDestroy() end
---public Void onTopPanelChange(CLPanelBase p)
---@param optional CLPanelBase p
function m:onTopPanelChange(p) end
---public Boolean hideSelfOnKeyBack()
---@return bool
function m:hideSelfOnKeyBack() end
---public Void hide()
function m:hide() end
---public Void init()
function m:init() end
---public Void setData(Object pars)
---@param optional Object pars
function m:setData(pars) end
---public Void procNetwork(String cmd, Int32 succ, String msg, Object pars)
---@param optional String cmd
---@param optional Int32 succ
---@param optional String msg
---@param optional Object pars
function m:procNetwork(cmd, succ, msg, pars) end
---public Void show()
---public Void show(Object pars)
---@param Object pars
function m:show(pars) end
---public Void onfinishShowMask(Object[] para)
---@param optional Object[] para
function m:onfinishShowMask(para) end
---public Void doPrepare(Object callback)
---@param optional Object callback
function m:doPrepare(callback) end
---public Void doHideMask()
function m:doHideMask() end
---public Void _show()
function m:_show() end
---public Void refresh()
function m:refresh() end
---public Void uiEventDelegate(GameObject go)
---@param optional GameObject go
function m:uiEventDelegate(go) end
---public Void onClick4Lua(GameObject button, String functionName)
---@param optional GameObject button
---@param optional String functionName
function m:onClick4Lua(button, functionName) end
---public Void onDoubleClick4Lua(GameObject button, String functionName)
---@param optional GameObject button
---@param optional String functionName
function m:onDoubleClick4Lua(button, functionName) end
---public Void onHover4Lua(GameObject button, String functionName, Boolean isOver)
---@param optional GameObject button
---@param optional String functionName
---@param optional Boolean isOver
function m:onHover4Lua(button, functionName, isOver) end
---public Void onPress4Lua(GameObject button, String functionName, Boolean isPressed)
---@param optional GameObject button
---@param optional String functionName
---@param optional Boolean isPressed
function m:onPress4Lua(button, functionName, isPressed) end
---public Void onDrag4Lua(GameObject button, String functionName, Vector2 delta)
---@param optional GameObject button
---@param optional String functionName
---@param optional Vector2 delta
function m:onDrag4Lua(button, functionName, delta) end
---public Void onDrop4Lua(GameObject button, String functionName, GameObject go)
---@param optional GameObject button
---@param optional String functionName
---@param optional GameObject go
function m:onDrop4Lua(button, functionName, go) end
---public Void onKey4Lua(GameObject button, String functionName, KeyCode key)
---@param optional GameObject button
---@param optional String functionName
---@param optional KeyCode key
function m:onKey4Lua(button, functionName, key) end
Coolape.CLPanelLua = m
return m
