---@class UIScrollView : UnityEngine.MonoBehaviour
---@field public list BetterList1UIScrollView
---@field public movement UIScrollView.Movement
---@field public dragEffect UIScrollView.DragEffect
---@field public restrictWithinPanel System.Boolean
---@field public disableDragIfFits System.Boolean
---@field public smoothDragStart System.Boolean
---@field public iOSDragEmulation System.Boolean
---@field public scrollWheelFactor System.Single
---@field public momentumAmount System.Single
---@field public dampenStrength System.Single
---@field public horizontalScrollBar UIProgressBar
---@field public verticalScrollBar UIProgressBar
---@field public showScrollBars UIScrollView.ShowCondition
---@field public customMovement UnityEngine.Vector2
---@field public contentPivot UIWidget.Pivot
---@field public onDragStarted UIScrollView.OnDragNotification
---@field public onDragFinished UIScrollView.OnDragNotification
---@field public onMomentumMove UIScrollView.OnDragNotification
---@field public onDragmMove UIScrollView.OnDragNotification
---@field public onStoppedMoving UIScrollView.OnDragNotification
---@field public onStartCenterOnChild UIScrollView.OnDragNotification
---@field public centerOnChild UICenterOnChild
---@field public panel UIPanel
---@field public isDragging System.Boolean
---@field public bounds UnityEngine.Bounds
---@field public canMoveHorizontally System.Boolean
---@field public canMoveVertically System.Boolean
---@field public shouldMoveHorizontally System.Boolean
---@field public shouldMoveVertically System.Boolean
---@field public currentMomentum UnityEngine.Vector3

local m = { }
---public UIScrollView .ctor()
---@return UIScrollView
function m.New() end
---public Boolean RestrictWithinBounds(Boolean instant)
---public Boolean RestrictWithinBounds(Boolean instant, Boolean horizontal, Boolean vertical)
---@return bool
---@param Boolean instant
---@param Boolean horizontal
---@param optional Boolean vertical
function m:RestrictWithinBounds(instant, horizontal, vertical) end
---public Void DisableSpring()
function m:DisableSpring() end
---public Void UpdateScrollbars()
---public Void UpdateScrollbars(Boolean recalculateBounds)
---@param Boolean recalculateBounds
function m:UpdateScrollbars(recalculateBounds) end
---public Void SetDragAmount(Single x, Single y, Boolean updateScrollbars)
---@param optional Single x
---@param optional Single y
---@param optional Boolean updateScrollbars
function m:SetDragAmount(x, y, updateScrollbars) end
---public Void InvalidateBounds()
function m:InvalidateBounds() end
---public Void ResetPosition()
function m:ResetPosition() end
---public Void UpdatePosition()
function m:UpdatePosition() end
---public Void OnScrollBar(GameObject go)
---@param optional GameObject go
function m:OnScrollBar(go) end
---public Void MoveRelative(Vector3 relative)
---@param optional Vector3 relative
function m:MoveRelative(relative) end
---public Void MoveAbsolute(Vector3 absolute)
---@param optional Vector3 absolute
function m:MoveAbsolute(absolute) end
---public Void Press(Boolean pressed)
---@param optional Boolean pressed
function m:Press(pressed) end
---public Void Drag()
function m:Drag() end
---public Void Scroll(Single delta)
---@param optional Single delta
function m:Scroll(delta) end
UIScrollView = m
return m
