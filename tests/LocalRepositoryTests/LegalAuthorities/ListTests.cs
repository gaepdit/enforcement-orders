using Enfo.LocalRepository.Repositories;

namespace EnfoTests.LocalRepositoryTests.LegalAuthorities;

[TestFixture]
public class ListTests
{
    [Test]
    public async Task ByDefault_ReturnsOnlyActive()
    {
        using var repository = new LocalLegalAuthorityRepository();
        var result = await repository.ListAsync();
        result.Should().BeEquivalentTo(LegalAuthorityData.LegalAuthorities.Where(e => e.Active));
    }

    [Test]
    public async Task IfIncludeAll_ReturnsAll()
    {
        using var repository = new LocalLegalAuthorityRepository();
        var result = await repository.ListAsync(true);
        result.Should().BeEquivalentTo(LegalAuthorityData.LegalAuthorities);
    }
}
