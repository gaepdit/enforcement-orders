using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.EnforcementOrder;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
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
            CurrentProposedOrders = await _repository.ListCurrentProposedEnforcementOrdersAsync();
            RecentExecutedOrders = await _repository.ListRecentlyExecutedEnforcementOrdersAsync();
            Message = TempData?.GetDisplayMessage();
        }
    }
}