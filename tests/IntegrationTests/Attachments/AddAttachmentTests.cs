using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.Attachments;

[TestFixture]
public class AddAttachmentTests
{
    [Test]
    public async Task AttachingValidItem_Succeeds()
    {
        // Arrange
        var file = new FormFile(Stream.Null, 0, 1, "test", "test.pdf");
        var orderId = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).Id;
        var initialCount = AttachmentData.Attachments.Count(a => a.EnforcementOrder.Id == orderId);

        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        // Act
        await repository.AddAttachmentAsync(orderId, file);

        // Assert
        repositoryHelper.ClearChangeTracker();

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
    public async Task WhenOrderIdDoesNotExist_ThrowsException()
    {
        var file = new FormFile(Stream.Null, 0, 0, string.Empty, string.Empty);
        const int orderId = -1;

        var action = async () =>
        {
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
            await repository.AddAttachmentAsync(orderId, file);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Order ID {orderId} does not exist. (Parameter '{nameof(orderId)}')")
            .And.ParamName.Should().Be(nameof(orderId));
    }

    [Test]
    public async Task WhenOrderIsDeleted_ThrowsException()
    {
        var file = new FormFile(Stream.Null, 0, 0, string.Empty, string.Empty);
        var orderId = EnforcementOrderData.EnforcementOrders.First(e => e.Deleted).Id;

        var action = async () =>
        {
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
            await repository.AddAttachmentAsync(orderId, file);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"Order ID {orderId} has been deleted and cannot be edited. (Parameter '{nameof(orderId)}')")
            .And.ParamName.Should().Be(nameof(orderId));
    }
}
