---@class System.Collections.Stack
---@field public Count System.Int32
---@field public IsSynchronized System.Boolean
---@field public SyncRoot System.Object
local m = { }
---public Stack .ctor()
---public Stack .ctor(Int32 initialCapacity)
---public Stack .ctor(ICollection col)
---@return Stack
---@param ICollection col
function m.New(col) end
---public Void Clear()
function m:Clear() end
---public Object Clone()
---@return Object
function m:Clone() end
---public Boolean Contains(Object obj)
---@return bool
---@param optional Object obj
function m:Contains(obj) end
---public Void CopyTo(Array array, Int32 index)
---@param optional Array array
---@param optional Int32 index
function m:CopyTo(array, index) end
---public IEnumerator GetEnumerator()
---@return IEnumerator
function m:GetEnumerator() end
---public Object Peek()
---@return Object
function m:Peek() end
---public Object Pop()
---@return Object
function m:Pop() end
---public Void Push(Object obj)
---@param optional Object obj
function m:Push(obj) end
---public Stack Synchronized(Stack stack)
---@return Stack
---@param optional Stack stack
function m.Synchronized(stack) end
---public Object[] ToArray()
---@return table
function m:ToArray() end
System.Collections.Stack = m
return m
