---@class System.Collections.Hashtable
---@field public Item System.Object
---@field public IsReadOnly System.Boolean
---@field public IsFixedSize System.Boolean
---@field public IsSynchronized System.Boolean
---@field public Keys System.Collections.ICollection
---@field public Values System.Collections.ICollection
---@field public SyncRoot System.Object
---@field public Count System.Int32

local m = { }
---public Hashtable .ctor()
---public Hashtable .ctor(Int32 capacity)
---public Hashtable .ctor(IEqualityComparer equalityComparer)
---public Hashtable .ctor(IDictionary d)
---public Hashtable .ctor(Int32 capacity, Single loadFactor)
---public Hashtable .ctor(IHashCodeProvider hcp, IComparer comparer)
---public Hashtable .ctor(Int32 capacity, IEqualityComparer equalityComparer)
---public Hashtable .ctor(IDictionary d, Single loadFactor)
---public Hashtable .ctor(IDictionary d, IEqualityComparer equalityComparer)
---public Hashtable .ctor(Int32 capacity, Single loadFactor, IEqualityComparer equalityComparer)
---public Hashtable .ctor(Int32 capacity, IHashCodeProvider hcp, IComparer comparer)
---public Hashtable .ctor(IDictionary d, IHashCodeProvider hcp, IComparer comparer)
---public Hashtable .ctor(IDictionary d, Single loadFactor, IEqualityComparer equalityComparer)
---public Hashtable .ctor(Int32 capacity, Single loadFactor, IHashCodeProvider hcp, IComparer comparer)
---public Hashtable .ctor(IDictionary d, Single loadFactor, IHashCodeProvider hcp, IComparer comparer)
---@return Hashtable
---@param IDictionary d
---@param Single loadFactor
---@param IHashCodeProvider hcp
---@param IComparer comparer
function m.New(d, loadFactor, hcp, comparer) end
---public Void Add(Object key, Object value)
---@param optional Object key
---@param optional Object value
function m:Add(key, value) end
---public Void Clear()
function m:Clear() end
---public Object Clone()
---@return Object
function m:Clone() end
---public Boolean Contains(Object key)
---@return bool
---@param optional Object key
function m:Contains(key) end
---public Boolean ContainsKey(Object key)
---@return bool
---@param optional Object key
function m:ContainsKey(key) end
---public Boolean ContainsValue(Object value)
---@return bool
---@param optional Object value
function m:ContainsValue(value) end
---public Void CopyTo(Array array, Int32 arrayIndex)
---@param optional Array array
---@param optional Int32 arrayIndex
function m:CopyTo(array, arrayIndex) end
---public IDictionaryEnumerator GetEnumerator()
---@return IDictionaryEnumerator
function m:GetEnumerator() end
---public Void Remove(Object key)
---@param optional Object key
function m:Remove(key) end
---public Hashtable Synchronized(Hashtable tb)
---@return Hashtable
---@param optional Hashtable tb
function m.Synchronized(table) end
---public Void GetObjectData(SerializationInfo info, StreamingContext context)
---@param optional SerializationInfo info
---@param optional StreamingContext context
function m:GetObjectData(info, context) end
---public Void OnDeserialization(Object sender)
---@param optional Object sender
function m:OnDeserialization(sender) end
System.Collections.Hashtable = m
return m
