---@class Coolape.JSON
---@field public TOKEN_NONE System.Int32
---@field public TOKEN_CURLY_OPEN System.Int32
---@field public TOKEN_CURLY_CLOSE System.Int32
---@field public TOKEN_SQUARED_OPEN System.Int32
---@field public TOKEN_SQUARED_CLOSE System.Int32
---@field public TOKEN_COLON System.Int32
---@field public TOKEN_COMMA System.Int32
---@field public TOKEN_STRING System.Int32
---@field public TOKEN_NUMBER System.Int32
---@field public TOKEN_TRUE System.Int32
---@field public TOKEN_FALSE System.Int32
---@field public TOKEN_NULL System.Int32

local m = { }
---public JSON .ctor()
---@return JSON
function m.New() end
---public Hashtable DecodeMap(String json)
---@return Hashtable
---@param optional String json
function m.DecodeMap(json) end
---public ArrayList DecodeList(String json)
---@return ArrayList
---@param optional String json
function m.DecodeList(json) end
---public Object JsonDecode(String json)
---public Object JsonDecode(String json, Boolean& success)
---@return Object
---@param String json
---@param optional Boolean& success
function m.JsonDecode(json, success) end
---public String JsonEncode(Object json)
---@return String
---@param optional Object json
function m.JsonEncode(json) end
Coolape.JSON = m
return m
