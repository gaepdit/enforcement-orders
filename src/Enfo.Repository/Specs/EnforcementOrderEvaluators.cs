using System.Linq;
using Enfo.Domain.Entities;
using JetBrains.Annotations;

namespace Enfo.Repository.Specs
{
    public static class EnforcementOrderEvaluators
    {
        public static IQueryable<EnforcementOrder> ApplySpecFilter(
            [NotNull] this IQueryable<EnforcementOrder> query, [NotNull] EnforcementOrderAdminSpec adminSpec) =>
            query.FilterByFacility(adminSpec.FacilityFilter)
                .FilterByCounty(adminSpec.County)
                .FilterByLegalAuth(adminSpec.LegalAuthId)
                .FilterByStartDate(adminSpec.FromDate, adminSpec.Status)
                .FilterByEndDate(adminSpec.TillDate, adminSpec.Status)
                .FilterByActivityStatus(adminSpec.Status)
                .FilterByPublicationStatus(adminSpec.PublicationStatus)
                .FilterByOrderNumber(adminSpec.OrderNumber)
                .FilterByText(adminSpec.TextContains)
                .FilterByIsPublic(adminSpec.OnlyPublic)
                .FilterByIsDeleted(adminSpec.ShowDeleted);

        public static IOrderedQueryable<EnforcementOrder> ApplySorting(
            [NotNull] this IQueryable<EnforcementOrder> query, OrderSorting sorting) =>
            sorting switch
            {
                OrderSorting.DateAsc =>
                    query.OrderBy(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                        .ThenBy(e => e.FacilityName),
                OrderSorting.DateDesc =>
                    query.OrderByDescending(e => e.ExecutedDate ?? e.ProposedOrderPostedDate)
                        .ThenBy(e => e.FacilityName),
                OrderSorting.FacilityDesc =>
                    query.OrderBy(e => e.FacilityName)
                        .ThenBy(e => e.ExecutedDate ?? e.ProposedOrderPostedDate),
                OrderSorting.FacilityAsc =>
                    query.OrderByDescending(e => e.FacilityName)
                        .ThenBy(e => e.ExecutedDate ?? e.ProposedOrderPostedDate),
                _ => query.OrderBy(e => 1)
            };

        public static IQueryable<EnforcementOrder> ApplyPagination(
            [NotNull] this IQueryable<EnforcementOrder> query, [NotNull] PaginationSpec paging) =>
            query.Skip(paging.Skip).Take(paging.Take);
    }
}