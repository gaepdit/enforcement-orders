using Enfo.LocalRepository.Repositories;

namespace EnfoTests.LocalRepositoryTests.EpdContacts;

[TestFixture]
public class GetTests
{
    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        using var repository = new LocalEpdContactRepository();
        var item = EpdContactData.EpdContacts[0];

        var result = await repository.GetAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNotExists_ReturnsNull()
    {
        using var repository = new LocalEpdContactRepository();
        var result = await repository.GetAsync(-1);
        result.Should().BeNull();
    }
}
