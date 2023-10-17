using Enfo.Domain.Services;
using Enfo.LocalRepository;
using EnfoTests.TestData;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.LocalRepositoryTests.EnforcementOrderTests;

[TestFixture]
public class DeleteRestoreTests
{
    [Test]
    public async Task Delete_Succeeds([Values] bool alreadyDeleted)
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => e.Deleted == alreadyDeleted).Id;
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IFileService>());

        await repository.DeleteAsync(itemId);

        var item = await repository.GetAdminViewAsync(itemId);
        item.Deleted.Should().BeTrue();
    }

    [Test]
    public async Task Restore_Succeeds([Values] bool alreadyDeleted)
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => e.Deleted == alreadyDeleted).Id;
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IFileService>());

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
            using var repository = new LocalEnforcementOrderRepository(Substitute.For<IFileService>());
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
            using var repository = new LocalEnforcementOrderRepository(Substitute.For<IFileService>());
            await repository.RestoreAsync(id);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be(nameof(id));
    }
}
