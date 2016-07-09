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
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections;
using System.Xml.Linq;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.IO;
using System.Linq.Dynamic;
using System.Net.Http;

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

        public static void SayHello()
        {
            Console.WriteLine("Hello");
        }


        #region Demo
        private static void Demo()
        {

        }

        #endregion
    }

}