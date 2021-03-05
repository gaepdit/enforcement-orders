using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Infrastructure.Contexts;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.Repository.Specs;
using Enfo.Repository.Utils;
using Enfo.Repository.Validation;
using Microsoft.EntityFrameworkCore;

namespace Enfo.Infrastructure.Repositories
{
    public sealed class EnforcementOrderRepository : IEnforcementOrderRepository
    {
        private readonly EnfoDbContext _context;

        public EnforcementOrderRepository(EnfoDbContext context) => _context = context;

        // public async Task<CreateEntityResult<EnforcementOrder>> CreateEnforcementOrderAsync(
        //     NewEnforcementOrderType createAs, string cause, int? commentContactId, DateTime? commentPeriodClosesDate,
        //     string county, string facilityName, DateTime? executedDate, DateTime? executedOrderPostedDate,
        //     DateTime? hearingCommentPeriodClosesDate, int? hearingContactId, DateTime? hearingDate,
        //     string hearingLocation, bool isHearingScheduled, int legalAuthorityId, string orderNumber,
        //     DateTime? proposedOrderPostedDate, EnforcementOrder.PublicationState publicationStatus, string requirements,
        //     decimal? settlementAmount)
        // {
        //     var result = ValidateNewEnforcementOrder(
        //         createAs, cause, commentContactId, commentPeriodClosesDate, county, facilityName, executedDate,
        //         executedOrderPostedDate, hearingCommentPeriodClosesDate, hearingContactId, hearingDate, hearingLocation,
        //         isHearingScheduled, legalAuthorityId, orderNumber, proposedOrderPostedDate, publicationStatus,
        //         requirements, settlementAmount);
        //
        //     if (await OrderNumberExists(orderNumber).ConfigureAwait(false))
        //     {
        //         result.AddErrorMessage("OrderNumber", "An Order with the same number already exists.");
        //     }
        //
        //     if (result.Success)
        //     {
        //         Add(result.NewItem);
        //         await CompleteAsync().ConfigureAwait(false);
        //     }
        //
        //     return result;
        // }
        //
        // public async Task<UpdateEntityResult> UpdateEnforcementOrderAsync(
        //     int id,
        //     string cause,
        //     int? commentContactId,
        //     DateTime? commentPeriodClosesDate,
        //     string county,
        //     string facilityName,
        //     DateTime? executedDate,
        //     DateTime? executedOrderPostedDate,
        //     DateTime? hearingCommentPeriodClosesDate,
        //     int? hearingContactId,
        //     DateTime? hearingDate,
        //     string hearingLocation,
        //     bool isExecutedOrder,
        //     bool isHearingScheduled,
        //     int legalAuthorityId,
        //     string orderNumber,
        //     DateTime? proposedOrderPostedDate,
        //     EnforcementOrder.PublicationState publicationStatus,
        //     string requirements,
        //     decimal? settlementAmount)
        // {
        //     var originalOrder = await GetAsync(id).ConfigureAwait(false);
        //
        //     var result = UpdateEnforcementOrder(originalOrder,
        //         cause, commentContactId, commentPeriodClosesDate, county, facilityName, executedDate,
        //         executedOrderPostedDate, hearingCommentPeriodClosesDate, hearingContactId, hearingDate, hearingLocation,
        //         isExecutedOrder, isHearingScheduled, legalAuthorityId, orderNumber, proposedOrderPostedDate,
        //         publicationStatus, requirements, settlementAmount);
        //
        //     if (await OrderNumberExists(orderNumber, id).ConfigureAwait(false))
        //     {
        //         result.AddErrorMessage("OrderNumber", "An Order with the same number already exists.");
        //     }
        //
        //     if (result.Success)
        //     {
        //         await CompleteAsync().ConfigureAwait(false);
        //     }
        //
        //     return result;
        // }

        public async Task<EnforcementOrderDetailedView> GetAsync(int id, bool onlyPublic = true)
        {
            var item = await _context.EnforcementOrders.AsNoTracking()
                .Include(e => e.CommentContact)
                .Include(e => e.HearingContact)
                .Include(e => e.LegalAuthority)
                .SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

            if (item == null || (onlyPublic && !item.GetIsPublic()))
                return null;

            return new EnforcementOrderDetailedView(item);
        }

