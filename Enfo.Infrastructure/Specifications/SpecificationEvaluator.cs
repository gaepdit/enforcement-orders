using Enfo.Domain.Entities;
using Enfo.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Enfo.Infrastructure.Specifications
{
    public static class SpecificationEvaluator
    {
        internal static IQueryable<TEntity> Apply<TEntity>(this IQueryable<TEntity> query, ISpecification<TEntity> specification)
            where TEntity : BaseEntity
        {
            // modify the IQueryable using the specification's criteria expression
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // Includes all expression-based includes
            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            // Include any string-based include statements
            query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            // Apply ordering if expressions are set
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.GroupBy != null)
            {
                query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
            }

            return query;
        }
    }
}
