using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Enfo.Domain.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; } // string-based includes allow for including children of children
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        Expression<Func<T, object>> GroupBy { get; }

        bool IsSatisfiedBy(T entity);
    }
}
