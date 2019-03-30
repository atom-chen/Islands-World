---@class System.Collections.Queue
---@field public Count System.Int32
---@field public IsSynchronized System.Boolean
---@field public SyncRoot System.Object

local m = { }
---public Queue .ctor()
---public Queue .ctor(ICollection col)
---public Queue .ctor(Int32 capacity)
---public Queue .ctor(Int32 capacity, Single growFactor)
---@return Queue
---@param Int32 capacity
---@param Single growFactor
function m.New(capacity, growFactor) end
---public Void CopyTo(Array array, Int32 index)
---@param optional Array array
---@param optional Int32 index
function m:CopyTo(array, index) end
---public IEnumerator GetEnumerator()
---@return IEnumerator
function m:GetEnumerator() end
---public Object Clone()
---@return Object
function m:Clone() end
---public Void Clear()
function m:Clear() end
---public Boolean Contains(Object obj)
---@return bool
---@param optional Object obj
function m:Contains(obj) end
---public Object Dequeue()
---@return Object
function m:Dequeue() end
---public Void Enqueue(Object obj)
---@param optional Object obj
function m:Enqueue(obj) end
---public Object Peek()
---@return Object
function m:Peek() end
---public Queue Synchronized(Queue queue)
---@return Queue
---@param optional Queue queue
function m.Synchronized(queue) end
---public Object[] ToArray()
---@return table
function m:ToArray() end
---public Void TrimToSize()
function m:TrimToSize() end
System.Collections.Queue = m
return m
