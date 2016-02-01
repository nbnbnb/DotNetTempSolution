using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Concrete
{
    /// <summary>
    /// 动态执行方法
    /// </summary>
    public class DynamicMethodExecutor
    {
        // 存储的执行方法委托
        // 第一个参数为执行对象
        // 第二个参数为参数列表
        // 第三个参数为返回值
        private Func<object, object[], object> _execute;

        public DynamicMethodExecutor(MethodInfo methodInfo)
        {
            this._execute = this.GetExecuteDelegate(methodInfo);
        }

        public DynamicMethodExecutor(Type type, string methodName) :
            this(type.GetMethod(methodName))
        { }

        public object Execute(object instance, object[] parameters)
        {
            return this._execute(instance, parameters);
        }

        private Func<object, object[], object> GetExecuteDelegate(MethodInfo methodInfo)
        {
            // 构造一个 ((T)instance).Method(parameters) 的表达式树
            // 构造表达式需要的参数
            ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
            ParameterExpression parameters = Expression.Parameter(typeof(object[]), "parameters");

            // 将方法参数转换为表达式参数
            // 需要将参数转换为合适的表达式类型，以符合方法签名
            List<Expression> paramExpression = new List<Expression>();
            var parameterInfos = methodInfo.GetParameters();
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                // 需要将参数转型为实际的类型，以符合正确的方法签名
                // 注意索引的使用方式
                // 获取一个参数值  var para=parameters[i];
                BinaryExpression paraObj = Expression.ArrayIndex(parameters, Expression.Constant(i));
                // 对参数进行转型
                // var castVal=(ParameterType)para
                UnaryExpression paraCastVal = Expression.Convert(paraObj, parameterInfos[i].ParameterType);
                // 最后添加到方法调用参数数组中的表示为 (ParameterType)(parameters[i])          
                paramExpression.Add(paraCastVal);
            }

            // 将实例转型后，完成方法的调用
            // 如果是静态方法，则设置 InstanceExpression 为 null
            // ((T)instance)
            UnaryExpression instanceCast = methodInfo.IsStatic ? null : Expression.Convert(instance, methodInfo.DeclaringType);

            // 方法调用
            MethodCallExpression callExpression = Expression.Call(instanceCast, methodInfo, paramExpression);

            // 需要判断方法的返回值
            // void 也是一种类型
            if (methodInfo.ReturnType == typeof(void))
            {
                Expression<Action<object, object[]>> lambda =
                    Expression.Lambda<Action<object, object[]>>(callExpression, instance, parameters);

                Action<object, object[]> action = lambda.Compile();

                // 转换为一个 Func<object, object[], null> 返回
                return (ins, para) =>
                {
                    action(ins, para);

                    // 返回一个空
                    return null;
                };
            }
            else
            {
                // 此次需要将方法的执行结果转换为 object，以符合返回值的签名
                UnaryExpression castMethodCall = Expression.Convert(callExpression, typeof(object));

                Expression<Func<object, object[], object>> lambda =
                    Expression.Lambda<Func<object, object[], object>>(castMethodCall, instance, parameters);
                
                // 返回最终需要的委托
                return lambda.Compile();
            }
        }
    }
}