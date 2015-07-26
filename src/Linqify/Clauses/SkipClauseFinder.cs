using System.Collections.Generic;
using System.Linq.Expressions;

namespace Linqify
{
    /// <summary>
    ///     finds TAKE clauses in the expression tree
    /// </summary>
    public class SkipClauseFinder : ExpressionVisitor
    {
        public static readonly string SkipMethodName = "Skip";

        // holds all take expressions
        private readonly List<MethodCallExpression> _skipExpressions = new List<MethodCallExpression>();

        /// <summary>
        ///     searches expression tree for "takes" and returns collection of all it finds.
        /// </summary>
        /// <param name="expression">query expression to search.</param>
        /// <returns>collection of take expressions.</returns>
        public MethodCallExpression[] GetAllSkips(Expression expression)
        {
            this.Visit(expression);
            return this._skipExpressions.ToArray();
        }

        /// <summary>
        ///     custom processing of MethodCallExpression NodeType that checks for a
        ///     where clause and retains expression as member of list of where clauses.
        /// </summary>
        /// <param name="expression">a MethodCallExpression node from the expression tree</param>
        /// <returns>expression that was passed in</returns>
        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            if (SkipMethodName.Equals(expression.Method.Name) && expression.Arguments.Count == 2)
            {
                this._skipExpressions.Add(expression);
            }

            this.Visit(expression.Arguments[0]);

            return expression;
        }
    }
}