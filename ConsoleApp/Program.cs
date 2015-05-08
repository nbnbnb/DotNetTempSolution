using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start.");
            Temp();
            Console.WriteLine("End.");
            Console.ReadKey();
        }

        private static void Temp()
        {
            
        }

        private static void ThreadDemo()
        {
            Task.Run(() => {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("Task Thread.");
            });

            Console.WriteLine("Main Thread");
        }
    }
}
