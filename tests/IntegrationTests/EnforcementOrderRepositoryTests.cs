using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.Pagination;
using Enfo.Domain.Utils;
using EnfoTests.Infrastructure.Helpers;
using EnfoTests.TestData;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnfoTests.Infrastructure;

[TestFixture]
public class EnforcementOrderRepositoryTests
{
    // GetAsync

    [Test]
    public async Task Get_WhenItemExistsAndIsPublic_ReturnsItem()
    {
        var item = EnforcementOrderData.EnforcementOrders.First(e => e.GetIsPublic);
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();

        var result = await repository.GetAsync(item.Id);

        var expected = ResourceHelper.GetEnforcementOrderDetailedView(item.Id);
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Get_WhenNotExists_ReturnsNull()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.GetAsync(-1)).Should().BeNull();
    }

    [Test]
    public async Task Get_WhenItemExistsButIsNotPublic_ReturnsNull()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic).Id;
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();

        var result = await repository.GetAsync(itemId);

        result.Should().BeNull();
    }

    // GetAdminViewAsync

    [Test]
    public async Task GetAdminView_WhenItemExistsAndIsPublic_ReturnsItem()
    {
        var item = EnforcementOrderData.EnforcementOrders.First(e => e.GetIsPublic);
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();

        var result = await repository.GetAdminViewAsync(item.Id);

        var expected = ResourceHelper.GetEnforcementOrderAdminView(item.Id);
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetAdminView_WhenNotExists_ReturnsNull()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.GetAdminViewAsync(-1)).Should().BeNull();
    }

    [Test]
    public async Task GetAdminView_WhenItemExistsButIsNotPublic_ReturnsItem()
    {
        var item = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic);
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();

        var result = await repository.GetAdminViewAsync(item.Id);

        var expected = ResourceHelper.GetEnforcementOrderAdminView(item.Id);
        result.Should().BeEquivalentTo(expected);
    }

    // ListAsync

    [Test]
    public async Task List_ByDefault_ReturnsOnlyPublic()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();

        var result = await repository.ListAsync(new EnforcementOrderSpec(), new PaginationSpec(1, 20));

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(EnforcementOrderData.EnforcementOrders.Count(e => e.GetIsPublic));
            result.Items.Should()
                .HaveCount(EnforcementOrderData.EnforcementOrders.Count(e => e.GetIsPublic));
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderSummaryView(EnforcementOrderData.EnforcementOrders
                    .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName)
                    .First(e => e.GetIsPublic).Id));
        });
    }

    [Test]
    public async Task List_WithFacilityNameSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).FacilityName,
        };

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAsync(spec, new PaginationSpec(1, 20));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
            .ThenBy(e => e.FacilityName)
            .Where(e =>
                !e.Deleted
                && string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .ToList();

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(expectedList.Count);
            result.Items.Should().HaveCount(expectedList.Count);
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderSummaryView(expectedList[0].Id));
        });
    }

    [Test]
    public async Task List_WithDateRangeBetweenProposedAndExecutedDates_ReturnsNone()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            FromDate = new DateTime(999, 3, 1),
            TillDate = new DateTime(999, 4, 1),
        };

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAsync(spec, new PaginationSpec(1, 20));

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(0);
            result.Items.Count.Should().Be(0);
            result.PageNumber.Should().Be(1);
        });
    }

    [Test]
    public async Task List_WithStartDateBeforeFacilityDates_ReturnsFacility()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            FromDate = new DateTime(999, 1, 1),
        };

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAsync(spec, new PaginationSpec(1, 20));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(expectedList.Count);
            result.PageNumber.Should().Be(1);

            var resultList = result.Items.ToList();
            resultList.Should().HaveCount(expectedList.Count);
            resultList.Should().BeEquivalentTo(expectedList);
        });
    }

    [Test]
    public async Task List_WithEndDateAfterFacilityDates_ReturnsFacility()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            TillDate = new DateTime(999, 6, 1),
        };

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAsync(spec, new PaginationSpec(1, 20));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(expectedList.Count);
            result.PageNumber.Should().Be(1);

            var resultList = result.Items.ToList();
            resultList.Should().HaveCount(expectedList.Count);
            resultList.Should().BeEquivalentTo(expectedList);
        });
    }

    // ListDetailedAsync

    [Test]
    public async Task ListDetailed_ByDefault_ReturnsOnlyPublic()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListDetailedAsync(new EnforcementOrderSpec(), new PaginationSpec(1, 20));

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(EnforcementOrderData.EnforcementOrders.Count(e => e.GetIsPublic));
            result.Items.Should()
                .HaveCount(EnforcementOrderData.EnforcementOrders.Count(e => e.GetIsPublic));
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderDetailedView(EnforcementOrderData.EnforcementOrders
                    .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName)
                    .First(e => e.GetIsPublic).Id));
        });
    }

    [Test]
    public async Task ListDetailed_WithFacilityNameSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderSpec
            { Facility = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).FacilityName };

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListDetailedAsync(spec, new PaginationSpec(1, 20));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
            .ThenBy(e => e.FacilityName)
            .Where(e =>
                !e.Deleted
                && string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .ToList();

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(expectedList.Count);
            result.Items.Should().HaveCount(expectedList.Count);
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderDetailedView(expectedList[0].Id));
        });
    }

    [Test]
    public async Task ListDetailed_WithDateRangeBetweenProposedAndExecutedDates_ReturnsNone()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            FromDate = new DateTime(999, 3, 1),
            TillDate = new DateTime(999, 4, 1),
        };

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListDetailedAsync(spec, new PaginationSpec(1, 20));

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(0);
            result.Items.Should().HaveCount(0);
            result.PageNumber.Should().Be(1);
        });
    }

    [Test]
    public async Task ListDetailed_WithStartDateBeforeFacilityDates_ReturnsFacility()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            FromDate = new DateTime(999, 1, 1),
        };

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListDetailedAsync(spec, new PaginationSpec(1, 20));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .ToList();

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(expectedList.Count);
            result.Items.Should().HaveCount(expectedList.Count);
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderDetailedView(expectedList[0].Id));
        });
    }

    [Test]
    public async Task ListDetailed_WithEndDateAfterFacilityDates_ReturnsFacility()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            TillDate = new DateTime(999, 6, 1),
        };

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListDetailedAsync(spec, new PaginationSpec(1, 20));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .ToList();

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(expectedList.Count);
            result.Items.Should();
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderDetailedView(expectedList[0].Id));
        });
    }

    // ListAdminAsync

    [Test]
    public async Task ListAdmin_ByDefault_ReturnsNonDeleted()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAdminAsync(new EnforcementOrderAdminSpec(), new PaginationSpec(1, 20));

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(EnforcementOrderData.EnforcementOrders.Count(e => !e.Deleted));
            result.Items.Should()
                .HaveCount(EnforcementOrderData.EnforcementOrders.Count(e => !e.Deleted));
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderAdminSummaryView(EnforcementOrderData.EnforcementOrders
                    .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName)
                    .First(e => !e.Deleted).Id));
        });
    }

    [Test]
    public async Task ListAdmin_WithSpecShowDeleted_ReturnsOnlyDeleted()
    {
        var spec = new EnforcementOrderAdminSpec { ShowDeleted = true };

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAdminAsync(spec, new PaginationSpec(1, 20));

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(EnforcementOrderData.EnforcementOrders.Count(e => e.Deleted));
            result.Items.Should()
                .HaveCount(EnforcementOrderData.EnforcementOrders.Count(e => e.Deleted));
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderAdminSummaryView(EnforcementOrderData.EnforcementOrders
                    .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName)
                    .First(e => e.Deleted).Id));
        });
    }

    [Test]
    public async Task ListAdmin_WithFacilityNameSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderAdminSpec
        {
            Facility = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).FacilityName,
        };

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAdminAsync(spec, new PaginationSpec(1, 20));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
            .ThenBy(e => e.FacilityName)
            .Where(e => !e.Deleted
                && string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .ToList();

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(expectedList.Count);
            result.Items.Should().HaveCount(expectedList.Count);
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderAdminSummaryView(expectedList[0].Id));
        });
    }

    [Test]
    public async Task ListAdmin_WithUnmatchedFacilityNameSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderAdminSpec
        {
            Facility = "none",
        };

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAdminAsync(spec, new PaginationSpec(1, 20));

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(0);
            result.Items.Count.Should().Be(0);
            result.PageNumber.Should().Be(1);
        });
    }

    [Test]
    public async Task ListAdmin_WithTextMatchSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderAdminSpec
        {
            Text = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).Cause[..4].ToLowerInvariant(),
        };

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAdminAsync(spec, new PaginationSpec(1, 20));

        var expectedList = EnforcementOrderData.EnforcementOrders
            .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
            .ThenBy(e => e.FacilityName)
            .Where(e => !e.Deleted)
            .Where(e => e.Cause != null && e.Cause.Contains(spec.Text) ||
                e.Requirements != null && e.Requirements.Contains(spec.Text))
            .ToList();

        Assert.Multiple(() =>
        {
            result.CurrentCount.Should().Be(expectedList.Count);
            result.Items.Should().HaveCount(expectedList.Count);
            result.PageNumber.Should().Be(1);
            result.Items[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderAdminSummaryView(expectedList[0].Id));
        });
    }

    // ExistsAsync

    [Test]
    public async Task Exists_GivenExists_ReturnsTrue()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => e.GetIsPublic).Id;
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.ExistsAsync(itemId)).Should().BeTrue();
    }

    [Test]
    public async Task Exists_GivenNotExists_ReturnsFalse()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.ExistsAsync(-1)).Should().BeFalse();
    }

    [Test]
    public async Task Exists_GivenNonpublic_ReturnsFalse()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic).Id;
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.ExistsAsync(itemId)).Should().BeFalse();
    }

    [Test]
    public async Task Exists_GivenNonpublicButAllowed_ReturnsTrue()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => !e.GetIsPublic).Id;
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.ExistsAsync(itemId, false)).Should().BeTrue();
    }

    // OrderNumberExists

    [Test]
    public async Task OrderNumberExists_GivenExists_ReturnsTrue()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.OrderNumberExistsAsync(EnforcementOrderData.EnforcementOrders.First().OrderNumber))
            .Should().BeTrue();
    }

    [Test]
    public async Task OrderNumberExists_GivenNotExists_ReturnsFalse()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.OrderNumberExistsAsync(Guid.NewGuid().ToString()))
            .Should().BeFalse();
    }

    [Test]
    public async Task OrderNumberExists_GivenExistsAndIgnore_ReturnsFalse()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.OrderNumberExistsAsync(EnforcementOrderData.EnforcementOrders.First().OrderNumber,
                EnforcementOrderData.EnforcementOrders.First().Id))
            .Should().BeFalse();
    }

    // ListCurrentProposedEnforcementOrders

    [Test]
    public async Task ListCurrentProposedEnforcementOrders_ReturnsCorrectly()
    {
        var order = EnforcementOrderData.EnforcementOrders.First(e => e.GetIsPublic && e.IsProposedOrder);
        order.CommentPeriodClosesDate = DateTime.Today.AddDays(1);

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListCurrentProposedEnforcementOrdersAsync();

        Assert.Multiple(() =>
        {
            result.Count.Should().Be(EnforcementOrderData.EnforcementOrders.Count(e =>
                e.GetIsPublic && e.IsProposedOrder && e.CommentPeriodClosesDate >= DateTime.Today));
            result[0].Should().BeEquivalentTo(ResourceHelper.GetEnforcementOrderSummaryView(order.Id));
        });
    }

    // ListRecentlyExecutedEnforcementOrders

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

        Assert.Multiple(() =>
        {
            result.Count.Should().Be(EnforcementOrderData.EnforcementOrders
                .Count(e => e.GetIsPublicExecutedOrder
                    && e.ExecutedOrderPostedDate >= DateUtils.MostRecentMonday()
                    && e.ExecutedOrderPostedDate <= DateTime.Today));
            result[0].Should().BeEquivalentTo(ResourceHelper.GetEnforcementOrderSummaryView(order.Id));
        });
    }

    // ListDraftEnforcementOrders

    [Test]
    public async Task ListDraftEnforcementOrders_ReturnsCorrectly()
    {
        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListDraftEnforcementOrdersAsync();

        Assert.Multiple(() =>
        {
            result.Count.Should().Be(EnforcementOrderData.EnforcementOrders
                .Count(e => !e.Deleted && e.PublicationStatus == EnforcementOrder.PublicationState.Draft));
            result[0].Should().BeEquivalentTo(
                ResourceHelper.GetEnforcementOrderAdminSummaryView(EnforcementOrderData.EnforcementOrders
                    .OrderBy(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName)
                    .First(e => !e.Deleted && e.PublicationStatus == EnforcementOrder.PublicationState.Draft).Id));
        });
    }

    // ListPendingEnforcementOrders

    [Test]
    public async Task ListPendingEnforcementOrders_ReturnsCorrectly()
    {
        var order = EnforcementOrderData.EnforcementOrders
            .AsQueryable().FilterForPending().ApplySorting(OrderSorting.DateAsc).First();

        using var repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListPendingEnforcementOrdersAsync();

        Assert.Multiple(() =>
        {
            result.Count.Should().Be(EnforcementOrderData.EnforcementOrders.AsQueryable().FilterForPending().Count());
            result[0].Should().BeEquivalentTo(ResourceHelper.GetEnforcementOrderAdminSummaryView(order.Id));
        });
    }

    // Write Methods

    // CreateAsync

    [Test]
    public async Task CreateOrder_AddsNewItem()
    {
        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        EnforcementOrderCreate sampleCreate = new()
        {
            Cause = "Cause of order",
            Requirements = "Requirements of order",
            FacilityName = "Facility 4",
            County = "Fulton",
            LegalAuthorityId = LegalAuthorityData.LegalAuthorities.First().Id,
            Progress = PublicationProgress.Draft,
            OrderNumber = "NEW-4",
            CreateAs = NewEnforcementOrderType.Proposed,
            CommentPeriodClosesDate = DateTime.Today.AddDays(1),
            CommentContactId = EpdContactData.EpdContacts.First().Id,
            ProposedOrderPostedDate = DateTime.Today,
        };
        var newId = await repository.CreateAsync(sampleCreate);
        repositoryHelper.ClearChangeTracker();

        var order = new EnforcementOrder(sampleCreate) { Id = newId };
        var expected = new EnforcementOrderAdminView(ResourceHelper.FillNavigationProperties(order));

        var item = await repository.GetAdminViewAsync(newId);
        item.Should().BeEquivalentTo(expected);
    }

    // UpdateAsync

    // Sample data for update
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

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be("OrderNumber");
    }

    // DeleteAsync

    [Test]
    public async Task Delete_Succeeds()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).Id;

        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        await repository.DeleteAsync(itemId);
        repositoryHelper.ClearChangeTracker();

        (await repository.GetAdminViewAsync(itemId))
            .Deleted.Should().BeTrue();
    }

    [Test]
    public async Task Delete_AlreadyDeletedItem_DoesNotChange()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => e.Deleted).Id;

        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        await repository.DeleteAsync(itemId);
        repositoryHelper.ClearChangeTracker();

        (await repository.GetAdminViewAsync(itemId))
            .Deleted.Should().BeTrue();
    }

    // RestoreAsync

    [Test]
    public async Task Restore_Succeeds()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => e.Deleted).Id;

        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        await repository.RestoreAsync(itemId);
        repositoryHelper.ClearChangeTracker();

        (await repository.GetAdminViewAsync(itemId))
            .Deleted.Should().BeFalse();
    }

    [Test]
    public async Task Restore_AlreadyDeletedItem_DoesNotChange()
    {
        var itemId = EnforcementOrderData.EnforcementOrders.First(e => !e.Deleted).Id;

        using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        await repository.RestoreAsync(itemId);
        repositoryHelper.ClearChangeTracker();

        (await repository.GetAdminViewAsync(itemId))
            .Deleted.Should().BeFalse();
    }
}
