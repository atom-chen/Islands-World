---@class Coolape.Net : Coolape.Tcp
---@field public self Coolape.Net
---@field public lua Coolape.CLBaseLua
---@field public _SuccessCodeValue System.Int32
---@field public switchNetType Coolape.Net.NetWorkType
---@field public host4Publish System.String
---@field public host4Test1 System.String
---@field public host4Test2 System.String
---@field public serializeluaPath System.String
---@field public gatePort System.Int32
---@field public httpPort System.Int32
---@field public httpFunc System.String
---@field public SuccessCode System.Int32
---@field public gateHost System.String

local m = { }
---public Net .ctor()
---@return Net
function m.New() end
---public Void setLua()
function m:setLua() end
---public Void initSerializeFunc()
function m:initSerializeFunc() end
---public Void connect(String host, Int32 port)
---@param optional String host
---@param optional Int32 port
function m:connect(host, port) end
---public Void dispatchData(Object data, Tcp tcp)
---@param optional Object data
---@param optional Tcp tcp
function m:dispatchData(data, tcp) end
---public Byte[] encodeData(Object obj)
---@return table
---@param optional Object obj
function m:encodeData(obj) end
---public Object parseRecivedData(MemoryStream buffer)
---@return Object
---@param optional MemoryStream buffer
function m:parseRecivedData(buffer) end
---public Void Update()
function m:Update() end
Coolape.Net = m
return m
