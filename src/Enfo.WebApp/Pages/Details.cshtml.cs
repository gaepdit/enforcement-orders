using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages;

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
