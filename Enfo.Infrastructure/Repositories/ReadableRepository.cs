using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Domain.Specifications;
using Enfo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<TEntity> GetByIdAsync(int id, ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                    context.Dispose();
                }

                disposedValue = true;
            }
        }

        // override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~ReadableRepository()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}