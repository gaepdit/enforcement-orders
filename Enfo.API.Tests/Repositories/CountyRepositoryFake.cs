using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enfo.API.Tests.Repositories
{
    public class CountyRepositoryFake : ICountyRepository
    {
        private readonly List<County> list;

        public CountyRepositoryFake(List<County> list)
        {
            this.list = list;
        }

        public Task<County> GetByIdAsync(int id)
        {
            County item = list.Find(e => e.Id == id);
            return Task.FromResult(item);
        }

        public Task<IReadOnlyList<County>> ListAllAsync()
        {
            return Task.FromResult(list as IReadOnlyList<County>);
        }

        public Task<IReadOnlyList<County>> ListAsync(ISpecification<County> spec)
        {
            return Task.FromResult(ApplySpecification(spec).ToList() as IReadOnlyList<County>);
        }

        public Task<int> CountAllAsync()
        {
            return Task.FromResult(list.Count());
        }

        public Task<int> CountAsync(ISpecification<County> spec)
        {
            return Task.FromResult(ApplySpecification(spec).Count());
        }

        private IQueryable<County> ApplySpecification(ISpecification<County> spec)
        {
            return SpecificationEvaluator<County>.GetQuery(list.AsQueryable(), spec);
        }
    }
}
