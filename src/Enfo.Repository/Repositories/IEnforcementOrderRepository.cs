using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Repository.Resources;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.Repository.Specs;
using Enfo.Repository.Validation;

namespace Enfo.Repository.Repositories
{
    public interface IEnforcementOrderRepository : IDisposable
    {
        Task<EnforcementOrderDetailedView> GetAsync(int id, bool onlyPublic = true);
        Task<EnforcementOrderAdminView> GetAdminViewAsync(int id);

        Task<PaginatedResult<EnforcementOrderSummaryView>> ListAsync(
            EnforcementOrderSpec spec, PaginationSpec paging);

        Task<int> CountAsync(EnforcementOrderSpec spec);
        Task<bool> ExistsAsync(int id, bool onlyPublic = true);
        Task<bool> OrderNumberExistsAsync(string orderNumber, int? ignoreId = null);
        Task<IReadOnlyList<EnforcementOrderSummaryView>> ListCurrentProposedEnforcementOrdersAsync();
        Task<IReadOnlyList<EnforcementOrderSummaryView>> ListDraftEnforcementOrdersAsync();
        Task<IReadOnlyList<EnforcementOrderSummaryView>> ListPendingEnforcementOrdersAsync();
        Task<IReadOnlyList<EnforcementOrderSummaryView>> ListRecentlyExecutedEnforcementOrdersAsync();
        Task<int> CreateAsync(EnforcementOrderCreate resource);
        Task UpdateAsync(int id, EnforcementOrderUpdate resource);
        Task DeleteAsync(int id);
        Task RestoreAsync(int id);
    }
}