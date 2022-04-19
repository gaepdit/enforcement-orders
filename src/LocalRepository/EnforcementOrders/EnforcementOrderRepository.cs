using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.Pagination;
using Enfo.Domain.Utils;

namespace Enfo.LocalRepository.EnforcementOrders;

public sealed class EnforcementOrderRepository : IEnforcementOrderRepository
{
    public Task<EnforcementOrderDetailedView> GetAsync(int id) =>
        EnforcementOrderData.EnforcementOrders
            .AsQueryable().FilterForOnlyPublic().Any(e => e.Id == id)
            ? Task.FromResult(new EnforcementOrderDetailedView(
                EnforcementOrderData.EnforcementOrders.SingleOrDefault(e => e.Id == id)!))
            : Task.FromResult(null as EnforcementOrderDetailedView);

    public Task<EnforcementOrderAdminView> GetAdminViewAsync(int id) =>
        EnforcementOrderData.EnforcementOrders.Any(e => e.Id == id)
            ? Task.FromResult(new EnforcementOrderAdminView(
                EnforcementOrderData.EnforcementOrders.SingleOrDefault(e => e.Id == id)!))
            : Task.FromResult(null as EnforcementOrderAdminView);

    public Task<PaginatedResult<EnforcementOrderSummaryView>> ListAsync(
        EnforcementOrderSpec spec, PaginationSpec paging)
    {
        Guard.NotNull(spec, nameof(spec));
        Guard.NotNull(paging, nameof(paging));

        var filteredItems = EnforcementOrderData.EnforcementOrders.AsQueryable()
            .ApplySpecFilter(spec);

        var items = filteredItems
            .ApplySorting(spec.Sort)
            .ApplyPagination(paging)
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        var count = filteredItems.Count();
        var result = new PaginatedResult<EnforcementOrderSummaryView>(items, count, paging);
        return Task.FromResult(result);
    }

    public Task<PaginatedResult<EnforcementOrderDetailedView>> ListDetailedAsync(
        EnforcementOrderSpec spec, PaginationSpec paging)
    {
        Guard.NotNull(spec, nameof(spec));
        Guard.NotNull(paging, nameof(paging));

        var filteredItems = EnforcementOrderData.EnforcementOrders.AsQueryable()
            .ApplySpecFilter(spec);

        var items = filteredItems
            .ApplySorting(spec.Sort)
            .ApplyPagination(paging)
            .Select(e => new EnforcementOrderDetailedView(e))
            .ToList();

        var count = filteredItems.Count();
        var result = new PaginatedResult<EnforcementOrderDetailedView>(items, count, paging);
        return Task.FromResult(result);
    }

    public Task<PaginatedResult<EnforcementOrderAdminSummaryView>> ListAdminAsync(
        EnforcementOrderAdminSpec spec, PaginationSpec paging)
    {
        Guard.NotNull(spec, nameof(spec));
        Guard.NotNull(paging, nameof(paging));

        var filteredItems = EnforcementOrderData.EnforcementOrders.AsQueryable()
            .ApplyAdminSpecFilter(spec);

        var items = filteredItems
            .ApplySorting(spec.Sort)
            .ApplyPagination(paging)
            .Select(e => new EnforcementOrderAdminSummaryView(e))
            .ToList();

        var count = filteredItems.Count();
        var result = new PaginatedResult<EnforcementOrderAdminSummaryView>(items, count, paging);
        return Task.FromResult(result);
    }

    public Task<bool> ExistsAsync(int id, bool onlyPublic = true) =>
        Task.FromResult(
            EnforcementOrderData.EnforcementOrders
                .Any(e => e.Id == id && (e.GetIsPublic || !onlyPublic)));

    public Task<bool> OrderNumberExistsAsync(string orderNumber, int? ignoreId = null) =>
        Task.FromResult(
            EnforcementOrderData.EnforcementOrders
                .Any(e => e.OrderNumber == orderNumber && !e.Deleted && e.Id != ignoreId));

    public Task<IReadOnlyList<EnforcementOrderDetailedView>> ListCurrentProposedEnforcementOrdersAsync() =>
        Task.FromResult(
            (IReadOnlyList<EnforcementOrderDetailedView>)
            EnforcementOrderData.EnforcementOrders.AsQueryable()
                .FilterForCurrentProposed()
                .ApplySorting(OrderSorting.DateAsc)
                .Select(e => new EnforcementOrderDetailedView(e))
                .ToList());

    public Task<IReadOnlyList<EnforcementOrderDetailedView>> ListRecentlyExecutedEnforcementOrdersAsync() =>
        Task.FromResult(
            (IReadOnlyList<EnforcementOrderDetailedView>)
            EnforcementOrderData.EnforcementOrders.AsQueryable()
                .FilterForRecentlyExecuted()
                .ApplySorting(OrderSorting.DateAsc)
                .Select(e => new EnforcementOrderDetailedView(e))
                .ToList());

    public Task<IReadOnlyList<EnforcementOrderAdminSummaryView>> ListDraftEnforcementOrdersAsync() =>
        Task.FromResult(
            (IReadOnlyList<EnforcementOrderAdminSummaryView>)
            EnforcementOrderData.EnforcementOrders.AsQueryable()
                .FilterForDrafts()
                .ApplySorting(OrderSorting.DateAsc)
                .Select(e => new EnforcementOrderAdminSummaryView(e))
                .ToList());

    public Task<IReadOnlyList<EnforcementOrderAdminSummaryView>> ListPendingEnforcementOrdersAsync() =>
        Task.FromResult(
            (IReadOnlyList<EnforcementOrderAdminSummaryView>)
            EnforcementOrderData.EnforcementOrders.AsQueryable()
                .FilterForPending()
                .ApplySorting(OrderSorting.DateAsc)
                .Select(e => new EnforcementOrderAdminSummaryView(e))
                .ToList());

    public Task<int> CreateAsync(EnforcementOrderCreate resource)
    {
        resource.TrimAll();
        var id = EnforcementOrderData.EnforcementOrders.Max(e => e.Id) + 1;
        var item = new EnforcementOrder(resource) { Id = id };
        EnforcementOrderData.EnforcementOrders.Add(item);

        return Task.FromResult(id);
    }

    public Task UpdateAsync(EnforcementOrderUpdate resource)
    {
        Guard.NotNull(resource, nameof(resource));

        var item = EnforcementOrderData.EnforcementOrders.SingleOrDefault(e => e.Id == resource.Id)
            ?? throw new ArgumentException($"ID ({resource.Id}) not found.", nameof(resource));

        if (item.Deleted)
            throw new ArgumentException("A deleted Enforcement Order cannot be modified.", nameof(resource));

        item.ApplyUpdate(resource);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id) => SetWhetherDeleted(id, true);

    public Task RestoreAsync(int id) => SetWhetherDeleted(id, false);

    private static Task SetWhetherDeleted(int id, bool deleted)
    {
        var item = EnforcementOrderData.EnforcementOrders.SingleOrDefault(e => e.Id == id)
            ?? throw new ArgumentException($"ID ({id}) not found.", nameof(id));

        item.Deleted = deleted;
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
