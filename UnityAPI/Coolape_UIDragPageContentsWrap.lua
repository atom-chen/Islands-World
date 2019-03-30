---@class Coolape.UIDragPageContents : UIDragScrollView
---@field public _gridPage Coolape.UIGridPage
---@field public transform UnityEngine.Transform
---@field public gridPage Coolape.UIGridPage

local m = { }
---public UIDragPageContents .ctor()
---@return UIDragPageContents
function m.New() end
---public Void OnPress(Boolean isPressed)
---@param optional Boolean isPressed
function m:OnPress(isPressed) end
---public Void OnDrag(Vector2 delta)
---@param optional Vector2 delta
function m:OnDrag(delta) end
---public Void init(Object obj, Int32 index)
---@param optional Object obj
---@param optional Int32 index
function m:init(obj, index) end
---public Void refreshCurrent(Int32 pageIndex, Object obj)
---@param optional Int32 pageIndex
---@param optional Object obj
function m:refreshCurrent(pageIndex, obj) end
Coolape.UIDragPageContents = m
return m
