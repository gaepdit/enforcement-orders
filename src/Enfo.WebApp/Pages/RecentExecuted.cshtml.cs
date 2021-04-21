﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.EnforcementOrder;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages
{
    public class RecentExecuted : PageModel
    {
        public IReadOnlyList<EnforcementOrderDetailedView> Orders { get; private set; }

        private readonly IEnforcementOrderRepository _repository;
        public RecentExecuted(IEnforcementOrderRepository repository) => _repository = repository;

        [UsedImplicitly]
        public async Task OnGetAsync()
        {
            // TODO: Remove after authentication is implemented
            ViewData["PageIsPublic"] = true;

            Orders = await _repository.ListRecentlyExecutedEnforcementOrdersAsync();
        }
    }
}