using CommonLib.Concrete;
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
        #region 在全部完成时收集结果
        /// <summary>
        /// 在全部完成时收集结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
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
            // 将已完成的 Task<T> 的结果传播到 TaskCompletionSource<T> 中
            // 如果原始任务正常完成，则将值复制到 TaskCompletionSource<T> 中
            // 如果产生错误，则可将异常复制到 TaskCompletionSource<T> 中

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

        #endregion

        #region AggregateException
        public static AggregatedExceptionAwaitale WithAggreagetdExceptions(this Task task)
        {
            return new AggregatedExceptionAwaitale(task);
        }
        #endregion

        #region 给任务附加取消
        private struct Void { }

        /// <summary>
        /// 给任务附加取消
        /// 泛型
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="originTask"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async Task<TResult> WithCancellation<TResult>(this Task<TResult> originTask,
            CancellationToken ct)
        {
            return await (Task<TResult>)WithCancellation((Task)originTask, ct);
        }

        /// <summary>
        /// 给任务附加取消
        /// 非泛型
        /// </summary>
        /// <param name="originTask"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async Task WithCancellation(this Task originTask, CancellationToken ct)
        {
            // 创建在 CancellatinToken 被取消时完成的一个 Task
            var cancelTask = new TaskCompletionSource<Void>();
            // 一旦 CancellationToken 被取消，就会执行 Register 的回调
            using (ct.Register(t => ((TaskCompletionSource<Void>)t).TrySetResult(new Void()), cancelTask))
            {
                //  二取一
                // 是正常任务完成，还是取消任务完成（执行了 Register 回调）
                Task any = await Task.WhenAny(originTask, cancelTask.Task);

                // 如果取消了，就抛出 OperationCanceledException
                if (any == cancelTask.Task)
                {
                    ct.ThrowIfCancellationRequested();
                }
            }

            await originTask;
        }

        #endregion

    }
}