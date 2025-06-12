using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;

namespace EnfoTests.Infrastructure.EnforcementOrderTests;

public class GetTests
{
    [Test]
    public async Task Get_WhenItemExistsAndIsPublic_ReturnsItem()
    {
        var item = EnforcementOrderData.EnforcementOrders.First(e => e.GetIsPublic);
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        var result = await repository.GetAsync(item.Id);

        var expected = ResourceHelper.GetEnforcementOrderDetailedView(item.Id);
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Get_WhenNotExists_ReturnsNull()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        (await repository.GetAsync(-1)).Should().BeNull();
    }

    [Test]
    public async Task Get_WhenItemExistsButIsNotPublic_ReturnsNull()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic).Id;
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        var result = await repository.GetAsync(itemId);

        result.Should().BeNull();
    }

    [Test]
    public async Task GetAdminView_WhenItemExistsAndIsPublic_ReturnsItem()
    {
        var item = EnforcementOrderData.EnforcementOrders.First(e => e.GetIsPublic);
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        var result = await repository.GetAdminViewAsync(item.Id);

        var expected = ResourceHelper.GetEnforcementOrderAdminView(item.Id);
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetAdminView_WhenNotExists_ReturnsNull()
    {
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        (await repository.GetAdminViewAsync(-1)).Should().BeNull();
    }

    [Test]
    public async Task GetAdminView_WhenItemExistsButIsNotPublic_ReturnsItem()
    {
        var item = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic);
        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        var result = await repository.GetAdminViewAsync(item.Id);

        var expected = ResourceHelper.GetEnforcementOrderAdminView(item.Id);
        result.Should().BeEquivalentTo(expected);
    }
}
