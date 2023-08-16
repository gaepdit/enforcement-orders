using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.EnforcementOrderTests;

public class OrderNumberExistsTests
{
    [Test]
    public async Task OrderNumberExists_GivenExists_ReturnsTrue()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.OrderNumberExistsAsync(EnforcementOrderData.EnforcementOrders[0].OrderNumber))
            .Should().BeTrue();
    }

    [Test]
    public async Task OrderNumberExists_GivenNotExists_ReturnsFalse()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.OrderNumberExistsAsync(Guid.NewGuid().ToString()))
            .Should().BeFalse();
    }

    [Test]
    public async Task OrderNumberExists_GivenExistsAndIgnore_ReturnsFalse()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.OrderNumberExistsAsync(EnforcementOrderData.EnforcementOrders[0].OrderNumber,
                EnforcementOrderData.EnforcementOrders[0].Id))
            .Should().BeFalse();
    }
}
