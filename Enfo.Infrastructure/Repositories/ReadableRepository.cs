using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enfo.Infrastructure.Repositories
{
    public class ReadableRepository<T> : IAsyncReadableRepository<T>
        where T : BaseEntity
    {
        internal readonly EnfoDbContext context;

        public ReadableRepository(EnfoDbContext context)
        {
            this.context = context;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await context.Set<T>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync().ConfigureAwait(false);
        }

        public Task<int> CountAllAsync()
        {
            return context.Set<T>().CountAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync().ConfigureAwait(false);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec);
        }
    }
}