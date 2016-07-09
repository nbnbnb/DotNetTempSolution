using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLib.Helper
{
    /// <summary>
    /// 用来显示尚未完成的异步操作
    /// 这在调试时特别有用，尤其是当应用程序因为错误的请求或者响应的服务器而挂起的时候
    /// 使用 TaskLogger 会影响内存和性能，所以只在调试生成中启用它（使用 DEBUG 符号开关）
    /// </summary>
    public static class TaskLogger
    {
        // 用来存储未完成的任务信息
        private static readonly ConcurrentDictionary<Task, TaskLogEntry> s_log =
            new ConcurrentDictionary<Task, TaskLogEntry>();

        // 由于值类型默认值为 0
        // 所以默认枚举值就是 None
        public enum TaskLogLevel { None, Pending };

        public static TaskLogLevel LogLevel { get; set; }

        public sealed class TaskLogEntry
        {
            public Task Task { get; internal set; }

            public String Tag { get; internal set; }

            public DateTime LogTime { get; internal set; }

            public String CallerMemberName { get; internal set; }

            public string CallerFilePath { get; internal set; }

            public Int32 CallerLineNumber { get; internal set; }

            public override string ToString()
            {
                return String.Format("LogTime={0}, Tag={1}, Member={2}, File={3} {4}, Line={5}",
                    LogTime, Tag ?? "(none)", CallerMemberName, CallerFilePath, CallerMemberName, CallerLineNumber);
            }
        }

        public static IEnumerable<TaskLogEntry> GetLogEntries()
        {
            return s_log.Values;
        }

        public static Task<TResult> Log<TResult>(this Task<TResult> task,
            String tag = null,
            [CallerMemberName] String callerMemberName = null,
            [CallerFilePath] String callerFilePath = null,
            [CallerLineNumber] Int32 callerLineNumber = -1)
        {
            return (Task<TResult>)Log((Task)task, tag, callerMemberName, callerFilePath, callerLineNumber);
        }

        public static Task Log(this Task task,
            String tag = null,
            [CallerMemberName] String callerMemberName = null,
            [CallerFilePath] String callerFilePath = null,
            [CallerLineNumber] Int32 callerLineNumber = -1)
        {
            if (LogLevel == TaskLogLevel.None)
            {
                return task;
            }

            var logEntry = new TaskLogEntry
            {
                Task = task,
                LogTime = DateTime.Now,
                Tag = tag,
                CallerMemberName = callerMemberName,
                CallerFilePath = callerFilePath,
                CallerLineNumber = callerLineNumber
            };

            s_log[task] = logEntry;
            task.ContinueWith(t =>
            {
                TaskLogEntry entry;
                // 如果事务已经执行了，则将其从集合中移除
                s_log.TryRemove(t, out entry);  
            }, TaskContinuationOptions.ExecuteSynchronously);

            return task;
        }

    }
}
