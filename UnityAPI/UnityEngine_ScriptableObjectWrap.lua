---@class UnityEngine.ScriptableObject : UnityEngine.Object

local m = { }
---public ScriptableObject .ctor()
---@return ScriptableObject
function m.New() end
---public ScriptableObject CreateInstance(String className)
---public ScriptableObject CreateInstance(Type t)
---@return ScriptableObject
---@param optional Type t
function m.CreateInstance(type) end
UnityEngine.ScriptableObject = m
return m
