---@class Coolape.InvokeEx : UnityEngine.MonoBehaviour
---@field public canFixedUpdate System.Boolean
---@field public canUpdate System.Boolean
---@field public self Coolape.InvokeEx
---@field public frameCounter System.Int64
---@field public fixedInvokeMap System.Collections.Hashtable
local m = { }
---public InvokeEx .ctor()
---@return InvokeEx
function m.New() end
---public Coroutine invoke(Object callbakFunc, Single sec)
---public Coroutine invoke(Object callbakFunc, Object orgs, Single sec)
---public Coroutine invoke(Object callbakFunc, Object orgs, Single sec, Boolean onlyOneCoroutine)
---@return Coroutine
---@param Object callbakFunc
---@param Object orgs
---@param optional Single sec
---@param optional Boolean onlyOneCoroutine
function m.invoke(callbakFunc, orgs, sec, onlyOneCoroutine) end
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
---public Void cancelInvoke()
---public Void cancelInvoke(Object callbakFunc)
---@param Object callbakFunc
function m.cancelInvoke(callbakFunc) end
---public Void cancelInvoke4Lua()
---public Void cancelInvoke4Lua(Object callbakFunc)
---@param Object callbakFunc
function m:cancelInvoke4Lua(callbakFunc) end
---public Void invokeByFixedUpdate(Object luaFunc, Single waitSec)
---public Void invokeByFixedUpdate(Object luaFunc, Object paras, Single waitSec)
---@param Object luaFunc
---@param optional Object paras
---@param optional Single waitSec
function m.invokeByFixedUpdate(luaFunc, paras, waitSec) end
---public Void fixedInvoke4Lua(Object luaFunc, Single waitSec)
---public Void fixedInvoke4Lua(Object luaFunc, Object paras, Single waitSec)
---@param Object luaFunc
---@param optional Object paras
---@param optional Single waitSec
function m:fixedInvoke4Lua(luaFunc, paras, waitSec) end
---public Void cancelInvokeByFixedUpdate()
---public Void cancelInvokeByFixedUpdate(Object func)
---@param Object func
function m.cancelInvokeByFixedUpdate(func) end
---public Void cancelFixedInvoke4Lua()
---public Void cancelFixedInvoke4Lua(Object func)
---@param Object func
function m:cancelFixedInvoke4Lua(func) end
---public Void FixedUpdate()
function m:FixedUpdate() end
---public Void invokeByUpdate(Object callbakFunc, Single sec)
---public Void invokeByUpdate(Object callbakFunc, Object orgs, Single sec)
---@param Object callbakFunc
---@param optional Object orgs
---@param optional Single sec
function m.invokeByUpdate(callbakFunc, orgs, sec) end
---public Void updateInvoke(Object callbakFunc, Single sec)
---public Void updateInvoke(Object callbakFunc, Object orgs, Single sec)
---@param Object callbakFunc
---@param optional Object orgs
---@param optional Single sec
function m:updateInvoke(callbakFunc, orgs, sec) end
---public Void cancelInvokeByUpdate()
---public Void cancelInvokeByUpdate(Object callbakFunc)
---@param Object callbakFunc
function m.cancelInvokeByUpdate(callbakFunc) end
---public Void cancelUpdateInvoke()
---public Void cancelUpdateInvoke(Object callbakFunc)
---@param Object callbakFunc
function m:cancelUpdateInvoke(callbakFunc) end
---public Void Update()
function m:Update() end
Coolape.InvokeEx = m
return m
