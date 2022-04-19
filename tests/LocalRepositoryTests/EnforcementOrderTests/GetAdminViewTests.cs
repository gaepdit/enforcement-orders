
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.LocalRepository.EnforcementOrders;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.EnforcementOrders;

[TestFixture]
public class GetAdminViewTests
{
    [Test]
    public async Task WhenItemExistsAndIsPublic_ReturnsItem()
    {
        using var repository = new EnforcementOrderRepository();
        var item = EnforcementOrderData.EnforcementOrders.First(e => e.GetIsPublic);

        var result = await repository.GetAdminViewAsync(item.Id);

        var expected = new EnforcementOrderAdminView(item);
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task WhenNotExists_ReturnsNull()
    {
        using var repository = new EnforcementOrderRepository();
        var result = await repository.GetAsync(-1);
        result.Should().BeNull();
    }

    [Test]
    public async Task WhenItemExistsButIsNotPublic_ReturnsItem()
    {
        using var repository = new EnforcementOrderRepository();
        var item = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic);

        var result = await repository.GetAdminViewAsync(item.Id);

        var expected = new EnforcementOrderAdminView(item);
        result.Should().BeEquivalentTo(expected);
    }
}
