---@class UnityEngine.Bounds : System.ValueType
---@field public center UnityEngine.Vector3
---@field public size UnityEngine.Vector3
---@field public extents UnityEngine.Vector3
---@field public min UnityEngine.Vector3
---@field public max UnityEngine.Vector3

local m = { }
---public Bounds .ctor(Vector3 center, Vector3 size)
---@return Bounds
---@param optional Vector3 center
---@param optional Vector3 size
function m.New(center, size) end
---public Int32 GetHashCode()
---@return number
function m:GetHashCode() end
---public Boolean Equals(Object other)
---public Boolean Equals(Bounds other)
---@return bool
---@param optional Bounds other
function m:Equals(other) end
---public Boolean op_Equality(Bounds lhs, Bounds rhs)
---@return bool
---@param optional Bounds lhs
---@param optional Bounds rhs
function m.op_Equality(lhs, rhs) end
---public Boolean op_Inequality(Bounds lhs, Bounds rhs)
---@return bool
---@param optional Bounds lhs
---@param optional Bounds rhs
function m.op_Inequality(lhs, rhs) end
---public Void SetMinMax(Vector3 min, Vector3 max)
---@param optional Vector3 min
---@param optional Vector3 max
function m:SetMinMax(min, max) end
---public Void Encapsulate(Vector3 point)
---public Void Encapsulate(Bounds bounds)
---@param optional Bounds bounds
function m:Encapsulate(bounds) end
---public Void Expand(Single amount)
---public Void Expand(Vector3 amount)
---@param optional Vector3 amount
function m:Expand(amount) end
---public Boolean Intersects(Bounds bounds)
---@return bool
---@param optional Bounds bounds
function m:Intersects(bounds) end
---public Boolean IntersectRay(Ray ray)
---public Boolean IntersectRay(Ray ray, Single& distance)
---@return bool
---@param Ray ray
---@param optional Single& distance
function m:IntersectRay(ray, distance) end
---public String ToString()
---public String ToString(String format)
---@return String
---@param String format
function m:ToString(format) end
---public Boolean Contains(Vector3 point)
---@return bool
---@param optional Vector3 point
function m:Contains(point) end
---public Single SqrDistance(Vector3 point)
---@return number
---@param optional Vector3 point
function m:SqrDistance(point) end
---public Vector3 ClosestPoint(Vector3 point)
---@return Vector3
---@param optional Vector3 point
function m:ClosestPoint(point) end
UnityEngine.Bounds = m
return m
