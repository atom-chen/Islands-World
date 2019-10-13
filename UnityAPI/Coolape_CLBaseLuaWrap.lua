---@class Coolape.CLBaseLua : UnityEngine.MonoBehaviour
---@field public isPause System.Boolean
---@field public luaPath System.String
---@field public lua XLua.LuaEnv
---@field public luaFuncMap System.Collections.Generic.Dictionary2System.StringXLua.LuaFunction
---@field public mainLua XLua.LuaEnv
---@field public luaTable XLua.LuaTable
---@field public isClassLua System.Boolean
---@field public transform UnityEngine.Transform
local m = { }
---public CLBaseLua .ctor()
---@return CLBaseLua
function m.New() end
---public Void resetMainLua()
function m:resetMainLua() end
---public Void setLua()
function m:setLua() end
---public Object[] doSetLua(Boolean Independent)
---@return table
---@param optional Boolean Independent
function m:doSetLua(Independent) end
---public Void onNotifyLua(GameObject gameObj, String funcName, Object paras)
---@param optional GameObject gameObj
---@param optional String funcName
---@param optional Object paras
function m:onNotifyLua(gameObj, funcName, paras) end
---public LuaFunction getLuaFunction(String funcName)
---@return function
---@param optional String funcName
function m:getLuaFunction(funcName) end
---public Object getLuaVar(String name)
---@return Object
---@param optional String name
function m:getLuaVar(name) end
---public Coroutine invoke4Lua(Object callbakFunc, Single sec)
---public Coroutine invoke4Lua(Object callbakFunc, Object orgs, Single sec)
---public Coroutine invoke4Lua(Object callbakFunc, Object orgs, Single sec, Boolean onlyOneCoroutine)
---@return Coroutine
---@param Object callbakFunc
---@param Object orgs
---@param optional Single sec
---@param optional Boolean onlyOneCoroutine
function m:invoke4Lua(callbakFunc, orgs, sec, onlyOneCoroutine) end
---public Int32 getCoroutineIndex(Object callbakFunc)
---@return number
---@param optional Object callbakFunc
function m:getCoroutineIndex(callbakFunc) end
---public Void setCoroutineIndex(Object callbakFunc, Int32 val)
---@param optional Object callbakFunc
---@param optional Int32 val
function m:setCoroutineIndex(callbakFunc, val) end
---public Object getKey4InvokeMap(Object callbakFunc, Hashtable map)
---@return Object
---@param optional Object callbakFunc
---@param optional Hashtable map
function m:getKey4InvokeMap(callbakFunc, map) end
---public Hashtable getCoroutines(Object callbakFunc)
---@return Hashtable
---@param optional Object callbakFunc
function m:getCoroutines(callbakFunc) end
---public Void setCoroutine(Object callbakFunc, Coroutine ct, Int32 index)
---@param optional Object callbakFunc
---@param optional Coroutine ct
---@param optional Int32 index
function m:setCoroutine(callbakFunc, ct, index) end
---public Void cleanCoroutines(Object callbakFunc)
---@param optional Object callbakFunc
function m:cleanCoroutines(callbakFunc) end
---public Void rmCoroutine(Object callbakFunc, Int32 index)
---@param optional Object callbakFunc
---@param optional Int32 index
function m:rmCoroutine(callbakFunc, index) end
---public Void cancelInvoke4Lua()
---public Void cancelInvoke4Lua(Object callbakFunc)
---@param Object callbakFunc
function m:cancelInvoke4Lua(callbakFunc) end
---public Void pause()
function m:pause() end
---public Void regain()
function m:regain() end
---public Void OnDestroy()
function m:OnDestroy() end
---public Void destoryLua()
function m:destoryLua() end
Coolape.CLBaseLua = m
return m
