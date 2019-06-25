using Enfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Enfo.Domain.Repositories
{
    public interface IAsyncReadableRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> GetByIdAsync(int id, Expression<Func<TEntity, object>> includeExpression);
        Task<TEntity> GetByIdAsync(int id, List<string> includeStrings);
        Task<IReadOnlyList<TEntity>> ListAsync();
        Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> specification);
        Task<int> CountAsync();
        Task<int> CountAsync(ISpecification<TEntity> specification);
    }
}