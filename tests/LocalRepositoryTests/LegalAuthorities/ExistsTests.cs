using Enfo.LocalRepository.Repositories;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;

namespace EnfoTests.LocalRepositoryTests.LegalAuthorities;

[TestFixture]
public class ExistsTests
{
    [Test]
    public async Task WhenItemExists_ReturnsTrue()
    {
        using var repository = new LocalLegalAuthorityRepository();
        var result = await repository.ExistsAsync(LegalAuthorityData.LegalAuthorities[0].Id);
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNotExists_ReturnsFalse()
    {
        using var repository = new LocalLegalAuthorityRepository();
        var result = await repository.ExistsAsync(-1);
        result.Should().BeFalse();
    }
}
