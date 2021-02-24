﻿using Enfo.Domain.Entities;
using Enfo.Infrastructure.SeedData;
using Enfo.Infrastructure.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.Data;
using Xunit;

namespace Enfo.Infrastructure.Tests
{
    public class WritableRepositoryTests
    {
        private readonly County[] _counties;
        private readonly EpdContact[] _epdContacts;

        public WritableRepositoryTests()
        {
            _counties = DomainData.GetCounties();
            _epdContacts = DomainData.GetEpdContacts();
        }

        [Fact]
        public async Task AddNewItemIncreasesCount()
        {
            using var repository = this.GetRepository<County>();

            County item = new County { CountyName = "NewCounty" };
            repository.Add(item);
            await repository.CompleteAsync().ConfigureAwait(false);

            int postCount = await repository.CountAsync().ConfigureAwait(false);

            postCount.Should().Be(_counties.Length + 1);
        }

        [Fact]
        public async Task AddNewItemIsAddedCorrectly()
        {
            using var repository = this.GetRepository<County>();

            County item = new County { CountyName = "NewCounty" };
            repository.Add(item);
            await repository.CompleteAsync().ConfigureAwait(false);

            var addedItem = await repository.GetByIdAsync(item.Id).ConfigureAwait(false);

            addedItem.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task AddNewItemFailsIfMissingRequiredProperty()
        {
            using var repository = this.GetRepository<County>();

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
            using var repository = this.GetRepository<EpdContact>();

            var newContact = new EpdContact
            {
                AddressId = 2002,
                ContactName = "Mr. Fake Name",
                Email = "fake.name@example.com",
                Organization = "Environmental Protection Division",
                Title = ""
            };

            repository.Add(newContact);
            await repository.CompleteAsync().ConfigureAwait(false);

            var itemList = await repository.ListAsync().ConfigureAwait(false);

            itemList.Count.Should().Be(_epdContacts.Length + 1);
            itemList.Single(e => e.Id == newContact.Id).Should().BeEquivalentTo(newContact);
        }
    }
}
