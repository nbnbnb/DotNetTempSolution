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
        private Func<object, object[], object> _execute;

        public DynamicMethodExecutor(MethodInfo methodInfo)
        {
            this._execute = this.GetExecuteDelegate(methodInfo);
        }

        public object Execute(object instance, object[] parameters)
        {
            return this._execute(instance, parameters);
        }

        private Func<object, object[], object> GetExecuteDelegate(MethodInfo methodInfo)
        {
            ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
            ParameterExpression parameters = Expression.Parameter(typeof(object[]), "parameters");

            // 构造一个 ((T)instance).Method(parameters) 的 表达式树

            // 构造方法调用时的表达式树参数
            List<Expression> paramExpression = new List<Expression>();
            var parameterInfos = methodInfo.GetParameters();
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                // 需要将参数转型为实际的类型，以符合正确的方法签名
                //获取一个参数值  var para=parameters[i];
                BinaryExpression paraObj = Expression.ArrayIndex(parameters, Expression.Constant(i));
                // 对参数进行转型
                // var castVal=(ParameterType)para
                UnaryExpression paraCastVal = Expression.Convert(paraObj, parameterInfos[i].ParameterType);
                // 最后添加到方法调用参数数组中
                paramExpression.Add(paraCastVal);
            }

            // 将实例转型后，完成方法的调用
            // ((T)instance)
            UnaryExpression instanceCast = methodInfo.IsStatic ? null : Expression.Convert(instance, methodInfo.DeclaringType);

            // 方法调用
            MethodCallExpression callExpression = Expression.Call(instanceCast, methodInfo, paramExpression);

            // 需要判断方法的返回值
            if (methodInfo.ReturnType == typeof(void))
            {
                Expression<Action<object, object[]>> lambda =
                    Expression.Lambda<Action<object, object[]>>(callExpression, instance, parameters);

                Action<object, object[]> action = lambda.Compile();

                // 再次转换为一个 lambda 返回
                return (ins, para) =>
                {
                    action(ins, para);

                    // 返回一个空
                    return null;
                };
            }
            else
            {
                // 此次需要将方法的执行结果转换为 object，已符合返回值的签名
                UnaryExpression castMethodCall = Expression.Convert(callExpression, typeof(object));

                Expression<Func<object, object[], object>> lambda =
                    Expression.Lambda<Func<object, object[], object>>(castMethodCall, instance, parameters);

                return lambda.Compile();
            }
        }
    }
}