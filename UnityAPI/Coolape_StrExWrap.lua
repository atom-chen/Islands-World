---@class Coolape.StrEx
---@field public sb System.Text.StringBuilder
---@field public Length System.Int32

local m = { }
---public StrEx .ctor()
---public StrEx .ctor(StringBuilder sb)
---@return StrEx
---@param StringBuilder sb
function m.New(sb) end
---public StrEx builder()
---@return StrEx
function m.builder() end
---public StrEx Append(Double v)
---public StrEx Append(Int64 v)
---public StrEx Append(StringBuilder v)
---public StrEx Append(String v)
---public StrEx Append(Byte v)
---public StrEx Append(Boolean v)
---public StrEx Append(Int32 v)
---public StrEx Append(Int16 v)
---@return StrEx
---@param optional Int16 v
function m:Append(v) end
---public StrEx AppendLine()
---public StrEx AppendLine(String v)
---@return StrEx
---@param String v
function m:AppendLine(v) end
---public StrEx AppendFormat(String fmt, Object o)
---@return StrEx
---@param optional String fmt
---@param optional Object o
function m:AppendFormat(fmt, o) end
---public Int32 Count()
---@return number
function m:Count() end
---public StrEx Clear()
---@return StrEx
function m:Clear() end
---public String ToString()
---@return String
function m:ToString() end
---public String Left(Int32 len)
---public String Left(String s, Int32 len)
---@return String
---@param String s
---@param optional Int32 len
function m:Left(s, len) end
---public String Right(Int32 len)
---public String Right(String s, Int32 len)
---@return String
---@param String s
---@param optional Int32 len
function m:Right(s, len) end
---public String Mid(Int32 begin, Int32 len)
---public String Mid(String s, Int32 start)
---public String Mid(String s, Int32 start, Int32 len)
---@return String
---@param String s
---@param optional Int32 start
---@param optional Int32 len
function m:Mid(s, start, len) end
---public String mapToString(Hashtable map)
---@return String
---@param optional Hashtable map
function m.mapToString(map) end
---public String listToString(ArrayList list)
---@return String
---@param optional ArrayList list
function m.listToString(list) end
---public StrEx ap(String s)
---public StrEx ap(String fmt, Object[] args)
---@return StrEx
---@param String fmt
---@param optional Object[] args
function m:ap(fmt, args) end
---public StrEx pn()
---public StrEx pn(String s)
---public StrEx pn(String fmt, Object[] args)
---@return StrEx
---@param String fmt
---@param Object[] args
function m:pn(fmt, args) end
---public String format(String fmt, Object[] args)
---@return String
---@param optional String fmt
---@param optional Object[] args
function m.format(fmt, args) end
---public String make(String s, Hashtable param)
---@return String
---@param optional String s
---@param optional Hashtable param
function m.make(s, param) end
---public String msToTime(Int64 ms)
---@return String
---@param optional Int64 ms
function m.msToTime(ms) end
---public Boolean isIpv4(String ip)
---@return bool
---@param optional String ip
function m.isIpv4(ip) end
---public Int32[] ipv4(String ipv4)
---@return table
---@param optional String ipv4
function m.ipv4(ipv4) end
---public Boolean isMailAddr(String mail)
---@return bool
---@param optional String mail
function m.isMailAddr(mail) end
---public Int32 getStrLen(String str)
---@return number
---@param optional String str
function m.getStrLen(str) end
---public Int32 getStrLen4Trim(String str)
---@return number
---@param optional String str
function m.getStrLen4Trim(str) end
---public String trimStr(String str)
---@return String
---@param optional String str
function m.trimStr(str) end
---public String appendSpce(String str, Int32 totalLen)
---@return String
---@param optional String str
---@param optional Int32 totalLen
function m.appendSpce(str, totalLen) end
Coolape.StrEx = m
return m
