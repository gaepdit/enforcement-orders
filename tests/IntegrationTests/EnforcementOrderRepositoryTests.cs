using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.Pagination;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Enfo.Domain.Utils.DateUtils;
using static EnfoTests.Helpers.DataHelper;
using static EnfoTests.Helpers.RepositoryHelper;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.Infrastructure;

public class EnforcementOrderRepositoryTests
{
    // GetAsync

    [Fact]
    public async Task Get_WhenItemExistsAndIsPublic_ReturnsItem()
    {
        var item = GetEnforcementOrders.First(e => e.GetIsPublic);
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();

        var result = await repository.GetAsync(item.Id);

        var expected = GetEnforcementOrderDetailedView(item.Id);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Get_WhenNotExists_ReturnsNull()
    {
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.GetAsync(-1)).Should().BeNull();
    }

    [Fact]
    public async Task Get_WhenItemExistsButIsNotPublic_ReturnsNull()
    {
        var itemId = GetEnforcementOrders.First(e => !e.GetIsPublic).Id;
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();

        var result = await repository.GetAsync(itemId);

        result.Should().BeNull();
    }

    // GetAdminViewAsync

    [Fact]
    public async Task GetAdminView_WhenItemExistsAndIsPublic_ReturnsItem()
    {
        var item = GetEnforcementOrders.First(e => e.GetIsPublic);
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();

        var result = await repository.GetAdminViewAsync(item.Id);

        var expected = GetEnforcementOrderAdminView(item.Id);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetAdminView_WhenNotExists_ReturnsNull()
    {
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.GetAdminViewAsync(-1)).Should().BeNull();
    }

    [Fact]
    public async Task GetAdminView_WhenItemExistsButIsNotPublic_ReturnsItem()
    {
        var item = GetEnforcementOrders.First(e => !e.GetIsPublic);
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();

        var result = await repository.GetAdminViewAsync(item.Id);

        var expected = GetEnforcementOrderAdminView(item.Id);
        result.Should().BeEquivalentTo(expected);
    }

    // ListAsync

    [Fact]
    public async Task List_ByDefault_ReturnsOnlyPublic()
    {
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();

        var result = await repository.ListAsync(new EnforcementOrderSpec(), new PaginationSpec(1, 20));

        result.CurrentCount.Should().Be(GetEnforcementOrders.Count(e => e.GetIsPublic));
        result.Items.Cast<EnforcementOrderSummaryView>().Should()
            .HaveCount(GetEnforcementOrders.Count(e => e.GetIsPublic));
        result.PageNumber.Should().Be(1);
        result.Items[0].Should().BeEquivalentTo(
            GetEnforcementOrderSummaryView(GetEnforcementOrders
                .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                .ThenBy(e => e.FacilityName)
                .First(e => e.GetIsPublic).Id));
    }

