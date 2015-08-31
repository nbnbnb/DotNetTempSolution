using CommonLib.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Extensions
{
    public static class EnumExtension
    {
        public static bool IsSet(this OrderType flags, OrderType flagsToTest)
        {
            if (flagsToTest == 0)
            {
                throw new ArgumentOutOfRangeException("flagsToTest", "Value must not be 0");
            }
            return (flags & flagsToTest) == flagsToTest;
        }

        public static bool IsClear(this OrderType flags, OrderType flagsToTest)
        {
            if (flagsToTest == 0)
            {
                throw new ArgumentOutOfRangeException("flagsToTest", "Value must not be 0");
            }
            return !IsSet(flags, flagsToTest);
        }

        public static OrderType Clear(this OrderType flags, OrderType clearFlags)
        {
            return flags & ~clearFlags;
        }

        public static void ForEach(this OrderType flags, Action<OrderType> processFlags)
        {
            if (processFlags == null)
            {
                throw new ArgumentNullException("processFlag");
            }

            for (UInt32 bit = 1; bit != 0; bit <<= 1)
            {
                UInt32 temp = ((UInt32)flags) & bit;
                if (temp != 0)
                {
                    processFlags((OrderType)temp);
                }
            }
        }

        public static bool AreEqual(this OrderType flags, int val)
        {
            return (int)flags == val;
        }
    }
}
