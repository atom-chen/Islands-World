---@class Coolape.CLCellLua : Coolape.CLCellBase
---@field public isNeedResetAtlase System.Boolean
local m = { }
---public CLCellLua .ctor()
---@return CLCellLua
function m.New() end
---public Void setLua()
function m:setLua() end
---public Void initLuaFunc()
function m:initLuaFunc() end
---public Void init(Object data)
---public Void init(Object data, Object onClick)
---@param Object data
---@param optional Object onClick
function m:init(data, onClick) end
---public Void refresh(Object paras)
---@param optional Object paras
function m:refresh(paras) end
---public Void OnClick()
function m:OnClick() end
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
Coolape.CLCellLua = m
return m
