using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Contexts;

namespace Enfo.Infrastructure.Repositories
{
    public class CountyRepository : BaseReadOnlyRepository<County>, ICountyRepository
    {
        public CountyRepository(EnfoDbContext context) : base(context) { }
    }
}