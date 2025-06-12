using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages;

public class RecentExecuted : PageModel
{
    public IReadOnlyList<EnforcementOrderDetailedView> Orders { get; private set; }

    public async Task OnGetAsync([FromServices] IEnforcementOrderRepository repository) =>
        Orders = await repository.ListRecentlyExecutedEnforcementOrdersAsync();
}
