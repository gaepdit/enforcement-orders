using System;
using System.Linq;
using Enfo.Domain.Entities;
using JetBrains.Annotations;
using static Enfo.Repository.Specs.EnforcementOrderSpec;
using static Enfo.Repository.Utils.DateUtils;

namespace Enfo.Repository.Specs
{
    public static class EnforcementOrderFilters
    {
        public static IQueryable<EnforcementOrder> FilterByFacility(
            [NotNull] this IQueryable<EnforcementOrder> query,
            string facilityFilter) =>
            string.IsNullOrWhiteSpace(facilityFilter)
                ? query
                : query.Where(e => e.FacilityName.Contains(facilityFilter));

        public static IQueryable<EnforcementOrder> FilterByCounty(
            [NotNull] this IQueryable<EnforcementOrder> query,
            string county) =>
            string.IsNullOrWhiteSpace(county)
                ? query
                : query.Where(e => e.FacilityName.Contains(county));

        public static IQueryable<EnforcementOrder> FilterByLegalAuth(
            [NotNull] this IQueryable<EnforcementOrder> query,
            int? legalAuthId) =>
            legalAuthId.HasValue
                ? query.Where(e => e.LegalAuthorityId == legalAuthId)
                : query;

        public static IQueryable<EnforcementOrder> FilterByStartDate(
            [NotNull] this IQueryable<EnforcementOrder> query,
            DateTime? fromDate, ActivityState status) =>
            fromDate.HasValue
                ? status switch
                {
                    ActivityState.All => query.Where(e =>
                        e.ProposedOrderPostedDate >= fromDate || e.ExecutedDate >= fromDate),
                    ActivityState.Executed => query.Where(e => e.ExecutedDate >= fromDate),
                    ActivityState.Proposed => query.Where(e => e.ProposedOrderPostedDate >= fromDate),
                    _ => query
                }
                : query;

        public static IQueryable<EnforcementOrder> FilterByEndDate(
            [NotNull] this IQueryable<EnforcementOrder> query,
            DateTime? tillDate, ActivityState status) =>
            tillDate.HasValue
                ? status switch
                {
                    ActivityState.All => query.Where(e =>
                        e.ProposedOrderPostedDate <= tillDate || e.ExecutedDate <= tillDate),
                    ActivityState.Executed => query.Where(e => e.ExecutedDate <= tillDate),
                    ActivityState.Proposed => query.Where(e => e.ProposedOrderPostedDate <= tillDate),
                    _ => query
                }
                : query;

        public static IQueryable<EnforcementOrder> FilterByActivityStatus(
            [NotNull] this IQueryable<EnforcementOrder> query,
            ActivityState status) =>
            status switch
            {
                ActivityState.Executed => query.Where(e => e.IsExecutedOrder),
                ActivityState.Proposed => query.Where(e => e.IsProposedOrder && !e.IsExecutedOrder),
                _ => query
            };

        public static IQueryable<EnforcementOrder> FilterByPublicationStatus(
            [NotNull] this IQueryable<EnforcementOrder> query,
            PublicationState status) =>
            status switch
            {
                PublicationState.Draft => query.Where(e =>
                    e.PublicationStatus == EnforcementOrder.PublicationState.Draft),
                PublicationState.Published => query.Where(e =>
                    e.PublicationStatus == EnforcementOrder.PublicationState.Published),
                _ => query
            };

        public static IQueryable<EnforcementOrder> FilterByOrderNumber(
            [NotNull] this IQueryable<EnforcementOrder> query,
            string orderNumber) =>
            string.IsNullOrWhiteSpace(orderNumber)
                ? query
                : query.Where(e => e.OrderNumber.Contains(orderNumber));

        public static IQueryable<EnforcementOrder> FilterByText(
            [NotNull] this IQueryable<EnforcementOrder> query,
            string text) =>
            string.IsNullOrWhiteSpace(text)
                ? query
                : query.Where(e => e.Cause.Contains(text) || e.Requirements.Contains(text));

        private static IQueryable<EnforcementOrder> FilterByCommentPeriod(
            [NotNull] this IQueryable<EnforcementOrder> query,
            DateTime commentPeriodClosesAfter) =>
            query.Where(e => e.CommentPeriodClosesDate >= commentPeriodClosesAfter);

        public static IQueryable<EnforcementOrder> FilterByIsPublic(
            [NotNull] this IQueryable<EnforcementOrder> query,
            bool onlyPublic) =>
            onlyPublic
                ? query.Where(e =>
                    !e.Deleted
                    && e.PublicationStatus == EnforcementOrder.PublicationState.Published
                    && (e.IsExecutedOrder && e.ExecutedOrderPostedDate.HasValue
                        && e.ExecutedOrderPostedDate.Value <= DateTime.Today
                        || (e.IsProposedOrder && e.ProposedOrderPostedDate.HasValue
                            && e.ProposedOrderPostedDate.Value <= DateTime.Today)))
                : query;

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
        public static IQueryable<EnforcementOrder> FilterByIsDeleted(
            [NotNull] this IQueryable<EnforcementOrder> query,
            bool showDeleted) =>
            query.Where(e => e.Deleted == showDeleted);

        // Current Proposed are public proposed orders 
        // (publication date in the past)
        // with comment close date in the future
        public static IQueryable<EnforcementOrder> FilterForCurrentProposed(
            [NotNull] this IQueryable<EnforcementOrder> query) =>
            query.FilterForPublicProposed()
                .FilterByCommentPeriod(DateTime.Today);

        // Draft are orders with publication status set to Draft
        public static IQueryable<EnforcementOrder> FilterForDrafts(
            [NotNull] this IQueryable<EnforcementOrder> query) =>
            query.FilterByIsDeleted(false)
                .FilterByPublicationStatus(PublicationState.Draft);

        // Pending are public proposed or executed orders with 
        // publication date after the current week
        public static IQueryable<EnforcementOrder> FilterForPending(
            [NotNull] this IQueryable<EnforcementOrder> query) =>
            query.FilterByIsPublic(true)
                .Where(e => e.ExecutedDate > MostRecentMonday()
                    || e.ProposedOrderPostedDate > MostRecentMonday());

        // Recently Executed are public executed orders with 
        // publication date within current week
        public static IQueryable<EnforcementOrder> FilterForRecentlyExecuted(
            [NotNull] this IQueryable<EnforcementOrder> query) =>
            query.FilterForPublicExecuted()
                .Where(e => e.ExecutedOrderPostedDate >= MostRecentMonday()
                    && e.ExecutedOrderPostedDate <= DateTime.Today);
    }
}