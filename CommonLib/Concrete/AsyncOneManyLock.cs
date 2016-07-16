using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLib.Concrete
{
    public enum OneManyMode
    {
        // 独占
        Exclusive,
        // 共享
        Shared
    }

    public sealed class AsyncOneManyLock
    {

        #region 锁的代码
        private SpinLock _lock = new SpinLock(true);  // 自旋锁不要加 readonly
        private void Lock()
        {
            bool taken = false;
            _lock.Enter(ref taken);
        }

        private void Unlock()
        {
            _lock.Exit();
        }

        #endregion

        #region 锁的状态和辅助方法
        private int _state = 0;
        private bool IsFree
        {
            get
            {
                return _state == 0;
            }
        }

        private bool IsOwnedByWriter
        {
            get
            {
                return _state == -1;
            }
        }

        private bool IsOwnedByReaders
        {
            get
            {
                return _state > 0;
            }
        }

        private int AddReaders(int count)
        {
            return _state += count;
        }

        private int SubtractReaders()
        {
            return --_state;
        }

        private void MakeWriter()
        {
            _state = -1;
        }

        private void MakeFree()
        {
            _state = 0;
        }

        #endregion

        // 目的是在非竞态条件时增强性能和减少内存消耗
        private readonly Task _noContentionAccessGranter;

        // 每个等待的 writer 都通过它们在这里排队的 TaskCompletionSource 来唤醒
        private readonly Queue<TaskCompletionSource<object>> _qWaitingWriters =
            new Queue<TaskCompletionSource<object>>();

        // 一个 TaskCompletionSource 收到信号，所有等待的 reader 都唤醒
        private TaskCompletionSource<Object> _waittingReadersSignal =
            new TaskCompletionSource<object>();

        private int _numWaitingReaders = 0;

        public AsyncOneManyLock()
        {
            _noContentionAccessGranter = Task.FromResult<object>(null);
        }

        public Task WaitAsync(OneManyMode mode)
        {
            Task accressGranter = _noContentionAccessGranter;  // 假定无竞争

            Lock();

            switch (mode)
            {
                case OneManyMode.Exclusive:
                    if (IsFree)
                    {
                        MakeWriter(); // 无竞争
                    }
                    else
                    {
                        // 有竞争：新的 writer 任务进入队列，并返回它使 writer 等待
                        var tcs = new TaskCompletionSource<Object>();
                        _qWaitingWriters.Enqueue(tcs);
                        accressGranter = tcs.Task;
                    }
                    break;
                case OneManyMode.Shared:
                    if (IsFree || (IsOwnedByReaders && _qWaitingWriters.Count == 0))
                    {
                        AddReaders(1); // 无竞争
                    }
                    else  // 有竞争
                    {
                        // 竞争：递增等待的 reader 数量，并返回 reader 任务使 reader 等待
                        _numWaitingReaders++;
                        accressGranter = _waittingReadersSignal.Task.ContinueWith(t => t.Result);
                    }
                    break;
            }
            Unlock();

            return accressGranter;
        }

        public void Release()
        {
            TaskCompletionSource<Object> accessGranter = null; // 假定没有代码被释放
            Lock();
            if (IsOwnedByWriter)
            {
                MakeFree(); // 一个 writer 离开
            }
            else
            {
                SubtractReaders(); // 一个 reader 离开
            }
            if (IsFree)
            {
                // 如果自由，唤醒 1 个等待的 writer 或所有等待的 readers
                if (_qWaitingWriters.Count > 0)
                {
                    MakeWriter();
                    accessGranter = _qWaitingWriters.Dequeue();
                }
                else if (_numWaitingReaders > 0)
                {
                    AddReaders(_numWaitingReaders);
                    _numWaitingReaders = 0;
                    accessGranter = _waittingReadersSignal;

                    // 为将来需要等待的 readers 创建一个新的 TCS
                    _waittingReadersSignal = new TaskCompletionSource<object>();
                }
            }
            Unlock();

            // 唤醒锁外面的 writer/reader，减少竞争机率以提高性能
            if (accessGranter != null)
            {
                accessGranter.SetResult(null);
            }
        }
    }

    public class Example
    {
        private static async Task AccessResourceViaAsyncSynchronizaton(AsyncOneManyLock asyncLock)
        {
            // TODO: 执行你想要的任何代码

            // 为想要的并发访问传递 OneManyMode.Exclusive 或 OneManyMode.Shared
            await asyncLock.WaitAsync(OneManyMode.Shared);  // 要求访问共享锁

            // 如果执行到这里，表明没有其他线程在向资源写入；可能有其他线程在读取
            // TODO：从资源读取...

            // 资源访问完毕就放弃锁，使其他代码能访问资源
            asyncLock.Release();

            // TODO: 执行你想要的任何代码...
        }

    }
}
