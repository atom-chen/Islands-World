---@class UnityEngine.GameObject : UnityEngine.Object
---@field public transform UnityEngine.Transform
---@field public layer System.Int32
---@field public activeSelf System.Boolean
---@field public activeInHierarchy System.Boolean
---@field public isStatic System.Boolean
---@field public tag System.String
---@field public scene UnityEngine.SceneManagement.Scene
---@field public gameObject UnityEngine.GameObject

local m = { }
---public GameObject .ctor()
---public GameObject .ctor(String name)
---public GameObject .ctor(String name, Type[] components)
---@return GameObject
---@param String name
---@param Type[] components
function m.New(name, components) end
---public GameObject CreatePrimitive(PrimitiveType t)
---@return GameObject
---@param optional PrimitiveType t
function m.CreatePrimitive(type) end
---public Component GetComponent(Type t)
---public Component GetComponent(String t)
---@return Component
---@param optional String t
function m:GetComponent(type) end
---public Component GetComponentInChildren(Type t)
---public Component GetComponentInChildren(Type t, Boolean includeInactive)
---@return Component
---@param Type t
---@param optional Boolean includeInactive
function m:GetComponentInChildren(type, includeInactive) end
---public Component GetComponentInParent(Type t)
---@return Component
---@param optional Type t
function m:GetComponentInParent(type) end
---public Component[] GetComponents(Type t)
---public Void GetComponents(Type t, List`1 results)
---@return table
---@param Type t
---@param optional List`1 results
function m:GetComponents(type, results) end
---public Component[] GetComponentsInChildren(Type t)
---public Component[] GetComponentsInChildren(Type t, Boolean includeInactive)
---@return table
---@param Type t
---@param optional Boolean includeInactive
function m:GetComponentsInChildren(type, includeInactive) end
---public Component[] GetComponentsInParent(Type t)
---public Component[] GetComponentsInParent(Type t, Boolean includeInactive)
---@return table
---@param Type t
---@param optional Boolean includeInactive
function m:GetComponentsInParent(type, includeInactive) end
---public GameObject FindWithTag(String tag)
---@return GameObject
---@param optional String tag
function m.FindWithTag(tag) end
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
---public Component AddComponent(Type componentType)
---@return Component
---@param optional Type componentType
function m:AddComponent(componentType) end
---public Void SetActive(Boolean value)
---@param optional Boolean value
function m:SetActive(value) end
---public Boolean CompareTag(String tag)
---@return bool
---@param optional String tag
function m:CompareTag(tag) end
---public GameObject FindGameObjectWithTag(String tag)
---@return GameObject
---@param optional String tag
function m.FindGameObjectWithTag(tag) end
---public GameObject[] FindGameObjectsWithTag(String tag)
---@return table
---@param optional String tag
function m.FindGameObjectsWithTag(tag) end
---public GameObject Find(String name)
---@return GameObject
---@param optional String name
function m.Find(name) end
UnityEngine.GameObject = m
return m
