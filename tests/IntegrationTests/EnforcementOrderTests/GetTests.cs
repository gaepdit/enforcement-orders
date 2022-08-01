using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.EnforcementOrderTests;

public class GetTests
{
    [Test]
    public async Task Get_WhenItemExistsAndIsPublic_ReturnsItem()
    {
        var item = EnforcementOrderData.EnforcementOrders.First(e => e.GetIsPublic);
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();

        var result = await repository.GetAsync(item.Id);

        var expected = ResourceHelper.GetEnforcementOrderDetailedView(item.Id);
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Get_WhenNotExists_ReturnsNull()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.GetAsync(-1)).Should().BeNull();
    }

    [Test]
    public async Task Get_WhenItemExistsButIsNotPublic_ReturnsNull()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic).Id;
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();

        var result = await repository.GetAsync(itemId);

        result.Should().BeNull();
    }

    [Test]
    public async Task GetAdminView_WhenItemExistsAndIsPublic_ReturnsItem()
    {
        var item = EnforcementOrderData.EnforcementOrders.First(e => e.GetIsPublic);
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();

        var result = await repository.GetAdminViewAsync(item.Id);

        var expected = ResourceHelper.GetEnforcementOrderAdminView(item.Id);
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetAdminView_WhenNotExists_ReturnsNull()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.GetAdminViewAsync(-1)).Should().BeNull();
    }

    [Test]
    public async Task GetAdminView_WhenItemExistsButIsNotPublic_ReturnsItem()
    {
        var item = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic);
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();

        var result = await repository.GetAdminViewAsync(item.Id);

        var expected = ResourceHelper.GetEnforcementOrderAdminView(item.Id);
        result.Should().BeEquivalentTo(expected);
    }
}
