---@class Coolape.NumEx
---@field public BYTE_MIN_VALUE System.SByte
---@field public BYTE_MAX_VALUE System.SByte
---@field public SHORT_MIN_VALUE System.Int16
---@field public SHORT_MAX_VALUE System.Int16
---@field public INT_MIN_VALUE System.Int64
---@field public INT_MAX_VALUE System.Int64
---@field public LONG_MIN_VALUE System.Double
---@field public LONG_MAX_VALUE System.Double
---@field public KB System.Int64
---@field public MB System.Int64
---@field public GB System.Int64
---@field public TB System.Int64
---@field public PB System.Int64

local m = { }
---public NumEx .ctor()
---@return NumEx
function m.New() end
---public Random _rnd()
---@return Random
function m._rnd() end
---public Boolean stringToBool(String s)
---@return bool
---@param optional String s
function m.stringToBool(s) end
---public Int32 stringToInt(String s)
---@return number
---@param optional String s
function m.stringToInt(s) end
---public Int64 stringToLong(String s)
---@return long
---@param optional String s
function m.stringToLong(s) end
---public Double stringToDouble(String s)
---@return number
---@param optional String s
function m.stringToDouble(s) end
---public Int32 NextInt(Int32 max)
---public Int32 NextInt(Int32 min, Int32 max)
---@return number
---@param Int32 min
---@param optional Int32 max
function m.NextInt(min, max) end
---public Boolean NextBool()
---public Boolean NextBool(Double probability)
---public Boolean NextBool(Double v, Int32 max)
---public Boolean NextBool(Int32 v, Int32 max)
---@return bool
---@param Int32 v
---@param Int32 max
function m.NextBool(v, max) end
---public String Next(String[] arrays)
---public Int32 Next(Int32[] arrays)
---public Object Next(ArrayList list)
---@return String
---@param optional ArrayList list
function m.Next(list) end
---public Int32 percent(Double v, Double max)
---@return number
---@param optional Double v
---@param optional Double max
function m.percent(v, max) end
---public Double Min(Double[] arrays)
---public Int32 Min(ArrayList arrays)
---public Int32 Min(Int32[] arrays)
---@return number
---@param optional Int32[] arrays
function m.Min(arrays) end
---public Double Max(Double[] arrays)
---public Int32 Max(ArrayList arrays)
---public Int32 Max(Int32[] arrays)
---@return number
---@param optional Int32[] arrays
function m.Max(arrays) end
---public String nStr(Int64 n, Int64 lMax)
---@return String
---@param optional Int64 n
---@param optional Int64 lMax
function m.nStr(n, lMax) end
---public String nStrForLen(String str, Int32 len)
---public String nStrForLen(Int32 n, Int32 len)
---@return String
---@param optional Int32 n
---@param optional Int32 len
function m.nStrForLen(n, len) end
---public Int32 readByte(Stream input)
---@return number
---@param optional Stream input
function m.readByte(input) end
---public Boolean readBool(Stream input)
---@return bool
---@param optional Stream input
function m.readBool(input) end
---public Char readChar(Stream input)
---@return number
---@param optional Stream input
function m.readChar(input) end
---public Int16 readShort(Stream input)
---@return number
---@param optional Stream input
function m.readShort(input) end
---public Int32 readInt(Stream input)
---@return number
---@param optional Stream input
function m.readInt(input) end
---public Int64 readLong(Stream input)
---@return long
---@param optional Stream input
function m.readLong(input) end
---public Double Int64BitsToDouble(Int64 v)
---@return number
---@param optional Int64 v
function m.Int64BitsToDouble(v) end
---public Int64 DoubleToInt64Bits(Double v)
---@return long
---@param optional Double v
function m.DoubleToInt64Bits(v) end
---public Int32 kb(Int64 nb)
---public Int32 kb(Int32 nb)
---@return number
---@param optional Int32 nb
function m.kb(nb) end
---public Int32 mb(Int64 nb)
---public Int32 mb(Int32 nb)
---@return number
---@param optional Int32 nb
function m.mb(nb) end
---public Int32 gb(Int64 nb)
---public Int32 gb(Int32 nb)
---@return number
---@param optional Int32 nb
function m.gb(nb) end
---public Int32 tb(Int64 nb)
---public Int32 tb(Int32 nb)
---@return number
---@param optional Int32 nb
function m.tb(nb) end
---public Int32 pb(Int64 nb)
---public Int32 pb(Int32 nb)
---@return number
---@param optional Int32 nb
function m.pb(nb) end
---public Int32 toInt(Object v)
---@return number
---@param optional Object v
function m.toInt(v) end
---public Int32 bio2Int(Byte[] buff)
---@return number
---@param optional Byte[] buff
function m.bio2Int(buff) end
---public Byte[] int2Bio(Int32 v)
---@return table
---@param optional Int32 v
function m.int2Bio(v) end
---public Int64 bio2Long(Byte[] buff)
---@return long
---@param optional Byte[] buff
function m.bio2Long(buff) end
---public Byte[] Long2Bio(Int64 v)
---@return table
---@param optional Int64 v
function m.Long2Bio(v) end
---public Double bio2Double(Byte[] buff)
---@return number
---@param optional Byte[] buff
function m.bio2Double(buff) end
---public Byte[] Double2Bio(Double v)
---@return table
---@param optional Double v
function m.Double2Bio(v) end
---public Byte[] getB2Int(Int32 v)
---@return table
---@param optional Int32 v
function m.getB2Int(v) end
---public Int32 getIntPart(Single x)
---@return number
---@param optional Single x
function m.getIntPart(x) end
Coolape.NumEx = m
return m
