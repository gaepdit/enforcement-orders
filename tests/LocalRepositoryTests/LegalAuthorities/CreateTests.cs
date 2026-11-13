using Enfo.Domain.LegalAuthorities;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.LocalRepository.Repositories;

namespace LocalRepositoryTests.LegalAuthorities;

[TestFixture]
public class CreateTests
{
    [Test]
    public async Task FromValidItem_AddsNew()
    {
        var resource = new LegalAuthorityCreateDto { AuthorityName = "New Item" };
        var expectedId = LegalAuthorityData.LegalAuthorities.Max(e => e.Id) + 1;
        var repository = new LocalLegalAuthorityRepository();

        var itemId = await repository.CreateAsync(resource);

        var item = new LegalAuthority { Id = itemId, AuthorityName = resource.AuthorityName };
        var expected = new LegalAuthorityViewDto(item);
        var newItem = await repository.GetAsync(itemId);

        using (new AssertionScope())
        {
            itemId.Should().Be(expectedId);
            newItem.Should().BeEquivalentTo(expected);
        }
    }
}
