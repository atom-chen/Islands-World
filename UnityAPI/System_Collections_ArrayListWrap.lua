---@class System.Collections.ArrayList
---@field public Capacity System.Int32
---@field public Count System.Int32
---@field public IsFixedSize System.Boolean
---@field public IsReadOnly System.Boolean
---@field public IsSynchronized System.Boolean
---@field public SyncRoot System.Object
---@field public Item System.Object
local m = { }
---public ArrayList .ctor()
---public ArrayList .ctor(Int32 capacity)
---public ArrayList .ctor(ICollection c)
---@return ArrayList
---@param ICollection c
function m.New(c) end
---public ArrayList Adapter(IList list)
---@return ArrayList
---@param optional IList list
function m.Adapter(list) end
---public Int32 Add(Object value)
---@return number
---@param optional Object value
function m:Add(value) end
---public Void AddRange(ICollection c)
---@param optional ICollection c
function m:AddRange(c) end
---public Int32 BinarySearch(Object value)
---public Int32 BinarySearch(Object value, IComparer comparer)
---public Int32 BinarySearch(Int32 index, Int32 count, Object value, IComparer comparer)
---@return number
---@param Int32 index
---@param Int32 count
---@param Object value
---@param optional IComparer comparer
function m:BinarySearch(index, count, value, comparer) end
---public Void Clear()
function m:Clear() end
---public Object Clone()
---@return Object
function m:Clone() end
---public Boolean Contains(Object item)
---@return bool
---@param optional Object item
function m:Contains(item) end
---public Void CopyTo(Array array)
---public Void CopyTo(Array array, Int32 arrayIndex)
---public Void CopyTo(Int32 index, Array array, Int32 arrayIndex, Int32 count)
---@param Int32 index
---@param Array array
---@param Int32 arrayIndex
---@param optional Int32 count
function m:CopyTo(index, array, arrayIndex, count) end
---public IList FixedSize(IList list)
---public ArrayList FixedSize(ArrayList list)
---@return IList
---@param optional ArrayList list
function m.FixedSize(list) end
---public IEnumerator GetEnumerator()
---public IEnumerator GetEnumerator(Int32 index, Int32 count)
---@return IEnumerator
---@param Int32 index
---@param Int32 count
function m:GetEnumerator(index, count) end
---public Int32 IndexOf(Object value)
---public Int32 IndexOf(Object value, Int32 startIndex)
---public Int32 IndexOf(Object value, Int32 startIndex, Int32 count)
---@return number
---@param Object value
---@param Int32 startIndex
---@param optional Int32 count
function m:IndexOf(value, startIndex, count) end
---public Void Insert(Int32 index, Object value)
---@param optional Int32 index
---@param optional Object value
function m:Insert(index, value) end
---public Void InsertRange(Int32 index, ICollection c)
---@param optional Int32 index
---@param optional ICollection c
function m:InsertRange(index, c) end
---public Int32 LastIndexOf(Object value)
---public Int32 LastIndexOf(Object value, Int32 startIndex)
---public Int32 LastIndexOf(Object value, Int32 startIndex, Int32 count)
---@return number
---@param Object value
---@param Int32 startIndex
---@param optional Int32 count
function m:LastIndexOf(value, startIndex, count) end
---public IList ReadOnly(IList list)
---public ArrayList ReadOnly(ArrayList list)
---@return IList
---@param optional ArrayList list
function m.ReadOnly(list) end
---public Void Remove(Object obj)
---@param optional Object obj
function m:Remove(obj) end
---public Void RemoveAt(Int32 index)
---@param optional Int32 index
function m:RemoveAt(index) end
---public Void RemoveRange(Int32 index, Int32 count)
---@param optional Int32 index
---@param optional Int32 count
function m:RemoveRange(index, count) end
---public ArrayList Repeat(Object value, Int32 count)
---@return ArrayList
---@param optional Object value
---@param optional Int32 count
function m.Repeat(value, count) end
---public Void Reverse()
---public Void Reverse(Int32 index, Int32 count)
---@param Int32 index
---@param Int32 count
function m:Reverse(index, count) end
---public Void SetRange(Int32 index, ICollection c)
---@param optional Int32 index
---@param optional ICollection c
function m:SetRange(index, c) end
---public ArrayList GetRange(Int32 index, Int32 count)
---@return ArrayList
---@param optional Int32 index
---@param optional Int32 count
function m:GetRange(index, count) end
---public Void Sort()
---public Void Sort(IComparer comparer)
---public Void Sort(Int32 index, Int32 count, IComparer comparer)
---@param Int32 index
---@param Int32 count
---@param IComparer comparer
function m:Sort(index, count, comparer) end
---public IList Synchronized(IList list)
---public ArrayList Synchronized(ArrayList list)
---@return IList
---@param optional ArrayList list
function m.Synchronized(list) end
---public Object[] ToArray()
---public Array ToArray(Type t)
---@return table
---@param Type t
function m:ToArray(type) end
---public Void TrimToSize()
function m:TrimToSize() end
System.Collections.ArrayList = m
return m
