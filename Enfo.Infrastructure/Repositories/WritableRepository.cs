using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Contexts;
using System.Threading.Tasks;

namespace Enfo.Infrastructure.Repositories
{
    public class WritableRepository<T> : ReadableRepository<T>, IAsyncWritableRepository<T>
        where T : BaseEntity
    {
        public WritableRepository(EnfoDbContext context) : base(context) { }

        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public Task CompleteAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}