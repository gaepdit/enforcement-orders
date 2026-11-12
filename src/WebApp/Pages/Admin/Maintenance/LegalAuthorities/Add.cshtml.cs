using Enfo.Domain.Identity;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.PageModelHelpers;
using FluentValidation;

namespace Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities;

[Authorize(Roles = AppRole.SiteMaintenance)]
public class Add : PageModel
{
    [BindProperty]
    public LegalAuthorityCommand Item { get; init; }

    public static MaintenanceOption ThisOption => MaintenanceOption.LegalAuthority;

    [TempData]
    public int HighlightId { get; set; }

    public static void OnGet()
    {
        // Method intentionally left empty.
    }

    public async Task<IActionResult> OnPostAsync(
        [FromServices] ILegalAuthorityRepository repository,
        [FromServices] IValidator<LegalAuthorityCommand> validator)
    {
        await validator.ApplyValidationAsync(Item, ModelState);
        if (!ModelState.IsValid) return Page();

        var id = await repository.CreateAsync(Item);

        HighlightId = id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"{Item.AuthorityName} successfully added.");
        return RedirectToPage("Index");
    }
}
