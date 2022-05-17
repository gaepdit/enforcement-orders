using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.Services;
using Enfo.LocalRepository.EnforcementOrders;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.EnforcementOrders;

[TestFixture]
public class GetAdminViewTests
{
    [Test]
    public async Task WhenItemExistsAndIsPublic_ReturnsItem()
    {
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => e.GetIsPublic).Id;

        var result = await repository.GetAdminViewAsync(itemId);

        var expected = EnforcementOrderData.GetEnforcementOrderAdminView(itemId);
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task WhenNotExists_ReturnsNull()
    {
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
        var result = await repository.GetAsync(-1);
        result.Should().BeNull();
    }

    [Test]
    public async Task WhenItemExistsButIsNotPublic_ReturnsItem()
    {
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic).Id;

        var result = await repository.GetAdminViewAsync(itemId);

        var expected = EnforcementOrderData.GetEnforcementOrderAdminView(itemId);
        result.Should().BeEquivalentTo(expected);
    }
}
