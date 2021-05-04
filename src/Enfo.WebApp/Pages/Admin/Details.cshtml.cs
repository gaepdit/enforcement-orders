using System.Threading.Tasks;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.EnforcementOrder;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin
{
    [Authorize]
    public class Details : PageModel
    {
        public EnforcementOrderAdminView Item { get; private set; }
        public DisplayMessage Message { get; private set; }

        [UsedImplicitly]
        public async Task<IActionResult> OnGetAsync([FromServices] IEnforcementOrderRepository repository, int? id)
        {
            if (id == null) return NotFound();
            Item = await repository.GetAdminViewAsync(id.Value);
            if (Item == null) return NotFound("ID not found.");
            Message = TempData?.GetDisplayMessage();
            return Page();
        }
    }
}