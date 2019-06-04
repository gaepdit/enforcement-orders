using Enfo.Domain.Entities;
using Enfo.Infrastructure.Contexts;

namespace Enfo.Infrastructure.Repositories
{
    public class CountyRepository : BaseRepository<County>, ICountyRepository
    {
        public CountyRepository(EnfoDbContext context) : base(context) { }
    }
}