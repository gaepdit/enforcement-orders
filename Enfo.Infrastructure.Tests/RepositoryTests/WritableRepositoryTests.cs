using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Enfo.Infrastructure.Tests.Helpers.RepositoryHelpers;

namespace Enfo.Infrastructure.Tests.RepositoryTests
{
    public class WritableRepositoryTests
    {
        [Fact]
        public async Task AddNewItemIncreasesCount()
        {
            IAsyncWritableRepository<County> repository = this.GetRepository<County>();

            int preCount = await repository.CountAsync().ConfigureAwait(false);

            County item = new County { CountyName = "NewCounty" };
            repository.Add(item);
            await repository.CompleteAsync().ConfigureAwait(false);

            int postCount = await repository.CountAsync().ConfigureAwait(false);

            postCount.Should().Be(preCount + 1);
        }

        [Fact]
        public async Task AddNewItemIsAddedCorrectly()
        {
            IAsyncWritableRepository<County> repository = this.GetRepository<County>();

            County item = new County { CountyName = "NewCounty" };
            repository.Add(item);
            await repository.CompleteAsync().ConfigureAwait(false);

            County addedItem = await repository.GetByIdAsync(160).ConfigureAwait(false);
            var expected = new County { Id = 160, Active = true, CountyName = "NewCounty" };

            addedItem.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddNewItemFailsIfMissingRequiredProperty()
        {
            IAsyncWritableRepository<County> repository = this.GetRepository<County>();
            repository.Add(new County { });

            Func<Task> action = async () => { await repository.CompleteAsync().ConfigureAwait(false); };

            (await action.Should().ThrowAsync<DbUpdateException>().ConfigureAwait(false))
                .WithMessage("An error occurred while updating the entries.*")
                .WithInnerException<Microsoft.Data.Sqlite.SqliteException>()
                .WithMessage("*NOT NULL constraint failed*");
        }

        [Fact]
        public async Task AddNewItemWithExistingRelatedEntityIsAddedCorrectly()
        {
            var repository = this.GetRepository<EpdContact>();

            var newContact = new EpdContact { AddressId = 2002, ContactName = "Mr. Fake Name", Email = "fake.name@example.com", Organization = "Environmental Protection Division", Title = "" };
            repository.Add(newContact);
            await repository.CompleteAsync().ConfigureAwait(false);

            var itemList = await repository.ListAsync().ConfigureAwait(false);

            newContact.Id.Should().Be(2003);

            itemList.Count.Should().Be(4);
            itemList.Single(e => e.Id == 2003).Should().BeEquivalentTo(newContact);
        }

        [Fact]
        public async Task AddNewItemWithNewRelatedEntityIsAddedCorrectly()
        {
            var repository = this.GetRepository<EpdContact>();

            var newAddress = new Address { City = "Atlanta", PostalCode = "33333", State = "GA", Street = "123 Fake St" };
            var newContact = new EpdContact { Address = newAddress, ContactName = "Mr. Fake Name", Email = "fake.name@example.com", Organization = "Environmental Protection Division", Title = "" };
            repository.Add(newContact);
            await repository.CompleteAsync().ConfigureAwait(false);

            var itemList = await repository.ListAsync().ConfigureAwait(false);

            newAddress.Id.Should().Be(2003);
            newContact.Id.Should().Be(2003);

            itemList.Count.Should().Be(4);
            itemList.Single(e => e.Id == 2003).Should().BeEquivalentTo(newContact);
        }
    }
}
