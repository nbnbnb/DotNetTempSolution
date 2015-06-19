using ConsoleApp.Demos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start.");
            Temp();
            Console.WriteLine("End.");
            Console.ReadKey();
        }

        private static void Temp()
        {
            //ExpressionTree.Start();
            DynamicQueryFeatures.MiscDemo();
            //Demo();
        }

        #region Demo
        private static void Demo()
        {

            var accessor = new DynamiePropertyAccessor<String>(typeof(UserName), "Name");

            UserName zhangjin = new UserName
            {
                Name = "ZhangJin"
            };

            string name = accessor.Execute(zhangjin);

            Console.WriteLine(name);
        }

        private class UserName
        {
            public string Name { get; set; }
        }

        private class DynamiePropertyAccessor<T>
        {
            private Func<object, T> _getter;

            public DynamiePropertyAccessor(Type type, string propertyName)
                : this(type.GetProperty(propertyName))
            {

            }

            public DynamiePropertyAccessor(PropertyInfo propertyInfo)
            {

                // 需要实现的原型为 ((T)instance).Property

                // 首先定义 lambda 的参数
                ParameterExpression instanceParam = Expression.Parameter(typeof(object), "instance");

                // 生成转型代码
                // (T)instance
                UnaryExpression instanceCast = Expression.Convert(instanceParam, propertyInfo.DeclaringType);

                // 读取属性
                MemberExpression propertyValue = Expression.Property(instanceCast, propertyInfo);

                // 生成复合委托类型的代码
                UnaryExpression propertyValueCast = Expression.Convert(propertyValue, typeof(T));

                _getter = Expression.Lambda<Func<object, T>>(propertyValueCast, instanceParam).Compile();
            }

            public T Execute(object obj)
            {
                return _getter(obj);
            }
        }
        #endregion
    }
}