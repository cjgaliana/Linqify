using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Linqify
{
    public static class AsyncExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> query)
        {
            var apiQuery = query as LinqifyQueryable<T>;
            var provider = query.Provider as LinqifyQueryProvider;
            var customParameters = apiQuery.CustomParameters;

            IEnumerable<T> results = (IEnumerable<T>)await provider.ExecuteAsync<IEnumerable<T>>(query.Expression, customParameters).ConfigureAwait(false);

            return results.ToList();
        }

        public static async Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> query)
            where T : class
        {
            var apiQuery = query as LinqifyQueryable<T>;
            var provider = query.Provider as LinqifyQueryProvider;
            var customParameters = apiQuery.CustomParameters;

            IEnumerable<T> results = (IEnumerable<T>)await provider.ExecuteAsync<T>(query.Expression, customParameters).ConfigureAwait(false);

            return results.FirstOrDefault();
        }

        public static async Task<T> FirstAsync<T>(this IQueryable<T> query)
            where T : class
        {
            var apiQuery = query as LinqifyQueryable<T>;
            var provider = query.Provider as LinqifyQueryProvider;
            var customParameters = apiQuery.CustomParameters;

            IEnumerable<T> results = (IEnumerable<T>)await provider.ExecuteAsync<T>(query.Expression, customParameters).ConfigureAwait(false);

            return results.First();
        }

        public static async Task<T> SingleOrDefaultAsync<T>(this IQueryable<T> query)
            where T : class
        {
            var apiQuery = query as LinqifyQueryable<T>;
            var provider = query.Provider as LinqifyQueryProvider;
            var customParameters = apiQuery.CustomParameters;

            IEnumerable<T> results = (IEnumerable<T>)await provider.ExecuteAsync<T>(query.Expression, customParameters).ConfigureAwait(false);

            return results.SingleOrDefault();
        }

        public static async Task<T> SingleAsync<T>(this IQueryable<T> query)
            where T : class
        {
            var apiQuery = query as LinqifyQueryable<T>;
            var provider = query.Provider as LinqifyQueryProvider;
            var customParameters = apiQuery.CustomParameters;

            IEnumerable<T> results = (IEnumerable<T>)await provider.ExecuteAsync<T>(query.Expression, customParameters).ConfigureAwait(false);

            return results.Single();
        }

        /// <summary>
        /// Enables use of .NET Cancellation Framework for this query.
        /// </summary>
        /// <returns>Streaming instance to support further LINQ opertations</returns>
        public static IQueryable<T> WithCancellation<T>(this IQueryable<T> query, CancellationToken cancelToken)
            where T : class
        {
            var provider = query.Provider as LinqifyQueryProvider;
            if (provider != null)
            {
                provider
                       .Context
                       .Executor
                       .CancellationToken = cancelToken;
            }

            return query;
        }
    }
}