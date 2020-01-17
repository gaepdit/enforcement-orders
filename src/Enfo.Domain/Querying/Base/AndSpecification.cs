using Enfo.Domain.Utils;

namespace Enfo.Domain.Querying
{
    public class AndSpecification<T> : Specification<T>
    {
        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            Check.NotNull(left, nameof(left));
            Check.NotNull(right, nameof(right));

            ApplyCriteria(left.Criteria);
            ApplyCriteria(right.Criteria);
        }
    }
}
