using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Enfo.Repository.Querying
{
    public interface IInclusion<T>
    {
        List<Expression<Func<T, object>>> Includes { get; }
        // string-based includes allow for including children of children
        List<string> IncludeStrings { get; }
    }
}
