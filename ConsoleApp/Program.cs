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
using Newtonsoft.Json;
using MVC5App.Models;

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
        private static void Demo()
        {
            string js = File.ReadAllText(@"H:\trigger.js");
            var gg= JsonConvert.DeserializeObject<TriggerJSModel>(js);
            Console.WriteLine(gg==null);
        }
        #endregion
    }
}