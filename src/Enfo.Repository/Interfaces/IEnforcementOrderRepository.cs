using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Repository.Pagination;
using Enfo.Repository.Querying;
using Enfo.Repository.Resources.EnforcementOrder;

namespace Enfo.Repository.Interfaces
{
    public interface IEnforcementOrderRepository

    {
        Task<EnforcementOrderView> GetAsync(int id, bool onlyIfPublic);
        Task<IPaginatedResult<EnforcementOrderSummaryView>> ListAsync(
            EnforcementOrderSpec spec,
            IPagination pagination = null);
        Task<int> CountAsync(EnforcementOrderSpec spec);
        Task<bool> ExistsAsync(int id);
        Task<bool> OrderNumberExists(string orderNumber, int ignoreId = -1);
        Task<IReadOnlyList<EnforcementOrderSummaryView>> ListCurrentProposedEnforcementOrders();
        Task<IReadOnlyList<EnforcementOrderSummaryView>> ListDraftEnforcementOrders();
        Task<IReadOnlyList<EnforcementOrderSummaryView>> ListPendingEnforcementOrders();
        Task<IReadOnlyList<EnforcementOrderSummaryView>> ListRecentlyExecutedEnforcementOrders();
        Task<int> CreateAsync(EnforcementOrderCreate resource);
        Task UpdateAsync(int id, EnforcementOrderUpdate resource);
    }
}