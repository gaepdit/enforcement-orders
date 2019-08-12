using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Enfo.Domain.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        protected BaseSpecification() { }
        protected BaseSpecification(Expression<Func<T, bool>> criteria) => Criteria = criteria;

        public virtual Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>(); // string-based includes allow for including children of children
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }
        public Expression<Func<T, object>> GroupBy { get; private set; }

        public bool IsSatisfiedBy(T entity) => Criteria.Compile()(entity);

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression) => Includes.Add(includeExpression);
        protected virtual void AddInclude(string includeString) => IncludeStrings.Add(includeString);
        protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression) => OrderBy = orderByExpression;
        protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression) => OrderByDescending = orderByDescendingExpression;
        protected virtual void ApplyGroupBy(Expression<Func<T, object>> groupByExpression) => GroupBy = groupByExpression;
    }
}