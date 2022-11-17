using Enfo.Domain.LegalAuthorities.Resources;
using EnfoTests.TestData;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using static EnfoTests.Infrastructure.Helpers.RepositoryHelper;

namespace EnfoTests.Infrastructure.LegalAuthorities;

[TestFixture]
public class ListTests
{
    [Test]
    public async Task List_ByDefault_ReturnsOnlyActive()
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
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
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var items = await repository.ListAsync(true);

        using (new AssertionScope())
        {
            items.Should().HaveCount(LegalAuthorityData.LegalAuthorities.Count);
            items.Should().BeEquivalentTo(LegalAuthorityData.LegalAuthorities.Select(e => new LegalAuthorityView(e)));
        }
    }
}
