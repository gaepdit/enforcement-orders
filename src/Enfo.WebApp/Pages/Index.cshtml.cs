using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.EnforcementOrder;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
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