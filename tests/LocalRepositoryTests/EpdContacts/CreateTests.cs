using Enfo.Domain.EpdContacts.Entities;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.LocalRepository;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.LocalRepositoryTests.EpdContacts;

[TestFixture]
public class CreateTests
{
    [Test]
    public async Task FromValidItem_AddsNew()
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

        var expectedId = EpdContactData.EpdContacts.Max(e => e.Id) + 1;
        var repository = new EpdContactRepository();

        var itemId = await repository.CreateAsync(resource);

        var epdContact = new EpdContact(resource) { Id = itemId };
        var expected = new EpdContactView(epdContact);

        var newItem = await repository.GetAsync(itemId);
        Assert.Multiple(() =>
        {
            itemId.Should().Be(expectedId);
            newItem.Should().BeEquivalentTo(expected);
        });
    }

    [Test]
    public async Task FromInvalidItem_ThrowsException()
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
            using var repository = new EpdContactRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be(nameof(EpdContactCommand.ContactName));
    }

    [Test]
    public async Task WithInvalidEmail_ThrowsException()
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
            using var repository = new EpdContactRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Value ({resource.Email}) is not valid. (Parameter '{nameof(resource.Email)}')")
            .And.ParamName.Should().Be(nameof(resource.Email));
    }

    [Test]
    public async Task WithInvalidTelephone_ThrowsException()
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
            using var repository = new EpdContactRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Value ({resource.Telephone}) is not valid. (Parameter '{nameof(resource.Telephone)}')")
            .And.ParamName.Should().Be(nameof(resource.Telephone));
    }

    [Test]
    public async Task WithInvalidStreet_ThrowsException()
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
            using var repository = new EpdContactRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Value cannot be null. (Parameter '{nameof(resource.Street)}')")
            .And.ParamName.Should().Be(nameof(resource.Street));
    }

    [Test]
    public async Task WithInvalidZIP_ThrowsException()
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
            PostalCode = "000",
        };

        var action = async () =>
        {
            using var repository = new EpdContactRepository();
            await repository.CreateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Value ({resource.PostalCode}) is not valid. (Parameter '{nameof(resource.PostalCode)}')")
            .And.ParamName.Should().Be(nameof(resource.PostalCode));
    }
}
