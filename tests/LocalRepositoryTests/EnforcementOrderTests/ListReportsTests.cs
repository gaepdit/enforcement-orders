using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.Services;
using Enfo.LocalRepository.Repositories;
using EnfoTests.TestData;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using static Enfo.Domain.Utils.DateUtils;

namespace EnfoTests.LocalRepositoryTests.EnforcementOrderTests;

[TestFixture]
public class ListReportsTests
{
    [Test]
    public async Task ListCurrentProposedEnforcementOrders_ReturnsCorrectList()
    {
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());

        var result = await repository.ListCurrentProposedEnforcementOrdersAsync();

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => e.GetIsPublic)
            .Where(e => e.IsProposedOrder)
            .Where(e => e.CommentPeriodClosesDate >= DateTime.Today)
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        result.Should().BeEquivalentTo(expectedList);
    }

    [Test]
    public async Task ListRecentlyExecutedEnforcementOrders_ReturnsCorrectList()
    {
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());

        var result = await repository.ListRecentlyExecutedEnforcementOrdersAsync();

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => e.GetIsPublicExecutedOrder)
            .Where(e => e.ExecutedOrderPostedDate >= MostRecentMonday())
            .Where(e => e.ExecutedOrderPostedDate <= DateTime.Today)
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        result.Should().BeEquivalentTo(expectedList);
    }

    [Test]
    public async Task ListDraftEnforcementOrders_ReturnsCorrectList()
    {
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());

        var result = await repository.ListDraftEnforcementOrdersAsync();

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => !e.Deleted)
            .Where(e => e.PublicationStatus == EnforcementOrder.PublicationState.Draft)
            .Select(e => new EnforcementOrderAdminSummaryView(e))
            .ToList();

        result.Should().BeEquivalentTo(expectedList);
    }

    [Test]
    public async Task ListPendingEnforcementOrders_ReturnsCorrectList()
    {
        using var repository = new LocalEnforcementOrderRepository(Substitute.For<IAttachmentStore>());

        var result = await repository.ListPendingEnforcementOrdersAsync();

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => e.PublicationStatus == EnforcementOrder.PublicationState.Published)
            .Where(e => e.GetLastPostedDate > MostRecentMonday())
            .Select(e => new EnforcementOrderAdminSummaryView(e))
            .ToList();

        result.Should().BeEquivalentTo(expectedList);
    }
}
