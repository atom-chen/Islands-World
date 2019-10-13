---@class Coolape.NewList : System.Collections.ArrayList
local m = { }
---public NewList .ctor()
---@return NewList
function m.New() end
---public NewList create()
---public NewList create(Object[] args)
---public NewList create(ArrayList list)
---@return NewList
---@param ArrayList list
function m.create(list) end
---public NewList add(Object val)
---public NewList add(Object[] args)
---@return NewList
---@param optional Object[] args
function m:add(args) end
---public Boolean Contains(Object o)
---@return bool
---@param optional Object o
function m:Contains(o) end
---public Object getObject(Int32 i)
---public Object getObject(ArrayList list, Int32 i)
---@return Object
---@param ArrayList list
---@param optional Int32 i
function m:getObject(list, i) end
---public Boolean getBool(Int32 i)
---@return bool
---@param optional Int32 i
function m:getBool(i) end
---public Byte getByte(Int32 i)
---@return number
---@param optional Int32 i
function m:getByte(i) end
---public Int32 getInt(Int32 i)
---@return number
---@param optional Int32 i
function m:getInt(i) end
---public Double getLong(Int32 i)
---@return number
---@param optional Int32 i
function m:getLong(i) end
---public Double getDouble(Int32 i)
---@return number
---@param optional Int32 i
function m:getDouble(i) end
---public String getString(Int32 i)
---@return String
---@param optional Int32 i
function m:getString(i) end
---public ArrayList getList(Int32 i)
---@return ArrayList
---@param optional Int32 i
function m:getList(i) end
---public NewList getNewList(Int32 i)
---@return NewList
---@param optional Int32 i
function m:getNewList(i) end
---public Hashtable getMap(Int32 i)
---@return Hashtable
---@param optional Int32 i
function m:getMap(i) end
---public NewMap getNewMap(Int32 i)
---@return NewMap
---@param optional Int32 i
function m:getNewMap(i) end
---public Int32 pageCount(Int32 pageSize)
---@return number
---@param optional Int32 pageSize
function m:pageCount(pageSize) end
---public ArrayList getPage(Int32 page, Int32 pageSize)
---@return ArrayList
---@param optional Int32 page
---@param optional Int32 pageSize
function m:getPage(page, pageSize) end
Coolape.NewList = m
return m
