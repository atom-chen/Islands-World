---@class SFourWayArrow : Coolape.CLBaseLua
---@field public self SFourWayArrow
---@field public arrow1 SFourWayArrowCell
---@field public arrow2 SFourWayArrowCell
---@field public arrow3 SFourWayArrowCell
---@field public arrow4 SFourWayArrowCell
---@field public building Coolape.CLBehaviour4Lua
---@field public cellSize System.Int32
---@field public material UnityEngine.Material
local m = { }
---public SFourWayArrow .ctor()
---@return SFourWayArrow
function m.New() end
---public Void init()
function m:init() end
---public Void show(CLBehaviour4Lua parent, Int32 size)
---@param optional CLBehaviour4Lua parent
---@param optional Int32 size
function m.show(parent, size) end
---public Void hide()
function m.hide() end
---public Void setMatToon()
---public Void setMatToon(Color color)
---@param Color color
function m.setMatToon(color) end
---public Void _show(GameObject parent, Int32 size)
---@param optional GameObject parent
---@param optional Int32 size
function m:_show(parent, size) end
---public Void _hide()
function m:_hide() end
---public Void Update()
function m:Update() end
---public Void OnClick()
function m:OnClick() end
---public Void OnPress(Boolean isPressed)
---@param optional Boolean isPressed
function m:OnPress(isPressed) end
---public Void OnDrag(Vector2 delta)
---@param optional Vector2 delta
function m:OnDrag(delta) end
SFourWayArrow = m
return m
