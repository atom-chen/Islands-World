---@class System.Object

local m = { }
---public Object .ctor()
---@return Object
function m.New() end
---public Boolean Equals(Object obj)
---public Boolean Equals(Object objA, Object objB)
---@return bool
---@param Object objA
---@param optional Object objB
function m:Equals(objA, objB) end
---public Int32 GetHashCode()
---@return number
function m:GetHashCode() end
---public Type GetType()
---@return string
function m:GetType() end
---public String ToString()
---@return String
function m:ToString() end
---public Boolean ReferenceEquals(Object objA, Object objB)
---@return bool
---@param optional Object objA
---@param optional Object objB
function m.ReferenceEquals(objA, objB) end
System.Object = m
return m
