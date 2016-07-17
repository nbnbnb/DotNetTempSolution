using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFConsoleApp.Initializer;
using EFConsoleApp.Entity;

namespace EFConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            Temp();
            Console.WriteLine("End");
        }

        static void Temp()
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EFConsoleAppDBContext>());
            Database.SetInitializer(new DropCreateDatabaseAlways<EFConsoleAppDBContext>());
            using (EFConsoleAppDBContext context = new EFConsoleAppDBContext())
            {
                Console.WriteLine(context.Lodgings.Count());
                
            }
        }
    }
}
