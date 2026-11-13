using Enfo.Domain.Identity;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.PageModelHelpers;
using FluentValidation;

namespace Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities;

[Authorize(Roles = AppRole.SiteMaintenance)]
public class AddModel : PageModel
{
    [BindProperty]
    public LegalAuthorityCommand Item { get; init; } = null!;

    public static MaintenanceOption ThisOption => MaintenanceOption.LegalAuthority;

    [TempData]
    public int HighlightId { get; set; }

    public async Task<IActionResult> OnPostAsync(
        [FromServices] ILegalAuthorityRepository repository,
        [FromServices] IValidator<LegalAuthorityCommand> validator)
    {
        Item.TrimAll();
        await validator.ApplyValidationAsync(Item, ModelState);
        if (!ModelState.IsValid) return Page();

        HighlightId = await repository.CreateAsync(Item);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"{Item.AuthorityName} successfully added.");
        return RedirectToPage("Index");
    }
}
