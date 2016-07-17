using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace EFConsoleApp.Entity
{
    public class Measurement
    {
        public decimal Reading { get; set; }

        public string Units { get; set; }
    }
}
