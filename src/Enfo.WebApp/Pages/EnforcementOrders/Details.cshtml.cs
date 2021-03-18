using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.WebApp.Extensions;
using Enfo.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.EnforcementOrders
{
    public class Details : PageModel
    {
        public EnforcementOrderDetailedView Item { get; private set; }
        public DisplayMessage Message { get; private set; }

        private readonly IEnforcementOrderRepository _repository;
        public Details(IEnforcementOrderRepository repository) => _repository = repository;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!id.HasValue) return NotFound();

            Item = await _repository.GetAsync(id.Value);

            if (Item == null) return NotFound();

            Message = TempData?.GetDisplayMessage();
            return Page();
        }
    }
}