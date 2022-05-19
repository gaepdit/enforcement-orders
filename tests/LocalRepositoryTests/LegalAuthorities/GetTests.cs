using Enfo.LocalRepository;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.LocalRepositoryTests.LegalAuthorities;

[TestFixture]
public class GetTests
{
    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        using var repository = new LegalAuthorityRepository();
        var item = LegalAuthorityData.LegalAuthorities.First();

        var result = await repository.GetAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNotExists_ReturnsNull()
    {
        using var repository = new LegalAuthorityRepository();
        var result = await repository.GetAsync(-1);
        result.Should().BeNull();
    }
}
