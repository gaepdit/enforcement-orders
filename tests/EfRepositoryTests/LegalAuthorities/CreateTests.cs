using EfRepositoryTests.Helpers;
using Enfo.Domain.LegalAuthorities;
using Enfo.Domain.LegalAuthorities.Resources;

namespace EfRepositoryTests.LegalAuthorities;

public class CreateTests
{
    [Test]
    public async Task Create_FromValidItem_AddsNew()
    {
        var resource = new LegalAuthorityCreateDto { AuthorityName = "New Item" };

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();

        var itemId = await repository.CreateAsync(resource);
        repositoryHelper.ClearChangeTracker();

        var item = new LegalAuthority { Id = itemId, AuthorityName = resource.AuthorityName };
        var expected = new LegalAuthorityViewDto(item);
        (await repository.GetAsync(itemId))
            .Should().BeEquivalentTo(expected);
    }
}
