using Enfo.LocalRepository;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.LocalRepositoryTests.LegalAuthorities;

[TestFixture]
public class NameExistsTests
{
    [Test]
    public async Task WhenNameExists_ReturnsTrue()
    {
        using var repository = new LegalAuthorityRepository();
        var result = await repository.NameExistsAsync(LegalAuthorityData.LegalAuthorities.First().AuthorityName);
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNameNotExists_ReturnsFalse()
    {
        using var repository = new LegalAuthorityRepository();
        var result = await repository.NameExistsAsync("none");
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenNameExists_WithMatchingId_ReturnsFalse()
    {
        var item = LegalAuthorityData.LegalAuthorities.First();
        using var repository = new LegalAuthorityRepository();

        var result = await repository.NameExistsAsync(item.AuthorityName, item.Id);
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenNameExists_WithNonMatchingId_ReturnsTrue()
    {
        var item = LegalAuthorityData.LegalAuthorities.First();
        using var repository = new LegalAuthorityRepository();
        var result = await repository.NameExistsAsync(item.AuthorityName, -1);
        result.Should().BeTrue();
    }
}
