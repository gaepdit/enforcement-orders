using System.Linq;
using Enfo.Domain.Entities;
using JetBrains.Annotations;
using static Enfo.Repository.Specs.EnforcementOrderSpec;

namespace Enfo.Repository.Specs
{
    public static class EnforcementOrderEvaluators
    {
        public static IQueryable<EnforcementOrder> ApplySpecFilter(
            [NotNull] this IQueryable<EnforcementOrder> query, [NotNull] EnforcementOrderSpec spec) =>
            query.FilterByFacility(spec.FacilityFilter)
                .FilterByCounty(spec.County)
                .FilterByLegalAuth(spec.LegalAuthId)
                .FilterByStartDate(spec.FromDate, spec.Status)
                .FilterByEndDate(spec.TillDate, spec.Status)
                .FilterByActivityStatus(spec.Status)
                .FilterByPublicationStatus(spec.PublicationStatus)
                .FilterByOrderNumber(spec.OrderNumber)
                .FilterByText(spec.TextContains)
                .FilterByIsPublic(spec.OnlyPublic)
                .FilterByIsDeleted(spec.ShowDeleted);

        public static IOrderedQueryable<EnforcementOrder> ApplySorting(
            [NotNull] this IQueryable<EnforcementOrder> query, EnforcementOrderSorting sorting) =>
            sorting switch
            {
                EnforcementOrderSorting.DateAsc =>
                    query.OrderBy(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                        .ThenBy(e => e.FacilityName.Trim().Trim('\n', '\r', '\t')),
                EnforcementOrderSorting.DateDesc =>
                    query.OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                        .ThenBy(e => e.FacilityName.Trim().Trim('\n', '\r', '\t')),
                EnforcementOrderSorting.FacilityDesc =>
                    query.OrderBy(e => e.FacilityName.Trim().Trim('\n', '\r', '\t'))
                        .ThenBy(e => e.ExecutedDate ?? e.ProposedOrderPostedDate),
                EnforcementOrderSorting.FacilityAsc =>
                    query.OrderByDescending(e => e.FacilityName.Trim().Trim('\n', '\r', '\t'))
                        .ThenBy(e => e.ExecutedDate ?? e.ProposedOrderPostedDate),
                _ => query.OrderBy(e => 1)
            };

        public static IQueryable<EnforcementOrder> ApplyPagination(
            [NotNull] this IQueryable<EnforcementOrder> query, [NotNull] PaginationSpec paging) =>
            query.Skip(paging.Skip).Take(paging.Take);
    }
}