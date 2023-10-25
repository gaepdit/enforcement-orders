using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Resources;
using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.EnforcementOrderTests;

public class UpdateTests
{
    private static EnforcementOrderUpdate NewSampleUpdate(EnforcementOrder order) => new()
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
    public async Task Update_Succeeds()
    {
        var existingOrder = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted);
        var itemId = existingOrder.Id;

        var itemUpdate = NewSampleUpdate(existingOrder);
        itemUpdate.Cause = "abc";

        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        await repository.UpdateAsync(itemUpdate);
        repositoryHelper.ClearChangeTracker();

        (await repository.GetAsync(itemId)).Cause.Should().Be("abc");
    }

    [Test]
    public async Task Update_DeletedOrder_ThrowsException()
    {
        var existingOrder = EnforcementOrderData.EnforcementOrders.First(e => e.Deleted);
        var itemUpdate = NewSampleUpdate(existingOrder);

        var action = async () =>
        {
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
            await repository.UpdateAsync(itemUpdate);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage("A deleted Enforcement Order cannot be modified. (Parameter 'resource')")
            .And.ParamName.Should().Be("resource");
    }

    [Test]
    public async Task Update_WithDuplicateOrderNumber_Fails()
    {
        var existingOrder = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted);

        var itemUpdate = NewSampleUpdate(existingOrder);
        itemUpdate.OrderNumber = EnforcementOrderData.EnforcementOrders.Last(e => !e.Deleted).OrderNumber;

        var action = async () =>
        {
            using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
            await repository.UpdateAsync(itemUpdate);
        };

        await action.Should().ThrowAsync<ArgumentException>();
    }
}
