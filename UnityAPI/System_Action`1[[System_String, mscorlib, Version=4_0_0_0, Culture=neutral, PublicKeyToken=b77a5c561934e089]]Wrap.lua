---@class System.Action`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]] : System.MulticastDelegate

local m = { }
---public Action`1 .ctor(Object object, IntPtr method)
---@return Action`1
---@param optional Object object
---@param optional IntPtr method
function m.New(object, method) end
---public Void Invoke(String obj)
---@param optional String obj
function m:Invoke(obj) end
---public IAsyncResult BeginInvoke(String obj, AsyncCallback callback, Object object)
---@return IAsyncResult
---@param optional String obj
---@param optional AsyncCallback callback
---@param optional Object object
function m:BeginInvoke(obj, callback, object) end
---public Void EndInvoke(IAsyncResult result)
---@param optional IAsyncResult result
function m:EndInvoke(result) end
System.Action`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]] = m
return m
