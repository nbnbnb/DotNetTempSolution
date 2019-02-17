using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Pool
{
    /// <summary>
    /// 被池化的对象需要实现的接口
    /// </summary>
    interface IPoolableObject : IDisposable
    {
        /// <summary>
        /// 对象占用的字节数
        /// </summary>
        int Size { get; }

        /// <summary>
        /// 对象的清理方法
        /// </summary>
        void Reset();

        /// <summary>
        /// 对象关联的 PoolManager
        /// </summary>
        /// <param name="poolManager"></param>
        void SetPoolManager(PoolManager poolManager);
    }
}
