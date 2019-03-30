---@class UnityEngine.Ray2D : System.ValueType
---@field public origin UnityEngine.Vector2
---@field public direction UnityEngine.Vector2

local m = { }
---public Ray2D .ctor(Vector2 origin, Vector2 direction)
---@return Ray2D
---@param optional Vector2 origin
---@param optional Vector2 direction
function m.New(origin, direction) end
---public Vector2 GetPoint(Single distance)
---@return Vector2
---@param optional Single distance
function m:GetPoint(distance) end
---public String ToString()
---public String ToString(String format)
---@return String
---@param String format
function m:ToString(format) end
UnityEngine.Ray2D = m
return m
