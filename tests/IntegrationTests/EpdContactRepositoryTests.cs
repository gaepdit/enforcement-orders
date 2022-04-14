using Enfo.Domain.EpdContacts.Entities;
using Enfo.Domain.EpdContacts.Resources;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static EnfoTests.Helpers.DataHelper;
using static EnfoTests.Helpers.RepositoryHelper;

namespace EnfoTests.Infrastructure
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
            var expected = new EpdContactView(epdContact);

            items[0].Should().BeEquivalentTo(expected);
        }

        // CreateAsync

        [Fact]
        public async Task Create_AddsNewItem()
        {
            var itemCreate = new EpdContactCommand()
            {
                Email = null,
                Organization = "EPD",
                Telephone = null,
                Title = "Title",
                ContactName = "C. Patel",
                City = "Abc",
                State = "GA",
                Street = "123 St",
                PostalCode = "00000"
            };

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetEpdContactRepository();

            var itemId = await repository.CreateAsync(itemCreate);
            repositoryHelper.ClearChangeTracker();

            var epdContact = new EpdContact(itemCreate) { Id = itemId };
            var expected = new EpdContactView(epdContact);

            (await repository.GetAsync(itemId)).Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Create_GivenNullName_ThrowsException()
        {
            var itemCreate = new EpdContactCommand()
            {
                Email = null,
                Organization = "EPD",
                Telephone = null,
                Title = "Title",
                ContactName = null,
                City = "Abc",
                State = "GA",
                Street = "123 St",
                PostalCode = "00000"
            };

            var action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.CreateAsync(itemCreate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .And.ParamName.Should().Be(nameof(EpdContactCommand.ContactName));
        }

        [Fact]
        public async Task Create_GivenInvalidEmail_ThrowsException()
        {
            var itemCreate = new EpdContactCommand()
            {
                Email = "abc",
                Organization = "EPD",
                Telephone = null,
                Title = "Title",
                ContactName = "C. Patel",
                City = "Abc",
                State = "GA",
                Street = "123 St",
                PostalCode = "00000"
            };

            var action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.CreateAsync(itemCreate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"Value ({itemCreate.Email}) is not valid. (Parameter '{nameof(itemCreate.Email)}')")
                .And.ParamName.Should().Be(nameof(itemCreate.Email));
        }

        [Fact]
        public async Task Create_GivenInvalidTelephone_ThrowsException()
        {
            var itemCreate = new EpdContactCommand()
            {
                Email = null,
                Organization = "EPD",
                Telephone = "abc",
                Title = "Title",
                ContactName = "C. Patel",
                City = "Abc",
                State = "GA",
                Street = "123 St",
                PostalCode = "00000"
            };

            var action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.CreateAsync(itemCreate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage(
                    $"Value ({itemCreate.Telephone}) is not valid. (Parameter '{nameof(itemCreate.Telephone)}')")
                .And.ParamName.Should().Be(nameof(itemCreate.Telephone));
        }

        [Fact]
        public async Task Create_GivenNullStreet_ThrowsException()
        {
            var itemCreate = new EpdContactCommand()
            {
                Email = null,
                Organization = "EPD",
                Telephone = null,
                Title = "Title",
                ContactName = "C. Patel",
                City = "Abc",
                State = "GA",
                Street = null,
                PostalCode = "00000"
            };

            var action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.CreateAsync(itemCreate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"Value cannot be null. (Parameter '{nameof(itemCreate.Street)}')")
                .And.ParamName.Should().Be(nameof(itemCreate.Street));
        }

        [Fact]
        public async Task Create_GivenInvalidZIP_ThrowsException()
        {
            var itemCreate = new EpdContactCommand()
            {
                Email = null,
                Organization = "EPD",
                Telephone = null,
                Title = "Title",
                ContactName = "C. Patel",
                City = "Abc",
                State = "GA",
                Street = "123 St",
                PostalCode = "123"
            };

            var action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.CreateAsync(itemCreate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage(
                    $"Value ({itemCreate.PostalCode}) is not valid. (Parameter '{nameof(itemCreate.PostalCode)}')")
                .And.ParamName.Should().Be(nameof(itemCreate.PostalCode));
        }

        // UpdateAsync

        [Fact]
        public async Task Update_SuccessfullyUpdatesItem()
        {
            var itemId = GetEpdContacts.First(e => e.Active).Id;
            var itemUpdate = new EpdContactCommand()
            {
                Id = itemId,
                Email = null,
                Organization = "New Org",
                Telephone = null,
                Title = "Title",
                ContactName = "C. Patel",
                City = "Abc",
                State = "GA",
                Street = "123 St",
                PostalCode = "00000",
            };

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetEpdContactRepository();

            await repository.UpdateAsync(itemUpdate);
            repositoryHelper.ClearChangeTracker();

            var item = await repository.GetAsync(itemId);
            item.Organization.Should().Be(itemUpdate.Organization);
            item.ContactName.Should().Be(itemUpdate.ContactName);
        }

        [Fact]
        public async Task Update_WithNoChanges_Succeeds()
        {
            var original = GetEpdContacts.First(e => e.Active);
            var itemId = original.Id;
            var itemUpdate = new EpdContactCommand()
            {
                Id = itemId,
                Email = original.Email,
                Organization = original.Organization,
                Telephone = original.Telephone,
                Title = original.Title,
                ContactName = original.ContactName,
                City = "Abc",
                State = "GA",
                Street = "123 St",
                PostalCode = "00000"
            };

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetEpdContactRepository();

            await repository.UpdateAsync(itemUpdate);
            repositoryHelper.ClearChangeTracker();

            var item = await repository.GetAsync(itemId);
            item.Organization.Should().Be(itemUpdate.Organization);
            item.ContactName.Should().Be(itemUpdate.ContactName);
        }

        [Fact]
        public async Task Update_GivenNullName_ThrowsException()
        {
            var itemId = GetEpdContacts.First(e => e.Active).Id;
            var itemUpdate = new EpdContactCommand()
            {
                Id = itemId,
                Email = null,
                Organization = "EPD",
                Telephone = null,
                Title = "Title",
                ContactName = null,
                City = "Abc",
                State = "GA",
                Street = "123 St",
                PostalCode = "00000"
            };

            var action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.UpdateAsync(itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .And.ParamName.Should().Be(nameof(itemUpdate.ContactName));
        }

        [Fact]
        public async Task Update_GivenInvalidEmail_ThrowsException()
        {
            var itemId = GetEpdContacts.First(e => e.Active).Id;
            var itemUpdate = new EpdContactCommand()
            {
                Id = itemId,
                Email = "abc",
                Organization = "EPD",
                Telephone = null,
                Title = "Title",
                ContactName = "C. Patel",
                City = "Abc",
                State = "GA",
                Street = "123 St",
                PostalCode = "00000"
            };

            var action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.UpdateAsync(itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"Value ({itemUpdate.Email}) is not valid. (Parameter '{nameof(itemUpdate.Email)}')")
                .And.ParamName.Should().Be(nameof(itemUpdate.Email));
        }

        [Fact]
        public async Task Update_GivenInvalidTelephone_ThrowsException()
        {
            var itemId = GetEpdContacts.First(e => e.Active).Id;
            var itemUpdate = new EpdContactCommand()
            {
                Id = itemId,
                Email = null,
                Organization = "EPD",
                Telephone = "abc",
                Title = "Title",
                ContactName = "C. Patel",
                City = "Abc",
                State = "GA",
                Street = "123 St",
                PostalCode = "00000"
            };

            var action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.UpdateAsync(itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage(
                    $"Value ({itemUpdate.Telephone}) is not valid. (Parameter '{nameof(itemUpdate.Telephone)}')")
                .And.ParamName.Should().Be(nameof(itemUpdate.Telephone));
        }

        [Fact]
        public async Task Update_GivenNullStreet_ThrowsException()
        {
            var itemId = GetEpdContacts.First(e => e.Active).Id;
            var itemUpdate = new EpdContactCommand()
            {
                Id = itemId,
                Email = null,
                Organization = "EPD",
                Telephone = null,
                Title = "Title",
                ContactName = "C. Patel",
                City = "Abc",
                State = "GA",
                Street = null,
                PostalCode = "00000"
            };

            var action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.UpdateAsync(itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"Value cannot be null. (Parameter '{nameof(itemUpdate.Street)}')")
                .And.ParamName.Should().Be(nameof(itemUpdate.Street));
        }

        [Fact]
        public async Task Update_GivenInvalidZIP_ThrowsException()
        {
            var itemId = GetEpdContacts.First(e => e.Active).Id;
            var itemUpdate = new EpdContactCommand()
            {
                Id = itemId,
                Email = null,
                Organization = "EPD",
                Telephone = null,
                Title = "Title",
                ContactName = "C. Patel",
                City = "Abc",
                State = "GA",
                Street = "123 St",
                PostalCode = "123"
            };

            var action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.UpdateAsync(itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage(
                    $"Value ({itemUpdate.PostalCode}) is not valid. (Parameter '{nameof(itemUpdate.PostalCode)}')")
                .And.ParamName.Should().Be(nameof(itemUpdate.PostalCode));
        }

        [Fact]
        public async Task Update_GivenMissingId_ThrowsException()
        {
            const int itemId = -1;
            var itemUpdate = new EpdContactCommand()
            {
                Id = itemId,
                Email = null,
                Organization = "New Org",
                Telephone = null,
                Title = "Title",
                ContactName = "C. Patel",
                City = "Abc",
                State = "GA",
                Street = "123 St",
                PostalCode = "00000"
            };

            var action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
                await repository.UpdateAsync(itemUpdate);
            };

            (await action.Should().ThrowAsync<ArgumentException>())
                .WithMessage($"ID ({itemId}) not found. (Parameter 'resource')")
                .And.ParamName.Should().Be("resource");
        }

        // UpdateStatusAsync

        [Fact]
        public async Task UpdateStatusToInactive_GivenActive_Succeeds()
        {
            var itemId = GetEpdContacts.First(e => e.Active).Id;

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetEpdContactRepository();

            await repository.UpdateStatusAsync(itemId, false);
            repositoryHelper.ClearChangeTracker();

            var item = await repository.GetAsync(itemId);
            item.Active.ShouldBeFalse();
        }

        [Fact]
        public async Task UpdateStatusToInactive_GivenInactive_Succeeds()
        {
            var itemId = GetEpdContacts.First(e => !e.Active).Id;

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetEpdContactRepository();

            await repository.UpdateStatusAsync(itemId, false);
            repositoryHelper.ClearChangeTracker();

            var item = await repository.GetAsync(itemId);
            item.Active.ShouldBeFalse();
        }

        [Fact]
        public async Task UpdateStatusToActive_GivenActive_Succeeds()
        {
            var itemId = GetEpdContacts.First(e => e.Active).Id;

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetEpdContactRepository();

            await repository.UpdateStatusAsync(itemId, true);
            repositoryHelper.ClearChangeTracker();

            var item = await repository.GetAsync(itemId);
            item.Active.ShouldBeTrue();
        }

        [Fact]
        public async Task UpdateStatusToActive_GivenInactive_Succeeds()
        {
            var itemId = GetEpdContacts.First(e => !e.Active).Id;

            using var repositoryHelper = CreateRepositoryHelper();
            using var repository = repositoryHelper.GetEpdContactRepository();

            await repository.UpdateStatusAsync(itemId, true);
            repositoryHelper.ClearChangeTracker();

            var item = await repository.GetAsync(itemId);
            item.Active.ShouldBeTrue();
        }

        [Fact]
        public async Task UpdateStatusToInactive_GivenInvalidId_ThrowsException()
        {
            const int itemId = -1;

            var action = async () =>
            {
                using var repository = CreateRepositoryHelper().GetEpdContactRepository();
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
