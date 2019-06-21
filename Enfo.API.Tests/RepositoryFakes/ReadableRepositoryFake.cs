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

        public Task<IReadOnlyList<T>> ListAllAsync()
        {
            return Task.FromResult(list as IReadOnlyList<T>);
        }

        public Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return Task.FromResult(ApplySpecification(spec).ToList() as IReadOnlyList<T>);
        }

        public Task<int> CountAllAsync()
        {
            return Task.FromResult(list.Count());
        }

        public Task<int> CountAsync(ISpecification<T> spec)
        {
            return Task.FromResult(ApplySpecification(spec).Count());
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(list.AsQueryable(), spec);
        }
    }
}
