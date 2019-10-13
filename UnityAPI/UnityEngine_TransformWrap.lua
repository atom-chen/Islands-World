---@class UnityEngine.Transform : UnityEngine.Component
---@field public position UnityEngine.Vector3
---@field public localPosition UnityEngine.Vector3
---@field public eulerAngles UnityEngine.Vector3
---@field public localEulerAngles UnityEngine.Vector3
---@field public right UnityEngine.Vector3
---@field public up UnityEngine.Vector3
---@field public forward UnityEngine.Vector3
---@field public rotation UnityEngine.Quaternion
---@field public localRotation UnityEngine.Quaternion
---@field public localScale UnityEngine.Vector3
---@field public parent UnityEngine.Transform
---@field public worldToLocalMatrix UnityEngine.Matrix4x4
---@field public localToWorldMatrix UnityEngine.Matrix4x4
---@field public root UnityEngine.Transform
---@field public childCount System.Int32
---@field public lossyScale UnityEngine.Vector3
---@field public hasChanged System.Boolean
---@field public hierarchyCapacity System.Int32
---@field public hierarchyCount System.Int32
local m = { }
---public Void SetParent(Transform p)
---public Void SetParent(Transform parent, Boolean worldPositionStays)
---@param Transform parent
---@param optional Boolean worldPositionStays
function m:SetParent(parent, worldPositionStays) end
---public Void SetPositionAndRotation(Vector3 position, Quaternion rotation)
---@param optional Vector3 position
---@param optional Quaternion rotation
function m:SetPositionAndRotation(position, rotation) end
---public Void Translate(Vector3 translation)
---public Void Translate(Vector3 translation, Space relativeTo)
---public Void Translate(Vector3 translation, Transform relativeTo)
---public Void Translate(Single x, Single y, Single z)
---public Void Translate(Single x, Single y, Single z, Space relativeTo)
---public Void Translate(Single x, Single y, Single z, Transform relativeTo)
---@param Single x
---@param Single y
---@param Single z
---@param optional Transform relativeTo
function m:Translate(x, y, z, relativeTo) end
---public Void Rotate(Vector3 eulers)
---public Void Rotate(Vector3 eulers, Space relativeTo)
---public Void Rotate(Vector3 axis, Single angle)
---public Void Rotate(Single xAngle, Single yAngle, Single zAngle)
---public Void Rotate(Vector3 axis, Single angle, Space relativeTo)
---public Void Rotate(Single xAngle, Single yAngle, Single zAngle, Space relativeTo)
---@param Single xAngle
---@param Single yAngle
---@param Single zAngle
---@param optional Space relativeTo
function m:Rotate(xAngle, yAngle, zAngle, relativeTo) end
---public Void RotateAround(Vector3 point, Vector3 axis, Single angle)
---@param optional Vector3 point
---@param optional Vector3 axis
---@param optional Single angle
function m:RotateAround(point, axis, angle) end
---public Void LookAt(Transform target)
---public Void LookAt(Vector3 worldPosition)
---public Void LookAt(Transform target, Vector3 worldUp)
---public Void LookAt(Vector3 worldPosition, Vector3 worldUp)
---@param Vector3 worldPosition
---@param optional Vector3 worldUp
function m:LookAt(worldPosition, worldUp) end
---public Vector3 TransformDirection(Vector3 direction)
---public Vector3 TransformDirection(Single x, Single y, Single z)
---@return Vector3
---@param Single x
---@param Single y
---@param optional Single z
function m:TransformDirection(x, y, z) end
---public Vector3 InverseTransformDirection(Vector3 direction)
---public Vector3 InverseTransformDirection(Single x, Single y, Single z)
---@return Vector3
---@param Single x
---@param Single y
---@param optional Single z
function m:InverseTransformDirection(x, y, z) end
---public Vector3 TransformVector(Vector3 vector)
---public Vector3 TransformVector(Single x, Single y, Single z)
---@return Vector3
---@param Single x
---@param Single y
---@param optional Single z
function m:TransformVector(x, y, z) end
---public Vector3 InverseTransformVector(Vector3 vector)
---public Vector3 InverseTransformVector(Single x, Single y, Single z)
---@return Vector3
---@param Single x
---@param Single y
---@param optional Single z
function m:InverseTransformVector(x, y, z) end
---public Vector3 TransformPoint(Vector3 position)
---public Vector3 TransformPoint(Single x, Single y, Single z)
---@return Vector3
---@param Single x
---@param Single y
---@param optional Single z
function m:TransformPoint(x, y, z) end
---public Vector3 InverseTransformPoint(Vector3 position)
---public Vector3 InverseTransformPoint(Single x, Single y, Single z)
---@return Vector3
---@param Single x
---@param Single y
---@param optional Single z
function m:InverseTransformPoint(x, y, z) end
---public Void DetachChildren()
function m:DetachChildren() end
---public Void SetAsFirstSibling()
function m:SetAsFirstSibling() end
---public Void SetAsLastSibling()
function m:SetAsLastSibling() end
---public Void SetSiblingIndex(Int32 index)
---@param optional Int32 index
function m:SetSiblingIndex(index) end
---public Int32 GetSiblingIndex()
---@return number
function m:GetSiblingIndex() end
---public Transform Find(String n)
---@return Transform
---@param optional String n
function m:Find(n) end
---public Boolean IsChildOf(Transform parent)
---@return bool
---@param optional Transform parent
function m:IsChildOf(parent) end
---public IEnumerator GetEnumerator()
---@return IEnumerator
function m:GetEnumerator() end
---public Transform GetChild(Int32 index)
---@return Transform
---@param optional Int32 index
function m:GetChild(index) end
UnityEngine.Transform = m
return m
