using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Repositories;
using EnfoTests.TestData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TestSupport.EfHelpers;

namespace EnfoTests.Infrastructure.Helpers
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

        private void SeedLegalAuthorityData()
        {
            if (_context.LegalAuthorities.Any()) return;
            _context.LegalAuthorities.AddRange(LegalAuthorityData.LegalAuthorities);
            _context.SaveChanges();
        }

        private void SeedEpdContactData()
        {
            if (_context.EpdContacts.Any()) return;
            _context.EpdContacts.AddRange(EpdContactData.EpdContacts);
            _context.SaveChanges();
        }

        private void SeedEnforcementOrderData()
        {
            if (_context.EnforcementOrders.Any()) return;
            if (!_context.EpdContacts.Any()) _context.EpdContacts.AddRange(EpdContactData.EpdContacts);
            if (!_context.LegalAuthorities.Any())
                _context.LegalAuthorities.AddRange(LegalAuthorityData.LegalAuthorities);
            _context.SaveChanges();
            _context.EnforcementOrders.AddRange(EnforcementOrderData.EnforcementOrders);
            _context.SaveChanges();
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
