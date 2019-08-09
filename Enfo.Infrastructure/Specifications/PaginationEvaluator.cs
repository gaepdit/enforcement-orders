using Enfo.Domain.Entities;
using Enfo.Domain.Specifications;
using System.Linq;

namespace Enfo.Infrastructure.Specifications
{
    public static class PaginationEvaluator
    {
        public static IQueryable<TEntity> Apply<TEntity>(this IQueryable<TEntity> query, IPagination pagination)
            where TEntity : BaseEntity
        {
            if (pagination.IsPagingEnabled)
            {
                return query.Skip(pagination.Skip).Take(pagination.Take);
            }

            return query;
        }
    }
}
