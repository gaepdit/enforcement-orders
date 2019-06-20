using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Contexts;

namespace Enfo.Infrastructure.Repositories
{
    public class EpdContactRepository : BaseWritableRepository<EpdContact>, IEpdContactRepository
    {
        public EpdContactRepository(EnfoDbContext context) : base(context) { }
    }
}