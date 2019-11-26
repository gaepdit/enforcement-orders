using Enfo.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace Enfo.Domain.Querying
{
    public interface ISorting<T>
        where T : BaseEntity
    {
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        Expression<Func<T, object>> GroupBy { get; }
    }
}
