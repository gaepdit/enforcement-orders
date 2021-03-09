using System;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Repository.Mapping;
using Enfo.Repository.Resources.EpdContact;
using FluentAssertions;
using Xunit;
using static Infrastructure.Tests.RepositoryHelperData;
using static Infrastructure.Tests.RepositoryHelper;

namespace Infrastructure.Tests
{
    public class EpdContactRepositoryTests
    {
        // GetAsync

        [Theory]
        [InlineData(2000)]
        [InlineData(2001)]
        public async Task Get_ReturnsItem(int id)
        {
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            var item = await repository.GetAsync(id);

            var epdContact = GetEpdContacts.Single(e => e.Id == id);
            epdContact.Address = GetAddresses.Single(e => e.Id == epdContact.AddressId);
            var expected = new EpdContactView(epdContact);

            item.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Get_GivenMissingId_ReturnsNull()
        {
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            var item = await repository.GetAsync(-1);
            item.Should().BeNull();
        }

        // ListAsync

        [Fact]
        public async Task List_ByDefault_ReturnsAllActive()
        {
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            var items = await repository.ListAsync();
            items.Should().HaveCount(GetEpdContacts.Count(e => e.Active));

            var epdContact = GetEpdContacts.First(e => e.Active);
            epdContact.Address = GetAddresses.Single(e => e.Id == epdContact.AddressId);
            var expected = new EpdContactView(epdContact);

            items[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task List_WithInactive_ReturnsAll()
        {
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            var items = await repository.ListAsync(true);
            items.Should().HaveCount(GetEpdContacts.Count());

            var epdContact = GetEpdContacts.First();
            epdContact.Address = GetAddresses.Single(e => e.Id == epdContact.AddressId);
            var expected = new EpdContactView(epdContact);

            items[0].Should().BeEquivalentTo(expected);
        }

        // CreateAsync

        [Fact]
        public async Task Create_AddsNewItem()
        {
            int itemId;
            var itemCreate = new EpdContactCreate()
            {
                Email = null,
                Organization = "EPD",
                Telephone = null,
                Title = "Title",
                AddressId = 2000,
                ContactName = "C. Patel",
            };

            using var repositoryHelper = CreateRepositoryHelper();

            using (var repository = repositoryHelper.GetEpdContactRepository())
            {
                itemId = await repository.CreateAsync(itemCreate);
            }

            using (var repository = repositoryHelper.GetEpdContactRepository())
            {
                var epdContact = itemCreate.ToEpdContact();
                epdContact.Address = GetAddresses.Single(e => e.Id == itemCreate.AddressId);
                epdContact.Id = itemId;
                var expected = new EpdContactView(epdContact);

                (await repository.GetAsync(itemId)).Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public async Task Create_GivenNullName_ThrowsException()
        {
            var itemCreate = new EpdContactCreate()
            {
                Email = null,
                Organization = "EPD",
                Telephone = null,
                Title = "Title",
                AddressId = 2000,
                ContactName = null,
            };

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.CreateAsync(itemCreate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .And.ParamName.Should().Be(nameof(EpdContactCreate.ContactName));
        }

        [Fact]
        public async Task Create_GivenInvalidEmail_ThrowsException()
        {
            var itemCreate = new EpdContactCreate()
            {
                Email = "abc",
                Organization = "EPD",
                Telephone = null,
                Title = "Title",
                AddressId = 2000,
                ContactName = "C. Patel",
            };

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.CreateAsync(itemCreate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"Value ({itemCreate.Email}) is not valid. (Parameter 'Email')")
                .And.ParamName.Should().Be(nameof(itemCreate.Email));
        }

        [Fact]
        public async Task Create_GivenInvalidTelephone_ThrowsException()
        {
            var itemCreate = new EpdContactCreate()
            {
                Email = null,
                Organization = "EPD",
                Telephone = "abc",
                Title = "Title",
                AddressId = 2000,
                ContactName = "C. Patel",
            };

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.CreateAsync(itemCreate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"Value ({itemCreate.Telephone}) is not valid. (Parameter 'Telephone')")
                .And.ParamName.Should().Be(nameof(itemCreate.Telephone));
        }

        // UpdateAsync

        [Fact]
        public async Task Update_SuccessfullyUpdatesItem()
        {
            var itemId = GetEpdContacts.First(e => e.Active).Id;
            var itemUpdate = new EpdContactUpdate()
            {
                Email = null,
                Organization = "New Org",
                Telephone = null,
                Title = "Title",
                AddressId = 2001,
                ContactName = "C. Patel",
            };

            using var repositoryHelper = CreateRepositoryHelper();

            using (var repository = repositoryHelper.GetEpdContactRepository())
            {
                await repository.UpdateAsync(itemId, itemUpdate);
            }

            using (var repository = repositoryHelper.GetEpdContactRepository())
            {
                var item = await repository.GetAsync(itemId);
                item.Organization.Should().Be(itemUpdate.Organization);
                item.ContactName.Should().Be(itemUpdate.ContactName);
            }
        }

        [Fact]
        public async Task Update_WithNoChanges_Succeeds()
        {
            var original = GetEpdContacts.First(e => e.Active);
            var itemId = original.Id;
            var itemUpdate = new EpdContactUpdate()
            {
                Active = original.Active,
                Email = original.Email,
                Organization = original.Organization,
                Telephone = original.Telephone,
                Title = original.Title,
                AddressId = original.AddressId,
                ContactName = original.ContactName,
            };

            using var repositoryHelper = CreateRepositoryHelper();

            using (var repository = repositoryHelper.GetEpdContactRepository())
            {
                await repository.UpdateAsync(itemId, itemUpdate);
            }

            using (var repository = repositoryHelper.GetEpdContactRepository())
            {
                var item = await repository.GetAsync(itemId);
                item.Organization.Should().Be(itemUpdate.Organization);
                item.ContactName.Should().Be(itemUpdate.ContactName);
            }
        }

        [Fact]
        public async Task Update_GivenNullName_ThrowsException()
        {
            var itemId = GetEpdContacts.First(e => e.Active).Id;
            var itemUpdate = new EpdContactUpdate()
            {
                Email = null,
                Organization = "EPD",
                Telephone = null,
                Title = "Title",
                AddressId = 2000,
                ContactName = null,
            };

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.UpdateAsync(itemId, itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .And.ParamName.Should().Be(nameof(itemUpdate.ContactName));
        }

        [Fact]
        public async Task Update_GivenInvalidEmail_ThrowsException()
        {
            var itemId = GetEpdContacts.First(e => e.Active).Id;
            var itemUpdate = new EpdContactUpdate()
            {
                Email = "abc",
                Organization = "EPD",
                Telephone = null,
                Title = "Title",
                AddressId = 2000,
                ContactName = "C. Patel",
            };

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.UpdateAsync(itemId, itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"Value ({itemUpdate.Email}) is not valid. (Parameter 'Email')")
                .And.ParamName.Should().Be(nameof(itemUpdate.Email));
        }

        [Fact]
        public async Task Update_GivenInvalidTelephone_ThrowsException()
        {
            var itemId = GetEpdContacts.First(e => e.Active).Id;
            var itemUpdate = new EpdContactUpdate()
            {
                Email = null,
                Organization = "EPD",
                Telephone = "abc",
                Title = "Title",
                AddressId = 2000,
                ContactName = "C. Patel",
            };

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.UpdateAsync(itemId, itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"Value ({itemUpdate.Telephone}) is not valid. (Parameter 'Telephone')")
                .And.ParamName.Should().Be(nameof(itemUpdate.Telephone));
        }

        [Fact]
        public async Task Update_GivenMissingId_ThrowsException()
        {
            var itemUpdate = new EpdContactUpdate()
            {
                Email = null,
                Organization = "New Org",
                Telephone = null,
                Title = "Title",
                AddressId = 2000,
                ContactName = "C. Patel",
            };

            const int itemId = -1;

            Func<Task> action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
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
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            var result = await repository.ExistsAsync(GetEpdContacts.First().Id);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Exists_GivenNotExists_ReturnsFalse()
        {
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            var result = await repository.ExistsAsync(-1);
            result.Should().BeFalse();
        }
    }
}