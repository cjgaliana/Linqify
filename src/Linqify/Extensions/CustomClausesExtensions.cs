using System.Linq;

namespace Linqify
{
    public static class CustomClausesExtensions
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> query, string parameterName)
        {
            var apiQuery = query as LinqifyQueryable<T>;
            if (apiQuery == null)
            {
                return query;
            }

            apiQuery.AddCustomApiParameter(
                new CustomApiParameter
                {
                    Key = "Include",
                    Value = parameterName
                });

            return apiQuery;
        }

        public static IQueryable<T> AddParameter<T>(this IQueryable<T> query, string key, object value)
        {
            var apiQuery = query as LinqifyQueryable<T>;
            if (apiQuery == null)
            {
                return query;
            }

            apiQuery.AddCustomApiParameter(
                new CustomApiParameter
                {
                    Key = key,
                    Value = value
                });
            return apiQuery;
        }
    }
}