﻿using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.LocalRepository.EnforcementOrders;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.EnforcementOrders;

[TestFixture]
public class GetTests
{
    [Test]
    public async Task WhenItemExistsAndIsPublic_ReturnsItem()
    {
        using var repository = new EnforcementOrderRepository();
        var item = EnforcementOrderData.EnforcementOrders.First(e => e.GetIsPublic);

        var result = await repository.GetAsync(item.Id);

        var expected = new EnforcementOrderDetailedView(item);
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task WhenNotExists_ReturnsNull()
    {
        using var repository = new EnforcementOrderRepository();
        var result = await repository.GetAsync(-1);
        result.Should().BeNull();
    }

    [Test]
    public async Task WhenItemExistsButIsNotPublic_ReturnsNull()
    {
        using var repository = new EnforcementOrderRepository();
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic).Id;

        var result = await repository.GetAsync(itemId);

        result.Should().BeNull();
    }
}