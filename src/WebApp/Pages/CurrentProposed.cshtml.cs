using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;

namespace Enfo.WebApp.Pages;

public class CurrentProposed : PageModel
{
    public IReadOnlyList<EnforcementOrderDetailedView> Orders { get; private set; }

    public async Task OnGetAsync([FromServices] IEnforcementOrderRepository repository) =>
        Orders = await repository.ListCurrentProposedEnforcementOrdersAsync();
}