    [Fact]
    public async Task List_WithFacilityNameSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = GetEnforcementOrders.First(e => !e.Deleted).FacilityName,
        };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAsync(spec, new PaginationSpec(1, 20));

        var expectedList = GetEnforcementOrders
            .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
            .ThenBy(e => e.FacilityName)
            .Where(e =>
                !e.Deleted
                && string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .ToList();

        result.CurrentCount.Should().Be(expectedList.Count);
        result.Items.Cast<EnforcementOrderSummaryView>().Should().HaveCount(expectedList.Count);
        result.PageNumber.Should().Be(1);
        result.Items[0].Should().BeEquivalentTo(
            GetEnforcementOrderSummaryView(expectedList[0].Id));
    }

    [Fact]
    public async Task List_WithDateRangeBetweenProposedAndExecutedDates_ReturnsNone()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            FromDate = new DateTime(2021, 3, 1),
            TillDate = new DateTime(2021, 4, 1),
        };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAsync(spec, new PaginationSpec(1, 20));

        result.CurrentCount.Should().Be(0);
        result.Items.Count.Should().Be(0);
        result.PageNumber.Should().Be(1);
    }

    [Fact]
    public async Task List_WithStartDateBeforeFacilityDates_ReturnsFacility()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            FromDate = new DateTime(2021, 1, 1),
        };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAsync(spec, new PaginationSpec(1, 20));

        var expectedList = GetEnforcementOrders
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        result.CurrentCount.Should().Be(expectedList.Count);
        result.PageNumber.Should().Be(1);

        var resultList = result.Items.Cast<EnforcementOrderSummaryView>().ToList();
        resultList.Should().HaveCount(expectedList.Count);
        resultList.Should().BeEquivalentTo(expectedList);
    }

    [Fact]
    public async Task List_WithEndDateAfterFacilityDates_ReturnsFacility()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            TillDate = new DateTime(2021, 6, 1),
        };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAsync(spec, new PaginationSpec(1, 20));

        var expectedList = GetEnforcementOrders
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        result.CurrentCount.Should().Be(expectedList.Count);
        result.PageNumber.Should().Be(1);

        var resultList = result.Items.Cast<EnforcementOrderSummaryView>().ToList();
        resultList.Should().HaveCount(expectedList.Count);
        resultList.Should().BeEquivalentTo(expectedList);
    }

    // ListDetailedAsync

    [Fact]
    public async Task ListDetailed_ByDefault_ReturnsOnlyPublic()
    {
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListDetailedAsync(new EnforcementOrderSpec(), new PaginationSpec(1, 20));

        result.CurrentCount.Should().Be(GetEnforcementOrders.Count(e => e.GetIsPublic));
        result.Items.Cast<EnforcementOrderDetailedView>().Should()
            .HaveCount(GetEnforcementOrders.Count(e => e.GetIsPublic));
        result.PageNumber.Should().Be(1);
        result.Items[0].Should().BeEquivalentTo(
            GetEnforcementOrderDetailedView(GetEnforcementOrders
                .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                .ThenBy(e => e.FacilityName)
                .First(e => e.GetIsPublic).Id));
    }

    [Fact]
    public async Task ListDetailed_WithFacilityNameSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = GetEnforcementOrders.First(e => !e.Deleted).FacilityName
        };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListDetailedAsync(spec, new PaginationSpec(1, 20));

        var expectedList = GetEnforcementOrders
            .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
            .ThenBy(e => e.FacilityName)
            .Where(e =>
                !e.Deleted
                && string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .ToList();

        result.CurrentCount.Should().Be(expectedList.Count);
        result.Items.Cast<EnforcementOrderDetailedView>().Should().HaveCount(expectedList.Count);
        result.PageNumber.Should().Be(1);
        result.Items[0].Should().BeEquivalentTo(
            GetEnforcementOrderDetailedView(expectedList[0].Id));
    }

    [Fact]
    public async Task ListDetailed_WithDateRangeBetweenProposedAndExecutedDates_ReturnsNone()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            FromDate = new DateTime(2021, 3, 1),
            TillDate = new DateTime(2021, 4, 1),
        };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListDetailedAsync(spec, new PaginationSpec(1, 20));

        result.CurrentCount.Should().Be(0);
        result.Items.Cast<EnforcementOrderDetailedView>().Should().HaveCount(0);
        result.PageNumber.Should().Be(1);
    }

    [Fact]
    public async Task ListDetailed_WithStartDateBeforeFacilityDates_ReturnsFacility()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            FromDate = new DateTime(2021, 1, 1),
        };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListDetailedAsync(spec, new PaginationSpec(1, 20));

        var expectedList = GetEnforcementOrders
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .ToList();

        result.CurrentCount.Should().Be(expectedList.Count);
        result.Items.Cast<EnforcementOrderDetailedView>().Should().HaveCount(expectedList.Count);
        result.PageNumber.Should().Be(1);
        result.Items[0].Should().BeEquivalentTo(
            GetEnforcementOrderDetailedView(expectedList[0].Id));
    }

    [Fact]
    public async Task ListDetailed_WithEndDateAfterFacilityDates_ReturnsFacility()
    {
        var spec = new EnforcementOrderSpec
        {
            Facility = "Date Range Test",
            TillDate = new DateTime(2021, 6, 1),
        };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListDetailedAsync(spec, new PaginationSpec(1, 20));

        var expectedList = GetEnforcementOrders
            .Where(e => string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .ToList();

        result.CurrentCount.Should().Be(expectedList.Count);
        result.Items.Cast<EnforcementOrderDetailedView>().Should().HaveCount(expectedList.Count);
        result.PageNumber.Should().Be(1);
        result.Items[0].Should().BeEquivalentTo(
            GetEnforcementOrderDetailedView(expectedList[0].Id));
    }

    // ListAdminAsync

    [Fact]
    public async Task ListAdmin_ByDefault_ReturnsNonDeleted()
    {
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAdminAsync(new EnforcementOrderAdminSpec(), new PaginationSpec(1, 20));

        result.CurrentCount.Should().Be(GetEnforcementOrders.Count(e => !e.Deleted));
        result.Items.Cast<EnforcementOrderAdminSummaryView>().Should()
            .HaveCount(GetEnforcementOrders.Count(e => !e.Deleted));
        result.PageNumber.Should().Be(1);
        result.Items[0].Should().BeEquivalentTo(
            GetEnforcementOrderAdminSummaryView(GetEnforcementOrders
                .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                .ThenBy(e => e.FacilityName)
                .First(e => !e.Deleted).Id));
    }

    [Fact]
    public async Task ListAdmin_WithSpecShowDeleted_ReturnsOnlyDeleted()
    {
        var spec = new EnforcementOrderAdminSpec { ShowDeleted = true };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAdminAsync(spec, new PaginationSpec(1, 20));

        result.CurrentCount.Should().Be(GetEnforcementOrders.Count(e => e.Deleted));
        result.Items.Cast<EnforcementOrderAdminSummaryView>().Should()
            .HaveCount(GetEnforcementOrders.Count(e => e.Deleted));
        result.PageNumber.Should().Be(1);
        result.Items[0].Should().BeEquivalentTo(
            GetEnforcementOrderAdminSummaryView(GetEnforcementOrders
                .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                .ThenBy(e => e.FacilityName)
                .First(e => e.Deleted).Id));
    }

    [Fact]
    public async Task ListAdmin_WithFacilityNameSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderAdminSpec
        {
            Facility = GetEnforcementOrders.First(e => !e.Deleted).FacilityName,
        };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAdminAsync(spec, new PaginationSpec(1, 20));

        var expectedList = GetEnforcementOrders
            .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
            .ThenBy(e => e.FacilityName)
            .Where(e => !e.Deleted
                && string.Equals(e.FacilityName, spec.Facility, StringComparison.Ordinal))
            .ToList();

        result.CurrentCount.Should().Be(expectedList.Count);
        result.Items.Cast<EnforcementOrderAdminSummaryView>().Should().HaveCount(expectedList.Count);
        result.PageNumber.Should().Be(1);
        result.Items[0].Should().BeEquivalentTo(
            GetEnforcementOrderAdminSummaryView(expectedList[0].Id));
    }

    [Fact]
    public async Task ListAdmin_WithUnmatchedFacilityNameSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderAdminSpec
        {
            Facility = "none",
        };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAdminAsync(spec, new PaginationSpec(1, 20));

        result.CurrentCount.Should().Be(0);
        result.Items.Count.Should().Be(0);
        result.PageNumber.Should().Be(1);
    }

    [Fact]
    public async Task ListAdmin_WithTextMatchSpec_ReturnsMatches()
    {
        var spec = new EnforcementOrderAdminSpec
        {
            Text = GetEnforcementOrders.First(e => !e.Deleted).Cause[..4].ToLowerInvariant(),
        };

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListAdminAsync(spec, new PaginationSpec(1, 20));

        var expectedList = GetEnforcementOrders
            .OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
            .ThenBy(e => e.FacilityName)
            .Where(e => !e.Deleted)
            .Where(e => e.Cause != null && e.Cause.Contains(spec.Text) ||
                e.Requirements != null && e.Requirements.Contains(spec.Text))
            .ToList();

        result.CurrentCount.Should().Be(expectedList.Count);
        result.Items.Cast<EnforcementOrderAdminSummaryView>().Should().HaveCount(expectedList.Count);
        result.PageNumber.Should().Be(1);
        result.Items[0].Should().BeEquivalentTo(
            GetEnforcementOrderAdminSummaryView(expectedList[0].Id));
    }

    // ExistsAsync

    [Fact]
    public async Task Exists_GivenExists_ReturnsTrue()
    {
        var itemId = GetEnforcementOrders.First(e => e.GetIsPublic).Id;
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.ExistsAsync(itemId)).Should().BeTrue();
    }

    [Fact]
    public async Task Exists_GivenNotExists_ReturnsFalse()
    {
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.ExistsAsync(-1)).Should().BeFalse();
    }

    [Fact]
    public async Task Exists_GivenNonpublic_ReturnsFalse()
    {
        var itemId = GetEnforcementOrders.First(e => !e.GetIsPublic).Id;
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.ExistsAsync(itemId)).Should().BeFalse();
    }

    [Fact]
    public async Task Exists_GivenNonpublicButAllowed_ReturnsTrue()
    {
        var itemId = GetEnforcementOrders.First(e => !e.GetIsPublic).Id;
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.ExistsAsync(itemId, false)).Should().BeTrue();
    }

    // OrderNumberExists

    [Fact]
    public async Task OrderNumberExists_GivenExists_ReturnsTrue()
    {
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.OrderNumberExistsAsync(GetEnforcementOrders.First().OrderNumber))
            .Should().BeTrue();
    }

    [Fact]
    public async Task OrderNumberExists_GivenNotExists_ReturnsFalse()
    {
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.OrderNumberExistsAsync(Guid.NewGuid().ToString()))
            .Should().BeFalse();
    }

    [Fact]
    public async Task OrderNumberExists_GivenExistsAndIgnore_ReturnsFalse()
    {
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        (await repository.OrderNumberExistsAsync(GetEnforcementOrders.First().OrderNumber,
                GetEnforcementOrders.First().Id))
            .Should().BeFalse();
    }

    // ListCurrentProposedEnforcementOrders

    [Fact]
    public async Task ListCurrentProposedEnforcementOrders_ReturnsCorrectly()
    {
        var order = GetEnforcementOrders.First(e => e.GetIsPublic && e.IsProposedOrder);
        order.CommentPeriodClosesDate = DateTime.Today.AddDays(1);

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListCurrentProposedEnforcementOrdersAsync();

        result.Count.Should().Be(GetEnforcementOrders.Count(e =>
            e.GetIsPublic && e.IsProposedOrder && e.CommentPeriodClosesDate >= DateTime.Today));
        result[0].Should().BeEquivalentTo(GetEnforcementOrderSummaryView(order.Id));
    }
    
    // ListRecentlyExecutedEnforcementOrders

    [Fact]
    public async Task ListRecentlyExecutedEnforcementOrders_ReturnsCorrectly()
    {
        var order = GetEnforcementOrders.First(e => e.GetIsPublicExecutedOrder);
        order.ExecutedOrderPostedDate = MostRecentMonday();

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListRecentlyExecutedEnforcementOrdersAsync();

        result.Count.Should().Be(
            GetEnforcementOrders.Count(e =>
                e.GetIsPublicExecutedOrder
                && e.ExecutedOrderPostedDate >= MostRecentMonday()
                && e.ExecutedOrderPostedDate <= DateTime.Today));
        result[0].Should().BeEquivalentTo(GetEnforcementOrderSummaryView(order.Id));
    }

    // ListDraftEnforcementOrders

    [Fact]
    public async Task ListDraftEnforcementOrders_ReturnsCorrectly()
    {
        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListDraftEnforcementOrdersAsync();

        result.Count.Should().Be(GetEnforcementOrders.Count(e =>
            !e.Deleted && e.PublicationStatus == EnforcementOrder.PublicationState.Draft));
        result[0].Should().BeEquivalentTo(
            GetEnforcementOrderAdminSummaryView(GetEnforcementOrders.First(e =>
                !e.Deleted && e.PublicationStatus == EnforcementOrder.PublicationState.Draft).Id));
    }

    // ListPendingEnforcementOrders

    [Fact]
    public async Task ListPendingEnforcementOrders_ReturnsCorrectly()
    {
        var order = GetEnforcementOrders.First(e => e.GetIsPublicProposedOrder || e.GetIsPublicExecutedOrder);
        order.ExecutedDate = DateTime.Today.AddDays(8);

        using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
        var result = await repository.ListPendingEnforcementOrdersAsync();

        result.Count.Should().Be(GetEnforcementOrders.Count(e =>
            e.PublicationStatus == EnforcementOrder.PublicationState.Published
            && e.GetLastPostedDate > MostRecentMonday()));
        result[0].Should().BeEquivalentTo(GetEnforcementOrderAdminSummaryView(order.Id));
    }
    
    // Write Methods

    // CreateAsync

    // Sample data for create
    private readonly EnforcementOrderCreate _sampleCreate = new()
    {
        Cause = "Cause of order",
        Requirements = "Requirements of order",
        FacilityName = "Facility 4",
        County = "Fulton",
        LegalAuthorityId = GetLegalAuthorities.First().Id,
        Progress = PublicationProgress.Draft,
        OrderNumber = "NEW-4",
        CreateAs = NewEnforcementOrderType.Proposed,
        CommentPeriodClosesDate = DateTime.Today.AddDays(1),
        CommentContactId = GetEpdContacts.First().Id,
        ProposedOrderPostedDate = DateTime.Today,
    };

    [Fact]
    public async Task CreateOrder_AddsNewItem()
    {
        using var repositoryHelper = CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        var newId = await repository.CreateAsync(_sampleCreate);
        repositoryHelper.ClearChangeTracker();

        var order = new EnforcementOrder(_sampleCreate) { Id = newId };
        var expected = new EnforcementOrderAdminView(FillNavigationProperties(order));

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

    [Fact]
    public async Task Update_Succeeds()
    {
        var existingOrder = GetEnforcementOrders.First(e => !e.Deleted);
        var itemId = existingOrder.Id;

        var itemUpdate = NewSampleUpdate(existingOrder);
        itemUpdate.Cause = "abc";

        using var repositoryHelper = CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        await repository.UpdateAsync(itemUpdate);
        repositoryHelper.ClearChangeTracker();

        (await repository.GetAsync(itemId)).Cause.Should().Be("abc");
    }

    [Fact]
    public async Task Update_DeletedOrder_ThrowsException()
    {
        var existingOrder = GetEnforcementOrders.First(e => e.Deleted);
        var itemUpdate = NewSampleUpdate(existingOrder);

        var action = async () =>
        {
            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            await repository.UpdateAsync(itemUpdate);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage("A deleted Enforcement Order cannot be modified. (Parameter 'resource')")
            .And.ParamName.Should().Be("resource");
    }

    [Fact]
    public async Task Update_WithDuplicateOrderNumber_Fails()
    {
        var existingOrder = GetEnforcementOrders.First(e => !e.Deleted);

        var itemUpdate = NewSampleUpdate(existingOrder);
        itemUpdate.OrderNumber = GetEnforcementOrders.Last(e => !e.Deleted).OrderNumber;

        var action = async () =>
        {
            using var repository = CreateRepositoryHelper().GetEnforcementOrderRepository();
            await repository.UpdateAsync(itemUpdate);
        };

        (await action.Should().ThrowAsync<ArgumentException>())
            .And.ParamName.Should().Be("OrderNumber");
    }

    // DeleteAsync

    [Fact]
    public async Task Delete_Succeeds()
    {
        var itemId = GetEnforcementOrders.First(e => !e.Deleted).Id;

        using var repositoryHelper = CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        await repository.DeleteAsync(itemId);
        repositoryHelper.ClearChangeTracker();

        (await repository.GetAdminViewAsync(itemId))
            .Deleted.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_AlreadyDeletedItem_DoesNotChange()
    {
        var itemId = GetEnforcementOrders.First(e => e.Deleted).Id;

        using var repositoryHelper = CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        await repository.DeleteAsync(itemId);
        repositoryHelper.ClearChangeTracker();

        (await repository.GetAdminViewAsync(itemId))
            .Deleted.Should().BeTrue();
    }

    // RestoreAsync

    [Fact]
    public async Task Restore_Succeeds()
    {
        var itemId = GetEnforcementOrders.First(e => e.Deleted).Id;

        using var repositoryHelper = CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        await repository.RestoreAsync(itemId);
        repositoryHelper.ClearChangeTracker();

        (await repository.GetAdminViewAsync(itemId))
            .Deleted.Should().BeFalse();
    }

    [Fact]
    public async Task Restore_AlreadyDeletedItem_DoesNotChange()
    {
        var itemId = GetEnforcementOrders.First(e => !e.Deleted).Id;

        using var repositoryHelper = CreateRepositoryHelper();
        using var repository = repositoryHelper.GetEnforcementOrderRepository();

        await repository.RestoreAsync(itemId);
        repositoryHelper.ClearChangeTracker();

        (await repository.GetAdminViewAsync(itemId))
            .Deleted.Should().BeFalse();
    }
}
