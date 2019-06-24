using Enfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Enfo.Infrastructure.Repositories
{
    public class EntityWithIncludeExpressionSpecification<T> : Specification<T>
        where T : BaseEntity
    {
        public EntityWithIncludeExpressionSpecification(int id, Expression<Func<T, object>> includeExpression)
            : base(e => e.Id == id)
        {
            AddInclude(includeExpression);
        }

        public EntityWithIncludeExpressionSpecification(int id, List<string> includeStrings)
            : base(e => e.Id == id)
        {
            foreach (var includeString in includeStrings)
            {
                AddInclude(includeString);
            }
        }
    }
}
