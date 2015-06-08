using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Helper
{
    public class CycleCalcHelper
    {

        public static Tuple<DateTime, DateTime> GetRange(DateTime checkDate, int cyclePoint, int cycleType)
        {
            if (cycleType == 1)  // 日结 
            {
                return GetDayRange(checkDate, cyclePoint);
            }
            else if (cycleType == 2) // 周结
            {
                return GetWeekRange(checkDate, cyclePoint);
            }
            else // 月结
            {
                return GetMonthRange(checkDate, cyclePoint);
            }
        }

        public static Tuple<DateTime, DateTime> GetMonthRange(DateTime checkDate, int cyclePoint)
        {
            int year = checkDate.Year;
            int month = checkDate.Month;
            int day = checkDate.Day;

            DateTime startDate;
            DateTime endDate;

            checkDate = new DateTime(year, month, day);

            // 当前月有多少天
            int dayCount_Current = (new DateTime(year, month, 1).AddMonths(1) -
                   new DateTime(year, month, 1)).Days;

            // 配置中的结算点
            int originCyclePoint = cyclePoint;

            // 最后计算开始日期
            // 对于特殊的结束日期【29,30,31】，需要重新计算
            // 计算上一个月有多少天
            int dayCount_Previous = (new DateTime(year, month, 1) - new DateTime(year, month, 1).AddMonths(-1)).Days;

            // 当前日期小于等于检测点
            // 周期跨到上一个月
            if (day <= cyclePoint)
            {

                // 例如 dayCount=28，cyclePoint=30
                // 如果超过了当前月的最大天数，则重置 CyclePoint
                // 然后计算正确的结束日期
                if (cyclePoint > dayCount_Current)
                {
                    cyclePoint = dayCount_Current;
                }
                endDate = checkDate.AddDays(cyclePoint - day);

                // 对于 29,30,31 这类特殊日期处理
                if (originCyclePoint > 28 && dayCount_Previous >= originCyclePoint)
                {
                    startDate = new DateTime(year, month, 1).AddMonths(-1).AddDays(originCyclePoint);
                }
                else
                {
                    startDate = endDate.AddMonths(-1).AddDays(1);
                }
            }
            // 当前日期大于检测点
            // 周期跨到下一个月
            else
            {
                startDate = checkDate.AddDays(cyclePoint - day).AddDays(1);
                endDate = startDate.AddMonths(1).AddDays(-1);

                // 对于 29,30,31 这类特殊日期处理
                // 计算结束日期
                if (originCyclePoint > 28 && dayCount_Previous >= originCyclePoint)
                {
                    endDate = startDate.AddMonths(1).AddDays(originCyclePoint - dayCount_Previous);
                }
                else
                {
                    endDate = startDate.AddMonths(1).AddDays(-1); // 原始方式
                }
            }

            endDate = endDate.AddHours(23)
                  .AddMinutes(59)
                  .AddSeconds(59);

            return new Tuple<DateTime, DateTime>(startDate, endDate);
        }

        public static Tuple<DateTime, DateTime> GetWeekRange(DateTime checkDate, int cyclePoint)
        {
            int weekDay = (int)checkDate.DayOfWeek;  // 周日为 0 

            int year = checkDate.Year;
            int month = checkDate.Month;
            int day = checkDate.Day;

            DateTime startDate;
            DateTime endDate;

            checkDate = new DateTime(year, month, day);


            if (weekDay <= cyclePoint)
            {
                endDate = checkDate.AddDays(cyclePoint - weekDay);
                startDate = endDate.AddDays(-6);
            }
            else
            {
                startDate = checkDate.AddDays(cyclePoint - weekDay).AddDays(1);
                endDate = startDate.AddDays(6);
            }

            endDate = endDate.AddHours(23)
                  .AddMinutes(59)
                  .AddSeconds(59);

            return new Tuple<DateTime, DateTime>(startDate, endDate);
        }

        public static Tuple<DateTime, DateTime> GetDayRange(DateTime checkDate, int cyclePoint)
        {
            int year = checkDate.Year;
            int month = checkDate.Month;
            int day = checkDate.Day;

            DateTime startDate = new DateTime(year, month, day);
            DateTime endDate = new DateTime(year, month, day, 23, 59, 59);

            return new Tuple<DateTime, DateTime>(startDate, endDate);
        }
    }
}
