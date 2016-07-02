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
        private async static void Demo()
        {
            await CatchMultipleExceptions();
        }

        private async static Task CatchMultipleExceptions()
        {
            Task task1 = Task.Run(() => { throw new Exception("Message 1"); });
            Task task2 = Task.Run(() => { throw new Exception("Message 2"); });

            try
            {
                // 由于 await 默认只会抛出 AggregateException 中的第一个异常
                // 所以此处对于多异常的场景进行了扩展
                // 在内部的方法中，使用了 Task.Wait()
                // 所以将直接抛出 AggregateException 异常，里面包含了所有的错误信息
                await Task.WhenAll(task1, task2).WithAggreagetdExceptions();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("Caught {0} exceptions {1}",
                    ex.InnerExceptions.Count,
                    string.Join(", ", ex.InnerExceptions.Select(m => m.Message)));
            }
        }

        #endregion
    }

}