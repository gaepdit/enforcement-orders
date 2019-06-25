using Enfo.Domain.Entities;

namespace Enfo.Domain.Repositories
{
    public interface IAsyncWritableRepository<TEntity> : IAsyncReadableRepository<TEntity>, IUnitOfWork 
        where TEntity : BaseEntity
    {
        void Add(TEntity entity);
    }
}