using Enfo.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enfo.Domain.Repositories
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        void Add(T entity);
        Task<int> CountAsync(ISpecification<T> spec);
    }
}