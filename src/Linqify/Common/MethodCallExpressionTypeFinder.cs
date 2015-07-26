using System;
using System.Linq.Expressions;

namespace Linqify
{
    public class MethodCallExpressionTypeFinder : ExpressionVisitor
    {
        private Type _genericType;

        /// <summary>
        ///     Gets the underlying type of the whole method call expression
        /// </summary>
        /// <param name="exp">MethodCallExpression</param>
        /// <returns>Type</returns>
        public Type GetGenericType(Expression exp)
        {
            var expresion = this.Visit(exp);
            if (expresion != null)
            {
                if (expresion.Type.GenericTypeArguments == null)
                {
                    return expresion.Type;
                }

                return expresion.Type.GenericTypeArguments[0];
            }

            return this._genericType;
        }

        /// <summary>
        ///     Sets the expression type when found
        /// </summary>
        /// <param name="expression">a MethodCallExpression node from the expression tree</param>
        /// <returns>expression that was passed in</returns>
        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            if (expression.Arguments.Count > 0)
            {
                this._genericType = expression.Method.GetGenericArguments()[0];
            }

            // look at extension source to see if there is an inner type
            this.Visit(expression.Arguments[0]);

            return expression;
        }
    }
}