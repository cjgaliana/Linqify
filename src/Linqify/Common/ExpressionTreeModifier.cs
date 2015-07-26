using System.Linq;
using System.Linq.Expressions;

namespace Linqify
{
    internal class ExpressionTreeModifier<T> : ExpressionVisitor
    {
        private readonly IQueryable<T> _queryableItems;

        internal ExpressionTreeModifier(IQueryable<T> items)
        {
            this._queryableItems = items;
        }

        internal Expression CopyAndModify(Expression expression)
        {
            return this.Visit(expression);
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c.Type.Name == "LinqifyQueryable`1")
            {
                return Expression.Constant(this._queryableItems);
            }

            return c;
        }
    }
}