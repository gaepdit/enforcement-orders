using Enfo.Domain.Services;
using Enfo.LocalRepository;
using EnfoTests.TestData;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.LocalRepositoryTests.EnforcementOrderTests;

[TestFixture]
public class GetTests
{
    [Test]
    public async Task WhenItemExistsAndIsPublic_ReturnsItem()
    {
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => e.GetIsPublic).Id;

        var result = await repository.GetAsync(itemId);

        var expected = EnforcementOrderData.GetEnforcementOrderDetailedView(itemId);
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
    public async Task WhenItemExistsButIsNotPublic_ReturnsNull()
    {
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic).Id;

        var result = await repository.GetAsync(itemId);

        result.Should().BeNull();
    }
}
