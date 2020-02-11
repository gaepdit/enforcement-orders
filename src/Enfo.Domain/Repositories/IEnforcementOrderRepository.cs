using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Entities;
using Enfo.Domain.Querying;
using static Enfo.Domain.Entities.EnforcementOrder;
using static Enfo.Domain.Entities.Enums;

namespace Enfo.Domain.Repositories
{
    public interface IEnforcementOrderRepository : IWritableRepository<EnforcementOrder>

    {
        Task<EnforcementOrder> GetEnforcementOrder(
            int id,
            bool onlyIfPublic);

        Task<IReadOnlyList<EnforcementOrder>> FindEnforcementOrdersAsync(
            string FacilityFilter,
            string County,
            int? LegalAuth,
            DateTime? FromDate,
            DateTime? TillDate,
            ActivityStatus Status,
            PublicationStatus PublicationStatus,
            string OrderNumber,
            string TextContains,
            bool onlyIfPublic,
            bool Deleted,
            EnforcementOrderSorting SortOrder,
            IPagination pagination = null);

        Task<int> CountEnforcementOrdersAsync(
            string FacilityFilter,
            string County,
            int? LegalAuth,
            DateTime? FromDate,
            DateTime? TillDate,
            ActivityStatus Status,
            PublicationStatus PublicationStatus,
            string OrderNumber,
            string TextContains,
            bool onlyIfPublic,
            bool Deleted);

        Task<IReadOnlyList<EnforcementOrder>> FindCurrentProposedEnforcementOrders();

        Task<IReadOnlyList<EnforcementOrder>> FindDraftEnforcementOrders();

        Task<IReadOnlyList<EnforcementOrder>> FindPendingEnforcementOrders();

        Task<IReadOnlyList<EnforcementOrder>> FindRecentlyExecutedEnforcementOrders();

        Task<bool> OrderNumberExists(
            string orderNumber,
            int ignoreId = -1);

        Task<CreateEntityResult<EnforcementOrder>> CreateEnforcementOrderAsync(
            NewEnforcementOrderType createAs,
            string cause,
            int? commentContactId,
            DateTime? commentPeriodClosesDate,
            string county,
            string facilityName,
            DateTime? executedDate,
            DateTime? executedOrderPostedDate,
            DateTime? hearingCommentPeriodClosesDate,
            int? hearingContactId,
            DateTime? hearingDate,
            string hearingLocation,
            bool isHearingScheduled,
            int legalAuthorityId,
            string orderNumber,
            DateTime? proposedOrderPostedDate,
            PublicationState publicationStatus,
            string requirements,
            decimal? settlementAmount);

        Task<UpdateEntityResult> UpdateEnforcementOrderAsync(
            int id,
            string cause,
            int? commentContactId,
            DateTime? commentPeriodClosesDate,
            string county,
            string facilityName,
            DateTime? executedDate,
            DateTime? executedOrderPostedDate,
            DateTime? hearingCommentPeriodClosesDate,
            int? hearingContactId,
            DateTime? hearingDate,
            string hearingLocation,
            bool isExecutedOrder,
            bool isHearingScheduled,
            int legalAuthorityId,
            string orderNumber,
            DateTime? proposedOrderPostedDate,
            PublicationState publicationStatus,
            string requirements,
            decimal? settlementAmount);
    }
}
