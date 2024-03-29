﻿using Enfo.Domain.EnforcementOrders.Entities;
using static Enfo.Domain.Utils.DateUtils;

namespace Enfo.Domain.EnforcementOrders.Specs;

public static class EnforcementOrderFilters
{
    internal static IQueryable<EnforcementOrder> FilterByFacility(
        [NotNull] this IQueryable<EnforcementOrder> query,
        string facilityFilter) =>
        string.IsNullOrWhiteSpace(facilityFilter)
            ? query
            : query.Where(e => e.FacilityName.ToLower().Contains(facilityFilter.ToLower()));

    internal static IQueryable<EnforcementOrder> FilterByCounty(
        [NotNull] this IQueryable<EnforcementOrder> query,
        string county) =>
        string.IsNullOrWhiteSpace(county)
            ? query
            : query.Where(e => e.County.Contains(county));

    internal static IQueryable<EnforcementOrder> FilterByLegalAuth(
        [NotNull] this IQueryable<EnforcementOrder> query,
        int? legalAuthId) =>
        legalAuthId.HasValue
            ? query.Where(e => e.LegalAuthorityId == legalAuthId)
            : query;

    internal static IQueryable<EnforcementOrder> FilterByDateRange(
        [NotNull] this IQueryable<EnforcementOrder> query,
        DateTime? fromDate, DateTime? tillDate, ActivityState status)
    {
        if (fromDate is null && tillDate is null) return query;

        return status switch
        {
            ActivityState.All when tillDate is null =>
                query.Where(e => e.ProposedOrderPostedDate >= fromDate || e.ExecutedDate >= fromDate),
            ActivityState.All when fromDate is null =>
                query.Where(e => e.ProposedOrderPostedDate <= tillDate || e.ExecutedDate <= tillDate),
            ActivityState.All =>
                query.Where(e => e.ProposedOrderPostedDate >= fromDate && e.ProposedOrderPostedDate <= tillDate
                    || e.ExecutedDate >= fromDate && e.ExecutedDate <= tillDate),
            ActivityState.Executed when tillDate is null =>
                query.Where(e => e.ExecutedDate >= fromDate),
            ActivityState.Executed when fromDate is null =>
                query.Where(e => e.ExecutedDate <= tillDate),
            ActivityState.Executed =>
                query.Where(e => e.ExecutedDate >= fromDate && e.ExecutedDate <= tillDate),
            ActivityState.Proposed when tillDate is null =>
                query.Where(e => e.ProposedOrderPostedDate >= fromDate),
            ActivityState.Proposed when fromDate is null =>
                query.Where(e => e.ProposedOrderPostedDate <= tillDate),
            ActivityState.Proposed =>
                query.Where(e => e.ProposedOrderPostedDate >= fromDate && e.ProposedOrderPostedDate <= tillDate),
            _ => query,
        };
    }

    internal static IQueryable<EnforcementOrder> FilterByActivityStatus(
        [NotNull] this IQueryable<EnforcementOrder> query,
        ActivityState status) =>
        status switch
        {
            ActivityState.Executed => query.Where(e => e.IsExecutedOrder),
            ActivityState.Proposed => query.Where(e => e.IsProposedOrder && !e.IsExecutedOrder),
            _ => query,
        };

    internal static IQueryable<EnforcementOrder> FilterForAttachments(
        [NotNull] this IQueryable<EnforcementOrder> query,
        bool withAttachments) =>
        withAttachments switch
        {
            true => query.Where(e => e.Attachments != null && e.Attachments.Any(a => !a.Deleted)),
            false => query,
        };

    internal static IQueryable<EnforcementOrder> FilterByPublicationStatus(
        [NotNull] this IQueryable<EnforcementOrder> query,
        PublicationState status) =>
        status switch
        {
            PublicationState.Draft => query.Where(e =>
                e.PublicationStatus == EnforcementOrder.PublicationState.Draft),
            PublicationState.Published => query.Where(e =>
                e.PublicationStatus == EnforcementOrder.PublicationState.Published),
            _ => query,
        };

    internal static IQueryable<EnforcementOrder> FilterByOrderNumber(
        [NotNull] this IQueryable<EnforcementOrder> query,
        string orderNumber) =>
        string.IsNullOrWhiteSpace(orderNumber)
            ? query
            : query.Where(e => e.OrderNumber.ToLower().Contains(orderNumber.ToLower()));

