using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Entities;
using Enfo.Infrastructure.Contexts;
using Enfo.Repository.Interfaces;
using Enfo.Repository.PaginationSpec;
using Enfo.Repository.Querying;
using Microsoft.EntityFrameworkCore;
using static Enfo.Domain.Entities.EnforcementOrder;
using static Enfo.Domain.Entities.Enums;
using static Enfo.Repository.Utils.DateUtils;
using static Enfo.Repository.Resources.EnforcementOrder.EnforcementOrderCreate;

namespace Enfo.Infrastructure.Repositories
{
    public class EnforcementOrderRepository :  IEnforcementOrderRepository
    {
        private readonly EnfoDbContext _context;
        public EnforcementOrderRepository(EnfoDbContext context) => _context = context;

        public Task<EnforcementOrder> GetEnforcementOrder(int id, bool onlyIfPublic)
        {
            ISpecification<EnforcementOrder> spec = new TrueSpec<EnforcementOrder>();

            if (onlyIfPublic)
                spec = spec.And(new IsPublicOrdersSpec());

            var include = new EnforcementOrderIncludeAll();

            return GetAsync(id, spec, include);
        }

        public Task<IReadOnlyList<EnforcementOrder>> FindEnforcementOrdersAsync(
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
            Pagination pagination = null)
        {
            // Either deleted or active items are returned; not both.
            ISpecification<EnforcementOrder> spec = new FilterOrdersByDeletedStatus(Deleted);

            if (onlyIfPublic)
                spec = spec.And(new IsPublicOrdersSpec());

            if (!FacilityFilter.IsNullOrWhiteSpace())
                spec = spec.And(new FilterOrdersByFacilityName(FacilityFilter));

            if (!County.IsNullOrWhiteSpace())
                spec = spec.And(new FilterOrdersByCounty(County));

            if (LegalAuth.HasValue)
                spec = spec.And(new FilterOrdersByLegalAuth(LegalAuth.Value));

            if (FromDate.HasValue)
                spec = spec.And(new FilterOrdersByStartDate(FromDate.Value, Status));

            if (TillDate.HasValue)
                spec = spec.And(new FilterOrdersByEndDate(TillDate.Value, Status));

            if (Status != ActivityStatus.All)
                spec = spec.And(new FilterOrdersByActivityStatus(Status));

            if (PublicationStatus != PublicationStatus.All)
                spec = spec.And(new FilterOrdersByPublicationStatus(PublicationStatus));

            if (!OrderNumber.IsNullOrWhiteSpace())
                spec = spec.And(new FilterOrdersByOrderNumber(OrderNumber));

            if (!TextContains.IsNullOrWhiteSpace())
                spec = spec.And(new FilterOrdersByText(TextContains));

            // Sorting
            var sorting = new SortEnforcementOrders(SortOrder);

            // Including
            var include = new EnforcementOrderIncludeLegalAuth();

            return ListAsync(spec, pagination, sorting, include);
        }

        public Task<int> CountEnforcementOrdersAsync(
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
            bool Deleted)
        {
            // Either deleted or active items are counted; not both.
            ISpecification<EnforcementOrder> spec = new FilterOrdersByDeletedStatus(Deleted);

            if (onlyIfPublic)
                spec = spec.And(new IsPublicOrdersSpec());

            if (!FacilityFilter.IsNullOrWhiteSpace())
                spec = spec.And(new FilterOrdersByFacilityName(FacilityFilter));

            if (!County.IsNullOrWhiteSpace())
                spec = spec.And(new FilterOrdersByCounty(County));

            if (LegalAuth.HasValue)
                spec = spec.And(new FilterOrdersByLegalAuth(LegalAuth.Value));

            if (FromDate.HasValue)
                spec = spec.And(new FilterOrdersByStartDate(FromDate.Value, Status));

            if (TillDate.HasValue)
                spec = spec.And(new FilterOrdersByEndDate(TillDate.Value, Status));

            if (Status != ActivityStatus.All)
                spec = spec.And(new FilterOrdersByActivityStatus(Status));

            if (PublicationStatus != PublicationStatus.All)
                spec = spec.And(new FilterOrdersByPublicationStatus(PublicationStatus));

            if (!OrderNumber.IsNullOrWhiteSpace())
                spec = spec.And(new FilterOrdersByOrderNumber(OrderNumber));

            if (!TextContains.IsNullOrWhiteSpace())
                spec = spec.And(new FilterOrdersByText(TextContains));

            return CountAsync(spec);
        }

