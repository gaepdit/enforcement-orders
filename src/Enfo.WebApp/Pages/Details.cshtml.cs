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
    public class Details : PageModel
    {
        public EnforcementOrderDetailedView Item { get; private set; }
        public DisplayMessage Message { get; private set; }

        [UsedImplicitly]
        public async Task<IActionResult> OnGetAsync([FromServices] IEnforcementOrderRepository repository, int? id)
        {
            if (id == null) return NotFound();
            Item = await repository.GetAsync(id.Value);
            if (Item == null) return NotFound("ID not found.");
            Message = TempData?.GetDisplayMessage();
            return Page();
        }
    }
}