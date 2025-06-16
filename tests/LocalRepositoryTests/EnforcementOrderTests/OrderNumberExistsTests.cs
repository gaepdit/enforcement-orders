using Enfo.Domain.Services;
using Enfo.LocalRepository.Repositories;

namespace EnfoTests.LocalRepositoryTests.EnforcementOrderTests;

[TestFixture]
public class OrderNumberExistsTests
{
    [Test]
    public async Task WhenExists_ReturnsTrue()
    {
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());
        var orderNumber = EnforcementOrderData.EnforcementOrders[0].OrderNumber;

        var result = await repository.OrderNumberExistsAsync(orderNumber);

        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenExistsInADifferentCase_ReturnsTrue()
    {
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());
        var orderNumber = EnforcementOrderData.EnforcementOrders[0].OrderNumber.ToLower();

        var result = await repository.OrderNumberExistsAsync(orderNumber);

        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNotExists_ReturnsFalse()
    {
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());
        var result = await repository.OrderNumberExistsAsync("none");
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenExistsAndIdIsIgnored_ReturnsFalse()
    {
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());
        var orderNumber = EnforcementOrderData.EnforcementOrders[0].OrderNumber;
        var id = EnforcementOrderData.EnforcementOrders[0].Id;

        var result = await repository.OrderNumberExistsAsync(orderNumber, id);

        result.Should().BeFalse();
    }
}
