using Demos;
using System;

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
            ExpressionTreeDemo.Demo04();
        }
        #endregion
    }

}