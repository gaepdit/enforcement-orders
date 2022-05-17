using Enfo.Domain.Services;
using Enfo.LocalRepository.Attachments;
using Enfo.LocalRepository.EnforcementOrders;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.Attachments;

[TestFixture]
public class DeleteAttachmentTests
{
    [Test]
    public async Task WhenItemExists_RemovesItem()
    {
        var initialFileCount = AttachmentData.Attachments.Count;
        var item = AttachmentData.Attachments.First();

        var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
        await repository.DeleteAttachmentAsync(item.EnforcementOrder.Id, item.Id);

        AttachmentData.Attachments.Count.Should().Be(initialFileCount - 1);
        AttachmentData.Attachments.Any(a => a.Id == item.Id).Should().BeFalse();
    }

    [Test]
    public async Task WhenOrderIdDoesNotExist_ThrowsException()
    {
        const int orderId = -1;

        var action = async () =>
        {
            var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
            await repository.DeleteAttachmentAsync(orderId, Guid.Empty);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Order ID {orderId} does not exist. (Parameter '{nameof(orderId)}')")
            .And.ParamName.Should().Be(nameof(orderId));
    }

    [Test]
    public async Task WhenOrderIsDeleted_ThrowsException()
    {
        var orderId = EnforcementOrderData.EnforcementOrders.First(e => e.Deleted).Id;

        var action = async () =>
        {
            var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
            await repository.DeleteAttachmentAsync(orderId, Guid.Empty);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Order ID {orderId} has been deleted and cannot be edited. (Parameter '{nameof(orderId)}')")
            .And.ParamName.Should().Be(nameof(orderId));
    }

    [Test]
    public async Task WhenAttachmentIdDoesNotExist_ThrowsException()
    {
        var orderId = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).Id;
        var attachmentId = Guid.NewGuid();

        var action = async () =>
        {
            var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
            await repository.DeleteAttachmentAsync(orderId, attachmentId);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Attachment ID {attachmentId} does not exist. (Parameter '{nameof(attachmentId)}')")
            .And.ParamName.Should().Be(nameof(attachmentId));
    }
}
