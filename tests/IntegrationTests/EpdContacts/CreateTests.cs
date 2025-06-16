using Enfo.Domain.EpdContacts.Entities;
using Enfo.Domain.EpdContacts.Resources;

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

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
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
            await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
            using var repository = repositoryHelper.GetEpdContactRepository();
            await repository.CreateAsync(resource);
        };

        await action.Should().ThrowAsync<ArgumentException>();
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
            await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
            using var repository = repositoryHelper.GetEpdContactRepository();
            await repository.CreateAsync(resource);
        };

        await action.Should().ThrowAsync<ArgumentException>();
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
            await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
            using var repository = repositoryHelper.GetEpdContactRepository();
            await repository.CreateAsync(resource);
        };

        await action.Should().ThrowAsync<ArgumentException>();
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
            await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
            using var repository = repositoryHelper.GetEpdContactRepository();
            await repository.CreateAsync(resource);
        };

        await action.Should().ThrowAsync<ArgumentException>();
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
            await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
            using var repository = repositoryHelper.GetEpdContactRepository();
            await repository.CreateAsync(resource);
        };

        await action.Should().ThrowAsync<ArgumentException>();
    }
}
