---@class UnityEngine.Vector3 : System.ValueType
---@field public kEpsilon System.Single
---@field public kEpsilonNormalSqrt System.Single
---@field public x System.Single
---@field public y System.Single
---@field public z System.Single
---@field public Item System.Single
---@field public normalized UnityEngine.Vector3
---@field public magnitude System.Single
---@field public sqrMagnitude System.Single
---@field public zero UnityEngine.Vector3
---@field public one UnityEngine.Vector3
---@field public forward UnityEngine.Vector3
---@field public back UnityEngine.Vector3
---@field public up UnityEngine.Vector3
---@field public down UnityEngine.Vector3
---@field public left UnityEngine.Vector3
---@field public right UnityEngine.Vector3
---@field public positiveInfinity UnityEngine.Vector3
---@field public negativeInfinity UnityEngine.Vector3
local m = { }
---public Vector3 .ctor(Single x, Single y)
---public Vector3 .ctor(Single x, Single y, Single z)
---@return Vector3
---@param Single x
---@param optional Single y
---@param optional Single z
function m.New(x, y, z) end
---public Vector3 Slerp(Vector3 a, Vector3 b, Single t)
---@return Vector3
---@param optional Vector3 a
---@param optional Vector3 b
---@param optional Single t
function m.Slerp(a, b, t) end
---public Vector3 SlerpUnclamped(Vector3 a, Vector3 b, Single t)
---@return Vector3
---@param optional Vector3 a
---@param optional Vector3 b
---@param optional Single t
function m.SlerpUnclamped(a, b, t) end
---public Void OrthoNormalize(Vector3& normal, Vector3& tangent)
---public Void OrthoNormalize(Vector3& normal, Vector3& tangent, Vector3& binormal)
---@param Vector3& normal
---@param optional Vector3& tangent
---@param optional Vector3& binormal
function m.OrthoNormalize(normal, tangent, binormal) end
---public Vector3 RotateTowards(Vector3 current, Vector3 target, Single maxRadiansDelta, Single maxMagnitudeDelta)
---@return Vector3
---@param optional Vector3 current
---@param optional Vector3 target
---@param optional Single maxRadiansDelta
---@param optional Single maxMagnitudeDelta
function m.RotateTowards(current, target, maxRadiansDelta, maxMagnitudeDelta) end
---public Vector3 Lerp(Vector3 a, Vector3 b, Single t)
---@return Vector3
---@param optional Vector3 a
---@param optional Vector3 b
---@param optional Single t
function m.Lerp(a, b, t) end
---public Vector3 LerpUnclamped(Vector3 a, Vector3 b, Single t)
---@return Vector3
---@param optional Vector3 a
---@param optional Vector3 b
---@param optional Single t
function m.LerpUnclamped(a, b, t) end
---public Vector3 MoveTowards(Vector3 current, Vector3 target, Single maxDistanceDelta)
---@return Vector3
---@param optional Vector3 current
---@param optional Vector3 target
---@param optional Single maxDistanceDelta
function m.MoveTowards(current, target, maxDistanceDelta) end
---public Vector3 SmoothDamp(Vector3 current, Vector3 target, Vector3& currentVelocity, Single smoothTime)
---public Vector3 SmoothDamp(Vector3 current, Vector3 target, Vector3& currentVelocity, Single smoothTime, Single maxSpeed)
---public Vector3 SmoothDamp(Vector3 current, Vector3 target, Vector3& currentVelocity, Single smoothTime, Single maxSpeed, Single deltaTime)
---@return Vector3
---@param Vector3 current
---@param Vector3 target
---@param optional Vector3& currentVelocity
---@param optional Single smoothTime
---@param optional Single maxSpeed
---@param optional Single deltaTime
function m.SmoothDamp(current, target, currentVelocity, smoothTime, maxSpeed, deltaTime) end
---public Void Set(Single newX, Single newY, Single newZ)
---@param optional Single newX
---@param optional Single newY
---@param optional Single newZ
function m:Set(newX, newY, newZ) end
---public Void Scale(Vector3 scale)
---public Vector3 Scale(Vector3 a, Vector3 b)
---@param Vector3 a
---@param optional Vector3 b
function m:Scale(a, b) end
---public Vector3 Cross(Vector3 lhs, Vector3 rhs)
---@return Vector3
---@param optional Vector3 lhs
---@param optional Vector3 rhs
function m.Cross(lhs, rhs) end
---public Int32 GetHashCode()
---@return number
function m:GetHashCode() end
---public Boolean Equals(Object other)
---public Boolean Equals(Vector3 other)
---@return bool
---@param optional Vector3 other
function m:Equals(other) end
---public Vector3 Reflect(Vector3 inDirection, Vector3 inNormal)
---@return Vector3
---@param optional Vector3 inDirection
---@param optional Vector3 inNormal
function m.Reflect(inDirection, inNormal) end
---public Void Normalize()
---public Vector3 Normalize(Vector3 value)
---@param Vector3 value
function m:Normalize(value) end
---public Single Dot(Vector3 lhs, Vector3 rhs)
---@return number
---@param optional Vector3 lhs
---@param optional Vector3 rhs
function m.Dot(lhs, rhs) end
---public Vector3 Project(Vector3 vector, Vector3 onNormal)
---@return Vector3
---@param optional Vector3 vector
---@param optional Vector3 onNormal
function m.Project(vector, onNormal) end
---public Vector3 ProjectOnPlane(Vector3 vector, Vector3 planeNormal)
---@return Vector3
---@param optional Vector3 vector
---@param optional Vector3 planeNormal
function m.ProjectOnPlane(vector, planeNormal) end
---public Single Angle(Vector3 from, Vector3 to)
---@return number
---@param optional Vector3 from
---@param optional Vector3 to
function m.Angle(from, to) end
---public Single SignedAngle(Vector3 from, Vector3 to, Vector3 axis)
---@return number
---@param optional Vector3 from
---@param optional Vector3 to
---@param optional Vector3 axis
function m.SignedAngle(from, to, axis) end
---public Single Distance(Vector3 a, Vector3 b)
---@return number
---@param optional Vector3 a
---@param optional Vector3 b
function m.Distance(a, b) end
---public Vector3 ClampMagnitude(Vector3 vector, Single maxLength)
---@return Vector3
---@param optional Vector3 vector
---@param optional Single maxLength
function m.ClampMagnitude(vector, maxLength) end
---public Single Magnitude(Vector3 vector)
---@return number
---@param optional Vector3 vector
function m.Magnitude(vector) end
---public Single SqrMagnitude(Vector3 vector)
---@return number
---@param optional Vector3 vector
function m.SqrMagnitude(vector) end
---public Vector3 Min(Vector3 lhs, Vector3 rhs)
---@return Vector3
---@param optional Vector3 lhs
---@param optional Vector3 rhs
function m.Min(lhs, rhs) end
---public Vector3 Max(Vector3 lhs, Vector3 rhs)
---@return Vector3
---@param optional Vector3 lhs
---@param optional Vector3 rhs
function m.Max(lhs, rhs) end
---public Vector3 op_Addition(Vector3 a, Vector3 b)
---@return Vector3
---@param optional Vector3 a
---@param optional Vector3 b
function m.op_Addition(a, b) end
---public Vector3 op_Subtraction(Vector3 a, Vector3 b)
---@return Vector3
---@param optional Vector3 a
---@param optional Vector3 b
function m.op_Subtraction(a, b) end
---public Vector3 op_UnaryNegation(Vector3 a)
---@return Vector3
---@param optional Vector3 a
function m.op_UnaryNegation(a) end
---public Vector3 op_Multiply(Vector3 a, Single d)
---public Vector3 op_Multiply(Single d, Vector3 a)
---@return Vector3
---@param optional Single d
---@param optional Vector3 a
function m.op_Multiply(d, a) end
---public Vector3 op_Division(Vector3 a, Single d)
---@return Vector3
---@param optional Vector3 a
---@param optional Single d
function m.op_Division(a, d) end
---public Boolean op_Equality(Vector3 lhs, Vector3 rhs)
---@return bool
---@param optional Vector3 lhs
---@param optional Vector3 rhs
function m.op_Equality(lhs, rhs) end
---public Boolean op_Inequality(Vector3 lhs, Vector3 rhs)
---@return bool
---@param optional Vector3 lhs
---@param optional Vector3 rhs
function m.op_Inequality(lhs, rhs) end
---public String ToString()
---public String ToString(String format)
---@return String
---@param String format
function m:ToString(format) end
UnityEngine.Vector3 = m
return m
