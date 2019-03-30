---@class System.Collections.Generic.List`1[[System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
---@field public Capacity System.Int32
---@field public Count System.Int32
---@field public Item System.Int32

local m = { }
---public List`1 .ctor()
---public List`1 .ctor(Int32 capacity)
---public List`1 .ctor(IEnumerable`1 collection)
---@return List`1
---@param IEnumerable`1 collection
function m.New(collection) end
---public Void Add(Int32 item)
---@param optional Int32 item
function m:Add(item) end
---public Void AddRange(IEnumerable`1 collection)
---@param optional IEnumerable`1 collection
function m:AddRange(collection) end
---public ReadOnlyCollection`1 AsReadOnly()
---@return ReadOnlyCollection`1
function m:AsReadOnly() end
---public Int32 BinarySearch(Int32 item)
---public Int32 BinarySearch(Int32 item, IComparer`1 comparer)
---public Int32 BinarySearch(Int32 index, Int32 count, Int32 item, IComparer`1 comparer)
---@return number
---@param Int32 index
---@param Int32 count
---@param Int32 item
---@param optional IComparer`1 comparer
function m:BinarySearch(index, count, item, comparer) end
---public Void Clear()
function m:Clear() end
---public Boolean Contains(Int32 item)
---@return bool
---@param optional Int32 item
function m:Contains(item) end
---public Void CopyTo(Int32[] array)
---public Void CopyTo(Int32[] array, Int32 arrayIndex)
---public Void CopyTo(Int32 index, Int32[] array, Int32 arrayIndex, Int32 count)
---@param Int32 index
---@param Int32[] array
---@param Int32 arrayIndex
---@param optional Int32 count
function m:CopyTo(index, array, arrayIndex, count) end
---public Boolean Exists(Predicate`1 match)
---@return bool
---@param optional Predicate`1 match
function m:Exists(match) end
---public Int32 Find(Predicate`1 match)
---@return number
---@param optional Predicate`1 match
function m:Find(match) end
---public List`1 FindAll(Predicate`1 match)
---@return List`1
---@param optional Predicate`1 match
function m:FindAll(match) end
---public Int32 FindIndex(Predicate`1 match)
---public Int32 FindIndex(Int32 startIndex, Predicate`1 match)
---public Int32 FindIndex(Int32 startIndex, Int32 count, Predicate`1 match)
---@return number
---@param Int32 startIndex
---@param Int32 count
---@param optional Predicate`1 match
function m:FindIndex(startIndex, count, match) end
---public Int32 FindLast(Predicate`1 match)
---@return number
---@param optional Predicate`1 match
function m:FindLast(match) end
---public Int32 FindLastIndex(Predicate`1 match)
---public Int32 FindLastIndex(Int32 startIndex, Predicate`1 match)
---public Int32 FindLastIndex(Int32 startIndex, Int32 count, Predicate`1 match)
---@return number
---@param Int32 startIndex
---@param Int32 count
---@param optional Predicate`1 match
function m:FindLastIndex(startIndex, count, match) end
---public Void ForEach(Action`1 action)
---@param optional Action`1 action
function m:ForEach(action) end
---public Enumerator GetEnumerator()
---@return Enumerator
function m:GetEnumerator() end
---public List`1 GetRange(Int32 index, Int32 count)
---@return List`1
---@param optional Int32 index
---@param optional Int32 count
function m:GetRange(index, count) end
---public Int32 IndexOf(Int32 item)
---public Int32 IndexOf(Int32 item, Int32 index)
---public Int32 IndexOf(Int32 item, Int32 index, Int32 count)
---@return number
---@param Int32 item
---@param Int32 index
---@param optional Int32 count
function m:IndexOf(item, index, count) end
---public Void Insert(Int32 index, Int32 item)
---@param optional Int32 index
---@param optional Int32 item
function m:Insert(index, item) end
---public Void InsertRange(Int32 index, IEnumerable`1 collection)
---@param optional Int32 index
---@param optional IEnumerable`1 collection
function m:InsertRange(index, collection) end
---public Int32 LastIndexOf(Int32 item)
---public Int32 LastIndexOf(Int32 item, Int32 index)
---public Int32 LastIndexOf(Int32 item, Int32 index, Int32 count)
---@return number
---@param Int32 item
---@param Int32 index
---@param optional Int32 count
function m:LastIndexOf(item, index, count) end
---public Boolean Remove(Int32 item)
---@return bool
---@param optional Int32 item
function m:Remove(item) end
---public Int32 RemoveAll(Predicate`1 match)
---@return number
---@param optional Predicate`1 match
function m:RemoveAll(match) end
---public Void RemoveAt(Int32 index)
---@param optional Int32 index
function m:RemoveAt(index) end
---public Void RemoveRange(Int32 index, Int32 count)
---@param optional Int32 index
---@param optional Int32 count
function m:RemoveRange(index, count) end
---public Void Reverse()
---public Void Reverse(Int32 index, Int32 count)
---@param Int32 index
---@param Int32 count
function m:Reverse(index, count) end
---public Void Sort()
---public Void Sort(Comparison`1 comparison)
---public Void Sort(IComparer`1 comparer)
---public Void Sort(Int32 index, Int32 count, IComparer`1 comparer)
---@param Int32 index
---@param Int32 count
---@param IComparer`1 comparer
function m:Sort(index, count, comparer) end
---public Int32[] ToArray()
---@return table
function m:ToArray() end
---public Void TrimExcess()
function m:TrimExcess() end
---public Boolean TrueForAll(Predicate`1 match)
---@return bool
---@param optional Predicate`1 match
function m:TrueForAll(match) end
System.Collections.Generic.List`1[[System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]] = m
return m
