using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Enfo.Infrastructure.Repositories
{
    public abstract class BaseWritableRepository<T> : BaseReadOnlyRepository<T>, IAsyncRepository<T>
        where T : BaseEntity
    {
        public BaseWritableRepository(DbContext context) : base(context) { }

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