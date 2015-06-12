using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Concrete
{
    public class DynamicPropertyAccessorCache
    {
        private Dictionary<Type, Dictionary<string, DynamicPropertyAccessor>> _cache
            = new Dictionary<Type, Dictionary<string, DynamicPropertyAccessor>>();

        private object _lock = new object();

        public DynamicPropertyAccessor GetAccessor(Type type, string propertyName)
        {
            Dictionary<string, DynamicPropertyAccessor> dict;
            DynamicPropertyAccessor accessor;

            if (_cache.TryGetValue(type, out dict))
            {
                if (dict.TryGetValue(propertyName, out accessor))
                {
                    return accessor;
                }
            }

            lock (_lock)
            {
                if (!_cache.ContainsKey(type)) // double check
                {
                    _cache[type] = new Dictionary<string, DynamicPropertyAccessor>();
                }

                if (!_cache[type].ContainsKey(propertyName))
                {
                    accessor = new DynamicPropertyAccessor(type, propertyName);
                    _cache[type][propertyName] = accessor;
                    return accessor;
                }
                else
                {
                    return _cache[type][propertyName];
                }
            }

        }
    }
}
