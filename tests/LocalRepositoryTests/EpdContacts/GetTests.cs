using Enfo.LocalRepository.EpdContacts;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.EpdContacts;

[TestFixture]
public class GetTests
{
    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        using var repository = new EpdContactRepository();
        var item = EpdContactData.EpdContacts.First();

        var result = await repository.GetAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNotExists_ReturnsNull()
    {
        using var repository = new EpdContactRepository();
        var result = await repository.GetAsync(-1);
        result.Should().BeNull();
    }
}
