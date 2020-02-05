using System.Linq;
using Enfo.Domain.Querying;

namespace Enfo.Infrastructure.QueryingEvaluators
{
    public static class SortingEvaluator
    {
        internal static IOrderedQueryable<T> Apply<T>(
            this IQueryable<T> query,
            ISorting<T> sorting)
            where T : class
        {
            var orderedQuery = query.OrderBy(e => 1);

            // Apply sorting if expressions are set
            if (sorting?.OrderBy?.Count > 0)
            {
                foreach (var ordering in sorting.OrderBy)
                {
                    orderedQuery = orderedQuery.ApplyOrdering(ordering);
                }
            }

            return orderedQuery;
        }

        private static IOrderedQueryable<T> ApplyOrdering<T>(
            this IOrderedQueryable<T> orderedQuery,
            Ordering<T> ordering) =>
            ordering.SortDirection == SortDirection.Ascending
                ? orderedQuery.ThenBy(ordering.OrderByExpression)
                : orderedQuery.ThenByDescending(ordering.OrderByExpression);
    }
}
