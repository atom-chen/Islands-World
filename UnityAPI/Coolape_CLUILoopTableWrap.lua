---@class Coolape.CLUILoopTable : UnityEngine.MonoBehaviour
---@field public cellCount System.Int32
---@field public mOffset UnityEngine.Vector2
---@field public scrollView UIScrollView
local m = { }
---public CLUILoopTable .ctor()
---@return CLUILoopTable
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
---public Void refreshContentOnly(Object data, Boolean isRePositionTable)
---@param Object data
---@param Boolean isRePositionTable
function m:refreshContentOnly(data, isRePositionTable) end
---public Void setList(Object data, Object initCellCallback)
---public Void setList(Object data, Object initCellCallback, Object onEndListCallback)
---public Void setList(Object data, Object initCellCallback, Object onEndListCallback, Boolean isNeedRePosition)
---public Void setList(Object data, Object initCellCallback, Object onEndListCallback, Boolean isNeedRePosition, Boolean isCalculatePosition)
---@param Object data
---@param Object initCellCallback
---@param Object onEndListCallback
---@param optional Boolean isNeedRePosition
---@param optional Boolean isCalculatePosition
function m:setList(data, initCellCallback, onEndListCallback, isNeedRePosition, isCalculatePosition) end
---public Void insertList(Object data, Boolean isNeedRePosition, Boolean isCalculatePosition)
---@param optional Object data
---@param optional Boolean isNeedRePosition
---@param optional Boolean isCalculatePosition
function m:insertList(data, isNeedRePosition, isCalculatePosition) end
---public Void appendList(Object data, Boolean isNeedRePosition, Boolean isCalculatePosition)
---@param optional Object data
---@param optional Boolean isNeedRePosition
---@param optional Boolean isCalculatePosition
function m:appendList(data, isNeedRePosition, isCalculatePosition) end
Coolape.CLUILoopTable = m
return m
