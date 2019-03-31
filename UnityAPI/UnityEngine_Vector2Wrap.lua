---@class UnityEngine.Vector2 : System.ValueType
---@field public x System.Single
---@field public y System.Single
---@field public kEpsilon System.Single
---@field public kEpsilonNormalSqrt System.Single
---@field public Item System.Single
---@field public normalized UnityEngine.Vector2
---@field public magnitude System.Single
---@field public sqrMagnitude System.Single
---@field public zero UnityEngine.Vector2
---@field public one UnityEngine.Vector2
---@field public up UnityEngine.Vector2
---@field public down UnityEngine.Vector2
---@field public left UnityEngine.Vector2
---@field public right UnityEngine.Vector2
---@field public positiveInfinity UnityEngine.Vector2
---@field public negativeInfinity UnityEngine.Vector2

local m = { }
---public Vector2 .ctor(Single x, Single y)
---@return Vector2
---@param optional Single x
---@param optional Single y
function m.New(x, y) end
---public Void Set(Single newX, Single newY)
---@param optional Single newX
---@param optional Single newY
function m:Set(newX, newY) end
---public Vector2 Lerp(Vector2 a, Vector2 b, Single t)
---@return Vector2
---@param optional Vector2 a
---@param optional Vector2 b
---@param optional Single t
function m.Lerp(a, b, t) end
---public Vector2 LerpUnclamped(Vector2 a, Vector2 b, Single t)
---@return Vector2
---@param optional Vector2 a
---@param optional Vector2 b
---@param optional Single t
function m.LerpUnclamped(a, b, t) end
---public Vector2 MoveTowards(Vector2 current, Vector2 target, Single maxDistanceDelta)
---@return Vector2
---@param optional Vector2 current
---@param optional Vector2 target
---@param optional Single maxDistanceDelta
function m.MoveTowards(current, target, maxDistanceDelta) end
---public Void Scale(Vector2 scale)
---public Vector2 Scale(Vector2 a, Vector2 b)
---@param Vector2 a
---@param optional Vector2 b
function m:Scale(a, b) end
---public Void Normalize()
function m:Normalize() end
---public String ToString()
---public String ToString(String format)
---@return String
---@param String format
function m:ToString(format) end
---public Int32 GetHashCode()
---@return number
function m:GetHashCode() end
---public Boolean Equals(Object other)
---public Boolean Equals(Vector2 other)
---@return bool
---@param optional Vector2 other
function m:Equals(other) end
---public Vector2 Reflect(Vector2 inDirection, Vector2 inNormal)
---@return Vector2
---@param optional Vector2 inDirection
---@param optional Vector2 inNormal
function m.Reflect(inDirection, inNormal) end
---public Vector2 Perpendicular(Vector2 inDirection)
---@return Vector2
---@param optional Vector2 inDirection
function m.Perpendicular(inDirection) end
---public Single Dot(Vector2 lhs, Vector2 rhs)
---@return number
---@param optional Vector2 lhs
---@param optional Vector2 rhs
function m.Dot(lhs, rhs) end
---public Single Angle(Vector2 from, Vector2 to)
---@return number
---@param optional Vector2 from
---@param optional Vector2 to
function m.Angle(from, to) end
---public Single SignedAngle(Vector2 from, Vector2 to)
---@return number
---@param optional Vector2 from
---@param optional Vector2 to
function m.SignedAngle(from, to) end
---public Single Distance(Vector2 a, Vector2 b)
---@return number
---@param optional Vector2 a
---@param optional Vector2 b
function m.Distance(a, b) end
---public Vector2 ClampMagnitude(Vector2 vector, Single maxLength)
---@return Vector2
---@param optional Vector2 vector
---@param optional Single maxLength
function m.ClampMagnitude(vector, maxLength) end
---public Single SqrMagnitude()
---public Single SqrMagnitude(Vector2 a)
---@return number
---@param Vector2 a
function m:SqrMagnitude(a) end
---public Vector2 Min(Vector2 lhs, Vector2 rhs)
---@return Vector2
---@param optional Vector2 lhs
---@param optional Vector2 rhs
function m.Min(lhs, rhs) end
---public Vector2 Max(Vector2 lhs, Vector2 rhs)
---@return Vector2
---@param optional Vector2 lhs
---@param optional Vector2 rhs
function m.Max(lhs, rhs) end
---public Vector2 SmoothDamp(Vector2 current, Vector2 target, Vector2& currentVelocity, Single smoothTime)
---public Vector2 SmoothDamp(Vector2 current, Vector2 target, Vector2& currentVelocity, Single smoothTime, Single maxSpeed)
---public Vector2 SmoothDamp(Vector2 current, Vector2 target, Vector2& currentVelocity, Single smoothTime, Single maxSpeed, Single deltaTime)
---@return Vector2
---@param Vector2 current
---@param Vector2 target
---@param optional Vector2& currentVelocity
---@param optional Single smoothTime
---@param optional Single maxSpeed
---@param optional Single deltaTime
function m.SmoothDamp(current, target, currentVelocity, smoothTime, maxSpeed, deltaTime) end
---public Vector2 op_Addition(Vector2 a, Vector2 b)
---@return Vector2
---@param optional Vector2 a
---@param optional Vector2 b
function m.op_Addition(a, b) end
---public Vector2 op_Subtraction(Vector2 a, Vector2 b)
---@return Vector2
---@param optional Vector2 a
---@param optional Vector2 b
function m.op_Subtraction(a, b) end
---public Vector2 op_Multiply(Vector2 a, Vector2 b)
---public Vector2 op_Multiply(Vector2 a, Single d)
---public Vector2 op_Multiply(Single d, Vector2 a)
---@return Vector2
---@param optional Single d
---@param optional Vector2 a
function m.op_Multiply(d, a) end
---public Vector2 op_Division(Vector2 a, Vector2 b)
---public Vector2 op_Division(Vector2 a, Single d)
---@return Vector2
---@param optional Vector2 a
---@param optional Single d
function m.op_Division(a, d) end
---public Vector2 op_UnaryNegation(Vector2 a)
---@return Vector2
---@param optional Vector2 a
function m.op_UnaryNegation(a) end
---public Boolean op_Equality(Vector2 lhs, Vector2 rhs)
---@return bool
---@param optional Vector2 lhs
---@param optional Vector2 rhs
function m.op_Equality(lhs, rhs) end
---public Boolean op_Inequality(Vector2 lhs, Vector2 rhs)
---@return bool
---@param optional Vector2 lhs
---@param optional Vector2 rhs
function m.op_Inequality(lhs, rhs) end
---public Vector2 op_Implicit(Vector3 v)
---public Vector3 op_Implicit(Vector2 v)
---@return Vector2
---@param optional Vector2 v
function m.op_Implicit(v) end
UnityEngine.Vector2 = m
return m
