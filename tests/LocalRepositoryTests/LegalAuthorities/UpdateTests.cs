using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.LocalRepository.Repositories;

namespace LocalRepositoryTests.LegalAuthorities;

[TestFixture]
public class UpdateTests
{
    [Test]
    public async Task FromValidItem_Updates()
    {
        var itemId = LegalAuthorityData.LegalAuthorities.First(e => e.Active).Id;
        var resource = new LegalAuthorityUpdateDto { AuthorityName = "New Name" };
        using var repository = new LocalLegalAuthorityRepository();

        await repository.UpdateAsync(itemId, resource);

        var updatedItem = await repository.GetAsync(itemId);
        updatedItem.Should().BeEquivalentTo(resource);
    }

    [Test]
    public async Task WithNoChanges_Succeeds()
    {
        var item = LegalAuthorityData.LegalAuthorities.First(e => e.Active);
        var resource = new LegalAuthorityUpdateDto { AuthorityName = item.AuthorityName };
        using var repository = new LocalLegalAuthorityRepository();

        await repository.UpdateAsync(item.Id, resource);

        var updatedItem = await repository.GetAsync(item.Id);
        updatedItem.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WithMissingId_ThrowsException()
    {
        const int itemId = -1;
        var resource = new LegalAuthorityUpdateDto { AuthorityName = "New Name" };

        var action = async () =>
        {
            using var repository = new LocalLegalAuthorityRepository();
            await repository.UpdateAsync(itemId, resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"ID ({itemId}) not found. (Parameter 'resource')")
            .And.ParamName.Should().Be(nameof(resource));
    }
}
