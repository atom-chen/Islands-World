/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  日期工具
  *Others:  
  *History:
*********************************************************************************
*/

using System.Collections;
using System;
using System.Text;
using UnityEngine;

/*
 * Java统计从1970年1月1日起的毫秒的数量表示日期。也就是说，例如，1970年1月2日，是在1月1日后的86，400，000毫秒。同样的，1969年12月31日是在1970年1月1日前86，400，000毫秒。Java的Date类使用long类型纪录这些毫秒值.因为long是有符号整数，所以日期可以在1970年1月1日之前，也可以在这之后。Long类型表示的最大正值和最大负值可以轻松的表示290，000，000年的时间，这适合大多数人的时间要求。
 * Java中可以用System.currentTimeMillis() 获取当前时间的long形式，它的标示形式是从1970年1月1日起的到当前的毫秒的数。
 * C# 日期型数据的长整型值是自 0001 年 1 月 1 日午夜 12:00,以来所经过时间以100 毫微秒为间隔表示时的数字。这个数在 C# 的 DateTime 中被称为Ticks(刻度)。DateTime 类型有一个名为 Ticks 的长整型只读属性，就保存着这个值。
 * .NET下计算时间的方式不太一样，它是计算单位是Ticks，这里就需要做一个C#时间转换。关于Ticks，msdn上是这样说的： 
 * A single tick represents one hundred nanoseconds or one ten-millionth of a second. The value of this property represents the number of 100-nanosecond intervals that have elapsed since 12:00:00 midnight, January 1, 0001. 
 * 就是从公元元年元月1日午夜到指定时间的千万分之一秒了，为了和Java比较，说成万分之一毫秒。 
 * 需要注意的是这里是用的 System.DateTime.UtcNow 而不是 System.DateTime.Now ，因为我们在东八区，如果用后面那种形式就会发现时间会和想象当中的差了8个小时。Java与C#时间转换到这里就彻底实现了。 
 * 得到这些信息后，很容易写出将Java的长整型时间转化为C#时间。


long time_JAVA_Long = 1207969641193;//java长整型日期，毫秒为单位
DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度
long time_tricks = tricks_1970 + time_JAVA_Long * 10000;//日志日期刻度
DateTime dt = new DateTime(time_tricks);//转化为DateTime
*/
namespace Coolape
{
    public class DateEx
    {
        public const string fmt_yyyy_MM_dd_HH_mm_ss = "yyyy-MM-dd HH:mm:ss";
        public const string fmt_yyyy_MM_dd_HH_mm_ss_fname = "yyyy_MM_dd_HH_mm_ss";
        public const string fmt_MM_dd_HH_mm = "MM-dd HH:mm";
        public const string fmt_yyyy_MM_dd = "yyyy-MM-dd";
        public const string fmt_yyyyMMdd = "yyyyMMdd";
        public const string fmt_yyyyMMddHHmm = "yyyyMMddHHmm";
        public const string fmt_HH_mm_ss = "HH:mm:ss";
        public const long TIME_MILLISECOND = 1;
        public const long TIME_SECOND = 1000 * TIME_MILLISECOND;
        public const long TIME_MINUTE = 60 * TIME_SECOND;
        public const long TIME_HOUR = 60 * TIME_MINUTE;
        public const long TIME_DAY = 24 * TIME_HOUR;
        public const long TIME_WEEK = 7 * TIME_DAY;
        public const long TIME_YEAR = 365 * TIME_DAY;

        public static bool isFinishInit = false;
        public static long begainTimeMs = 0;
        public static float offsetSeconds = 0;
        static DateTime dat0 = new DateTime(1970, 1, 1);

        public static void init(long serverTimeMs = 0)
        {
            if (serverTimeMs == 0)
            {
                begainTimeMs = toJavaDate(DateTime.UtcNow);
            }
            else
            {
                begainTimeMs = serverTimeMs;
            }
            offsetSeconds = Time.realtimeSinceStartup;
            isFinishInit = true;
        }

        public static long now
        {
            get
            {
                if (!isFinishInit)
                    init();
                //				return DateTime.Now.ToFileTime ();
                return nowMS * 10000;
            }
        }

        public static long nowMS
        {
            get
            {
                if (!isFinishInit)
                    init();
                //				return DateTime.Now.ToFileTime () / 10000;
                return begainTimeMs + (long)((Time.realtimeSinceStartup - offsetSeconds) * 1000);
            }
        }

        public static string format(string fmt)
        {
            //			return format (DateTime.Now, fmt);
            return formatByMs(nowMS, fmt);
        }

        public static string nowString()
        {
            return format(fmt_yyyy_MM_dd_HH_mm_ss);
        }

        public static string format(DateTime d, string fmt)
        {
            return d.ToString(fmt);
        }


