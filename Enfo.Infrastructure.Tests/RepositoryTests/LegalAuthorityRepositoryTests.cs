using Enfo.Domain.Entities;
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

        [Fact]
        public async Task AddNewLegalAuthorityIncreasesCount()
        {
            ILegalAuthorityRepository repository = GetRepository();

            LegalAuthority item = new LegalAuthority()
            {
                AuthorityName = "New",
                OrderNumberTemplate = "abc"
            };

            int preCount = await repository.CountAllAsync().ConfigureAwait(false);

            repository.Add(item);

            await repository.CompleteAsync().ConfigureAwait(false);

            int postCount = await repository.CountAllAsync().ConfigureAwait(false);

            postCount.Should().Be(preCount + 1);
        }

        [Fact]
        public async Task AddNewLegalAuthorityIsAddedCorrectly()
        {
            ILegalAuthorityRepository repository = GetRepository();

            LegalAuthority item = new LegalAuthority()
            {
                AuthorityName = "New",
                OrderNumberTemplate = "abc"
            };

            repository.Add(item);

            await repository.CompleteAsync().ConfigureAwait(false);

            LegalAuthority addedItem = await repository.GetByIdAsync(4)
                .ConfigureAwait(false);

            var expected = new LegalAuthority() { Id = 4, Active = true, AuthorityName = "New", OrderNumberTemplate = "abc" };

            addedItem.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddNewLegalAuthorityRequiresName()
        {
            ILegalAuthorityRepository repository = GetRepository();

            repository.Add(new LegalAuthority() { });

            Func<Task> action = async () => { await repository.CompleteAsync().ConfigureAwait(false); };

            (await action.Should().ThrowAsync<DbUpdateException>().ConfigureAwait(false))
                .WithMessage("An error occurred while updating the entries.*")
                .WithInnerException<Microsoft.Data.Sqlite.SqliteException>()
                .WithMessage("*NOT NULL constraint failed*");
        }
    }
}
