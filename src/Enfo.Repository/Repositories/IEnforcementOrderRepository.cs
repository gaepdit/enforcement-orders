using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Repository.Resources;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.Repository.Specs;

namespace Enfo.Repository.Repositories
{
    public interface IEnforcementOrderRepository : IDisposable
    {
        Task<EnforcementOrderDetailedView> GetAsync(int id);
        Task<EnforcementOrderAdminView> GetAdminViewAsync(int id);
        Task<PaginatedResult<EnforcementOrderSummaryView>> ListAsync(EnforcementOrderSpec spec, PaginationSpec paging);
        Task<PaginatedResult<EnforcementOrderAdminSummaryView>> ListAdminAsync(EnforcementOrderAdminSpec spec, PaginationSpec paging);
        Task<bool> ExistsAsync(int id, bool onlyPublic = true);
        Task<bool> OrderNumberExistsAsync(string orderNumber, int? ignoreId = null);
        Task<IReadOnlyList<EnforcementOrderDetailedView>> ListCurrentProposedEnforcementOrdersAsync();
        Task<IReadOnlyList<EnforcementOrderSummaryView>> ListRecentlyExecutedEnforcementOrdersAsync();
        Task<IReadOnlyList<EnforcementOrderAdminSummaryView>> ListDraftEnforcementOrdersAsync();
        Task<IReadOnlyList<EnforcementOrderAdminSummaryView>> ListPendingEnforcementOrdersAsync();
        Task<int> CreateAsync(EnforcementOrderCreate resource);
        Task UpdateAsync(int id, EnforcementOrderUpdate resource);
        Task DeleteAsync(int id);
        Task RestoreAsync(int id);
    }
}