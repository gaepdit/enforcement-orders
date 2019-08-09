using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Enfo.Domain.Specifications
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public virtual Expression<Func<T, bool>> Criteria { get; }
        public virtual Expression<Func<T, object>> OrderBy { get; }
        public virtual Expression<Func<T, object>> OrderByDescending { get; }
        public virtual Expression<Func<T, object>> GroupBy { get; }

        public bool IsSatisfiedBy(T entity)
        {
            Func<T, bool> predicate = Criteria.Compile();
            return predicate(entity);
        }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression) => Includes.Add(includeExpression);

        // string-based includes allow for including children of children
        public List<string> IncludeStrings { get; } = new List<string>();
        protected virtual void AddInclude(string includeString) => IncludeStrings.Add(includeString);
    }
}