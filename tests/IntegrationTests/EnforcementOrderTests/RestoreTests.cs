using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.EnforcementOrderTests;

[TestFixture]
public class RestoreTests
{
    [Test]
    public async Task Restore_Succeeds()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => e.Deleted).Id;

        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
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

        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        await repository.RestoreAsync(itemId);
        repositoryHelper.ClearChangeTracker();

        (await repository.GetAdminViewAsync(itemId))
            .Deleted.Should().BeFalse();
    }
}
