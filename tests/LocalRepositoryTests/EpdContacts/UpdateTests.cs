using Enfo.Domain.EpdContacts.Resources;
using Enfo.LocalRepository.Repositories;

namespace LocalRepositoryTests.EpdContacts;

[TestFixture]
public class UpdateTests
{
    [Test]
    public async Task FromValidItem_Updates()
    {
        var itemId = EpdContactData.EpdContacts.First(e => e.Active).Id;
        var resource = new EpdContactCommand
        {
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

        using var repository = new LocalEpdContactRepository();

        await repository.UpdateAsync(itemId, resource);

        var updatedItem = await repository.GetAsync(itemId);
        updatedItem.Should().BeEquivalentTo(resource);
    }

    [Test]
    public async Task WithNoChanges_Succeeds()
    {
        var original = EpdContactData.EpdContacts.First(e => e.Active);
        var resource = new EpdContactCommand
        {
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

        using var repository = new LocalEpdContactRepository();

        await repository.UpdateAsync(original.Id, resource);

        var updatedItem = await repository.GetAsync(original.Id);
        updatedItem.Should().BeEquivalentTo(original);
    }

    [Test]
    public async Task FromInvalidItem_ThrowsException()
    {
        var itemId = EpdContactData.EpdContacts.First(e => e.Active).Id;
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
            using var repository = new LocalEpdContactRepository();
            await repository.UpdateAsync(itemId, resource);
        };

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WithMissingId_ThrowsException()
    {
        const int itemId = -1;
        var resource = new EpdContactCommand
        {
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
            using var repository = new LocalEpdContactRepository();
            await repository.UpdateAsync(itemId, resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"ID ({itemId}) not found. (Parameter 'resource')")
            .And.ParamName.Should().Be(nameof(resource));
    }
}
