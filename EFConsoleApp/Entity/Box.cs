using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFConsoleApp.Entity
{
    public class Box
    {
        public long BoxId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 盒子里面许多水
        /// </summary>
        [ForeignKey("BoxId")]
        public List<Water> Waters { get; set; }
    }
}
