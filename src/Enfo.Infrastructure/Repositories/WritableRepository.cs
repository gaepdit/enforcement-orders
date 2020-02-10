using System.Threading.Tasks;
using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Contexts;

namespace Enfo.Infrastructure.Repositories
{
    public class WritableRepository<T> : ReadableRepository<T>, IWritableRepository<T>
        where T : BaseEntity
    {
        public WritableRepository(EnfoDbContext context) : base(context) { }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
