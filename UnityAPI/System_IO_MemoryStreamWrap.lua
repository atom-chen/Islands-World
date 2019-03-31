---@class System.IO.MemoryStream : System.IO.Stream
---@field public CanRead System.Boolean
---@field public CanSeek System.Boolean
---@field public CanWrite System.Boolean
---@field public Capacity System.Int32
---@field public Length System.Int64
---@field public Position System.Int64

local m = { }
---public MemoryStream .ctor()
---public MemoryStream .ctor(Int32 capacity)
---public MemoryStream .ctor(Byte[] buffer)
---public MemoryStream .ctor(Byte[] buffer, Boolean writable)
---public MemoryStream .ctor(Byte[] buffer, Int32 index, Int32 count)
---public MemoryStream .ctor(Byte[] buffer, Int32 index, Int32 count, Boolean writable)
---public MemoryStream .ctor(Byte[] buffer, Int32 index, Int32 count, Boolean writable, Boolean publiclyVisible)
---@return MemoryStream
---@param Byte[] buffer
---@param Int32 index
---@param Int32 count
---@param Boolean writable
---@param Boolean publiclyVisible
function m.New(buffer, index, count, writable, publiclyVisible) end
---public Void Flush()
function m:Flush() end
---public Task FlushAsync(CancellationToken cancellationToken)
---@return Task
---@param optional CancellationToken cancellationToken
function m:FlushAsync(cancellationToken) end
---public Byte[] GetBuffer()
---@return table
function m:GetBuffer() end
---public Boolean TryGetBuffer(ArraySegment`1& buffer)
---@return bool
---@param optional ArraySegment`1& buffer
function m:TryGetBuffer(buffer) end
---public Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
---@return number
---@param optional Byte[] buffer
---@param optional Int32 offset
---@param optional Int32 count
function m:Read(buffer, offset, count) end
---public Task`1 ReadAsync(Byte[] buffer, Int32 offset, Int32 count, CancellationToken cancellationToken)
---@return Task`1
---@param optional Byte[] buffer
---@param optional Int32 offset
---@param optional Int32 count
---@param optional CancellationToken cancellationToken
function m:ReadAsync(buffer, offset, count, cancellationToken) end
---public Int32 ReadByte()
---@return number
function m:ReadByte() end
---public Task CopyToAsync(Stream destination, Int32 bufferSize, CancellationToken cancellationToken)
---@return Task
---@param optional Stream destination
---@param optional Int32 bufferSize
---@param optional CancellationToken cancellationToken
function m:CopyToAsync(destination, bufferSize, cancellationToken) end
---public Int64 Seek(Int64 offset, SeekOrigin loc)
---@return long
---@param optional Int64 offset
---@param optional SeekOrigin loc
function m:Seek(offset, loc) end
---public Void SetLength(Int64 value)
---@param optional Int64 value
function m:SetLength(value) end
---public Byte[] ToArray()
---@return table
function m:ToArray() end
---public Void Write(Byte[] buffer, Int32 offset, Int32 count)
---@param optional Byte[] buffer
---@param optional Int32 offset
---@param optional Int32 count
function m:Write(buffer, offset, count) end
---public Task WriteAsync(Byte[] buffer, Int32 offset, Int32 count, CancellationToken cancellationToken)
---@return Task
---@param optional Byte[] buffer
---@param optional Int32 offset
---@param optional Int32 count
---@param optional CancellationToken cancellationToken
function m:WriteAsync(buffer, offset, count, cancellationToken) end
---public Void WriteByte(Byte value)
---@param optional Byte value
function m:WriteByte(value) end
---public Void WriteTo(Stream stream)
---@param optional Stream stream
function m:WriteTo(stream) end
System.IO.MemoryStream = m
return m
