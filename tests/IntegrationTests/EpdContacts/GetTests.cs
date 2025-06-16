using Enfo.Domain.EpdContacts.Resources;

namespace EnfoTests.Infrastructure.EpdContacts;

public class GetTests
{
    [Test]
    [TestCase(2000)]
    [TestCase(2001)]
    public async Task Get_WhenItemExists_ReturnsItem(int id)
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEpdContactRepository();
        var item = await repository.GetAsync(id);

        var epdContact = EpdContactData.EpdContacts.Single(e => e.Id == id);
        var expected = new EpdContactView(epdContact);

        item.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Get_WhenNotExists_ReturnsNull()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEpdContactRepository();
        var item = await repository.GetAsync(-1);
        item.Should().BeNull();
    }
}
