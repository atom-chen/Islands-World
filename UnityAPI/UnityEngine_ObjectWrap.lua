---@class UnityEngine.Object
---@field public name System.String
---@field public hideFlags UnityEngine.HideFlags

local m = { }
---public Object .ctor()
---@return Object
function m.New() end
---public Int32 GetInstanceID()
---@return number
function m:GetInstanceID() end
---public Int32 GetHashCode()
---@return number
function m:GetHashCode() end
---public Boolean Equals(Object other)
---@return bool
---@param optional Object other
function m:Equals(other) end
---public Boolean op_Implicit(Object exists)
---@return bool
---@param optional Object exists
function m.op_Implicit(exists) end
---public Object Instantiate(Object original)
---public Object Instantiate(Object original, Transform parent)
---public Object Instantiate(Object original, Transform parent, Boolean instantiateInWorldSpace)
---public Object Instantiate(Object original, Vector3 position, Quaternion rotation)
---public Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent)
---@return Object
---@param Object original
---@param Vector3 position
---@param Quaternion rotation
---@param optional Transform parent
function m.Instantiate(original, position, rotation, parent) end
---public Void Destroy(Object obj)
---public Void Destroy(Object obj, Single t)
---@param Object obj
---@param optional Single t
function m.Destroy(obj, t) end
---public Void DestroyImmediate(Object obj)
---public Void DestroyImmediate(Object obj, Boolean allowDestroyingAssets)
---@param Object obj
---@param optional Boolean allowDestroyingAssets
function m.DestroyImmediate(obj, allowDestroyingAssets) end
---public Object[] FindObjectsOfType(Type t)
---@return table
---@param optional Type t
function m.FindObjectsOfType(type) end
---public Void DontDestroyOnLoad(Object target)
---@param optional Object target
function m.DontDestroyOnLoad(target) end
---public Object FindObjectOfType(Type t)
---@return Object
---@param optional Type t
function m.FindObjectOfType(type) end
---public String ToString()
---@return String
function m:ToString() end
---public Boolean op_Equality(Object x, Object y)
---@return bool
---@param optional Object x
---@param optional Object y
function m.op_Equality(x, y) end
---public Boolean op_Inequality(Object x, Object y)
---@return bool
---@param optional Object x
---@param optional Object y
function m.op_Inequality(x, y) end
UnityEngine.Object = m
return m
