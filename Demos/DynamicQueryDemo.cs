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

            LambdaExpression exp_1 = DynamicExpression2.ParseLambda(
                new ParameterExpression[] { a, b },
                typeof(int),
                "a+b+@0+@1+c",  // @0 和 @1 表示的是索引参数，c 表示的是命名参数
                10,
                -4,
                args);

            Delegate dlg = exp_1.Compile();
            var sg = dlg.DynamicInvoke(1, 2);

            Console.WriteLine(sg);

            // 参数一定要在下面的构造前添加
            args.Add("address", "WuHane");

            // 另一个重载【针对一个实体】
            LambdaExpression exp_2 = DynamicExpression2.ParseLambda(
                typeof(Person), // 这就表示了，此表达式需要要给 Person 类型的参数
                typeof(bool),
                "it.Name=\"张进\" and Age=@0 and Address=address",
                30, // 索引参数
                args); // 命名参数

            dlg = exp_2.Compile();

            Console.WriteLine(dlg.DynamicInvoke(zj));

            // 泛型的重载【必须有一个返回值】

            Expression<Func<Person, bool>> exp_3 = DynamicExpression2.ParseLambda<Person, bool>(
                "it.Name=\"张进\" and Age=@0 and Address=address",
                30,
                args);
            Console.WriteLine(exp_3.Compile()(zj));

            // 上面的都是有指定了返回值类型
            // 可以不指定返回值，使用表达式中的返回值
            LambdaExpression exp_4 = DynamicExpression2.ParseLambda(
                 typeof(Person),
                 null, // 使用表达中的类型
                 "it.Age+15");
            dlg = exp_4.Compile();

            Console.WriteLine(dlg.DynamicInvoke(zj));
        }

        /// <summary>
        /// 使用 Parse 方法创建表达式片段【body】
        /// </summary>
        public static void ParseDemo()
        {
            // 这两个参数在 Parse 方法和 Lambda 方法中都会被引用
            ParameterExpression x = Expression.Parameter(typeof(int), "x");
            ParameterExpression y = Expression.Parameter(typeof(int), "y");

            Dictionary<string, object> symbols = new Dictionary<string, object>();
            symbols.Add("x", x);
            symbols.Add("y", y);

            // 参数传递为 null，表示依赖表示的返回值类型
            // 根据参数字典创建表达式片段
            Expression body = DynamicExpression2.Parse(null, "(x + y) * 2", symbols);

            LambdaExpression e = Expression.Lambda(body, new ParameterExpression[] { x, y });

            Delegate dlg = e.Compile();
            Console.WriteLine(dlg.DynamicInvoke(3, 4));
        }

        /// <summary>
        /// 动态创建类型【僵尸类】
        /// </summary>
        public static void CreateClassDemo()
        {
            // 这些创建的类在一个内存程序集中，不能被卸载
            // 类型的 Equals 和 GetHashCode 已被重新，比较的是值相等

            DynamicProperty name = new DynamicProperty("Name", typeof(String));
            DynamicProperty age = new DynamicProperty("Age", typeof(int));

            Type btking = DynamicExpression2.CreateClass(name, age);

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

            IQueryable<Person> query_1 = persons;
            IQueryable query_2 = persons;

            // IQueryable
            query_2.Select("it.Age");
            query_2.Where("it.Name=@0", "A");
            query_2.OrderBy("it.Name ascending,it.Age descending");
            query_2.GroupBy("Name", "Age");

            //下面的 4 个方法，IQueryable 原始是没有的，IQueryable<T> 是有的
            query_2.Take(30);
            query_2.Skip(20);
            query_2.Any();
            query_2.Count();

            // IQueryable<T>            
            query_1.Where("it.Name=@0", "A");
            query_1.OrderBy("it.Name ascending,it.Age descending");

            // IQueryable<T> 继承 IQueryable 非泛型的方法
            IQueryable group = query_1.GroupBy("Name", "Age");
            query_1.Select("it.Age");

            // 对于 GroupBy 方法，返回的信息需要进行强制类型转换
            IEnumerable<IGrouping<string, int>> ct = (IEnumerable<IGrouping<string, int>>)group;

            foreach (IGrouping<string, int> cc in ct)
            {
                Console.WriteLine(cc.Key);
                foreach (var ff in cc)  // 直接在 cc 上进行迭代，这个方法真心不好理解
                {
                    Console.WriteLine("   " + ff);
                }
            }
        }

        /// <summary>
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

            Expression<Func<Person, bool>> e1 = DynamicExpression2.ParseLambda<Person, bool>("Name=@0", "C");
            Expression<Func<Person, bool>> e2 = DynamicExpression2.ParseLambda<Person, bool>("Age=@0", 3);
            // 结合操作
            IQueryable<Person> query = persons.Where("@0(it) and @1(it)", e1, e2);

            Console.WriteLine(query.Count());
        }

        public static void IEnumerableExtensionDemo()
        {
            var tp = new List<Person>();
            tp.Add(new Person { Name = "A", Age = 1, Address = "AAA" });
            tp.Add(new Person { Name = "B", Age = 2, Address = "BBB" });
            tp.Add(new Person { Name = "C", Age = 3, Address = "CCC" });
            tp.Add(new Person { Name = "A", Age = 11, Address = "AAA-AAA" });


            People people = new People();
            people.Persons = tp;

            // 这些方法在 IEnumerable 中是被支持的
            //seq.Where(predicate)
            //seq.Any()
            //seq.Any(predicate)     
            //seq.All(predicate)
            //seq.Count()                                                
            //seq.Count(predicate)
            //seq.Min(selector)        
            //seq.Max(selector)
            //seq.Sum(selector)        
            //seq.Average(selector)


            LambdaExpression exp = DynamicExpression2.ParseLambda(
                    typeof(People),  // 这就表示了，此表达式需要要给 People 类型的参数，所以就不需要添加 ParameterExpression 了
                    null,
                    "it.Persons.Any()");

            Delegate dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke(people));

            exp = DynamicExpression2.ParseLambda(
                    typeof(People),  // 这就表示了，此表达式需要要给 People 类型的参数，所以就不需要添加 ParameterExpression 了
                    null,
                    "Persons.Where(it.Age==1).Count()");  // 链式调用

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke(people));

            exp = DynamicExpression2.ParseLambda(
                    typeof(People),
                    null,
                    "Persons.Min(it.Age)");

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke(people));

            exp = DynamicExpression2.ParseLambda(
                    typeof(People),
                    null,
                    "Persons.Max(it.Age)");

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke(people));

            exp = DynamicExpression2.ParseLambda(
                    typeof(People),
                    null,
                    "Persons.Average(it.Age)");

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke(people));
        }

        /// <summary>
        /// 一些杂项功能
        /// </summary>
        public static void MiscDemo()
        {
            LambdaExpression exp = DynamicExpression2.ParseLambda(
                            new ParameterExpression[] { },
                            null,
                            "DateTime(2007, 1, 1)");  // 创建时间


            Delegate dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

            exp = DynamicExpression2.ParseLambda(
                            new ParameterExpression[] { },
                            null,
                            "Guid.NewGuid()");  // 执行方法

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

            exp = DynamicExpression2.ParseLambda(
                            new ParameterExpression[] { },
                            null,
                            "Convert.ToBoolean(1)");  // 执行方法

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());
            // 这些基元方法都是可以执行的，静态或实例，同时还添加了  Convert 和 Math 这两个对象
            //Object
            //Boolean
            //Char
            //String
            //SByte
            //Byte
            //Int16
            //UInt16
            //Int32
            //UInt32
            //Int64
            //UInt64
            //Decimal
            //Single
            //Double
            //DateTime
            //TimeSpan
            //Guid

            exp = DynamicExpression2.ParseLambda(
                            new ParameterExpression[] { },
                            null,
                            "Math.Atan2(4,7)");  // 执行方法

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

            exp = DynamicExpression2.ParseLambda(
                            new ParameterExpression[] { },
                            null,
                            "new(123 as A,456 as B,\"张进\" as C)");  // 创建匿名对象

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

            exp = DynamicExpression2.ParseLambda(
                            new ParameterExpression[] { },
                            null,
                            "iif(!true,123,456)");  // 三目运算符, true false 常量

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

            exp = DynamicExpression2.ParseLambda(
                          new ParameterExpression[] { },
                          null,
                          "\"123\"[1]");  // 索引支持

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

            exp = DynamicExpression2.ParseLambda(
                        new ParameterExpression[] { },
                        null,
                        "\"abc\".ToUpper()");  // 实例方法

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

            exp = DynamicExpression2.ParseLambda(
                      new ParameterExpression[] { },
                      null,
                      "String.Concat(\"123\",\"456\")");  // 静态方法

            dlg = exp.Compile();
            Console.WriteLine(dlg.DynamicInvoke());

            exp = DynamicExpression2.ParseLambda(
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

        private class People
        {
            public IEnumerable<Person> Persons { get; set; }
        }

        public static LambdaExpression exp { get; set; }
    }
}
