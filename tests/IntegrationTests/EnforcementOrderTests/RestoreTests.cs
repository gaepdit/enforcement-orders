namespace EnfoTests.Infrastructure.EnforcementOrderTests;

[TestFixture]
public class RestoreTests
{
    [Test]
    public async Task Restore_Succeeds()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => e.Deleted).Id;

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        await repository.RestoreAsync(itemId);
        repositoryHelper.ClearChangeTracker();

        (await repository.GetAdminViewAsync(itemId))
            .Deleted.Should().BeFalse();
    }

    [Test]
    public async Task Restore_AlreadyDeletedItem_DoesNotChange()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).Id;

        await using var repositoryHelper = await RepositoryHelper.CreateRepositoryHelperAsync();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        await repository.RestoreAsync(itemId);
        repositoryHelper.ClearChangeTracker();

        (await repository.GetAdminViewAsync(itemId))
            .Deleted.Should().BeFalse();
    }
}
