using Enfo.Domain.EnforcementOrders.Resources;
using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.Attachments;

[TestFixture]
public class GetAttachmentTests
{
    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var item = new AttachmentView(AttachmentData.Attachments.First(a => !a.Deleted));

        var result = await repository.GetAttachmentAsync(item.Id);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNotExists_ReturnsNull()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.GetAttachmentAsync(Guid.Empty);
        result.Should().BeNull();
    }
}
