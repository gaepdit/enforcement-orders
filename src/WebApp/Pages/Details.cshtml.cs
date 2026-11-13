using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;

namespace Enfo.WebApp.Pages;

public class DetailsModel : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public EnforcementOrderDetailedView Item { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync([FromServices] IEnforcementOrderRepository repository)
    {
        if (Id == 0) return NotFound();
        Item = await repository.GetAsync(Id);
        if (Item == null) return NotFound("ID not found.");
        return Page();
    }
}
