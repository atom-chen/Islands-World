---@class UnityEngine.Ray : System.ValueType
---@field public origin UnityEngine.Vector3
---@field public direction UnityEngine.Vector3
local m = { }
---public Ray .ctor(Vector3 origin, Vector3 direction)
---@return Ray
---@param optional Vector3 origin
---@param optional Vector3 direction
function m.New(origin, direction) end
---public Vector3 GetPoint(Single distance)
---@return Vector3
---@param optional Single distance
function m:GetPoint(distance) end
---public String ToString()
---public String ToString(String format)
---@return String
---@param String format
function m:ToString(format) end
UnityEngine.Ray = m
return m
