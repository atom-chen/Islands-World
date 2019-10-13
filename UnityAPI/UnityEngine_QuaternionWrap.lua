---@class UnityEngine.Quaternion : System.ValueType
---@field public x System.Single
---@field public y System.Single
---@field public z System.Single
---@field public w System.Single
---@field public kEpsilon System.Single
---@field public Item System.Single
---@field public identity UnityEngine.Quaternion
---@field public eulerAngles UnityEngine.Vector3
---@field public normalized UnityEngine.Quaternion
local m = { }
---public Quaternion .ctor(Single x, Single y, Single z, Single w)
---@return Quaternion
---@param optional Single x
---@param optional Single y
---@param optional Single z
---@param optional Single w
function m.New(x, y, z, w) end
---public Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
---@return Quaternion
---@param optional Vector3 fromDirection
---@param optional Vector3 toDirection
function m.FromToRotation(fromDirection, toDirection) end
---public Quaternion Inverse(Quaternion rotation)
---@return Quaternion
---@param optional Quaternion rotation
function m.Inverse(rotation) end
---public Quaternion Slerp(Quaternion a, Quaternion b, Single t)
---@return Quaternion
---@param optional Quaternion a
---@param optional Quaternion b
---@param optional Single t
function m.Slerp(a, b, t) end
---public Quaternion SlerpUnclamped(Quaternion a, Quaternion b, Single t)
---@return Quaternion
---@param optional Quaternion a
---@param optional Quaternion b
---@param optional Single t
function m.SlerpUnclamped(a, b, t) end
---public Quaternion Lerp(Quaternion a, Quaternion b, Single t)
---@return Quaternion
---@param optional Quaternion a
---@param optional Quaternion b
---@param optional Single t
function m.Lerp(a, b, t) end
---public Quaternion LerpUnclamped(Quaternion a, Quaternion b, Single t)
---@return Quaternion
---@param optional Quaternion a
---@param optional Quaternion b
---@param optional Single t
function m.LerpUnclamped(a, b, t) end
---public Quaternion AngleAxis(Single angle, Vector3 axis)
---@return Quaternion
---@param optional Single angle
---@param optional Vector3 axis
function m.AngleAxis(angle, axis) end
---public Quaternion LookRotation(Vector3 forward)
---public Quaternion LookRotation(Vector3 forward, Vector3 upwards)
---@return Quaternion
---@param Vector3 forward
---@param optional Vector3 upwards
function m.LookRotation(forward, upwards) end
---public Void Set(Single newX, Single newY, Single newZ, Single newW)
---@param optional Single newX
---@param optional Single newY
---@param optional Single newZ
---@param optional Single newW
function m:Set(newX, newY, newZ, newW) end
---public Quaternion op_Multiply(Quaternion lhs, Quaternion rhs)
---public Vector3 op_Multiply(Quaternion rotation, Vector3 point)
---@return Quaternion
---@param optional Quaternion rotation
---@param optional Vector3 point
function m.op_Multiply(rotation, point) end
---public Boolean op_Equality(Quaternion lhs, Quaternion rhs)
---@return bool
---@param optional Quaternion lhs
---@param optional Quaternion rhs
function m.op_Equality(lhs, rhs) end
---public Boolean op_Inequality(Quaternion lhs, Quaternion rhs)
---@return bool
---@param optional Quaternion lhs
---@param optional Quaternion rhs
function m.op_Inequality(lhs, rhs) end
---public Single Dot(Quaternion a, Quaternion b)
---@return number
---@param optional Quaternion a
---@param optional Quaternion b
function m.Dot(a, b) end
---public Void SetLookRotation(Vector3 view)
---public Void SetLookRotation(Vector3 view, Vector3 up)
---@param Vector3 view
---@param optional Vector3 up
function m:SetLookRotation(view, up) end
---public Single Angle(Quaternion a, Quaternion b)
---@return number
---@param optional Quaternion a
---@param optional Quaternion b
function m.Angle(a, b) end
---public Quaternion Euler(Vector3 euler)
---public Quaternion Euler(Single x, Single y, Single z)
---@return Quaternion
---@param Single x
---@param Single y
---@param optional Single z
function m.Euler(x, y, z) end
---public Void ToAngleAxis(Single& angle, Vector3& axis)
---@param optional Single& angle
---@param optional Vector3& axis
function m:ToAngleAxis(angle, axis) end
---public Void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection)
---@param optional Vector3 fromDirection
---@param optional Vector3 toDirection
function m:SetFromToRotation(fromDirection, toDirection) end
---public Quaternion RotateTowards(Quaternion from, Quaternion to, Single maxDegreesDelta)
---@return Quaternion
---@param optional Quaternion from
---@param optional Quaternion to
---@param optional Single maxDegreesDelta
function m.RotateTowards(from, to, maxDegreesDelta) end
---public Void Normalize()
---public Quaternion Normalize(Quaternion q)
---@param Quaternion q
function m:Normalize(q) end
---public Int32 GetHashCode()
---@return number
function m:GetHashCode() end
---public Boolean Equals(Object other)
---public Boolean Equals(Quaternion other)
---@return bool
---@param optional Quaternion other
function m:Equals(other) end
---public String ToString()
---public String ToString(String format)
---@return String
---@param String format
function m:ToString(format) end
UnityEngine.Quaternion = m
return m
