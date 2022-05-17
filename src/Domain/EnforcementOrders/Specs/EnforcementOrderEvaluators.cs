using Enfo.Domain.Pagination;

namespace Enfo.Domain.EnforcementOrders.Specs;

public static class EnforcementOrderEvaluators
{
    public static IQueryable<Entities.EnforcementOrder> ApplySpecFilter(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query, [NotNull] EnforcementOrderSpec spec) =>
        query.FilterForOnlyPublic()
            .FilterByFacility(spec.Facility)
            .FilterByCounty(spec.County)
            .FilterByLegalAuth(spec.LegalAuth)
            .FilterByDateRange(spec.FromDate, spec.TillDate, spec.Status)
            .FilterByActivityStatus(spec.Status)
            .FilterByOrderNumber(spec.OrderNumber);

    public static IQueryable<Entities.EnforcementOrder> ApplyAdminSpecFilter(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query, [NotNull] EnforcementOrderAdminSpec spec) =>
        query.FilterByFacility(spec.Facility)
            .FilterByCounty(spec.County)
            .FilterByLegalAuth(spec.LegalAuth)
            .FilterByDateRange(spec.FromDate, spec.TillDate, spec.Status)
            .FilterByActivityStatus(spec.Status)
            .FilterForAttachments(spec.WithAttachments)
            .FilterByPublicationStatus(spec.Progress)
            .FilterByOrderNumber(spec.OrderNumber)
            .FilterByText(spec.Text)
            .FilterByIsDeleted(spec.ShowDeleted ?? false);

    public static IOrderedQueryable<Entities.EnforcementOrder> ApplySorting(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query, OrderSorting sorting) =>
        sorting switch
        {
            OrderSorting.DateAsc =>
                query.OrderBy(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName),
            OrderSorting.DateDesc =>
                query.OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                    .ThenBy(e => e.FacilityName),
            OrderSorting.FacilityAsc =>
                query.OrderBy(e => e.FacilityName)
                    .ThenBy(e => e.ExecutedDate ?? e.ProposedOrderPostedDate),
            OrderSorting.FacilityDesc =>
                query.OrderByDescending(e => e.FacilityName)
                    .ThenBy(e => e.ExecutedDate ?? e.ProposedOrderPostedDate),
            _ => query.OrderBy(e => 1)
        };

    public static IQueryable<Entities.EnforcementOrder> ApplyPagination(
        [NotNull] this IQueryable<Entities.EnforcementOrder> query, [NotNull] PaginationSpec paging) =>
        query.Skip(paging.Skip).Take(paging.Take);
}
