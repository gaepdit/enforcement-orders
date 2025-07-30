using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.Domain.Users;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;
using Microsoft.EntityFrameworkCore;

namespace Enfo.WebApp.Pages.Admin.Maintenance.Contacts;

[Authorize(Roles = AppRole.SiteMaintenance)]
public class Edit(IEpdContactRepository repository) : PageModel
{
    [BindProperty]
    public EpdContactCommand Item { get; set; }

    [TempData]
    public int HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.EpdContact;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();
        var originalItem = await repository.GetAsync(id.Value);
        if (originalItem == null) return NotFound("ID not found.");

        if (!originalItem.Active)
        {
            TempData.SetDisplayMessage(Context.Warning, $"Inactive {ThisOption.PluralName} cannot be edited.");
            return RedirectToPage("Index");
        }

        Item = new EpdContactCommand(originalItem);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Item.Id is null) return BadRequest();

        var originalItem = await repository.GetAsync(Item.Id.Value);
        if (originalItem == null) return NotFound();

        if (!originalItem.Active)
        {
            TempData.SetDisplayMessage(Context.Warning, $"Inactive {ThisOption.PluralName} cannot be edited.");
            return RedirectToPage("Index");
        }

        if (!ModelState.IsValid) return Page();

        try
        {
            await repository.UpdateAsync(Item);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await repository.ExistsAsync(Item.Id.Value)) return NotFound();
            throw;
        }

        HighlightId = Item.Id.Value;
        TempData.SetDisplayMessage(Context.Success, $"{ThisOption.SingularName} successfully updated.");
        return RedirectToPage("Index");
    }
}
