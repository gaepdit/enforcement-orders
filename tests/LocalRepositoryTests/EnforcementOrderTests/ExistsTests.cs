﻿using Enfo.Domain.Services;
using Enfo.LocalRepository;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using TestData;

namespace LocalRepositoryTests.EnforcementOrders;

[TestFixture]
public class ExistsTests
{
    [Test]
    public async Task WhenItemExistsAndIsPublic_ReturnsTrue()
    {
        using var repository =new EnforcementOrderRepository(new Mock<IFileService>().Object);
        var id = EnforcementOrderData.EnforcementOrders.First(e => e.GetIsPublic).Id;

        var result = await repository.ExistsAsync(id);

        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNotExists_ReturnsFalse()
    {
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
        var result = await repository.ExistsAsync(-1);
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenItemExistsButIsNotPublic_ReturnsFalse()
    {
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
        var id = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic).Id;

        var result = await repository.ExistsAsync(id);

        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenItemExistsAndIsNotPublic_ButNonPublicAllowed_ReturnsTrue()
    {
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
        var id = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic).Id;

        var result = await repository.ExistsAsync(id, false);

        result.Should().BeTrue();
    }
}
