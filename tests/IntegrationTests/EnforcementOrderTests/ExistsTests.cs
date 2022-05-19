using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.EnforcementOrderTests;

public class ExistsTests
{
    [Test]
    public async Task Exists_GivenExists_ReturnsTrue()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => e.GetIsPublic).Id;
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.ExistsAsync(itemId)).Should().BeTrue();
    }

    [Test]
    public async Task Exists_GivenNotExists_ReturnsFalse()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.ExistsAsync(-1)).Should().BeFalse();
    }

    [Test]
    public async Task Exists_GivenNonpublic_ReturnsFalse()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic).Id;
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.ExistsAsync(itemId)).Should().BeFalse();
    }

    [Test]
    public async Task Exists_GivenNonpublicButAllowed_ReturnsTrue()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic).Id;
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.ExistsAsync(itemId, false)).Should().BeTrue();
    }
}
