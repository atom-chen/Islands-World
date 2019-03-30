---@class Coolape.NewMap : System.Collections.Hashtable

local m = { }
---public NewMap .ctor()
---@return NewMap
function m.New() end
---public NewMap create()
---public NewMap create(Object[] args)
---public NewMap create(Hashtable map)
---@return NewMap
---@param Hashtable map
function m.create(map) end
---public NewMap put(Object[] args)
---public NewMap put(Object key, Object val)
---@return NewMap
---@param Object key
---@param optional Object val
function m:put(key, val) end
---public NewMap putPut(Object key, Object val)
---@return NewMap
---@param optional Object key
---@param optional Object val
function m:putPut(key, val) end
---public Object getObject(Object key)
---public Object getObject(Hashtable map, Object key)
---@return Object
---@param Hashtable map
---@param optional Object key
function m:getObject(map, key) end
---public Boolean getBool(Object key)
---@return bool
---@param optional Object key
function m:getBool(key) end
---public Byte getByte(Object key)
---@return number
---@param optional Object key
function m:getByte(key) end
---public Int16 getShort(Object key)
---@return number
---@param optional Object key
function m:getShort(key) end
---public Int32 getInt(Object key)
---@return number
---@param optional Object key
function m:getInt(key) end
---public Int64 getLong(Object key)
---@return long
---@param optional Object key
function m:getLong(key) end
---public Single getFloat(Object key)
---@return number
---@param optional Object key
function m:getFloat(key) end
---public Double getDouble(Object key)
---@return number
---@param optional Object key
function m:getDouble(key) end
---public Byte[] getBytes(Object key)
---@return table
---@param optional Object key
function m:getBytes(key) end
---public String getString(Object key)
---@return String
---@param optional Object key
function m:getString(key) end
---public ArrayList getList(Object key)
---@return ArrayList
---@param optional Object key
function m:getList(key) end
---public NewList getNewList(Object key)
---@return NewList
---@param optional Object key
function m:getNewList(key) end
---public Hashtable getMap(Object key)
---@return Hashtable
---@param optional Object key
function m:getMap(key) end
---public NewMap getNewMap(Object key)
---@return NewMap
---@param optional Object key
function m:getNewMap(key) end
---public NewSet getNewSet(Object key)
---@return NewSet
---@param optional Object key
function m:getNewSet(key) end
Coolape.NewMap = m
return m
