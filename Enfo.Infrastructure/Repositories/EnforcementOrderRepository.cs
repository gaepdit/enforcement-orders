using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Contexts;

namespace Enfo.Infrastructure.Repositories
{
    public class EnforcementOrderRepository : BaseWritableRepository<EnforcementOrder>, IEnforcementOrderRepository
    {
        public EnforcementOrderRepository(EnfoDbContext context) : base(context) { }
    }
}