---@class Coolape.CLUILoopGrid : UnityEngine.MonoBehaviour
---@field public cellCount System.Int32
---@field public isPlayTween System.Boolean
---@field public twType Coolape.TweenType
---@field public tweenSpeed System.Single
---@field public twDuration System.Single
---@field public twMethod UITweener.Method
---@field public itemList System.Collections.Generic.List1UIWidget
---@field public grid UIGrid
---@field public panel UIPanel
---@field public list System.Collections.ArrayList
---@field public scrollView UIScrollView
local m = { }
---public CLUILoopGrid .ctor()
---@return CLUILoopGrid
function m.New() end
---public Void init()
function m:init() end
---public Void setOldClip(Vector2 oldClipOffset, Vector3 oldScrollViewPos, Vector3 oldGridPosition)
---@param optional Vector2 oldClipOffset
---@param optional Vector3 oldScrollViewPos
---@param optional Vector3 oldGridPosition
function m:setOldClip(oldClipOffset, oldScrollViewPos, oldGridPosition) end
---public Void resetClip()
function m:resetClip() end
---public Void refreshContentOnly()
---public Void refreshContentOnly(Object data)
---public Void refreshContentOnly(Object data, Boolean UpdatePosition)
---@param Object data
---@param Boolean UpdatePosition
function m:refreshContentOnly(data, UpdatePosition) end
---public Void insertList(Object data)
---@param optional Object data
function m:insertList(data) end
---public Void setListData(Object data, Object initCellCallback, Boolean isFirst)
---@param optional Object data
---@param optional Object initCellCallback
---@param optional Boolean isFirst
function m:setListData(data, initCellCallback, isFirst) end
---public Void setList(Object data, Object initCellCallback)
---public Void setList(Object data, Object initCellCallback, Object onEndListCallback)
---public Void setList(Object data, Object initCellCallback, Object onHeadListCallback, Object onEndListCallback)
---public Void setList(Object data, Object initCellCallback, Object onHeadListCallback, Object onEndListCallback, Boolean isNeedRePosition)
---public Void setList(Object data, Object initCellCallback, Object onHeadListCallback, Object onEndListCallback, Boolean isNeedRePosition, Boolean isPlayTween)
---public Void setList(Object data, Object initCellCallback, Object onHeadListCallback, Object onEndListCallback, Boolean isNeedRePosition, Boolean isPlayTween, Single tweenSpeed)
---public Void setList(Object data, Object initCellCallback, Object onHeadListCallback, Object onEndListCallback, Boolean isNeedRePosition, Boolean isPlayTween, Single tweenSpeed, Single twDuration)
---public Void setList(Object data, Object initCellCallback, Object onHeadListCallback, Object onEndListCallback, Boolean isNeedRePosition, Boolean isPlayTween, Single tweenSpeed, Single twDuration, Method twMethod)
---public Void setList(Object data, Object initCellCallback, Object onHeadListCallback, Object onEndListCallback, Boolean isNeedRePosition, Boolean isPlayTween, Single tweenSpeed, Single twDuration, Method twMethod, TweenType twType)
---@param Object data
---@param Object initCellCallback
---@param Object onHeadListCallback
---@param Object onEndListCallback
---@param Boolean isNeedRePosition
---@param Boolean isPlayTween
---@param Single tweenSpeed
---@param Single twDuration
---@param optional Method twMethod
---@param optional TweenType twType
function m:setList(data, initCellCallback, onHeadListCallback, onEndListCallback, isNeedRePosition, isPlayTween, tweenSpeed, twDuration, twMethod, twType) end
---public Void appendList(Object data)
---@param optional Object data
function m:appendList(data) end
---public Void Update()
function m:Update() end
Coolape.CLUILoopGrid = m
return m
