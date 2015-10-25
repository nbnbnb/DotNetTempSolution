using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLib.Extensions
{
    public static class TaskExtension
    {
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
                return String.Format("LogTime={0}, Tag={1}, Member={2}, File={3} {4}",
                    LogTime, Tag ?? "(none)", CallerMemberName, CallerFilePath, CallerMemberName);
            }
        }

        private static readonly ConcurrentDictionary<Task, TaskLogEntry> s_log =
            new ConcurrentDictionary<Task, TaskLogEntry>();

        public static IEnumerable<TaskLogEntry> GetLogEntries()
        {
            return s_log.Values;
        }

        /// <summary>
        /// 注意这些方法特性的使用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="tag"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        /// <returns></returns>
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
                s_log.TryRemove(t, out entry);
            }, TaskContinuationOptions.ExecuteSynchronously);

            return task;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NoWarning(this Task task) { }

        private struct Void { }

        public static async Task<TResult> WithCancellation<TResult>(this Task<TResult> originTask,
            CancellationToken ct)
        {
            return await (Task<TResult>)WithCancellation(originTask, ct);
        }

        public static async Task WithCancellation(this Task originTask, CancellationToken ct)
        {
            // 创建在 CancellatinToken 被取消时完成的一个 Task
            var cancelTask = new TaskCompletionSource<Void>();
            // 一旦 CancellationToken 被取消，就完成 Task
            using (ct.Register(t => ((TaskCompletionSource<Void>)t).TrySetResult(new Void()), cancelTask))
            {
                //  创建在原始 Task 或 CancellationToken Task 完成时都完成的一个 Task
                Task any = await Task.WhenAny(originTask, cancelTask.Task);

                // 任何 Task 因为 CancellationToken 而完成，就抛出 OperationCanceledException
                if (any == cancelTask.Task)
                {
                    ct.ThrowIfCancellationRequested();
                }
            }

            await originTask;
        }

        public static IEnumerable<Task<T>> InCompletionOrder<T>(this IEnumerable<Task<T>> source)
        {
            var inputs = source.ToList();
            var boxes = inputs.Select(m => new TaskCompletionSource<T>()).ToList();

            int currentIndex = -1;
            foreach (var task in inputs)
            {
                task.ContinueWith(completed =>
                {
                    var nextBox = boxes[Interlocked.Increment(ref currentIndex)];
                    PropagateResult(completed, nextBox);
                }, TaskContinuationOptions.ExecuteSynchronously);
            }

            return boxes.Select(m => m.Task);
        }

        private static void PropagateResult<T>(Task<T> completed, TaskCompletionSource<T> nextBox)
        {
            if (completed.IsCanceled)
            {
                nextBox.SetCanceled();
            }
            else if (completed.IsFaulted)
            {
                nextBox.SetException(completed.Exception);
            }
            else
            {
                nextBox.SetResult(completed.Result);
            }
        }
    }
}