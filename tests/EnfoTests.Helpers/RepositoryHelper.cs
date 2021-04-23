using System;
using System.Linq;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Repositories;
using Enfo.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using TestSupport.EfHelpers;

namespace EnfoTests.Helpers
{
    public sealed class RepositoryHelper : IDisposable
    {
        private readonly DbContextOptions<EnfoDbContext> _options = SqliteInMemory.CreateOptions<EnfoDbContext>();
        private readonly EnfoDbContext _context;

        private RepositoryHelper()
        {
            _context = new EnfoDbContext(_options, null);
            _context.Database.EnsureCreated();
        }

        public static RepositoryHelper CreateRepositoryHelper() => new();

        public void ClearChangeTracker() => _context.ChangeTracker.Clear();

        private void SeedAddressData()
        {
            if (_context.Addresses.Any()) return;
            _context.Addresses.AddRange(DataHelper.GetAddresses);
            _context.SaveChanges();
        }

        private void SeedLegalAuthorityData()
        {
            if (_context.LegalAuthorities.Any()) return;
            _context.LegalAuthorities.AddRange(DataHelper.GetLegalAuthorities);
            _context.SaveChanges();
        }

        private void SeedEpdContactData()
        {
            if (_context.EpdContacts.Any()) return;
            if (!_context.Addresses.Any()) _context.Addresses.AddRange(DataHelper.GetAddresses);
            _context.EpdContacts.AddRange(DataHelper.GetEpdContacts);
            _context.SaveChanges();
        }

        private void SeedEnforcementOrderData()
        {
            if (_context.EnforcementOrders.Any()) return;
            if (!_context.Addresses.Any()) _context.Addresses.AddRange(DataHelper.GetAddresses);
            if (!_context.EpdContacts.Any()) _context.EpdContacts.AddRange(DataHelper.GetEpdContacts);
            if (!_context.LegalAuthorities.Any())
                _context.LegalAuthorities.AddRange(DataHelper.GetLegalAuthorities);
            _context.SaveChanges();
            _context.EnforcementOrders.AddRange(DataHelper.GetEnforcementOrders);
            _context.SaveChanges();
        }

        public IAddressRepository GetAddressRepository()
        {
            SeedAddressData();
            return new AddressRepository(new EnfoDbContext(_options, null));
        }

        public ILegalAuthorityRepository GetLegalAuthorityRepository()
        {
            SeedLegalAuthorityData();
            return new LegalAuthorityRepository(new EnfoDbContext(_options, null));
        }

        public IEpdContactRepository GetEpdContactRepository()
        {
            SeedEpdContactData();
            return new EpdContactRepository(new EnfoDbContext(_options, null));
        }

        public IEnforcementOrderRepository GetEnforcementOrderRepository()
        {
            SeedEnforcementOrderData();
            return new EnforcementOrderRepository(new EnfoDbContext(_options, null));
        }

        public void Dispose() => _context.Dispose();
    }
}