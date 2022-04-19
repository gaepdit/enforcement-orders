using Enfo.Domain.EpdContacts.Entities;
using Enfo.Domain.EpdContacts.Resources;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static EnfoTests.Helpers.DataHelper;
using static EnfoTests.Helpers.RepositoryHelper;

namespace EnfoTests.Infrastructure;

public class EpdContactRepositoryTests
{
    // GetAsync

    [Theory]
    [InlineData(2000)]
    [InlineData(2001)]
    public async Task Get_WhenItemExists_ReturnsItem(int id)
    {
        using var repository = CreateRepositoryHelper().GetEpdContactRepository();
        var item = await repository.GetAsync(id);

        var epdContact = GetEpdContacts.Single(e => e.Id == id);
        var expected = new EpdContactView(epdContact);

        item.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Get_WhenNotExists_ReturnsNull()
    {
        using var repository = CreateRepositoryHelper().GetEpdContactRepository();
        var item = await repository.GetAsync(-1);
        item.Should().BeNull();
    }

    // ListAsync

    [Fact]
    public async Task List_ByDefault_ReturnsOnlyActive()
    {
        using var repository = CreateRepositoryHelper().GetEpdContactRepository();
        var items = await repository.ListAsync();
        items.Should().HaveCount(GetEpdContacts.Count(e => e.Active));

        var epdContact = GetEpdContacts.First(e => e.Active);
        var expected = new EpdContactView(epdContact);

        items[0].Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task List_IfIncludeAll_ReturnsAll()
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
    public async Task Update_FromValidItem_Updates()
    {
        var itemId = GetEpdContacts.First(e => e.Active).Id;
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

    [Fact]
    public async Task Update_WithNoChanges_Succeeds()
    {
        var original = GetEpdContacts.First(e => e.Active);
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

    [Fact]
    public async Task Update_GivenNullName_ThrowsException()
    {
        var itemId = GetEpdContacts.First(e => e.Active).Id;
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

    [Fact]
    public async Task Update_GivenInvalidEmail_ThrowsException()
    {
        var itemId = GetEpdContacts.First(e => e.Active).Id;
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

    [Fact]
    public async Task Update_GivenInvalidTelephone_ThrowsException()
    {
        var itemId = GetEpdContacts.First(e => e.Active).Id;
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

    [Fact]
    public async Task Update_GivenNullStreet_ThrowsException()
    {
        var itemId = GetEpdContacts.First(e => e.Active).Id;
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

    [Fact]
    public async Task Update_GivenInvalidZIP_ThrowsException()
    {
        var itemId = GetEpdContacts.First(e => e.Active).Id;
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

    [Fact]
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

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateStatus_IfChangeStatus_Succeeds(bool newActiveStatus)
    {
        var itemId = GetEpdContacts.First(e => e.Active != newActiveStatus).Id;

        using var repositoryHelper = CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEpdContactRepository();

        await repository.UpdateStatusAsync(itemId, newActiveStatus);
        repositoryHelper.ClearChangeTracker();

        var item = await repository.GetAsync(itemId);
        item.Active.Should().Be(newActiveStatus);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateStatus_IfStatusUnchanged_Succeeds(bool newActiveStatus)
    {
        var itemId = GetEpdContacts.First(e => e.Active == newActiveStatus).Id;

        using var repositoryHelper = CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEpdContactRepository();

        await repository.UpdateStatusAsync(itemId, newActiveStatus);
        repositoryHelper.ClearChangeTracker();

        var item = await repository.GetAsync(itemId);
        item.Active.Should().Be(newActiveStatus);
    }

    [Fact]
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
