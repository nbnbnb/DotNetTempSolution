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

            var to = Expression.Parameter(typeof(int), "to");
            var res = Expression.Variable(typeof(List<int>), "res");
            var i = Expression.Parameter(typeof(int), "i");

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

            var gg = getPrimes(100);

            foreach (var num in getPrimes(100))
                Console.WriteLine(num);

        }
    }
}