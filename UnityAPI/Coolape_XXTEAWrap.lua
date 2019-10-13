---@class Coolape.XXTEA
---@field public key System.String
---@field public defaultKey System.Byte
local m = { }
---public XXTEA .ctor()
---@return XXTEA
function m.New() end
---public Byte[] encodeStr(String Data, String Key)
---@return table
---@param optional String Data
---@param optional String Key
function m.encodeStr(Data, Key) end
---public String decodeStr(Byte[] Data, String Key)
---@return String
---@param optional Byte[] Data
---@param optional String Key
function m.decodeStr(Data, Key) end
---public Byte[] Encrypt(Byte[] Data, Byte[] Key)
---public UInt32[] Encrypt(UInt32[] v, UInt32[] k)
---@return table
---@param optional UInt32[] v
---@param optional UInt32[] k
function m.Encrypt(v, k) end
---public Byte[] Decrypt(Byte[] Data, Byte[] Key)
---public UInt32[] Decrypt(UInt32[] v, UInt32[] k)
---@return table
---@param optional UInt32[] v
---@param optional UInt32[] k
function m.Decrypt(v, k) end
Coolape.XXTEA = m
return m
