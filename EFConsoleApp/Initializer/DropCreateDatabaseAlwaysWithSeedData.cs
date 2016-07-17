using EFConsoleApp.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFConsoleApp.Initializer
{
    public class DropCreateDatabaseAlwaysWithSeedData :
        DropCreateDatabaseAlways<EFConsoleAppDBContext>
    {
        protected override void Seed(EFConsoleAppDBContext context)
        {
            base.Seed(context);

            context.Database.ExecuteSqlCommand("CREATE NONCLUSTERED INDEX IDX_NC_Lodgings_Name ON Lodgings (Name)");
            context.Destinations.Add(new Destination { Name = "Great Barrier Reef" });
            context.Destinations.Add(new Destination { Name = "Grand Canyon" });

            // 注意，不需要调用 context.SaveChange()
        }
    }
}
