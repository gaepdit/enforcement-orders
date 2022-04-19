using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages
{
    public class Index : PageModel
    {
        public IReadOnlyList<EnforcementOrderSummaryView> CurrentProposedOrders { get; private set; }
        public IReadOnlyList<EnforcementOrderSummaryView> RecentExecutedOrders { get; private set; }
        public DisplayMessage Message { get; private set; }

        [UsedImplicitly]
        public async Task OnGetAsync([FromServices] IEnforcementOrderRepository repository)
        {
            CurrentProposedOrders = await repository.ListCurrentProposedEnforcementOrdersAsync();
            RecentExecutedOrders = await repository.ListRecentlyExecutedEnforcementOrdersAsync();
            Message = TempData?.GetDisplayMessage();
        }
    }
}