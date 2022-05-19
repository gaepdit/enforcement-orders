using Enfo.Domain.EpdContacts.Resources;
using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.EpdContacts;

public class GetTests
{
    [Test]
    [TestCase(2000)]
    [TestCase(2001)]
    public async Task Get_WhenItemExists_ReturnsItem(int id)
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEpdContactRepository();
        var item = await repository.GetAsync(id);

        var epdContact = EpdContactData.EpdContacts.Single(e => e.Id == id);
        var expected = new EpdContactView(epdContact);

        item.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Get_WhenNotExists_ReturnsNull()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEpdContactRepository();
        var item = await repository.GetAsync(-1);
        item.Should().BeNull();
    }
}
