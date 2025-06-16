using Enfo.Domain.EpdContacts.Resources;
using EnfoTests.EfRepository.Helpers;

namespace EnfoTests.EfRepository.EpdContacts;

public class ListTests
{
    [Test]
    public async Task List_ByDefault_ReturnsOnlyActive()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEpdContactRepository();
        var items = await repository.ListAsync();
        items.Should().HaveCount(EpdContactData.EpdContacts.Count(e => e.Active));

        var epdContact = EpdContactData.EpdContacts.First(e => e.Active);
        var expected = new EpdContactView(epdContact);

        items[0].Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task List_IfIncludeAll_ReturnsAll()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEpdContactRepository();
        var items = await repository.ListAsync(true);
        items.Should().HaveCount(EpdContactData.EpdContacts.Count);

        var epdContact = EpdContactData.EpdContacts[0];
        var expected = new EpdContactView(epdContact);

        items[0].Should().BeEquivalentTo(expected);
    }
}
