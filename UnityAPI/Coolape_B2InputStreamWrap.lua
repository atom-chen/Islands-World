---@class Coolape.B2InputStream
local m = { }
---public B2InputStream .ctor()
---@return B2InputStream
function m.New() end
---public Int32 ReadByte(Stream s)
---@return number
---@param optional Stream s
function m.ReadByte(s) end
---public Int32 readInt(Stream s)
---public Int32 readInt(Byte[] b, Ref rf)
---@return number
---@param Byte[] b
---@param optional Ref rf
function m.readInt(b, rf) end
---public Int32 byte2Int(Byte[] b, Ref rf)
---@return number
---@param optional Byte[] b
---@param optional Ref rf
function m.byte2Int(b, rf) end
---public Object readObject(Stream s)
---@return Object
---@param optional Stream s
function m.readObject(s) end
Coolape.B2InputStream = m
return m
