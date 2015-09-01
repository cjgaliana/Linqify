using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Linqify
{
    /// <summary>
    ///     IQueryable of T part of the REST API
    /// </summary>
    /// <typeparam name="T">Type to operate on</typeparam>
    public class LinqifyQueryable<T> : IOrderedQueryable<T>
    {
        /// <summary>
        ///     init with LinqifyContext
        /// </summary>
        /// <param name="context"></param>
        public LinqifyQueryable(LinqifyContext context)
        {
            this.Provider = new LinqifyQueryProvider();
            this.Expression = Expression.Constant(this);
            this.CustomParameters = new List<CustomApiParameter>();

            // lets provider reach back to LinqifyContext,
            // where execute implementation resides
            ((LinqifyQueryProvider)this.Provider).Context = context;
        }

        /// <summary>
        ///     modified as internal because LINQ to API is Unusable
        ///     without LinqifyContext, but provider still needs access
        /// </summary>
        /// <param name="provider">IQueryProvider</param>
        /// <param name="expression">Expression Tree</param>
        internal LinqifyQueryable(LinqifyQueryProvider provider, Expression expression)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (!typeof(IQueryable<T>).GetTypeInfo().IsAssignableFrom(expression.Type.GetTypeInfo()))
            {
                throw new ArgumentOutOfRangeException("expression");
            }

            this.Provider = provider;
            this.Expression = expression;
            this.CustomParameters = new List<CustomApiParameter>();
        }

        public IList<CustomApiParameter> CustomParameters { get; private set; }

        /// <summary>
        ///     IQueryProvider part of LINQ to API
        /// </summary>
        public IQueryProvider Provider { get; private set; }

        /// <summary>
        ///     expression tree
        /// </summary>
        public Expression Expression { get; private set; }

        /// <summary>
        ///     type of T in IQueryable of T
        /// </summary>
        public Type ElementType
        {
            get { return typeof(T); }
        }

        /// <summary>
        ///     executes when iterating over collection
        /// </summary>
        /// <returns>query results</returns>
        public IEnumerator<T> GetEnumerator()
        {
            Task<object> tsk =
                Task.Run(() => (((LinqifyQueryProvider)this.Provider).ExecuteAsync<IEnumerable<T>>(this.Expression)));
            return ((IEnumerable<T>)tsk.Result).GetEnumerator();
        }

        /// <summary>
        ///     non-generic execution when collection is iterated over
        /// </summary>
        /// <returns>query results</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this.Provider.Execute<IEnumerable>(this.Expression)).GetEnumerator();
        }

        public void AddCustomApiParameter(CustomApiParameter parameter)
        {
            if (this.CustomParameters == null)
            {
                this.CustomParameters = new List<CustomApiParameter>();
            }

            this.CustomParameters.Add(parameter);
        }
    }
}