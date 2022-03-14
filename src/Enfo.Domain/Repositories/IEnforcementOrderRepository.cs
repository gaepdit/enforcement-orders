using Enfo.Domain.Resources;
using Enfo.Domain.Resources.EnforcementOrder;
using Enfo.Domain.Specs;

namespace Enfo.Domain.Repositories;

public interface IEnforcementOrderRepository : IDisposable
{
    Task<EnforcementOrderDetailedView> GetAsync(int id);
    Task<EnforcementOrderAdminView> GetAdminViewAsync(int id);
    Task<PaginatedResult<EnforcementOrderSummaryView>> ListAsync(EnforcementOrderSpec spec, PaginationSpec paging);
    Task<PaginatedResult<EnforcementOrderDetailedView>> ListDetailedAsync(EnforcementOrderSpec spec, PaginationSpec paging);
    Task<PaginatedResult<EnforcementOrderAdminSummaryView>> ListAdminAsync(EnforcementOrderAdminSpec spec, PaginationSpec paging);
    Task<bool> ExistsAsync(int id, bool onlyPublic = true);
    Task<bool> OrderNumberExistsAsync(string orderNumber, int? ignoreId = null);
    Task<IReadOnlyList<EnforcementOrderDetailedView>> ListCurrentProposedEnforcementOrdersAsync();
    Task<IReadOnlyList<EnforcementOrderDetailedView>> ListRecentlyExecutedEnforcementOrdersAsync();
    Task<IReadOnlyList<EnforcementOrderAdminSummaryView>> ListDraftEnforcementOrdersAsync();
    Task<IReadOnlyList<EnforcementOrderAdminSummaryView>> ListPendingEnforcementOrdersAsync();
    Task<int> CreateAsync(EnforcementOrderCreate resource);
    Task UpdateAsync(EnforcementOrderUpdate resource);
    Task DeleteAsync(int id);
    Task RestoreAsync(int id);
}
