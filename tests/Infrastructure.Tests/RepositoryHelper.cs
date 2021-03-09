using System;
using System.Linq;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Repositories;
using Enfo.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using TestSupport.EfHelpers;

namespace Infrastructure.Tests
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

        private void SeedAddressData()
        {
            if (_context.Addresses.Any()) return;
            _context.Addresses.AddRange(RepositoryHelperData.GetAddresses);
            _context.SaveChanges();
        }

        private void SeedLegalAuthorityData()
        {
            if (_context.LegalAuthorities.Any()) return;
            _context.LegalAuthorities.AddRange(RepositoryHelperData.GetLegalAuthorities);
            _context.SaveChanges();
        }

        private void SeedEpdContactData()
        {
            if (_context.EpdContacts.Any()) return;
            if (!_context.Addresses.Any()) _context.Addresses.AddRange(RepositoryHelperData.GetAddresses);
            _context.EpdContacts.AddRange(RepositoryHelperData.GetEpdContacts);
            _context.SaveChanges();
        }

        private void SeedEnforcementOrderData()
        {
            if (_context.EnforcementOrders.Any()) return;
            if (!_context.Addresses.Any()) _context.Addresses.AddRange(RepositoryHelperData.GetAddresses);
            if (!_context.EpdContacts.Any()) _context.EpdContacts.AddRange(RepositoryHelperData.GetEpdContacts);
            if (!_context.LegalAuthorities.Any())
                _context.LegalAuthorities.AddRange(RepositoryHelperData.GetLegalAuthorities);
            _context.SaveChanges();
            _context.EnforcementOrders.AddRange(RepositoryHelperData.GetEnforcementOrders);
            _context.SaveChanges();
        }

        public IAddressRepository GetAddressRepository()
        {
            SeedAddressData();
            return new AddressRepository(new EnfoDbContext(_options));
        }

        public ILegalAuthorityRepository GetLegalAuthorityRepository()
        {
            SeedLegalAuthorityData();
            return new LegalAuthorityRepository(new EnfoDbContext(_options));
        }

        public IEpdContactRepository GetEpdContactRepository()
        {
            SeedEpdContactData();
            return new EpdContactRepository(new EnfoDbContext(_options));
        }

        public IEnforcementOrderRepository GetEnforcementOrderRepository()
        {
            SeedEnforcementOrderData();
            return new EnforcementOrderRepository(new EnfoDbContext(_options));
        }

        public void Dispose() => _context.Dispose();
    }
}