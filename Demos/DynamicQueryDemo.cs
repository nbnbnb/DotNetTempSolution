using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;

namespace Demos
{
    public static class DynamicQueryDemo
    {
        /// <summary>
        /// 使用 ParseLambda 方法创建表达式树
        /// </summary>
        public static void ParseLambdaDemo()
        {
            // 一共有 3 个方法重载
            ParameterExpression a = Expression.Parameter(typeof(int), "a");
            ParameterExpression b = Expression.Parameter(typeof(int), "b");

            // 索引参数字典
            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add("c", 1000);

            Person zj = new Person
            {
                Name = "张进",
                Age = 30,
                Address = "WuHan"
            };

            LambdaExpression exp_1 = DynamicExpressionExt.ParseLambda(
                new ParameterExpression[] { a, b },  // 命名参数，参数值在调用时指定
                typeof(int), // 返回类型
                "a+b+@0+@1+c",
                10,  // @0
                -4,  // @1
                args // c
                );

            Delegate dlg = exp_1.Compile();
            object res = dlg.DynamicInvoke(1, 1);  // 传递的命名参数 a 和 b

            Console.WriteLine(res); // 1【a】+1【b】+10【@0】+(-4)【@1】+1000【c】

            // 参数一定要在下面的构造前添加
            args.Add("address", "WuHan");

            // 另一个重载【针对一个实体】
            LambdaExpression exp_2 = DynamicExpressionExt.ParseLambda(
                typeof(Person), // 指定 itType，这就表示了，此表达式需要传递 Person 类型的参数
                typeof(bool),
                "it.Name=\"张进\" and Age=@0 and Address=address",  // 使用 it 表示实体对象
                30, // @0
                args // address
                );

            dlg = exp_2.Compile();

            Console.WriteLine(dlg.DynamicInvoke(zj));  // 传递的 zj 为 Person 类型

            // 第一个参数为 Person 类型
            // 返回类型为 bool
            Expression<Func<Person, bool>> exp_3 = DynamicExpressionExt.ParseLambda<Person, bool>(
                "it.Name=\"张进\" and Age=@0 and Address=address",
                30,
                args);
            Console.WriteLine(exp_3.Compile()(zj));

            // 上面的都是有指定了返回值类型
            // 可以不指定返回值，使用表达式中的返回值
            LambdaExpression exp_4 = DynamicExpressionExt.ParseLambda(
                 typeof(Person), // itType
                 null, // resultType，设置为 null，则会使用表达中的类型
                 "it.Age+15" // 使用表达式的结果作为返回值
                 );
            dlg = exp_4.Compile();

            Console.WriteLine(dlg.DynamicInvoke(zj));
        }

        /// <summary>
        /// 使用 Parse 方法创建表达式树
        /// </summary>
        public static void ParseDemo()
        {
            // 这两个参数在 Parse 方法和 Lambda 方法中都会被引用
            ParameterExpression m = Expression.Parameter(typeof(int), "m");
            ParameterExpression n = Expression.Parameter(typeof(int), "n");

            Dictionary<string, object> symbols = new Dictionary<string, object>();
            symbols.Add("x", m); // 方法调用时指定的参数
            symbols.Add("y", n); // 方法调用时指定的参数

            // 参数传递为 null，表示依赖表示的返回值类型
            // 根据参数字典创建表达式片段
            Expression body = DynamicExpressionExt.Parse(
                null,  // 设置为 null，表示使用表达式结果的返回值类型
                "(x + y) * 2",
                symbols  // 参数字典 x、y 表示的是两个命名参数，其值在方法调用时指定
                );

            LambdaExpression e = Expression.Lambda(body, m, n);

            Delegate dlg = e.Compile();
            Console.WriteLine(dlg.DynamicInvoke(3, 4));  // 传递命名参数 m 和 n
        }

