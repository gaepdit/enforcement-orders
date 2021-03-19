using System.Threading.Tasks;
using Enfo.Repository;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.WebApp.Extensions;
using Enfo.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.EnforcementOrders
{
    // TODO: Auth
    // [Authorize(Roles = UserRoles.OrderAdministrator)]
    public class Admin : PageModel
    {
        public EnforcementOrderAdminView Item { get; private set; }
        public DisplayMessage Message { get; private set; }

        private readonly IEnforcementOrderRepository _repository;
        public Admin(IEnforcementOrderRepository repository) => _repository = repository;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound("Order ID not found.");

            Item = await _repository.GetAdminViewAsync(id.Value);

            if (Item == null) return NotFound("Order ID not found.");

            Message = TempData?.GetDisplayMessage();
            return Page();
        }
    }
}