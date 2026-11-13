using Enfo.Domain.EpdContacts;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.Domain.Identity;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.PageModelHelpers;

namespace Enfo.WebApp.Pages.Admin.Maintenance.Contacts;

[Authorize(Roles = AppRole.SiteMaintenance)]
public class EditModel(IEpdContactRepository repository) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public EpdContactCommand Item { get; set; } = null!;

    [TempData]
    public int HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.EpdContact;

    public async Task<IActionResult> OnGetAsync(CancellationToken token = default)
    {
        if (Id == 0) return NotFound();
        var originalItem = await repository.GetAsync(Id, token);
        if (originalItem == null) return NotFound("ID not found.");

        Item = new EpdContactCommand(originalItem);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token = default)
    {
        if (Id == 0) return BadRequest();
        if (!ModelState.IsValid) return Page();

        await repository.UpdateAsync(Id, Item, token);

        HighlightId = Id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success,
            $"{ThisOption.SingularName} successfully updated.");
        return RedirectToPage("Index");
    }
}
