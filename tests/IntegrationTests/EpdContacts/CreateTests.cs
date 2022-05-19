using Enfo.Domain.EpdContacts.Entities;
using Enfo.Domain.EpdContacts.Resources;
using EnfoTests.Infrastructure.Helpers;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.EpdContacts;

public class CreateTests
{
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

        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
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
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEpdContactRepository();
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
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEpdContactRepository();
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
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEpdContactRepository();
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
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEpdContactRepository();
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
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEpdContactRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Value ({resource.PostalCode}) is not valid. (Parameter '{nameof(resource.PostalCode)}')")
            .And.ParamName.Should().Be(nameof(resource.PostalCode));
    }
}
