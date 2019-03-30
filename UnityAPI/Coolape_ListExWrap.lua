---@class Coolape.ListEx
---@field public list System.Collections.ArrayList

local m = { }
---public ListEx .ctor()
---public ListEx .ctor(ArrayList list)
---@return ListEx
---@param ArrayList list
function m.New(list) end
---public ListEx builder()
---@return ListEx
function m.builder() end
---public ListEx Add(Object o)
---@return ListEx
---@param optional Object o
function m:Add(o) end
---public ArrayList ToList()
---public ArrayList ToList(Object[] arrays)
---@return ArrayList
---@param Object[] arrays
function m:ToList(arrays) end
---public Int32 Count()
---@return number
function m:Count() end
---public ListEx Clear()
---@return ListEx
function m:Clear() end
---public ListEx set(Int32 index, Object obj)
---@return ListEx
---@param optional Int32 index
---@param optional Object obj
function m:set(index, obj) end
---public Object get(Int32 index)
---public Object get(ArrayList list, Int32 index)
---@return Object
---@param ArrayList list
---@param optional Int32 index
function m:get(list, index) end
---public Boolean getBool(Int32 index)
---public Boolean getBool(ArrayList list, Int32 index)
---@return bool
---@param ArrayList list
---@param optional Int32 index
function m:getBool(list, index) end
---public Byte getByte(Int32 index)
---public Byte getByte(ArrayList list, Int32 index)
---@return number
---@param ArrayList list
---@param optional Int32 index
function m:getByte(list, index) end
---public Int32 getInt(Int32 index)
---public Int32 getInt(ArrayList list, Int32 index)
---@return number
---@param ArrayList list
---@param optional Int32 index
function m:getInt(list, index) end
---public Int64 getLong(Int32 index)
---public Int64 getLong(ArrayList list, Int32 index)
---@return long
---@param ArrayList list
---@param optional Int32 index
function m:getLong(list, index) end
---public Double getDouble(Int32 index)
---public Double getDouble(ArrayList list, Int32 index)
---@return number
---@param ArrayList list
---@param optional Int32 index
function m:getDouble(list, index) end
---public String getString(Int32 index)
---public String getString(ArrayList list, Int32 index)
---@return String
---@param ArrayList list
---@param optional Int32 index
function m:getString(list, index) end
---public ArrayList getList(Int32 index)
---public ArrayList getList(ArrayList list, Int32 index)
---@return ArrayList
---@param ArrayList list
---@param optional Int32 index
function m:getList(list, index) end
---public Hashtable getMap(Int32 index)
---public Hashtable getMap(ArrayList list, Int32 index)
---@return Hashtable
---@param ArrayList list
---@param optional Int32 index
function m:getMap(list, index) end
---public ArrayList toList(Object[] args)
---@return ArrayList
---@param optional Object[] args
function m.toList(args) end
---public ArrayList newList()
---@return ArrayList
function m.newList() end
---public Boolean containsIntVal(ArrayList list, Int32 v)
---@return bool
---@param optional ArrayList list
---@param optional Int32 v
function m.containsIntVal(list, v) end
---public Boolean withIn(Int32[] list, Int32 v)
---public Boolean withIn(ArrayList list, Object v)
---@return bool
---@param optional ArrayList list
---@param optional Object v
function m.withIn(list, v) end
---public String Next(String[] arrays)
---public Int32 Next(Int32[] arrays)
---public Object Next(ArrayList list)
---@return String
---@param optional ArrayList list
function m.Next(list) end
---public ArrayList Next2(ArrayList list)
---@return ArrayList
---@param optional ArrayList list
function m.Next2(list) end
---public ArrayList Copy(ArrayList list)
---@return ArrayList
---@param optional ArrayList list
function m.Copy(list) end
---public String ToString()
---public String ToString(ArrayList list)
---@return String
---@param ArrayList list
function m:ToString(list) end
---public ArrayList shuffleRnd(ArrayList src)
---@return ArrayList
---@param optional ArrayList src
function m.shuffleRnd(src) end
---public ArrayList sort(ArrayList list, IComparer c)
---@return ArrayList
---@param optional ArrayList list
---@param optional IComparer c
function m.sort(list, c) end
---public List`1 sortHashtable(List`1 list, String key)
---@return List`1
---@param optional List`1 list
---@param optional String key
function m.sortHashtable(list, key) end
---public List`1 sortNewMap(List`1 list, String key)
---@return List`1
---@param optional List`1 list
---@param optional String key
function m.sortNewMap(list, key) end
---public Int32 compareTo(Object o1, Object o2)
---@return number
---@param optional Object o1
---@param optional Object o2
function m.compareTo(o1, o2) end
---public Boolean isNull(IList list)
---@return bool
---@param optional IList list
function m.isNull(list) end
---public Boolean isNullOrEmpty(IList list)
---@return bool
---@param optional IList list
function m.isNullOrEmpty(list) end
---public Void clearList(IList list)
---@param optional IList list
function m.clearList(list) end
---public Void clearNullList(IList list)
---@param optional IList list
function m.clearNullList(list) end
---public NewList getListNone(Int32 allLen, Int32 curLen)
---@return NewList
---@param optional Int32 allLen
---@param optional Int32 curLen
function m.getListNone(allLen, curLen) end
Coolape.ListEx = m
return m
