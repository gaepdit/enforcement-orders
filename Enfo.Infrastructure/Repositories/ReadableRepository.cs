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

        public async Task<T> GetByIdAsync(int id, Expression<Func<T, object>> includeExpression)
        {
            var spec = new EntityWithIncludeExpressionSpecification<T>(id, includeExpression);

            return await ApplySpecification(spec).SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<T> GetByIdAsync(int id, List<string> includeStrings)
        {
            var spec = new EntityWithIncludeExpressionSpecification<T>(id, includeStrings);

            return await ApplySpecification(spec).SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<T>> ListAsync()
        {
            return await context.Set<T>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync().ConfigureAwait(false);
        }

        public Task<int> CountAsync()
        {
            return context.Set<T>().CountAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).CountAsync().ConfigureAwait(false);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec);
        }
    }
}