using System.Linq;
using Enfo.Repository.Querying;

namespace Enfo.Infrastructure.QueryingEvaluators
{
    public static class PaginationEvaluator
    {
        internal static IQueryable<T> Apply<T>(
            this IQueryable<T> query,
            IPagination pagination)
        {
            // Apply pagination
            if (pagination != null && pagination.IsPagingEnabled)
            {
                query = query.Skip(pagination.Skip).Take(pagination.Take);
            }

            return query;
        }
    }
}
