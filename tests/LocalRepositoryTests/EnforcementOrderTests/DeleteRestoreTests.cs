using Enfo.LocalRepository.EnforcementOrders;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.EnforcementOrders;

[TestFixture]
public class DeleteRestoreTests
{
    [Test]
    public async Task Delete_Succeeds([Values] bool alreadyDeleted)
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => e.Deleted == alreadyDeleted).Id;
        using var repository = new EnforcementOrderRepository();

        await repository.DeleteAsync(itemId);

        var item = await repository.GetAdminViewAsync(itemId);
        item.Deleted.Should().BeTrue();
    }

    [Test]
    public async Task Restore_Succeeds([Values] bool alreadyDeleted)
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => e.Deleted == alreadyDeleted).Id;
        using var repository = new EnforcementOrderRepository();

        await repository.RestoreAsync(itemId);

        var item = await repository.GetAdminViewAsync(itemId);
        item.Deleted.Should().BeFalse();
    }

    [Test]
    public async Task Delete_FromMissingId_ThrowsException()
    {
        const int itemId = -1;

        var action = async () =>
        {
            using var repository = new EnforcementOrderRepository();
            await repository.DeleteAsync(itemId);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be("id");
    }

    [Test]
    public async Task Restore_FromMissingId_ThrowsException()
    {
        const int itemId = -1;

        var action = async () =>
        {
            using var repository = new EnforcementOrderRepository();
            await repository.RestoreAsync(itemId);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be("id");
    }
}
