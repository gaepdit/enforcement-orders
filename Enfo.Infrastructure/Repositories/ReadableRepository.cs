using Enfo.Domain.Entities;
using Enfo.Domain.Querying;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.QueryingEvaluators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enfo.Infrastructure.Repositories
{
    public class ReadableRepository<T> : IAsyncReadableRepository<T>
        where T : BaseEntity
    {
        internal readonly EnfoDbContext context;

        public ReadableRepository(EnfoDbContext context) =>
            this.context = context;

        public async Task<T> GetByIdAsync(
            int id,
            ISpecification<T> specification = null,
            IInclusion<T> inclusion = null) =>
            await context.Set<T>()
                .Apply(specification)
                .Apply(inclusion)
            .SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

        public async Task<IReadOnlyList<T>> ListAsync(
            ISpecification<T> specification = null,
            IPagination pagination = null,
            ISorting<T> sorting = null,
            IInclusion<T> inclusion = null) =>
            await context.Set<T>()
                .Apply(specification)
                .Apply(sorting)
                .Apply(pagination)
                .Apply(inclusion)
            .ToListAsync().ConfigureAwait(false);

        public async Task<int> CountAsync(
            ISpecification<T> specification) =>
            await context.Set<T>()
                .Apply(specification)
            .CountAsync().ConfigureAwait(false);

        // IDisposable Support
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
    }
}
