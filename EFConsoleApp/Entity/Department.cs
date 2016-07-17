using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFConsoleApp.Entity
{
    public partial class Department
    {
        public int DepartmentID { get; set; }
        public DepartmentNames Name { get; set; }
        public decimal Budget { get; set; }
    }
}
