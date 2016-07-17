using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace EFConsoleApp.Entity
{
    public class InternetSpecial
    {
        public int InternetSpecialId { get; set; }

        public int Nights { get; set; }

        public decimal CustUSD { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [ForeignKey("Accommodation")]
        public int AccommodationId { get; set; }

        public Lodging Accommodation { get; set; }
    }
}