        public static string formatByMs(long ms, string fmt = fmt_yyyy_MM_dd_HH_mm_ss)
        {
            long us = ms * 10000 + dat0.Ticks;
            DateTime d = new DateTime(us);
            return d.ToLocalTime().ToString(fmt);
        }

        public static DateTime javaDate(long ms)
        {
            long tm = ms * 10000 + dat0.Ticks;
            return new DateTime(tm);
        }

        // 取得客户端当前时间
        static public long toJavaNTimeLong()
        {
            return toJavaDate(DateTime.UtcNow);
        }

        public static long toJavaDate(DateTime dat)
        {
            DateTime d2 = dat.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - dat0.Ticks);
            return (long)ts.TotalMilliseconds;

            /*long v = (dat.Ticks - dat0.Ticks) / 10000;
            return v;*/
        }
        //服务器同步时间diffCSTime:表示客服端与服务器端的时间差，isCellMS:表示到秒，毫秒往上收了一秒
        public static long newDateLong(long diffCSTime = 0, bool isCellMS = false)
        {
            long time = diffCSTime + toJavaNTimeLong();
            if (isCellMS)
            {
                double tmT = time / (double)TIME_SECOND;
                tmT = System.Math.Ceiling(tmT);
                time = (long)tmT * TIME_SECOND;
            }
            return time;
        }

        public static long diffTimeWithServer = 0;

        public static long nowServerTime
        {
            get
            {
                //				return diffTimeWithServer + toJavaNTimeLong ();
                return nowMS;
            }
        }

        // [0]=天，[1]=时，[2]=分，[3]=秒，[4]=毫秒
        static public int[] getTimeArray(long ms)
        {
            long tmpMs = ms;

            int ss = 1000;
            int mi = ss * 60;
            int hh = mi * 60;
            int dd = hh * 24;
            int day = 0, hour = 0, minute = 0, second = 0, milliSecond = 0;

            if (tmpMs > dd)
            {
                day = (int)(tmpMs / dd);
                tmpMs %= dd;
            }

            if (tmpMs > hh)
            {
                hour = (int)(tmpMs / hh);
                tmpMs %= hh;
            }

            if (tmpMs > mi)
            {
                minute = (int)(tmpMs / mi);
                tmpMs %= mi;
            }

            if (tmpMs > ss)
            {
                second = (int)(tmpMs / ss);
                tmpMs %= ss;
            }

            milliSecond = (int)tmpMs;

            return new int[] { day, hour, minute, second, milliSecond };
        }

        public static string toHHMMSS(long ms)
        {
            int[] ss = getTimeArray(ms);
            return PStr.b().a(ss[1]).a(":").a(ss[2]).a(":").a(ss[3]).e();
        }

        public static string toHHMMSS2(long ms)
        {
            int[] ss = getTimeArray(ms);
            return PStr.b().a(ss[1]).a(Localization.Get("UIHour")).a(ss[2]).a(Localization.Get("UIMinute")).a(ss[3]).a(Localization.Get("UISecond")).e();
        }

        // 时间格式化为:HH:mm:ss;
        public static string toStrEn(long ms)
        {
            int[] arr = getTimeArray(ms);
            int hour = arr[0] * 24 + arr[1];
            String strHour = "";
            String strMinute = "";
            String strSecond = "";
            if (hour > 0)
            {
                strHour = hour < 10 ? PStr.b().a("0").a(hour).e() : PStr.b().a("").a(hour).e();
                strHour = PStr.b().a(strHour).a(":").e();
            }
            int minute = arr[2];
            if (minute >= 0)
            {
                strMinute = minute < 10 ? PStr.b().a("0").a(minute).e() : PStr.b().a(minute).a("").e();
                strMinute = PStr.b().a(strMinute).a(":").e();
            }
            int second = arr[3];
            if (second >= 0)
            {
                strSecond = second < 10 ? PStr.b().a("0").a(second).e() : PStr.b().a(second).a("").e();
            }
            return PStr.b().a(strHour).a(strMinute).a(strSecond).e();
        }

        // 时间格式化为:HH时mm分ss秒;
        public static string toStrCn(long ms)
        {
            int[] arr = getTimeArray(ms);
            int hour = arr[0] * 24 + arr[1];
            String strHour = "";
            String strMinute = "";
            String strSecond = "";
            if (hour > 0)
            {
                strHour = hour < 10 ? PStr.b().a("0").a(hour).e() : PStr.b().a(hour).a("").e();
                strHour = PStr.b().a(strHour).a(Localization.Get("UIHour")).e();
            }
            int minute = arr[2];
            if (minute > 0)
            {
                strMinute = minute < 10 ? PStr.b().a("0").a(minute).e() : PStr.b().a(minute).a("").e();
                strMinute = PStr.b().a(strMinute).a(Localization.Get("UIMinute")).e();
            }
            int second = arr[3];
            if (second >= 0)
            {
                strSecond = second < 10 ? PStr.b().a("0").a(second).e() : PStr.b().a(second).a("").e();
                strSecond = PStr.b().a(strSecond).a(Localization.Get("UISecond")).e();
            }
            return PStr.b().a(strHour).a(strMinute).a(strSecond).e();
        }

