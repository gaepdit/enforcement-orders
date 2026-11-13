using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;

namespace Enfo.WebApp.Pages;

public class IndexModel : PageModel
{
    public IReadOnlyList<EnforcementOrderSummaryView> CurrentProposedOrders { get; private set; } = null!;
    public IReadOnlyList<EnforcementOrderSummaryView> RecentExecutedOrders { get; private set; } = null!;

    public async Task OnGetAsync([FromServices] IEnforcementOrderRepository repository)
    {
        CurrentProposedOrders = await repository.ListCurrentProposedEnforcementOrdersAsync();
        RecentExecutedOrders = await repository.ListRecentlyExecutedEnforcementOrdersAsync();
    }
}
