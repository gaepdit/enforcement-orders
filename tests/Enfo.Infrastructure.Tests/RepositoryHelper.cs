using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Repositories;
using Enfo.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using TestSupport.EfHelpers;

namespace Enfo.Infrastructure.Tests
{
    public class RepositoryHelper
    {
        private readonly DbContextOptions<EnfoDbContext> _options = SqliteInMemory.CreateOptions<EnfoDbContext>();

        public RepositoryHelper()
        {
            using var context = new EnfoDbContext(_options);
            context.Database.EnsureCreated();
            context.Addresses.AddRange(RepositoryHelperData.GetAddresses());
            context.SaveChanges();
        }

        public IAddressRepository GetAddressRepository() => new AddressRepository(new EnfoDbContext(_options));
    }
}