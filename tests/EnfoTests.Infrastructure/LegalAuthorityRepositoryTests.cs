using System;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Repository.Mapping;
using Enfo.Repository.Resources.LegalAuthority;
using FluentAssertions;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static EnfoTests.Helpers.DataHelper;
using static EnfoTests.Helpers.RepositoryHelper;

namespace EnfoTests.Infrastructure
{
    public class LegalAuthorityRepositoryTests
    {
        // GetAsync

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Get_ReturnsItem(int id)
        {
            using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
            var item = await repository.GetAsync(id);
            item.Should().BeEquivalentTo(new LegalAuthorityView(GetLegalAuthorities.Single(e => e.Id == id)));
        }

        [Fact]
        public async Task Get_GivenMissingId_ReturnsNull()
        {
            using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
            var item = await repository.GetAsync(-1);
            item.Should().BeNull();
        }

        // ListAsync

        [Fact]
        public async Task List_ByDefault_ReturnsAllActive()
        {
            using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
            var items = await repository.ListAsync();
            items.Should().HaveCount(GetLegalAuthorities.Count(e => e.Active));
            items[0].Should().BeEquivalentTo(new LegalAuthorityView(GetLegalAuthorities.First(e => e.Active)));
        }

        [Fact]
        public async Task List_WithInactive_ReturnsAll()
        {
            using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
            var items = await repository.ListAsync(true);
            items.Should().HaveCount(GetLegalAuthorities.Count());
            items[0].Should().BeEquivalentTo(new LegalAuthorityView(GetLegalAuthorities.First()));
        }

        // CreateAsync

        [Fact]
        public async Task Create_AddsNewItem()
        {
            var itemCreate = new LegalAuthorityCreate() {AuthorityName = "New Item"};

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetLegalAuthorityRepository();

            var itemId = await repository.CreateAsync(itemCreate);
            repositoryHelper.ClearChangeTracker();

            var item = itemCreate.ToLegalAuthorityEntity();
            item.Id = itemId;
            var expected = new LegalAuthorityView(item);
            (await repository.GetAsync(itemId))
                .Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Create_GivenNullName_ThrowsException()
        {
            var itemCreate = new LegalAuthorityCreate() {AuthorityName = null};

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
                await repository.CreateAsync(itemCreate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .And.ParamName.Should().Be(nameof(LegalAuthorityCreate.AuthorityName));
        }

        // UpdateAsync

        [Fact]
        public async Task Update_SuccessfullyUpdatesItem()
        {
            var itemId = GetLegalAuthorities.First(e => e.Active).Id;
            var itemUpdate = new LegalAuthorityUpdate() {AuthorityName = "New Name"};

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetLegalAuthorityRepository();

            await repository.UpdateAsync(itemId, itemUpdate);
            repositoryHelper.ClearChangeTracker();

            var item = await repository.GetAsync(itemId);
            item.Should().BeEquivalentTo(itemUpdate);
        }

        [Fact]
        public async Task Update_WithNoChanges_Succeeds()
        {
            var original = GetLegalAuthorities.First(e => e.Active);
            var itemId = original.Id;
            var itemUpdate = new LegalAuthorityUpdate() {AuthorityName = original.AuthorityName};

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetLegalAuthorityRepository();

            await repository.UpdateAsync(itemId, itemUpdate);
            repositoryHelper.ClearChangeTracker();

            var item = await repository.GetAsync(itemId);
            item.Should().BeEquivalentTo(new LegalAuthorityView(original));
        }

        [Fact]
        public async Task Update_GivenNullStreet_ThrowsException()
        {
            var itemId = GetLegalAuthorities.First(e => e.Active).Id;
            var itemUpdate = new LegalAuthorityUpdate() {AuthorityName = null};

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
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
                using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
                await repository.UpdateAsync(itemId, itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"ID ({itemId}) not found. (Parameter 'id')")
                .And.ParamName.Should().Be("id");
        }

        // UpdateStatusAsync

        [Fact]
        public async Task UpdateStatusToInactive_GivenActive_Succeeds()
        {
            var itemId = GetLegalAuthorities.First(e => e.Active).Id;

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetLegalAuthorityRepository();

            await repository.UpdateStatusAsync(itemId, false);
            repositoryHelper.ClearChangeTracker();

            var item = await repository.GetAsync(itemId);
            item.Active.ShouldBeFalse();
        }

        [Fact]
        public async Task UpdateStatusToInactive_GivenInactive_Succeeds()
        {
            var itemId = GetLegalAuthorities.First(e => !e.Active).Id;

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetLegalAuthorityRepository();

            await repository.UpdateStatusAsync(itemId, false);
            repositoryHelper.ClearChangeTracker();

            var item = await repository.GetAsync(itemId);
            item.Active.ShouldBeFalse();
        }

        [Fact]
        public async Task UpdateStatusToActive_GivenActive_Succeeds()
        {
            var itemId = GetLegalAuthorities.First(e => e.Active).Id;

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetLegalAuthorityRepository();

            await repository.UpdateStatusAsync(itemId, true);
            repositoryHelper.ClearChangeTracker();

            var item = await repository.GetAsync(itemId);
            item.Active.ShouldBeTrue();
        }

        [Fact]
        public async Task UpdateStatusToActive_GivenInactive_Succeeds()
        {
            var itemId = GetLegalAuthorities.First(e => !e.Active).Id;

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetLegalAuthorityRepository();

            await repository.UpdateStatusAsync(itemId, true);
            repositoryHelper.ClearChangeTracker();

            var item = await repository.GetAsync(itemId);
            item.Active.ShouldBeTrue();
        }

        [Fact]
        public async Task UpdateStatusToInactive_GivenInvalidId_ThrowsException()
        {
            const int itemId = -1;

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
                await repository.UpdateStatusAsync(itemId, true);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"ID ({itemId}) not found. (Parameter 'id')")
                .And.ParamName.Should().Be("id");
        }

        // ExistsAsync

        [Fact]
        public async Task Exists_GivenExists_ReturnsTrue()
        {
            using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
            var result = await repository.ExistsAsync(GetLegalAuthorities.First().Id);
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Exists_GivenNotExists_ReturnsFalse()
        {
            using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
            var result = await repository.ExistsAsync(-1);
            result.ShouldBeFalse();
        }

        // NameExistsAsync

        [Fact]
        public async Task NameExists_GivenExistingName_ReturnsTrue()
        {
            var item = GetLegalAuthorities.First();
            using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
            (await repository.NameExistsAsync(item.AuthorityName)).ShouldBeTrue();
        }

        [Fact]
        public async Task NameExists_GivenNonexistentName_ReturnsFalse()
        {
            using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
            (await repository.NameExistsAsync(Guid.NewGuid().ToString())).ShouldBeFalse();
        }

        [Fact]
        public async Task NameExists_GivenExistingNameAndMatchingId_ReturnsFalse()
        {
            var item = GetLegalAuthorities.First();
            using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
            (await repository.NameExistsAsync(item.AuthorityName, item.Id)).ShouldBeFalse();
        }

        [Fact]
        public async Task NameExists_GivenExistingNameAndNonMatchingId_ReturnsTrue()
        {
            var item = GetLegalAuthorities.First();
            using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
            (await repository.NameExistsAsync(item.AuthorityName, -1)).ShouldBeTrue();
        }
    }
}