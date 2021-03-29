using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.EnforcementOrder;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages
{
    public class RecentExecuted : PageModel
    {
        public IReadOnlyList<EnforcementOrderDetailedView> Orders { get; private set; }

        private readonly IEnforcementOrderRepository _repository;
        public RecentExecuted(IEnforcementOrderRepository repository) => _repository = repository;

        public async Task OnGet() => Orders = await _repository.ListRecentlyExecutedEnforcementOrdersAsync();
    }
}