using CommonLib.Concrete;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicUnitTest
{
    [TestClass]
    public class DynamicPropertyAccessorTest
    {
        [TestMethod]
        public void CanGetValue()
        {
            // Arrange
            UserInfo userInfo = new UserInfo
            {
                UserName = "BTKing",
                UserAge = 29
            };

            DynamicPropertyAccessor accessor_username = new DynamicPropertyAccessor(typeof(UserInfo), "UserName");
            DynamicPropertyAccessor accessor_userage = new DynamicPropertyAccessor(typeof(UserInfo), "UserAge");

            // Act
            object username = accessor_username.GetValue(userInfo);
            object userage = accessor_userage.GetValue(userInfo);

            // Assert
            Assert.AreEqual(username, "BTKing");
            Assert.AreEqual(userage, 29);
        }

        private class UserInfo
        {
            public string UserName { get; set; }

            public int UserAge { get; set; }
        }
    }
}