        /// <summary>
        /// 动态创建类型
        /// 默认 ToString 返回为 JSON 字符串
        /// </summary>
        public static void CreateClassDemo()
        {
            // 这些创建的类在一个内存程序集中，不能被卸载
            // 类型的 Equals 和 GetHashCode 已被重新，比较的是值相等

            DynamicProperty name = new DynamicProperty("Name", typeof(String));
            DynamicProperty age = new DynamicProperty("Age", typeof(int));

            Type btking = DynamicExpressionExt.CreateClass(name, age);

            // 动态程序集
            // DynamicClasses, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
            Console.WriteLine(btking.Assembly.FullName);

            dynamic obj = Activator.CreateInstance(btking);
            obj.Name = "ZhangJin";  // 使用 Dynamic 方式进行赋值
            btking.GetProperty("Age").SetValue(obj, 30);  // 使用传统反射反射进行赋值
            Console.WriteLine(obj.Age);
            Console.WriteLine(obj.Name);

            // 已经重写了 ToString 方法
            // {Name=ZhangJin, Age=30}
            Console.WriteLine(obj);
        }

        /// <summary>
        /// 对 IQueryable 类型添加的扩展方法
        /// 使之支持字符串方式的重载
        /// </summary>
        public static void IQueryableExtensionMethodsDemo()
        {
            var tp = new List<Person>();
            tp.Add(new Person { Name = "A", Age = 1, Address = "AAA" });
            tp.Add(new Person { Name = "B", Age = 2, Address = "BBB" });
            tp.Add(new Person { Name = "C", Age = 3, Address = "CCC" });
            tp.Add(new Person { Name = "A", Age = 11, Address = "AAA-AAA" });

            IQueryable<Person> persons = tp.AsQueryable();

            // 泛型的 IQueryable
            IQueryable<Person> query_1 = persons;

            // 非泛型的 IQueryable
            IQueryable query_2 = persons;

            // IQueryable
            query_2.Select("it.Age");
            query_2.Where("it.Name=@0", "A");
            query_2.OrderBy("it.Name ascending,it.Age descending");
            query_2.GroupBy("Name", "Age");

            // 下面的 4 个方法，IQueryable 原始是没有的，IQueryable<T> 是有的
            // 此处都是使用的自定义扩展方法
            query_2.Take(30);
            query_2.Skip(20);
            query_2.Any();
            query_2.Count();

            // IQueryable<T>            
            query_1.Where("it.Name=@0", "A");
            query_1.OrderBy("it.Name ascending,it.Age descending");

            // IQueryable<T> 继承 IQueryable 非泛型的方法
            IQueryable group = query_1.GroupBy("Name", "Age");  // Name 为 keySelector，Age 为 elementSelector

            // 对于 GroupBy 返回的对象，强类型转换一下，即可获取其中内容
            IEnumerable<IGrouping<string, int>> items = (IEnumerable<IGrouping<string, int>>)group;

            foreach (IGrouping<string, int> item in items)
            {
                Console.WriteLine(item.Key);
                foreach (int age in item) // 在 IGrouping<string, int> 进行迭代
                {
                    Console.WriteLine(" Age=" + age);
                }
            }
        }

        /// <summary>
        /// 组合多个字符串查询表达式
        /// Lambda 表达式的动态调用
        /// </summary>
        public static void DynamicInvocationDemo()
        {
            var tp = new List<Person>();
            tp.Add(new Person { Name = "A", Age = 1, Address = "AAA" });
            tp.Add(new Person { Name = "B", Age = 2, Address = "BBB" });
            tp.Add(new Person { Name = "C", Age = 3, Address = "CCC" });
            tp.Add(new Person { Name = "A", Age = 11, Address = "AAA-AAA" });

            IQueryable<Person> persons = tp.AsQueryable();

            Expression<Func<Person, bool>> e1 = DynamicExpressionExt.ParseLambda<Person, bool>("Name=@0", "C");
            Expression<Func<Person, bool>> e2 = DynamicExpressionExt.ParseLambda<Person, bool>("Age=@0", 3);

            // 组合多个表达式
            // 注意，此处需要设置 (it)
            IQueryable<Person> query = persons.Where("@0(it) and @1(it)", e1, e2);

            Console.WriteLine(query.Count());
        }

