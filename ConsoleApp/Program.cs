using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting;
using System.Reflection;
using System.IO;
using System.Text;
using System.Collections.Concurrent;
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
        private static void Demo()
        {
            AOPDemo.PropertyGetAOP();
        }
        #endregion
    }

}









