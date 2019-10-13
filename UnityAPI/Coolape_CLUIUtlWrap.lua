---@class Coolape.CLUIUtl
local m = { }
---public CLUIUtl .ctor()
---@return CLUIUtl
function m.New() end
---public Void appendList4Lua(UIGrid parent, GameObject prefabChild, ArrayList list, Int32 beforCount, Object initCallback)
---@param optional UIGrid parent
---@param optional GameObject prefabChild
---@param optional ArrayList list
---@param optional Int32 beforCount
---@param optional Object initCallback
function m.appendList4Lua(parent, prefabChild, list, beforCount, initCallback) end
---public Void appendList(UIGrid parent, GameObject prefabChild, ArrayList list, Type itype, Int32 beforCount, Object initCallback)
---public Void appendList(UIGrid parent, GameObject prefabChild, ArrayList list, Type itype, Int32 beforCount, GameObject nextPage, Object initCallback, Single offset)
---@param UIGrid parent
---@param GameObject prefabChild
---@param optional ArrayList list
---@param optional Type itype
---@param optional Int32 beforCount
---@param optional GameObject nextPage
---@param optional Object initCallback
---@param optional Single offset
function m.appendList(parent, prefabChild, list, itype, beforCount, nextPage, initCallback, offset) end
---public Void resetList4Lua(Object parent, GameObject prefabChild, Object list, Object initCallback)
---public Void resetList4Lua(Object parent, GameObject prefabChild, Object list, Object initCallback, Boolean isReposition)
---public Void resetList4Lua(Object parent, GameObject prefabChild, Object list, Object initCallback, Boolean isReposition, Boolean isPlayTween, Single tweenSpeed)
---@param Object parent
---@param GameObject prefabChild
---@param Object list
---@param optional Object initCallback
---@param optional Boolean isReposition
---@param optional Boolean isPlayTween
---@param optional Single tweenSpeed
function m.resetList4Lua(parent, prefabChild, list, initCallback, isReposition, isPlayTween, tweenSpeed) end
---public Void resetList(Object parent, GameObject prefabChild, Object list, Type itype, Object initCallback, Boolean isReposition, Boolean isPlayTween, Single tweenSpeed)
---public Void resetList(Object parent, GameObject prefabChild, Object data, Type itype, GameObject nextPage, Boolean isShowNoneContent, Object initCallback, Boolean isReposition, Boolean isPlayTween, Single tweenSpeed)
---@param Object parent
---@param GameObject prefabChild
---@param optional Object data
---@param optional Type itype
---@param optional GameObject nextPage
---@param optional Boolean isShowNoneContent
---@param optional Object initCallback
---@param optional Boolean isReposition
---@param optional Boolean isPlayTween
---@param optional Single tweenSpeed
function m.resetList(parent, prefabChild, data, itype, nextPage, isShowNoneContent, initCallback, isReposition, isPlayTween, tweenSpeed) end
---public Void resetCellTween(Int32 index, Object gridObj, GameObject cell, Single tweenSpeed, Single duration, Method method, TweenType twType)
---@param optional Int32 index
---@param optional Object gridObj
---@param optional GameObject cell
---@param optional Single tweenSpeed
---@param optional Single duration
---@param optional Method method
---@param optional TweenType twType
function m.resetCellTween(index, gridObj, cell, tweenSpeed, duration, method, twType) end
---public Void resetCellTweenPosition(Int32 index, Object gridObj, GameObject cell, Single tweenSpeed, Single duration, Method method)
---@param optional Int32 index
---@param optional Object gridObj
---@param optional GameObject cell
---@param optional Single tweenSpeed
---@param optional Single duration
---@param optional Method method
function m.resetCellTweenPosition(index, gridObj, cell, tweenSpeed, duration, method) end
---public Void resetCellTweenScale(Int32 index, Object gridObj, GameObject cell, Single tweenSpeed, Single duration, Method method)
---@param optional Int32 index
---@param optional Object gridObj
---@param optional GameObject cell
---@param optional Single tweenSpeed
---@param optional Single duration
---@param optional Method method
function m.resetCellTweenScale(index, gridObj, cell, tweenSpeed, duration, method) end
---public Void resetCellTweenAlpha(Int32 index, Object gridObj, GameObject cell, Single tweenSpeed, Single duration, Method method)
---@param optional Int32 index
---@param optional Object gridObj
---@param optional GameObject cell
---@param optional Single tweenSpeed
---@param optional Single duration
---@param optional Method method
function m.resetCellTweenAlpha(index, gridObj, cell, tweenSpeed, duration, method) end
---public Void resetChatList(GameObject grid, GameObject prefabChild, ArrayList list, Type itype, Single offsetY, Object initCallback)
---@param optional GameObject grid
---@param optional GameObject prefabChild
---@param optional ArrayList list
---@param optional Type itype
---@param optional Single offsetY
---@param optional Object initCallback
function m.resetChatList(grid, prefabChild, list, itype, offsetY, initCallback) end
---public Void showConfirm(String msg, Object callback)
---public Void showConfirm(String msg, Object callback1, Object callback2)
---public Void showConfirm(String msg, Boolean isShowOneButton, String button1, Object callback1, String button2, Object callback2)
---@param String msg
---@param Boolean isShowOneButton
---@param String button1
---@param Object callback1
---@param optional String button2
---@param optional Object callback2
function m.showConfirm(msg, isShowOneButton, button1, callback1, button2, callback2) end
---public Void setSpriteFit(UISprite sprite, String sprName)
---public Void setSpriteFit(UISprite sprite, String sprName, Int32 maxSize)
---@param UISprite sprite
---@param optional String sprName
---@param optional Int32 maxSize
function m.setSpriteFit(sprite, sprName, maxSize) end
---public Void onGetSprite(Object[] paras)
---@param optional Object[] paras
function m.onGetSprite(paras) end
---public Void setAllSpriteGray(GameObject gobj, Boolean isGray)
---@param optional GameObject gobj
---@param optional Boolean isGray
function m.setAllSpriteGray(gobj, isGray) end
---public Void setSpriteGray(UISprite spr, Boolean isGray)
---@param optional UISprite spr
---@param optional Boolean isGray
function m.setSpriteGray(spr, isGray) end
---public Void resetAtlasAndFont(Transform tr, Boolean isClean)
---@param optional Transform tr
---@param optional Boolean isClean
function m.resetAtlasAndFont(tr, isClean) end
---public Void _resetAtlasAndFont(Transform tr, Boolean isClean)
---@param optional Transform tr
---@param optional Boolean isClean
function m._resetAtlasAndFont(tr, isClean) end
Coolape.CLUIUtl = m
return m
