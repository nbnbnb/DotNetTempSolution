using CommonLib.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Demos
{
    public class ExpressionTree
    {
        public static void Start()
        {
            Demo08();
        }

        /// <summary>
        /// 通过 lambda 创建表达式树
        /// 基本语句
        /// </summary>
        private static void Demo01()
        {
            Expression<Func<int, bool>> lambda = num => num < 5;

            // 将表达式树描述的 lambda 表达式编译为可执行代码
            // 并生成表示该 lambda 表达式的委托
            Func<int, bool> func = lambda.Compile();

            bool res = func(2);

            Console.WriteLine(res);
        }

        /// <summary>
        /// 通过 API 创建表达式树 
        /// 基本语句
        /// Expression.Parameter
        /// Expression.Constant
        /// Expression.MakeBinary
        /// </summary>
        private static void Demo02()
        {
            ParameterExpression num = Expression.Parameter(typeof(int), "num");
            ConstantExpression five = Expression.Constant(5, typeof(int));
            BinaryExpression lessThan = Expression.MakeBinary(ExpressionType.LessThan, num, five);

            Expression<Func<int, bool>> lambda = Expression.Lambda<Func<int, bool>>(lessThan, num);

            Func<int, bool> func = lambda.Compile();

            bool res = func(2);

            Console.WriteLine(res);
        }

        /// <summary>
        /// 通过 API 创建表达式树
        /// loop
        /// Expression.Parameter
        /// Expression.Label
        /// Expression.Block
        /// Expression.Loop
        /// Expression.IfThenElse
        /// Expression.Assign
        /// Expression.GreaterThan
        /// Expression.MultiplyAssign
        /// Expression.PostDecrementAssign
        /// Expression.Break
        /// </summary>
        private static void Demo03()
        {
            ParameterExpression value = Expression.Parameter(typeof(int), "value");
            ParameterExpression result = Expression.Parameter(typeof(int), "result");

            // 这个用于设置循环体中用于中断的目标
            // 指定跳转到标签时传递的值的类型
            // 这个类型同时也指定了 lambda 的返回值
            LabelTarget label = Expression.Label(typeof(int));

            Expression loopExpression = Expression.IfThenElse(
                    Expression.GreaterThan(value, Expression.Constant(1)),  // test
                    Expression.MultiplyAssign(result, Expression.PostDecrementAssign(value)),  // if true【result*=value--】
                    Expression.Break(label, result) // if false 跳转到指定的标签，同时将结果值传递给标签
                );

            BlockExpression block = Expression.Block(
                // 声明代码块中要使用的变量
                // 注意，这个不是参数，而是 loop 语句中的局部变量
                // 参数在 Expression.Lambda 方法时指定
                    new[] { result },

                    // 下面的为表达式语句
                // 给变量赋值
                    Expression.Assign(result, Expression.Constant(1)),

                    // 定义一个循环语句
                    Expression.Loop(loopExpression, label)
                );

            // 指定表达式语句 bloac
            // 和参数 value
            int factorial = Expression.Lambda<Func<int, int>>(block, value).Compile()(6);

            Console.WriteLine(factorial);
        }

        /// <summary>
        /// 构造 for 语句块
        /// </summary>
        private static void Demo04()
        {
            //设置语句原型
            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine(i);
            //}

            MethodInfo cw = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) });
            ParameterExpression i = Expression.Parameter(typeof(int), "i");

            // 这个跳出是不需要返回值的
            LabelTarget label = Expression.Label();

            // 由于是静态方法，所以传递为 null
            Expression writeLine = Expression.Call(null, cw, i);
            Expression decrementII = Expression.PostDecrementAssign(i); // 对 i 递减，并赋值

            BlockExpression blockWrite = Expression.Block(writeLine, decrementII);

            // 此处在条件判断中添加了 Block 块
            Expression condition = Expression.IfThenElse(
                Expression.GreaterThanOrEqual(i, Expression.Constant(0)),
                blockWrite,  // 执行方法
                Expression.Break(label));

            // 将 Label 传递到 Loop 中
            Expression loop = Expression.Loop(condition, label);

            // 将 Loop 放到 Block 中
            BlockExpression forBlock = Expression.Block(loop);

            // 在 for 循环中使用的变量，从此次传递进去
            Expression.Lambda<Action<int>>(forBlock, i).Compile()(5);
        }

        /// <summary>
        /// 解析表达树
        /// </summary>
        private static void Demo05()
        {
            Expression<Func<int, bool>> lambda = num => num > 5;

            ParameterExpression param = lambda.Parameters[0];
            BinaryExpression binary = (BinaryExpression)lambda.Body;
            ParameterExpression left = (ParameterExpression)binary.Left;
            ConstantExpression right = (ConstantExpression)binary.Right;

            Console.WriteLine("Decomposed expression: {0} => {1} {2} {3}",
                              param.Name, left.Name, binary.NodeType, right.Value);
        }

        /// <summary>
        /// 修改表达式树
        /// </summary>
        private static void Demo06()
        {
            // 表达式树是不可变的，这意味着不能直接修改表达式树
            // 若要更改表达式树，必须创建现有表达式树的一个副本，并在创建副本的过程中执行所需更改
            // 您可以使用 ExpressionVisitor 类遍历现有表达式树，并复制它访问的每个节点

            Expression<Func<string, bool>> expr = name => name.Length > 10 && name.StartsWith("G");
            Console.WriteLine(expr); // name => ((name.Length > 10) && name.StartsWith("G"))

            AndAlsoModifier treeModifier = new AndAlsoModifier();
            Expression modifiedExpr = treeModifier.Modify((Expression)expr);

            Console.WriteLine(modifiedExpr); // name => ((name.Length > 10) || name.StartsWith("G"))
        }

        /// <summary>
        /// 一个复杂的表达式生成实例
        /// </summary>
        private static void Demo07()
        {
            // 原型
            /*
            Func<int, List<int>> getPrimes_Origin = to_para =>
             {
                 var res_para = new List<int>();
                 for (int n = 2; n <= to_para; n++)
                 {
                     bool found = false;

                     for (int d = 2; d <= Math.Sqrt(n); d++)
                     {
                         if (n % d == 0)
                         {
                             found = true;
                             break;
                         }
                     }

                     if (!found)
                         res_para.Add(n);
                 }
                 return res_para;
             };

            foreach (var num in getPrimes_Origin(100))
                Console.WriteLine(num);

            
             * 基本循环语句
             * for (initializer; condition; update) 
                   body
             * 
             * 等价于下面的语句
                { 
                    initializer; 
                    while (condition) 
                    { 
                        body; 
                        continueLabel: 
                        update; 
                    } 
                    breakLabel: 
                }
             * 
             * 在表达式树中没有终止循环条件，所以需要使用标签跳转的方式
                { 
                    initializer; 
                    while (true) 
                    { 
                        if (!condition) 
                            goto breakLabel; 
                        body; 
                        continueLabel: 
                        update; 
                    } 
                        breakLabel: 
                }
             * */

            // 这是输入 lambda 的两个表达式
            var to = Expression.Parameter(typeof(int), "to");
            var res = Expression.Variable(typeof(List<int>), "res");

            // 循环中使用到的参数
            var n = Expression.Variable(typeof(int), "n");
            var found = Expression.Variable(typeof(bool), "found");
            var d = Expression.Variable(typeof(int), "d");
            var breakOuter = Expression.Label();
            var breakInner = Expression.Label();


            var getPrimes =
                // Func<int,List<int>> getPrimes
                Expression.Lambda<Func<int, List<int>>>(
                // {
                    Expression.Block(
                // List<int> res;
                        new[] { res },
                // res=new List<int>;
                        Expression.Assign(
                            res,
                            Expression.New(typeof(List<int>))
                        ),
                // {
                        Expression.Block(
                // int n
                            new[] { n },
                // n=2
                            Expression.Assign(
                                n,
                                Expression.Constant(2)
                            ),
                // while(True)
                            Expression.Loop(
                // {
                                Expression.Block(
                // if
                                    Expression.IfThen(
                // (!
                                        Expression.Not(
                // (n<=to)
                                            Expression.LessThanOrEqual(
                                                n,
                                                to
                                            )
                                        ),
                // break
                                        Expression.Break(breakOuter)
                                    ),
                                    Expression.Block(
                // bool found;
                                        new[] { found },
                // found=false;
                                        Expression.Assign(
                                            found,
                                            Expression.Constant(false)
                                        ),
                // {
                                        Expression.Block(
                // int d;
                                            new[] { d },
                                            Expression.Assign(
                                                d,
                                                Expression.Constant(2)
                                            ),
                                            Expression.Loop(
                // {
                                                Expression.Block(
                // if
                                                    Expression.IfThen(
                // (!
                                                        Expression.Not(
                // d<=Math.Sqrt(n)
                                                            Expression.LessThanOrEqual(
                                                                d,
                                                                Expression.Convert(
                                                                    Expression.Call(
                                                                        null,
                                                                        typeof(Math).GetMethod("Sqrt"),
                                                                        Expression.Convert(
                                                                            n,
                                                                            typeof(double)
                                                                        )
                                                                    ),
                                                                    typeof(int)
                                                                )
                                                            )
                                                        ),
                // break
                                                       Expression.Break(breakInner)
                                                    ),
                                                    Expression.Block(
                // if(n%d==0)
                                                        Expression.IfThen(
                                                            Expression.Equal(
                                                                Expression.Modulo(
                                                                    n,
                                                                    d
                                                                ),
                                                                Expression.Constant(0)
                                                            ),
                                                            Expression.Block(
                // found=true;
                                                                Expression.Assign(
                                                                    found,
                                                                    Expression.Constant(true)
                                                                ),
                // break
                                                                Expression.Break(breakInner)
                                                            )
                                                        )
                                                    ),
                // d++
                                                    Expression.PostIncrementAssign(d)
                                                ),
                                                breakInner
                                            )
                                        ),
                                        Expression.IfThen(
                // (!found)
                                            Expression.Not(found),
                                            Expression.Call(
                                                res,
                                                typeof(List<int>).GetMethod("Add"),
                                                n
                                            )
                                        )
                                    ),
                // n++
                                    Expression.PostIncrementAssign(n)
                                ),
                                breakOuter
                            )
                        ),
                        res
                    ),
                    to
                ).Compile();
        }

        /// <summary>
        /// 一个 for(int i=0;i<100;i++) 的例子
        /// </summary>
        private static void Demo08()
        {
            // 这是传递给 lambda 的参数
            var to = Expression.Variable(typeof(int), "to");

            // 这是两个 Block 中使用的参数
            // 需要在 Block 中通过 new[]{ param } 的形式进行定义
            var res = Expression.Variable(typeof(List<int>), "res");
            var i = Expression.Variable(typeof(int), "i");

            var breakLabel = Expression.Label(typeof(List<int>));

            var getPrimes =
                // Func<int, List<int>> getPrimes =
                Expression.Lambda<Func<int, List<int>>>(
                // {
                    Expression.Block(
                // List<int> res;
                        new[] { res },
                // res = new List<int>();
                        Expression.Assign(
                            res,
                            Expression.New(typeof(List<int>))
                        ),
                        Expression.Loop(
                            Expression.Block(
                                new[] { i },
                                Expression.PostIncrementAssign(i),
                                Expression.IfThenElse(
                                    Expression.LessThanOrEqual(i, to),
                                    Expression.Block(
                                        Expression.Call(res, typeof(List<int>).GetMethod("Add"), i)
                                    ),
                                    Expression.Break(breakLabel, res)
                                )
                            ),
                            breakLabel
                        )
                    ),
                    to
                // }
                ).Compile();

            foreach (var num in getPrimes(100))
            {
                Console.WriteLine(num);
            }
        }
    }
}
