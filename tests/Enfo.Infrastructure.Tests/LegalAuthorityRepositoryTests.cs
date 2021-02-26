using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.Entities;
using Enfo.Repository.Mapping;
using Enfo.Repository.Resources.LegalAuthority;
using FluentAssertions;
using Xunit;
using static Enfo.Infrastructure.Tests.RepositoryHelper;

namespace Enfo.Infrastructure.Tests
{
    public class LegalAuthorityRepositoryTests
    {
        private readonly List<LegalAuthority> _authorities;
        public LegalAuthorityRepositoryTests() => _authorities = RepositoryHelperData.GetLegalAuthorities().ToList();

        // GetAsync

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Get_ReturnsItem(int id)
        {
            using var repository = CreateRepositoryHelper().SeedLegalAuthorityData().GetLegalAuthorityRepository();
            var item = await repository.GetAsync(id).ConfigureAwait(false);
            item.Should().BeEquivalentTo(new LegalAuthorityView(_authorities.Single(e => e.Id == id)));
        }

        [Fact]
        public async Task Get_GivenMissingId_ReturnsNull()
        {
            using var repository = CreateRepositoryHelper().SeedLegalAuthorityData().GetLegalAuthorityRepository();
            var item = await repository.GetAsync(-1).ConfigureAwait(false);
            item.Should().BeNull();
        }

        // ListAsync

        [Fact]
        public async Task List_ByDefault_ReturnsAllActive()
        {
            using var repository = CreateRepositoryHelper().SeedLegalAuthorityData().GetLegalAuthorityRepository();
            var items = await repository.ListAsync().ConfigureAwait(false);
            items.Should().HaveCount(_authorities.Count(e => e.Active));
            items[0].Should().BeEquivalentTo(new LegalAuthorityView(_authorities.First(e => e.Active)));
        }

        [Fact]
        public async Task List_WithInactive_ReturnsAll()
        {
            using var repository = CreateRepositoryHelper().SeedLegalAuthorityData().GetLegalAuthorityRepository();
            var items = await repository.ListAsync(true).ConfigureAwait(false);
            items.Should().HaveCount(_authorities.Count);
            items[0].Should().BeEquivalentTo(new LegalAuthorityView(_authorities.First()));
        }

        // CreateAsync

        [Fact]
        public async Task Create_AddsNewItem()
        {
            int itemId;
            var itemCreate = new LegalAuthorityCreate() {AuthorityName = "New Item"};

            var repositoryHelper = CreateRepositoryHelper().SeedLegalAuthorityData();
            
            using (var repository = repositoryHelper.GetLegalAuthorityRepository())
            {
                itemId = await repository.CreateAsync(itemCreate);
            }

            using (var repository = repositoryHelper.GetLegalAuthorityRepository())
            {
                var expected = new LegalAuthorityView(itemCreate.ToLegalAuthority()) {Id = itemId};
                (await repository.GetAsync(itemId))
                    .Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public async Task Create_GivenNullName_ThrowsException()
        {
            var itemCreate = new LegalAuthorityCreate() {AuthorityName = null};

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().SeedLegalAuthorityData().GetLegalAuthorityRepository();
                await repository.CreateAsync(itemCreate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .And.ParamName.Should().Be(nameof(LegalAuthorityCreate.AuthorityName));
        }

        // UpdateAsync

        [Fact]
        public async Task Update_SuccessfullyUpdatesItem()
        {
            var itemId = _authorities.First(e => e.Active).Id;
            var itemUpdate = new LegalAuthorityUpdate() {AuthorityName = "New Name"};

            var repositoryHelper = CreateRepositoryHelper().SeedLegalAuthorityData();

            using (var repository = repositoryHelper.GetLegalAuthorityRepository())
            {
                await repository.UpdateAsync(itemId, itemUpdate);
            }

            using (var repository = repositoryHelper.GetLegalAuthorityRepository())
            {
                var item = await repository.GetAsync(itemId);
                item.Should().BeEquivalentTo(itemUpdate);
            }
        }

        [Fact]
        public async Task Update_WithNoChanges_Succeeds()
        {
            var original = _authorities.First(e => e.Active);
            var itemId = original.Id;
            var itemUpdate = new LegalAuthorityUpdate() {AuthorityName = original.AuthorityName};

            var repositoryHelper = CreateRepositoryHelper().SeedLegalAuthorityData();

            using (var repository = repositoryHelper.GetLegalAuthorityRepository())
            {
                await repository.UpdateAsync(itemId, itemUpdate);
            }

            using (var repository = repositoryHelper.GetLegalAuthorityRepository())
            {
                var item = await repository.GetAsync(itemId);
                item.Should().BeEquivalentTo(new LegalAuthorityView(original));
            }
        }

        [Fact]
        public async Task Update_GivenNullStreet_ThrowsException()
        {
            var itemId = _authorities.First(e => e.Active).Id;
            var itemUpdate = new LegalAuthorityUpdate() {AuthorityName = null};

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().SeedLegalAuthorityData().GetLegalAuthorityRepository();
                await repository.UpdateAsync(itemId, itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .And.ParamName.Should().Be(nameof(LegalAuthorityCreate.AuthorityName));
        }

        [Fact]
        public async Task Update_GivenMissingId_ThrowsException()
        {
            var itemUpdate = new LegalAuthorityUpdate() {AuthorityName = "New Name"};
            const int itemId = -1;

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().SeedLegalAuthorityData().GetLegalAuthorityRepository();
                await repository.UpdateAsync(itemId, itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"ID ({itemId}) not found. (Parameter 'id')")
                .And.ParamName.Should().Be("id");
        }

        // ExistsAsync

        [Fact]
        public async Task Exists_GivenExists_ReturnsTrue()
        {
            using var repository = CreateRepositoryHelper().SeedLegalAuthorityData().GetLegalAuthorityRepository();
            var result = await repository.ExistsAsync(_authorities[0].Id);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Exists_GivenNotExists_ReturnsFalse()
        {
            using var repository = CreateRepositoryHelper().SeedLegalAuthorityData().GetLegalAuthorityRepository();
            var result = await repository.ExistsAsync(-1);
            result.Should().BeFalse();
        }
    }
}