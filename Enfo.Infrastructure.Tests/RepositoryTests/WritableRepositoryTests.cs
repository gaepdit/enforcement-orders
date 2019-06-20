using Enfo.Domain.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;
using static Enfo.Infrastructure.Tests.RepositoryTests.FakeRepository;

namespace Enfo.Infrastructure.Tests.RepositoryTests
{
    public class WritableRepositoryTests
    {
        private IAsyncRepository<Entity> GetRepository([CallerMemberName] string dbName = null)
        {
            var options = new DbContextOptionsBuilder<EntityDbContext>()
                .UseSqlite($"Data Source={dbName}.db")
                .Options;

            var context = new EntityDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return new WritableRepository(context);
        }

        [Fact]
        public async Task AddNewItemIncreasesCount()
        {
            IAsyncRepository<Entity> repository = GetRepository();
            Entity item = new Entity { Name = "Cherry" };
            int preCount = await repository.CountAllAsync().ConfigureAwait(false);
            repository.Add(item);
            await repository.CompleteAsync().ConfigureAwait(false);

            int postCount = await repository.CountAllAsync().ConfigureAwait(false);

            postCount.Should().Be(preCount + 1);
        }

        [Fact]
        public async Task AddNewItemIsAddedCorrectly()
        {
            IAsyncRepository<Entity> repository = GetRepository();
            Entity item = new Entity { Name = "Cherry" };
            repository.Add(item);
            await repository.CompleteAsync().ConfigureAwait(false);

            Entity addedItem = await repository.GetByIdAsync(3).ConfigureAwait(false);
            var expected = new Entity { Id = 3, Active = true, Name = "Cherry" };

            addedItem.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddNewItemFailsIfMissingRequiredProperty()
        {
            IAsyncRepository<Entity> repository = GetRepository();
            repository.Add(new Entity { });

            Func<Task> action = async () => { await repository.CompleteAsync().ConfigureAwait(false); };

            (await action.Should().ThrowAsync<DbUpdateException>().ConfigureAwait(false))
                .WithMessage("An error occurred while updating the entries.*")
                .WithInnerException<Microsoft.Data.Sqlite.SqliteException>()
                .WithMessage("*NOT NULL constraint failed*");
        }
    }
}
