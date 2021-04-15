using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.WebApp.Extensions;
using Enfo.WebApp.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages
{
    public class Index : PageModel
    {
        public IReadOnlyList<EnforcementOrderSummaryView> CurrentProposedOrders { get; private set; }
        public IReadOnlyList<EnforcementOrderSummaryView> RecentExecutedOrders { get; private set; }
        public DisplayMessage Message { get; private set; }

        private readonly IEnforcementOrderRepository _repository;
        public Index(IEnforcementOrderRepository repository) => _repository = repository;

        [UsedImplicitly]
        public async Task OnGetAsync()
        {
            // TODO: Remove after authentication is implemented
            ViewData["PageIsPublic"] = true;

            CurrentProposedOrders = await _repository.ListCurrentProposedEnforcementOrdersAsync();
            RecentExecutedOrders = await _repository.ListRecentlyExecutedEnforcementOrdersAsync();
            Message = TempData?.GetDisplayMessage();
        }
    }
}