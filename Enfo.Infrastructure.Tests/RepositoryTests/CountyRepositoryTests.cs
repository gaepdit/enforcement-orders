using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
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
    public class CountyRepositoryTests
    {
        private ICountyRepository GetRepository([CallerMemberName] string dbName = null)
        {
            var options = new DbContextOptionsBuilder<EnfoDbContext>()
                .UseSqlite($"Data Source={dbName}.db")
                .Options;

            var context = new EnfoDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return new CountyRepository(context);
        }

        [Fact]
        public async Task GetAllCountiesReturnsListAsync()
        {
            ICountyRepository repository = GetRepository();

            IReadOnlyList<County> items = await repository.ListAllAsync()
                .ConfigureAwait(false);

            var expected = new County() { Id = 1, CountyName = "Appling" };

            items.Should().HaveCount(159);
            items[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetCountyByIdReturnsItemAsync()
        {
            ICountyRepository repository = GetRepository();

            County item = await repository.GetByIdAsync(1)
                .ConfigureAwait(false);

            var expected = new County() { Id = 1, CountyName = "Appling" };

            item.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetCountyByMissingIdReturnsNullAsync()
        {
            ICountyRepository repository = GetRepository();

            County item = await repository.GetByIdAsync(-1)
                .ConfigureAwait(false);

            item.Should().BeNull();
        }

        [Fact]
        public async Task CountCountiesWithSpec()
        {
            ICountyRepository repository = GetRepository();

            var spec = new Specification<County>(e => e.CountyName.StartsWith("B"));

            int count = await repository.CountAsync(spec)
                .ConfigureAwait(false);

            count.Should().Be(16);
        }
    }
}
