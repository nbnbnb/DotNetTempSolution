using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Concrete
{
    public class CharToStringModifier : ExpressionVisitor
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
            if (b.NodeType == ExpressionType.Equal)
            {
                Expression left = this.Visit(b.Left);
                Expression right = this.Visit(b.Right);
                if (left.NodeType == ExpressionType.Convert &&
                    right.NodeType == ExpressionType.Constant)
                {
                    ConstantExpression express = (ConstantExpression)right;
                    if (express.Value is Int32)
                    {
                        left = Expression.Convert(left, typeof(char));
                        right = Expression.Convert(right, typeof(char));

                        return Expression.MakeBinary(ExpressionType.Equal, left, right);
                    }
                }

                if (right.NodeType == ExpressionType.Convert &&
                    left.NodeType == ExpressionType.Constant)
                {
                    ConstantExpression express = (ConstantExpression)left;
                    if (express.Value is Int32)
                    {
                        right = Expression.Convert(right, typeof(char));
                        left = Expression.Convert(left, typeof(char));

                        return Expression.MakeBinary(ExpressionType.Equal, left, right);
                    }
                }
            }

            return base.VisitBinary(b);
        }
    }
}
