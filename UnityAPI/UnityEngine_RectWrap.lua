---@class UnityEngine.Rect : System.ValueType
---@field public zero UnityEngine.Rect
---@field public x System.Single
---@field public y System.Single
---@field public position UnityEngine.Vector2
---@field public center UnityEngine.Vector2
---@field public min UnityEngine.Vector2
---@field public max UnityEngine.Vector2
---@field public width System.Single
---@field public height System.Single
---@field public size UnityEngine.Vector2
---@field public xMin System.Single
---@field public yMin System.Single
---@field public xMax System.Single
---@field public yMax System.Single

local m = { }
---public Rect .ctor(Rect source)
---public Rect .ctor(Vector2 position, Vector2 size)
---public Rect .ctor(Single x, Single y, Single width, Single height)
---@return Rect
---@param Single x
---@param Single y
---@param Single width
---@param optional Single height
function m.New(x, y, width, height) end
---public Rect MinMaxRect(Single xmin, Single ymin, Single xmax, Single ymax)
---@return Rect
---@param optional Single xmin
---@param optional Single ymin
---@param optional Single xmax
---@param optional Single ymax
function m.MinMaxRect(xmin, ymin, xmax, ymax) end
---public Void Set(Single x, Single y, Single width, Single height)
---@param optional Single x
---@param optional Single y
---@param optional Single width
---@param optional Single height
function m:Set(x, y, width, height) end
---public Boolean Contains(Vector2 point)
---public Boolean Contains(Vector3 point)
---public Boolean Contains(Vector3 point, Boolean allowInverse)
---@return bool
---@param Vector3 point
---@param optional Boolean allowInverse
function m:Contains(point, allowInverse) end
---public Boolean Overlaps(Rect other)
---public Boolean Overlaps(Rect other, Boolean allowInverse)
---@return bool
---@param Rect other
---@param optional Boolean allowInverse
function m:Overlaps(other, allowInverse) end
---public Vector2 NormalizedToPoint(Rect rectangle, Vector2 normalizedRectCoordinates)
---@return Vector2
---@param optional Rect rectangle
---@param optional Vector2 normalizedRectCoordinates
function m.NormalizedToPoint(rectangle, normalizedRectCoordinates) end
---public Vector2 PointToNormalized(Rect rectangle, Vector2 point)
---@return Vector2
---@param optional Rect rectangle
---@param optional Vector2 point
function m.PointToNormalized(rectangle, point) end
---public Boolean op_Inequality(Rect lhs, Rect rhs)
---@return bool
---@param optional Rect lhs
---@param optional Rect rhs
function m.op_Inequality(lhs, rhs) end
---public Boolean op_Equality(Rect lhs, Rect rhs)
---@return bool
---@param optional Rect lhs
---@param optional Rect rhs
function m.op_Equality(lhs, rhs) end
---public Int32 GetHashCode()
---@return number
function m:GetHashCode() end
---public Boolean Equals(Object other)
---public Boolean Equals(Rect other)
---@return bool
---@param optional Rect other
function m:Equals(other) end
---public String ToString()
---public String ToString(String format)
---@return String
---@param String format
function m:ToString(format) end
UnityEngine.Rect = m
return m
