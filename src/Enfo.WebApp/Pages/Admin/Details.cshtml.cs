using System.Threading.Tasks;
using Enfo.Repository;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin
{
    // TODO: Auth
    // [Authorize(Roles = UserRole.OrderAdministrator)]
    public class Details : PageModel
    {
        public EnforcementOrderAdminView Item { get; private set; }
        public DisplayMessage Message { get; private set; }

        private readonly IEnforcementOrderRepository _repository;
        public Details(IEnforcementOrderRepository repository) => _repository = repository;

        [UsedImplicitly]
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            Item = await _repository.GetAdminViewAsync(id.Value);
            if (Item == null) return NotFound("ID not found.");
            Message = TempData?.GetDisplayMessage();
            return Page();
        }
    }
}