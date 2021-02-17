using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Enfo.Repository.Querying
{
    public abstract class Inclusion<T> : IInclusion<T>
    {
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        // string-based includes allow for including children of children
        public List<string> IncludeStrings { get; } = new List<string>();

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression) =>
            Includes.Add(includeExpression);

        protected virtual void AddInclude(string includeString) =>
            IncludeStrings.Add(includeString);
    }
}
