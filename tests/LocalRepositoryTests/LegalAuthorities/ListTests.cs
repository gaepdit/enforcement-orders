using Enfo.LocalRepository.LegalAuthorities;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.LegalAuthorities;

[TestFixture]
public class ListTests
{
    [Test]
    public async Task ByDefault_ReturnsOnlyActive()
    {
        using var repository = new LegalAuthorityRepository();
        var result = await repository.ListAsync();
        result.Should().BeEquivalentTo(LegalAuthorityData.LegalAuthorities.Where(e => e.Active));
    }

    [Test]
    public async Task IfIncludeAll_ReturnsAll()
    {
        using var repository = new LegalAuthorityRepository();
        var result = await repository.ListAsync(true);
        result.Should().BeEquivalentTo(LegalAuthorityData.LegalAuthorities);
    }
}
