namespace Enfo.Repository.Querying
{
    public static class SpecificationExtensions
    {
        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right) =>
            new AndSpecification<T>(left, right);
    }
}
