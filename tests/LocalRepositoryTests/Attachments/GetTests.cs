using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.LocalRepository.Attachments;
using Enfo.LocalRepository.EnforcementOrders;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.Attachments;

[TestFixture]
public class GetTests
{
    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        using var repository = new EnforcementOrderRepository();
        var item = new AttachmentView(AttachmentData.Attachments.First());

        var result = await repository.GetAttachmentAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNotExists_ReturnsNull()
    {
        using var repository = new EnforcementOrderRepository();
        var result = await repository.GetAttachmentAsync(Guid.Empty);
        result.Should().BeNull();
    }
}
