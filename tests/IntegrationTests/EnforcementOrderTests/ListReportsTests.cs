using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.Utils;
using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure.EnforcementOrderTests;

public class ListReportsTests
{
    [Test]
    public async Task ListCurrentProposedEnforcementOrders_ReturnsCorrectly()
    {
        var order = EnforcementOrderData.EnforcementOrders.First(e => e.GetIsPublic && e.IsProposedOrder);
        order.CommentPeriodClosesDate = DateTime.Today.AddDays(1);

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListCurrentProposedEnforcementOrdersAsync();

        using (new AssertionScope())
        {
            result.Count.Should().Be(EnforcementOrderData.EnforcementOrders.Count(e =>
                e.GetIsPublic && e.IsProposedOrder && e.CommentPeriodClosesDate >= DateTime.Today));
            result[0].Should().BeEquivalentTo(ResourceHelper.GetEnforcementOrderSummaryView(order.Id));
        }
    }

    [Test]
    public async Task ListRecentlyExecutedEnforcementOrders_ReturnsCorrectly()
    {
        var order = EnforcementOrderData.EnforcementOrders
            .OrderBy(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
            .ThenBy(e => e.FacilityName)
            .First(e => e.GetIsPublicExecutedOrder);
        order.ExecutedOrderPostedDate = DateUtils.MostRecentMonday();

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListRecentlyExecutedEnforcementOrdersAsync();

        using (new AssertionScope())
        {
            result.Count.Should().Be(EnforcementOrderData.EnforcementOrders
                .Count(e => e.GetIsPublicExecutedOrder
                    && e.ExecutedOrderPostedDate >= DateUtils.MostRecentMonday()
                    && e.ExecutedOrderPostedDate <= DateTime.Today));
            result[0].Should().BeEquivalentTo(ResourceHelper.GetEnforcementOrderSummaryView(order.Id));
        }
    }

    [Test]
    public async Task ListDraftEnforcementOrders_ReturnsCorrectly()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListDraftEnforcementOrdersAsync();

        using (new AssertionScope())
        {
            result.Count.Should().Be(EnforcementOrderData.EnforcementOrders
                .Count(e => !e.Deleted && e.PublicationStatus == EnforcementOrder.PublicationState.Draft));
            result[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderAdminSummaryView(EnforcementOrderData.EnforcementOrders
                    .OrderBy(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName)
                    .First(e => !e.Deleted && e.PublicationStatus == EnforcementOrder.PublicationState.Draft).Id));
        }
    }

    [Test]
    public async Task ListPendingEnforcementOrders_ReturnsCorrectly()
    {
        var order = EnforcementOrderData.EnforcementOrders
            .AsQueryable().FilterForPending().ApplySorting(OrderSorting.DateAsc).First();

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListPendingEnforcementOrdersAsync();

        using (new AssertionScope())
        {
            result.Count.Should().Be(EnforcementOrderData.EnforcementOrders.AsQueryable().FilterForPending().Count());
            result[0].Should().BeEquivalentTo(ResourceHelper.GetEnforcementOrderAdminSummaryView(order.Id));
        }
    }
}
