using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Enfo.Infrastructure.Repositories
{
    public class ReadableRepository<TEntity> : IAsyncReadableRepository<TEntity>
        where TEntity : BaseEntity
    {
        internal readonly EnfoDbContext context;

        public ReadableRepository(EnfoDbContext context)
        {
            this.context = context;
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
        }

        public async Task<TEntity> GetByIdAsync(int id, Expression<Func<TEntity, object>> includeExpression)
        {
            var spec = new EntityWithIncludeExpressionSpecification<TEntity>(id, includeExpression);

            return await ApplySpecification(spec).SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<TEntity> GetByIdAsync(int id, List<string> includeStrings)
        {
            var spec = new EntityWithIncludeExpressionSpecification<TEntity>(id, includeStrings);

            return await ApplySpecification(spec).SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<TEntity>> ListAsync()
        {
            return await context.Set<TEntity>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).ToListAsync().ConfigureAwait(false);
        }

        public Task<int> CountAsync()
        {
            return context.Set<TEntity>().CountAsync();
        }

        public async Task<int> CountAsync(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).CountAsync().ConfigureAwait(false);
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(context.Set<TEntity>().AsQueryable(), spec);
        }
    }
}