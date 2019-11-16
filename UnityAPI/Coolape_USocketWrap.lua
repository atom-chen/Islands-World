---@class Coolape.USocket
---@field public host System.String
---@field public port System.Int32
---@field public mSocket System.Net.Sockets.Socket
---@field public mTmpBufferSize System.Int32
---@field public mTmpBuffer System.Byte
---@field public isActive System.Boolean
---@field public timeoutCheckTimer System.Threading.Timer
---@field public timeoutMSec System.Int32
---@field public maxTimeoutTimes System.Int32
local m = { }
---public USocket .ctor(String host, Int32 port)
---@return USocket
---@param optional String host
---@param optional Int32 port
function m.New(host, port) end
---public Void init(String host, Int32 port)
---@param optional String host
---@param optional Int32 port
function m:init(host, port) end
---public Void connectAsync(NetCallback onConnectStateChgCallback)
---@param optional NetCallback onConnectStateChgCallback
function m:connectAsync(onConnectStateChgCallback) end
---public Void close()
function m:close() end
---public Void ReceiveAsync(OnReceiveCallback callback)
---@param optional OnReceiveCallback callback
function m:ReceiveAsync(callback) end
---public Void SendAsync(Byte[] data)
---@param optional Byte[] data
function m:SendAsync(data) end
---public Void sendTimeOut(Object orgs)
---@param optional Object orgs
function m:sendTimeOut(orgs) end
Coolape.USocket = m
return m