        public static string ToTimeStr2(long msec)
        {
            // 将毫秒数换算成x天x时x分x秒x毫秒
            int day = 0, hour = 0, minute = 0, second = 0;
            string retstr = "";

            long remainder;
            day = (int)(msec / 86400000);
            retstr = (day == 0) ? "" : PStr.b().a(day).a(Localization.Get("UIDay")).e();

            remainder = msec % 86400000;
            if (remainder != 0)
            {
                hour = (int)remainder / 3600000;
            }
            //		hour += day * 24;
            //			retstr += ((retstr.Length > 0 || hour > 0) ? (hour < 10 ? "0" + hour + Localization.Get("UIHour") : hour + Localization.Get("UIHour")) : "");
            string hstr = ((retstr.Length > 0 || hour > 0) ? PStr.b().a(hour).a(Localization.Get("UIHour")).e() : "");
            retstr = PStr.b().a(retstr).a(hstr).e();

            remainder = remainder % 3600000;
            if (remainder != 0)
            {
                minute = (int)remainder / 60000;
            }
            //			retstr += ((retstr.Length > 0 || minute > 0) ? (minute < 10 ? "0" + minute + Localization.Get("UIMinute") : minute + Localization.Get("UIMinute")) : "00" + Localization.Get("UIMinute"));
            string mstr = ((retstr.Length > 0 || minute > 0) ? PStr.b().a(minute).a(Localization.Get("UIMinute")).e() : "");
            retstr = PStr.b().a(retstr).a(mstr).e();

            second = (int)remainder % 60000;
            second = second / 1000;
            retstr = PStr.b().a(retstr).a(second).a(Localization.Get("UISecond")).e();
            return retstr;
        }

        public static string ToTimeStr3(long msec)
        {
            // 将毫秒数换算成x天x时
            int day = 0, hour = 0, minute = 0, second = 0;
            string retstr = "";

            long remainder;
            day = (int)(msec / 86400000);
            retstr = (day == 0) ? "" : PStr.b().a(day).a(Localization.Get("UIDay")).e();

            remainder = msec % 86400000;
            if (remainder != 0)
            {
                hour = (int)remainder / 3600000;
            }
            string hstr = ((retstr.Length > 0 || hour > 0) ? PStr.b().a(hour).a(Localization.Get("UIHour")).e() : "");

            return PStr.b().a(retstr).a(hstr).e();
        }

        public static string ToTimeCost(long msec)
        {
            int day = 0, hour = 0, minute = 0, second = 0;
            string retstr = "";

            long remainder;
            day = (int)(msec / 86400000);
            retstr = (day == 0) ? "" : PStr.b().a(day).a(Localization.Get("DayBefore")).e();
            if (!string.IsNullOrEmpty(retstr))
            {
                return retstr;
            }

            remainder = msec % 86400000;
            if (remainder != 0)
            {
                hour = (int)remainder / 3600000;
            }
            //		hour += day * 24;
            string hstr = ((retstr.Length > 0 || hour > 0) ? PStr.b().a(hour).a(Localization.Get("HourBefore")).e() : "");
            retstr = PStr.b().a(retstr).a(hstr).e();
            if (!string.IsNullOrEmpty(retstr))
            {
                return retstr;
            }

            remainder = remainder % 3600000;
            if (remainder != 0)
            {
                minute = (int)remainder / 60000;
            }
            //			retstr += ((retstr.Length > 0 || minute > 0) ? (minute + Localization.Get("MinutesBefore")) : "0" + Localization.Get("MinutesBefore"));
            string mstr = ((retstr.Length > 0 || minute > 0) ? PStr.b().a(minute).a(Localization.Get("MinutesBefore")).e() : "");
            retstr = PStr.b().a(retstr).a(mstr).e();
            if (!string.IsNullOrEmpty(retstr))
            {
                return retstr;
            }

            second = (int)remainder % 60000;
            second = second / 1000;
            //			retstr += (second < 10 ? "0" + second + Localization.Get("SecondBefore") : second + Localization.Get("SecondBefore"));
            retstr = PStr.b().a(retstr).a(second).a(Localization.Get("SecondBefore")).e();
            return retstr;
        }

