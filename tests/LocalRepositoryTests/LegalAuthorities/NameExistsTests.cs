using Enfo.LocalRepository;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EnfoTests.LocalRepositoryTests.LegalAuthorities;

[TestFixture]
public class NameExistsTests
{
    [Test]
    public async Task WhenNameExists_ReturnsTrue()
    {
        using var repository = new LegalAuthorityRepository();
        var result = await repository.NameExistsAsync(LegalAuthorityData.LegalAuthorities[0].AuthorityName);
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNameNotExists_ReturnsFalse()
    {
        using var repository = new LegalAuthorityRepository();
        var result = await repository.NameExistsAsync("none");
        result.Should().BeFalse();
    }
}
