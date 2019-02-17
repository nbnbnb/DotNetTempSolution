using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Pool
{
    /// <summary>
    /// 管理池化对象
    /// </summary>
    class PoolManager
    {

        /// <summary>
        /// 池子对象
        /// 内部私有容器
        /// </summary>
        private class Pool
        {
            /// <summary>
            /// 池子占用的字节数
            /// </summary>
            public int PooledSize { get; set; }

            public int Count
            {
                get
                {
                    return this.Stack.Count;
                }
            }

            /// <summary>
            /// 用于管理池子中的对象
            /// </summary>
            public Stack<IPoolableObject> Stack { get; private set; }

            /// <summary>
            /// 初始化池子容器
            /// </summary>
            public Pool()
            {
                this.Stack = new Stack<IPoolableObject>();
            }
        }

        /// <summary>
        /// 每个类型的池子最大值
        /// 10MB
        /// </summary>
        private const int MaxSizePerType = 10 * (1 << 10);  // 10MB

        /// <summary>
        /// 类型-池子字典容器
        /// </summary>
        private Dictionary<Type, Pool> pools = new Dictionary<Type, Pool>();

        /// <summary>
        /// 获取所有池子中对象的总数
        /// </summary>
        public int TotalCount
        {
            get
            {
                return this.pools.Values.Sum(m => m.Count);
            }
        }

        /// <summary>
        /// 从池中获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetObject<T>() where T : class, IPoolableObject, new()
        {
            T valueToReturn = null;
            if (pools.TryGetValue(typeof(T), out Pool pool))
            {
                if (pool.Stack.Count > 0)
                {
                    // 如果池中有值，则从池中返回
                    valueToReturn = pool.Stack.Pop() as T;
                }
            }

            // 没有则创建一个新的对象
            // 注意，此对象创建完成后直接返回
            if (valueToReturn == null)
            {
                valueToReturn = new T();
                // 设置 PoolManager
                valueToReturn.SetPoolManager(this);
            }

            return valueToReturn;
        }

        /// <summary>
        /// 对象返回池中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void ReturnObject<T>(T value) where T : class, IPoolableObject, new()
        {
            // 如果类型字典中没有这个对象
            // 则创建类型，然后入池
            if (!pools.TryGetValue(typeof(T), out Pool pool))
            {
                pool = new Pool();
                pools[typeof(T)] = pool;
            }

            // 在范围之内
            if (value.Size + pool.PooledSize < MaxSizePerType)
            {
                // 池子占用的字节数（对象占用的字节数求和）
                pool.PooledSize += value.Size;
                // 执行对象清理方法
                value.Reset();
                // 对象最后返回池中
                pool.Stack.Push(value);
            }

            // 超过了范围
            // 那么这个对象应该被销毁
        }
    }
}
