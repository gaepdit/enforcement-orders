using static Enfo.Domain.Utils.DateUtils;

namespace Enfo.Domain.EnforcementOrders.Specs;

public static class EnforcementOrderFilters
{
    internal static IQueryable<Entities.EnforcementOrder> FilterByFacility(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query,
        string facilityFilter) =>
        string.IsNullOrWhiteSpace(facilityFilter)
            ? query
            : query.Where(e => e.FacilityName.Contains(facilityFilter));

    internal static IQueryable<Entities.EnforcementOrder> FilterByCounty(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query,
        string county) =>
        string.IsNullOrWhiteSpace(county)
            ? query
            : query.Where(e => e.County.Contains(county));

    internal static IQueryable<Entities.EnforcementOrder> FilterByLegalAuth(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query,
        int? legalAuthId) =>
        legalAuthId.HasValue
            ? query.Where(e => e.LegalAuthorityId == legalAuthId)
            : query;

    internal static IQueryable<Entities.EnforcementOrder> FilterByDateRange(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query,
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
            _ => query
        };
    }

    internal static IQueryable<Entities.EnforcementOrder> FilterByActivityStatus(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query,
        ActivityState status) =>
        status switch
        {
            ActivityState.Executed => query.Where(e => e.IsExecutedOrder),
            ActivityState.Proposed => query.Where(e => e.IsProposedOrder && !e.IsExecutedOrder),
            _ => query
        };

    internal static IQueryable<Entities.EnforcementOrder> FilterByPublicationStatus(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query,
        PublicationState status) =>
        status switch
        {
            PublicationState.Draft => query.Where(e =>
                e.PublicationStatus == Entities.EnforcementOrder.PublicationState.Draft),
            PublicationState.Published => query.Where(e =>
                e.PublicationStatus == Entities.EnforcementOrder.PublicationState.Published),
            _ => query
        };

    internal static IQueryable<Entities.EnforcementOrder> FilterByOrderNumber(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query,
        string orderNumber) =>
        string.IsNullOrWhiteSpace(orderNumber)
            ? query
            : query.Where(e => e.OrderNumber.Contains(orderNumber));

    internal static IQueryable<Entities.EnforcementOrder> FilterByText(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query,
        string text) =>
        string.IsNullOrWhiteSpace(text)
            ? query
            : query.Where(e => e.Cause.Contains(text) || e.Requirements.Contains(text));

    private static IQueryable<Entities.EnforcementOrder> FilterForOpenCommentPeriod(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query) =>
        query.Where(e => e.CommentPeriodClosesDate >= DateTime.Today);

    public static IQueryable<Entities.EnforcementOrder> FilterForOnlyPublic(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query) =>
        query.Where(e =>
            !e.Deleted
            && e.PublicationStatus == Entities.EnforcementOrder.PublicationState.Published
            && (e.IsExecutedOrder && e.ExecutedOrderPostedDate.HasValue
                && e.ExecutedOrderPostedDate.Value <= DateTime.Today
                || (e.IsProposedOrder && e.ProposedOrderPostedDate.HasValue
                    && e.ProposedOrderPostedDate.Value <= DateTime.Today)));

    private static IQueryable<Entities.EnforcementOrder> FilterForPublicProposed(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query) =>
        query.Where(e =>
            !e.Deleted
            && e.PublicationStatus == Entities.EnforcementOrder.PublicationState.Published
            && e.IsProposedOrder
            && e.ProposedOrderPostedDate.HasValue
            && e.ProposedOrderPostedDate.Value <= DateTime.Today);

    private static IQueryable<Entities.EnforcementOrder> FilterForPublicExecuted(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query) =>
        query.Where(e =>
            !e.Deleted
            && e.PublicationStatus == Entities.EnforcementOrder.PublicationState.Published
            && e.IsExecutedOrder
            && e.ExecutedOrderPostedDate.HasValue
            && e.ExecutedOrderPostedDate.Value <= DateTime.Today);

    // Either deleted or active items are returned; not both.
    internal static IQueryable<Entities.EnforcementOrder> FilterByIsDeleted(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query,
        bool showDeleted) =>
        query.Where(e => e.Deleted == showDeleted);

    // Current Proposed are public proposed orders 
    // (publication date in the past)
    // with comment close date in the future
    public static IQueryable<Entities.EnforcementOrder> FilterForCurrentProposed(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query) =>
        query.FilterForPublicProposed().FilterForOpenCommentPeriod();

    // Recently Executed are public executed orders with 
    // publication date of most recent Monday (or between
    // most recent Monday and today)
    public static IQueryable<Entities.EnforcementOrder> FilterForRecentlyExecuted(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query) =>
        query.FilterForPublicExecuted()
            .Where(e => e.ExecutedOrderPostedDate >= MostRecentMonday());

    // Draft are orders with publication status set to Draft
    // or are missing publication dates
    public static IQueryable<Entities.EnforcementOrder> FilterForDrafts(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query) =>
        query.FilterByIsDeleted(false)
            .Where(e =>
                e.PublicationStatus == Entities.EnforcementOrder.PublicationState.Draft
                || ((e.IsExecutedOrder && !e.ExecutedOrderPostedDate.HasValue)
                    || (e.IsProposedOrder && !e.ProposedOrderPostedDate.HasValue))
            );

    // Pending are public proposed or executed orders with 
    // publication date after the current week
    public static IQueryable<Entities.EnforcementOrder> FilterForPending(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query) =>
        query.FilterByIsDeleted(false)
            .Where(e =>
                e.PublicationStatus == Entities.EnforcementOrder.PublicationState.Published
                && ((e.IsExecutedOrder && e.ExecutedOrderPostedDate.HasValue && e.ExecutedDate > MostRecentMonday())
                    || (e.IsProposedOrder && e.ProposedOrderPostedDate.HasValue &&
                        e.ProposedOrderPostedDate > MostRecentMonday()))
            );
}
