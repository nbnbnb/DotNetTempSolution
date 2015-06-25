using ConsoleApp.Demos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
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
            //ExpressionTree.Start();
            DynamicQueryFeatures.MiscDemo();
            //Demo();
        }

        #region Demo
        private static void Demo()
        {
			Console.WriteLine("main");
			Console.WriteLine("feature");
        }
        #endregion
    }
}