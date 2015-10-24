using CommonLib.Concrete;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using CommonLib.Extensions;
using System.Net;
using System.Threading;
using Demos;

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
            Demo();
        }

        #region Demo
        private static  void Demo()
        {
            TaskDemo.EventAwaiterDemo();
        }
        #endregion

    }

}