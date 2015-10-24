using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLib.Concrete
{
    public class EventAwaiter<TEventArgs> : INotifyCompletion
    {
        private ConcurrentQueue<TEventArgs> m_events = new ConcurrentQueue<TEventArgs>();
        private Action m_continuation;

        #region 状态机调用的成员
        /// <summary>
        /// 编译器可以在 await 的任何操作数上调用 GetAwaiter
        /// 所以操作数不一定是 Task 对象，可以是任意类型
        /// 只要提供了一个可以调用的 GetAwaiter 方法
        /// </summary>
        /// <returns></returns>
        public EventAwaiter<TEventArgs> GetAwaiter()
        {
            // 状态机首先调用这个来获得 awaiter，我们自己返回自己
            return this;
        }

        /// <summary>
        /// 告诉状态机是否发生了任何事情
        /// 执行 OnCompleted
        /// </summary>
        public Boolean IsCompleted
        {
            get
            {
                Console.WriteLine(1);
                return m_events.Count > 0;
            }
        }

        /// <summary>
        /// 状态机告诉我们以后要调用什么方法，我们把它保存起来
        /// 这是下一次的回调
        /// </summary>
        /// <param name="continuation"></param>
        public void OnCompleted(Action continuation)
        {
            Console.WriteLine(2);
            Volatile.Write(ref m_continuation, continuation);
        }

        /// <summary>
        /// 状态机查询结果，这是 awaite 操作符的结果
        /// </summary>
        /// <returns></returns>
        public TEventArgs GetResult()
        {
            Console.WriteLine(4);
            TEventArgs e;
            m_events.TryDequeue(out e);
            return e;
        }
        #endregion

        /// <summary>
        /// 如果都引发了事件，多个线程可能同时调用
        /// 当事件触发时，类似于调用一次状态机的 MoveNext 方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public void EventRaised(Object sender, TEventArgs eventArgs)
        {
            Console.WriteLine(3);
            // 存储事件参数
            m_events.Enqueue(eventArgs);

            Action continuation = Interlocked.Exchange(ref m_continuation, null);

            if (continuation != null)
            {
                continuation();
            }
        }
    }
}
