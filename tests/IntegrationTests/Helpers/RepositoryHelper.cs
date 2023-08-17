using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.Services;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Repositories;
using EnfoTests.TestData;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Linq;
using TestSupport.EfHelpers;

namespace EnfoTests.Infrastructure.Helpers
{
    public sealed class RepositoryHelper : IDisposable
    {
        private readonly DbContextOptions<EnfoDbContext> _options = SqliteInMemory.CreateOptions<EnfoDbContext>();
        private readonly EnfoDbContext _context;
        public EnfoDbContext DbContext { get; set; }

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
            _context.Attachments.AddRange(AttachmentData.Attachments);
            _context.EnforcementOrders.AddRange(EnforcementOrderData.EnforcementOrders);
            _context.SaveChanges();
        }

        public ILegalAuthorityRepository GetLegalAuthorityRepository()
        {
            SeedLegalAuthorityData();
            DbContext = new EnfoDbContext(_options, null);
            return new LegalAuthorityRepository(DbContext);
        }

        public IEpdContactRepository GetEpdContactRepository()
        {
            SeedEpdContactData();
            DbContext = new EnfoDbContext(_options, null);
            return new EpdContactRepository(DbContext);
        }

        public IEnforcementOrderRepository GetEnforcementOrderRepository()
        {
            SeedEnforcementOrderData();
            DbContext = new EnfoDbContext(_options, null);
            return new EnforcementOrderRepository(
                DbContext,
                Substitute.For<IFileService>(),
                Substitute.For<IErrorLogger>()
            );
        }

        public void Dispose() => _context.Dispose();
    }
}
