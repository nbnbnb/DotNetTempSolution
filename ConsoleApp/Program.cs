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
            TaskDemo.EventAwaiterDemo();
        }

        private static async Task<String> AwaitWebClient(Uri uri)
        {
            var wc = new WebClient();
            var tcs = new TaskCompletionSource<String>();

            wc.DownloadStringCompleted += (s, e) =>
              {
                  // 设置任务的几种状态
                  if (e.Cancelled)
                  {
                      tcs.SetCanceled();
                  }
                  else if (e.Error != null)
                  {
                      tcs.SetException(e.Error);
                  }
                  else
                  {
                      tcs.SetResult(e.Result);
                  }
              };

            // 启动异步任务
            wc.DownloadStringAsync(uri);

            // 等待 TaskCompletionSource 的 Task
            return await tcs.Task;
        }

        #endregion
    }

}