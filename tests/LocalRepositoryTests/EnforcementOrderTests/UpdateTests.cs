using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.Services;
using Enfo.LocalRepository;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestData;

namespace LocalRepositoryTests.EnforcementOrders;

[TestFixture]
public class UpdateTests
{
    // Sample data for update
    private static EnforcementOrderUpdate getUpdateResource(EnforcementOrder order) => new()
    {
        Id = order.Id,
        Cause = order.Cause,
        County = order.County,
        Requirements = order.Requirements,
        ExecutedDate = order.ExecutedDate,
        FacilityName = order.FacilityName,
        HearingDate = order.HearingDate,
        HearingLocation = order.HearingLocation,
        OrderNumber = order.OrderNumber,
        Progress = order.PublicationStatus == EnforcementOrder.PublicationState.Draft
            ? PublicationProgress.Draft
            : PublicationProgress.Published,
        SettlementAmount = order.SettlementAmount,
        CommentContactId = order.CommentContactId,
        HearingContactId = order.HearingContactId,
        IsExecutedOrder = order.IsExecutedOrder,
        IsHearingScheduled = order.IsHearingScheduled,
        LegalAuthorityId = order.LegalAuthorityId,
        CommentPeriodClosesDate = order.CommentPeriodClosesDate,
        ExecutedOrderPostedDate = order.ExecutedOrderPostedDate,
        ProposedOrderPostedDate = order.ProposedOrderPostedDate,
        HearingCommentPeriodClosesDate = order.HearingCommentPeriodClosesDate,
    };

    [Test]
    public async Task FromValidItem_Updates()
    {
        var original = EnforcementOrderData.EnforcementOrders.First();
        var resource = getUpdateResource(original);
        resource.Cause = "new text";

        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        await repository.UpdateAsync(resource);

        var expectedItem = new EnforcementOrderAdminSummaryView(original);
        var updatedItem = await repository.GetAdminViewAsync(original.Id);
        updatedItem.Should().BeEquivalentTo(expectedItem);
    }

    [Test]
    public async Task WithNoChanges_Succeeds()
    {
        var original = EnforcementOrderData.EnforcementOrders.First();
        var resource = getUpdateResource(original);

        using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);

        await repository.UpdateAsync(resource);

        var expectedItem = new EnforcementOrderAdminSummaryView(original);
        var updatedItem = await repository.GetAdminViewAsync(original.Id);
        updatedItem.Should().BeEquivalentTo(expectedItem);
    }

    [Test]
    public async Task FromInvalidItem_ThrowsException()
    {
        var original = EnforcementOrderData.EnforcementOrders.First();
        var resource = getUpdateResource(original);
        resource.County = null;

        var action = async () =>
        {
            using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
            await repository.UpdateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be(nameof(EnforcementOrderCreate.County));
    }

    [Test]
    public async Task WithMissingId_ThrowsException()
    {
        var original = new EnforcementOrderAdminView(EnforcementOrderData.EnforcementOrders.First());
        var resource = new EnforcementOrderUpdate(original) { Id = -1 };

        var action = async () =>
        {
            using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
            await repository.UpdateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"ID ({resource.Id}) not found. (Parameter 'resource')")
            .And.ParamName.Should().Be(nameof(resource));
    }

    [Test]
    public async Task FromDeletedOrder_ThrowsException()
    {
        var original = EnforcementOrderData.EnforcementOrders.First(e => e.Deleted);
        var resource = getUpdateResource(original);

        var action = async () =>
        {
            using var repository = new EnforcementOrderRepository(new Mock<IFileService>().Object);
            await repository.UpdateAsync(resource);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage("A deleted Enforcement Order cannot be modified. (Parameter 'resource')")
            .And.ParamName.Should().Be(nameof(resource));
    }
}
