using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Dynamic
{
    public class DynamicPropertyAccessorCache
    {
        private object _mutex = new object();

        private Dictionary<Type, Dictionary<string, DynamicPropertyAccessor>> _cache =
            new Dictionary<Type, Dictionary<string, DynamicPropertyAccessor>>();

        public DynamicPropertyAccessor GetAccessor(Type type, string propertyName)
        {
            DynamicPropertyAccessor accessor;
            Dictionary<string, DynamicPropertyAccessor> typeCahce;

            if (this._cache.TryGetValue(type, out typeCahce))
            {
                if (typeCahce.TryGetValue(propertyName, out accessor))
                {
                    return accessor;
                }
            }

            lock (_mutex)
            {
                if (!this._cache.ContainsKey(type))
                {
                    this._cache[type] = new Dictionary<string, DynamicPropertyAccessor>();
                }

                accessor = new DynamicPropertyAccessor(type, propertyName);
                this._cache[type][propertyName] = accessor;

                return accessor;
            }
        }
    }
}
