using Enfo.Domain.Services;
using Enfo.LocalRepository.EnforcementOrders;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.EnforcementOrders;

[TestFixture]
public class OrderNumberExistsTests
{
    [Test]
    public async Task WhenExists_ReturnsTrue()
    {
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
        var orderNumber = EnforcementOrderData.EnforcementOrders.First().OrderNumber;

        var result = await repository.OrderNumberExistsAsync(orderNumber);

        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNotExists_ReturnsFalse()
    {
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
        var result = await repository.OrderNumberExistsAsync("none");
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenExistsAndIdIsIgnored_ReturnsFalse()
    {
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
        var orderNumber = EnforcementOrderData.EnforcementOrders.First().OrderNumber;
        var id = EnforcementOrderData.EnforcementOrders.First().Id;

        var result = await repository.OrderNumberExistsAsync(orderNumber, id);

        result.Should().BeFalse();
    }
}
