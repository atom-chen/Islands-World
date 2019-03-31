---@class Coolape.CLPanelBase : Coolape.CLBaseLua
---@field public isNeedBackplate System.Boolean
---@field public destroyWhenHide System.Boolean
---@field public isNeedResetAtlase System.Boolean
---@field public isNeedMask4Init System.Boolean
---@field public isNeedMask4InitOnlyOnce System.Boolean
---@field public isHideWithEffect System.Boolean
---@field public isRefeshContentWhenEffectFinish System.Boolean
---@field public EffectRoot UnityEngine.Transform
---@field public effectType Coolape.CLPanelBase.EffectType
---@field public EffectList System.Collections.Generic.List1UITweener
---@field public isActive System.Boolean
---@field public panel UIPanel
---@field public destoryDelaySec System.Int32
---@field public mask4Hide UnityEngine.GameObject
---@field public isFinishLoad System.Boolean
---@field public isFinishInit System.Boolean
---@field public depth System.Int32

local m = { }
---public Int32 SortByName(UITweener a, UITweener b)
---@return number
---@param optional UITweener a
---@param optional UITweener b
function m.SortByName(a, b) end
---public Void Start()
function m:Start() end
---public Void sortTweeners(UITweener[] list)
---@param optional UITweener[] list
function m:sortTweeners(list) end
---public Void showWithEffect(Object finishShowingCallback)
---@param optional Object finishShowingCallback
function m:showWithEffect(finishShowingCallback) end
---public Void hideWithEffect(Boolean moveOut)
---@param optional Boolean moveOut
function m:hideWithEffect(moveOut) end
---public Void finishMoveOut()
function m:finishMoveOut() end
---public Void procNetwork(String fname, Int32 succ, String msg, Object pars)
---@param optional String fname
---@param optional Int32 succ
---@param optional String msg
---@param optional Object pars
function m:procNetwork(fname, succ, msg, pars) end
---public Void init()
function m:init() end
---public Void getSubPanelsDepth()
function m:getSubPanelsDepth() end
---public Void setSubPanelsDepth()
function m:setSubPanelsDepth() end
---public Void setData(Object pars)
---@param optional Object pars
function m:setData(pars) end
---public Void show()
---public Void show(Object pars)
---@param Object pars
function m:show(pars) end
---public Void onTopPanelChange(CLPanelBase p)
---@param optional CLPanelBase p
function m:onTopPanelChange(p) end
---public Void refresh()
function m:refresh() end
---public Boolean hideSelfOnKeyBack()
---@return bool
function m:hideSelfOnKeyBack() end
---public Void hide()
function m:hide() end
---public Void OnDestroy()
function m:OnDestroy() end
---public Void prepare(Object callback, Object orgs)
---@param optional Object callback
---@param optional Object orgs
function m:prepare(callback, orgs) end
---public Void prepareSprites4BorrowMode(ArrayList list, Int32 i, Object callback, Object orgs)
---@param optional ArrayList list
---@param optional Int32 i
---@param optional Object callback
---@param optional Object orgs
function m:prepareSprites4BorrowMode(list, i, callback, orgs) end
---public Void prepareOneSprite4BorrowMode(UIAtlas atlas, String spriteName, Object callback, Object orgs)
---@param optional UIAtlas atlas
---@param optional String spriteName
---@param optional Object callback
---@param optional Object orgs
function m:prepareOneSprite4BorrowMode(atlas, spriteName, callback, orgs) end
Coolape.CLPanelBase = m
return m
