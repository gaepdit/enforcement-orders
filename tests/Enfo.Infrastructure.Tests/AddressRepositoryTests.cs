using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.Entities;
using Enfo.Repository.Mapping;
using Enfo.Repository.Resources.Address;
using FluentAssertions;
using Xunit;

namespace Enfo.Infrastructure.Tests
{
    public class AddressRepositoryTests
    {
        private readonly List<Address> _addresses;
        public AddressRepositoryTests() => _addresses = RepositoryHelperData.GetAddresses().ToList();

        // GetAsync

        [Theory]
        [InlineData(2000)]
        [InlineData(2001)]
        public async Task Get_ReturnsItem(int id)
        {
            using var repository = new RepositoryHelper().GetAddressRepository();
            var item = await repository.GetAsync(id).ConfigureAwait(false);
            item.Should().BeEquivalentTo(new AddressView(_addresses.Single(e => e.Id == id)));
        }

        [Fact]
        public async Task Get_GivenMissingId_ReturnsNull()
        {
            using var repository = new RepositoryHelper().GetAddressRepository();
            var item = await repository.GetAsync(-1).ConfigureAwait(false);
            item.Should().BeNull();
        }

        // ListAsync

        [Fact]
        public async Task List_ByDefault_ReturnsAllActive()
        {
            using var repository = new RepositoryHelper().GetAddressRepository();
            var items = await repository.ListAsync().ConfigureAwait(false);
            items.Should().HaveCount(_addresses.Count(e => e.Active));
            items[0].Should().BeEquivalentTo(new AddressView(_addresses.First(e => e.Active)));
        }

        [Fact]
        public async Task List_WithInactive_ReturnsAll()
        {
            using var repository = new RepositoryHelper().GetAddressRepository();
            var items = await repository.ListAsync(true).ConfigureAwait(false);
            items.Should().HaveCount(_addresses.Count);
            items[0].Should().BeEquivalentTo(new AddressView(_addresses.First()));
        }

        // CreateAsync

        [Fact]
        public async Task Create_AddsNewItem()
        {
            int itemId;
            var itemCreate = new AddressCreate()
            {
                City = "Atlanta",
                PostalCode = "30354",
                State = "GA",
                Street = "300 Main St.",
            };

            var repositoryHelper = new RepositoryHelper();

            using (var repository = repositoryHelper.GetAddressRepository())
            {
                itemId = await repository.CreateAsync(itemCreate);
            }

            using (var repository = repositoryHelper.GetAddressRepository())
            {
                var expected = new AddressView(itemCreate.ToAddress()) {Id = itemId};
                (await repository.GetAsync(itemId)).Should().BeEquivalentTo(expected);
            }
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
                using var repository = new RepositoryHelper().GetAddressRepository();
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
                using var repository = new RepositoryHelper().GetAddressRepository();
                await repository.CreateAsync(itemCreate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"Postal Code ({itemCreate.PostalCode}) is not valid. (Parameter 'resource')")
                .And.ParamName.Should().Be("resource");
        }

        // UpdateAsync

        [Fact]
        public async Task Update_SuccessfullyUpdatesItem()
        {
            var itemId = _addresses.First(e => e.Active).Id;
            var itemUpdate = new AddressUpdate()
            {
                City = "NewCity",
                PostalCode = "30354",
                State = "GA",
                Street = "400 Main St.",
                Street2 = "Box 1",
            };

            var repositoryHelper = new RepositoryHelper();

            using (var repository = repositoryHelper.GetAddressRepository())
            {
                await repository.UpdateAsync(itemId, itemUpdate);
            }

            using (var repository = repositoryHelper.GetAddressRepository())
            {
                var item = await repository.GetAsync(itemId);
                item.Should().BeEquivalentTo(itemUpdate);
            }
        }

        [Fact]
        public async Task Update_WithNoChanges_Succeeds()
        {
            var original = _addresses.First(e => e.Active);
            var itemId = original.Id;
            var itemUpdate = new AddressUpdate()
            {
                Active = original.Active,
                City = original.City,
                PostalCode = original.PostalCode,
                State = original.State,
                Street = original.Street,
                Street2 = original.Street2,
            };

            var repositoryHelper = new RepositoryHelper();

            using (var repository = repositoryHelper.GetAddressRepository())
            {
                await repository.UpdateAsync(itemId, itemUpdate);
            }

            using (var repository = repositoryHelper.GetAddressRepository())
            {
                var item = await repository.GetAsync(itemId);
                item.Should().BeEquivalentTo(new AddressView(original));
            }
        }

        [Fact]
        public async Task Update_GivenNullStreet_ThrowsException()
        {
            var itemId = _addresses.First(e => e.Active).Id;
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
                using var repository = new RepositoryHelper().GetAddressRepository();
                await repository.UpdateAsync(itemId, itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .And.ParamName.Should().Be(nameof(AddressCreate.Street));
        }

        [Fact]
        public async Task Update_GivenInvalidZIP_ThrowsException()
        {
            var itemId = _addresses.First(e => e.Active).Id;
            var itemUpdate = new AddressUpdate()
            {
                City = "NewCity",
                PostalCode = "123",
                State = "GA",
                Street = "100 Main St.",
            };

            Func<Task> action = async () =>
            {
                using var repository = new RepositoryHelper().GetAddressRepository();
                await repository.UpdateAsync(itemId, itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"Postal Code ({itemUpdate.PostalCode}) is not valid. (Parameter 'resource')")
                .And.ParamName.Should().Be("resource");
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

            var itemId = -1;

            Func<Task> action = async () =>
            {
                using var repository = new RepositoryHelper().GetAddressRepository();
                await repository.UpdateAsync(itemId, itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"ID ({itemId}) not found. (Parameter 'id')")
                .And.ParamName.Should().Be("id");
        }
    }
}