using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Entities
{
    [Flags]
    public enum OrderType
    {
        Ordered = 0x01,
        PrePay = 0x02,
        Payed = 0x04,
        Processing = 0x08,
        Processed = 0x10,
        Completed = 0x20
    }
}
