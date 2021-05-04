using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.EnforcementOrder;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages
{
    public class RecentExecuted : PageModel
    {
        public IReadOnlyList<EnforcementOrderDetailedView> Orders { get; private set; }

        [UsedImplicitly]
        public async Task OnGetAsync([FromServices] IEnforcementOrderRepository repository) =>
            Orders = await repository.ListRecentlyExecutedEnforcementOrdersAsync();
    }
}