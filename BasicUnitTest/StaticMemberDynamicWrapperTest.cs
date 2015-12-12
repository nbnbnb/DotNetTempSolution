using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonLib.Helper;

namespace BasicUnitTest
{
    [TestClass]
    public class StaticMemberDynamicWrapperTest
    {
        [TestMethod]
        public void InvokeMethod_Test()
        {
            // Arrange
            dynamic dyn = new StaticMemberDynamicWrapper(typeof(String));
            // Act
            dynamic res = dyn.Concat("A", "B");
            // Assert
            Assert.AreEqual("AB", res);
        }

        [TestMethod]
        public void ReadProperty_Test()
        {
            // Arrange
            dynamic dyn = new StaticMemberDynamicWrapper(typeof(ABC));
            // Act
            dynamic res = dyn.Name;
            // Assert
            Assert.AreEqual(res, "ABC");
        }

        [TestMethod]
        public void WriteProperty_Test()
        {
            // Arrange
            dynamic dyn = new StaticMemberDynamicWrapper(typeof(ABC));
            // Act
            dyn.Name = "123";
            // Assert
            Assert.AreNotEqual(dyn.Name, "ABC");
            Assert.AreEqual(dyn.Name, "123");
        }


        private class ABC
        {
            public static string Name = "ABC";
        }

    }
}
