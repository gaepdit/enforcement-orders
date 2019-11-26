using Enfo.Domain.Entities;
using Enfo.Domain.Querying;
using System.Linq;

namespace Enfo.Infrastructure.QueryingEvaluators
{
    public static class SortingEvaluator
    {
        internal static IQueryable<T> Apply<T>(this IQueryable<T> query, ISorting<T> sorting)
            where T : BaseEntity
        {
            if (sorting == null)
            {
                return query;
            }

            // Apply sorting if expressions are set
            if (sorting.OrderBy != null)
            {
                query = query.OrderBy(sorting.OrderBy);
            }
            else if (sorting.OrderByDescending != null)
            {
                query = query.OrderByDescending(sorting.OrderByDescending);
            }

            if (sorting.GroupBy != null)
            {
                query = query.GroupBy(sorting.GroupBy).SelectMany(x => x);
            }

            return query;
        }
    }
}
