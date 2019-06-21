using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;
using static Enfo.Infrastructure.Tests.RepositoryTests.FakeRepository;

namespace Enfo.Infrastructure.Tests.RepositoryTests
{
    public class ReadableRepositoryTests
    {
        private IAsyncReadableRepository<Entity> GetRepository([CallerMemberName] string dbName = null)
        {
            var options = new DbContextOptionsBuilder<EnfoDbContext>()
                .UseSqlite($"Data Source={dbName}.db")
                .Options;

            var context = new EntityDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return new ReadableRepository(context);
        }

        [Fact]
        public async Task GetAllReturnsListAsync()
        {
            IAsyncReadableRepository<Entity> repository = GetRepository();
            IReadOnlyList<Entity> items = await repository.ListAllAsync().ConfigureAwait(false);

            var expected = new Entity() { Id = 1, Name = "Apple", Active = true };

            items.Should().HaveCount(2);
            items[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdReturnsItemAsync()
        {
            IAsyncReadableRepository<Entity> repository = GetRepository();

            Entity item = await repository.GetByIdAsync(2).ConfigureAwait(false);

            var expected = new Entity() { Id = 2, Name = "Banana", Active = false };

            item.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByMissingIdReturnsNullAsync()
        {
            IAsyncReadableRepository<Entity> repository = GetRepository();
            Entity item = await repository.GetByIdAsync(-1).ConfigureAwait(false);

            item.Should().BeNull();
        }

        [Fact]
        public async Task CountWithSpec()
        {
            IAsyncReadableRepository<Entity> repository = GetRepository();
            var spec = new Specification<Entity>(e => e.Name.StartsWith("B", StringComparison.CurrentCultureIgnoreCase));
            int count = await repository.CountAsync(spec).ConfigureAwait(false);

            count.Should().Be(1);
        }
    }
}
