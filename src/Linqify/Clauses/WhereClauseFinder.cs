using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Linqify
{
    /// <summary>
    ///     finds where clauses in the expression tree
    /// </summary>
    public class WhereClauseFinder : ExpressionVisitor
    {
        private static readonly string[] WhereMethodNames =
        {
            "Where",
            "Single",
            "SingleOrDefault",
            "First",
            "FirstOrDefault"
        };

        // holds all where expressions
        private readonly List<MethodCallExpression> _whereExpressions = new List<MethodCallExpression>();

        /// <summary>
        ///     searches expression tree for wheres and returns collection of all it finds.
        /// </summary>
        /// <param name="expression">query expression to search.</param>
        /// <returns>collection of where expressions.</returns>
        public MethodCallExpression[] GetAllWheres(Expression expression)
        {
            this.Visit(expression);
            return this._whereExpressions.ToArray();
        }

        /// <summary>
        ///     custom processing of MethodCallExpression NodeType that checks for a
        ///     where clause and retains expression as member of list of where clauses.
        /// </summary>
        /// <param name="expression">a MethodCallExpression node from the expression tree</param>
        /// <returns>expression that was passed in</returns>
        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            if (WhereMethodNames.Contains(expression.Method.Name) && expression.Arguments.Count == 2)
            {
                this._whereExpressions.Add(expression);
            }

            this.Visit(expression.Arguments[0]);

            return expression;
        }
    }
}