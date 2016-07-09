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
    /// <summary>
    /// 创建自定义 Awaiter 时，需要实现 INotifyCompletion 接口
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
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

            Console.WriteLine(0);
            // 状态机首先调用这个来获得 awaiter，直接返回自己
            // 因为它已经实现了 IsCompleted、OnCompleted、GetResult 这些必须方法
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
        /// 将其保存到 Action 变量中
        /// 下一步的操作就是 await 后续的代码
        /// </summary>
        /// <param name="continuation"></param>
        public void OnCompleted(Action continuation)
        {
            Console.WriteLine(2);
            Volatile.Write(ref m_continuation, continuation);
        }

        /// <summary>
        /// 状态机查询结果，await 等待的就是这个结果
        /// 将队列清空后，IsComplete 就会返回 False
        /// 此时将会调用 OnCompleted 执行下一次 MoveNext
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
            // 此时，IsComplete 就会返回 True
            // 然后 GetResult 返回就行执行
            // 当队列清空之后，IsComplete 就为 False
            // 
            m_events.Enqueue(eventArgs);
            // 此处使用了不为空的简化语法
            // 执行完了一个 ContinueWith，然后将其设置为 null
            // Invoke 的执行，内部执行了一次状态机的 MoveNext
            Interlocked.Exchange(ref m_continuation, null)?.Invoke();
        }
    }
}
