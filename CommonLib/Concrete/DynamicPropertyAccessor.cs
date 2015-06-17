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

        private DynamicMethodExecutor _setter;

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

            MethodInfo setMethod = methodInfo.GetSetMethod();
            if (null != setMethod)
            {
                _setter = new DynamicMethodExecutor(setMethod);
            }

        }

        public object GetValue(object obj)
        {
            return _getter(obj);
        }

        public void SetValue(object obj, object value)
        {
            if (_setter == null)
            {
                throw new NotSupportedException("Can not set the property");
            }
            _setter.Execute(obj, new object[] { value });
        }
    }
}
