using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.Attachments;

[TestFixture]
public class DeleteAttachmentTests
{
    [Test]
    public async Task WhenAttachmentExists_MarksAsDeleted()
    {
        // Arrange
        var initialFileCount = AttachmentData.Attachments.Count;
        var attachment = AttachmentData.Attachments.First(a => !a.Deleted);

        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();
        
        // Act
        await repository.DeleteAttachmentAsync(attachment.EnforcementOrder.Id, attachment.Id).ConfigureAwait(false);

        // Assert
        repositoryHelper.ClearChangeTracker();

        await using var context = repositoryHelper.DbContext;
        var updatedAttachment = context.Attachments.Single(a => a.Id == attachment.Id);

        using (new AssertionScope())
        {
            context.Attachments.Count().Should().Be(initialFileCount);
            updatedAttachment.Deleted.Should().BeTrue();
            updatedAttachment.DateDeleted.Should().NotBeNull();
        }

        // Cleanup
        updatedAttachment.Deleted = false;
        updatedAttachment.DateDeleted = null;
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    [Test]
    public async Task WhenOrderIdDoesNotExist_ThrowsException()
    {
        const int orderId = -1;

        var action = async () =>
        {
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
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
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
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
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
            await repository.DeleteAttachmentAsync(orderId, attachmentId);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Attachment ID {attachmentId} does not exist. (Parameter '{nameof(attachmentId)}')")
            .And.ParamName.Should().Be(nameof(attachmentId));
    }

    [Test]
    public async Task WhenOrderDoesNotIncludeAttachment_ThrowsException()
    {
        var attachment = AttachmentData.Attachments.First(a => !a.Deleted);

        var orderId = EnforcementOrderData.EnforcementOrders
            .First(e => !e.Deleted && e.Id != attachment.EnforcementOrder.Id).Id;

        var action = async () =>
        {
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
            await repository.DeleteAttachmentAsync(orderId, attachment.Id);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage(
                $"Order ID {orderId} does not include Attachment ID {attachment.Id}. (Parameter 'attachmentId')")
            .And.ParamName.Should().Be("attachmentId");
    }
}
