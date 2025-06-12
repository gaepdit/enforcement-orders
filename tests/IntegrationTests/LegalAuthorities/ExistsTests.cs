using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;

namespace EnfoTests.Infrastructure.LegalAuthorities;

public class ExistsTests
{
    [Test]
    public async Task Exists_WhenItemExists_ReturnsTrue()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();
        var result = await repository.ExistsAsync(LegalAuthorityData.LegalAuthorities[0].Id);
        result.Should().BeTrue();
    }

    [Test]
    public async Task Exists_WhenItemNotExists_ReturnsFalse()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();
        var result = await repository.ExistsAsync(-1);
        result.Should().BeFalse();
    }
}
