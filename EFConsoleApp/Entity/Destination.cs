using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace EFConsoleApp.Entity
{
    public class Destination
    {
        [Column("LocationId")]
        public int DestinationId { get; set; }

        [Required]
        [Column("LocationName")]
        public string Name { get; set; }

        [MaxLength(500)]
        [MinLength(5)]
        public string Country { get; set; }
        
        public string Description { get; set; }

        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }
        
        public List<Lodging> Lodgings { get; set; }
    }
}
