using Enfo.Domain.Querying;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Enfo.Domain.Tests.Helpers
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

    internal class NullCriterionSpecification : Specification<int>
    {
        public NullCriterionSpecification()
        {
            Expression<Func<int, bool>> criterion = null;
            ApplyCriteria(criterion);
        }
    }

    internal class NullCriteriaSpecification : Specification<int>
    {
        public NullCriteriaSpecification()
        {
            List<Expression<Func<int, bool>>> criteria = null;
            ApplyCriteria(criteria);
        }
    }

    internal class EmptyCriteriaSpecification : Specification<int>
    {
        public EmptyCriteriaSpecification()
        {
            List<Expression<Func<int, bool>>> criteria = new List<Expression<Func<int, bool>>>();
            ApplyCriteria(criteria);
        }
    }
}
