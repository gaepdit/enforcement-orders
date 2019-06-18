using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enfo.API.Tests.Repositories
{
    public class LegalAuthorityRepositoryFake : ILegalAuthorityRepository
    {
        private readonly List<LegalAuthority> list;

        public LegalAuthorityRepositoryFake(List<LegalAuthority> list)
        {
            this.list = list;
        }

        public Task<LegalAuthority> GetByIdAsync(int id)
        {
            LegalAuthority item = list.Find(e => e.Id == id);
            return Task.FromResult(item);
        }

        public Task<IReadOnlyList<LegalAuthority>> ListAllAsync()
        {
            return Task.FromResult(list as IReadOnlyList<LegalAuthority>);
        }

        public Task<IReadOnlyList<LegalAuthority>> ListAsync(ISpecification<LegalAuthority> spec)
        {
            return Task.FromResult(ApplySpecification(spec).ToList() as IReadOnlyList<LegalAuthority>);
        }

        public void Add(LegalAuthority entity)
        {
            list.Add(entity);
        }

        public Task<int> CountAllAsync()
        {
            return Task.FromResult(list.Count());
        }

        public Task<int> CountAsync(ISpecification<LegalAuthority> spec)
        {
            return Task.FromResult(ApplySpecification(spec).Count());
        }

        private IQueryable<LegalAuthority> ApplySpecification(ISpecification<LegalAuthority> spec)
        {
            return SpecificationEvaluator<LegalAuthority>.GetQuery(list.AsQueryable(), spec);
        }

        public Task CompleteAsync() {
            return Task.CompletedTask;
        }
    }
}
