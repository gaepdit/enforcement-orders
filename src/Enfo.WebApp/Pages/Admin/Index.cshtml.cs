﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.WebApp.Extensions;
using Enfo.WebApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin
{
    public class Index : PageModel
    {
        public IReadOnlyList<EnforcementOrderSummaryView> CurrentProposedOrders { get; private set; }
        public IReadOnlyList<EnforcementOrderSummaryView> RecentExecutedOrders { get; private set; }
        public IReadOnlyList<EnforcementOrderSummaryView> PendingOrders { get; private set; }
        public IReadOnlyList<EnforcementOrderSummaryView> DraftOrders { get; private set; }
        public DisplayMessage Message { get; private set; }

        private readonly IEnforcementOrderRepository _repository;
        public Index(IEnforcementOrderRepository repository) => _repository = repository;

        public async Task OnGet()
        {
            CurrentProposedOrders = await _repository.ListCurrentProposedEnforcementOrdersAsync();
            RecentExecutedOrders = await _repository.ListRecentlyExecutedEnforcementOrdersAsync();
            PendingOrders = await _repository.ListPendingEnforcementOrdersAsync();
            DraftOrders = await _repository.ListDraftEnforcementOrdersAsync();
            Message = TempData?.GetDisplayMessage();
        }
    }
}