using Enfo.Domain.Entities;
using Enfo.Domain.Querying;
using System.Linq;

namespace Enfo.Infrastructure.QueryingEvaluators
{
    public static class PaginationEvaluator
    {
        internal static IQueryable<T> Apply<T>(
            this IQueryable<T> query,
            IPagination pagination)
            where T : BaseEntity
        {
            // Apply pagination
            if (pagination == null || !pagination.IsPagingEnabled)
            {
                return query;
            }

            return query.Skip(pagination.Skip).Take(pagination.Take);
        }
    }
}
