using Enfo.Repository.Utils;

namespace Enfo.Repository.Querying
{
    public class AndSpecification<T> : Specification<T>
    {
        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            Guard.NotNull(left, nameof(left));
            Guard.NotNull(right, nameof(right));

            ApplyCriteria(left.Criteria);
            ApplyCriteria(right.Criteria);
        }
    }
}
