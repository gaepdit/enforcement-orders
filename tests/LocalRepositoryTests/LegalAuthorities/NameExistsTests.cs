using Enfo.LocalRepository.Repositories;

namespace EnfoTests.LocalRepositoryTests.LegalAuthorities;

[TestFixture]
public class NameExistsTests
{
    [Test]
    public async Task WhenNameExists_ReturnsTrue()
    {
        using var repository = new LocalLegalAuthorityRepository();
        var result = await repository.NameExistsAsync(LegalAuthorityData.LegalAuthorities[0].AuthorityName);
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNameNotExists_ReturnsFalse()
    {
        using var repository = new LocalLegalAuthorityRepository();
        var result = await repository.NameExistsAsync("none");
        result.Should().BeFalse();
    }
}
