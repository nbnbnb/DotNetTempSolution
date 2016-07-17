using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace EFConsoleApp.Entity
{
    [ComplexType]
    public class PersonInfo
    {
        public Measurement Weight { get; set; }

        public Measurement Height { get; set; }

        public string DietryRestrictions { get; set; }
    }
}
