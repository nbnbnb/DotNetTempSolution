using CommonLib.Entities;
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
                return GetDayRange(checkDate);
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
            // cyclePoint 为起始天

            int year = checkDate.Year;
            int month = checkDate.Month;
            int day = checkDate.Day;

            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            checkDate = new DateTime(year, month, day);

            // 闰年处理
            int februaryDay = DateTime.IsLeapYear(year) ? 29 : 28;

            if (cyclePoint <= 28)
            {
                DateTime cycleDate = new DateTime(year, month, cyclePoint);

                if (checkDate < cycleDate) // 上一个月
                {
                    startDate = cycleDate.AddMonths(-1);
                    endDate = cycleDate.AddSeconds(-1);
                }
                else // 下一个月
                {
                    startDate = cycleDate;
                    endDate = cycleDate.AddMonths(1).AddSeconds(-1);
                }
            }

            // 对于 29,30,31 这类日期，需要特殊处理
            // 1.29~2.28,3.1~3.28,3.29~4.28,4.29~5.28,5.29~6.28,6.29~7.28,7.29~8.28,8.29~9.28,9.29~10.28,10.29~11.28,11.29~12.28,12.29~1.28
            //int[] days_start_29 = { 29, 1, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29 };
            //int[] days_end_29 = { februaryDay, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28 };

            // 1.30~2.28,3.1~3.29,3.30~4.29,4.30~5.29,5.30~6.29,6.30~7.29,7.30~8.29,8.30~9.29,9.30~10.29,10.30~11.29,11.30~12.29,12.30~1.29
            //int[] days_start_30 = { 30, 1, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 };
            //int[] days_end_30 = { februaryDay, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29 };

            // 1.31~2.28,3.1~3.30,3.31~4.30,5.1~5.30,5.31~6.30,7.1~7.30,7.31~8.30,8.31~9.30,10.1~10.30,10.31~11.30,12.1~12.30,12.31~1.30
            // int[] days_start_31 = { 31, 1, 31, 1, 31, 1, 31, 31, 1, 31, 1 };
            // int[] days_end_31 = { februaryDay, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 };

            #region 29
            if (cyclePoint == 29)
            {
                if (day < 29 && month == 1) // 跳转到上一年
                {
                    // 上年的  12.29 ~ 今年 1.28
                    startDate = new DateTime(year - 1, 12, 29);
                    endDate = new DateTime(year, 1, 28, 23, 59, 59);
                }
                else if ((month == 1 && day >= 29) || (month == 2 && day < 29))
                {
                    startDate = new DateTime(year, 1, 29);
                    endDate = new DateTime(year, 2, februaryDay, 23, 59, 59);
                }
                else if ((month == 2 && day >= 29) || (month == 3 && day < 29))
                {
                    startDate = new DateTime(year, 3, 1);
                    endDate = new DateTime(year, 3, 28, 23, 59, 59);
                }
                else if ((month == 3 && day >= 29) || (month == 4 && day < 29))
                {
                    startDate = new DateTime(year, 3, 29);
                    endDate = new DateTime(year, 4, 28, 23, 59, 59);
                }
                else if ((month == 4 && day >= 29) || (month == 5 && day < 29))
                {
                    startDate = new DateTime(year, 4, 29);
                    endDate = new DateTime(year, 5, 28, 23, 59, 59);
                }
                else if ((month == 5 && day >= 29) || (month == 6 && day < 29))
                {
                    startDate = new DateTime(year, 5, 29);
                    endDate = new DateTime(year, 6, 28, 23, 59, 59);
                }
                else if ((month == 6 && day >= 29) || (month == 7 && day < 29))
                {
                    startDate = new DateTime(year, 6, 29);
                    endDate = new DateTime(year, 7, 28, 23, 59, 59);
                }
                else if ((month == 7 && day >= 29) || (month == 8 && day < 29))
                {
                    startDate = new DateTime(year, 7, 29);
                    endDate = new DateTime(year, 8, 28, 23, 59, 59);
                }
                else if ((month == 8 && day >= 29) || (month == 9 && day < 29))
                {
                    startDate = new DateTime(year, 8, 29);
                    endDate = new DateTime(year, 9, 28, 23, 59, 59);
                }
                else if ((month == 9 && day >= 29) || (month == 10 && day < 29))
                {
                    startDate = new DateTime(year, 9, 29);
                    endDate = new DateTime(year, 10, 28, 23, 59, 59);
                }
                else if ((month == 10 && day >= 29) || (month == 11 && day < 29))
                {
                    startDate = new DateTime(year, 10, 29);
                    endDate = new DateTime(year, 11, 28, 23, 59, 59);
                }
                else if ((month == 11 && day >= 29) || (month == 12 && day < 29))
                {
                    startDate = new DateTime(year, 11, 29);
                    endDate = new DateTime(year, 12, 28, 23, 59, 59);
                }
                else if (month == 12 && day >= 29)
                {
                    // 下一年
                    startDate = new DateTime(year, 12, 29);
                    endDate = new DateTime(year + 1, 1, 28, 23, 59, 59);
                }
            }
            #endregion

            #region 30
            if (cyclePoint == 30)
            {
                if (day < 30 && month == 1) // 跳转到上一年
                {
                    // 上年的  12.30 ~ 今年 1.29
                    startDate = new DateTime(year - 1, 12, 30);
                    endDate = new DateTime(year, 1, 29, 23, 59, 59);
                }
                else if ((month == 1 && day >= 30) || (month == 2 && day < 30))
                {
                    startDate = new DateTime(year, 1, 30);
                    endDate = new DateTime(year, 2, februaryDay, 23, 59, 59);
                }
                else if ((month == 2 && day >= 30) || (month == 3 && day < 30))
                {
                    startDate = new DateTime(year, 3, 1);
                    endDate = new DateTime(year, 3, 29, 23, 59, 59);
                }
                else if ((month == 3 && day >= 30) || (month == 4 && day < 30))
                {
                    startDate = new DateTime(year, 3, 30);
                    endDate = new DateTime(year, 4, 29, 23, 59, 59);
                }
                else if ((month == 4 && day >= 30) || (month == 5 && day < 30))
                {
                    startDate = new DateTime(year, 4, 30);
                    endDate = new DateTime(year, 5, 29, 23, 59, 59);
                }
                else if ((month == 5 && day >= 30) || (month == 6 && day < 30))
                {
                    startDate = new DateTime(year, 5, 30);
                    endDate = new DateTime(year, 6, 29, 23, 59, 59);
                }
                else if ((month == 6 && day >= 30) || (month == 7 && day < 30))
                {
                    startDate = new DateTime(year, 6, 30);
                    endDate = new DateTime(year, 7, 29, 23, 59, 59);
                }
                else if ((month == 7 && day >= 30) || (month == 8 && day < 30))
                {
                    startDate = new DateTime(year, 7, 30);
                    endDate = new DateTime(year, 8, 29, 23, 59, 59);
                }
                else if ((month == 8 && day >= 30) || (month == 9 && day < 30))
                {
                    startDate = new DateTime(year, 8, 30);
                    endDate = new DateTime(year, 9, 29, 23, 59, 59);
                }
                else if ((month == 9 && day >= 30) || (month == 10 && day < 30))
                {
                    startDate = new DateTime(year, 9, 30);
                    endDate = new DateTime(year, 10, 29, 23, 59, 59);
                }
                else if ((month == 10 && day >= 30) || (month == 11 && day < 30))
                {
                    startDate = new DateTime(year, 10, 30);
                    endDate = new DateTime(year, 11, 29, 23, 59, 59);
                }
                else if ((month == 11 && day >= 30) || (month == 12 && day < 30))
                {
                    startDate = new DateTime(year, 11, 30);
                    endDate = new DateTime(year, 12, 29, 23, 59, 59);
                }
                else if (month == 12 && day >= 30)
                {
                    // 下一年
                    startDate = new DateTime(year, 12, 30);
                    endDate = new DateTime(year + 1, 1, 29, 23, 59, 59);
                }
            }
            #endregion

            #region 31
            if (cyclePoint == 31)
            {
                if (day < 31 && month == 1) // 跳转到上一年
                {
                    // 上年的  12.30 ~ 今年 1.29
                    startDate = new DateTime(year - 1, 12, 31);
                    endDate = new DateTime(year, 1, 30, 23, 59, 59);
                }
                else if ((month == 1 && day >= 31) || (month == 2 && day < 31))
                {
                    startDate = new DateTime(year, 1, 31);
                    endDate = new DateTime(year, 2, februaryDay, 23, 59, 59);
                }
                else if ((month == 2 && day >= 31) || (month == 3 && day < 31))
                {
                    startDate = new DateTime(year, 3, 1);
                    endDate = new DateTime(year, 3, 30, 23, 59, 59);
                }
                else if ((month == 3 && day >= 31) || (month == 4 && day < 31))
                {
                    startDate = new DateTime(year, 3, 31);
                    endDate = new DateTime(year, 4, 30, 23, 59, 59);
                }
                else if ((month == 4 && day >= 31) || (month == 5 && day < 31))
                {
                    startDate = new DateTime(year, 5, 1);
                    endDate = new DateTime(year, 5, 30, 23, 59, 59);
                }
                else if ((month == 5 && day >= 31) || (month == 6 && day < 31))
                {
                    startDate = new DateTime(year, 5, 31);
                    endDate = new DateTime(year, 6, 30, 23, 59, 59);
                }
                else if ((month == 6 && day >= 31) || (month == 7 && day < 31))
                {
                    startDate = new DateTime(year, 7, 1);
                    endDate = new DateTime(year, 7, 30, 23, 59, 59);
                }
                else if ((month == 7 && day >= 31) || (month == 8 && day < 31))
                {
                    startDate = new DateTime(year, 7, 31);
                    endDate = new DateTime(year, 8, 30, 23, 59, 59);
                }
                else if ((month == 8 && day >= 30) || (month == 9 && day < 30))
                {
                    startDate = new DateTime(year, 8, 31);
                    endDate = new DateTime(year, 9, 30, 23, 59, 59);
                }
                else if ((month == 9 && day >= 31) || (month == 10 && day < 31))
                {
                    startDate = new DateTime(year, 10, 1);
                    endDate = new DateTime(year, 10, 30, 23, 59, 59);
                }
                else if ((month == 10 && day >= 31) || (month == 11 && day < 31))
                {
                    startDate = new DateTime(year, 10, 31);
                    endDate = new DateTime(year, 11, 30, 23, 59, 59);
                }
                else if ((month == 11 && day >= 31) || (month == 12 && day < 31))
                {
                    startDate = new DateTime(year, 12, 1);
                    endDate = new DateTime(year, 12, 30, 23, 59, 59);
                }
                else if (month == 12 && day >= 31)
                {
                    // 下一年
                    startDate = new DateTime(year, 12, 31);
                    endDate = new DateTime(year + 1, 1, 30, 23, 59, 59);
                }
            }
            #endregion

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


            if (weekDay < cyclePoint)
            {
                endDate = checkDate.AddDays(cyclePoint - weekDay);
                startDate = endDate.AddDays(-6);
            }
            else
            {
                startDate = checkDate.AddDays(cyclePoint - weekDay);
                endDate = startDate.AddDays(6);
            }

            endDate = endDate.AddHours(23)
                  .AddMinutes(59)
                  .AddSeconds(59);

            return new Tuple<DateTime, DateTime>(startDate, endDate);
        }

        public static Tuple<DateTime, DateTime> GetDayRange(DateTime checkDate)
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
