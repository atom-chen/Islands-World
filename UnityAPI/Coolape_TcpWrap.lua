---@class Coolape.Tcp : UnityEngine.MonoBehaviour
---@field public host System.String
---@field public port System.Int32
---@field public connected System.Boolean
---@field public serializeInMainThread System.Boolean
---@field public isStopping System.Boolean
---@field public __maxLen System.Int32
---@field public socket Coolape.USocket
---@field public CONST_Connect System.String
---@field public CONST_OutofNetConnect System.String
---@field public receivedDataQueue System.Collections.Queue
local m = { }
---public Tcp .ctor()
---@return Tcp
function m.New() end
---public Void init(String host, Int32 port)
---public Void init(String host, Int32 port, TcpDispatchDelegate dispatcher)
---@param String host
---@param optional Int32 port
---@param optional TcpDispatchDelegate dispatcher
function m:init(host, port, dispatcher) end
---public Void connect()
---public Void connect(Object obj)
---@param Object obj
function m:connect(obj) end
---public Void onConnectStateChg(USocket s, Object result)
---@param optional USocket s
---@param optional Object result
function m:onConnectStateChg(s, result) end
---public Void outofNetConnect(Int32 code, String msg)
---@param optional Int32 code
---@param optional String msg
function m:outofNetConnect(code, msg) end
---public Void outofLine(USocket s, Object obj)
---@param optional USocket s
---@param optional Object obj
function m:outofLine(s, obj) end
---public Void stop()
function m:stop() end
---public Boolean send(Object obj)
---@return bool
---@param optional Object obj
function m:send(obj) end
---public Object packMessage(Object obj)
---@return Object
---@param optional Object obj
function m:packMessage(obj) end
---public Byte[] encodeData(Object obj)
---@return table
---@param optional Object obj
function m:encodeData(obj) end
---public Void onReceive(USocket s, Byte[] bytes, Int32 len)
---@param optional USocket s
---@param optional Byte[] bytes
---@param optional Int32 len
function m:onReceive(s, bytes, len) end
---public IEnumerator wrapBuffer2Unpack()
---@return IEnumerator
function m:wrapBuffer2Unpack() end
---public Void unpackMsg(MemoryStream buffer)
---@param optional MemoryStream buffer
function m:unpackMsg(buffer) end
---public Object parseRecivedData(MemoryStream buffer)
---@return Object
---@param optional MemoryStream buffer
function m:parseRecivedData(buffer) end
---public Void enqueueData(Object obj)
---@param optional Object obj
function m:enqueueData(obj) end
---public Void Update()
function m:Update() end
Coolape.Tcp = m
return m
