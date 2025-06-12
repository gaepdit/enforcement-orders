using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;

namespace EnfoTests.Infrastructure.LegalAuthorities;

[TestFixture]
public class NameExistsTests
{
    [Test]
    public async Task NameExists_WhenNameExists_ReturnsTrue()
    {
        var item = LegalAuthorityData.LegalAuthorities[0];
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();
        (await repository.NameExistsAsync(item.AuthorityName)).Should().BeTrue();
    }

    [Test]
    public async Task NameExists_WhenNameNotExists_ReturnsFalse()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();
        (await repository.NameExistsAsync(Guid.NewGuid().ToString())).Should().BeFalse();
    }
}
