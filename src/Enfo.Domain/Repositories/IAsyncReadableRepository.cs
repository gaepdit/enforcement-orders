using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Entities;
using Enfo.Domain.Querying;

namespace Enfo.Domain.Repositories
{
    public interface IAsyncReadableRepository<T> : IDisposable
        where T : BaseEntity
    {
        Task<T> GetByIdAsync(
            int id,
            ISpecification<T> specification = null,
            IInclusion<T> inclusion = null);

        Task<IReadOnlyList<T>> ListAsync(
            ISpecification<T> specification = null,
            IPagination pagination = null,
            ISorting<T> sorting = null,
            IInclusion<T> inclusion = null);

        Task<int> CountAsync(ISpecification<T> specification = null);

        Task<bool> IdExists(
            int id,
            ISpecification<T> specification = null);
    }
}
