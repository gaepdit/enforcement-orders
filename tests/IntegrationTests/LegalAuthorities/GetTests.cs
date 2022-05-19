using Enfo.Domain.LegalAuthorities.Resources;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using static EnfoTests.Infrastructure.Helpers.RepositoryHelper;

namespace EnfoTests.Infrastructure.LegalAuthorities;

[TestFixture]
public class GetTests
{
    [Test]
    [TestCase(1)]
    [TestCase(2)]
    public async Task Get_WhenItemExists_ReturnsItem(int id)
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var item = new LegalAuthorityView(LegalAuthorityData.LegalAuthorities.Single(e => e.Id == id));

        var result = await repository.GetAsync(id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task Get_WhenNotExists_ReturnsNull()
    {
        using var repository = CreateRepositoryHelper().GetLegalAuthorityRepository();
        var item = await repository.GetAsync(-1);
        item.Should().BeNull();
    }
}
