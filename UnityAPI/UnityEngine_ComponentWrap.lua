---@class UnityEngine.Component : UnityEngine.Object
---@field public transform UnityEngine.Transform
---@field public gameObject UnityEngine.GameObject
---@field public tag System.String

local m = { }
---public Component .ctor()
---@return Component
function m.New() end
---public Component GetComponent(String t)
---public Component GetComponent(Type t)
---@return Component
---@param optional Type t
function m:GetComponent(type) end
---public Component GetComponentInChildren(Type t)
---public Component GetComponentInChildren(Type t, Boolean includeInactive)
---@return Component
---@param Type t
---@param optional Boolean includeInactive
function m:GetComponentInChildren(t, includeInactive) end
---public Component[] GetComponentsInChildren(Type t)
---public Component[] GetComponentsInChildren(Type t, Boolean includeInactive)
---@return table
---@param Type t
---@param optional Boolean includeInactive
function m:GetComponentsInChildren(t, includeInactive) end
---public Component GetComponentInParent(Type t)
---@return Component
---@param optional Type t
function m:GetComponentInParent(t) end
---public Component[] GetComponentsInParent(Type t)
---public Component[] GetComponentsInParent(Type t, Boolean includeInactive)
---@return table
---@param Type t
---@param optional Boolean includeInactive
function m:GetComponentsInParent(t, includeInactive) end
---public Component[] GetComponents(Type t)
---public Void GetComponents(Type t, List`1 results)
---@return table
---@param Type t
---@param optional List`1 results
function m:GetComponents(type, results) end
---public Boolean CompareTag(String tag)
---@return bool
---@param optional String tag
function m:CompareTag(tag) end
---public Void SendMessageUpwards(String methodName)
---public Void SendMessageUpwards(String methodName, SendMessageOptions options)
---public Void SendMessageUpwards(String methodName, Object value)
---public Void SendMessageUpwards(String methodName, Object value, SendMessageOptions options)
---@param String methodName
---@param Object value
---@param optional SendMessageOptions options
function m:SendMessageUpwards(methodName, value, options) end
---public Void SendMessage(String methodName)
---public Void SendMessage(String methodName, SendMessageOptions options)
---public Void SendMessage(String methodName, Object value)
---public Void SendMessage(String methodName, Object value, SendMessageOptions options)
---@param String methodName
---@param Object value
---@param optional SendMessageOptions options
function m:SendMessage(methodName, value, options) end
---public Void BroadcastMessage(String methodName)
---public Void BroadcastMessage(String methodName, SendMessageOptions options)
---public Void BroadcastMessage(String methodName, Object parameter)
---public Void BroadcastMessage(String methodName, Object parameter, SendMessageOptions options)
---@param String methodName
---@param Object parameter
---@param optional SendMessageOptions options
function m:BroadcastMessage(methodName, parameter, options) end
UnityEngine.Component = m
return m
