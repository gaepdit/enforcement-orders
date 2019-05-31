using Enfo.DataAccess.Contexts;
using Enfo.DataAccess.Services;
using Enfo.Models.Models;
using Enfo.Models.Resources;
using Enfo.Models.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Enfo.DataAccess.Tests.ServiceTests
{
    public class LegalAuthorityServiceTests
    {
        private ILegalAuthorityService GetService([CallerMemberName] string dbName = null)
        {
            var options = new DbContextOptionsBuilder<EnfoDbContext>()
                .UseSqlite($"Data Source={dbName}.db")
                .Options;

            var context = new EnfoDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.LegalAuthorities.AddRange(new List<LegalAuthority>()
            {
                new LegalAuthority { Id = 1, Active = true, AuthorityName = "ABC", OrderNumberTemplate = "abc" },
                new LegalAuthority { Id = 2, Active = false, AuthorityName = "DEF", OrderNumberTemplate = "def" },
                new LegalAuthority { Id = 3, Active = true, AuthorityName = "GHI", OrderNumberTemplate = "ghi" }
            });

            context.SaveChanges();

            return new LegalAuthorityService(context);
        }

        [Fact]
        public async Task GetAllLegalAuthoritiesReturnsListAsync()
        {
            ILegalAuthorityService service = GetService();

            IEnumerable<LegalAuthorityResource> items = await service.GetAllAsync()
                .ConfigureAwait(true);
            
            var expected = new LegalAuthorityResource() { Id = 1, Active = true, AuthorityName = "ABC", OrderNumberTemplate = "abc" };

            items.Should().HaveCount(3);
            items.First().Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetLegalAuthorityByIdReturnsItemAsync()
        {
            ILegalAuthorityService service = GetService();

            LegalAuthorityResource item = await service.GetByIdAsync(1)
                .ConfigureAwait(false);

            var expected = new LegalAuthorityResource() { Id = 1, Active = true, AuthorityName = "ABC", OrderNumberTemplate = "abc" };

            item.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetLegalAuthorityByMissingIdReturnsNullAsync()
        {
            ILegalAuthorityService service = GetService();

            LegalAuthorityResource item = await service.GetByIdAsync(-1)
                .ConfigureAwait(false);

            item.Should().BeNull();
        }
    }
}
