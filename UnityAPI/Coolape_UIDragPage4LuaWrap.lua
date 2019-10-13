---@class Coolape.UIDragPage4Lua : Coolape.UIDragPageContents
---@field public uiLua Coolape.CLCellLua
local m = { }
---public UIDragPage4Lua .ctor()
---@return UIDragPage4Lua
function m.New() end
---public Void init(Object obj, Int32 index)
---@param optional Object obj
---@param optional Int32 index
function m:init(obj, index) end
---public Void refreshCurrent(Int32 pageIndex, Object obj)
---@param optional Int32 pageIndex
---@param optional Object obj
function m:refreshCurrent(pageIndex, obj) end
Coolape.UIDragPage4Lua = m
return m
