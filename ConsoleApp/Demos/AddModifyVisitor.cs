using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Demos
{
    /// <summary>
    /// 将 AndAlso 转换为 OrElse
    /// </summary>
    public class AndAlsoModifier : ExpressionVisitor
    {
        /// <summary>
        /// 在构造函数中调用父类的虚方法 Visit
        /// </summary>
        /// <param name="expression">需要更改的表达式树</param>
        /// <returns></returns>
        public Expression Modify(Expression expression)
        {
            return Visit(expression);
        }

        /// <summary>
        /// 重写二元操作方法
        /// 重写后返回的还是一个二元操作
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression b)
        {
            if (b.NodeType == ExpressionType.AndAlso)
            {
                Expression left = this.Visit(b.Left);
                Expression right = this.Visit(b.Right);

                // Make this binary expression an OrElse operation instead of an AndAlso operation. 
                return Expression.MakeBinary(ExpressionType.OrElse, left, right, b.IsLiftedToNull, b.Method);
            }

            return base.VisitBinary(b);
        }
    }
}
