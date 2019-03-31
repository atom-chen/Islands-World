---@class UnityEngine.Vector4 : System.ValueType
---@field public kEpsilon System.Single
---@field public x System.Single
---@field public y System.Single
---@field public z System.Single
---@field public w System.Single
---@field public Item System.Single
---@field public normalized UnityEngine.Vector4
---@field public magnitude System.Single
---@field public sqrMagnitude System.Single
---@field public zero UnityEngine.Vector4
---@field public one UnityEngine.Vector4
---@field public positiveInfinity UnityEngine.Vector4
---@field public negativeInfinity UnityEngine.Vector4

local m = { }
---public Vector4 .ctor(Single x, Single y)
---public Vector4 .ctor(Single x, Single y, Single z)
---public Vector4 .ctor(Single x, Single y, Single z, Single w)
---@return Vector4
---@param Single x
---@param Single y
---@param optional Single z
---@param optional Single w
function m.New(x, y, z, w) end
---public Void Set(Single newX, Single newY, Single newZ, Single newW)
---@param optional Single newX
---@param optional Single newY
---@param optional Single newZ
---@param optional Single newW
function m:Set(newX, newY, newZ, newW) end
---public Vector4 Lerp(Vector4 a, Vector4 b, Single t)
---@return Vector4
---@param optional Vector4 a
---@param optional Vector4 b
---@param optional Single t
function m.Lerp(a, b, t) end
---public Vector4 LerpUnclamped(Vector4 a, Vector4 b, Single t)
---@return Vector4
---@param optional Vector4 a
---@param optional Vector4 b
---@param optional Single t
function m.LerpUnclamped(a, b, t) end
---public Vector4 MoveTowards(Vector4 current, Vector4 target, Single maxDistanceDelta)
---@return Vector4
---@param optional Vector4 current
---@param optional Vector4 target
---@param optional Single maxDistanceDelta
function m.MoveTowards(current, target, maxDistanceDelta) end
---public Void Scale(Vector4 scale)
---public Vector4 Scale(Vector4 a, Vector4 b)
---@param Vector4 a
---@param optional Vector4 b
function m:Scale(a, b) end
---public Int32 GetHashCode()
---@return number
function m:GetHashCode() end
---public Boolean Equals(Object other)
---public Boolean Equals(Vector4 other)
---@return bool
---@param optional Vector4 other
function m:Equals(other) end
---public Void Normalize()
---public Vector4 Normalize(Vector4 a)
---@param Vector4 a
function m:Normalize(a) end
---public Single Dot(Vector4 a, Vector4 b)
---@return number
---@param optional Vector4 a
---@param optional Vector4 b
function m.Dot(a, b) end
---public Vector4 Project(Vector4 a, Vector4 b)
---@return Vector4
---@param optional Vector4 a
---@param optional Vector4 b
function m.Project(a, b) end
---public Single Distance(Vector4 a, Vector4 b)
---@return number
---@param optional Vector4 a
---@param optional Vector4 b
function m.Distance(a, b) end
---public Single Magnitude(Vector4 a)
---@return number
---@param optional Vector4 a
function m.Magnitude(a) end
---public Vector4 Min(Vector4 lhs, Vector4 rhs)
---@return Vector4
---@param optional Vector4 lhs
---@param optional Vector4 rhs
function m.Min(lhs, rhs) end
---public Vector4 Max(Vector4 lhs, Vector4 rhs)
---@return Vector4
---@param optional Vector4 lhs
---@param optional Vector4 rhs
function m.Max(lhs, rhs) end
---public Vector4 op_Addition(Vector4 a, Vector4 b)
---@return Vector4
---@param optional Vector4 a
---@param optional Vector4 b
function m.op_Addition(a, b) end
---public Vector4 op_Subtraction(Vector4 a, Vector4 b)
---@return Vector4
---@param optional Vector4 a
---@param optional Vector4 b
function m.op_Subtraction(a, b) end
---public Vector4 op_UnaryNegation(Vector4 a)
---@return Vector4
---@param optional Vector4 a
function m.op_UnaryNegation(a) end
---public Vector4 op_Multiply(Vector4 a, Single d)
---public Vector4 op_Multiply(Single d, Vector4 a)
---@return Vector4
---@param optional Single d
---@param optional Vector4 a
function m.op_Multiply(d, a) end
---public Vector4 op_Division(Vector4 a, Single d)
---@return Vector4
---@param optional Vector4 a
---@param optional Single d
function m.op_Division(a, d) end
---public Boolean op_Equality(Vector4 lhs, Vector4 rhs)
---@return bool
---@param optional Vector4 lhs
---@param optional Vector4 rhs
function m.op_Equality(lhs, rhs) end
---public Boolean op_Inequality(Vector4 lhs, Vector4 rhs)
---@return bool
---@param optional Vector4 lhs
---@param optional Vector4 rhs
function m.op_Inequality(lhs, rhs) end
---public Vector4 op_Implicit(Vector3 v)
---public Vector3 op_Implicit(Vector4 v)
---public Vector4 op_Implicit(Vector2 v)
---public Vector2 op_Implicit(Vector4 v)
---@return Vector4
---@param optional Vector4 v
function m.op_Implicit(v) end
---public String ToString()
---public String ToString(String format)
---@return String
---@param String format
function m:ToString(format) end
---public Single SqrMagnitude()
---public Single SqrMagnitude(Vector4 a)
---@return number
---@param Vector4 a
function m:SqrMagnitude(a) end
UnityEngine.Vector4 = m
return m
