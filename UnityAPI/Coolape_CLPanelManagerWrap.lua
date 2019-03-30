---@class Coolape.CLPanelManager : UnityEngine.MonoBehaviour
---@field public self Coolape.CLPanelManager
---@field public _uiPanelRoot UnityEngine.Transform
---@field public depth System.Int32
---@field public Const_RenderQueue System.Int32
---@field public cachePanelSize System.Int32
---@field public showingPanels System.Collections.Hashtable
---@field public panelCacheQueue System.Collections.ArrayList
---@field public seaShowPanel System.Collections.Generic.Queue1Coolape.CLPanelBase
---@field public isShowPanel System.Boolean
---@field public isHidePanel System.Boolean
---@field public seaHidePanel System.Collections.Generic.Queue1Coolape.CLPanelBase
---@field public panelRetainLayer System.Collections.Generic.Stack1Coolape.CLPanelBase
---@field public isShowTopPanel System.Boolean
---@field public topPanel Coolape.CLPanelBase
---@field public oldPanel Coolape.CLPanelBase
---@field public oldoldPanel Coolape.CLPanelBase
---@field public isShowPrePanel System.Boolean
---@field public depthOffset System.Int32
---@field public mainPanelName System.String
---@field public panelBuff System.Collections.Hashtable
---@field public panelAssetBundle System.Collections.Hashtable
---@field public isFinishStart System.Boolean
---@field public uiPanelRoot UnityEngine.Transform
---@field public panels4Retain Coolape.CLPanelBase
---@field public mask UnityEngine.GameObject

local m = { }
---public CLPanelManager .ctor()
---@return CLPanelManager
function m.New() end
---public Void onShowPanel(CLPanelBase panel)
---@param optional CLPanelBase panel
function m.onShowPanel(panel) end
---public Void onHidePanel(CLPanelBase panel)
---@param optional CLPanelBase panel
function m.onHidePanel(panel) end
---public Void showPanel(CLPanelBase panel)
---@param optional CLPanelBase panel
function m.showPanel(panel) end
---public Void hidePanel(CLPanelBase panel)
---@param optional CLPanelBase panel
function m.hidePanel(panel) end
---public Void showTopPanel(CLPanelBase panel)
---public Void showTopPanel(CLPanelBase panel, Boolean isRetainCurr, Boolean isShowCurr)
---@param CLPanelBase panel
---@param Boolean isRetainCurr
---@param optional Boolean isShowCurr
function m.showTopPanel(panel, isRetainCurr, isShowCurr) end
---public Void doShowTopPanel(CLPanelBase panel, Boolean isRetainCurr, Boolean isShowCurr, Boolean immed)
---@param optional CLPanelBase panel
---@param optional Boolean isRetainCurr
---@param optional Boolean isShowCurr
---@param optional Boolean immed
function m.doShowTopPanel(panel, isRetainCurr, isShowCurr, immed) end
---public Void hideTopPanel()
---public Void hideTopPanel(CLPanelBase panel)
---public Void hideTopPanel(CLPanelBase panel, Boolean showMain, Boolean immed)
---@param CLPanelBase panel
---@param Boolean showMain
---@param Boolean immed
function m.hideTopPanel(panel, showMain, immed) end
---public Void rmPanelRetainLayer(CLPanelBase panel)
---@param optional CLPanelBase panel
function m.rmPanelRetainLayer(panel) end
---public Void hideAllPanel()
function m.hideAllPanel() end
---public Void Update()
function m:Update() end
---public Void Start()
function m:Start() end
---public Void clean()
function m:clean() end
---public Void reset()
function m:reset() end
---public Void resetPanelLua()
function m.resetPanelLua() end
---public Void destoryAllPanel()
function m.destoryAllPanel() end
---public Void getPanelAsy(String pName, Object callback)
---public Void getPanelAsy(String pName, Object callback, Object paras)
---@param String pName
---@param optional Object callback
---@param optional Object paras
function m.getPanelAsy(pName, callback, paras) end
---public Void onGetPanelAssetBundle(Object[] args)
---@param optional Object[] args
function m.onGetPanelAssetBundle(args) end
---public Void finishGetPanel(String pName, AssetBundle ab, Object callback, Object paras)
---@param optional String pName
---@param optional AssetBundle ab
---@param optional Object callback
---@param optional Object paras
function m.finishGetPanel(pName, ab, callback, paras) end
---public CLPanelBase getPanel(String pName)
---@return CLPanelBase
---@param optional String pName
function m.getPanel(pName) end
---public Void destroyPanel(CLPanelBase p)
---public Void destroyPanel(CLPanelBase p, Boolean needCallHideFunc)
---@param CLPanelBase p
---@param optional Boolean needCallHideFunc
function m.destroyPanel(p, needCallHideFunc) end
Coolape.CLPanelManager = m
return m
