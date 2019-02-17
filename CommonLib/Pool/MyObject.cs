using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Pool
{
    /// <summary>
    /// 自定义对象
    /// 需要被池化，实现 IPoolableObject 接口
    /// </summary>
    class MyObject : IPoolableObject
    {
        private PoolManager poolManager;

        public byte[] Data { get; set; }

        public int UsableLength { get; set; }

        /// <summary>
        /// 返回自身的大小
        /// </summary>
        public int Size
        {
            get
            {
                return Data != null ? Data.Length : 0;
            }
        }

        /// <summary>
        /// 注意
        /// Dispose 实际上是返回池中
        /// </summary>
        public void Dispose()
        {
            // 返回池中
            this.poolManager.ReturnObject(this);
        }

        /// <summary>
        /// 返回池中时执行的清理动作
        /// </summary>
        public void Reset()
        {
            UsableLength = 0;
        }

        /// <summary>
        /// 设置 PoolManager
        /// </summary>
        /// <param name="poolManager"></param>
        public void SetPoolManager(PoolManager poolManager)
        {
            this.poolManager = poolManager;
        }
    }
}
