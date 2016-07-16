using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Concrete
{
    /// <summary>
    /// 线程协调
    /// </summary>
    public sealed class AsyncCoordinator
    {
        private int _opCount = 1;
        private int _statusReported = 0;
        private Action<CoordinationStatus> _callback;


    }
}
