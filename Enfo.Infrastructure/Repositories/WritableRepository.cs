using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Contexts;
using System.Threading.Tasks;

namespace Enfo.Infrastructure.Repositories
{
    public class WritableRepository<TEntity> : ReadableRepository<TEntity>, IAsyncWritableRepository<TEntity>
        where TEntity : BaseEntity
    {
        public WritableRepository(EnfoDbContext context) : base(context) { }

        public void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }

        public Task<int> CompleteAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}