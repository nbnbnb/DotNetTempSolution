using CommonLib.Entities;
using CommonLib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib.Extensions;

namespace BasicUnitTest
{
    [TestClass]
    public class EnumExtensionTest
    {
        [TestMethod]
        public void IsSet_Test()
        {
            OrderType ordered = OrderType.Ordered;
            OrderType payed = OrderType.Payed;
            OrderType processed = OrderType.Processed;
            OrderType merge = ordered | payed | processed;

            Assert.IsTrue(merge.IsSet(ordered));
            Assert.IsTrue(merge.IsSet(payed));
            Assert.IsTrue(merge.IsSet(processed));
        }

        [TestMethod]
        public void Equal_Int_Test()
        {
            OrderType ordered = OrderType.Ordered;
            OrderType payed = OrderType.Payed;
            OrderType processed = OrderType.Processed;

            Assert.IsTrue(ordered.AreEqual(1));
            Assert.IsTrue(payed.AreEqual(4));
            Assert.IsTrue(processed.AreEqual(8));
        }
    }

}