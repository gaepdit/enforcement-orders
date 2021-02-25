using System;
using System.Linq;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Repositories;
using Enfo.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using TestSupport.EfHelpers;

namespace Enfo.Infrastructure.Tests
{
    public sealed class RepositoryHelper : IDisposable
    {
        private readonly DbContextOptions<EnfoDbContext> _options = SqliteInMemory.CreateOptions<EnfoDbContext>();
        private readonly EnfoDbContext _context;

        private RepositoryHelper()
        {
            _context = new EnfoDbContext(_options);
            _context.Database.EnsureCreated();
        }

        public static RepositoryHelper CreateRepositoryHelper() => new RepositoryHelper();

        public RepositoryHelper SeedAddressData()
        {
            if (_context.Addresses.Any()) return this;

            _context.Addresses.AddRange(RepositoryHelperData.GetAddresses());
            _context.SaveChanges();

            return this;
        }

        public RepositoryHelper SeedLegalAuthorityData()
        {
            if (_context.LegalAuthorities.Any()) return this;

            _context.LegalAuthorities.AddRange(RepositoryHelperData.GetLegalAuthorities());
            _context.SaveChanges();

            return this;
        }

        public RepositoryHelper SeedEpdContactData()
        {
            if (_context.EpdContacts.Any()) return this;

            if (!_context.Addresses.Any()) _context.Addresses.AddRange(RepositoryHelperData.GetAddresses());
            _context.EpdContacts.AddRange(RepositoryHelperData.GetEpdContacts());
            _context.SaveChanges();

            return this;
        }

        public IAddressRepository GetAddressRepository() =>
            new AddressRepository(new EnfoDbContext(_options));

        public ILegalAuthorityRepository GetLegalAuthorityRepository() =>
            new LegalAuthorityRepository(new EnfoDbContext(_options));

        public IEpdContactRepository GetEpdContactRepository() =>
            new EpdContactRepository(new EnfoDbContext(_options));

        public void Dispose() => _context.Dispose();
    }
}