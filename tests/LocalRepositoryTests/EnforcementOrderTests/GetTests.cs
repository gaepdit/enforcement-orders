using Enfo.Domain.Services;
using Enfo.LocalRepository.Repositories;

namespace EnfoTests.LocalRepositoryTests.EnforcementOrderTests;

[TestFixture]
public class GetTests
{
    [Test]
    public async Task WhenItemExistsAndIsPublic_ReturnsItem()
    {
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => e.GetIsPublic).Id;

        var result = await repository.GetAsync(itemId);

        var expected = EnforcementOrderData.GetEnforcementOrderDetailedView(itemId);
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task WhenNotExists_ReturnsNull()
    {
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());
        var result = await repository.GetAsync(-1);
        result.Should().BeNull();
    }

    [Test]
    public async Task WhenItemExistsButIsNotPublic_ReturnsNull()
    {
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic).Id;

        var result = await repository.GetAsync(itemId);

        result.Should().BeNull();
    }
}
