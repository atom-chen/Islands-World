---@class Coolape.PStr
---@field public sb System.Text.StringBuilder

local m = { }
---public PStr b()
---public PStr b(Object[] objs)
---public PStr b(String s)
---@return PStr
---@param String s
function m.b(s) end
---public PStr begin()
---public PStr begin(String s)
---public PStr begin(Object[] objs)
---@return PStr
---@param Object[] objs
function m.begin(objs) end
---public PStr a(SByte value)
---public PStr a(Single value)
---public PStr a(Int64 value)
---public PStr a(Object value)
---public PStr a(UInt16 value)
---public PStr a(Char value)
---public PStr a(Byte[] value)
---public PStr a(UInt32 value)
---public PStr a(UInt64 value)
---public PStr a(Boolean value)
---public PStr a(Byte value)
---public PStr a(String value)
---public PStr a(Char[] value)
---public PStr a(Decimal value)
---public PStr a(Int32 value)
---public PStr a(Object[] objs)
---public PStr a(Double value)
---public PStr a(Int16 value)
---public PStr a(String s, Object[] args)
---public PStr a(String fmt, NewMap map)
---public PStr a(Char value, Int32 repeatCount)
---public PStr a(Char[] value, Int32 startIndex, Int32 charCount)
---public PStr a(String value, Int32 startIndex, Int32 count)
---@return PStr
---@param String value
---@param Int32 startIndex
---@param optional Int32 count
function m:a(value, startIndex, count) end
---public PStr fmt(String fmt, Object[] args)
---@return PStr
---@param optional String fmt
---@param optional Object[] args
function m:fmt(fmt, args) end
---public PStr a_kv(String fmt, Object[] args)
---@return PStr
---@param optional String fmt
---@param optional Object[] args
function m:a_kv(fmt, args) end
---public PStr an(String s)
---public PStr an(Object[] objs)
---@return PStr
---@param optional Object[] objs
function m:an(objs) end
---public Int32 Length()
---@return number
function m:Length() end
---public String e()
---public String e(String s)
---@return String
---@param String s
function m:e(s) end
---public String end()
---public String end(String s)
---@return String
---@param String s
function m:end(s) end
---public String str()
---@return String
function m:str() end
Coolape.PStr = m
return m
