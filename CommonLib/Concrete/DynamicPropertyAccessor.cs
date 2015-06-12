using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Concrete
{
    public class DynamicPropertyAccessor
    {
        // 需要实现的代码原型为
        // ((T)instance).Property
        // 技术实现：表达式树

        private Func<object, object> _getter;

        public DynamicPropertyAccessor(Type type, string propertyName)
            : this(type.GetProperty(propertyName))
        {

        }

        public DynamicPropertyAccessor(PropertyInfo methodInfo)
        {
            // 表达式原型
            // ((T)instance).Property
            ParameterExpression instanceExpression = Expression.Parameter(typeof(object), "instance");

            // (T)instance
            UnaryExpression instanceCast = Expression.Convert(instanceExpression, methodInfo.ReflectedType);

            // ((T)instance).Property
            MemberExpression memberExpression = Expression.Property(instanceCast, methodInfo);

            // 最后需要将结果转换为 object，以符合方法的签名
            // (object)(((T)instance).Property)
            UnaryExpression castPropertyValue = Expression.Convert(memberExpression, typeof(object));

            Expression<Func<object, object>> lambda = Expression.Lambda<Func<object, object>>(castPropertyValue, instanceExpression);

            _getter = lambda.Compile();
        }

        public object GetValue(object obj)
        {
            return _getter(obj);
        }
    }
}
