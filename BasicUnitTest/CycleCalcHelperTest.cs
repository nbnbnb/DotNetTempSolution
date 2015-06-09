using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonLib.Helper;

namespace BasicUnitTest
{
    [TestClass]
    public class CycleCalcHelperTest
    {
        [TestMethod]
        public void GetRange_Type_01()
        {
            // 当前日期大于检测点
            // 周期跨到下一个月

            // Arrange            
            DateTime checkDate = new DateTime(2015, 1, 18);
            int cyclePoint = 16;

            // Act
            Tuple<DateTime, DateTime> res = CycleCalcHelper.GetMonthRange(checkDate, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2015, 1, 17), res.Item1);  // Start
            Assert.AreEqual(new DateTime(2015, 2, 16, 23, 59, 59), res.Item2);  // End
        }

        [TestMethod]
        public void GetRange_Type_02()
        {
            // 当前日期等于检测点
            // 周期跨到上一个月

            // Arrange            
            DateTime checkDate = new DateTime(2015, 1, 18);
            int cyclePoint = 18;

            // Act
            Tuple<DateTime, DateTime> res = CycleCalcHelper.GetMonthRange(checkDate, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2014, 12, 19), res.Item1);
            Assert.AreEqual(new DateTime(2015, 1, 18, 23, 59, 59), res.Item2);
        }

        [TestMethod]
        public void GetRange_Type_03()
        {
            // 当前日期小于检测点
            // 周期跨到上一个月

            // Arrange            
            DateTime checkDate = new DateTime(2015, 1, 18);
            int cyclePoint = 23;

            // Act
            Tuple<DateTime, DateTime> res = CycleCalcHelper.GetMonthRange(checkDate, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2014, 12, 24), res.Item1);
            Assert.AreEqual(new DateTime(2015, 1, 23, 23, 59, 59), res.Item2);
        }

        [TestMethod]
        public void GetRange_Type_04()
        {
            // 29 号测试 -1

            // 当前日期小于检测点
            // 周期跨到上一个月

            // Arrange            
            DateTime checkDate = new DateTime(2015, 1, 18);
            int cyclePoint = 29;

            // Act
            Tuple<DateTime, DateTime> res = CycleCalcHelper.GetMonthRange(checkDate, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2014, 12, 30), res.Item1);
            Assert.AreEqual(new DateTime(2015, 1, 29, 23, 59, 59), res.Item2);
        }

        [TestMethod]
        public void GetRange_Type_05()
        {
            // 29 号测试 -2

            // 当前日期小于检测点
            // 周期跨到上一个月

            // Arrange            
            DateTime checkDate = new DateTime(2015, 2, 18);  // 此年2月份只有 28 天
            int cyclePoint = 29;

            // Act
            Tuple<DateTime, DateTime> res = CycleCalcHelper.GetMonthRange(checkDate, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2015, 1, 30), res.Item1);
            Assert.AreEqual(new DateTime(2015, 2, 28, 23, 59, 59), res.Item2);  // 此处结束日期应该为  28 号
        }

        [TestMethod]
        public void GetRange_Type_06()
        {
            // 29 号测试 -2

            // 当前日期小于检测点
            // 周期跨到上一个月

            // Arrange            
            DateTime checkDate = new DateTime(2012, 2, 18);  // 此年2月份只有 29 天【闰年】
            int cyclePoint = 29;

            // Act
            Tuple<DateTime, DateTime> res = CycleCalcHelper.GetMonthRange(checkDate, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2012, 1, 30), res.Item1);
            Assert.AreEqual(new DateTime(2012, 2, 29, 23, 59, 59), res.Item2);
        }

        [TestMethod]
        public void GetRange_Type_07()
        {
            // 31 号测试 

            // 当前日期小于检测点
            // 周期跨到上一个月

            // Arrange            
            DateTime checkDate = new DateTime(2015, 1, 18);  // 31 天
            int cyclePoint = 31;

            // Act
            Tuple<DateTime, DateTime> res = CycleCalcHelper.GetMonthRange(checkDate, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2015, 1, 1), res.Item1);
            Assert.AreEqual(new DateTime(2015, 1, 31, 23, 59, 59), res.Item2);
        }

        [TestMethod]
        public void GetRange_Type_08()
        {
            // 30 号测试 

            // 当前日期小于检测点
            // 周期跨到上一个月

            // Arrange            
            DateTime checkDate = new DateTime(2015, 4, 18);  // 30 天
            int cyclePoint = 30;

            // Act
            Tuple<DateTime, DateTime> res = CycleCalcHelper.GetMonthRange(checkDate, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2015, 3, 31), res.Item1);
            Assert.AreEqual(new DateTime(2015, 4, 30, 23, 59, 59), res.Item2);
        }

        [TestMethod]
        public void GetRange_Type_09()
        {
            // cyclePoint<weekDay

            // Arrange            
            DateTime checkDate = new DateTime(2015, 6, 11);  // 周四
            // 0 代表周日
            int cyclePoint = 2;

            // Act
            Tuple<DateTime, DateTime> res = CycleCalcHelper.GetWeekRange(checkDate, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2015, 6, 10), res.Item1);
            Assert.AreEqual(new DateTime(2015, 6, 16, 23, 59, 59), res.Item2);
        }

        [TestMethod]
        public void GetRange_Type_10()
        {
            // cyclePoint<=weekDay

            // Arrange            
            DateTime checkDate = new DateTime(2015, 6, 11);  // 周四 weekDay=4
            // 0 代表周日
            int cyclePoint = 4;

            // Act
            Tuple<DateTime, DateTime> res = CycleCalcHelper.GetWeekRange(checkDate, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2015, 6, 5), res.Item1);
            Assert.AreEqual(new DateTime(2015, 6, 11, 23, 59, 59), res.Item2);
        }

        [TestMethod]
        public void GetRange_Type_11()
        {
            // cyclePoint>weekDay

            // Arrange            
            DateTime checkDate = new DateTime(2015, 6, 11);  // 周四 weekDay=4
            // 0 代表周日
            int cyclePoint = 6;

            // Act
            Tuple<DateTime, DateTime> res = CycleCalcHelper.GetWeekRange(checkDate, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2015, 6, 7), res.Item1);
            Assert.AreEqual(new DateTime(2015, 6, 13, 23, 59, 59), res.Item2);
        }

        [TestMethod]
        public void GetRange_Type_12()
        {
            // 对月份的综合测试

            // Arrange       

            // 日期全部在 25 号之前
            int cyclePoint = 25;

            DateTime checkDate_1 = new DateTime(2015, 1, 12);
            DateTime checkDate_2 = new DateTime(2015, 2, 18);
            DateTime checkDate_3 = new DateTime(2015, 3, 11);
            DateTime checkDate_4 = new DateTime(2015, 4, 19);
            DateTime checkDate_5 = new DateTime(2015, 5, 23);
            DateTime checkDate_6 = new DateTime(2015, 6, 1);
            DateTime checkDate_7 = new DateTime(2015, 7, 11);
            DateTime checkDate_8 = new DateTime(2015, 8, 22);
            DateTime checkDate_9 = new DateTime(2015, 9, 19);
            DateTime checkDate_10 = new DateTime(2015, 10, 10);
            DateTime checkDate_11 = new DateTime(2015, 11, 11);
            DateTime checkDate_12 = new DateTime(2015, 12, 18);
            DateTime checkDate_13 = new DateTime(2016, 1, 1);

            // Act
            Tuple<DateTime, DateTime> res_1 = CycleCalcHelper.GetMonthRange(checkDate_1, cyclePoint);
            Tuple<DateTime, DateTime> res_2 = CycleCalcHelper.GetMonthRange(checkDate_2, cyclePoint);
            Tuple<DateTime, DateTime> res_3 = CycleCalcHelper.GetMonthRange(checkDate_3, cyclePoint);
            Tuple<DateTime, DateTime> res_4 = CycleCalcHelper.GetMonthRange(checkDate_4, cyclePoint);
            Tuple<DateTime, DateTime> res_5 = CycleCalcHelper.GetMonthRange(checkDate_5, cyclePoint);
            Tuple<DateTime, DateTime> res_6 = CycleCalcHelper.GetMonthRange(checkDate_6, cyclePoint);
            Tuple<DateTime, DateTime> res_7 = CycleCalcHelper.GetMonthRange(checkDate_7, cyclePoint);
            Tuple<DateTime, DateTime> res_8 = CycleCalcHelper.GetMonthRange(checkDate_8, cyclePoint);
            Tuple<DateTime, DateTime> res_9 = CycleCalcHelper.GetMonthRange(checkDate_9, cyclePoint);
            Tuple<DateTime, DateTime> res_10 = CycleCalcHelper.GetMonthRange(checkDate_10, cyclePoint);
            Tuple<DateTime, DateTime> res_11 = CycleCalcHelper.GetMonthRange(checkDate_11, cyclePoint);
            Tuple<DateTime, DateTime> res_12 = CycleCalcHelper.GetMonthRange(checkDate_12, cyclePoint);
            Tuple<DateTime, DateTime> res_13 = CycleCalcHelper.GetMonthRange(checkDate_13, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2014, 12, 26), res_1.Item1);
            Assert.AreEqual(new DateTime(2015, 1, 25, 23, 59, 59), res_1.Item2);

            Assert.AreEqual(new DateTime(2015, 1, 26), res_2.Item1);
            Assert.AreEqual(new DateTime(2015, 2, 25, 23, 59, 59), res_2.Item2);

            Assert.AreEqual(new DateTime(2015, 2, 26), res_3.Item1);
            Assert.AreEqual(new DateTime(2015, 3, 25, 23, 59, 59), res_3.Item2);

            Assert.AreEqual(new DateTime(2015, 3, 26), res_4.Item1);
            Assert.AreEqual(new DateTime(2015, 4, 25, 23, 59, 59), res_4.Item2);

            Assert.AreEqual(new DateTime(2015, 4, 26), res_5.Item1);
            Assert.AreEqual(new DateTime(2015, 5, 25, 23, 59, 59), res_5.Item2);

            Assert.AreEqual(new DateTime(2015, 5, 26), res_6.Item1);
            Assert.AreEqual(new DateTime(2015, 6, 25, 23, 59, 59), res_6.Item2);

            Assert.AreEqual(new DateTime(2015, 6, 26), res_7.Item1);
            Assert.AreEqual(new DateTime(2015, 7, 25, 23, 59, 59), res_7.Item2);

            Assert.AreEqual(new DateTime(2015, 7, 26), res_8.Item1);
            Assert.AreEqual(new DateTime(2015, 8, 25, 23, 59, 59), res_8.Item2);

            Assert.AreEqual(new DateTime(2015, 8, 26), res_9.Item1);
            Assert.AreEqual(new DateTime(2015, 9, 25, 23, 59, 59), res_9.Item2);

            Assert.AreEqual(new DateTime(2015, 9, 26), res_10.Item1);
            Assert.AreEqual(new DateTime(2015, 10, 25, 23, 59, 59), res_10.Item2);

            Assert.AreEqual(new DateTime(2015, 10, 26), res_11.Item1);
            Assert.AreEqual(new DateTime(2015, 11, 25, 23, 59, 59), res_11.Item2);

            Assert.AreEqual(new DateTime(2015, 11, 26), res_12.Item1);
            Assert.AreEqual(new DateTime(2015, 12, 25, 23, 59, 59), res_12.Item2);

            Assert.AreEqual(new DateTime(2015, 12, 26), res_13.Item1);
            Assert.AreEqual(new DateTime(2016, 1, 25, 23, 59, 59), res_13.Item2);
        }


        [TestMethod]
        public void GetRange_Type_13()
        {
            // 对月份的综合测试

            // Arrange       

            // 日期全部在 3 号之后
            int cyclePoint = 3;

            DateTime checkDate_1 = new DateTime(2015, 1, 12);
            DateTime checkDate_2 = new DateTime(2015, 2, 18);
            DateTime checkDate_3 = new DateTime(2015, 3, 11);
            DateTime checkDate_4 = new DateTime(2015, 4, 19);
            DateTime checkDate_5 = new DateTime(2015, 5, 23);
            DateTime checkDate_6 = new DateTime(2015, 6, 7);
            DateTime checkDate_7 = new DateTime(2015, 7, 11);
            DateTime checkDate_8 = new DateTime(2015, 8, 22);
            DateTime checkDate_9 = new DateTime(2015, 9, 19);
            DateTime checkDate_10 = new DateTime(2015, 10, 10);
            DateTime checkDate_11 = new DateTime(2015, 11, 11);
            DateTime checkDate_12 = new DateTime(2015, 12, 18);
            DateTime checkDate_13 = new DateTime(2016, 1, 5);

            // Act
            Tuple<DateTime, DateTime> res_1 = CycleCalcHelper.GetMonthRange(checkDate_1, cyclePoint);
            Tuple<DateTime, DateTime> res_2 = CycleCalcHelper.GetMonthRange(checkDate_2, cyclePoint);
            Tuple<DateTime, DateTime> res_3 = CycleCalcHelper.GetMonthRange(checkDate_3, cyclePoint);
            Tuple<DateTime, DateTime> res_4 = CycleCalcHelper.GetMonthRange(checkDate_4, cyclePoint);
            Tuple<DateTime, DateTime> res_5 = CycleCalcHelper.GetMonthRange(checkDate_5, cyclePoint);
            Tuple<DateTime, DateTime> res_6 = CycleCalcHelper.GetMonthRange(checkDate_6, cyclePoint);
            Tuple<DateTime, DateTime> res_7 = CycleCalcHelper.GetMonthRange(checkDate_7, cyclePoint);
            Tuple<DateTime, DateTime> res_8 = CycleCalcHelper.GetMonthRange(checkDate_8, cyclePoint);
            Tuple<DateTime, DateTime> res_9 = CycleCalcHelper.GetMonthRange(checkDate_9, cyclePoint);
            Tuple<DateTime, DateTime> res_10 = CycleCalcHelper.GetMonthRange(checkDate_10, cyclePoint);
            Tuple<DateTime, DateTime> res_11 = CycleCalcHelper.GetMonthRange(checkDate_11, cyclePoint);
            Tuple<DateTime, DateTime> res_12 = CycleCalcHelper.GetMonthRange(checkDate_12, cyclePoint);
            Tuple<DateTime, DateTime> res_13 = CycleCalcHelper.GetMonthRange(checkDate_13, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2015, 1, 4), res_1.Item1);
            Assert.AreEqual(new DateTime(2015, 2, 3, 23, 59, 59), res_1.Item2);

            Assert.AreEqual(new DateTime(2015, 2, 4), res_2.Item1);
            Assert.AreEqual(new DateTime(2015, 3, 3, 23, 59, 59), res_2.Item2);

            Assert.AreEqual(new DateTime(2015, 3, 4), res_3.Item1);
            Assert.AreEqual(new DateTime(2015, 4, 3, 23, 59, 59), res_3.Item2);

            Assert.AreEqual(new DateTime(2015, 4, 4), res_4.Item1);
            Assert.AreEqual(new DateTime(2015, 5, 3, 23, 59, 59), res_4.Item2);

            Assert.AreEqual(new DateTime(2015, 5, 4), res_5.Item1);
            Assert.AreEqual(new DateTime(2015, 6, 3, 23, 59, 59), res_5.Item2);

            Assert.AreEqual(new DateTime(2015, 6, 4), res_6.Item1);
            Assert.AreEqual(new DateTime(2015, 7, 3, 23, 59, 59), res_6.Item2);

            Assert.AreEqual(new DateTime(2015, 7, 4), res_7.Item1);
            Assert.AreEqual(new DateTime(2015, 8, 3, 23, 59, 59), res_7.Item2);

            Assert.AreEqual(new DateTime(2015, 8, 4), res_8.Item1);
            Assert.AreEqual(new DateTime(2015, 9, 3, 23, 59, 59), res_8.Item2);

            Assert.AreEqual(new DateTime(2015, 9, 4), res_9.Item1);
            Assert.AreEqual(new DateTime(2015, 10, 3, 23, 59, 59), res_9.Item2);

            Assert.AreEqual(new DateTime(2015, 10, 4), res_10.Item1);
            Assert.AreEqual(new DateTime(2015, 11, 3, 23, 59, 59), res_10.Item2);

            Assert.AreEqual(new DateTime(2015, 11, 4), res_11.Item1);
            Assert.AreEqual(new DateTime(2015, 12, 3, 23, 59, 59), res_11.Item2);

            Assert.AreEqual(new DateTime(2015, 12, 4), res_12.Item1);
            Assert.AreEqual(new DateTime(2016, 1, 3, 23, 59, 59), res_12.Item2);

        }

        [TestMethod]
        public void GetRange_Type_14()
        {
            // 对月份的综合测试

            // Arrange       

            // 特殊日期点
            // 2015 年 2 月 有 28 天
            int cyclePoint = 29;

            DateTime checkDate_1 = new DateTime(2015, 1, 12);
            DateTime checkDate_2 = new DateTime(2015, 2, 18);
            DateTime checkDate_3 = new DateTime(2015, 3, 11);
            DateTime checkDate_4 = new DateTime(2015, 4, 19);
            DateTime checkDate_5 = new DateTime(2015, 5, 31);
            DateTime checkDate_6 = new DateTime(2015, 5, 30);
            DateTime checkDate_7 = new DateTime(2015, 5, 29);

            // Act
            Tuple<DateTime, DateTime> res_1 = CycleCalcHelper.GetMonthRange(checkDate_1, cyclePoint);
            Tuple<DateTime, DateTime> res_2 = CycleCalcHelper.GetMonthRange(checkDate_2, cyclePoint);
            Tuple<DateTime, DateTime> res_3 = CycleCalcHelper.GetMonthRange(checkDate_3, cyclePoint);
            Tuple<DateTime, DateTime> res_4 = CycleCalcHelper.GetMonthRange(checkDate_4, cyclePoint);
            Tuple<DateTime, DateTime> res_5 = CycleCalcHelper.GetMonthRange(checkDate_5, cyclePoint);
            Tuple<DateTime, DateTime> res_6 = CycleCalcHelper.GetMonthRange(checkDate_6, cyclePoint);
            Tuple<DateTime, DateTime> res_7 = CycleCalcHelper.GetMonthRange(checkDate_7, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2014, 12, 30), res_1.Item1);
            Assert.AreEqual(new DateTime(2015, 1, 29, 23, 59, 59), res_1.Item2);

            Assert.AreEqual(new DateTime(2015, 1, 30), res_2.Item1);
            Assert.AreEqual(new DateTime(2015, 2, 28, 23, 59, 59), res_2.Item2);

            Assert.AreEqual(new DateTime(2015, 3, 1), res_3.Item1);
            Assert.AreEqual(new DateTime(2015, 3, 29, 23, 59, 59), res_3.Item2);

            Assert.AreEqual(new DateTime(2015, 3, 30), res_4.Item1);
            Assert.AreEqual(new DateTime(2015, 4, 29, 23, 59, 59), res_4.Item2);

            Assert.AreEqual(new DateTime(2015, 5, 30), res_5.Item1);
            Assert.AreEqual(new DateTime(2015, 6, 29, 23, 59, 59), res_5.Item2);

            Assert.AreEqual(new DateTime(2015, 5, 30), res_6.Item1);
            Assert.AreEqual(new DateTime(2015, 6, 29, 23, 59, 59), res_6.Item2);

            Assert.AreEqual(new DateTime(2015, 4, 30), res_7.Item1);
            Assert.AreEqual(new DateTime(2015, 5, 29, 23, 59, 59), res_7.Item2);
        }

        [TestMethod]
        public void GetRange_Type_15()
        {
            // 对月份的综合测试

            // Arrange       

            // 特殊日期点
            // 2012 年 2 月 有 29 天 【闰年】
            int cyclePoint = 29;

            DateTime checkDate_1 = new DateTime(2012, 1, 12);
            DateTime checkDate_2 = new DateTime(2012, 2, 18);
            DateTime checkDate_3 = new DateTime(2012, 3, 11);
            DateTime checkDate_4 = new DateTime(2012, 4, 19);
            DateTime checkDate_5 = new DateTime(2012, 5, 31);
            DateTime checkDate_6 = new DateTime(2012, 5, 30);
            DateTime checkDate_7 = new DateTime(2012, 5, 29);

            // Act
            Tuple<DateTime, DateTime> res_1 = CycleCalcHelper.GetMonthRange(checkDate_1, cyclePoint);
            Tuple<DateTime, DateTime> res_2 = CycleCalcHelper.GetMonthRange(checkDate_2, cyclePoint);
            Tuple<DateTime, DateTime> res_3 = CycleCalcHelper.GetMonthRange(checkDate_3, cyclePoint);
            Tuple<DateTime, DateTime> res_4 = CycleCalcHelper.GetMonthRange(checkDate_4, cyclePoint);
            Tuple<DateTime, DateTime> res_5 = CycleCalcHelper.GetMonthRange(checkDate_5, cyclePoint);
            Tuple<DateTime, DateTime> res_6 = CycleCalcHelper.GetMonthRange(checkDate_6, cyclePoint);
            Tuple<DateTime, DateTime> res_7 = CycleCalcHelper.GetMonthRange(checkDate_7, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2011, 12, 30), res_1.Item1);
            Assert.AreEqual(new DateTime(2012, 1, 29, 23, 59, 59), res_1.Item2);

            Assert.AreEqual(new DateTime(2012, 1, 30), res_2.Item1);
            Assert.AreEqual(new DateTime(2012, 2, 29, 23, 59, 59), res_2.Item2);

            Assert.AreEqual(new DateTime(2012, 3, 1), res_3.Item1);
            Assert.AreEqual(new DateTime(2012, 3, 29, 23, 59, 59), res_3.Item2);

            Assert.AreEqual(new DateTime(2012, 3, 30), res_4.Item1);
            Assert.AreEqual(new DateTime(2012, 4, 29, 23, 59, 59), res_4.Item2);

            Assert.AreEqual(new DateTime(2012, 5, 30), res_5.Item1);
            Assert.AreEqual(new DateTime(2012, 6, 29, 23, 59, 59), res_5.Item2);

            Assert.AreEqual(new DateTime(2012, 5, 30), res_6.Item1);
            Assert.AreEqual(new DateTime(2012, 6, 29, 23, 59, 59), res_6.Item2);

            Assert.AreEqual(new DateTime(2012, 4, 30), res_7.Item1);
            Assert.AreEqual(new DateTime(2012, 5, 29, 23, 59, 59), res_7.Item2);
        }

        [TestMethod]
        public void GetRange_Type_16()
        {
            // 对月份的综合测试

            // Arrange       

            // 特殊日期点
            // 2012 年 2 月 有 29 天 【闰年】
            int cyclePoint = 30;

            DateTime checkDate_1 = new DateTime(2012, 1, 12);
            DateTime checkDate_2 = new DateTime(2012, 2, 18);
            DateTime checkDate_3 = new DateTime(2012, 3, 11);
            DateTime checkDate_4 = new DateTime(2012, 4, 19);
            DateTime checkDate_5 = new DateTime(2012, 5, 31);
            DateTime checkDate_6 = new DateTime(2012, 5, 30);
            DateTime checkDate_7 = new DateTime(2012, 5, 29);

            // Act
            Tuple<DateTime, DateTime> res_1 = CycleCalcHelper.GetMonthRange(checkDate_1, cyclePoint);
            Tuple<DateTime, DateTime> res_2 = CycleCalcHelper.GetMonthRange(checkDate_2, cyclePoint);
            Tuple<DateTime, DateTime> res_3 = CycleCalcHelper.GetMonthRange(checkDate_3, cyclePoint);
            Tuple<DateTime, DateTime> res_4 = CycleCalcHelper.GetMonthRange(checkDate_4, cyclePoint);
            Tuple<DateTime, DateTime> res_5 = CycleCalcHelper.GetMonthRange(checkDate_5, cyclePoint);
            Tuple<DateTime, DateTime> res_6 = CycleCalcHelper.GetMonthRange(checkDate_6, cyclePoint);
            Tuple<DateTime, DateTime> res_7 = CycleCalcHelper.GetMonthRange(checkDate_7, cyclePoint);

            Assert.AreEqual(new DateTime(2011, 12, 31), res_1.Item1);
            Assert.AreEqual(new DateTime(2012, 1, 30, 23, 59, 59), res_1.Item2);

            Assert.AreEqual(new DateTime(2012, 1, 31), res_2.Item1);
            Assert.AreEqual(new DateTime(2012, 2, 29, 23, 59, 59), res_2.Item2);

            Assert.AreEqual(new DateTime(2012, 3, 1), res_3.Item1);
            Assert.AreEqual(new DateTime(2012, 3, 30, 23, 59, 59), res_3.Item2);

            Assert.AreEqual(new DateTime(2012, 3, 31), res_4.Item1);
            Assert.AreEqual(new DateTime(2012, 4, 30, 23, 59, 59), res_4.Item2);

            Assert.AreEqual(new DateTime(2012, 5, 31), res_5.Item1);
            Assert.AreEqual(new DateTime(2012, 6, 30, 23, 59, 59), res_5.Item2);

            Assert.AreEqual(new DateTime(2012, 5, 1), res_6.Item1);
            Assert.AreEqual(new DateTime(2012, 5, 30, 23, 59, 59), res_6.Item2);

            Assert.AreEqual(new DateTime(2012, 5, 1), res_7.Item1);
            Assert.AreEqual(new DateTime(2012, 5, 30, 23, 59, 59), res_7.Item2);
        }

        [TestMethod]
        public void GetRange_Type_17()
        {
            // 对月份的综合测试

            // Arrange       

            // 特殊日期点
            // 2012 年 2 月 有 29 天 【闰年】
            int cyclePoint = 31;

            DateTime checkDate_1 = new DateTime(2012, 1, 12);
            DateTime checkDate_2 = new DateTime(2012, 2, 18);
            DateTime checkDate_3 = new DateTime(2012, 3, 11);
            DateTime checkDate_4 = new DateTime(2012, 4, 19);
            DateTime checkDate_5 = new DateTime(2012, 5, 31);
            DateTime checkDate_6 = new DateTime(2012, 5, 30);
            DateTime checkDate_7 = new DateTime(2012, 5, 29);

            // Act
            Tuple<DateTime, DateTime> res_1 = CycleCalcHelper.GetMonthRange(checkDate_1, cyclePoint);
            Tuple<DateTime, DateTime> res_2 = CycleCalcHelper.GetMonthRange(checkDate_2, cyclePoint);
            Tuple<DateTime, DateTime> res_3 = CycleCalcHelper.GetMonthRange(checkDate_3, cyclePoint);
            Tuple<DateTime, DateTime> res_4 = CycleCalcHelper.GetMonthRange(checkDate_4, cyclePoint);
            Tuple<DateTime, DateTime> res_5 = CycleCalcHelper.GetMonthRange(checkDate_5, cyclePoint);
            Tuple<DateTime, DateTime> res_6 = CycleCalcHelper.GetMonthRange(checkDate_6, cyclePoint);
            Tuple<DateTime, DateTime> res_7 = CycleCalcHelper.GetMonthRange(checkDate_7, cyclePoint);

            Assert.AreEqual(new DateTime(2012, 1, 1), res_1.Item1);
            Assert.AreEqual(new DateTime(2012, 1, 31, 23, 59, 59), res_1.Item2);

            Assert.AreEqual(new DateTime(2012, 2, 1), res_2.Item1);
            Assert.AreEqual(new DateTime(2012, 2, 29, 23, 59, 59), res_2.Item2);

            Assert.AreEqual(new DateTime(2012, 3, 1), res_3.Item1);
            Assert.AreEqual(new DateTime(2012, 3, 31, 23, 59, 59), res_3.Item2);

            Assert.AreEqual(new DateTime(2012, 4, 1), res_4.Item1);
            Assert.AreEqual(new DateTime(2012, 4, 30, 23, 59, 59), res_4.Item2);

            Assert.AreEqual(new DateTime(2012, 5, 1), res_5.Item1);
            Assert.AreEqual(new DateTime(2012, 5, 31, 23, 59, 59), res_5.Item2);

            Assert.AreEqual(new DateTime(2012, 5, 1), res_6.Item1);
            Assert.AreEqual(new DateTime(2012, 5, 31, 23, 59, 59), res_6.Item2);

            Assert.AreEqual(new DateTime(2012, 5, 1), res_7.Item1);
            Assert.AreEqual(new DateTime(2012, 5, 31, 23, 59, 59), res_7.Item2);
        }

        [TestMethod]
        public void GetRange_Type_18()
        {
            // 对月份的综合测试

            // Arrange       

            // 特殊日期点
            // 2015 年 2 月 有 28 天
            int cyclePoint = 28;

            DateTime checkDate_1 = new DateTime(2015, 1, 12);
            DateTime checkDate_2 = new DateTime(2015, 2, 18);
            DateTime checkDate_3 = new DateTime(2015, 3, 11);
            DateTime checkDate_4 = new DateTime(2015, 4, 19);
            DateTime checkDate_5 = new DateTime(2015, 5, 31);
            DateTime checkDate_6 = new DateTime(2015, 5, 30);
            DateTime checkDate_7 = new DateTime(2015, 5, 29);

            // Act
            Tuple<DateTime, DateTime> res_1 = CycleCalcHelper.GetMonthRange(checkDate_1, cyclePoint);
            Tuple<DateTime, DateTime> res_2 = CycleCalcHelper.GetMonthRange(checkDate_2, cyclePoint);
            Tuple<DateTime, DateTime> res_3 = CycleCalcHelper.GetMonthRange(checkDate_3, cyclePoint);
            Tuple<DateTime, DateTime> res_4 = CycleCalcHelper.GetMonthRange(checkDate_4, cyclePoint);
            Tuple<DateTime, DateTime> res_5 = CycleCalcHelper.GetMonthRange(checkDate_5, cyclePoint);
            Tuple<DateTime, DateTime> res_6 = CycleCalcHelper.GetMonthRange(checkDate_6, cyclePoint);
            Tuple<DateTime, DateTime> res_7 = CycleCalcHelper.GetMonthRange(checkDate_7, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2014, 12, 29), res_1.Item1);
            Assert.AreEqual(new DateTime(2015, 1, 28, 23, 59, 59), res_1.Item2);

            Assert.AreEqual(new DateTime(2015, 1, 29), res_2.Item1);
            Assert.AreEqual(new DateTime(2015, 2, 28, 23, 59, 59), res_2.Item2);

            Assert.AreEqual(new DateTime(2015, 3, 1), res_3.Item1);
            Assert.AreEqual(new DateTime(2015, 3, 28, 23, 59, 59), res_3.Item2);

            Assert.AreEqual(new DateTime(2015, 3, 29), res_4.Item1);
            Assert.AreEqual(new DateTime(2015, 4, 28, 23, 59, 59), res_4.Item2);

            Assert.AreEqual(new DateTime(2015, 5, 29), res_5.Item1);
            Assert.AreEqual(new DateTime(2015, 6, 28, 23, 59, 59), res_5.Item2);

            Assert.AreEqual(new DateTime(2015, 5, 29), res_6.Item1);
            Assert.AreEqual(new DateTime(2015, 6, 28, 23, 59, 59), res_6.Item2);

            Assert.AreEqual(new DateTime(2015, 5, 29), res_7.Item1);
            Assert.AreEqual(new DateTime(2015, 6, 28, 23, 59, 59), res_7.Item2);
        }


        [TestMethod]
        public void GetRange_Type_19()
        {
            // cyclePoint<weekDay

            // Arrange            
            DateTime checkDate = new DateTime(2012, 2, 1);  // 周3
            // 0 代表周日
            int cyclePoint = 3;

            // Act
            Tuple<DateTime, DateTime> res = CycleCalcHelper.GetWeekRange(checkDate, cyclePoint);

            // Assert            
            Assert.AreEqual(new DateTime(2012, 1, 26), res.Item1);
            Assert.AreEqual(new DateTime(2012, 2, 1, 23, 59, 59), res.Item2);
        }

    }
}
