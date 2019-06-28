using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Enfo.Domain.Specifications
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public Specification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }

        public bool IsSatisfiedBy(T entity)
        {
            Func<T, bool> predicate = Criteria.Compile();
            return predicate(entity);
        }

        public List<Expression<Func<T, object>>> Includes { get; } =
            new List<Expression<Func<T, object>>>();

        public List<string> IncludeStrings { get; } =
            new List<string>();

        public Expression<Func<T, object>> OrderBy { get; }
        public Expression<Func<T, object>> OrderByDescending { get; }
        public Expression<Func<T, object>> GroupBy { get; }

        public int Take { get; }
        public int Skip { get; }
        public bool IsPagingEnabled { get; }


        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        // string-based includes allow for including children of children
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
    }
}