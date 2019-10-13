---@class Coolape.UIGridPage : UIGrid
---@field public isLimitless System.Boolean
---@field public isReverse System.Boolean
---@field public dragSensitivity System.Single
---@field public page1 Coolape.UIDragPageContents
---@field public page2 Coolape.UIDragPageContents
---@field public page3 Coolape.UIDragPageContents
---@field public currCell System.Int32
---@field public currPage Coolape.UIGridPage.LoopPage
---@field public scrollView UIScrollView
---@field public moveToCell Coolape.UIMoveToCell
local m = { }
---public UIGridPage .ctor()
---@return UIGridPage
function m.New() end
---public Void Start()
function m:Start() end
---public Void init(Object pageList, Object onRefreshCurrentPage, Int32 defaltPage)
---@param optional Object pageList
---@param optional Object onRefreshCurrentPage
---@param optional Int32 defaltPage
function m:init(pageList, onRefreshCurrentPage, defaltPage) end
---public Void moveTo(Boolean force)
---public Void moveTo(Int32 index)
---@param optional Int32 index
function m:moveTo(index) end
---public Void onPress(Boolean isPressed)
---@param optional Boolean isPressed
function m:onPress(isPressed) end
---public Void onDrag(Vector2 delta)
---@param optional Vector2 delta
function m:onDrag(delta) end
---public Void procMoveCell()
function m:procMoveCell() end
Coolape.UIGridPage = m
return m
