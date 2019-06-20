using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Contexts;

namespace Enfo.Infrastructure.Repositories
{
    public class AddressRepository : BaseWritableRepository<Address>, IAddressRepository
    {
        public AddressRepository(EnfoDbContext context) : base(context) { }
    }
}