using EFConsoleApp.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFConsoleApp
{
    public class EFConsoleAppDBContext : DbContext
    {

        public EFConsoleAppDBContext() : base("EFConsoleApp")
        {

        }

        public DbSet<Destination> Destinations { get; set; }

        public DbSet<Lodging> Lodgings { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<Person> Peoples { get; set; }

        public DbSet<Box> Boxes { get; set; }

        /// <summary>
        /// 包含枚举类型
        /// </summary>
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Lodging>()
                .Property(m => m.Name).IsUnicode(false);

            modelBuilder.Entity<Trip>()
                .HasMany(t => t.Activities)
                .WithMany(a => a.Trips)
                .Map(c =>
                {
                    c.ToTable("TripActivities");
                    c.MapLeftKey("TripIdentifier");
                    c.MapRightKey("ActivityId");
                });

            // 将实体映射到多张物理表中
            // 不要漏掉任何属性
            modelBuilder.Entity<Destination>()
                .Map(m =>
                {
                    m.Properties(n => new
                    {
                        n.Name,
                        n.Country,
                        n.Description
                    });
                    m.ToTable("Locations");
                })
                .Map(m =>
                {
                    m.Properties(n => new { n.Photo });
                    m.ToTable("LocatioinPhotos");
                });
        }
    }
}
