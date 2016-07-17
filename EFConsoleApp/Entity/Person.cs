using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace EFConsoleApp.Entity
{
    [Table("People")]
    public class Person
    {
        public Person()
        {
            Address = new Address();
            Info = new PersonInfo
            {
                Weight = new Measurement(),
                Height = new Measurement()
            };
        }

        public int PersonId { get; set; }

        [ConcurrencyCheck]
        public int SocialSecurityNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Address Address { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public PersonInfo Info { get; set; }

        public List<Lodging> PrimaryContactFor { get; set; }

        public List<Lodging> SecondaryContactFor { get; set; }

        // 合并表时，必须添加 Require
        [Required]
        public PersonPhoto Photo { get; set; }

        public List<Reservation> Reservations { get; set; }

        [NotMapped]
        public string TodayForecast { get; set; }
    }
}