        /// <summary>
        /// 让 IEnumerable 支持字符串查询条件
        /// </summary>
        public static void IEnumerableExtensionDemo()
        {
            var persons = new List<Person>();
            persons.Add(new Person { Name = "A", Age = 1, Address = "AAA" });
            persons.Add(new Person { Name = "B", Age = 2, Address = "BBB" });
            persons.Add(new Person { Name = "C", Age = 3, Address = "CCC" });
            persons.Add(new Person { Name = "A", Age = 11, Address = "AAA-AAA" });

            MySource source = new MySource();
            source.Persons = persons;

            // 这些方法在 IEnumerable 中是被支持的
            // seq.Where(predicate)
            // seq.Any()
            // seq.Any(predicate)     
            // seq.All(predicate)
            // seq.Count()                                                
            // seq.Count(predicate)
            // seq.Min(selector)        
            // seq.Max(selector)
            // seq.Sum(selector)        
            // seq.Average(selector)

            LambdaExpression exp = DynamicExpressionExt.ParseLambda(
                    typeof(MySource),  // itType
                    null,
                    "it.Persons.Any()");

            Delegate dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke(source));

            exp = DynamicExpressionExt.ParseLambda(
                    typeof(MySource),  // itType
                    null,
                    "Persons.Where(it.Age==1).Count()");  // 链式调用

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke(source));

            exp = DynamicExpressionExt.ParseLambda(
                    typeof(MySource),
                    null,
                    "Persons.Min(it.Age)");

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke(source));

            exp = DynamicExpressionExt.ParseLambda(
                    typeof(MySource),
                    null,
                    "Persons.Max(it.Age)");

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke(source));

            exp = DynamicExpressionExt.ParseLambda(
                    typeof(MySource),
                    null,
                    "Persons.Average(it.Age)");

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke(source));
        }

        /// <summary>
        /// 一些杂项功能
        /// </summary>
        public static void MiscDemo()
        {
            LambdaExpression exp = DynamicExpressionExt.ParseLambda(
                            new ParameterExpression[] { },
                            null,
                            "DateTime(2007, 1, 1)");  // 创建时间


            Delegate dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

            exp = DynamicExpressionExt.ParseLambda(
                            new ParameterExpression[] { },
                            null,
                            "Guid.NewGuid()");  // 执行方法

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

            exp = DynamicExpressionExt.ParseLambda(
                            new ParameterExpression[] { },
                            null,
                            "Convert.ToBoolean(1)");  // 执行方法

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());
            // 这些基于类型方法都是可以执行的，静态或实例，同时还添加了 Convert 和 Math 这两个对象
            // Object
            // Boolean
            // Char
            // String
            // SByte
            // Byte
            // Int16
            // UInt16
            // Int32
            // UInt32
            // Int64
            // UInt64
            // Decimal
            // Single
            // Double
            // DateTime
            // TimeSpan
            // Guid

            exp = DynamicExpressionExt.ParseLambda(
                            new ParameterExpression[] { },
                            null,
                            "Math.Atan2(4,7)");  // 执行方法

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

            exp = DynamicExpressionExt.ParseLambda(
                            new ParameterExpression[] { },
                            null,
                            "new(123 as A,456 as B,\"张进\" as C)");  // 创建匿名对象

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

            exp = DynamicExpressionExt.ParseLambda(
                            new ParameterExpression[] { },
                            null,
                            "iif(!true,123,456)");  // 三目运算符, true false 常量

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

            exp = DynamicExpressionExt.ParseLambda(
                          new ParameterExpression[] { },
                          null,
                          "\"123\"[1]");  // 索引支持

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

            exp = DynamicExpressionExt.ParseLambda(
                        new ParameterExpression[] { },
                        null,
                        "\"abc\".ToUpper()");  // 实例方法

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

            exp = DynamicExpressionExt.ParseLambda(
                      new ParameterExpression[] { },
                      null,
                      "String.Concat(\"123\",\"456\")");  // 静态方法

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

            exp = DynamicExpressionExt.ParseLambda(
                      new ParameterExpression[] { },
                      null,
                      "\"abc\".Length");  // 属性访问

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

        }

        private class Person
        {
            public string Name { get; set; }

            public int Age { get; set; }

            public string Address { get; set; }
        }

        private class MySource
        {
            public IEnumerable<Person> Persons { get; set; }
        }
    }
}
