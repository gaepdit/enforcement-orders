using Enfo.Domain.Services;
using Enfo.LocalRepository;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestData;

namespace LocalRepositoryTests.EnforcementOrders;

[TestFixture]
public class DeleteRestoreTests
{
    [Test]
    public async Task Delete_Succeeds([Values] bool alreadyDeleted)
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => e.Deleted == alreadyDeleted).Id;
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        await repository.DeleteAsync(itemId);

        var item = await repository.GetAdminViewAsync(itemId);
        item.Deleted.Should().BeTrue();
    }

    [Test]
    public async Task Restore_Succeeds([Values] bool alreadyDeleted)
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => e.Deleted == alreadyDeleted).Id;
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        await repository.RestoreAsync(itemId);

        var item = await repository.GetAdminViewAsync(itemId);
        item.Deleted.Should().BeFalse();
    }

    [Test]
    public async Task Delete_FromMissingId_ThrowsException()
    {
        const int id = -1;

        var action = async () =>
        {
            using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
            await repository.DeleteAsync(id);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be(nameof(id));
    }

    [Test]
    public async Task Restore_FromMissingId_ThrowsException()
    {
        const int id = -1;

        var action = async () =>
        {
            using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
            await repository.RestoreAsync(id);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be(nameof(id));
    }
}
