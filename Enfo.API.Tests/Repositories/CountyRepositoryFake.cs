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
        private readonly List<County> counties;

        public CountyRepositoryFake(List<County> counties)
        {
            this.counties = counties;
        }

        public Task<County> GetByIdAsync(int id)
        {
            County county = counties.Find(e => e.Id == id);
            return Task.FromResult(county);
        }

        public Task<IReadOnlyList<County>> ListAllAsync()
        {
            return Task.FromResult(counties as IReadOnlyList<County>);
        }

        public Task<IReadOnlyList<County>> ListAsync(ISpecification<County> spec)
        {
            return Task.FromResult(ApplySpecification(spec).ToList() as IReadOnlyList<County>);
        }

        public void Add(County entity)
        {
            counties.Add(entity);
        }

        public Task<int> CountAllAsync()
        {
            return Task.FromResult(counties.Count());
        }

        public Task<int> CountAsync(ISpecification<County> spec)
        {
            return Task.FromResult(ApplySpecification(spec).Count());
        }

        private IQueryable<County> ApplySpecification(ISpecification<County> spec)
        {
            return SpecificationEvaluator<County>.GetQuery(counties.AsQueryable(), spec);
        }
    }
}