        public Task<IReadOnlyList<EnforcementOrder>> FindCurrentProposedEnforcementOrders()
        {
            // Current Proposed are public proposed orders 
            // (publication date in the past)
            // with comment close date in the future
            var spec = new IsPublicProposedOrderSpec()
                .And(new FilterOrdersByCommentPeriod(DateTime.Today));

            var include = new EnforcementOrderIncludeLegalAuth();

            return ListAsync(spec, inclusion: include);
        }

        public Task<IReadOnlyList<EnforcementOrder>> FindDraftEnforcementOrders()
        {
            // Draft are orders with publication status set to Draft
            var spec = new FilterOrdersByPublicationStatus(PublicationStatus.Draft);

            var include = new EnforcementOrderIncludeLegalAuth();

            return ListAsync(spec, inclusion: include);
        }

        public Task<IReadOnlyList<EnforcementOrder>> FindPendingEnforcementOrders()
        {
            // Pending are public proposed or executed orders with 
            // publication date after the current week
            var lastPostedAfter = GetNextWeekday(DateTime.Today.AddDays(-6), DayOfWeek.Monday);

            var spec = new IsPublicOrdersSpec()
                .And(new FilterOrdersByLastPostedDate(lastPostedAfter));

            var include = new EnforcementOrderIncludeLegalAuth();

            return ListAsync(spec, inclusion: include);
        }

        public Task<IReadOnlyList<EnforcementOrder>> FindRecentlyExecutedEnforcementOrders()
        {
            // Recently Executed are public executed orders with 
            // publication date within current week

            // fromDate is most recent Monday
            var fromDate = GetNextWeekday(DateTime.Today.AddDays(-6), DayOfWeek.Monday);

            var spec = new IsPublicExecutedOrderSpec()
                .And(new FilterOrdersByExecutedOrderPostedDate(fromDate, DateTime.Today));

            var include = new EnforcementOrderIncludeLegalAuth();

            return ListAsync(spec, inclusion: include);
        }

        public async Task<bool> OrderNumberExists(string orderNumber,
            int ignoreId = -1)
        {
            return await _context.Set<EnforcementOrder>()
            .AnyAsync(e => e.OrderNumber == orderNumber && e.Id != ignoreId)
            .ConfigureAwait(false);
        }

        public async Task<CreateEntityResult<EnforcementOrder>> CreateEnforcementOrderAsync(
            NewEnforcementOrderType createAs, string cause, int? commentContactId, DateTime? commentPeriodClosesDate,
            string county, string facilityName, DateTime? executedDate, DateTime? executedOrderPostedDate,
            DateTime? hearingCommentPeriodClosesDate, int? hearingContactId, DateTime? hearingDate,
            string hearingLocation, bool isHearingScheduled, int legalAuthorityId, string orderNumber,
            DateTime? proposedOrderPostedDate, PublicationState publicationStatus, string requirements,
            decimal? settlementAmount)
        {
            var result = ValidateNewEnforcementOrder(
                createAs, cause, commentContactId, commentPeriodClosesDate, county, facilityName, executedDate,
                executedOrderPostedDate, hearingCommentPeriodClosesDate, hearingContactId, hearingDate, hearingLocation,
                isHearingScheduled, legalAuthorityId, orderNumber, proposedOrderPostedDate, publicationStatus,
                requirements, settlementAmount);

            if (await OrderNumberExists(orderNumber).ConfigureAwait(false))
            {
                result.AddErrorMessage("OrderNumber", "An Order with the same number already exists.");
            }

            if (result.Success)
            {
                Add(result.NewItem);
                await CompleteAsync().ConfigureAwait(false);
            }

            return result;
        }

        public async Task<UpdateEntityResult> UpdateEnforcementOrderAsync(
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
            decimal? settlementAmount)
        {
            var originalOrder = await GetAsync(id).ConfigureAwait(false);

            var result = UpdateEnforcementOrder(originalOrder,
                cause, commentContactId, commentPeriodClosesDate, county, facilityName, executedDate,
                executedOrderPostedDate, hearingCommentPeriodClosesDate, hearingContactId, hearingDate, hearingLocation,
                isExecutedOrder, isHearingScheduled, legalAuthorityId, orderNumber, proposedOrderPostedDate,
                publicationStatus, requirements, settlementAmount);

            if (await OrderNumberExists(orderNumber, id).ConfigureAwait(false))
            {
                result.AddErrorMessage("OrderNumber", "An Order with the same number already exists.");
            }

            if (result.Success)
            {
                await CompleteAsync().ConfigureAwait(false);
            }

            return result;
        }
    }
}
