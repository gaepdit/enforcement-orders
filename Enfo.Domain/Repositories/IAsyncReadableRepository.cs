using Enfo.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enfo.Domain.Repositories
{
    public interface IAsyncReadableRepository<T> 
        where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAsync();
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification);
        Task<int> CountAsync();
        Task<int> CountAsync(ISpecification<T> specification);
    }
}