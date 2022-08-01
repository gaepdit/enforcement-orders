using Enfo.Domain.EpdContacts.Resources;
using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.EpdContacts;

public class ListTests
{
    [Test]
    public async Task List_ByDefault_ReturnsOnlyActive()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEpdContactRepository();
        var items = await repository.ListAsync();
        items.Should().HaveCount(EpdContactData.EpdContacts.Count(e => e.Active));

        var epdContact = EpdContactData.EpdContacts.First(e => e.Active);
        var expected = new EpdContactView(epdContact);

        items[0].Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task List_IfIncludeAll_ReturnsAll()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEpdContactRepository();
        var items = await repository.ListAsync(true);
        items.Should().HaveCount(EpdContactData.EpdContacts.Count);

        var epdContact = EpdContactData.EpdContacts.First();
        var expected = new EpdContactView(epdContact);

        items[0].Should().BeEquivalentTo(expected);
    }
}
