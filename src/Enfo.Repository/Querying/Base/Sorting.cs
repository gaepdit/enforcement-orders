using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Enfo.Repository.Querying
{
    public abstract class Sorting<T> : ISorting<T>
        where T : class
    {
        public virtual List<Ordering<T>> OrderBy { get; private set; }
            = new List<Ordering<T>>();

        protected virtual void ApplyOrderBy(
            Expression<Func<T, object>> orderByExpression) =>
            OrderBy.Add(new Ordering<T>
            {
                OrderByExpression = orderByExpression,
                SortDirection = SortDirection.Ascending
            });

        protected virtual void ApplyOrderByDescending(
            Expression<Func<T, object>> orderByExpression) =>
            OrderBy.Add(new Ordering<T>
            {
                OrderByExpression = orderByExpression,
                SortDirection = SortDirection.Descending
            });
    }
}
