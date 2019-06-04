using Enfo.Domain.Entities;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Enfo.Infrastructure.Tests.RepositoryTests
{
    public class LegalAuthorityRepositoryTests
    {
        private ILegalAuthorityRepository GetRepository([CallerMemberName] string dbName = null)
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

            return new LegalAuthorityRepository(context);
        }

        [Fact]
        public async Task GetAllLegalAuthoritiesReturnsListAsync()
        {
            ILegalAuthorityRepository repository = GetRepository();

            var items = await repository.ListAllAsync()
               .ConfigureAwait(true);

            var expected = new LegalAuthority() { Id = 1, Active = true, AuthorityName = "ABC", OrderNumberTemplate = "abc" };

            items.Should().HaveCount(3);
            items[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetLegalAuthorityByIdReturnsItemAsync()
        {
            ILegalAuthorityRepository repository = GetRepository();

            LegalAuthority item = await repository.GetByIdAsync(1)
                .ConfigureAwait(false);

            var expected = new LegalAuthority() { Id = 1, Active = true, AuthorityName = "ABC", OrderNumberTemplate = "abc" };

            item.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetLegalAuthorityByMissingIdReturnsNullAsync()
        {
            ILegalAuthorityRepository repository = GetRepository();

            LegalAuthority item = await repository.GetByIdAsync(-1)
                .ConfigureAwait(false);

            item.Should().BeNull();
        }
    }
}
