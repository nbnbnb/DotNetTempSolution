using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLib.Concrete
{
    public class DynamicMethodExecutorCache
    {
        private Dictionary<Type, Dictionary<String, DynamicMethodExecutor>> _cache =
            new Dictionary<Type, Dictionary<String, DynamicMethodExecutor>>();
        private object _lock = new object();
        private bool _flag = false;

        public DynamicMethodExecutor GetExecutor(Type type, string methodName)
        {
            Dictionary<String, DynamicMethodExecutor> dict = null;
            DynamicMethodExecutor res = null;
            if (_cache.TryGetValue(type, out dict))
            {
                if (dict.TryGetValue(methodName, out res))
                {
                    return res;
                }
            }

            try
            {
                Monitor.TryEnter(_lock, ref _flag);

                if (!_cache.ContainsKey(type))
                {
                    _cache[type] = new Dictionary<string, DynamicMethodExecutor>();
                }

                if (!_cache[type].ContainsKey(methodName))
                {
                    res = new DynamicMethodExecutor(type.GetMethod(methodName));
                    _cache[type][methodName] = res;
                    return res;
                }
                else
                {
                    return _cache[type][methodName];
                }

            }
            finally
            {
                if (_flag)
                {
                    _flag = false;
                    Monitor.Exit(_lock);
                }
            }

        }
    }
}
