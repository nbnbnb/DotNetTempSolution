using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Dynamic
{
    public class DynamicPropertyAccessor
    {
        private Func<object, object> m_getter;
        private DynamicMethodExecutor m_dynamicSetter;

        public DynamicPropertyAccessor(Type type, string propertyName)
            : this(type.GetProperty(propertyName))
        {

        }

        public DynamicPropertyAccessor(PropertyInfo propertyInfo)
        {
            // target (object)((({TargetType}).instance).{Property})

            // preparing parameter, object type
            ParameterExpression instance = Expression.Parameter(typeof(object), "instance");

            // ({TargetType})instance
            Expression instanceCast = Expression.Convert(instance, propertyInfo.ReflectedType);

            // (({TargetType})instance).{Property}
            Expression propertyAccess = Expression.Property(instanceCast, propertyInfo);

            // (object)((({TargetType})instance).{Property})
            UnaryExpression castPropertyValue = Expression.Convert(propertyAccess, typeof(object));

            // Lambda express
            Expression<Func<object, object>> lambda =
                Expression.Lambda<Func<object, object>>(castPropertyValue, instance);

            // 生成一个访问属性的委托
            this.m_getter = lambda.Compile();

            MethodInfo setMethod = propertyInfo.GetSetMethod();
            if (setMethod != null)
            {
                this.m_dynamicSetter = new DynamicMethodExecutor(setMethod);
            }
        }

        public object GetValue(object o)
        {
            return this.m_getter(o);
        }

        public void SetValue(object o, object value)
        {
            if (this.m_dynamicSetter == null)
            {
                throw new NotSupportedException("Cannot set the property");
            }

            this.m_dynamicSetter.Execute(o, new object[] { value });
        }
    }
}
