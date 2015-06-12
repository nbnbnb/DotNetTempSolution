using CommonLib.Concrete;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BasicUnitTest
{
    [TestClass]
    public class DynamicMethodExecutorTest
    {
        [TestMethod]
        public void StaticMethodTest()
        {
            // Arrange
            // 定位到一个整型参数的那个重载
            MethodInfo info = typeof(Console).GetMethod("WriteLine", new[] { typeof(int) });
            DynamicMethodExecutor executor = new DynamicMethodExecutor(info);

            // Act
            object res = executor.Execute(null, new object[] { 123 });

            // Assert
            Assert.AreEqual(null, res);
        }

        [TestMethod]
        public void InstanceMethodTest()
        {
            // Arrange
            UserInfo userInfo = new UserInfo();
            MethodInfo getAge = typeof(UserInfo).GetMethod("GetAge", BindingFlags.Public | BindingFlags.Instance);
            MethodInfo sayName = typeof(UserInfo).GetMethod("SayName", BindingFlags.Public | BindingFlags.Instance);

            DynamicMethodExecutor executor_getAge = new DynamicMethodExecutor(getAge);
            DynamicMethodExecutor executor_sayName = new DynamicMethodExecutor(sayName);

            // Act
            object getAge_res = executor_getAge.Execute(userInfo, null);
            object sayName_res = executor_sayName.Execute(userInfo, new object[] { "BTKing" });

            // Assert
            Assert.AreEqual(getAge_res, 28);
            Assert.AreEqual(sayName_res, null);
        }


        private class UserInfo
        {
            public int GetAge()
            {
                return 28;
            }

            public void SayName(string name)
            {
                Console.WriteLine("Hello " + name);
            }
        }
    }
}
