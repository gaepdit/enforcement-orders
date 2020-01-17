using Enfo.Domain.Querying;
using System.Linq;

namespace Enfo.Infrastructure.QueryingEvaluators
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> Apply<T>(
            this IQueryable<T> query,
            ISpecification<T> specification)
        {
            // modify the IQueryable using the specification's criteria expression
            if (specification?.Criteria?.Count > 0)
            {
                foreach (var criterion in specification.Criteria)
                {
                    query = query.Where(criterion);
                }
            }

            return query;
        }
    }
}
