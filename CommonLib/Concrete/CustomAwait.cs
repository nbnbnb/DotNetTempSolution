using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Concrete
{
    /// <summary>
    /// 为包含返回值的异步操作建立的虚拟接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAwaitable<T>
    {
        IAWaiter<T> GetAwaiter();
    }

    public interface IAWaiter<T> : INotifyCompletion
    {
        bool IsCompleted { get; }

        T GetResult();
    }

    /// <summary>
    /// 为没有返回值的异步操作建立的虚拟接口
    /// </summary>
    public interface IAwaitable
    {
        IAWaiter GetAwaiter();
    }

    public interface IAWaiter : INotifyCompletion
    {
        bool IsCompleted { get; }

        void GetResult();
    }

    public class AggregatedExceptionAwaitale : IAwaitable
    {
        private Task _task;

        public AggregatedExceptionAwaitale(Task task)
        {
            _task = task;
        }

        public IAWaiter GetAwaiter()
        {
            return new AggregatedExceptionAwaiter(_task);
        }
    }

    public class AggregatedExceptionAwaitale<T> : IAwaitable<T>
    {
        private Task<T> _task;

        public AggregatedExceptionAwaitale(Task<T> task)
        {
            _task = task;
        }

        public IAWaiter<T> GetAwaiter()
        {
            return new AggregatedExceptionAwaiter<T>(_task);
        }
    }

    public class AggregatedExceptionAwaiter : IAWaiter
    {
        private Task _task;
        public AggregatedExceptionAwaiter(Task task)
        {
            _task = task;
        }

        public bool IsCompleted
        {
            get
            {
                return _task.GetAwaiter().IsCompleted;
            }
        }

        public void GetResult()
        {
            _task.Wait();  // 如果错误，将会抛出 AggregateException
        }

        public void OnCompleted(Action continuation)
        {
            _task.GetAwaiter().OnCompleted(continuation);
        }
    }

    public class AggregatedExceptionAwaiter<T> : IAWaiter<T>
    {
        private Task<T> _task;
        public AggregatedExceptionAwaiter(Task<T> task)
        {
            _task = task;
        }

        public bool IsCompleted
        {
            get
            {
                return _task.GetAwaiter().IsCompleted;
            }
        }

        public T GetResult()
        {
            return _task.Result;  // 如果错误，将会抛出 AggregateException
        }

        public void OnCompleted(Action continuation)
        {
            _task.GetAwaiter().OnCompleted(continuation);
        }
    }
}
