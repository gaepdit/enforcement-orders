using System;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Repository.Mapping;
using Enfo.Repository.Resources.Address;
using FluentAssertions;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static TestHelpers.DataHelper;
using static TestHelpers.RepositoryHelper;

namespace Infrastructure.Tests
{
    public class AddressRepositoryTests
    {
        // GetAsync

        [Theory]
        [InlineData(2000)]
        [InlineData(2001)]
        public async Task Get_ReturnsItem(int id)
        {
            using var repository = CreateRepositoryHelper().GetAddressRepository();
            var item = await repository.GetAsync(id);
            item.Should().BeEquivalentTo(new AddressView(GetAddresses.Single(e => e.Id == id)));
        }

        [Fact]
        public async Task Get_GivenMissingId_ReturnsNull()
        {
            using var repository = CreateRepositoryHelper().GetAddressRepository();
            var item = await repository.GetAsync(-1);
            item.Should().BeNull();
        }

        // ListAsync

        [Fact]
        public async Task List_ByDefault_ReturnsAllActive()
        {
            using var repository = CreateRepositoryHelper().GetAddressRepository();
            var items = await repository.ListAsync();
            items.Should().HaveCount(GetAddresses.Count(e => e.Active));
            items[0].Should().BeEquivalentTo(new AddressView(GetAddresses.First(e => e.Active)));
        }

        [Fact]
        public async Task List_WithInactive_ReturnsAll()
        {
            using var repository = CreateRepositoryHelper().GetAddressRepository();
            var items = await repository.ListAsync(true);
            items.Should().HaveCount(GetAddresses.Count());
            items[0].Should().BeEquivalentTo(new AddressView(GetAddresses.First()));
        }

        // CreateAsync

        [Fact]
        public async Task Create_AddsNewItem()
        {
            var itemCreate = new AddressCreate()
            {
                City = "Atlanta",
                PostalCode = "30354",
                State = "GA",
                Street = "300 Main St.",
            };

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetAddressRepository();
            
            var itemId = await repository.CreateAsync(itemCreate);
            repositoryHelper.ClearChangeTracker();

            var item = itemCreate.ToAddress();
            item.Id = itemId;
            var expected = new AddressView(item);
            (await repository.GetAsync(itemId))
                .Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Create_GivenNullStreet_ThrowsException()
        {
            var itemCreate = new AddressCreate()
            {
                City = "Atlanta",
                PostalCode = "30354",
                State = "GA",
            };

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetAddressRepository();
                await repository.CreateAsync(itemCreate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .And.ParamName.Should().Be(nameof(AddressCreate.Street));
        }

        [Fact]
        public async Task Create_GivenInvalidZIP_ThrowsException()
        {
            var itemCreate = new AddressCreate()
            {
                City = "Atlanta",
                PostalCode = "123",
                State = "GA",
                Street = "100 Main St.",
                Street2 = "Box 100",
            };

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetAddressRepository();
                await repository.CreateAsync(itemCreate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"Value ({itemCreate.PostalCode}) is not valid. (Parameter 'PostalCode')")
                .And.ParamName.Should().Be(nameof(itemCreate.PostalCode));
        }

        // UpdateAsync

        [Fact]
        public async Task Update_SuccessfullyUpdatesItem()
        {
            var itemId = GetAddresses.First(e => e.Active).Id;
            var itemUpdate = new AddressUpdate()
            {
                City = "NewCity",
                PostalCode = "30354",
                State = "GA",
                Street = "400 Main St.",
                Street2 = "Box 1",
            };

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetAddressRepository();
            
            await repository.UpdateAsync(itemId, itemUpdate);
            repositoryHelper.ClearChangeTracker();
                
            var item = await repository.GetAsync(itemId);
            item.Should().BeEquivalentTo(itemUpdate);
        }

        [Fact]
        public async Task Update_WithNoChanges_Succeeds()
        {
            var original = GetAddresses.First(e => e.Active);
            var itemId = original.Id;
            var itemUpdate = new AddressUpdate()
            {
                City = original.City,
                PostalCode = original.PostalCode,
                State = original.State,
                Street = original.Street,
                Street2 = original.Street2,
            };

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetAddressRepository();

            await repository.UpdateAsync(itemId, itemUpdate);
            repositoryHelper.ClearChangeTracker();

            var item = await repository.GetAsync(itemId);
            item.Should().BeEquivalentTo(new AddressView(original));
        }

        [Fact]
        public async Task Update_GivenNullStreet_ThrowsException()
        {
            var itemId = GetAddresses.First(e => e.Active).Id;
            var itemUpdate = new AddressUpdate()
            {
                City = "NewCity",
                PostalCode = "30354",
                State = "GA",
                Street = null,
                Street2 = "Box 1",
            };

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetAddressRepository();
                await repository.UpdateAsync(itemId, itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .And.ParamName.Should().Be(nameof(AddressCreate.Street));
        }

        [Fact]
        public async Task Update_GivenInvalidZIP_ThrowsException()
        {
            var itemId = GetAddresses.First(e => e.Active).Id;
            var itemUpdate = new AddressUpdate()
            {
                City = "NewCity",
                PostalCode = "123",
                State = "GA",
                Street = "100 Main St.",
            };

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetAddressRepository();
                await repository.UpdateAsync(itemId, itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"Value ({itemUpdate.PostalCode}) is not valid. (Parameter 'PostalCode')")
                .And.ParamName.Should().Be(nameof(itemUpdate.PostalCode));
        }

        [Fact]
        public async Task Update_GivenMissingId_ThrowsException()
        {
            var itemUpdate = new AddressUpdate()
            {
                City = "NewCity",
                PostalCode = "30354",
                State = "GA",
                Street = "400 Main St.",
                Street2 = "Box 1",
            };

            const int itemId = -1;

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetAddressRepository();
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
            var itemId = GetAddresses.First(e => e.Active).Id;

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetAddressRepository();

            await repository.UpdateStatusAsync(itemId, false);
            repositoryHelper.ClearChangeTracker();

            var item = await repository.GetAsync(itemId);
            item.Active.ShouldBeFalse();
        }

        [Fact]
        public async Task UpdateStatusToInactive_GivenInactive_Succeeds()
        {
            var itemId = GetAddresses.First(e => !e.Active).Id;

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetAddressRepository();

            await repository.UpdateStatusAsync(itemId, false);
            repositoryHelper.ClearChangeTracker();

            var item = await repository.GetAsync(itemId);
            item.Active.ShouldBeFalse();
        }

        [Fact]
        public async Task UpdateStatusToActive_GivenActive_Succeeds()
        {
            var itemId = GetAddresses.First(e => e.Active).Id;

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetAddressRepository();

            await repository.UpdateStatusAsync(itemId, true);
            repositoryHelper.ClearChangeTracker();

            var item = await repository.GetAsync(itemId);
            item.Active.ShouldBeTrue();
        }

        [Fact]
        public async Task UpdateStatusToActive_GivenInactive_Succeeds()
        {
            var itemId = GetAddresses.First(e => !e.Active).Id;

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetAddressRepository();

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
                using var repository = CreateRepositoryHelper().GetAddressRepository();
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
            using var repository = CreateRepositoryHelper().GetAddressRepository();
            var result = await repository.ExistsAsync(GetAddresses.First().Id);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Exists_GivenNotExists_ReturnsFalse()
        {
            using var repository = CreateRepositoryHelper().GetAddressRepository();
            var result = await repository.ExistsAsync(-1);
            result.Should().BeFalse();
        }
    }
}