        public async Task<EnforcementOrderAdminView> GetAdminViewAsync(int id)
        {
            var item = (await _context.EnforcementOrders.AsNoTracking()
                .Include(e => e.CommentContact)
                .Include(e => e.HearingContact)
                .Include(e => e.LegalAuthority)
                .SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false));

            return item == null ? null : new EnforcementOrderAdminView(item);
        }

        public async Task<PaginatedResult<EnforcementOrderSummaryView>> ListAsync(
            EnforcementOrderSpec spec, PaginationSpec paging)
        {
            Guard.NotNull(spec, nameof(spec));
            Guard.NotNull(paging, nameof(paging));

            var filteredItems = _context.EnforcementOrders.AsNoTracking()
                .ApplySpecFilter(spec);

            var items = await filteredItems
                .ApplySorting(spec.SortOrder)
                .Include(e => e.LegalAuthority)
                .ApplyPagination(paging)
                .Select(e => new EnforcementOrderSummaryView(e))
                .ToListAsync().ConfigureAwait(false);

            var count = await filteredItems.CountAsync().ConfigureAwait(false);

            return new PaginatedResult<EnforcementOrderSummaryView>(items, count, paging);
        }

        public async Task<int> CountAsync(EnforcementOrderSpec spec) =>
            await _context.EnforcementOrders.AsNoTracking()
                .ApplySpecFilter(spec ?? new EnforcementOrderSpec())
                .CountAsync().ConfigureAwait(false);

        public async Task<bool> ExistsAsync(int id, bool onlyPublic = true)
        {
            var item = await _context.EnforcementOrders.AsNoTracking()
                .Include(e => e.CommentContact)
                .Include(e => e.HearingContact)
                .Include(e => e.LegalAuthority)
                .SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

            return item != null && (!onlyPublic || item.GetIsPublic());
        }

        public async Task<bool> OrderNumberExistsAsync(string orderNumber, int? ignoreId = null) =>
            await _context.EnforcementOrders.AsNoTracking()
                .AnyAsync(e => e.OrderNumber == orderNumber && e.Id != ignoreId)
                .ConfigureAwait(false);

        // Current Proposed are public proposed orders 
        // (publication date in the past)
        // with comment close date in the future
        public async Task<IReadOnlyList<EnforcementOrderSummaryView>> ListCurrentProposedEnforcementOrdersAsync() =>
            await _context.EnforcementOrders.AsNoTracking()
                .FilterForCurrentProposed()
                .ApplySorting(EnforcementOrderSpec.EnforcementOrderSorting.DateAsc)
                .Include(e => e.LegalAuthority)
                .Select(e => new EnforcementOrderSummaryView(e))
                .ToListAsync().ConfigureAwait(false);

        // Draft are orders with publication status set to Draft
        public async Task<IReadOnlyList<EnforcementOrderSummaryView>> ListDraftEnforcementOrdersAsync() =>
            await _context.EnforcementOrders.AsNoTracking()
                .FilterForDrafts()
                .ApplySorting(EnforcementOrderSpec.EnforcementOrderSorting.DateAsc)
                .Include(e => e.LegalAuthority)
                .Select(e => new EnforcementOrderSummaryView(e))
                .ToListAsync().ConfigureAwait(false);

        public async Task<IReadOnlyList<EnforcementOrderSummaryView>> ListPendingEnforcementOrdersAsync() =>
            await _context.EnforcementOrders.AsNoTracking()
                .FilterForPending()
                .ApplySorting(EnforcementOrderSpec.EnforcementOrderSorting.DateAsc)
                .Include(e => e.LegalAuthority)
                .Select(e => new EnforcementOrderSummaryView(e))
                .ToListAsync().ConfigureAwait(false);

        public async Task<IReadOnlyList<EnforcementOrderSummaryView>> ListRecentlyExecutedEnforcementOrdersAsync() =>
            await _context.EnforcementOrders.AsNoTracking()
                .FilterForRecentlyExecuted()
                .ApplySorting(EnforcementOrderSpec.EnforcementOrderSorting.DateAsc)
                .Include(e => e.LegalAuthority)
                .Select(e => new EnforcementOrderSummaryView(e))
                .ToListAsync().ConfigureAwait(false);

        public async Task<int> CreateAsync(EnforcementOrderCreate resource)
        {
            throw new NotImplementedException();
        }

        public async Task<ResourceValidationResult> UpdateAsync(int id, EnforcementOrderUpdate resource)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task RestoreAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose() => _context.Dispose();
    }
}
