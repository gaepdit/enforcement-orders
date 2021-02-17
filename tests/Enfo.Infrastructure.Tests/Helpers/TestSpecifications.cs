using Enfo.Domain.Entities;
using Enfo.Repository.Querying;

namespace Enfo.Infrastructure.Tests.Helpers
{
    internal class StringStartsWithSpecification : Specification<string>
    {
        public StringStartsWithSpecification(string startsWith) =>
            ApplyCriteria(e => e.ToLower().StartsWith(startsWith.ToLower()));
    }

    internal class StringEndsWithSpecification : Specification<string>
    {
        public StringEndsWithSpecification(string endsWith) =>
            ApplyCriteria(e => e.ToLower().EndsWith(endsWith.ToLower()));
    }

    internal class EmptySpecification : Specification<string> { }

    internal class CountyNameStartsWithSpecification : Specification<County>
    {
        public CountyNameStartsWithSpecification(string startsWith) =>
            ApplyCriteria(e => e.CountyName.ToLower().StartsWith(startsWith.ToLower()));
    }

    internal class CountyNameStartsWithSpecificationCaseSensitive : Specification<County>
    {
        public CountyNameStartsWithSpecificationCaseSensitive(string startsWith) =>
            ApplyCriteria(e => e.CountyName.StartsWith(startsWith));
    }

    internal class CountyNameEndsWithSpecification : Specification<County>
    {
        public CountyNameEndsWithSpecification(string endsWith) =>
            ApplyCriteria(e => e.CountyName.ToLower().EndsWith(endsWith.ToLower()));
    }
}
