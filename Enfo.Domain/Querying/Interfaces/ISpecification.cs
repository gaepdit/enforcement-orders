using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Enfo.Domain.Querying
{
    public interface ISpecification<T>
    {
        List<Expression<Func<T, bool>>> Criteria { get; }
    }
}
