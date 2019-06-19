using Enfo.Domain.Entities;

namespace Enfo.Domain.Repositories
{
    public interface IAsyncRepository<T> : IAsyncReadOnlyRepository<T>, IUnitOfWork 
        where T : BaseEntity
    {
        void Add(T entity);
    }
}