using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Domain.Specifications;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enfo.Infrastructure.Repositories
{
    public class ReadableRepository<TEntity> : IAsyncReadableRepository<TEntity>
        where TEntity : BaseEntity
    {
        internal readonly EnfoDbContext context;

        public ReadableRepository(EnfoDbContext context) =>
            this.context = context;

        public async Task<TEntity> GetByIdAsync(int id) =>
            await context.Set<TEntity>().FindAsync(id).ConfigureAwait(false);

        public async Task<TEntity> GetByIdAsync(int id, ISpecification<TEntity> specification) =>
            await context.Set<TEntity>().Apply(specification)
            .SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

        public async Task<IReadOnlyList<TEntity>> ListAsync() =>
            await context.Set<TEntity>().ToListAsync().ConfigureAwait(false);

        public async Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> specification) =>
            await context.Set<TEntity>().Apply(specification)
            .ToListAsync().ConfigureAwait(false);

        public async Task<IReadOnlyList<TEntity>> ListAsync(IPagination pagination) =>
            await context.Set<TEntity>().Apply(pagination)
            .ToListAsync().ConfigureAwait(false);

        public async Task<IReadOnlyList<TEntity>> ListAsync(
            ISpecification<TEntity> specification,
            IPagination pagination) =>
            await context.Set<TEntity>().Apply(specification).Apply(pagination)
            .ToListAsync().ConfigureAwait(false);

        public Task<int> CountAsync() =>
            context.Set<TEntity>().CountAsync();

        public async Task<int> CountAsync(ISpecification<TEntity> specification) =>
            await context.Set<TEntity>().Apply(specification)
            .CountAsync().ConfigureAwait(false);

        // IDisposable
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