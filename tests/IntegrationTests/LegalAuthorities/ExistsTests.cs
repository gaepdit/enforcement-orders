using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.LegalAuthorities;

public class ExistsTests
{
    [Test]
    public async Task Exists_WhenItemExists_ReturnsTrue()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetLegalAuthorityRepository();
        var result = await repository.ExistsAsync(LegalAuthorityData.LegalAuthorities.First().Id);
        result.Should().BeTrue();
    }

    [Test]
    public async Task Exists_WhenItemNotExists_ReturnsFalse()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetLegalAuthorityRepository();
        var result = await repository.ExistsAsync(-1);
        result.Should().BeFalse();
    }
}
