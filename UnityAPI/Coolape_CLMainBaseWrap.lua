---@class Coolape.CLMainBase : Coolape.CLBehaviourWithUpdate4Lua
---@field public self Coolape.CLMainBase
---@field public firstPanel System.String
local m = { }
---public CLMainBase .ctor()
---@return CLMainBase
function m.New() end
---public Void Start()
function m:Start() end
---public IEnumerator gameInit()
---@return IEnumerator
function m:gameInit() end
---public Void setLanguage(String language)
---@param optional String language
function m:setLanguage(language) end
---public Void init()
function m:init() end
---public Void onGetStreamingAssets(Object[] para)
---@param optional Object[] para
function m:onGetStreamingAssets(para) end
---public Void reStart()
function m:reStart() end
---public Void doRestart()
function m:doRestart() end
---public Void OnDestroy()
function m:OnDestroy() end
---public Void OnApplicationQuit()
function m:OnApplicationQuit() end
---public Void setLua()
function m:setLua() end
---public Void initGetLuaFunc()
function m:initGetLuaFunc() end
---public Void Update()
function m:Update() end
---public Void onOffline()
function m:onOffline() end
---public Void doOffline()
function m:doOffline() end
Coolape.CLMainBase = m
return m
