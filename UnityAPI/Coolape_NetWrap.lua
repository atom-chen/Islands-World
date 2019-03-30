---@class Coolape.Net : Coolape.CLBaseLua
---@field public self Coolape.Net
---@field public _SuccessCodeValue System.Int32
---@field public isReallyUseNet System.Boolean
---@field public switchNetType Coolape.Net.NetWorkType
---@field public host4Publish System.String
---@field public host4Test1 System.String
---@field public host4Test2 System.String
---@field public gatePort System.Int32
---@field public httpPort System.Int32
---@field public httpFunc System.String
---@field public host System.String
---@field public port System.Int32
---@field public gateTcp Coolape.Tcp
---@field public gameTcp Coolape.Tcp
---@field public netGateDataQueue System.Collections.Queue
---@field public netGameDataQueue System.Collections.Queue
---@field public SuccessCode System.Int32
---@field public gateHost System.String

local m = { }
---public Net .ctor()
---@return Net
function m.New() end
---public Void setLua()
function m:setLua() end
---public Void dispatchGate4Lua(Object obj, Tcp tcp)
---@param optional Object obj
---@param optional Tcp tcp
function m:dispatchGate4Lua(obj, tcp) end
---public Void dispatchGame4Lua(Object obj, Tcp tcp)
---@param optional Object obj
---@param optional Tcp tcp
function m:dispatchGame4Lua(obj, tcp) end
---public Void connectGate()
function m:connectGate() end
---public Void connectGame(String host, Int32 port)
---@param optional String host
---@param optional Int32 port
function m:connectGame(host, port) end
---public Void sendGate(Object data)
---@param optional Object data
function m:sendGate(data) end
---public Void send(Object data)
---@param optional Object data
function m:send(data) end
Coolape.Net = m
return m
