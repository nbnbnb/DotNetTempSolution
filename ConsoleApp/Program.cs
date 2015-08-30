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
            Expression<Func<UserInfo, bool>> expr = name => name.UserAge > 10 && 'A' == name.UserType;
            Console.WriteLine(expr); 
            CharToStringModifier treeModifier = new CharToStringModifier();
            Expression modifiedExpr = treeModifier.Modify(expr);
            Console.WriteLine(modifiedExpr); 
        }

        private class UserInfo
        {
            public char UserType { get; set; }

            public short UserAge { get; set; }
        }
        #endregion
    }

}