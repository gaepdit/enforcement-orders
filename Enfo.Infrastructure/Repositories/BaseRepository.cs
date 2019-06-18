using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enfo.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly DbContext context;

        public BaseRepository(DbContext context)
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

        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
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

        public Task CompleteAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}