using Enfo.Domain.LegalAuthorities.Entities;
using Enfo.Domain.LegalAuthorities.Resources;
using EnfoTests.Infrastructure.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace EnfoTests.Infrastructure.LegalAuthorities;

public class CreateTests
{
    [Test]
    public async Task Create_FromValidItem_AddsNew()
    {
        var resource = new LegalAuthorityCommand { AuthorityName = "New Item" };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();

        var itemId = await repository.CreateAsync(resource);
        repositoryHelper.ClearChangeTracker();

        var item = new LegalAuthority(resource) { Id = itemId };
        var expected = new LegalAuthorityView(item);
        (await repository.GetAsync(itemId))
            .Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Create_FromInvalidItem_ThrowsException()
    {
        var resource = new LegalAuthorityCommand { AuthorityName = null };

        var action = async () =>
        {
            await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
            using var repository = repositoryHelper.GetLegalAuthorityRepository();
            await repository.CreateAsync(resource);
        };

        await action.Should().ThrowAsync<ArgumentException>();
    }
}
