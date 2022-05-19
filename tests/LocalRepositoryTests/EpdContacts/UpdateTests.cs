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
public class UpdateTests
{
    [Test]
    public async Task FromValidItem_Updates()
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

        using var repository = new EpdContactRepository();

        await repository.UpdateAsync(resource);

        var updatedItem = await repository.GetAsync(itemId);
        updatedItem.Should().BeEquivalentTo(resource);
    }

    [Test]
    public async Task WithNoChanges_Succeeds()
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

        using var repository = new EpdContactRepository();

        await repository.UpdateAsync(resource);

        var updatedItem = await repository.GetAsync(original.Id);
        updatedItem.Should().BeEquivalentTo(original);
    }

    [Test]
    public async Task FromInvalidItem_ThrowsException()
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
            using var repository = new EpdContactRepository();
            await repository.UpdateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be(nameof(EpdContactCommand.ContactName));
    }

    [Test]
    public async Task WithMissingId_ThrowsException()
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
            using var repository = new EpdContactRepository();
            await repository.UpdateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"ID ({itemId}) not found. (Parameter 'resource')")
            .And.ParamName.Should().Be(nameof(resource));
    }
}
