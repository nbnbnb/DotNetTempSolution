using CommonLib.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicUnitTest
{
    [TestClass]
    public class CycleCalcHelperTest
    {

        //29 号
        //1.29~2.28,3.1~3.28,3.29~4.28,4.29~5.28,5.29~6.28,6.29~7.28,7.29~8.28,8.29~9.28,9.29~10.28,10.29~11.28,11.29~12.28,12.29~1.28

        //30 号
        //1.30~2.28,3.1~3.29,3.30~4.29,4.30~5.29,5.30~6.29,6.30~7.29,7.30~8.29,8.30~9.29,9.30~10.29,10.30~11.29,11.30~12.29,12.30~1.29

        //31 号
        //1.31~2.28,2.1~2.28,3.1~3.30,3.31~4.30,5.1~5.30,5.31~6.30,7.1~7.30,7.31~8.30,8.31~9.30,10.1~10.30,10.31~11.30,12.1~12.30,12.31~1.30

        //如果是闰年，则 2 月都以29号结束

        [TestMethod]
        public void MonthRangeTest_29()
        {
            //1.29~2.28,3.1~3.28,3.29~4.28,4.29~5.28,5.29~6.28,6.29~7.28,7.29~8.28,8.29~9.28,9.29~10.28,10.29~11.28,11.29~12.28,12.29~1.28

            // Arrange
            int cyclePoint = 29;  // 5 号之后的一个月数据都填入这个周期中
            DateTime checkDate_1 = new DateTime(2015, 1, 13);
            DateTime checkDate_2 = new DateTime(2015, 2, 13);
            DateTime checkDate_3 = new DateTime(2015, 3, 13);
            DateTime checkDate_4 = new DateTime(2015, 4, 13);
            DateTime checkDate_5 = new DateTime(2015, 5, 13);
            DateTime checkDate_6 = new DateTime(2015, 6, 13);
            DateTime checkDate_7 = new DateTime(2015, 7, 13);
            DateTime checkDate_8 = new DateTime(2015, 8, 13);
            DateTime checkDate_9 = new DateTime(2015, 9, 13);
            DateTime checkDate_10 = new DateTime(2015, 10, 13);
            DateTime checkDate_11 = new DateTime(2015, 11, 13);
            DateTime checkDate_12 = new DateTime(2015, 12, 13);
            DateTime checkDate_13 = new DateTime(2015, 12, 30);

            // Act
            var res_1 = CycleCalcHelper.GetRange(checkDate_1, cyclePoint, 3);
            var res_2 = CycleCalcHelper.GetRange(checkDate_2, cyclePoint, 3);
            var res_3 = CycleCalcHelper.GetRange(checkDate_3, cyclePoint, 3);
            var res_4 = CycleCalcHelper.GetRange(checkDate_4, cyclePoint, 3);
            var res_5 = CycleCalcHelper.GetRange(checkDate_5, cyclePoint, 3);
            var res_6 = CycleCalcHelper.GetRange(checkDate_6, cyclePoint, 3);
            var res_7 = CycleCalcHelper.GetRange(checkDate_7, cyclePoint, 3);
            var res_8 = CycleCalcHelper.GetRange(checkDate_8, cyclePoint, 3);
            var res_9 = CycleCalcHelper.GetRange(checkDate_9, cyclePoint, 3);
            var res_10 = CycleCalcHelper.GetRange(checkDate_10, cyclePoint, 3);
            var res_11 = CycleCalcHelper.GetRange(checkDate_11, cyclePoint, 3);
            var res_12 = CycleCalcHelper.GetRange(checkDate_12, cyclePoint, 3);
            var res_13 = CycleCalcHelper.GetRange(checkDate_13, cyclePoint, 3);

            // Assert
            Assert.AreEqual(res_1.Item1, new DateTime(2014, 12, 29));
            Assert.AreEqual(res_1.Item2, new DateTime(2015, 1, 28, 23, 59, 59));

            Assert.AreEqual(res_2.Item1, new DateTime(2015, 1, 29));
            Assert.AreEqual(res_2.Item2, new DateTime(2015, 2, 28, 23, 59, 59));

            Assert.AreEqual(res_3.Item1, new DateTime(2015, 3, 1));
            Assert.AreEqual(res_3.Item2, new DateTime(2015, 3, 28, 23, 59, 59));

            Assert.AreEqual(res_4.Item1, new DateTime(2015, 3, 29));
            Assert.AreEqual(res_4.Item2, new DateTime(2015, 4, 28, 23, 59, 59));

            Assert.AreEqual(res_5.Item1, new DateTime(2015, 4, 29));
            Assert.AreEqual(res_5.Item2, new DateTime(2015, 5, 28, 23, 59, 59));

            Assert.AreEqual(res_6.Item1, new DateTime(2015, 5, 29));
            Assert.AreEqual(res_6.Item2, new DateTime(2015, 6, 28, 23, 59, 59));

            Assert.AreEqual(res_7.Item1, new DateTime(2015, 6, 29));
            Assert.AreEqual(res_7.Item2, new DateTime(2015, 7, 28, 23, 59, 59));

            Assert.AreEqual(res_8.Item1, new DateTime(2015, 7, 29));
            Assert.AreEqual(res_8.Item2, new DateTime(2015, 8, 28, 23, 59, 59));

            Assert.AreEqual(res_9.Item1, new DateTime(2015, 8, 29));
            Assert.AreEqual(res_9.Item2, new DateTime(2015, 9, 28, 23, 59, 59));

            Assert.AreEqual(res_10.Item1, new DateTime(2015, 9, 29));
            Assert.AreEqual(res_10.Item2, new DateTime(2015, 10, 28, 23, 59, 59));

            Assert.AreEqual(res_11.Item1, new DateTime(2015, 10, 29));
            Assert.AreEqual(res_11.Item2, new DateTime(2015, 11, 28, 23, 59, 59));

            Assert.AreEqual(res_12.Item1, new DateTime(2015, 11, 29));
            Assert.AreEqual(res_12.Item2, new DateTime(2015, 12, 28, 23, 59, 59));

            Assert.AreEqual(res_13.Item1, new DateTime(2015, 12, 29));
            Assert.AreEqual(res_13.Item2, new DateTime(2016, 1, 28, 23, 59, 59));
        }

        [TestMethod]
        public void MonthRangeTest_30()
        {
            // 1.30~2.28,3.1~3.29,3.30~4.29,4.30~5.29,5.30~6.29,6.30~7.29,7.30~8.29,8.30~9.29,9.30~10.29,10.30~11.29,11.30~12.29,12.30~1.29

            // Arrange
            int cyclePoint = 30;  // 5 号之后的一个月数据都填入这个周期中
            DateTime checkDate_1 = new DateTime(2015, 1, 13);
            DateTime checkDate_2 = new DateTime(2015, 2, 13);
            DateTime checkDate_3 = new DateTime(2015, 3, 13);
            DateTime checkDate_4 = new DateTime(2015, 4, 13);
            DateTime checkDate_5 = new DateTime(2015, 5, 13);
            DateTime checkDate_6 = new DateTime(2015, 6, 13);
            DateTime checkDate_7 = new DateTime(2015, 7, 13);
            DateTime checkDate_8 = new DateTime(2015, 8, 13);
            DateTime checkDate_9 = new DateTime(2015, 9, 13);
            DateTime checkDate_10 = new DateTime(2015, 10, 13);
            DateTime checkDate_11 = new DateTime(2015, 11, 13);
            DateTime checkDate_12 = new DateTime(2015, 12, 13);
            DateTime checkDate_13 = new DateTime(2015, 12, 31);

            // Act
            var res_1 = CycleCalcHelper.GetRange(checkDate_1, cyclePoint, 3);
            var res_2 = CycleCalcHelper.GetRange(checkDate_2, cyclePoint, 3);
            var res_3 = CycleCalcHelper.GetRange(checkDate_3, cyclePoint, 3);
            var res_4 = CycleCalcHelper.GetRange(checkDate_4, cyclePoint, 3);
            var res_5 = CycleCalcHelper.GetRange(checkDate_5, cyclePoint, 3);
            var res_6 = CycleCalcHelper.GetRange(checkDate_6, cyclePoint, 3);
            var res_7 = CycleCalcHelper.GetRange(checkDate_7, cyclePoint, 3);
            var res_8 = CycleCalcHelper.GetRange(checkDate_8, cyclePoint, 3);
            var res_9 = CycleCalcHelper.GetRange(checkDate_9, cyclePoint, 3);
            var res_10 = CycleCalcHelper.GetRange(checkDate_10, cyclePoint, 3);
            var res_11 = CycleCalcHelper.GetRange(checkDate_11, cyclePoint, 3);
            var res_12 = CycleCalcHelper.GetRange(checkDate_12, cyclePoint, 3);
            var res_13 = CycleCalcHelper.GetRange(checkDate_13, cyclePoint, 3);

            // Assert
            Assert.AreEqual(res_1.Item1, new DateTime(2014, 12, 30));
            Assert.AreEqual(res_1.Item2, new DateTime(2015, 1, 29, 23, 59, 59));

            Assert.AreEqual(res_2.Item1, new DateTime(2015, 1, 30));
            Assert.AreEqual(res_2.Item2, new DateTime(2015, 2, 28, 23, 59, 59));

            Assert.AreEqual(res_3.Item1, new DateTime(2015, 3, 1));
            Assert.AreEqual(res_3.Item2, new DateTime(2015, 3, 29, 23, 59, 59));

            Assert.AreEqual(res_4.Item1, new DateTime(2015, 3, 30));
            Assert.AreEqual(res_4.Item2, new DateTime(2015, 4, 29, 23, 59, 59));

            Assert.AreEqual(res_5.Item1, new DateTime(2015, 4, 30));
            Assert.AreEqual(res_5.Item2, new DateTime(2015, 5, 29, 23, 59, 59));

            Assert.AreEqual(res_6.Item1, new DateTime(2015, 5, 30));
            Assert.AreEqual(res_6.Item2, new DateTime(2015, 6, 29, 23, 59, 59));

            Assert.AreEqual(res_7.Item1, new DateTime(2015, 6, 30));
            Assert.AreEqual(res_7.Item2, new DateTime(2015, 7, 29, 23, 59, 59));

            Assert.AreEqual(res_8.Item1, new DateTime(2015, 7, 30));
            Assert.AreEqual(res_8.Item2, new DateTime(2015, 8, 29, 23, 59, 59));

            Assert.AreEqual(res_9.Item1, new DateTime(2015, 8, 30));
            Assert.AreEqual(res_9.Item2, new DateTime(2015, 9, 29, 23, 59, 59));

            Assert.AreEqual(res_10.Item1, new DateTime(2015, 9, 30));
            Assert.AreEqual(res_10.Item2, new DateTime(2015, 10, 29, 23, 59, 59));

            Assert.AreEqual(res_11.Item1, new DateTime(2015, 10, 30));
            Assert.AreEqual(res_11.Item2, new DateTime(2015, 11, 29, 23, 59, 59));

            Assert.AreEqual(res_12.Item1, new DateTime(2015, 11, 30));
            Assert.AreEqual(res_12.Item2, new DateTime(2015, 12, 29, 23, 59, 59));

            Assert.AreEqual(res_13.Item1, new DateTime(2015, 12, 30));
            Assert.AreEqual(res_13.Item2, new DateTime(2016, 1, 29, 23, 59, 59));
        }

        [TestMethod]
        public void MonthRangeTest_31()
        {
            // 1.31~2.28,3.1~3.30,3.31~4.30,5.1~5.30,5.31~6.30,7.1~7.30,7.31~8.30,8.31~9.30,10.1~10.30,10.31~11.30,12.1~12.30,12.31~1.30

            // Arrange
            int cyclePoint = 31;
            DateTime checkDate_1 = new DateTime(2015, 1, 13);
            DateTime checkDate_2 = new DateTime(2015, 2, 13);
            DateTime checkDate_3 = new DateTime(2015, 3, 13);
            DateTime checkDate_4 = new DateTime(2015, 4, 13);
            DateTime checkDate_5 = new DateTime(2015, 5, 13);
            DateTime checkDate_6 = new DateTime(2015, 6, 13);
            DateTime checkDate_7 = new DateTime(2015, 7, 13);
            DateTime checkDate_8 = new DateTime(2015, 8, 13);
            DateTime checkDate_9 = new DateTime(2015, 9, 13);
            DateTime checkDate_10 = new DateTime(2015, 10, 13);
            DateTime checkDate_11 = new DateTime(2015, 11, 13);
            DateTime checkDate_12 = new DateTime(2015, 12, 13);
            DateTime checkDate_13 = new DateTime(2015, 12, 31);

            // Act
            var res_1 = CycleCalcHelper.GetRange(checkDate_1, cyclePoint, 3);
            var res_2 = CycleCalcHelper.GetRange(checkDate_2, cyclePoint, 3);
            var res_3 = CycleCalcHelper.GetRange(checkDate_3, cyclePoint, 3);
            var res_4 = CycleCalcHelper.GetRange(checkDate_4, cyclePoint, 3);
            var res_5 = CycleCalcHelper.GetRange(checkDate_5, cyclePoint, 3);
            var res_6 = CycleCalcHelper.GetRange(checkDate_6, cyclePoint, 3);
            var res_7 = CycleCalcHelper.GetRange(checkDate_7, cyclePoint, 3);
            var res_8 = CycleCalcHelper.GetRange(checkDate_8, cyclePoint, 3);
            var res_9 = CycleCalcHelper.GetRange(checkDate_9, cyclePoint, 3);
            var res_10 = CycleCalcHelper.GetRange(checkDate_10, cyclePoint, 3);
            var res_11 = CycleCalcHelper.GetRange(checkDate_11, cyclePoint, 3);
            var res_12 = CycleCalcHelper.GetRange(checkDate_12, cyclePoint, 3);
            var res_13 = CycleCalcHelper.GetRange(checkDate_13, cyclePoint, 3);

            // Assert
            Assert.AreEqual(res_1.Item1, new DateTime(2014, 12, 31));
            Assert.AreEqual(res_1.Item2, new DateTime(2015, 1, 30, 23, 59, 59));

            Assert.AreEqual(res_2.Item1, new DateTime(2015, 1, 31));
            Assert.AreEqual(res_2.Item2, new DateTime(2015, 2, 28, 23, 59, 59));

            Assert.AreEqual(res_3.Item1, new DateTime(2015, 3, 1));
            Assert.AreEqual(res_3.Item2, new DateTime(2015, 3, 30, 23, 59, 59));

            Assert.AreEqual(res_4.Item1, new DateTime(2015, 3, 31));
            Assert.AreEqual(res_4.Item2, new DateTime(2015, 4, 30, 23, 59, 59));

            Assert.AreEqual(res_5.Item1, new DateTime(2015, 5, 1));
            Assert.AreEqual(res_5.Item2, new DateTime(2015, 5, 30, 23, 59, 59));

            Assert.AreEqual(res_6.Item1, new DateTime(2015, 5, 31));
            Assert.AreEqual(res_6.Item2, new DateTime(2015, 6, 30, 23, 59, 59));

            Assert.AreEqual(res_7.Item1, new DateTime(2015, 7, 1));
            Assert.AreEqual(res_7.Item2, new DateTime(2015, 7, 30, 23, 59, 59));

            Assert.AreEqual(res_8.Item1, new DateTime(2015, 7, 31));
            Assert.AreEqual(res_8.Item2, new DateTime(2015, 8, 30, 23, 59, 59));

            Assert.AreEqual(res_9.Item1, new DateTime(2015, 8, 31));
            Assert.AreEqual(res_9.Item2, new DateTime(2015, 9, 30, 23, 59, 59));

            Assert.AreEqual(res_10.Item1, new DateTime(2015, 10, 1));
            Assert.AreEqual(res_10.Item2, new DateTime(2015, 10, 30, 23, 59, 59));

            Assert.AreEqual(res_11.Item1, new DateTime(2015, 10, 31));
            Assert.AreEqual(res_11.Item2, new DateTime(2015, 11, 30, 23, 59, 59));

            Assert.AreEqual(res_12.Item1, new DateTime(2015, 12, 1));
            Assert.AreEqual(res_12.Item2, new DateTime(2015, 12, 30, 23, 59, 59));

            Assert.AreEqual(res_13.Item1, new DateTime(2015, 12, 31));
            Assert.AreEqual(res_13.Item2, new DateTime(2016, 1, 30, 23, 59, 59));
        }

        [TestMethod]
        public void WeekRangeTest_1()
        {
            // Arrange            
            DateTime checkDate = new DateTime(2015, 6, 11);  // 周四 weekDay=4
            // 0 代表周日
            int cyclePoint = 4;

            // Act
            Tuple<DateTime, DateTime> res = CycleCalcHelper.GetWeekRange(checkDate, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2015, 6, 11), res.Item1);
            Assert.AreEqual(new DateTime(2015, 6, 17, 23, 59, 59), res.Item2);
        }

        [TestMethod]
        public void WeekRangeTest_2()
        {
            // Arrange            
            DateTime checkDate = new DateTime(2015, 6, 6);
            // 0 代表周日
            int cyclePoint = 4;

            // Act
            Tuple<DateTime, DateTime> res = CycleCalcHelper.GetWeekRange(checkDate, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2015, 6, 4), res.Item1);
            Assert.AreEqual(new DateTime(2015, 6, 10, 23, 59, 59), res.Item2);
        }

        [TestMethod]
        public void DayRangeTest()
        {
             // Arrange            
            DateTime checkDate = new DateTime(2015, 6, 6);

            // Act
            Tuple<DateTime, DateTime> res = CycleCalcHelper.GetDayRange(checkDate);

            // Assert            
            Assert.AreEqual(new DateTime(2015, 6, 6), res.Item1);
            Assert.AreEqual(new DateTime(2015, 6, 6, 23, 59, 59), res.Item2);
        }
    }
}
