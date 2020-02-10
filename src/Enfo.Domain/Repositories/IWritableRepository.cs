using Enfo.Domain.Entities;

namespace Enfo.Domain.Repositories
{
    public interface IWritableRepository<T> : IReadableRepository<T>, IUnitOfWork
        where T : BaseEntity
    {
        void Add(T entity);
    }
}
