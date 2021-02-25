using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.Entities;
using Enfo.Repository.Mapping;
using Enfo.Repository.Resources.EpdContact;
using FluentAssertions;
using Xunit;
using static Enfo.Infrastructure.Tests.RepositoryHelper;

namespace Enfo.Infrastructure.Tests
{
    public class EpdContactRepositoryTests
    {
        private readonly List<EpdContact> _contacts;
        private readonly List<Address> _addresses;

        public EpdContactRepositoryTests()
        {
            _addresses = RepositoryHelperData.GetAddresses().ToList();
            _contacts = RepositoryHelperData.GetEpdContacts().ToList();
        }

        // GetAsync

        [Theory]
        [InlineData(2000)]
        [InlineData(2001)]
        public async Task Get_ReturnsItem(int id)
        {
            using var repository = CreateRepositoryHelper().SeedEpdContactData().GetEpdContactRepository();
            var item = await repository.GetAsync(id).ConfigureAwait(false);

            var epdContact = _contacts.Single(e => e.Id == id);
            epdContact.Address = _addresses.Single(e => e.Id == epdContact.AddressId);
            var expected = new EpdContactView(epdContact);

            item.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Get_GivenMissingId_ReturnsNull()
        {
            using var repository = CreateRepositoryHelper().SeedEpdContactData().GetEpdContactRepository();
            var item = await repository.GetAsync(-1).ConfigureAwait(false);
            item.Should().BeNull();
        }

        // ListAsync

        [Fact]
        public async Task List_ByDefault_ReturnsAllActive()
        {
            using var repository = CreateRepositoryHelper().SeedEpdContactData().GetEpdContactRepository();
            var items = await repository.ListAsync().ConfigureAwait(false);
            items.Should().HaveCount(_contacts.Count(e => e.Active));

            var epdContact = _contacts.First(e => e.Active);
            epdContact.Address = _addresses.Single(e => e.Id == epdContact.AddressId);
            var expected = new EpdContactView(epdContact);

            items[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task List_WithInactive_ReturnsAll()
        {
            using var repository = CreateRepositoryHelper().SeedEpdContactData().GetEpdContactRepository();
            var items = await repository.ListAsync(true).ConfigureAwait(false);
            items.Should().HaveCount(_contacts.Count);

            var epdContact = _contacts.First();
            epdContact.Address = _addresses.Single(e => e.Id == epdContact.AddressId);
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

            var repositoryHelper = CreateRepositoryHelper().SeedEpdContactData();

            using (var repository = repositoryHelper.GetEpdContactRepository())
            {
                itemId = await repository.CreateAsync(itemCreate);
            }

            using (var repository = repositoryHelper.GetEpdContactRepository())
            {
                var epdContact = itemCreate.ToEpdContact();
                epdContact.Address=_addresses.Single(e => e.Id == itemCreate.AddressId);
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
                using var repository = CreateRepositoryHelper().SeedEpdContactData().GetEpdContactRepository();
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
                using var repository = CreateRepositoryHelper().SeedEpdContactData().GetEpdContactRepository();
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
                using var repository = CreateRepositoryHelper().SeedEpdContactData().GetEpdContactRepository();
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
            var itemId = _contacts.First(e => e.Active).Id;
            var itemUpdate = new EpdContactUpdate()
            {
                Email = null,
                Organization = "New Org",
                Telephone = null,
                Title = "Title",
                AddressId = 2001,
                ContactName = "C. Patel",
            };

            var repositoryHelper = CreateRepositoryHelper().SeedEpdContactData();

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
            var original = _contacts.First(e => e.Active);
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

            var repositoryHelper = CreateRepositoryHelper().SeedEpdContactData();

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
            var itemId = _contacts.First(e => e.Active).Id;
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
                using var repository = CreateRepositoryHelper().SeedEpdContactData().GetEpdContactRepository();
                await repository.UpdateAsync(itemId, itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .And.ParamName.Should().Be(nameof(EpdContactCreate.ContactName));
        }

        [Fact]
        public async Task Update_GivenInvalidEmail_ThrowsException()
        {
            var itemId = _contacts.First(e => e.Active).Id;
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
                using var repository = CreateRepositoryHelper().SeedEpdContactData().GetEpdContactRepository();
                await repository.UpdateAsync(itemId, itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"Value ({itemUpdate.Email}) is not valid. (Parameter 'Email')")
                .And.ParamName.Should().Be(nameof(itemUpdate.Email));
        }

        [Fact]
        public async Task Update_GivenInvalidTelephone_ThrowsException()
        {
            var itemId = _contacts.First(e => e.Active).Id;
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
                using var repository = CreateRepositoryHelper().SeedEpdContactData().GetEpdContactRepository();
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
                using var repository = CreateRepositoryHelper().SeedEpdContactData().GetEpdContactRepository();
                await repository.UpdateAsync(itemId, itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"ID ({itemId}) not found. (Parameter 'id')")
                .And.ParamName.Should().Be("id");
        }
    }
}