using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.Services;
using Enfo.LocalRepository.Repositories;

namespace LocalRepositoryTests.Attachments;

[TestFixture]
public class GetAttachmentTests
{
    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());
        var item = new AttachmentView(AttachmentData.Attachments[0]);

        var result = await repository.GetAttachmentAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNotExists_ReturnsNull()
    {
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());
        var result = await repository.GetAttachmentAsync(Guid.Empty);
        result.Should().BeNull();
    }
}
