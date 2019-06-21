using Enfo.Domain.Entities;

namespace Enfo.Domain.Repositories
{
    public interface IAsyncWritableRepository<T> : IAsyncReadableRepository<T>, IUnitOfWork 
        where T : BaseEntity
    {
        void Add(T entity);
    }
}