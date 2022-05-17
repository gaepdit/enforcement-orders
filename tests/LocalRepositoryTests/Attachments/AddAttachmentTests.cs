using Enfo.Domain.Services;
using Enfo.LocalRepository.Attachments;
using Enfo.LocalRepository.EnforcementOrders;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.Attachments;

[TestFixture]
public class AddAttachmentTests
{
    [Test]
    public async Task AttachingValidItem_Succeeds()
    {
        // Arrange
        var files = new List<IFormFile> { new FormFile(Stream.Null, 0, 1, "test", "test.pdf") };
        var orderId = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).Id;
        var initialCount = AttachmentData.Attachments.Count(a => a.EnforcementOrder.Id == orderId);

        // Act
        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
        await repository.AddAttachmentsAsync(orderId, files);

        // Assert
        var order = await repository.GetAsync(orderId);

        Assert.Multiple(() =>
        {
            order.Attachments.Count.Should().Be(initialCount + 1);
            var attachment = order.Attachments.Last();
            attachment.FileName.Should().Be("test.pdf");
            attachment.FileExtension.Should().Be(".pdf");
            attachment.Size.Should().Be(1);
        });
    }

    [Test]
    public async Task WhenFilesListIsEmpty_ThrowsException()
    {
        var files = new List<IFormFile>();

        var action = async () =>
        {
            using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
            await repository.AddAttachmentsAsync(default, files);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Files list must not be empty. (Parameter '{nameof(files)}')")
            .And.ParamName.Should().Be(nameof(files));
    }

    [Test]
    public async Task WhenOrderIdDoesNotExist_ThrowsException()
    {
        var files = new List<IFormFile> { new FormFile(Stream.Null, 0, 0, string.Empty, string.Empty) };
        const int orderId = -1;

        var action = async () =>
        {
            using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
            await repository.AddAttachmentsAsync(orderId, files);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Order ID {orderId} does not exist. (Parameter '{nameof(orderId)}')")
            .And.ParamName.Should().Be(nameof(orderId));
    }

    [Test]
    public async Task WhenOrderIsDeleted_ThrowsException()
    {
        var files = new List<IFormFile> { new FormFile(Stream.Null, 0, 0, string.Empty, string.Empty) };
        var orderId = EnforcementOrderData.EnforcementOrders.First(e => e.Deleted).Id;

        var action = async () =>
        {
            using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
            await repository.AddAttachmentsAsync(orderId, files);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Order ID {orderId} has been deleted and cannot be edited. (Parameter '{nameof(orderId)}')")
            .And.ParamName.Should().Be(nameof(orderId));
    }
}
