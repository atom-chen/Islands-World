---@class UnityEngine.ScriptableObject : UnityEngine.Object

local m = { }
---public ScriptableObject .ctor()
---@return ScriptableObject
function m.New() end
---public ScriptableObject CreateInstance(Type t)
---public ScriptableObject CreateInstance(String className)
---@return ScriptableObject
---@param optional String className
function m.CreateInstance(className) end
UnityEngine.ScriptableObject = m
return m
