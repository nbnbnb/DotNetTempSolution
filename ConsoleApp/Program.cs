using CommonLib.Concrete;
using Demos;
using System;
using System.Linq.Expressions;

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
            //DynamicQueryFeatures.MiscDemo();
            Demo();
        }

        #region Demo
        private static void Demo()
        {

        }
        #endregion
    }

}