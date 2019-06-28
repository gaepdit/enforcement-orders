using Enfo.Domain.Entities;
using Enfo.Domain.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enfo.Domain.Repositories
{
    public interface IAsyncReadableRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> GetByIdAsync(int id, ISpecification<TEntity> specification);
        Task<IReadOnlyList<TEntity>> ListAsync();
        Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> specification);
        Task<int> CountAsync();
        Task<int> CountAsync(ISpecification<TEntity> specification);
    }
}