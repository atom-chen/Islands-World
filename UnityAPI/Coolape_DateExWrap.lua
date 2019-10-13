---@class Coolape.DateEx
---@field public fmt_yyyy_MM_dd_HH_mm_ss System.String
---@field public fmt_yyyy_MM_dd_HH_mm_ss_fname System.String
---@field public fmt_MM_dd_HH_mm System.String
---@field public fmt_yyyy_MM_dd System.String
---@field public fmt_yyyyMMdd System.String
---@field public fmt_yyyyMMddHHmm System.String
---@field public fmt_HH_mm_ss System.String
---@field public TIME_MILLISECOND System.Int64
---@field public TIME_SECOND System.Int64
---@field public TIME_MINUTE System.Int64
---@field public TIME_HOUR System.Int64
---@field public TIME_DAY System.Int64
---@field public TIME_WEEK System.Int64
---@field public TIME_YEAR System.Int64
---@field public isFinishInit System.Boolean
---@field public begainTimeMs System.Int64
---@field public offsetSeconds System.Single
---@field public diffTimeWithServer System.Int64
---@field public now System.Int64
---@field public nowMS System.Int64
---@field public nowServerTime System.Int64
local m = { }
---public DateEx .ctor()
---@return DateEx
function m.New() end
---public Void init(Int64 serverTimeMs)
---@param optional Int64 serverTimeMs
function m.init(serverTimeMs) end
---public String format(String fmt)
---public String format(DateTime d, String fmt)
---@return String
---@param DateTime d
---@param optional String fmt
function m.format(d, fmt) end
---public String nowString()
---@return String
function m.nowString() end
---public String formatByMs(Int64 ms, String fmt)
---@return String
---@param optional Int64 ms
---@param optional String fmt
function m.formatByMs(ms, fmt) end
---public DateTime javaDate(Int64 ms)
---@return DateTime
---@param optional Int64 ms
function m.javaDate(ms) end
---public Int64 toJavaNTimeLong()
---@return long
function m.toJavaNTimeLong() end
---public Int64 toJavaDate(DateTime dat)
---@return long
---@param optional DateTime dat
function m.toJavaDate(dat) end
---public Int64 newDateLong(Int64 diffCSTime, Boolean isCellMS)
---@return long
---@param optional Int64 diffCSTime
---@param optional Boolean isCellMS
function m.newDateLong(diffCSTime, isCellMS) end
---public Int32[] getTimeArray(Int64 ms)
---@return table
---@param optional Int64 ms
function m.getTimeArray(ms) end
---public String toHHMMSS(Int64 ms)
---@return String
---@param optional Int64 ms
function m.toHHMMSS(ms) end
---public String toHHMMSS2(Int64 ms)
---@return String
---@param optional Int64 ms
function m.toHHMMSS2(ms) end
---public String toStrEn(Int64 ms)
---@return String
---@param optional Int64 ms
function m.toStrEn(ms) end
---public String toStrCn(Int64 ms)
---@return String
---@param optional Int64 ms
function m.toStrCn(ms) end
---public String ToTimeStr2(Int64 msec)
---@return String
---@param optional Int64 msec
function m.ToTimeStr2(msec) end
---public String ToTimeStr3(Int64 msec)
---@return String
---@param optional Int64 msec
function m.ToTimeStr3(msec) end
---public String ToTimeCost(Int64 msec)
---@return String
---@param optional Int64 msec
function m.ToTimeCost(msec) end
---public String ToTimeStr(Int64 msec)
---@return String
---@param optional Int64 msec
function m.ToTimeStr(msec) end
---public Int64 getLongJavaByHMS(String hms)
---@return long
---@param optional String hms
function m.getLongJavaByHMS(hms) end
---public String nowStrYyyyMMdd()
---@return String
function m.nowStrYyyyMMdd() end
---public String nxtStrYyyyMMdd()
---@return String
function m.nxtStrYyyyMMdd() end
---public Boolean isSameDateStr(String dateStr)
---@return bool
---@param optional String dateStr
function m.isSameDateStr(dateStr) end
---public String nowStrYyyyMMddHHmm()
---@return String
function m.nowStrYyyyMMddHHmm() end
---public String nxtStrYyyyMMddHHmm()
---@return String
function m.nxtStrYyyyMMddHHmm() end
---public Boolean isBeforeNow4yyMMddHHmm(String dateStr)
---@return bool
---@param optional String dateStr
function m.isBeforeNow4yyMMddHHmm(dateStr) end
---public Int64 getLongJavaByYMDHMS(String yyMMddHHmmss)
---@return long
---@param optional String yyMMddHHmmss
function m.getLongJavaByYMDHMS(yyMMddHHmmss) end
---public Int32 getWeek(Int32 year, Int32 month, Int32 day)
---@return number
---@param optional Int32 year
---@param optional Int32 month
---@param optional Int32 day
function m.getWeek(year, month, day) end
---public Int32 getMothDays(Int32 year, Int32 month)
---@return number
---@param optional Int32 year
---@param optional Int32 month
function m.getMothDays(year, month) end
---public Boolean isLeapYear(Int32 year)
---@return bool
---@param optional Int32 year
function m.isLeapYear(year) end
Coolape.DateEx = m
return m
