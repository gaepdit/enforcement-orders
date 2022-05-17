using Enfo.Domain.EpdContacts.Entities;
using Enfo.Domain.EpdContacts.Resources;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestData;
using static EnfoTests.Infrastructure.Helpers.RepositoryHelper;

namespace EnfoTests.Infrastructure;

[TestFixture]
public class EpdContactRepositoryTests
{
    // GetAsync

    [Test]
    [TestCase(2000)]
    [TestCase(2001)]
    public async Task Get_WhenItemExists_ReturnsItem(int id)
    {
        using var repository = CreateRepositoryHelper().GetEpdContactRepository();
        var item = await repository.GetAsync(id);

        var epdContact = EpdContactData.EpdContacts.Single(e => e.Id == id);
        var expected = new EpdContactView(epdContact);

        item.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Get_WhenNotExists_ReturnsNull()
    {
        using var repository = CreateRepositoryHelper().GetEpdContactRepository();
        var item = await repository.GetAsync(-1);
        item.Should().BeNull();
    }

    // ListAsync

    [Test]
    public async Task List_ByDefault_ReturnsOnlyActive()
    {
        using var repository = CreateRepositoryHelper().GetEpdContactRepository();
        var items = await repository.ListAsync();
        items.Should().HaveCount(EpdContactData.EpdContacts.Count(e => e.Active));

        var epdContact = EpdContactData.EpdContacts.First(e => e.Active);
        var expected = new EpdContactView(epdContact);

        items[0].Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task List_IfIncludeAll_ReturnsAll()
    {
        using var repository = CreateRepositoryHelper().GetEpdContactRepository();
        var items = await repository.ListAsync(true);
        items.Should().HaveCount(EpdContactData.EpdContacts.Count);

        var epdContact = EpdContactData.EpdContacts.First();
        var expected = new EpdContactView(epdContact);

        items[0].Should().BeEquivalentTo(expected);
    }

    // CreateAsync

    [Test]
    public async Task Create_FromValidItem_AddsNewItem()
    {
        var resource = new EpdContactCommand
        {
            Email = null,
            Organization = "EPD",
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

        var itemId = await repository.CreateAsync(resource);
        repositoryHelper.ClearChangeTracker();

        var epdContact = new EpdContact(resource) { Id = itemId };
        var expected = new EpdContactView(epdContact);

        (await repository.GetAsync(itemId)).Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Create_FromInvalidItem_ThrowsException()
    {
        var resource = new EpdContactCommand
        {
            Email = null,
            Organization = "EPD",
            Telephone = null,
            Title = "Title",
            ContactName = null,
            City = "Abc",
            State = "GA",
            Street = "123 St",
            PostalCode = "00000",
        };

        var action = async () =>
        {
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be(nameof(EpdContactCommand.ContactName));
    }

    [Test]
    public async Task Create_GivenInvalidEmail_ThrowsException()
    {
        var resource = new EpdContactCommand
        {
            Email = "abc",
            Organization = "EPD",
            Telephone = null,
            Title = "Title",
            ContactName = "C. Patel",
            City = "Abc",
            State = "GA",
            Street = "123 St",
            PostalCode = "00000",
        };

        var action = async () =>
        {
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Value ({resource.Email}) is not valid. (Parameter '{nameof(resource.Email)}')")
            .And.ParamName.Should().Be(nameof(resource.Email));
    }

    [Test]
    public async Task Create_GivenInvalidTelephone_ThrowsException()
    {
        var resource = new EpdContactCommand
        {
            Email = null,
            Organization = "EPD",
            Telephone = "abc",
            Title = "Title",
            ContactName = "C. Patel",
            City = "Abc",
            State = "GA",
            Street = "123 St",
            PostalCode = "00000",
        };

        var action = async () =>
        {
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Value ({resource.Telephone}) is not valid. (Parameter '{nameof(resource.Telephone)}')")
            .And.ParamName.Should().Be(nameof(resource.Telephone));
    }

    [Test]
    public async Task Create_GivenNullStreet_ThrowsException()
    {
        var resource = new EpdContactCommand
        {
            Email = null,
            Organization = "EPD",
            Telephone = null,
            Title = "Title",
            ContactName = "C. Patel",
            City = "Abc",
            State = "GA",
            Street = null,
            PostalCode = "00000",
        };

        var action = async () =>
        {
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Value cannot be null. (Parameter '{nameof(resource.Street)}')")
            .And.ParamName.Should().Be(nameof(resource.Street));
    }

    [Test]
    public async Task Create_GivenInvalidZIP_ThrowsException()
    {
        var resource = new EpdContactCommand
        {
            Email = null,
            Organization = "EPD",
            Telephone = null,
            Title = "Title",
            ContactName = "C. Patel",
            City = "Abc",
            State = "GA",
            Street = "123 St",
            PostalCode = "123",
        };

        var action = async () =>
        {
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Value ({resource.PostalCode}) is not valid. (Parameter '{nameof(resource.PostalCode)}')")
            .And.ParamName.Should().Be(nameof(resource.PostalCode));
    }

    // UpdateAsync

    [Test]
    public async Task Update_FromValidItem_Updates()
    {
        var itemId = EpdContactData.EpdContacts.First(e => e.Active).Id;
        var resource = new EpdContactCommand
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

        await repository.UpdateAsync(resource);
        repositoryHelper.ClearChangeTracker();

        var item = await repository.GetAsync(itemId);
        item.Organization.Should().Be(resource.Organization);
        item.ContactName.Should().Be(resource.ContactName);
    }

    [Test]
    public async Task Update_WithNoChanges_Succeeds()
    {
        var original = EpdContactData.EpdContacts.First(e => e.Active);
        var resource = new EpdContactCommand
        {
            Id = original.Id,
            Email = original.Email,
            Organization = original.Organization,
            Telephone = original.Telephone,
            Title = original.Title,
            ContactName = original.ContactName,
            City = "Abc",
            State = "GA",
            Street = "123 St",
            PostalCode = "00000",
        };

        using var repositoryHelper = CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEpdContactRepository();

        await repository.UpdateAsync(resource);
        repositoryHelper.ClearChangeTracker();

        var item = await repository.GetAsync(original.Id);
        item.Organization.Should().Be(resource.Organization);
        item.ContactName.Should().Be(resource.ContactName);
    }

    [Test]
    public async Task Update_GivenNullName_ThrowsException()
    {
        var itemId = EpdContactData.EpdContacts.First(e => e.Active).Id;
        var resource = new EpdContactCommand
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
            PostalCode = "00000",
        };

        var action = async () =>
        {
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            await repository.UpdateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be(nameof(resource.ContactName));
    }

    [Test]
    public async Task Update_GivenInvalidEmail_ThrowsException()
    {
        var itemId = EpdContactData.EpdContacts.First(e => e.Active).Id;
        var resource = new EpdContactCommand
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
            PostalCode = "00000",
        };

        var action = async () =>
        {
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            await repository.UpdateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Value ({resource.Email}) is not valid. (Parameter '{nameof(resource.Email)}')")
            .And.ParamName.Should().Be(nameof(resource.Email));
    }

    [Test]
    public async Task Update_GivenInvalidTelephone_ThrowsException()
    {
        var itemId = EpdContactData.EpdContacts.First(e => e.Active).Id;
        var resource = new EpdContactCommand
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
            PostalCode = "00000",
        };

        var action = async () =>
        {
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            await repository.UpdateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage(
                $"Value ({resource.Telephone}) is not valid. (Parameter '{nameof(resource.Telephone)}')")
            .And.ParamName.Should().Be(nameof(resource.Telephone));
    }

    [Test]
    public async Task Update_GivenNullStreet_ThrowsException()
    {
        var itemId = EpdContactData.EpdContacts.First(e => e.Active).Id;
        var resource = new EpdContactCommand
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
            PostalCode = "00000",
        };

        var action = async () =>
        {
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            await repository.UpdateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Value cannot be null. (Parameter '{nameof(resource.Street)}')")
            .And.ParamName.Should().Be(nameof(resource.Street));
    }

    [Test]
    public async Task Update_GivenInvalidZIP_ThrowsException()
    {
        var itemId = EpdContactData.EpdContacts.First(e => e.Active).Id;
        var resource = new EpdContactCommand
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
            PostalCode = "123",
        };

        var action = async () =>
        {
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            await repository.UpdateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage(
                $"Value ({resource.PostalCode}) is not valid. (Parameter '{nameof(resource.PostalCode)}')")
            .And.ParamName.Should().Be(nameof(resource.PostalCode));
    }

    [Test]
    public async Task Update_GivenMissingId_ThrowsException()
    {
        const int itemId = -1;
        var resource = new EpdContactCommand
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

        var action = async () =>
        {
            using var repository = CreateRepositoryHelper().GetEpdContactRepository();
            await repository.UpdateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"ID ({itemId}) not found. (Parameter 'resource')")
            .And.ParamName.Should().Be("resource");
    }

    // UpdateStatusAsync

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public async Task UpdateStatus_IfChangeStatus_Succeeds(bool newActiveStatus)
    {
        var itemId = EpdContactData.EpdContacts.First(e => e.Active != newActiveStatus).Id;

        using var repositoryHelper = CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEpdContactRepository();

        await repository.UpdateStatusAsync(itemId, newActiveStatus);
        repositoryHelper.ClearChangeTracker();

        var item = await repository.GetAsync(itemId);
        item.Active.Should().Be(newActiveStatus);
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public async Task UpdateStatus_IfStatusUnchanged_Succeeds(bool newActiveStatus)
    {
        var itemId = EpdContactData.EpdContacts.First(e => e.Active == newActiveStatus).Id;

        using var repositoryHelper = CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEpdContactRepository();

        await repository.UpdateStatusAsync(itemId, newActiveStatus);
        repositoryHelper.ClearChangeTracker();

        var item = await repository.GetAsync(itemId);
        item.Active.Should().Be(newActiveStatus);
    }

    [Test]
    public async Task UpdateStatus_FromMissingId_ThrowsException()
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

    [Test]
    public async Task Exists_GivenExists_ReturnsTrue()
    {
        using var repository = CreateRepositoryHelper().GetEpdContactRepository();
        var result = await repository.ExistsAsync(EpdContactData.EpdContacts.First().Id);
        result.Should().BeTrue();
    }

    [Test]
    public async Task Exists_GivenNotExists_ReturnsFalse()
    {
        using var repository = CreateRepositoryHelper().GetEpdContactRepository();
        var result = await repository.ExistsAsync(-1);
        result.Should().BeFalse();
    }
}
