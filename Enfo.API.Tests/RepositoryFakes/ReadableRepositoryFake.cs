using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enfo.API.Tests.RepositoryFakes
{
    public class ReadableRepositoryFake<T> : IAsyncReadableRepository<T>
        where T : BaseEntity
    {
        internal readonly List<T> list;

        public ReadableRepositoryFake(List<T> list)
        {
            this.list = list;
        }

        public Task<T> GetByIdAsync(int id)
        {
            T item = list.Find(e => e.Id == id);
            return Task.FromResult(item);
        }

        public Task<IReadOnlyList<T>> ListAsync()
        {
            return Task.FromResult(list as IReadOnlyList<T>);
        }

        public Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification)
        {
            return Task.FromResult(ApplySpecification(specification).ToList() as IReadOnlyList<T>);
        }

        public Task<int> CountAsync()
        {
            return Task.FromResult(list.Count());
        }

        public Task<int> CountAsync(ISpecification<T> specification)
        {
            return Task.FromResult(ApplySpecification(specification).Count());
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(list.AsQueryable(), specification);
        }
    }
}
