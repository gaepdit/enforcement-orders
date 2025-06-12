using Enfo.Domain.LegalAuthorities.Resources;
using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace EnfoTests.Infrastructure.LegalAuthorities;

[TestFixture]
public class ListTests
{
    [Test]
    public async Task List_ByDefault_ReturnsOnlyActive()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();
        var items = await repository.ListAsync();

        using (new AssertionScope())
        {
            items.Should().HaveCount(LegalAuthorityData.LegalAuthorities.Count(e => e.Active));
            items[0].Should()
                .BeEquivalentTo(new LegalAuthorityView(LegalAuthorityData.LegalAuthorities.First(e => e.Active)));
        }
    }

    [Test]
    public async Task List_IfIncludeAll_ReturnsAll()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetLegalAuthorityRepository();
        var items = await repository.ListAsync(true);

        using (new AssertionScope())
        {
            items.Should().HaveCount(LegalAuthorityData.LegalAuthorities.Count);
            items.Should().BeEquivalentTo(LegalAuthorityData.LegalAuthorities.Select(e => new LegalAuthorityView(e)));
        }
    }
}
