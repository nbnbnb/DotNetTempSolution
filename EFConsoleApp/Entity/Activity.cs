using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EFConsoleApp.Entity
{
    public class Activity
    {
        public int ActivityId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<Trip> Trips { get; set; }
    }
}
