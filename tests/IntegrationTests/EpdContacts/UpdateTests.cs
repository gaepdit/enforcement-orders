using Enfo.Domain.EpdContacts.Resources;
using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.EpdContacts;

public class UpdateTests
{
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

        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
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

        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
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
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEpdContactRepository();
            await repository.UpdateAsync(resource);
        };

        await action.Should().ThrowAsync<ArgumentException>();
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
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEpdContactRepository();
            await repository.UpdateAsync(resource);
        };

        await action.Should().ThrowAsync<ArgumentException>();
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
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEpdContactRepository();
            await repository.UpdateAsync(resource);
        };

        await action.Should().ThrowAsync<ArgumentException>();
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
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEpdContactRepository();
            await repository.UpdateAsync(resource);
        };

        await action.Should().ThrowAsync<ArgumentException>();
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
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEpdContactRepository();
            await repository.UpdateAsync(resource);
        };

        await action.Should().ThrowAsync<ArgumentException>();
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
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEpdContactRepository();
            await repository.UpdateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"ID ({itemId}) not found. (Parameter 'resource')")
            .And.ParamName.Should().Be("resource");
    }
}
