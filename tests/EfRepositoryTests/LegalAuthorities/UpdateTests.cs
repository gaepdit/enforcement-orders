using EfRepositoryTests.Helpers;
using Enfo.Domain.LegalAuthorities.Resources;

namespace EfRepositoryTests.LegalAuthorities;

public class UpdateTests
{
    [Test]
    public async Task Update_FromValidItem_Updates()
    {
        var itemId = LegalAuthorityData.LegalAuthorities.First(e => e.Active).Id;
        var resource = new LegalAuthorityUpdateDto { AuthorityName = "New Name" };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();

        await repository.UpdateAsync(itemId, resource);
        repositoryHelper.ClearChangeTracker();

        var item = await repository.GetAsync(itemId);
        item.Should().BeEquivalentTo(resource);
    }

    [Test]
    public async Task Update_WithNoChanges_Succeeds()
    {
        var original = LegalAuthorityData.LegalAuthorities.First(e => e.Active);
        var resource = new LegalAuthorityUpdateDto { AuthorityName = original.AuthorityName, Active = original.Active };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();

        await repository.UpdateAsync(original.Id, resource);
        repositoryHelper.ClearChangeTracker();

        var item = await repository.GetAsync(original.Id);
        item.Should().BeEquivalentTo(new LegalAuthorityViewDto(original));
    }

    [Test]
    public async Task Update_WithMissingId_ThrowsException()
    {
        const int itemId = -1;
        var itemUpdate = new LegalAuthorityUpdateDto { AuthorityName = "New Name" };

        var action = async () =>
        {
            await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
            using var repository = repositoryHelper.GetLegalAuthorityRepository();
            await repository.UpdateAsync(itemId, itemUpdate);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"ID ({itemId}) not found. (Parameter 'resource')")
            .And.ParamName.Should().Be("resource");
    }
}
