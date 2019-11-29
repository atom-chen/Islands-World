---@class MyBoundsPool : Coolape.AbstractObjectPool`1[[UnityEngine.Bounds, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@field public pool MyBoundsPool
local m = { }
---public MyBoundsPool .ctor()
---@return MyBoundsPool
function m.New() end
---public Bounds createObject(String key)
---@return Bounds
---@param optional String key
function m:createObject(key) end
---public Bounds resetObject(Bounds t)
---@return Bounds
---@param optional Bounds t
function m:resetObject(t) end
---public Bounds borrow()
---public Bounds borrow(Vector3 center, Vector3 size)
---@return Bounds
---@param Vector3 center
---@param Vector3 size
function m.borrow(center, size) end
---public Void returnObj(Bounds b)
---@param optional Bounds b
function m.returnObj(b) end
MyBoundsPool = m
return m
