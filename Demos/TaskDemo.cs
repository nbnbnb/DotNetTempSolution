using CommonLib.Concrete;
using CommonLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonLib.Helper;

namespace Demos
{
    public class TaskDemo
    {
        /// <summary>
        /// 等待异步操作时利用超时和取消功能
        /// </summary>
        public static async void TaskLoggerDemo()
        {
#if DEBUG
            // 使用 TaskLogger 会影响内存和性能，所以只有在调试生成中启用它
            TaskLogger.LogLevel = TaskLogger.TaskLogLevel.Pending;
#endif
            var tasks = new List<Task>
            {
                // 调用 Log 扩展方法时
                // 会将任务信息添加到集合中，当任务完成时，会中这个集合中移除
                // 通过监控这个集合，就能查看当前进程中的未完成任务信息
                Task.Delay(2000).Log("2s op"),
                Task.Delay(5000).Log("5s op"),
                Task.Delay(6000).Log("6s op"),
            };

            try
            {
                // 设置 3s 后超时
                await Task.WhenAll(tasks).WithCancellation(new CancellationTokenSource(3000).Token);
            }
            catch (OperationCanceledException)
            {
                // 任何 Task 触发了取消
                // 忽略
            }

            // 显示尚未完成的异步操作
            // 这在调试时特别有用，尤其是当应用程序因为错误的请求或者未响应的服务器而挂起的时候
            foreach (var op in TaskLogger.GetLogEntries().OrderBy(m => m.LogTime))
            {
                Console.WriteLine(op);
            }
        }

        public static void EventAwaiterDemo()
        {
            ShowExceptions();

            for (Int32 x = 0; x < 3; x++)
            {
                try
                {
                    switch (x)
                    {
                        case 0: throw new InvalidOperationException();
                        case 1: throw new ObjectDisposedException("");
                        case 2: throw new ArgumentOutOfRangeException();
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// 不会阻塞调用线程
        /// </summary>
        private static async void ShowExceptions()
        {
            // 实例化一个自定义的 Awaiter
            // 只要满足 await 的要求，就可以等待它
            var eventAwaiter = new EventAwaiter<FirstChanceExceptionEventArgs>();

            AppDomain.CurrentDomain.FirstChanceException += eventAwaiter.EventRaised;

            // 可以无限 ContinueWith
            while (true)
            {
                FirstChanceExceptionEventArgs arg = await eventAwaiter;  // awaiter 的 Result
                // 下面是 continuation
                Console.WriteLine("AppDomain exception: {0}", arg.Exception.GetType());
            }
        }

    }
}
