using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Enfo.Domain.Utils;

namespace Enfo.Domain.Querying
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public virtual List<Expression<Func<T, bool>>> Criteria { get; private set; }

        protected virtual void ApplyCriteria(Expression<Func<T, bool>> criterion) =>
            (Criteria ??= new List<Expression<Func<T, bool>>>())
                .Add(Check.NotNull(criterion, nameof(criterion)));

        protected virtual void ApplyCriteria(List<Expression<Func<T, bool>>> criteria) =>
            (Criteria ??= new List<Expression<Func<T, bool>>>())
                .AddRange(Check.NotNullOrEmpty(criteria, nameof(criteria)));
    }
}
