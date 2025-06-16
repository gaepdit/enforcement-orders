using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.Domain.Users;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin.Maintenance.Contacts;

[Authorize(Roles = AppRole.SiteMaintenance)]
public class Add : PageModel
{
    [BindProperty]
    public EpdContactCommand Item { get; init; }

    public static MaintenanceOption ThisOption => MaintenanceOption.EpdContact;

    [TempData]
    public int HighlightId { get; set; }

    public static void OnGet()
    {
        // Method intentionally left empty.
    }

    public async Task<IActionResult> OnPostAsync([FromServices] IEpdContactRepository repository)
    {
        Item.TrimAll();
        if (!ModelState.IsValid) return Page();
        HighlightId = await repository.CreateAsync(Item);
        TempData.SetDisplayMessage(Context.Success, $"{ThisOption.SingularName} successfully added.");
        return RedirectToPage("Index");
    }
}
