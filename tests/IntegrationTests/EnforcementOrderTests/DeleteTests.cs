namespace EnfoTests.Infrastructure.EnforcementOrderTests;

public class DeleteTests
{
    [Test]
    public async Task Delete_Succeeds()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).Id;

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        await repository.DeleteAsync(itemId);
        repositoryHelper.ClearChangeTracker();

        (await repository.GetAdminViewAsync(itemId))
            .Deleted.Should().BeTrue();
    }

    [Test]
    public async Task Delete_AlreadyDeletedItem_DoesNotChange()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => e.Deleted).Id;

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        await repository.DeleteAsync(itemId);
        repositoryHelper.ClearChangeTracker();

        (await repository.GetAdminViewAsync(itemId))
            .Deleted.Should().BeTrue();
    }
}