    internal static IQueryable<EnforcementOrder> FilterByText(
        [NotNull] this IQueryable<EnforcementOrder> query,
        string text) =>
        string.IsNullOrWhiteSpace(text)
            ? query
            : query.Where(e => e.Cause.ToLower().Contains(text.ToLower()) || e.Requirements.ToLower().Contains(text.ToLower()));

    private static IQueryable<EnforcementOrder> FilterForOpenCommentPeriod(
        [NotNull] this IQueryable<EnforcementOrder> query) =>
        query.Where(e => e.CommentPeriodClosesDate >= DateTime.Today);

    public static IQueryable<EnforcementOrder> FilterForOnlyPublic(
        [NotNull] this IQueryable<EnforcementOrder> query) =>
        query.Where(e =>
            !e.Deleted
            && e.PublicationStatus == EnforcementOrder.PublicationState.Published
            && (e.IsExecutedOrder && e.ExecutedOrderPostedDate.HasValue
                && e.ExecutedOrderPostedDate.Value <= DateTime.Today
                || (e.IsProposedOrder && e.ProposedOrderPostedDate.HasValue
                    && e.ProposedOrderPostedDate.Value <= DateTime.Today)));

    private static IQueryable<EnforcementOrder> FilterForPublicProposed(
        [NotNull] this IQueryable<EnforcementOrder> query) =>
        query.Where(e =>
            !e.Deleted
            && e.PublicationStatus == EnforcementOrder.PublicationState.Published
            && e.IsProposedOrder
            && e.ProposedOrderPostedDate.HasValue
            && e.ProposedOrderPostedDate.Value <= DateTime.Today);

    private static IQueryable<EnforcementOrder> FilterForPublicExecuted(
        [NotNull] this IQueryable<EnforcementOrder> query) =>
        query.Where(e =>
            !e.Deleted
            && e.PublicationStatus == EnforcementOrder.PublicationState.Published
            && e.IsExecutedOrder
            && e.ExecutedOrderPostedDate.HasValue
            && e.ExecutedOrderPostedDate.Value <= DateTime.Today);

    // Either deleted or active items are returned; not both.
    internal static IQueryable<EnforcementOrder> FilterByIsDeleted(
        [NotNull] this IQueryable<EnforcementOrder> query,
        bool showDeleted) =>
        query.Where(e => e.Deleted == showDeleted);

    // Current Proposed are public proposed orders 
    // (publication date in the past)
    // with comment close date in the future
    public static IQueryable<EnforcementOrder> FilterForCurrentProposed(
        [NotNull] this IQueryable<EnforcementOrder> query) =>
        query.FilterForPublicProposed().FilterForOpenCommentPeriod();

    // Recently Executed are public executed orders with 
    // publication date of most recent Monday (or between
    // most recent Monday and today)
    public static IQueryable<EnforcementOrder> FilterForRecentlyExecuted(
        [NotNull] this IQueryable<EnforcementOrder> query) =>
        query.FilterForPublicExecuted()
            .Where(e => e.ExecutedOrderPostedDate >= MostRecentMonday());

    // Draft are orders with publication status set to Draft
    // or are missing publication dates
    public static IQueryable<EnforcementOrder> FilterForDrafts(
        [NotNull] this IQueryable<EnforcementOrder> query) =>
        query.FilterByIsDeleted(false)
            .Where(e =>
                e.PublicationStatus == EnforcementOrder.PublicationState.Draft
                || ((e.IsExecutedOrder && !e.ExecutedOrderPostedDate.HasValue)
                    || (e.IsProposedOrder && !e.ProposedOrderPostedDate.HasValue))
            );

    // Pending are public proposed or executed orders with 
    // publication date after the current week
    public static IQueryable<EnforcementOrder> FilterForPending(
        [NotNull] this IQueryable<EnforcementOrder> query) =>
        query.FilterByIsDeleted(false)
            .Where(e =>
                e.PublicationStatus == EnforcementOrder.PublicationState.Published
                && ((e.IsExecutedOrder && e.ExecutedOrderPostedDate.HasValue && e.ExecutedDate > MostRecentMonday())
                    || (e.IsProposedOrder && e.ProposedOrderPostedDate.HasValue &&
                        e.ProposedOrderPostedDate > MostRecentMonday()))
            );
}
