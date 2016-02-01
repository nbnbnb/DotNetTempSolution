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
            // 期望的表达式原型
            // ((T)instance).Property

            // 首先创建参数 
            // instance
            ParameterExpression instanceExpression = Expression.Parameter(typeof(object), "instance");

            // 根据 MethodInfo 可以获得 (T)
            // 然后执行下面的一元转换
            // (T)instance
            UnaryExpression instanceCast = Expression.Convert(instanceExpression, methodInfo.ReflectedType);

            // 使用 Property 方法访问表达式属性
            // ((T)instance).Property
            MemberExpression memberExpression = Expression.Property(instanceCast, methodInfo);
            
            // 再次执行一元转换
            // 将结果转换为 object，以符合方法的签名
            // (object)(((T)instance).Property)
            UnaryExpression castPropertyValue = Expression.Convert(memberExpression, typeof(object));

            // 创建待执行的表达式树
            Expression<Func<object, object>> lambda = Expression.Lambda<Func<object, object>>(castPropertyValue, instanceExpression);

            // 编译表达式为委托
            // 委托已经经过 JIT 变成成可执行代码
            // 不会有反射的性能损失了
            _getter = lambda.Compile();

            // 通过属性获得相对应的 Setter 方法
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
