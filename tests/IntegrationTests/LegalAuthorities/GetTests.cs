using Enfo.Domain.LegalAuthorities.Resources;
using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;

namespace EnfoTests.Infrastructure.LegalAuthorities;

[TestFixture]
public class GetTests
{
    [Test]
    [TestCase(1)]
    [TestCase(2)]
    public async Task Get_WhenItemExists_ReturnsItem(int id)
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();
        var item = new LegalAuthorityView(LegalAuthorityData.LegalAuthorities.Single(e => e.Id == id));

        var result = await repository.GetAsync(id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task Get_WhenNotExists_ReturnsNull()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();
        var item = await repository.GetAsync(-1);
        item.Should().BeNull();
    }
}
