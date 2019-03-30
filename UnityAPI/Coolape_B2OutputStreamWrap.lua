---@class Coolape.B2OutputStream

local m = { }
---public B2OutputStream .ctor()
---@return B2OutputStream
function m.New() end
---public Byte[] toBytes(Int32 v)
---@return table
---@param optional Int32 v
function m.toBytes(v) end
---public Void WriteByte(Stream os, Byte v)
---public Void WriteByte(Stream os, Int32 v)
---@param optional Stream os
---@param optional Int32 v
function m.WriteByte(os, v) end
---public Void writeNull(Stream os)
---@param optional Stream os
function m.writeNull(os) end
---public Void writeBoolean(Stream os, Boolean v)
---@param optional Stream os
---@param optional Boolean v
function m.writeBoolean(os, v) end
---public Void writeByte(Stream os, Int32 v)
---@param optional Stream os
---@param optional Int32 v
function m.writeByte(os, v) end
---public Void writeShort(Stream os, Int32 v)
---@param optional Stream os
---@param optional Int32 v
function m.writeShort(os, v) end
---public Void writeB2Int(Stream os, Int32 v)
---public Void writeB2Int(Stream os, B2Int v)
---@param optional Stream os
---@param optional B2Int v
function m.writeB2Int(os, v) end
---public Void writeInt(Stream os, Int32 v)
---@param optional Stream os
---@param optional Int32 v
function m.writeInt(os, v) end
---public Void writeIntArray(Stream os, Int32[] v)
---@param optional Stream os
---@param optional Int32[] v
function m.writeIntArray(os, v) end
---public Void writeInt2DArray(Stream os, Int32[][] v)
---@param optional Stream os
---@param optional Int32[][] v
function m.writeInt2DArray(os, v) end
---public Void writeLong(Stream os, Int64 v)
---@param optional Stream os
---@param optional Int64 v
function m.writeLong(os, v) end
---public Void writeDouble(Stream os, Double val)
---@param optional Stream os
---@param optional Double val
function m.writeDouble(os, val) end
---public Void writeString(Stream os, String v)
---@param optional Stream os
---@param optional String v
function m.writeString(os, v) end
---public Void writeBytes(Stream os, Byte[] v)
---@param optional Stream os
---@param optional Byte[] v
function m.writeBytes(os, v) end
---public Void writeVector(Stream os, ArrayList v)
---@param optional Stream os
---@param optional ArrayList v
function m.writeVector(os, v) end
---public Void writeMap(Stream os, Hashtable v)
---@param optional Stream os
---@param optional Hashtable v
function m.writeMap(os, v) end
---public Void writeObject(Stream os, Object obj)
---@param optional Stream os
---@param optional Object obj
function m.writeObject(os, obj) end
Coolape.B2OutputStream = m
return m