        public static string ToTimeStr(long msec)
        {
            // 将毫秒数换算成x天x时x分x秒x毫秒
            int day = 0, hour = 0, minute = 0, second = 0;
            string retstr = "";

            long remainder;
            day = (int)(msec / 86400000);
            retstr = (day == 0) ? "" : PStr.b().a(day).a(":").e();

            remainder = msec % 86400000;
            if (remainder != 0)
            {
                hour = (int)remainder / 3600000;
            }
            hour += day * 24;
            string hstr = ((retstr.Length > 0 || hour > 0) ? (hour < 10 ? PStr.b().a("0").a(hour).a(":").e() : PStr.b().a(hour).a(":").e()) : "");
            retstr = PStr.b().a(retstr).a(hstr).e();

            remainder = remainder % 3600000;
            if (remainder != 0)
            {
                minute = (int)remainder / 60000;
            }
            string mstr = ((retstr.Length > 0 || minute > 0) ? (minute < 10 ? PStr.b().a("0").a(minute).a(":").e() : PStr.b().a(minute, ":").e()) : "00:");
            retstr = PStr.b().a(retstr).a(mstr).e();

            second = (int)remainder % 60000;
            second = second / 1000;
            string sstr = (second < 10 ? PStr.b().a("0", second).e() : PStr.b().a(second, "").e());
            retstr = PStr.b().a(retstr).a(sstr).e();
            return retstr;
        }

        static public long getLongJavaByHMS(string hms)
        {
            hms = hms.Replace("\\\\", "");
            string yyMMddHHmmss = format(fmt_yyyy_MM_dd) + " " + hms;
            return getLongJavaByYMDHMS(yyMMddHHmmss);
        }

        static public string nowStrYyyyMMdd()
        {
            return format(fmt_yyyyMMdd);
        }

        static public string nxtStrYyyyMMdd()
        {
            DateTime dt = DateTime.Now;
            DateTime nxtDt = dt.AddDays(1);
            return format(nxtDt, fmt_yyyyMMdd);
        }

        static public bool isSameDateStr(String dateStr)
        {
            if (string.IsNullOrEmpty(dateStr))
                return false;
            string nowStr = nowStrYyyyMMdd();
            int v = nowStr.CompareTo(dateStr);
            bool flag = v > -1;
            return flag;
        }


        static public string nowStrYyyyMMddHHmm()
        {
            return format(fmt_yyyyMMddHHmm);
        }

        static public string nxtStrYyyyMMddHHmm()
        {
            DateTime dt = DateTime.Now;
            DateTime nxtDt = dt.AddMinutes(1);
            return format(nxtDt, fmt_yyyyMMddHHmm);
        }

        static public bool isBeforeNow4yyMMddHHmm(String dateStr)
        {
            if (string.IsNullOrEmpty(dateStr))
                return false;
            string nowStr = nowStrYyyyMMddHHmm();
            int v = nowStr.CompareTo(dateStr);
            bool flag = v > -1;
            return flag;
        }

        static public long getLongJavaByYMDHMS(string yyMMddHHmmss)
        {
            try
            {
                yyMMddHHmmss = yyMMddHHmmss.Replace("\\\\", "");
                DateTime dt = DateTime.Parse(yyMMddHHmmss);
                long jl = toJavaDate(dt);
                //				return jl + diffTimeWithServer;
                return jl;
            }
            catch (Exception)
            {

                return 0;
            }
        }


        //
        public static int getWeek(int year, int month, int day)
        {
            DateTime dt = new DateTime(year, month, day);
            string weekstr = dt.DayOfWeek.ToString();
            int w = 0;
            switch (weekstr)
            {
                case "Monday":
                    //                    weekstr = "星期一"; 
                    w = 1;
                    break;
                case "Tuesday":
                    //                    weekstr = "星期二"; 
                    w = 2;
                    break;
                case "Wednesday":
                    //                    weekstr = "星期三"; 
                    w = 3;
                    break;
                case "Thursday":
                    //                    weekstr = "星期四"; 
                    w = 4;
                    break;
                case "Friday":
                    //                    weekstr = "星期五"; 
                    w = 5;
                    break;
                case "Saturday":
                    //                    weekstr = "星期六"; 
                    w = 6;
                    break;
                case "Sunday":
                    //                    weekstr = "星期日"; 
                    w = 0;
                    break;
            }
            return w;
        }

        // 取得某年某月有几天
        public static int getMothDays(int year, int month)
        {
            int Result = 30;
            if (month == 1 ||
                month == 3 ||
                month == 5 ||
                month == 7 ||
                month == 8 ||
                month == 10 ||
                month == 12)
            {
                Result = 31;
            }
            else if (month == 2)
            {
                if (isLeapYear(year))
                {
                    Result = 29;
                }
                else
                {
                    Result = 28;
                }
            }
            return Result;
        }

        // 是否闰年
        public static bool isLeapYear(int year)
        {
            if ((year % 4 == 0 && year % 100 != 0) ||
                (year % 400 == 0))
            {
                return true;
            }
            return false;
        }
    }
}
