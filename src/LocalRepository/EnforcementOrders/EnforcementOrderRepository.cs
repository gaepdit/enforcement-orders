using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.Pagination;

namespace Enfo.LocalRepository.EnforcementOrders;

public sealed class EnforcementOrderRepository : IEnforcementOrderRepository
{
    public Task<EnforcementOrderDetailedView> GetAsync(int id) => throw new NotImplementedException();

    public Task<EnforcementOrderAdminView> GetAdminViewAsync(int id) => throw new NotImplementedException();

    public Task<PaginatedResult<EnforcementOrderSummaryView>> ListAsync(EnforcementOrderSpec spec, PaginationSpec paging) => throw new NotImplementedException();

    public Task<PaginatedResult<EnforcementOrderDetailedView>> ListDetailedAsync(EnforcementOrderSpec spec, PaginationSpec paging) => throw new NotImplementedException();

    public Task<PaginatedResult<EnforcementOrderAdminSummaryView>> ListAdminAsync(EnforcementOrderAdminSpec spec, PaginationSpec paging) => throw new NotImplementedException();

    public Task<bool> ExistsAsync(int id, bool onlyPublic = true) => throw new NotImplementedException();

    public Task<bool> OrderNumberExistsAsync(string orderNumber, int? ignoreId = null) => throw new NotImplementedException();

    public Task<IReadOnlyList<EnforcementOrderDetailedView>> ListCurrentProposedEnforcementOrdersAsync() => throw new NotImplementedException();

    public Task<IReadOnlyList<EnforcementOrderDetailedView>> ListRecentlyExecutedEnforcementOrdersAsync() => throw new NotImplementedException();

    public Task<IReadOnlyList<EnforcementOrderAdminSummaryView>> ListDraftEnforcementOrdersAsync() => throw new NotImplementedException();

    public Task<IReadOnlyList<EnforcementOrderAdminSummaryView>> ListPendingEnforcementOrdersAsync() => throw new NotImplementedException();

    public Task<int> CreateAsync(EnforcementOrderCreate resource) => throw new NotImplementedException();

    public Task UpdateAsync(EnforcementOrderUpdate resource) => throw new NotImplementedException();

    public Task DeleteAsync(int id) => throw new NotImplementedException();

    public Task RestoreAsync(int id) => throw new NotImplementedException();
    
    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
