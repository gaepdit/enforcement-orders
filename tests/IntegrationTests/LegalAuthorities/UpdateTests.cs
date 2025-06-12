using Enfo.Domain.LegalAuthorities.Resources;
using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;

namespace EnfoTests.Infrastructure.LegalAuthorities;

public class UpdateTests
{
    [Test]
    public async Task Update_FromValidItem_Updates()
    {
        var itemId = LegalAuthorityData.LegalAuthorities.First(e => e.Active).Id;
        var resource = new LegalAuthorityCommand { Id = itemId, AuthorityName = "New Name" };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();

        await repository.UpdateAsync(resource);
        repositoryHelper.ClearChangeTracker();

        var item = await repository.GetAsync(itemId);
        item.Should().BeEquivalentTo(resource);
    }

    [Test]
    public async Task Update_WithNoChanges_Succeeds()
    {
        var original = LegalAuthorityData.LegalAuthorities.First(e => e.Active);
        var resource = new LegalAuthorityCommand { Id = original.Id, AuthorityName = original.AuthorityName };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();

        await repository.UpdateAsync(resource);
        repositoryHelper.ClearChangeTracker();

        var item = await repository.GetAsync(original.Id);
        item.Should().BeEquivalentTo(new LegalAuthorityView(original));
    }

    [Test]
    public async Task Update_FromInvalidItem_ThrowsException()
    {
        var itemId = LegalAuthorityData.LegalAuthorities.First(e => e.Active).Id;
        var resource = new LegalAuthorityCommand { Id = itemId, AuthorityName = null };

        var action = async () =>
        {
            await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
            using var repository = repositoryHelper.GetLegalAuthorityRepository();
            await repository.UpdateAsync(resource);
        };

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task Update_WithMissingId_ThrowsException()
    {
        const int itemId = -1;
        var itemUpdate = new LegalAuthorityCommand { Id = itemId, AuthorityName = "New Name" };

        var action = async () =>
        {
            await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
            using var repository = repositoryHelper.GetLegalAuthorityRepository();
            await repository.UpdateAsync(itemUpdate);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"ID ({itemId}) not found. (Parameter 'resource')")
            .And.ParamName.Should().Be("resource");
    }
}
