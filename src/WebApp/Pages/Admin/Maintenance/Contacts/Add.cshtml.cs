using Enfo.Domain.EpdContacts;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.Domain.Identity;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.PageModelHelpers;

namespace Enfo.WebApp.Pages.Admin.Maintenance.Contacts;

[Authorize(Roles = AppRole.SiteMaintenance)]
public class AddModel : PageModel
{
    [BindProperty]
    public EpdContactCommand Item { get; init; } = null!;

    public static MaintenanceOption ThisOption => MaintenanceOption.EpdContact;

    [TempData]
    public int HighlightId { get; set; }

    public async Task<IActionResult> OnPostAsync([FromServices] IEpdContactRepository repository,
        CancellationToken token)
    {
        Item.TrimAll();
        if (!ModelState.IsValid) return Page();

        HighlightId = await repository.CreateAsync(Item, token);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success,
            $"{ThisOption.SingularName} successfully added.");
        return RedirectToPage("Index");
    }
}
