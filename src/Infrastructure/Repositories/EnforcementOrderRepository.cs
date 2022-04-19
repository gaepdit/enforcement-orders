using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.Pagination;
using Enfo.Domain.Utils;
using Enfo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Enfo.Infrastructure.Repositories;

public sealed class EnforcementOrderRepository : IEnforcementOrderRepository
{
    private readonly EnfoDbContext _context;
    public EnforcementOrderRepository(EnfoDbContext context) => _context = context;

    public async Task<EnforcementOrderDetailedView> GetAsync(int id)
    {
        var item = await _context.EnforcementOrders.AsNoTracking()
            .FilterForOnlyPublic()
            .Include(e => e.CommentContact)
            .Include(e => e.HearingContact)
            .Include(e => e.LegalAuthority)
            .SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

        return item == null ? null : new EnforcementOrderDetailedView(item);
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
            .ApplySorting(spec.Sort)
            .Include(e => e.LegalAuthority)
            .ApplyPagination(paging)
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToListAsync().ConfigureAwait(false);

        var count = await filteredItems.CountAsync().ConfigureAwait(false);

        return new PaginatedResult<EnforcementOrderSummaryView>(items, count, paging);
    }

    public async Task<PaginatedResult<EnforcementOrderDetailedView>> ListDetailedAsync(
        EnforcementOrderSpec spec, PaginationSpec paging)
    {
        Guard.NotNull(spec, nameof(spec));
        Guard.NotNull(paging, nameof(paging));

        var filteredItems = _context.EnforcementOrders.AsNoTracking()
            .ApplySpecFilter(spec);

        var items = await filteredItems
            .ApplySorting(spec.Sort)
            .Include(e => e.CommentContact)
            .Include(e => e.HearingContact)
            .Include(e => e.LegalAuthority)
            .ApplyPagination(paging)
            .Select(e => new EnforcementOrderDetailedView(e))
            .ToListAsync().ConfigureAwait(false);

        var count = await filteredItems.CountAsync().ConfigureAwait(false);

        return new PaginatedResult<EnforcementOrderDetailedView>(items, count, paging);
    }

    public async Task<PaginatedResult<EnforcementOrderAdminSummaryView>> ListAdminAsync(
        EnforcementOrderAdminSpec spec, PaginationSpec paging)
    {
        Guard.NotNull(spec, nameof(spec));
        Guard.NotNull(paging, nameof(paging));

        var filteredItems = _context.EnforcementOrders.AsNoTracking()
            .ApplyAdminSpecFilter(spec);

        var items = await filteredItems
            .ApplySorting(spec.Sort)
            .Include(e => e.LegalAuthority)
            .ApplyPagination(paging)
            .Select(e => new EnforcementOrderAdminSummaryView(e))
            .ToListAsync().ConfigureAwait(false);

        var count = await filteredItems.CountAsync().ConfigureAwait(false);

            return new PaginatedResult<EnforcementOrderAdminSummaryView>(items, count, paging);
        }

        public async Task<bool> ExistsAsync(int id, bool onlyPublic = true)
        {
            var item = await _context.EnforcementOrders.AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

            return item != null && (!onlyPublic || item.GetIsPublic);
        }

        public Task<bool> OrderNumberExistsAsync(string orderNumber, int? ignoreId = null) =>
            _context.EnforcementOrders.AsNoTracking()
            .AnyAsync(e => e.OrderNumber == orderNumber && !e.Deleted && e.Id != ignoreId);

    // Current Proposed are public proposed orders (publication date in the past)
    // with comment close date in the future
    public async Task<IReadOnlyList<EnforcementOrderDetailedView>> ListCurrentProposedEnforcementOrdersAsync() =>
        await _context.EnforcementOrders.AsNoTracking()
            .FilterForCurrentProposed()
            .ApplySorting(OrderSorting.DateAsc)
            .Include(e => e.CommentContact)
            .Include(e => e.HearingContact)
            .Include(e => e.LegalAuthority)
            .Select(e => new EnforcementOrderDetailedView(e))
            .ToListAsync().ConfigureAwait(false);

    // Recently Executed are public executed orders with 
    // publication date within current week
    public async Task<IReadOnlyList<EnforcementOrderDetailedView>> ListRecentlyExecutedEnforcementOrdersAsync() =>
        await _context.EnforcementOrders.AsNoTracking()
            .FilterForRecentlyExecuted()
            .ApplySorting(OrderSorting.DateAsc)
            .Include(e => e.CommentContact)
            .Include(e => e.HearingContact)
            .Include(e => e.LegalAuthority)
            .Select(e => new EnforcementOrderDetailedView(e))
            .ToListAsync().ConfigureAwait(false);

    // Draft are orders with publication status set to Draft
    public async Task<IReadOnlyList<EnforcementOrderAdminSummaryView>> ListDraftEnforcementOrdersAsync() =>
        await _context.EnforcementOrders.AsNoTracking()
            .FilterForDrafts()
            .ApplySorting(OrderSorting.DateAsc)
            .Include(e => e.LegalAuthority)
            .Select(e => new EnforcementOrderAdminSummaryView(e))
            .ToListAsync().ConfigureAwait(false);

    // Pending are public proposed or executed orders with 
    // publication date after the current week
    public async Task<IReadOnlyList<EnforcementOrderAdminSummaryView>> ListPendingEnforcementOrdersAsync() =>
        await _context.EnforcementOrders.AsNoTracking()
            .FilterForPending()
            .ApplySorting(OrderSorting.DateAsc)
            .Include(e => e.LegalAuthority)
            .Select(e => new EnforcementOrderAdminSummaryView(e))
            .ToListAsync().ConfigureAwait(false);

    public async Task<int> CreateAsync(EnforcementOrderCreate resource)
    {
        resource.TrimAll();
        var item = new EnforcementOrder(resource);
        await _context.EnforcementOrders.AddAsync(item).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
        return item.Id;
    }

    public async Task UpdateAsync(EnforcementOrderUpdate resource)
    {
        Guard.NotNull(resource, nameof(resource));

        var item = await _context.EnforcementOrders.FindAsync(resource.Id).ConfigureAwait(false)
            ?? throw new ArgumentException($"ID ({resource.Id}) not found.", nameof(resource));

        if (item.Deleted)
            throw new ArgumentException("A deleted Enforcement Order cannot be modified.", nameof(resource));

        item.ApplyUpdate(resource);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.EnforcementOrders.FindAsync(id).ConfigureAwait(false)
            ?? throw new ArgumentException($"ID ({id}) not found.", nameof(id));

        item.Deleted = true;
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task RestoreAsync(int id)
    {
        var item = await _context.EnforcementOrders.FindAsync(id).ConfigureAwait(false)
            ?? throw new ArgumentException($"ID ({id}) not found.", nameof(id));

        item.Deleted = false;
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public void Dispose() => _context.Dispose();
}
