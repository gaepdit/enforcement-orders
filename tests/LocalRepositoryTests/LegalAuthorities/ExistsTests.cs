using Enfo.LocalRepository;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.LocalRepositoryTests.LegalAuthorities;

[TestFixture]
public class ExistsTests
{
    [Test]
    public async Task WhenItemExists_ReturnsTrue()
    {
        using var repository = new LegalAuthorityRepository();
        var result = await repository.ExistsAsync(LegalAuthorityData.LegalAuthorities.First().Id);
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNotExists_ReturnsFalse()
    {
        using var repository = new LegalAuthorityRepository();
        var result = await repository.ExistsAsync(-1);
        result.Should().BeFalse();
    }
}
