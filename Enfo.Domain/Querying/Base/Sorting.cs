using Enfo.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace Enfo.Domain.Querying
{
    public abstract class Sorting<T> : ISorting<T>
        where T : BaseEntity
    {
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }
        public Expression<Func<T, object>> GroupBy { get; private set; }

        // TODO: Use an ordered list like to enable OrderBy().ThenBy().
        protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
            OrderByDescending = null;
        }

        protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
            OrderBy = null;
        }

        protected virtual void ApplyGroupBy(Expression<Func<T, object>> groupByExpression) =>
            GroupBy = groupByExpression;
    }
}