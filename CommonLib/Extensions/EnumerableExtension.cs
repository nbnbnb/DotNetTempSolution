using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Extensions
{
    public static class EnumerableExtension
    {
        /// <summary>
        /// Enumerable 的 Sum 方法只支持部分类型【int,long,float,double,decimal】
        /// 不包括例如无符号值，byte 和 short，还有一些自定义类型【它们支持 + 操作符】
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T DynamicSum<T>(this IEnumerable<T> source)
        {
            dynamic total = default(T);
            foreach (T item in source)
            {
                // 由于 C# 编译器通常会在执行加法前将两个操作数提升为 int
                // 如果不强制转换，total 变量将保存为一个 int 值
                // 当返回语句试图将其转换为 byte 时将抛出异常
                total = (T)(total + item);
            }

            return total;
        }
    }
}
