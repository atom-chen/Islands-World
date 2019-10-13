---@class Coolape.MapEx
---@field public map System.Collections.Hashtable
local m = { }
---public MapEx .ctor()
---public MapEx .ctor(Hashtable map)
---@return MapEx
---@param Hashtable map
function m.New(map) end
---public MapEx builder()
---@return MapEx
function m.builder() end
---public MapEx Add(Object key, Object value)
---@return MapEx
---@param optional Object key
---@param optional Object value
function m:Add(key, value) end
---public Int32 Count()
---@return number
function m:Count() end
---public MapEx Clear()
---@return MapEx
function m:Clear() end
---public MapEx Set(Object key, Object value)
---@return MapEx
---@param optional Object key
---@param optional Object value
function m:Set(key, value) end
---public Hashtable ToMap()
---public Hashtable ToMap(ArrayList list)
---@return Hashtable
---@param ArrayList list
function m:ToMap(list) end
---public ICollection Keys()
---@return ICollection
function m:Keys() end
---public ArrayList KeysList()
---@return ArrayList
function m:KeysList() end
---public ICollection Values()
---@return ICollection
function m:Values() end
---public ArrayList ValuesList()
---@return ArrayList
function m:ValuesList() end
---public Boolean ContainsKey(Object key)
---@return bool
---@param optional Object key
function m:ContainsKey(key) end
---public Boolean ContainsValue(Object value)
---@return bool
---@param optional Object value
function m:ContainsValue(value) end
---public Object get(Object key)
---public Object get(Hashtable map, Object key)
---@return Object
---@param Hashtable map
---@param optional Object key
function m:get(map, key) end
---public Boolean getBool(Object key)
---public Boolean getBool(Object map, Object key)
---@return bool
---@param Object map
---@param optional Object key
function m:getBool(map, key) end
---public Byte getByte(Object key)
---public Byte getByte(Hashtable map, Object key)
---@return number
---@param Hashtable map
---@param optional Object key
function m:getByte(map, key) end
---public Int32 getInt(Object key)
---public Int32 getInt(Object map, Object key)
---@return number
---@param Object map
---@param optional Object key
function m:getInt(map, key) end
---public Int64 getLong(Object key)
---public Int64 getLong(Hashtable map, Object key)
---@return long
---@param Hashtable map
---@param optional Object key
function m:getLong(map, key) end
---public Double getDouble(Object key)
---public Double getDouble(Hashtable map, Object key)
---@return number
---@param Hashtable map
---@param optional Object key
function m:getDouble(map, key) end
---public String getString(Object key)
---public String getString(Object map, Object key)
---@return String
---@param Object map
---@param optional Object key
function m:getString(map, key) end
---public ArrayList getList(Object key)
---public ArrayList getList(Hashtable map, Object key)
---@return ArrayList
---@param Hashtable map
---@param optional Object key
function m:getList(map, key) end
---public Hashtable getMap(Object key)
---public Hashtable getMap(Hashtable map, Object key)
---@return Hashtable
---@param Hashtable map
---@param optional Object key
function m:getMap(map, key) end
---public Void set(Hashtable map, Object key, Object value)
---@param optional Hashtable map
---@param optional Object key
---@param optional Object value
function m.set(map, key, value) end
---public Object getObject(Hashtable map, Object key)
---@return Object
---@param optional Hashtable map
---@param optional Object key
function m.getObject(map, key) end
---public Byte[] getBytes(Object map, Object key)
---@return table
---@param optional Object map
---@param optional Object key
function m.getBytes(map, key) end
---public Int32 getBytes2Int(Object map, Object key)
---@return number
---@param optional Object map
---@param optional Object key
function m.getBytes2Int(map, key) end
---public Void setInt2Bytes(Object map, Object key, Int32 val)
---@param optional Object map
---@param optional Object key
---@param optional Int32 val
function m.setInt2Bytes(map, key, val) end
---public Void setIntKey(Hashtable map, Int32 key, Object val)
---@param optional Hashtable map
---@param optional Int32 key
---@param optional Object val
function m.setIntKey(map, key, val) end
---public Object getByIntKey(Hashtable map, Int32 key)
---@return Object
---@param optional Hashtable map
---@param optional Int32 key
function m.getByIntKey(map, key) end
---public Hashtable newMap()
---@return Hashtable
function m.newMap() end
---public Hashtable createKvs(Object[] kv)
---@return Hashtable
---@param optional Object[] kv
function m.createKvs(kv) end
---public Hashtable putKvs(Hashtable map, Object[] kv)
---public NewMap putKvs(NewMap map, Object[] kv)
---@return Hashtable
---@param optional NewMap map
---@param optional Object[] kv
function m.putKvs(map, kv) end
---public Boolean isNull(Hashtable map)
---@return bool
---@param optional Hashtable map
function m.isNull(map) end
---public Boolean isNullOrEmpty(Hashtable map)
---@return bool
---@param optional Hashtable map
function m.isNullOrEmpty(map) end
---public Void clearMap(Hashtable map)
---@param optional Hashtable map
function m.clearMap(map) end
---public Void clearNullMap(Hashtable map)
---@param optional Hashtable map
function m.clearNullMap(map) end
---public ArrayList keys2List(Hashtable map)
---@return ArrayList
---@param optional Hashtable map
function m.keys2List(map) end
---public ArrayList vals2List(Hashtable map)
---@return ArrayList
---@param optional Hashtable map
function m.vals2List(map) end
---public Hashtable cloneMap(Hashtable old, Hashtable nwMap)
---@return Hashtable
---@param optional Hashtable old
---@param optional Hashtable nwMap
function m.cloneMap(old, nwMap) end
---public Boolean isHashtable(Object obj)
---@return bool
---@param optional Object obj
function m.isHashtable(obj) end
Coolape.MapEx = m
return m
